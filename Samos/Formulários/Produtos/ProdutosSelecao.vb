Imports Inviktus.Core
Imports MySql.Data.MySqlClient

Public Class ProdutosSelecao

    Private DtProduto As DataTable = Nothing

    Public Sub New()

        InitializeComponent()

        Dim form As New clsGeForm(Me, True, False, True,, Sub()
                                                              btnVincular.PerformClick()
                                                          End Sub, Sub()
                                                                       btnNovo.PerformClick()
                                                                   End Sub, Sub()
                                                                                btnAlterar.PerformClick()
                                                                            End Sub, Sub()
                                                                                         btnExcluir.PerformClick()
                                                                                     End Sub, Sub()
                                                                                                  btnImprimir.PerformClick()
                                                                                              End Sub)
        form.ConfiguraMe()

    End Sub

    Private Sub ProdutosSelecao_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        RefreshDados()

    End Sub

    Private Sub MontaGrid()


        Dim Grid As New CfgGrid(10)

        With Grid
            .ColNome(0) = "Cód."
            .ColNome(1) = "Descrição do Prod"
            .ColNome(2) = "UND"
            .ColNome(3) = "Valor Venda"
            .ColNome(4) = "Valor Custo"
            .ColNome(5) = "EAN"
            .ColNome(6) = "CFOP"
            .ColNome(7) = "NCM"
            .ColNome(8) = "CEST"
            .ColNome(9) = "Categoria"

            .ColTamanho(0) = 60
            .ColTamanho(1) = 250
            .ColTamanho(2) = 60
            .ColTamanho(3) = 80
            .ColTamanho(4) = 80
            .ColTamanho(5) = 100
            .ColTamanho(6) = 60
            .ColTamanho(7) = 80
            .ColTamanho(8) = 80
            .ColTamanho(9) = 200

            .ColFormato(3) = CfgGrid.enFormato.Moeda
            .ColFormato(4) = CfgGrid.enFormato.Moeda

            .ColAlinhamento(0) = CfgGrid.enAlinhamento.Centro
            .ColAlinhamento(1) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(2) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(3) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(4) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(5) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(6) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(7) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(8) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(9) = CfgGrid.enAlinhamento.Esquerda

            .Configura(GridProduto, True)
        End With

    End Sub
    Private Sub RefreshDados()
        Try
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim da As New MySqlDataAdapter("SELECT i.codigo AS 'Código',
                            i.descricao as 'Descrição',
                            i.unidade AS 'Und',
                            i.venda AS 'Venda',
                            i.custo AS 'Custo',
                            i.CodGTIN AS 'EAN',
                            i.CFOP AS 'CFOP',
                            i.NCM AS 'NCM',
                            i.CEst AS 'Cest',
                            e.Nome AS 'Categoria'
                            FROM 
                            itens i 
                            INNER JOIN especies e ON e.Codigo = i.especie
                            ORDER BY i.descricao", conn)
                DtProduto = New DataTable()
                da.Fill(DtProduto)
            End Using
            GridProduto.DataSource = DtProduto
            MontaGrid()
        Catch ex As Exception
            MsgBox("Erro ao atualizar os dados: " & ex.Message, vbCritical)
        End Try
    End Sub



    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim myForm As New RelatorioGenerico
        myForm.DataSource = GridProduto.DataSource
        myForm.Titulo = "Produtos"
        myForm.ShowDialog()
    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        Dim myform As New Produtos With {
            .CodigoAtual = 0
        }
        myform.ShowDialog()
    End Sub

    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click

        If GridProduto.SelectedRows.Count > 0 Then
            Dim myForm As New Produtos With {
                .CodigoAtual = GridProduto.SelectedRows(0).Cells(0).Value
            }
            myForm.ShowDialog()
            RefreshDados()
        Else
            MsgBox("Selecione um produto para alteração.", vbCritical)
        End If


    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click

        If GridProduto.SelectedRows.Count = 0 Then
            MsgBox("Nenhum produto selecionado!", vbCritical)
            Exit Sub
        End If

        If MsgBox("Confirma exclusão deste item...", vbQuestion + vbYesNo) = vbYes Then

            Try
                Dim item As Long = GridProduto.SelectedRows(0).Cells(0).Value

                Dim DtConsumo As DataTable = clsGeBanco.dataTable($"SELECT * FROM consumos where item = {item}")

                If DtConsumo.Rows.Count > 0 Then
                    MsgBox("Este item contem movimentos de consumos... Exclusão bloqueada...", vbCritical)
                    Exit Sub
                Else
                    Dim err As String = String.Empty

                    Dim crsaldo As New crSaldoItens With {.item = item}
                    Dim crsaldo2 As New crSaldoItens2 With {.item = item}
                    Dim crproduto As New crProdutos With {.codigo = item}

                    err = err & vbCrLf & crsaldo.Executar(TabelaBase.Operacao.DELETE)
                    err = err & vbCrLf & crsaldo2.Executar(TabelaBase.Operacao.DELETE)
                    err = err & vbCrLf & crproduto.Executar(TabelaBase.Operacao.DELETE)

                    If err IsNot String.Empty Then
                        MsgBox("Item excluído com sucesso!", vbInformation)
                        RefreshDados()
                    Else
                        MsgBox("Erro na exlusão!" & vbCrLf & err, vbCritical)
                        Exit Sub
                    End If

                End If

            Catch ex As Exception
                MsgBox("Erro ao excluir produto!", vbCritical)
            End Try

        End If
    End Sub

    Private Sub txtBusca_TextChanged(sender As Object, e As EventArgs) Handles txtBusca.TextChanged
        Try


            Dim filtro As String = txtBusca.Text.Trim().Replace("'", "''")

            If DtProduto IsNot Nothing Then

                Dim dtFiltrado As DataTable = DtProduto.Clone()

                For Each row As DataRow In DtProduto.Select($"Descrição LIKE '%{filtro}%'")
                    dtFiltrado.ImportRow(row)
                Next

                GridProduto.DataSource = dtFiltrado

            End If


        Catch ex As Exception
            MsgBox("Erro ao filtrar os dados: " & ex.Message, MsgBoxStyle.Critical, "Atenção")
        End Try
    End Sub
End Class