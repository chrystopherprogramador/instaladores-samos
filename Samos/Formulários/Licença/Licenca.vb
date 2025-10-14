Public Class Licenca

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Dim Form As New clsGeForm(Me, True, False, False,,, Nothing, Nothing, Nothing, Nothing)
        Form.ConfiguraMe()
    End Sub

    Private Sub Licenca_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxTNumeroHD.Text = PegaNumero(DriveSys)
        txtData.Text = Date.Today.ToString("dd/MM/yyyy")
    End Sub

    Private Sub btnDesbloquear_Click(sender As Object, e As EventArgs) Handles btnDesbloquear.Click
        Dim Password As String
        Dim Hd As String
        Dim t As Integer
        Dim MES As Integer
        Dim cPos As Integer
        Static ACESSO As Integer = 0

        Try
            MES = 0

            Hd = PegaNumero(DriveSys).ToString()

            ' Caso o serial do HD seja negativo, transforme em positivo
            t = If(Hd.StartsWith("-"), -1, 1)

            ' Extrai o mês do número serial para cálculo de verificação
            If IsNumeric(Mid(txtPassword.Text, 1, 2)) Then
                MES = CInt(Mid(txtPassword.Text, 1, 2))
            Else
                MES = 13
            End If

            ' Criptografia do serial do disco rígido
            Password = Math.Round(CDec(((CLng(Hd) * 2) / MES) * 2 * t), 0).ToString()

            ' Localiza a posição do caractere "A"
            cPos = InStr(1, txtPassword.Text, "A")

            ' Variável Static permite múltiplas tentativas
            If ACESSO = 0 Then ACESSO = 1

            ' Monta o código esperado
            Dim codigoEsperado As String = Strings.Left(txtPassword.Text, cPos) & Password & DigitoVerificador(Strings.Left(txtPassword.Text, cPos) & Password)

            ' Faz a conferência da autenticação
            If txtPassword.Text.Trim() = codigoEsperado Then
                DesBloqueio(txtPassword.Text, Hd)
                MessageBox.Show("Sistema liberado. Bom trabalho.", "Nosso Programa", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
            Else
                MessageBox.Show("Licença inválida, tente novamente!", "Autenticação de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error)

                If ACESSO < 4 Then
                    txtPassword.Focus()
                ElseIf ACESSO = 4 Then
                    MessageBox.Show("Cópia não autorizada. Contate o revendedor.", "Autenticação de Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Application.Exit()
                End If

                ACESSO += 1
                txtPassword.SelectAll()
            End If

        Catch ex As Exception
            MessageBox.Show("Erro ao validar licença: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class