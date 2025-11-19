Imports Inviktus.Core

Public Class OperadorasSelecao

    Private DtOperadoras As DataTable = Nothing

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
    Private Sub RefreshDados()

        DtOperadoras = clsGeBanco.dataTable("SELECT codigo,nome,Cartaodebito,CodNFCE from operadoras")
        GridOperadoras.DataSource = DtOperadoras

        Dim grid As New CfgGrid(4)

        With grid

            .ColNome(0) = "Código"
            .ColTamanho(0) = 80
            .ColAlinhamento(0) = DataGridViewContentAlignment.MiddleCenter
            .ColFormato(0) = "000"
            .ColNome(1) = "Nome"
            .ColTamanho(1) = 250
            .ColAlinhamento(1) = DataGridViewContentAlignment.MiddleLeft
            .ColNome(2) = "Cartão Débito"
            .ColTamanho(2) = 120
            .ColAlinhamento(2) = DataGridViewContentAlignment.MiddleCenter
            .ColNome(3) = "Cód. NFCE"
            .ColTamanho(3) = 100
            .ColAlinhamento(3) = DataGridViewContentAlignment.MiddleCenter
            .Configura(GridOperadoras, True)


        End With
    End Sub
    Private Sub OperadorasSelecao_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshDados()
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim myForm As New RelatorioGenerico
        myForm.DataSource = GridOperadoras.DataSource
        myForm.Titulo = "Operadoras de Cartão"
        myForm.ShowDialog()
    End Sub

    Private Sub txtBusca_TextChanged(sender As Object, e As EventArgs) Handles txtBusca.TextChanged
        Try


            Dim filtro As String = txtBusca.Text.Trim().Replace("'", "''")

            If DtOperadoras IsNot Nothing Then

                Dim dtFiltrado As DataTable = DtOperadoras.Clone()

                For Each row As DataRow In DtOperadoras.Select($"nome LIKE '%{filtro}%'")
                    dtFiltrado.ImportRow(row)
                Next

                GridOperadoras.DataSource = dtFiltrado

            End If


        Catch ex As Exception
            MsgBox("Erro ao filtrar os dados: " & ex.Message, MsgBoxStyle.Critical, "Atenção")
        End Try
    End Sub

    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click
        If GridOperadoras.SelectedRows.Count = 0 Then
            MsgBox("Selecione uma operadora para alterar.", vbExclamation)
            Exit Sub
        End If

        Dim myForm As New Operadoras With {.CodigoAtual = GridOperadoras.SelectedRows(0).Cells("codigo").Value}
        If myForm.ShowDialog() = DialogResult.OK Then
            RefreshDados()
        End If
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        If GridOperadoras.SelectedRows.Count = 0 Then
            MsgBox("Selecione uma operadora para a exclusão!", vbExclamation)
            Exit Sub
        End If

        Dim cr As New crOperadoras With {
            .codigo = GridOperadoras.SelectedRows(0).Cells("codigo").Value
        }
        If MsgBox("Confirma a exclusão da operadora '" & cr.nome & "'?", vbQuestion + vbYesNo) = vbYes Then
            If cr.Executar(TabelaBase.Operacao.DELETE) = String.Empty Then
                MsgBox("Operadora excluída com sucesso!", vbInformation)
                RefreshDados()
            Else
                MsgBox("Erro ao excluir a operadora!", vbCritical)
            End If
        End If
    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        Dim myForm As New Operadoras With {.CodigoAtual = 0}
        If myForm.ShowDialog = DialogResult.OK Then
            RefreshDados()
        End If
    End Sub
End Class