Imports MySql.Data.MySqlClient
Public Class clsGeBanco

    Public Shared Function dataTable(query As String) As DataTable
        Dim dt As New DataTable
        Try
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("Erro ao executar consulta: " & ex.Message)
        End Try
        Return dt
    End Function

    Public Shared Function IncrementarCampo(tabela As String, campo As String) As Integer
        Try
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()

                Dim cmd As New MySqlCommand($"SELECT MAX({campo}) FROM {tabela}", conn)
                Dim resultado = cmd.ExecuteScalar()

                If resultado IsNot DBNull.Value AndAlso resultado IsNot Nothing Then
                    Return Convert.ToInt32(resultado) + 1
                Else
                    Return 1 ' Começa do 1 se não houver registros
                End If
            End Using
        Catch ex As Exception
            MsgBox("Erro ao incrementar campo: " & ex.Message, MsgBoxStyle.Critical)
            Return -1
        End Try
    End Function

End Class
