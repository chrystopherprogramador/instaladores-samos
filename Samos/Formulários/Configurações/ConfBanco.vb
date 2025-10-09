Imports MySql.Data.MySqlClient

Public Class ConfBanco
    Private Sub ConfBanco_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim config As New clsGeForm(Me, True, False, False, Sub()
                                                                btnGravar.PerformClick()
                                                            End Sub)
        config.ConfiguraMe()
    End Sub

    Private Sub btnGravar_Click(sender As Object, e As EventArgs) Handles btnGravar.Click

        Try

            If txtServidor.Text = String.Empty Then
                MsgBox("Informe o IP do Servidor", vbExclamation)
                txtServidor.Focus()
                Exit Sub
            End If
            If txtBanco.Text = String.Empty Then
                MsgBox("Informe o Nome do Banco", vbExclamation)
                txtBanco.Focus()
                Exit Sub
            End If
            If txtusuario.Text = String.Empty Then
                MsgBox("Informe o Usuário do Banco", vbExclamation)
                txtusuario.Focus()
                Exit Sub
            End If
            If txtPorta.Text = String.Empty Then
                MsgBox("Informe a Porta do Banco", vbExclamation)
                txtPorta.Focus()
                Exit Sub
            End If
            If txtSenha.Text = String.Empty Then
                If MsgBox("A senha do banco está vazia, deseja continuar?", vbQuestion + vbYesNo) = vbNo Then
                    txtSenha.Focus()
                    Exit Sub
                End If
            End If

            Dim arquivoIni As String = IO.Path.Combine(Application.StartupPath, "AdmMotel.ini")
            Dim ini As New IniReader(arquivoIni)
            ini.WriteValue("CONFIG", "IP_SERVIDOR", txtServidor.Text)
            ini.WriteValue("CONFIG", "NOME", txtBanco.Text)
            ini.WriteValue("CONFIG", "USUARIO", txtusuario.Text)
            ini.WriteValue("CONFIG", "SENHA", txtSenha.Text)
            ini.WriteValue("CONFIG", "PORTA", txtPorta.Text)

            MsgBox("Configurações gravadas com sucesso!", vbInformation)
            Me.Close()

        Catch ex As Exception
            MsgBox("Erro ao gravar as configurações: " & ex.Message, vbCritical)
        End Try

    End Sub

    Private Sub btnTestar_Click(sender As Object, e As EventArgs) Handles btnTestar.Click
        Try
            If txtServidor.Text = String.Empty Then
                MsgBox("Informe o IP do Servidor", vbExclamation)
                txtServidor.Focus()
                Exit Sub
            End If
            If txtBanco.Text = String.Empty Then
                MsgBox("Informe o Nome do Banco", vbExclamation)
                txtBanco.Focus()
                Exit Sub
            End If
            If txtusuario.Text = String.Empty Then
                MsgBox("Informe o Usuário do Banco", vbExclamation)
                txtusuario.Focus()
                Exit Sub
            End If
            If txtPorta.Text = String.Empty Then
                MsgBox("Informe a Porta do Banco", vbExclamation)
                txtPorta.Focus()
                Exit Sub
            End If
            If txtSenha.Text = String.Empty Then
                If MsgBox("A senha do banco está vazia, deseja continuar?", vbQuestion + vbYesNo) = vbNo Then
                    txtSenha.Focus()
                    Exit Sub
                End If
            End If
            Dim strConexao As String = $"Server={txtServidor.Text};Port={txtPorta.Text};Database={txtBanco.Text};Uid={txtusuario.Text};Pwd={txtSenha.Text};"
            If TestaConexao(strConexao) Then
                MsgBox("Conexão bem sucedida!", vbInformation)
            Else
                MsgBox("Falha na conexão. Verifique os dados e tente novamente.", vbCritical)
            End If
        Catch ex As Exception
            MsgBox("Erro ao testar a conexão: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Function TestaConexao(connectionString As String) As Boolean
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Return True
            End Using
        Catch
            Return False
        End Try
    End Function
End Class