Imports System.Windows.Interop
Imports Inviktus.Core
Imports MySql.Data.MySqlClient

Public Class SuitesSelecao

    Dim DtSigla As DataTable = Nothing
    Dim DtSuite As DataTable = Nothing

    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Dim form As New clsGeForm(Me, True, False, False,,, Sub()
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

    Private Sub MontaGrid()


        'Aqui pai o senhor vai fazer a construção do Grid, pensa como se fosse um array ou uma matriz começa sempre da posição 0 para frente
        'Sempre primeiro alimenta o Data Table e após manda construir o grid, primeiro ele vai preencher os dados e depois vai estilizar

        Dim Grid As New CfgGrid(5)

        With Grid
            .ColNome(0) = "Cód."
            .ColNome(1) = "Categoria"
            .ColNome(2) = "Nome"
            .ColNome(3) = "Qtd. Pessoas"
            .ColNome(4) = "Perc Adicional"

            .ColTamanho(0) = 100
            .ColTamanho(1) = 250
            .ColTamanho(2) = 250
            .ColTamanho(3) = 150
            .ColTamanho(4) = 150

            .Configura(GridSuites, True)
        End With

    End Sub
    Private Sub RefreshDados(Optional filtro As String = "", Optional categori As String = "")

        Dim strSql As String = String.Empty
        strSql = "SELECT 
                    s.numero,si.sigla,s.nome,
                    si.qtdpessoa,si.txadd
                    FROM suites s
                    INNER JOIN sigla si on si.codigo = s.sigla "
        If Len(Trim(filtro)) <> 0 Then
            strSql = strSql & "WHERE s.nome LIKE '%" & txtBusca.Text & "%' "
        End If
        strSql = strSql & "ORDER BY s.numero"

        DtSuite = clsGeBanco.dataTable(strSql)
        GridSuites.DataSource = DtSuite
        MontaGrid()

    End Sub
    Private Sub SuitesSelecao_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        'Pai a função que se chama na classe clsGeBanco.dataTable é só uma função para simplificar a conexão do DataTable 
        'O DataTable é como se fosse o DATA do VB6
        'Sempre o senhor referência ele com um select
        Try
            RefreshDados()
            'Outra coisa pai, no vb6 tinha o OnError Goto aqui ainda existe, só que o tratamento de erro por Try Cath é muito melhor, porque pode ir
            'em bloco por bloco dentro do fonte, não é algo que pega uma função geral por exemplo e que ali trata erro por código
            'Aqui por tratar erros específicos, o Exception é algo geral, mas pode usar por exemplo MySqlException que trata só erro que der na classe do banco


            'Aqui vou carregar um DataTable para alimentar o combo de filtros das categorias

            DtSigla = clsGeBanco.dataTable("SELECT codigo,sigla FROM sigla ORDER BY sigla")

            For Each row As DataRow In DtSigla.Rows
                Dim item As New SiglaItem With { 'Sigla Item é uma classe declarada no final do arquivo, com as propriedades de um combo, posso manter o visual para o usuário com o texto e passar o código no value, para o select fica melhor e mais leve
                    .Text = row("sigla").ToString(),
                    .Value = CLng(row("codigo"))
                }
                cmbSiglas.Items.Add(item)
            Next

        Catch ex As Exception
            MsgBox("Erro crítico ao abrir formulário!" &
                    vbCrLf & "Erro: " & ex.Message, vbCritical, "Erro crítico")
            Close()
        End Try

    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click
        Dim myForm As New Suites
        myForm.CodigoAtual = 0
        If myForm.ShowDialog = DialogResult.OK Then
            RefreshDados()
        End If
    End Sub
End Class


Public Class SiglaItem
    Public Property Text As String
    Public Property Value As String

    Public Overrides Function ToString() As String
        Return Text
    End Function
End Class