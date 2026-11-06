Imports OpenCvSharp
Imports Tesseract
Imports System.Text.RegularExpressions
Imports System.Linq
Imports System.IO

Public Module OCRPlaca

    ' Caminho do diretório de dados de idioma do Tesseract
    Private ReadOnly Property TessDataPath As String
        Get
            Return Path.Combine(AppContext.BaseDirectory, "tessdata")
        End Get
    End Property

    ' Representa um palpite de placa com a confiança e quantas vezes apareceu.
    Private Class Candidato
        Public Placa As String
        Public ConfiancaTotal As Single
        Public Votos As Integer
    End Class

    ' ===========================================================================
    ' PIPELINE PÚBLICO: detecta, reconhece e retorna a placa normalizada
    ' Usa votação ponderada por confiança entre todas as variações/regiões.
    ' Retorna "" quando não consegue reconhecer uma placa válida.
    ' ===========================================================================
    Public Function ProcessarImagem(caminhoImagem As String) As String
        If String.IsNullOrWhiteSpace(caminhoImagem) OrElse Not File.Exists(caminhoImagem) Then
            Return ""
        End If

        If Not Directory.Exists(TessDataPath) Then
            RegistrarFalha(caminhoImagem, "Diretório tessdata não encontrado.")
            Return ""
        End If

        Dim src = Cv2.ImRead(caminhoImagem, ImreadModes.Color)
        If src Is Nothing OrElse src.Empty() Then Return ""

        ' Acumula votos por placa candidata.
        Dim urna As New Dictionary(Of String, Candidato)(StringComparer.Ordinal)

        Try
            ' 1) Reúne regiões candidatas, priorizando a detecção mais precisa.
            Dim regioes As New List(Of Mat)

            ' 1a) ALPR ONNX (mais preciso) — recorte exato da placa, se disponível.
            Dim recorteAlpr = AlprOnnx.DetectarPlaca(src)
            If recorteAlpr IsNot Nothing AndAlso Not recorteAlpr.Empty() Then
                regioes.Add(recorteAlpr)
            End If

            ' 1b) Detecção clássica por blackhat/Sobel (boa para placas em foto real).
            regioes.AddRange(DetectarRegioesPlaca(src))

            ' 1c) Fallback: imagem completa (garante que sempre há o que processar).
            regioes.Add(src.Clone())

            ' 2) Uma única engine reutilizada para todas as tentativas (mais rápido).
            Using engine As New TesseractEngine(TessDataPath, "eng", EngineMode.Default)
                engine.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")

                For Each regiao In regioes
                    For Each preProcessada In GerarVariacoes(regiao)
                        For Each leitura In ReconhecerCandidatos(engine, preProcessada)
                            ' leitura.Texto é o bruto; geramos placas válidas possíveis dele.
                            For Each placa In ExtrairPlacasValidas(leitura.Texto)
                                Dim atual As Candidato = Nothing
                                If Not urna.TryGetValue(placa, atual) Then
                                    atual = New Candidato With {.Placa = placa, .ConfiancaTotal = 0, .Votos = 0}
                                    urna(placa) = atual
                                End If
                                atual.Votos += 1
                                atual.ConfiancaTotal += leitura.Confianca
                            Next
                        Next
                        preProcessada.Dispose()
                    Next
                    regiao.Dispose()
                Next
            End Using

            If urna.Count = 0 Then
                RegistrarFalha(caminhoImagem, "OCR não reconheceu uma placa válida.")
                Return ""
            End If

            ' 3) Escolhe o vencedor: pontuação = votos * peso de formato + confiança média.
            Dim vencedor = urna.Values.
                OrderByDescending(Function(c) ScoreFinal(c)).
                First()

            Return vencedor.Placa
        Finally
            src.Dispose()
        End Try
    End Function

    ' Pontuação final combinando frequência, confiança e plausibilidade de formato.
    Private Function ScoreFinal(c As Candidato) As Double
        Dim confMedia As Double = If(c.Votos > 0, c.ConfiancaTotal / c.Votos, 0)
        Return (c.Votos * 100.0) + (confMedia * 50.0) + PesoFormato(c.Placa)
    End Function

    Private Function PesoFormato(placa As String) As Double
        ' Pequeno desempate: Mercosul e antigo são igualmente válidos.
        If Regex.IsMatch(placa, "^[A-Z]{3}[0-9][A-Z][0-9]{2}$") Then Return 2
        If Regex.IsMatch(placa, "^[A-Z]{3}[0-9]{4}$") Then Return 2
        Return 0
    End Function

    ' ===========================================================================
    ' DETECÇÃO: localiza regiões com características de placa usando realce de
    ' texto (blackhat + gradiente de Sobel + fechamento morfológico). Essa
    ' abordagem é muito mais robusta em fotos reais do que Canny puro.
    ' Retorna lista de recortes (pode ser vazia — nesse caso usa-se o fallback).
    ' ===========================================================================
    Private Function DetectarRegioesPlaca(src As Mat) As List(Of Mat)
        Dim recortes As New List(Of Mat)

        Using gray = New Mat()
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY)

            ' Realça regiões escuras sobre fundo claro (caracteres da placa).
            Dim rectKernel = Cv2.GetStructuringElement(MorphShapes.Rect, New Size(13, 5))
            Using blackhat = New Mat()
                Cv2.MorphologyEx(gray, blackhat, MorphTypes.BlackHat, rectKernel)

                ' Gradiente horizontal (bordas verticais dos caracteres).
                Using grad = New Mat()
                    Cv2.Sobel(blackhat, grad, MatType.CV_32F, 1, 0, ksize:=-1)
                    Cv2.ConvertScaleAbs(grad, grad)

                    ' Conecta os caracteres em blocos.
                    Using fechado = New Mat()
                        Cv2.MorphologyEx(grad, fechado, MorphTypes.Close, rectKernel)

                        Using thr = New Mat()
                            Cv2.Threshold(fechado, thr, 0, 255, ThresholdTypes.Binary Or ThresholdTypes.Otsu)

                            Dim sqKernel = Cv2.GetStructuringElement(MorphShapes.Rect, New Size(21, 21))
                            Cv2.MorphologyEx(thr, thr, MorphTypes.Close, sqKernel)

                            Dim contours()() As Point = Nothing
                            Dim hierarchy() As HierarchyIndex = Nothing
                            Cv2.FindContours(thr, contours, hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple)

                            If contours IsNot Nothing Then
                                Dim ordenados = contours.
                                    OrderByDescending(Function(c) Cv2.ContourArea(c)).
                                    Take(20)

                                For Each contour In ordenados
                                    Dim rect As OpenCvSharp.Rect = Cv2.BoundingRect(contour)
                                    If rect.Height = 0 Then Continue For
                                    Dim proporcao As Double = rect.Width / CDbl(rect.Height)

                                    ' Placas têm proporção (largura/altura) tipicamente entre 2 e 6.
                                    If proporcao >= 2.0 AndAlso proporcao <= 6.5 AndAlso rect.Width >= 80 AndAlso rect.Height >= 25 Then
                                        Dim margem = CInt(rect.Height * 0.12)
                                        Dim x = Math.Max(0, rect.X - margem)
                                        Dim y = Math.Max(0, rect.Y - margem)
                                        Dim w = Math.Min(src.Width - x, rect.Width + margem * 2)
                                        Dim h = Math.Min(src.Height - y, rect.Height + margem * 2)
                                        If w > 0 AndAlso h > 0 Then
                                            Dim roi As New OpenCvSharp.Rect(x, y, w, h)
                                            recortes.Add(New Mat(src, roi))
                                        End If
                                    End If
                                Next
                            End If
                        End Using
                    End Using
                End Using
            End Using
        End Using

        Return recortes
    End Function

    ' ===========================================================================
    ' PRÉ-PROCESSAMENTO: gera várias versões da mesma imagem para maximizar a
    ' chance de o OCR ler corretamente em diferentes condições de luz/contraste.
    ' ===========================================================================
    Private Function GerarVariacoes(origem As Mat) As List(Of Mat)
        Dim variacoes As New List(Of Mat)

        ' Trabalha sempre em escala de cinza redimensionada (altura alvo ~ 120px).
        Dim gray = New Mat()
        If origem.Channels() > 1 Then
            Cv2.CvtColor(origem, gray, ColorConversionCodes.BGR2GRAY)
        Else
            gray = origem.Clone()
        End If

        Dim alturaAlvo As Integer = 200
        If gray.Height > 0 AndAlso gray.Height < alturaAlvo Then
            Dim escala As Double = alturaAlvo / CDbl(gray.Height)
            Dim ampliada = New Mat()
            Cv2.Resize(gray, ampliada, New Size(0, 0), escala, escala, InterpolationFlags.Cubic)
            gray.Dispose()
            gray = ampliada
        End If

        ' Melhora contraste local (CLAHE).
        Using clahe = Cv2.CreateCLAHE(2.0, New Size(8, 8))
            Dim contraste = New Mat()
            clahe.Apply(gray, contraste)

            ' Variação 1: Otsu binário.
            Dim otsu = New Mat()
            Cv2.Threshold(contraste, otsu, 0, 255, ThresholdTypes.Binary Or ThresholdTypes.Otsu)
            variacoes.Add(otsu)

            ' Variação 2: Otsu invertido (placas claras sobre fundo escuro).
            Dim otsuInv = New Mat()
            Cv2.Threshold(contraste, otsuInv, 0, 255, ThresholdTypes.BinaryInv Or ThresholdTypes.Otsu)
            variacoes.Add(otsuInv)

            ' Variação 3: threshold adaptativo (iluminação irregular).
            Dim adapt = New Mat()
            Cv2.AdaptiveThreshold(contraste, adapt, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 31, 9)
            variacoes.Add(adapt)

            ' Variação 4: nitidez (unsharp mask) para combater desfoque/ângulo.
            Dim sharpen = New Mat()
            Using suave = New Mat()
                Cv2.GaussianBlur(contraste, suave, New Size(0, 0), 3)
                Cv2.AddWeighted(contraste, 1.5, suave, -0.5, 0, sharpen)
            End Using
            variacoes.Add(sharpen)

            ' Variação 5: apenas o contraste em tons de cinza (Tesseract binariza sozinho).
            variacoes.Add(contraste.Clone())
        End Using

        gray.Dispose()
        Return variacoes
    End Function

    ' ===========================================================================
    ' ===========================================================================
    ' OCR: roda o Tesseract com múltiplos modos de segmentação e devolve
    ' os textos lidos junto com a confiança real reportada pelo motor.
    ' ===========================================================================
    Private Structure Leitura
        Public Texto As String
        Public Confianca As Single
    End Structure

    Private Function ReconhecerCandidatos(engine As TesseractEngine, imagem As Mat) As List(Of Leitura)
        Dim resultados As New List(Of Leitura)

        ' PSM 7 = linha única, PSM 8 = palavra única, PSM 6 = bloco uniforme.
        Dim modos() As PageSegMode = {PageSegMode.SingleLine, PageSegMode.SingleWord, PageSegMode.SingleBlock}

        Try
            Dim bytesImg() As Byte = Nothing
            Cv2.ImEncode(".png", imagem, bytesImg)
            Using pix As Pix = Pix.LoadFromMemory(bytesImg)
                For Each modo As PageSegMode In modos
                    engine.DefaultPageSegMode = modo
                    Using page As Page = engine.Process(pix)
                        Dim texto As String = page.GetText()
                        If Not String.IsNullOrWhiteSpace(texto) Then
                            resultados.Add(New Leitura With {.Texto = texto, .Confianca = page.GetMeanConfidence()})
                        End If
                    End Using
                Next
            End Using
        Catch
            ' Falha de imagem nessa variação é ignorada; outras variações continuam.
        End Try

        Return resultados
    End Function

    ' ===========================================================================
    ' EXTRAÇÃO: a partir de um texto bruto do OCR, gera TODAS as placas válidas
    ' plausíveis (sem forçar uma única interpretação destrutiva). Cada candidata
    ' entra na votação; a correção posicional é só uma alternativa, não a verdade.
    ' Formato antigo:   LLL NNNN     (3 letras + 4 números)
    ' Formato Mercosul: LLL N L NN   (3 letras + num + letra + 2 números)
    ' ===========================================================================
    Private Function ExtrairPlacasValidas(textoOCR As String) As List(Of String)
        Dim placas As New List(Of String)
        If String.IsNullOrEmpty(textoOCR) Then Return placas

        Dim limpo = Regex.Replace(textoOCR.ToUpperInvariant(), "[^A-Z0-9]", "")
        If limpo.Length < 7 Then Return placas

        ' Avalia todas as janelas de 7 caracteres.
        For inicio = 0 To limpo.Length - 7
            Dim janela = limpo.Substring(inicio, 7)

            ' (a) Se a janela JÁ é uma placa válida sem mexer, é a mais confiável.
            If EhPlacaValida(janela) Then
                AdicionarUnico(placas, janela)
            End If

            ' (b) Alternativas com correção posicional leve (cada formato).
            Dim corrAntigo = CorrigirFormato(janela, False)
            If corrAntigo <> "" Then AdicionarUnico(placas, corrAntigo)

            Dim corrMercosul = CorrigirFormato(janela, True)
            If corrMercosul <> "" Then AdicionarUnico(placas, corrMercosul)
        Next

        Return placas
    End Function

    Private Sub AdicionarUnico(lista As List(Of String), valor As String)
        If valor <> "" AndAlso Not lista.Contains(valor) Then lista.Add(valor)
    End Sub

    Private Function EhPlacaValida(p As String) As Boolean
        Return Regex.IsMatch(p, "^[A-Z]{3}[0-9]{4}$") OrElse
               Regex.IsMatch(p, "^[A-Z]{3}[0-9][A-Z][0-9]{2}$")
    End Function

    ' Corrige uma janela para um formato específico, mas SÓ troca caracteres que
    ' são do tipo errado para a posição. Caracteres já válidos são preservados.
    Private Function CorrigirFormato(janela As String, mercosul As Boolean) As String
        Dim ch = janela.ToCharArray()

        ' Posições 0,1,2 = letras (ambos formatos). 3 = número (ambos).
        ch(0) = GarantirLetra(ch(0))
        ch(1) = GarantirLetra(ch(1))
        ch(2) = GarantirLetra(ch(2))
        ch(3) = GarantirNumero(ch(3))

        If mercosul Then
            ch(4) = GarantirLetra(ch(4))   ' posição 4 = letra
            ch(5) = GarantirNumero(ch(5))
            ch(6) = GarantirNumero(ch(6))
        Else
            ch(4) = GarantirNumero(ch(4))
            ch(5) = GarantirNumero(ch(5))
            ch(6) = GarantirNumero(ch(6))
        End If

        Dim resultado = New String(ch)
        If EhPlacaValida(resultado) Then Return resultado
        Return ""
    End Function

    ' Se já for letra, mantém. Só converte dígito ambíguo em letra.
    Private Function GarantirLetra(c As Char) As Char
        If c >= "A"c AndAlso c <= "Z"c Then Return c
        Select Case c
            Case "0"c : Return "O"c
            Case "1"c : Return "I"c
            Case "2"c : Return "Z"c
            Case "4"c : Return "A"c
            Case "5"c : Return "S"c
            Case "6"c : Return "G"c
            Case "7"c : Return "T"c
            Case "8"c : Return "B"c
            Case Else : Return c
        End Select
    End Function

    ' Se já for número, mantém. Só converte letra ambígua em dígito.
    Private Function GarantirNumero(c As Char) As Char
        If c >= "0"c AndAlso c <= "9"c Then Return c
        Select Case c
            Case "O"c, "Q"c, "D"c : Return "0"c
            Case "I"c, "L"c : Return "1"c
            Case "Z"c : Return "2"c
            Case "A"c : Return "4"c
            Case "S"c : Return "5"c
            Case "G"c : Return "6"c
            Case "T"c : Return "7"c
            Case "B"c : Return "8"c
            Case Else : Return c
        End Select
    End Function

    ' ===========================================================================
    ' LOG DE FALHA: salva a imagem original e registra o motivo.
    ' ===========================================================================
    Private Sub RegistrarFalha(caminhoImagem As String, motivo As String)
        Try
            Dim nomeFalha = $"falha_{Now:yyyyMMdd_HHmmss}.jpg"
            Dim destino = Path.Combine(AppContext.BaseDirectory, nomeFalha)
            File.Copy(caminhoImagem, destino, True)

            Dim logPath = Path.Combine(AppContext.BaseDirectory, "falhas_log.txt")
            Dim log = $"[{Now:yyyy-MM-dd HH:mm:ss}] {motivo} -> {Path.GetFileName(caminhoImagem)}{vbCrLf}"
            File.AppendAllText(logPath, log)
        Catch
            ' Logging nunca deve quebrar o fluxo principal.
        End Try
    End Sub

End Module
