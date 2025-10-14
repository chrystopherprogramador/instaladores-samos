Imports Inviktus.Core
Imports MySql.Data.MySqlClient
Public Class AreaSelecao

    Private dtArea As DataTable
    Private Sub MontaGrid()


        Dim Grid As New CfgGrid(3)

        With Grid
            .ColNome(0) = "Cód."
            .ColNome(1) = "Descrição"
            .ColNome(2) = "Impressora"

            .ColTamanho(0) = 60
            .ColTamanho(1) = 250
            .ColTamanho(2) = 200

            .ColAlinhamento(0) = CfgGrid.enAlinhamento.Centro
            .ColAlinhamento(1) = CfgGrid.enAlinhamento.Esquerda
            .ColAlinhamento(2) = CfgGrid.enAlinhamento.Esquerda

            .Configura(GridArea, True)
        End With

    End Sub

    Public Sub New()

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
    Private Sub AreaSelecao_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        RefreshDados()
    End Sub

    Private Sub btnPesquisar_Click(sender As Object, e As EventArgs) Handles btnPesquisar.Click

        Try


            Dim filtro As String = txtBusca.Text.Trim().Replace("'", "''")

            If dtArea IsNot Nothing Then

                Dim dtFiltrado As DataTable = dtArea.Clone()

                For Each row As DataRow In dtArea.Select($"nome LIKE '%{filtro}%'")
                    dtFiltrado.ImportRow(row)
                Next

                GridArea.DataSource = dtFiltrado

            End If


        Catch ex As Exception
            MsgBox("Erro ao filtrar os dados: " & ex.Message, MsgBoxStyle.Critical, "Atenção")
        End Try


    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim myForm As New RelatorioGenerico
        myForm.DataSource = GridArea.DataSource
        myForm.Titulo = "Áreas de Serviço"
        myForm.ShowDialog()
    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        Dim myForm As New Area
        myForm.CodigoAtual = 0
        myForm.ShowDialog()
        RefreshDados()
    End Sub

    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click
        If GridArea.SelectedRows.Count = 0 Then
            MsgBox("Selecione uma área para alterar!", vbExclamation)
            Exit Sub
        Else
            Dim myForm As New Area
            myForm.CodigoAtual = CLng(GridArea.SelectedRows(0).Cells(0).Value)
            myForm.ShowDialog()
            RefreshDados()
        End If
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        If GridArea.SelectedRows.Count = 0 Then
            MsgBox("Selecione uma área para excluir!", vbExclamation)
            Exit Sub
        Else
            If MsgBox("Confirma a exclusão da área de serviço selecionada?", vbQuestion + vbYesNo) = vbYes Then
                Try
                    crAreaServico.Codigo = CLng(GridArea.SelectedRows(0).Cells(0).Value)
                    Dim resultado As String = crAreaServico.ConsisteGravar(crAreaServico.typeAreaServico.DELETE)
                    If resultado = String.Empty Then
                        MsgBox("Área de serviço excluída com sucesso!", vbInformation)
                        RefreshDados()
                    Else
                        MsgBox("Erro ao excluir a área de serviço: " & resultado, vbCritical)
                    End If
                Catch ex As Exception
                    MsgBox("Erro ao excluir a área de serviço: " & ex.Message, vbCritical)
                End Try
            End If
        End If
    End Sub

    Private Sub RefreshDados()
        Try
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim da As New MySqlDataAdapter("SELECT * FROM area", conn)
                dtArea = New DataTable()
                da.Fill(dtArea)
            End Using
            GridArea.DataSource = dtArea
            MontaGrid()
        Catch ex As Exception
            MsgBox("Erro ao atualizar os dados: " & ex.Message, vbCritical)
        End Try
    End Sub
    Private Sub AreaSelecao_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        RefreshDados()
    End Sub
End Class