Imports System.Windows.Forms.MonthCalendar
Imports Inviktus.Core
Imports MySql.Data.MySqlClient

Public Class EspecieSelecao

    Private DtEspecie As DataTable = Nothing
    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

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
    Private Sub MontaGrid()


        Dim Grid As New CfgGrid(3)

        With Grid
            .ColNome(0) = "Cód."
            .ColNome(1) = "Nome"
            .ColNome(2) = "Vendas"

            .ColTamanho(0) = 60
            .ColTamanho(1) = 250
            .ColTamanho(2) = 60

            .ColAlinhamento(0) = CfgGrid.enAlinhamento.Centro
            .ColAlinhamento(1) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(2) = CfgGrid.enAlinhamento.Esquerda

            .Configura(GridEspecie, True)
        End With

    End Sub

    Private Sub EspecieSelecao_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        RefreshDados()

    End Sub

    Private Sub RefreshDados()
        Try
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim da As New MySqlDataAdapter("SELECT * FROM especies", conn)
                DtEspecie = New DataTable()
                da.Fill(DtEspecie)
            End Using
            GridEspecie.DataSource = DtEspecie
            MontaGrid()
        Catch ex As Exception
            MsgBox("Erro ao atualizar os dados: " & ex.Message, vbCritical)
        End Try
    End Sub


    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim myForm As New RelatorioGenerico
        myForm.DataSource = GridEspecie.DataSource
        myForm.Titulo = "Categorias de Produtos"
        myForm.ShowDialog()
    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        Dim myForm As New Especie With {
            .CodigoAtual = 0
        }
        If myForm.ShowDialog() = DialogResult.OK Then
            RefreshDados()
        End If
    End Sub

    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click
        If GridEspecie.CurrentRow Is Nothing Then
            MsgBox("Selecione uma espécie para alterar.", vbExclamation)
            Exit Sub
        End If
        Dim myForm As New Especie With {
            .CodigoAtual = CLng(GridEspecie.CurrentRow.Cells(0).Value)
        }
        If myForm.ShowDialog() = DialogResult.OK Then
            RefreshDados()
        End If
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        If GridEspecie.CurrentRow Is Nothing Then
            MsgBox("Selecione uma espécie para excluir.", vbExclamation)
            Exit Sub
        End If
        If MsgBox("Confirma a exclusão da espécie selecionada?", vbYesNo + vbQuestion) = vbYes Then
            Try
                Dim especie As New crEspecie With {
                    .codigo = CLng(GridEspecie.CurrentRow.Cells(0).Value)
                }
                especie.Executar(TabelaBase.Operacao.DELETE)
                RefreshDados()
            Catch ex As Exception
                MsgBox("Erro ao excluir a espécie: " & ex.Message, vbCritical)
            End Try
        End If
    End Sub

    Private Sub txtBusca_TextChanged(sender As Object, e As EventArgs) Handles txtBusca.TextChanged
        Try


            Dim filtro As String = txtBusca.Text.Trim().Replace("'", "''")

            If DtEspecie IsNot Nothing Then

                Dim dtFiltrado As DataTable = DtEspecie.Clone()

                For Each row As DataRow In DtEspecie.Select($"nome LIKE '%{filtro}%'")
                    dtFiltrado.ImportRow(row)
                Next

                GridEspecie.DataSource = dtFiltrado

            End If


        Catch ex As Exception
            MsgBox("Erro ao filtrar os dados: " & ex.Message, MsgBoxStyle.Critical, "Atenção")
        End Try
    End Sub
End Class