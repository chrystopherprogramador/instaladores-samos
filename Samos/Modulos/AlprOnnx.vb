Imports OpenCvSharp
Imports OpenCvSharp.Dnn
Imports System.IO
Imports System.Linq

' ===========================================================================
' AlprOnnx — Reconhecimento de placas (ALPR) offline usando modelos ONNX.
'
' Estágio 1 (detecção): localiza a região da placa na foto com um modelo
'                       ONNX estilo YOLO (plate_detector.onnx).
' Estágio 2 (recorte):  devolve o recorte preciso e endireitado da placa.
'
' Degradação elegante: se o modelo não existir, DetectarPlaca devolve Nothing
' e o chamador (OCRPlaca) usa o fluxo Tesseract na imagem inteira.
' Quando o modelo existe, o recorte preciso melhora drasticamente o OCR.
' ===========================================================================
Public Module AlprOnnx

    ' Resolução de entrada esperada pelo detector (YOLOv5/v8 padrão = 640).
    Private Const TAMANHO_ENTRADA As Integer = 640
    Private Const LIMIAR_CONFIANCA As Single = 0.25F
    Private Const LIMIAR_NMS As Single = 0.45F

    Private ReadOnly Property ModelsPath As String
        Get
            Return Path.Combine(AppContext.BaseDirectory, "models")
        End Get
    End Property

    Private ReadOnly Property CaminhoDetector As String
        Get
            Return Path.Combine(ModelsPath, "plate_detector.onnx")
        End Get
    End Property

    ' Rede carregada uma única vez (lazy).
    Private _detector As Net
    Private _detectorTentado As Boolean = False

    ''' <summary>
    ''' Indica se o detector ONNX de placa está disponível e carregado.
    ''' </summary>
    Public ReadOnly Property DetectorDisponivel As Boolean
        Get
            Return ObterDetector() IsNot Nothing
        End Get
    End Property

    ' Carrega o detector ONNX sob demanda; retorna Nothing se o arquivo não existir
    ' ou se o carregamento falhar (mantém o fallback Tesseract funcionando).
    Private Function ObterDetector() As Net
        If _detectorTentado Then Return _detector

        _detectorTentado = True
        Try
            If File.Exists(CaminhoDetector) Then
                _detector = CvDnn.ReadNetFromOnnx(CaminhoDetector)
                _detector.SetPreferableBackend(Backend.OPENCV)
                _detector.SetPreferableTarget(Target.CPU)
            End If
        Catch
            _detector = Nothing
        End Try

        Return _detector
    End Function

    ''' <summary>
    ''' Detecta a placa na imagem e devolve o recorte (com margem) ou Nothing.
    ''' O chamador é responsável por liberar o Mat retornado.
    ''' </summary>
    Public Function DetectarPlaca(src As Mat) As Mat
        Dim net = ObterDetector()
        If net Is Nothing OrElse src Is Nothing OrElse src.Empty() Then
            Return Nothing
        End If

        Try
            Dim caixa = DetectarMelhorCaixa(net, src)
            If caixa Is Nothing Then Return Nothing

            Dim r As Rect = caixa.Value
            ' Margem de segurança ao redor da placa.
            Dim mx As Integer = CInt(r.Width * 0.06)
            Dim my As Integer = CInt(r.Height * 0.18)
            Dim x = Math.Max(0, r.X - mx)
            Dim y = Math.Max(0, r.Y - my)
            Dim w = Math.Min(src.Width - x, r.Width + mx * 2)
            Dim h = Math.Min(src.Height - y, r.Height + my * 2)
            If w <= 0 OrElse h <= 0 Then Return Nothing

            Dim roi As New Rect(x, y, w, h)
            Return New Mat(src, roi)
        Catch
            Return Nothing
        End Try
    End Function

    ' Roda o detector e devolve a melhor caixa (maior confiança após NMS).
    Private Function DetectarMelhorCaixa(net As Net, src As Mat) As Rect?
        Dim blob = CvDnn.BlobFromImage(src, 1.0 / 255.0,
                                       New Size(TAMANHO_ENTRADA, TAMANHO_ENTRADA),
                                       New Scalar(0, 0, 0), True, False)
        net.SetInput(blob)

        Dim saida As Mat = net.Forward()
        Try
            ' Normaliza o formato de saída para uma matriz 2D [N x dim].
            Dim det = ReformatarSaida(saida)
            If det Is Nothing Then Return Nothing

            Dim linhas = det.Rows
            Dim colunas = det.Cols

            Dim caixas As New List(Of Rect)
            Dim confs As New List(Of Single)

            Dim escalaX As Single = src.Width / CSng(TAMANHO_ENTRADA)
            Dim escalaY As Single = src.Height / CSng(TAMANHO_ENTRADA)

            For i = 0 To linhas - 1
                ' Layout YOLO: [cx, cy, w, h, obj, classe...]
                Dim obj As Single = det.At(Of Single)(i, 4)
                If obj < LIMIAR_CONFIANCA Then Continue For

                ' Confiança final = obj * melhor score de classe (se houver classes).
                Dim conf As Single = obj
                If colunas > 5 Then
                    Dim melhorClasse As Single = 0
                    For c = 5 To colunas - 1
                        Dim sc As Single = det.At(Of Single)(i, c)
                        If sc > melhorClasse Then melhorClasse = sc
                    Next
                    conf = obj * melhorClasse
                End If
                If conf < LIMIAR_CONFIANCA Then Continue For

                Dim cx As Single = det.At(Of Single)(i, 0) * escalaX
                Dim cy As Single = det.At(Of Single)(i, 1) * escalaY
                Dim ww As Single = det.At(Of Single)(i, 2) * escalaX
                Dim hh As Single = det.At(Of Single)(i, 3) * escalaY

                Dim left = CInt(cx - ww / 2)
                Dim top = CInt(cy - hh / 2)
                caixas.Add(New Rect(left, top, CInt(ww), CInt(hh)))
                confs.Add(conf)
            Next

            If caixas.Count = 0 Then Return Nothing

            ' Non-Maximum Suppression para remover caixas sobrepostas.
            Dim indices() As Integer = Nothing
            CvDnn.NMSBoxes(caixas, confs, LIMIAR_CONFIANCA, LIMIAR_NMS, indices)
            If indices Is Nothing OrElse indices.Length = 0 Then Return Nothing

            ' Escolhe a caixa de maior confiança entre as mantidas.
            Dim melhorIdx As Integer = indices(0)
            Dim melhorConf As Single = confs(melhorIdx)
            For Each idx In indices
                If confs(idx) > melhorConf Then
                    melhorConf = confs(idx)
                    melhorIdx = idx
                End If
            Next

            Dim melhor = caixas(melhorIdx)
            ' Garante que a caixa fique dentro da imagem.
            melhor.X = Math.Max(0, melhor.X)
            melhor.Y = Math.Max(0, melhor.Y)
            If melhor.X + melhor.Width > src.Width Then melhor.Width = src.Width - melhor.X
            If melhor.Y + melhor.Height > src.Height Then melhor.Height = src.Height - melhor.Y
            If melhor.Width <= 0 OrElse melhor.Height <= 0 Then Return Nothing

            Return melhor
        Finally
            saida.Dispose()
            blob.Dispose()
        End Try
    End Function

    ' Saídas ONNX podem vir como [1 x N x dim] (3D) ou já [N x dim] (2D).
    ' Esta função devolve sempre uma matriz 2D [N x dim].
    Private Function ReformatarSaida(saida As Mat) As Mat
        If saida Is Nothing OrElse saida.Empty() Then Return Nothing

        If saida.Dims = 2 Then
            Return saida
        End If

        If saida.Dims = 3 Then
            ' Formato [1, N, dim] -> reshape para [N, dim] (canal 1, N linhas).
            Dim n As Integer = saida.Size(1)
            Return saida.Reshape(1, n)
        End If

        Return Nothing
    End Function

End Module
