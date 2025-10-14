Public Class crSaldoItens
    Inherits TabelaBase

    Public Property item As Long
    Public Property saldo As Double
    Public Property entradas As Long
    Public Property saidas As Long
    Public Property mes As Date

    Protected Overrides ReadOnly Property NomeTabela As String
        Get
            Return "slditens"
        End Get
    End Property

    Protected Overrides ReadOnly Property CampoChave As String
        Get
            Return "item"
        End Get
    End Property
End Class
