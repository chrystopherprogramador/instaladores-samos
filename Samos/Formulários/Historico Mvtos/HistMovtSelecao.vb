Imports Inviktus.Core

Public Class HistMovtSelecao

    Private DtHist As DataTable = Nothing

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Dim cls As New clsGeForm(Me, True, False, False,,, Sub()
                                                               btnNovo.PerformClick()
                                                           End Sub, Sub()
                                                                        btnAlterar.PerformClick()
                                                                    End Sub, Sub()
                                                                                 btnExcluir.PerformClick()
                                                                             End Sub, Sub()
                                                                                          btnImprimir.PerformClick()
                                                                                      End Sub)
        cls.ConfiguraMe()
    End Sub

    Public Sub refreshDados()

        Try
            Using conn As New MySql.Data.MySqlClient.MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter("SELECT codigo, descricao, tipo FROM historicos", conn)
                DtHist = New DataTable()
                da.Fill(DtHist)
            End Using
            GridHist.DataSource = DtHist
        Catch ex As Exception
            MsgBox("Erro ao carregar dados: " & ex.Message, vbCritical)
        End Try


        Dim grid As New CfgGrid(3)

        With grid
            .ColNome(0) = "Cód."
            .ColNome(1) = "Descrição"
            .ColNome(2) = "Tipo Movimento"

            .ColTamanho(0) = 150
            .ColTamanho(1) = 300
            .ColTamanho(2) = 200
            .ColAlinhamento(0) = CfgGrid.enAlinhamento.Centro
            .ColAlinhamento(1) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(2) = CfgGrid.enAlinhamento.Esquerda
            .Configura(GridHist, True)
        End With


    End Sub
    Private Sub HistMovt_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        refreshDados()

    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim myForm As New RelatorioGenerico
        myForm.DataSource = CType(GridHist.DataSource, DataTable)
        myForm.Titulo = "Categorias de Produtos"
        myForm.ShowDialog()
    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        Dim myForm As New HistMov With {.CodigoAtual = 0
        }
        If myForm.ShowDialog = DialogResult.OK Then
            refreshDados()
        End If
    End Sub

    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click
        If GridHist.CurrentRow IsNot Nothing Then
            Dim codigo As Integer = Convert.ToInt32(GridHist.CurrentRow.Cells("codigo").Value)
            Dim myForm As New HistMov With {.CodigoAtual = codigo
            }
            If myForm.ShowDialog = DialogResult.OK Then
                refreshDados()
            End If
        Else
            MsgBox("Selecione um histórico para alterar.", vbExclamation)
        End If
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        If GridHist.SelectedRows.Count > 0 Then
            Dim codigo As Integer = Convert.ToInt32(GridHist.SelectedRows(0).Cells("codigo").Value)
            If MsgBox("Tem certeza que deseja excluir o histórico selecionado?", vbQuestion + vbYesNo) = vbYes Then
                Dim cr As New crHistMovto With {
                    .codigo = codigo
                }
                If Len(Trim(cr.Executar(TabelaBase.Operacao.DELETE))) = 0 Then
                    MsgBox("Histórico excluído com sucesso.", vbInformation)
                    refreshDados()
                Else
                    MsgBox("Erro ao excluir o histórico.", vbCritical)
                End If
            End If
        Else
            MsgBox("Selecione um histórico para excluir.", vbExclamation)
        End If
    End Sub

    Private Sub GridHist_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles GridHist.CellEnter
        If GridHist.SelectedRows.Count > 0 Then
            If clsGeBanco.dataTable($"select * from movtosestoque where historico = {GridHist.SelectedRows(0).Cells(0).Value}").Rows.Count > 0 Then
                btnExcluir.Enabled = False
            Else
                btnExcluir.Enabled = True
            End If
        End If
    End Sub
End Class