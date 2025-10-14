Public Class crSaldoItens2
    Inherits TabelaBase

    Public Property item As Long
    Public Property saldo As Double
    Public Property area As Long
    Public Property entradas As Long
    Public Property saidas As Long
    Public Property mes As Date

    Protected Overrides ReadOnly Property NomeTabela As String
        Get
            Return "slditens2"
        End Get
    End Property

    Protected Overrides ReadOnly Property CampoChave As String
        Get
            Return "item"
        End Get
    End Property
End Class
