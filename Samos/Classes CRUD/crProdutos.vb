Public Class crProdutos
    Inherits TabelaBase
    Public Property codigo As Long
    Public Property descricao As String
    Public Property unidade As String
    Public Property categoria As Long
    Public Property tributacao As Integer
    Public Property aliquota As Decimal
    Public Property venda As Decimal
    Public Property custo As Decimal
    Public Property tipo As Integer
    Public Property vendamaxima As Decimal
    Public Property devolucao As String
    Public Property especie As Long
    Public Property minimo As Decimal
    Public Property maximo As Decimal
    Public Property itemadm As Integer
    Public Property frigobar As Decimal
    Public Property servico As Boolean
    Public Property retornavel As Boolean
    Public Property composicaoopcional As Boolean
    Public Property produtobrinde As Boolean
    Public Property CodGTIN As String
    Public Property Consumacao As Boolean
    Public Property CFOP As Integer
    Public Property NCM As Integer
    Public Property expromesp As Boolean
    Public Property CEst As Integer
    Public Property cardapioonline As Boolean
    Public Property foto As String
    Public Property alteraupload As Boolean
    Public Property inativo As Boolean
    Public Property QtdeImp1 As Integer = 0
    Public Property QtdeImp2 As Integer = 0
    Public Property QtdeImp3 As Integer = 0
    Public Property ialterado As Boolean = False

    Protected Overrides ReadOnly Property NomeTabela As String
        Get
            Return "itens"
        End Get
    End Property

    Protected Overrides ReadOnly Property CampoChave As String
        Get
            Return "codigo"
        End Get
    End Property


End Class
