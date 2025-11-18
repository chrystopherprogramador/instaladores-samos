Public Class crOperadoras
    Inherits TabelaBase

    Public Property codigo As Long
    Public Property nome As String
    Public Property Cartaodebito As Boolean
    Public Property CodNFCE As Integer

    Protected Overrides ReadOnly Property NomeTabela As String
        Get
            Return "operadoras"
        End Get
    End Property

    Protected Overrides ReadOnly Property CampoChave As String
        Get
            Return "codigo"
        End Get
    End Property
End Class
