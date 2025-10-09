Public Class clsGeVariaveis

    'Variaveis Globais

    'Usuários
    Public Shared vIdUsuario As Integer = 0
    Public Shared vNomeUsuario As String = ""
    Public Shared vNivelAcesso As Integer = 0

    'Sistema
    Public Shared _ipServidor As String = ""
    Public Shared _nomeBanco As String = ""
    Public Shared _usuarioBanco As String = ""
    Public Shared _senhaBanco As String = ""
    Public Shared _portaBanco As String = ""

    Public Shared _strPathBanco As String = ""
    Public Shared _strPathBackup As String = ""
    Public Shared _systemLoaded As Boolean = False

End Class
