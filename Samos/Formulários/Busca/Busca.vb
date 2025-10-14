Imports Inviktus.Core

Public Class Busca

    Public Property Dados As DataTable
    Public Property ColunasVisuais As Dictionary(Of String, String)
    Public Property CampoRetorno As String
    Public Property CamposFiltro As List(Of String)
    Public Property Resultado As Object

    Public Sub New()
        InitializeComponent()
        Dim form As New clsGeForm(Me, True, False, False)
        form.ConfiguraMe()
    End Sub

    Private Sub Busca_Load(sender As Object, e As EventArgs) Handles MyBase.Load


    End Sub

    Public Sub Inicializar()
        dgvBusca.DataSource = Dados

        ' Aplica nomes visuais
        ConfiguraGridInteligente(dgvBusca, Dados, ColunasVisuais)

        ' Preenche filtro
        cboFiltro.DataSource = CamposFiltro.Select(Function(c) New With {.Valor = c, .Texto = ColunasVisuais(c)}).ToList()
        cboFiltro.DisplayMember = "Texto"
        cboFiltro.ValueMember = "Valor"
    End Sub

    Private Sub txtFiltro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        If cboFiltro.SelectedValue IsNot Nothing Then
            Dim campo = cboFiltro.SelectedValue.ToString()
            Dim filtro = txtFiltro.Text.Replace("'", "''")
            Dim dv As New DataView(Dados)
            dv.RowFilter = $"{campo} LIKE '%{filtro}%'"
            dgvBusca.DataSource = dv
        End If
    End Sub

    Private Sub dgvBusca_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBusca.CellDoubleClick
        If dgvBusca.CurrentRow IsNot Nothing Then
            Resultado = dgvBusca.CurrentRow.Cells(CampoRetorno).Value
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub btnSelecionar_Click(sender As Object, e As EventArgs) Handles btnSelecionar.Click
        If dgvBusca.CurrentRow IsNot Nothing Then
            Resultado = dgvBusca.CurrentRow.Cells(CampoRetorno).Value
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Public Sub ConfiguraGridInteligente(grid As DataGridView, tabela As DataTable, nomesVisuais As Dictionary(Of String, String))
        Dim qtdColunas As Integer = tabela.Columns.Count
        Dim cfg As New CfgGrid(qtdColunas)

        For i As Integer = 0 To qtdColunas - 1
            Dim colName = tabela.Columns(i).ColumnName
            Dim tipo = tabela.Columns(i).DataType

            ' Nome visual
            If nomesVisuais.ContainsKey(colName) Then
                cfg.ColNome(i) = nomesVisuais(colName)
            Else
                cfg.ColNome(i) = colName
            End If

            ' Tamanho automático baseado no conteúdo
            Dim larguraMax As Integer = TextRenderer.MeasureText(cfg.ColNome(i), grid.Font).Width
            For Each row As DataRow In tabela.Rows
                Dim texto = row(colName).ToString()
                larguraMax = Math.Max(larguraMax, TextRenderer.MeasureText(texto, grid.Font).Width)
            Next
            cfg.ColTamanho(i) = Math.Min(larguraMax + 20, 300)

            ' Formato e alinhamento
            If tipo = GetType(Decimal) Or tipo = GetType(Double) Then
                cfg.ColFormato(i) = CfgGrid.enFormato.Moeda
                cfg.ColAlinhamento(i) = CfgGrid.enAlinhamento.Direita
            ElseIf tipo = GetType(Integer) Then
                cfg.ColAlinhamento(i) = CfgGrid.enAlinhamento.Centro
            Else
                cfg.ColAlinhamento(i) = CfgGrid.enAlinhamento.Esquerda
            End If
        Next

        ' Aplica no grid
        cfg.Configura(grid, True)
    End Sub

    Private Sub dgvBusca_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvBusca.CellContentClick

    End Sub

    Private Sub dgvBusca_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvBusca.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dgvBusca.CurrentRow IsNot Nothing Then
                Resultado = dgvBusca.CurrentRow.Cells(CampoRetorno).Value
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub
End Class