Imports OpenCvSharp
Imports Tesseract
Imports System.Text.RegularExpressions
Imports System.Linq
Imports System.IO

Public Module OCRPlaca

    ' Detecta e recorta a placa da imagem original com heurística avançada
    Public Function DetectarPlaca(caminhoImagem As String) As String
        Dim src = Cv2.ImRead(caminhoImagem)
        If src.Empty() Then Return Nothing

        Dim gray = New Mat()
        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY)

        ' Suaviza ruído
        Dim blurred = New Mat()
        Cv2.GaussianBlur(gray, blurred, New Size(5, 5), 0)

        ' Binarização adaptativa
        Dim thresh = New Mat()
        Cv2.AdaptiveThreshold(blurred, thresh, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 11, 2)

        ' Morfologia para reforçar contornos
        Dim kernel = Cv2.GetStructuringElement(MorphShapes.Rect, New Size(5, 5))
        Dim morphed = New Mat()
        Cv2.MorphologyEx(thresh, morphed, MorphTypes.Close, kernel)

        ' Detecta contornos
        Dim contours()() As Point
        Dim hierarchy() As HierarchyIndex
        Cv2.FindContours(morphed, contours, hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple)

        ' Ordena por área (maior primeiro)
        Dim contornosOrdenados = contours.OrderByDescending(Function(c) Cv2.ContourArea(c))

        Dim candidatos As New List(Of OpenCvSharp.Rect)

        For Each contour In contornosOrdenados
            Dim rect As OpenCvSharp.Rect = Cv2.BoundingRect(contour)
            Dim proporcao = rect.Width / rect.Height
            Dim area = rect.Width * rect.Height

            ' Heurística avançada: proporção, área, posição
            If proporcao > 2 And proporcao < 6 And rect.Width > 100 And rect.Height > 30 And area > 3000 And area < 30000 Then
                candidatos.Add(rect)
            End If

            Dim debug = $"Contorno: W={rect.Width}, H={rect.Height}, Proporção={Math.Round(proporcao, 2)}, Área={area}, Y={rect.Y}"
            File.AppendAllText("debug_contornos.txt", debug & vbCrLf)
        Next

        If candidatos.Count > 0 Then
            ' Escolhe o retângulo com proporção mais próxima de 4.5
            Dim melhor As OpenCvSharp.Rect = candidatos.OrderBy(Function(r) Math.Abs((r.Width / r.Height) - 4.5)).First()
            Dim placa = New Mat(src, melhor)

            ' Salva imagem recortada com timestamp
            Dim nomeArquivo = $"placa_{Now:yyyyMMdd_HHmmss}.jpg"
            Dim caminhoPlaca = Path.Combine(AppContext.BaseDirectory, nomeArquivo)
            Cv2.ImWrite(caminhoPlaca, placa)
            Return caminhoPlaca
        End If

        Dim debugImg = src.Clone()
        For Each c In candidatos
            Cv2.Rectangle(debugImg, c, Scalar.Red, 2)
        Next
        Cv2.ImWrite(Path.Combine(AppContext.BaseDirectory, "debug_contornos.jpg"), debugImg)

        Return Nothing
    End Function

    ' Reconhece o texto da placa e filtra apenas o padrão AAA0000
    Public Function ReconhecerTexto(caminhoPlaca As String) As String
        If Not File.Exists(caminhoPlaca) Then Return ""

        Dim tessPath As String = AppContext.BaseDirectory
        Using engine As New TesseractEngine(tessPath, "eng", EngineMode.Default)
            engine.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
            Using img = Pix.LoadFromFile(caminhoPlaca)
                Using page = engine.Process(img)
                    Dim textoOCR As String = page.GetText().Trim()

                    ' Remove quebras de linha e espaços
                    Dim textoUnificado As String = Regex.Replace(textoOCR, "[\s\r\n]", "")

                    ' Extrai padrão de placa brasileira
                    Dim padraoPlaca As String = "[A-Z]{3}[0-9]{4}"
                    Dim placaDetectada As String = Regex.Match(textoUnificado, padraoPlaca).Value

                    Return placaDetectada
                End Using
            End Using
        End Using
    End Function

    ' Pipeline completo: detecta, reconhece e registra
    Public Function ProcessarImagem(caminhoImagem As String) As String
        Dim caminhoPlaca = DetectarPlaca(caminhoImagem)

        If caminhoPlaca IsNot Nothing Then
            Dim textoPlaca = ReconhecerTexto(caminhoPlaca)
            If textoPlaca <> "" Then
                Return textoPlaca
            Else
                RegistrarFalha(caminhoImagem, "OCR não reconheceu texto válido.")
            End If
        Else
            RegistrarFalha(caminhoImagem, "Não foi possível detectar a placa.")
        End If

        Return ""
    End Function

    ' Registra falha e salva imagem original
    Private Sub RegistrarFalha(caminhoImagem As String, motivo As String)
        Dim nomeFalha = $"falha_{Now:yyyyMMdd_HHmmss}.jpg"
        Dim destino = Path.Combine(AppContext.BaseDirectory, nomeFalha)
        File.Copy(caminhoImagem, destino, True)

        Dim logPath = Path.Combine(AppContext.BaseDirectory, "falhas_log.txt")
        Dim log = $"[{Now:yyyy-MM-dd HH:mm:ss}] {motivo} → {Path.GetFileName(caminhoImagem)}{vbCrLf}"
        File.AppendAllText(logPath, log)
    End Sub

End Module
