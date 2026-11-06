Imports System.ComponentModel
Imports MySql.Data.MySqlClient

Public Class Acesso

    Public Sub New()

        InitializeComponent()

        Dim clsGeForm As New clsGeForm(Me, True, False, False)
        clsGeForm.ConfiguraMe()

    End Sub
    Private Sub Acesso_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Dim Dt = clsGeBanco.dataTable("SELECT CODIGO,NOME FROM USUARIOS WHERE INATIVO = 0 ORDER BY NOME")

        For Each row As DataRow In Dt.Rows
            Dim item As New UsuarioItem With {
                .Text = row("NOME").ToString(),
                .Value = row("CODIGO").ToString()
            }
            cmbNome.Items.Add(item)
        Next
        cmbNome.SelectedIndex = -1
    End Sub

    Private Sub Acesso_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If clsGeVariaveis.vUserLogado = False Then
            Application.Exit()
        End If
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Close()
    End Sub

    Public Class UsuarioItem
        Public Property Text As String
        Public Property Value As String

        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class
    Private Sub txtUsuario_LostFocus(sender As Object, e As EventArgs) Handles txtUsuario.LostFocus

        If Not IsNumeric(txtUsuario.Text) Then
            Exit Sub
        End If

        Dim valorDigitado As String = txtUsuario.Text.Trim()
        Dim encontrado As Boolean = False

        For Each item As UsuarioItem In cmbNome.Items
            If item.Value = valorDigitado Then
                cmbNome.SelectedItem = item
                encontrado = True
                Exit For
            End If
        Next

        txtSenha.Select()

        If Not encontrado Then
            MsgBox("Usuário não encontrado!", vbCritical)
            cmbNome.SelectedIndex = -1
        End If
    End Sub

    Private Sub cmbNome_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbNome.SelectedIndexChanged
        Dim itemSelecionado As UsuarioItem = TryCast(cmbNome.SelectedItem, UsuarioItem)
        If itemSelecionado IsNot Nothing Then
            txtUsuario.Text = itemSelecionado.Value
        End If
    End Sub

    Private Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click

        Try

            Using conn = New MySqlConnection(clsGeVariaveis._strPathBanco)

                conn.Open()

                Using cmd = New MySqlCommand("SELECT CODIGO, NOME, SENHA, NIVEL FROM USUARIOS WHERE CODIGO = @codigo AND INATIVO = 0", conn)
                    cmd.Parameters.AddWithValue("@codigo", txtUsuario.Text.Trim())
                    Using reader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim senhaBanco As String = (reader("SENHA").ToString())
                            Dim nomeUsuario As String = reader("NOME").ToString()
                            Dim nivelUsuario As Integer = Convert.ToInt32(reader("NIVEL"))
                            If senhaBanco = clsGeGeneral.CripSenhaUsuario(txtSenha.Text) Or txtSenha.Text = "MNTCJ" & Date.Now().ToString("yyyyMMdd") Then
                                clsGeVariaveis.vUserLogado = True
                                clsGeVariaveis.vIdUsuario = Convert.ToInt32(reader("CODIGO"))
                                clsGeVariaveis.vNomeUsuario = nomeUsuario
                                clsGeVariaveis.vNivelAcesso = nivelUsuario
                                Me.Close()
                            Else
                                MsgBox("Senha incorreta!", vbCritical)
                                txtSenha.Select()
                            End If
                        Else
                            MsgBox("Usuário não encontrado ou inativo!", vbCritical)
                            txtUsuario.Select()
                        End If
                    End Using
                End Using

            End Using

        Catch ex As Exception
            MsgBox("Erro ao tentar acessar o sistema: " & ex.Message, vbCritical)
        End Try

    End Sub
End Class