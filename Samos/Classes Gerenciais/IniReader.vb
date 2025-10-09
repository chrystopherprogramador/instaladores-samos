Imports System.Runtime.InteropServices
Imports System.Text

Public Class IniReader
    Private _filePath As String

    Public Sub New(filePath As String)
        _filePath = filePath

        ' Se o arquivo não existir, cria um vazio
        If Not IO.File.Exists(_filePath) Then
            IO.File.WriteAllText(_filePath, "")
        End If
    End Sub

    ' Lê um valor de uma chave específica dentro de uma seção
    Public Function ReadValue(section As String, key As String, Optional defaultValue As String = "") As String
        Dim buffer As New StringBuilder(1024)
        GetPrivateProfileString(section, key, defaultValue, buffer, buffer.Capacity, _filePath)
        Return buffer.ToString()
    End Function

    ' Escreve um valor em uma chave dentro de uma seção
    Public Sub WriteValue(section As String, key As String, value As String)
        WritePrivateProfileString(section, key, value, _filePath)
    End Sub

    ' Remove uma chave de uma seção
    Public Sub DeleteKey(section As String, key As String)
        WritePrivateProfileString(section, key, Nothing, _filePath)
    End Sub

    ' Remove uma seção inteira
    Public Sub DeleteSection(section As String)
        WritePrivateProfileString(section, Nothing, Nothing, _filePath)
    End Sub

    ' Verifica se o arquivo existe
    Public Function Exists() As Boolean
        Return IO.File.Exists(_filePath)
    End Function

    ' API do Windows para leitura
    <DllImport("kernel32", CharSet:=CharSet.Unicode)>
    Private Shared Function GetPrivateProfileString(section As String, key As String, defaultValue As String,
                                                    retVal As StringBuilder, size As Integer, filePath As String) As Integer
    End Function

    ' API do Windows para escrita
    <DllImport("kernel32", CharSet:=CharSet.Unicode)>
    Private Shared Function WritePrivateProfileString(section As String, key As String, value As String, filePath As String) As Boolean
    End Function
End Class
