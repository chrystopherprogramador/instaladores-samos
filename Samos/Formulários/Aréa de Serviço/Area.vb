
Imports Mysqlx

Public Class Area

    Public Property CodigoAtual As Long = 0

    Private Sub Area_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Configura Formulário
        Dim form As New clsGeForm(Me, True, False, False, Sub()
                                                              btnGravar.PerformClick()
                                                          End Sub)
        form.ConfiguraMe()


        'Carrega DataTable com todas as impressoras instaladas
        Dim DtImp As DataTable = Gene.ListarImpressorasInstaladas()
        If DtImp.Rows.Count > 0 Then
            For Each row As DataRow In DtImp.Rows
                cmbImpressora.Items.Add(row("Nome").ToString)
            Next
        End If

        'Carrega dados do banco para o formulário

        Try

            crAreaServico.Codigo = CodigoAtual

            If CodigoAtual > 0 Then
                crAreaServico.ConsisteGravar(crAreaServico.typeAreaServico.SELEC)
                txtCodigo.Text = crAreaServico.Codigo
                txtDescricao.Text = crAreaServico.Descricao
                cmbImpressora.Text = crAreaServico.Porta
                txtCodigo.Enabled = False
            Else
                txtCodigo.Text = clsGeBanco.IncrementarCampo("area", "codigo")
            End If

        Catch ex As Exception
            MsgBox("Erro ao carregar formulário.", vbCritical)
            Close()
        End Try




    End Sub

    Private Sub btnGravar_Click(sender As Object, e As EventArgs) Handles btnGravar.Click

        If Val(txtCodigo.Text) = 0 Or txtCodigo.Text = String.Empty Or Not IsNumeric(txtCodigo.Text) Then
            MsgBox("Código inválido!", vbCritical)
        End If

        If txtDescricao.Text = String.Empty Then
            MsgBox("Descrição inválida!", vbCritical)
            Exit Sub
        End If

        crAreaServico.Codigo = txtCodigo.Text
        crAreaServico.Descricao = txtDescricao.Text
        crAreaServico.Porta = cmbImpressora.Text
        If CodigoAtual = 0 Then
            If Not (crAreaServico.ConsisteGravar(crAreaServico.typeAreaServico.INSERT)) = String.Empty Then
                MsgBox("Erro ao gravar os dados!", vbCritical)
            Else

                If clsGeVariaveis.EstoqueSeparado = True Then

                    ProgressoAtualizaSaldo.Visible = True
                    ProgressoAtualizaSaldo.Value = 0
                    ProgressoAtualizaSaldo.Maximum = clsGeBanco.dataTable("SELECT * FROM itens").Rows.Count
                    Application.DoEvents()
                    AtualizaEstoque()

                End If

                MsgBox("Área gravada com sucesso!", vbInformation)
            End If
        Else
            If Not (crAreaServico.ConsisteGravar(crAreaServico.typeAreaServico.UPDATE)) = String.Empty Then
                MsgBox("Erro ao editar os dados!", vbCritical)
            Else
                MsgBox("Área editada com sucesso!", vbInformation)
            End If
        End If

    End Sub


    Public Sub AtualizaEstoque()

        Try



            Dim MesRef As Date = Date.Now.ToString("01/01/yyyy")

            Dim DtItem As DataTable = clsGeBanco.dataTable("SELECT * FROM itens")

            For Each row As DataRow In DtItem.Rows


                For X = 1 To 12

                    Dim saldo As New crSaldoItens2 With {
                        .item = row("codigo"),
                        .saldo = 0,
                        .area = crAreaServico.Codigo,
                        .entradas = 0,
                        .saidas = 0,
                        .mes = MesRef
                    }
                    saldo.Executar(TabelaBase.Operacao.INSERT)

                    MesRef = DateAdd(DateInterval.Month, 1, MesRef)


                Next

                ProgressoAtualizaSaldo.Value += 1

            Next

        Catch ex As Exception
            MsgBox("Erro ao atualizar o estoque!", vbCritical)
        Finally
            ProgressoAtualizaSaldo.Visible = False
        End Try
    End Sub

End Class