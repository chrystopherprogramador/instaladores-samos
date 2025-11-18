Public Class crHistMovto
    Inherits TabelaBase

    Public Property codigo As Long
    Public Property descricao As String
    Public Property tipo As String

    Protected Overrides ReadOnly Property NomeTabela As String
        Get
            Return "historicos"
        End Get
    End Property

    Protected Overrides ReadOnly Property CampoChave As String
        Get
            Return "codigo"
        End Get
    End Property
End Class
