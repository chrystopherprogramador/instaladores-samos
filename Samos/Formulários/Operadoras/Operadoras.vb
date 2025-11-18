Public Class Operadoras

    Public Property CodigoAtual As Long = 0
    Public Sub New()
        ' Esta chamada é requerida pelo designer.
        InitializeComponent()
        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Dim form As New clsGeForm(Me, True, False, False, Sub()
                                                              btnGravar.PerformClick()
                                                          End Sub)
        form.ConfiguraMe()
    End Sub
    Private Sub Operadoras_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            If CodigoAtual = 0 Then
                txtCodigo.Text = clsGeBanco.IncrementarCampo("operadoras", "codigo").ToString()
            Else
                Dim op As New crOperadoras With {
                    .codigo = CodigoAtual
                }
                op.Executar(TabelaBase.Operacao.SELEC)
                txtCodigo.Text = op.codigo
                txtNome.Text = op.nome
                chDebito.Checked = op.Cartaodebito
                txtCodNfce.Text = op.CodNFCE
            End If

        Catch ex As Exception
            MsgBox("Erro ao carregar dados da operadora: " & ex.Message, vbCritical)
            Close()
        End Try

    End Sub

    Private Sub btnGravar_Click(sender As Object, e As EventArgs) Handles btnGravar.Click
        If txtNome.Text = String.Empty Then
            MsgBox("O nome da operadora não pode estar vazio.", vbExclamation)
            txtNome.Focus()
            Exit Sub
        End If
        Dim cr As New crOperadoras
        cr.codigo = CLng(txtCodigo.Text)
        cr.nome = txtNome.Text
        cr.Cartaodebito = chDebito.Checked
        cr.CodNFCE = txtCodNfce.Text
        If CodigoAtual = 0 Then
            If (cr.Executar(TabelaBase.Operacao.INSERT) = String.Empty) Then
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MsgBox("Erro ao inserir a operadora.", vbCritical)
            End If
        Else
            If (cr.Executar(TabelaBase.Operacao.UPDATE) = String.Empty) Then
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MsgBox("Erro ao alterar operadora!", vbCritical)
            End If
        End If


    End Sub
End Class