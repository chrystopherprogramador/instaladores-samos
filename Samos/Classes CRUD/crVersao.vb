Public Class crVersao

    Inherits TabelaBase

    Public Property Codigo As Integer
    Public Property NrVersao As String
    Public Property dataV As Date


    Protected Overrides ReadOnly Property NomeTabela As String
        Get
            Return "versao"
            Throw New NotImplementedException()
        End Get
    End Property

    Protected Overrides ReadOnly Property CampoChave As String
        Get
            Return "codigo"
            Throw New NotImplementedException()
        End Get
    End Property
End Class
