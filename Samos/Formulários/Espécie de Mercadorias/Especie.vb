Public Class Especie

    Public Property CodigoAtual As Long = 0
    Private Sub Especie_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try



            Dim form As New clsGeForm(Me, True, False, False, Sub()
                                                                  btnGravar.PerformClick()
                                                              End Sub)
            form.ConfiguraMe()

            If CodigoAtual <> 0 Then
                Dim especie As New crEspecie With {
                    .codigo = CodigoAtual
                }
                especie.Executar(TabelaBase.Operacao.SELEC)

                txtCodigo.Text = especie.codigo
                txtNome.Text = especie.nome
                chEspecie.Checked = especie.vendas
                txtCodigo.Enabled = False
            Else
                txtCodigo.Text = clsGeBanco.IncrementarCampo("especies", "Codigo").ToString()
            End If




        Catch ex As Exception
            MsgBox("Falha ao carregar o formulário: " & ex.Message, vbCritical)
        End Try


    End Sub

    Private Sub btnGravar_Click(sender As Object, e As EventArgs) Handles btnGravar.Click

        If txtCodigo.Text.Trim() = "" Then
            MsgBox("O código da espécie não pode estar vazio.", vbExclamation)
            txtCodigo.Focus()
            Exit Sub
        End If

        If txtNome.Text.Trim() = "" Then
            MsgBox("O nome da espécie não pode estar vazio.", vbExclamation)
            txtNome.Focus()
            Exit Sub
        End If

        Try

            If clsGeBanco.dataTable("select * from especies where codigo = " & txtCodigo.Text).Rows.Count > 0 AndAlso CodigoAtual = 0 Then
                MsgBox("Espécie já cadastrada com este código.", vbExclamation)
                Exit Sub
            End If

            Dim especie As New crEspecie With {
                .codigo = CLng(txtCodigo.Text),
                .nome = txtNome.Text.Trim(),
                .vendas = If(chEspecie.Checked, 1, 0)
            }

            If CodigoAtual = 0 Then
                If especie.Executar(TabelaBase.Operacao.INSERT) = String.Empty Then
                    MsgBox("Espécie cadastrada com sucesso!", vbInformation)
                    Me.Close()
                Else
                    MsgBox("Falha ao cadastrar a espécie.", vbCritical)
                End If
            Else
                If especie.Executar(TabelaBase.Operacao.UPDATE) = String.Empty Then
                    MsgBox("Espécie cadastrada com sucesso!", vbInformation)
                    Me.Close()
                Else
                    MsgBox("Falha ao cadastrar a espécie.", vbCritical)
                End If
            End If

        Catch ex As Exception

        End Try

    End Sub
End Class