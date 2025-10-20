Public Class Usuario

    Public Property CodigoAtual As Long = 0

    Private crud As crUsuario
    Sub New()
        ' Esta chamada é requerida pelo designer.
        InitializeComponent()
        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Dim form As New clsGeForm(Me, True, False, False, Sub()
                                                              btnGravar.PerformClick()
                                                          End Sub)
        form.ConfiguraMe()

    End Sub
    Private Sub Usuario_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If CodigoAtual = 0 Then
            txtCodigo.Text = clsGeBanco.IncrementarCampo("usuarios", "codigo", "codigo < 900").ToString("0000")
        Else
            crud = New crUsuario
            crud.codigo = CodigoAtual
            crud.Executar(TabelaBase.Operacao.SELEC)
            txtCodigo.Text = crud.codigo.ToString("0000")
            txtNome.Text = crud.nome
            txtNivel.Text = crud.nivel.ToString("0000")
            chSup.Checked = crud.supervisor
            chInativo.Checked = crud.inativo

        End If
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click

        Dim frm As New Busca With {
        .Dados = clsGeBanco.dataTable("SELECT DISTINCT Nivel,Descricao FROM niveisacesso ORDER BY Descricao "),
        .ColunasVisuais = New Dictionary(Of String, String) From {
            {"Nivel", "Código"},
            {"Descricao", "Descrição"}
        },
        .CampoRetorno = "Nivel",
        .CamposFiltro = New List(Of String) From {"Descricao"}
        }

        frm.Inicializar()

        If frm.ShowDialog() = DialogResult.OK Then
            Dim codigoSelecionado = frm.Resultado
            txtNivel.Text = codigoSelecionado
            txtNivel.Select()
        End If
    End Sub

    Private Sub txtNivel_TextChanged(sender As Object, e As EventArgs) Handles txtNivel.TextChanged

        Try


            If IsNumeric(txtNivel.Text) Then
                Dim nivel As Long = CLng(txtNivel.Text)
                Dim Dt As DataTable = clsGeBanco.dataTable("SELECT Descricao FROM niveisacesso WHERE Nivel=" & nivel.ToString)
                If Dt.Rows.Count > 0 Then
                    txtDisNivel.Text = Dt.Rows(0).Item("Descricao").ToString
                Else
                    txtDisNivel.Text = String.Empty
                End If
            Else
                txtDisNivel.Text = String.Empty
            End If

        Catch ex As Exception
            MsgBox("Erro ao buscar o nível: " & ex.Message, vbCritical)
        End Try
    End Sub

    Private Sub btnGravar_Click(sender As Object, e As EventArgs) Handles btnGravar.Click
        If txtDisNivel.Text = String.Empty Then
            MsgBox("Nível de acesso inválido.", vbInformation)
            txtNivel.Focus()
            Exit Sub
        End If
        If txtNome.Text.Trim = String.Empty Then
            MsgBox("Nome do usuário inválido.", vbInformation)
            txtNome.Focus()
            Exit Sub
        End If
        If Not IsNumeric(txtCodigo.Text) Then
            MsgBox("Código do usuário inválido.", vbInformation)
            txtCodigo.Focus()
            Exit Sub
        End If
        If clsGeBanco.dataTable("SELECT * FROM usuarios WHERE codigo=" & txtCodigo.Text).Rows.Count > 0 And CodigoAtual = 0 Then
            MsgBox("Usuário já cadastrado.", vbInformation)
            txtCodigo.Focus()
            Exit Sub
        End If

        Try
            Dim err As String = String.Empty
            If crud Is Nothing Then
                crud = New crUsuario
            End If
            With crud
                If CodigoAtual = 0 Then
                    .codigo = CLng(txtCodigo.Text)
                Else
                    .codigo = CodigoAtual
                End If
                .nome = txtNome.Text.Trim
                .nivel = CLng(txtNivel.Text)
                .supervisor = chSup.Checked
                .inativo = chInativo.Checked
                If MsgBox("Deseja reiniciar a senha do usuário", vbQuestion + vbYesNo) = MsgBoxResult.Yes Then
                    .senha = clsGeGeneral.CripSenhaUsuario("1")
                End If
                If CodigoAtual = 0 Then
                    err = .Executar(TabelaBase.Operacao.INSERT)
                Else
                    err = .Executar(TabelaBase.Operacao.UPDATE)
                End If
            End With
            If String.Empty <> err Then
                MsgBox("Erro ao gravar o usuário: " & err, vbCritical)
                Exit Sub
            Else
                Me.Close()
            End If
        Catch ex As Exception
            MsgBox("Erro ao gravar o usuário: " & ex.Message, vbCritical)
        End Try

    End Sub
End Class