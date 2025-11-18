Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports AForge.Video
Imports AForge.Video.DirectShow
Imports OpenCvSharp
Imports Tesseract
Public Class Ocupação


    'Captura de Imagem antes de enviar para o OCR
    Private videoSource As VideoCaptureDevice
    Private videoDevices As FilterInfoCollection

    Public Property codigoAtual As Long = 0
    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Dim form As New clsGeForm(Me, True, False, False)
        form.ConfiguraMe()

    End Sub

    Private Sub IniciarCamera()
        Try
            videoDevices = New FilterInfoCollection(FilterCategory.VideoInputDevice)

            If videoDevices.Count = 0 Then
                Return
            End If

            ' Pega a primeira câmera disponível
            videoSource = New VideoCaptureDevice(videoDevices(0).MonikerString)

            ' Define o evento para capturar os frames
            AddHandler videoSource.NewFrame, AddressOf CapturarFrame

            videoSource.Start()
        Catch ex As Exception
            MsgBox("Erro ao iniciar câmera: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub CapturarFrame(sender As Object, eventArgs As NewFrameEventArgs)
        Dim imagem As Bitmap = CType(eventArgs.Frame.Clone(), Bitmap)
        PtFotoPlaca.Image = imagem
    End Sub

    Private Sub Ocupação_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblTitulo.Text = $"Ocupação suíte {codigoAtual}"
        IniciarCamera()

        disMovto.Text = Format(clsGeBanco.IncrementarCampo("movtosq", "numero"), "00000000")

    End Sub

    Private Sub Ocupação_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If videoSource IsNot Nothing AndAlso videoSource.IsRunning Then
            videoSource.SignalToStop()
            videoSource.WaitForStop()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim dialog As New OpenFileDialog()
        dialog.Title = "Selecione uma imagem de placa"
        dialog.Filter = "Imagens (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"

        If dialog.ShowDialog() = DialogResult.OK Then
            Dim caminhoImagem As String = dialog.FileName
            Dim placa = ProcessarImagem(caminhoImagem)

            If placa <> "" Then
                MsgBox("Placa detectada: " & placa)
            Else
                MsgBox("Falha ao detectar ou reconhecer a placa.")
            End If
        Else
            MsgBox("Nenhuma imagem foi selecionada.")
        End If
    End Sub

End Class