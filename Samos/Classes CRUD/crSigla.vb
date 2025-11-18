Public Class crSigla

    Inherits TabelaBase

    Public Property codigo As Long
    Public Property sigla As String
    Public Property txadd As Decimal
    Public Property qtdpessoa As Integer
    Public Property bloqueiavi As Boolean
    Public Property emissao As Boolean



    Protected Overrides ReadOnly Property NomeTabela As String
        Get
            Return "sigla"
        End Get
    End Property

    Protected Overrides ReadOnly Property CampoChave As String
        Get
            Return "codigo"
        End Get
    End Property
End Class
