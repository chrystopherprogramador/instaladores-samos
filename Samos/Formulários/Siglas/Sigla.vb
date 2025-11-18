Public Class Sigla

    Public Property CodigoAtual As Long = 0

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Dim form As New clsGeForm(Me, True, False, False, Sub()
                                                              btnGravar.PerformClick()
                                                          End Sub)
        form.ConfiguraMe()
    End Sub
    Private Sub Sigla_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If CodigoAtual <> 0 Then

            Dim crud As New crSigla

            With crud
                .codigo = CodigoAtual

                .Executar(TabelaBase.Operacao.SELEC)

                txtNome.Text = .sigla
                txtQtdPessoa.Text = .qtdpessoa
                txtTxAdd.Text = Format(.txadd, "F2")
                chcBloqueiaTerminal.Checked = .bloqueiavi
                chcEmissaoContrato.Checked = .emissao

            End With


        End If

    End Sub

    Private Sub btnGravar_Click(sender As Object, e As EventArgs) Handles btnGravar.Click


        If Len(Trim(txtNome.Text)) = 0 Then
            MsgBox("Sigla não informada!", vbCritical)
            txtNome.Select()
            Exit Sub
        End If

        Try

            Dim crud As New crSigla

            With crud
                .sigla = txtNome.Text
                .qtdpessoa = txtQtdPessoa.Text
                .txadd = txtTxAdd.Text
                .bloqueiavi = chcBloqueiaTerminal.Checked
                .emissao = chcEmissaoContrato.Checked


                If CodigoAtual = 0 Then
                    If Len(Trim(.Executar(TabelaBase.Operacao.INSERT))) = 0 Then
                        MsgBox("Registro gravado com sucesso!", vbInformation)
                        Close()
                    Else
                        MsgBox("Erro ao gravar registro!", vbCritical)
                    End If
                Else
                    .codigo = CodigoAtual
                    If Len(Trim(.Executar(TabelaBase.Operacao.UPDATE))) = 0 Then
                        MsgBox("Registro alterado com suscesso!", vbCritical)
                        Close()
                    Else
                        MsgBox("Erro ao alterar registro!", vbCritical)
                    End If
                End If
            End With

        Catch ex As Exception
            MsgBox("Erro ao gravar registro: " & ex.Message, vbCritical)
        End Try

    End Sub
End Class