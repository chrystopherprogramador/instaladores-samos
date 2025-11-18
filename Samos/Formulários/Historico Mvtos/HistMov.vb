Public Class HistMov

    Public Sub New()
        ' Esta chamada é requerida pelo designer.
        InitializeComponent()
        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Dim form As New clsGeForm(Me, True, False, False, Sub()
                                                              btnGravar.PerformClick()
                                                          End Sub)
        form.ConfiguraMe()
    End Sub

    Public Property CodigoAtual As Long = 0
    Private Sub HistMov_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If CodigoAtual = 0 Then
            txtCodigo.Text = clsGeBanco.IncrementarCampo("historicos", "Codigo").ToString()
        Else
            Dim hist As New crHistMovto With {
                .codigo = CodigoAtual
            }
            hist.Executar(TabelaBase.Operacao.SELEC)
            txtCodigo.Text = hist.codigo
            txtNome.Text = hist.descricao
            If hist.tipo = "E" Then
                cmbTipo.SelectedIndex = 0
            Else
                cmbTipo.SelectedIndex = 1
            End If
        End If
    End Sub

    Private Sub btnGravar_Click(sender As Object, e As EventArgs) Handles btnGravar.Click

        If txtCodigo.Text = String.Empty Then
            MsgBox("O código não pode estar vazio.", vbExclamation)
            txtCodigo.Focus()
            Exit Sub
        End If

        If txtNome.Text = String.Empty Then
            MsgBox("A descrição não pode estar vazio.", vbExclamation)
            txtNome.Focus()
            Exit Sub
        End If

        If cmbTipo.SelectedIndex = -1 Then
            MsgBox("Selecione o tipo de movimento.", vbExclamation)
            cmbTipo.Focus()
            Exit Sub
        End If

        Dim cr As New crHistMovto

        With cr

            .codigo = CLng(txtCodigo.Text)
            .descricao = txtNome.Text
            .tipo = Mid(cmbTipo.Text, 1, 1).ToString()

            If CodigoAtual = 0 Then
                If (Len(Trim(.Executar(TabelaBase.Operacao.INSERT))) = 0) Then
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Else
                    MsgBox("Erro ao gravar o histórico de movimento.", vbCritical)
                End If
            Else
                If (Len(Trim(.Executar(TabelaBase.Operacao.UPDATE))) = 0) Then
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Else
                    MsgBox("Erro ao gravar o histórico de movimento.", vbCritical)
                End If
            End If

        End With
    End Sub
End Class