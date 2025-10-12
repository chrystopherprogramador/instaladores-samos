Public Class crEspecie
    Inherits TabelaBase

    Public Property codigo As Long
    Public Property nome As String
    Public Property vendas As Long

    Protected Overrides ReadOnly Property NomeTabela As String
        Get
            Return "especies"
        End Get
    End Property

    Protected Overrides ReadOnly Property CampoChave As String
        Get
            Return "codigo"
        End Get
    End Property
End Class
