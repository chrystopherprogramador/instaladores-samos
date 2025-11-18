Imports Inviktus.Core

Public Class SiglaSelecao

    Dim DtSigla As DataTable = Nothing
    Private Sub montaGrid()

        Dim grid As New CfgGrid(6)


        Try


            With grid

                .ColNome(0) = "Código"
                .ColNome(1) = "Sigla"
                .ColNome(2) = "Tx Adicional"
                .ColNome(3) = "Qtd d/Pessoas"
                .ColNome(4) = "Bloq. Terminal"
                .ColNome(5) = "Emissão"

                .ColTamanho(0) = 60
                .ColTamanho(1) = 60
                .ColTamanho(2) = 100
                .ColTamanho(3) = 100
                .ColTamanho(4) = 100
                .ColTamanho(5) = 100

                .Configura(GridSigla, True)

            End With

        Catch ex As Exception
            MsgBox("Erro ao montar o Grid: " & ex.Message, vbCritical)
        End Try


    End Sub

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Dim form As New clsGeForm(Me, True, False, False,,, Sub()
                                                                btnNovo.PerformClick()
                                                            End Sub, Sub()
                                                                         btnAlterar.PerformClick()
                                                                     End Sub,, Sub()
                                                                                   btnImprimir.PerformClick()
                                                                               End Sub)
        form.ConfiguraMe()

    End Sub

    Private Sub refreshDados()
        DtSigla = clsGeBanco.dataTable("SELECT * FROM sigla ORDER BY sigla")

        GridSigla.DataSource = DtSigla
        montaGrid()
    End Sub
    Private Sub SiglaSelecao_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        refreshDados()

    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        Dim myForm As New Sigla With {.CodigoAtual = 0}
        myForm.ShowDialog()
        refreshDados()
    End Sub

    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click
        If GridSigla.SelectedRows.Count > 0 Then
            Dim myForm As New Sigla With {.CodigoAtual = GridSigla.SelectedRows(0).Cells(0).Value}
            myForm.ShowDialog()
            refreshDados()
        End If
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim myForm As New RelatorioGenerico
        myForm.DataSource = GridSigla.DataSource
        myForm.Titulo = "Relatório de Categorias de Suítes"
        myForm.ShowDialog()
    End Sub
End Class