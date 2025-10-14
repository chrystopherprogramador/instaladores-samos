Public Class crUsuario
    Inherits TabelaBase

    Public Property codigo As Long
    Public Property nome As String
    Public Property senha As String
    Public Property nivel As Long
    Public Property supervisor As Long
    Public Property inativo As Long



    Protected Overrides ReadOnly Property NomeTabela As String
        Get
            Return "usuarios"
        End Get
    End Property

    Protected Overrides ReadOnly Property CampoChave As String
        Get
            Return "codigo"
        End Get
    End Property
End Class
