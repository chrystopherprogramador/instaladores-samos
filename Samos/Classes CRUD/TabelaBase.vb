Imports MySql.Data.MySqlClient
Imports System.Reflection

Public MustInherit Class TabelaBase
    Protected MustOverride ReadOnly Property NomeTabela As String
    Protected MustOverride ReadOnly Property CampoChave As String

    Public Enum Operacao
        INSERT
        UPDATE
        DELETE
        SELEC
    End Enum

    Public Function Executar(acao As Operacao) As String
        Try
            Select Case acao
                Case Operacao.INSERT : Inserir()
                Case Operacao.UPDATE : Alterar()
                Case Operacao.DELETE : Excluir()
                Case Operacao.SELEC : Mostrar()
                Case Else : Return "Operação inválida."
            End Select
        Catch ex As Exception
            Return ex.Message
        End Try
        Return String.Empty
    End Function

    Private Sub Inserir()
        Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
            conn.Open()

            ' Obtém todos os campos da classe
            Dim todosCampos = ObterCampos()

            ' Filtra apenas os campos com valor útil
            Dim camposValidos = todosCampos.Where(Function(kvp)
                                                      Dim valor = kvp.Value
                                                      If valor Is Nothing Then Return False
                                                      If TypeOf valor Is String AndAlso String.IsNullOrWhiteSpace(valor.ToString()) Then Return False
                                                      Return True
                                                  End Function).ToDictionary(Function(kvp) kvp.Key, Function(kvp) kvp.Value)

            ' Monta a query com os campos válidos
            Dim nomes = String.Join(",", camposValidos.Keys)
            Dim parametros = String.Join(",", camposValidos.Keys.Select(Function(k) "@" & k))
            Dim sql = $"INSERT INTO {NomeTabela} ({nomes}) VALUES ({parametros})"

            Dim cmd As New MySqlCommand(sql, conn)
            For Each kvp In camposValidos
                cmd.Parameters.AddWithValue("@" & kvp.Key, kvp.Value)
            Next

            cmd.ExecuteNonQuery()
        End Using
    End Sub


    Private Sub Alterar()
        Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
            conn.Open()
            Dim campos = ObterCampos()

            ' Filtra apenas os campos com valor útil e que não são a chave
            Dim camposValidos = campos.Where(Function(kvp)
                                                 Dim nome = kvp.Key
                                                 Dim valor = kvp.Value

                                                 If nome = CampoChave Then Return False
                                                 If valor Is Nothing Then Return False
                                                 If TypeOf valor Is String AndAlso String.IsNullOrWhiteSpace(valor.ToString()) Then Return False
                                                 Return True
                                             End Function).ToDictionary(Function(kvp) kvp.Key, Function(kvp) kvp.Value)

            ' Monta o SET apenas com os campos válidos
            Dim setClause = String.Join(",", camposValidos.Keys.Select(Function(k) $"{k}=@{k}"))
            Dim sql = $"UPDATE {NomeTabela} SET {setClause} WHERE {CampoChave}=@{CampoChave}"

            Dim cmd As New MySqlCommand(sql, conn)

            ' Adiciona os parâmetros válidos
            For Each kvp In camposValidos
                cmd.Parameters.AddWithValue("@" & kvp.Key, kvp.Value)
            Next

            ' Adiciona o parâmetro da chave
            cmd.Parameters.AddWithValue("@" & CampoChave, campos(CampoChave))

            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub Excluir()
        Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
            conn.Open()
            Dim campos = ObterCampos()
            Dim sql = $"DELETE FROM {NomeTabela} WHERE {CampoChave}=@{CampoChave}"
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@" & CampoChave, campos(CampoChave))
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub Mostrar()
        Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
            conn.Open()
            Dim campos = ObterCampos()
            Dim sql = $"SELECT * FROM {NomeTabela} WHERE {CampoChave}=@{CampoChave}"
            Dim cmd As New MySqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@" & CampoChave, campos(CampoChave))
            Dim reader = cmd.ExecuteReader()
            If reader.Read() Then
                PreencherPropriedades(reader)
            End If
        End Using
    End Sub

    Private Function ObterCampos() As Dictionary(Of String, Object)
        Dim campos As New Dictionary(Of String, Object)
        For Each prop In Me.GetType().GetProperties(BindingFlags.Public Or BindingFlags.Instance)
            If prop.CanRead AndAlso prop.CanWrite Then
                campos(prop.Name) = prop.GetValue(Me)
            End If
        Next
        Return campos
    End Function

    Private Sub PreencherPropriedades(reader As MySqlDataReader)
        For Each prop In Me.GetType().GetProperties(BindingFlags.Public Or BindingFlags.Instance)
            If reader.HasRows AndAlso reader.GetOrdinal(prop.Name) >= 0 AndAlso Not reader.IsDBNull(reader.GetOrdinal(prop.Name)) Then
                prop.SetValue(Me, Convert.ChangeType(reader(prop.Name), prop.PropertyType))
            End If
        Next
    End Sub
End Class
