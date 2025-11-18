Imports Inviktus.Core
Imports MySql.Data.MySqlClient

Public Class UsuariosSelecao

    Private DtUsuario As DataTable = Nothing
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

        Dim grid As New CfgGrid(5)

        With grid

            .ColNome(0) = "Cod."
            .ColNome(1) = "Usuário"
            .ColNome(2) = "Nível"
            .ColNome(3) = "Supervisor"
            .ColNome(4) = "Inativo"

            .ColTamanho(0) = 60
            .ColTamanho(1) = 200
            .ColTamanho(2) = 150
            .ColTamanho(3) = 80
            .ColTamanho(4) = 80

            .ColAlinhamento(0) = DataGridViewContentAlignment.MiddleCenter
            .ColAlinhamento(1) = DataGridViewContentAlignment.MiddleLeft
            .ColAlinhamento(2) = DataGridViewContentAlignment.MiddleLeft
            .ColAlinhamento(3) = DataGridViewContentAlignment.MiddleCenter
            .ColAlinhamento(4) = DataGridViewContentAlignment.MiddleCenter
            .ColFormato(0) = "0000"


            .Configura(GridUsuarios, True)

        End With

    End Sub
    Private Sub RefreshDados()
        Try
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim da As New MySqlDataAdapter("SELECT u.codigo AS 'Codigo',
	                                            u.nome AS 'Nome',
	                                            n.Descricao AS 'Nível',
	                                            u.Supervisor AS 'Supervisor',
	                                            u.inativo AS 'Inativo'
                                                FROM 
                                                usuarios u 
                                                INNER JOIN niveisacesso n ON n.Nivel = u.nivel
                                                GROUP BY u.codigo,u.nome,n.Descricao,u.Supervisor,u.inativo
                                                ", conn)
                DtUsuario = New DataTable()
                da.Fill(DtUsuario)
            End Using
            GridUsuarios.DataSource = DtUsuario
            MontaGrid()
        Catch ex As Exception
            MsgBox("Erro ao atualizar os dados: " & ex.Message, vbCritical)
        End Try
    End Sub
    Private Sub UsuariosSelecao_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshDados()
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim myForm As New RelatorioGenerico
        myForm.Text = "Relatório de Usuários"
        myForm.DataSource = DtUsuario
        myForm.ShowDialog()
        RefreshDados()
    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        Dim myForm As New Usuario
        myForm.CodigoAtual = 0
        myForm.ShowDialog()
        RefreshDados()
    End Sub

    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click
        If GridUsuarios.SelectedRows.Count = 0 Then
            MsgBox("Selecione um usuário para alterar.", vbInformation)
            Exit Sub
        End If
        Dim myform As New Usuario
        myform.CodigoAtual = CLng(GridUsuarios.SelectedRows(0).Cells(0).Value)
        myform.ShowDialog()
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        If GridUsuarios.SelectedRows.Count = 0 Then
            MsgBox("Selecione um usuário para excluir.", vbInformation)
            Exit Sub
        End If
        If MsgBox("Confirma a exclusão do usuário selecionado?", vbQuestion + vbYesNo) = vbNo Then
            Exit Sub
        Else
            Try
                Dim crud As New crUsuario
                crud.codigo = CLng(GridUsuarios.SelectedRows(0).Cells(0).Value)
                If Not String.Empty = crud.Executar(TabelaBase.Operacao.DELETE) Then
                    MsgBox("Erro ao excluir o usuário.", vbCritical)
                Else
                    MsgBox("Usuário excluído com sucesso.", vbInformation)
                    RefreshDados()
                End If
            Catch ex As Exception
                MsgBox("Erro ao excluir o usuário: " & ex.Message, vbCritical)
            End Try
        End If
    End Sub

    Private Sub btnPesquisar_Click(sender As Object, e As EventArgs)

    End Sub
End Class