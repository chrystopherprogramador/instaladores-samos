Imports MySql.Data.MySqlClient
Public Class crAreaServico


    Public Shared Property Codigo As Long = 0
    Public Shared Property Descricao As String = String.Empty

    Public Shared Property Porta As String = String.Empty
    Public Enum typeAreaServico
        INSERT
        UPDATE
        DELETE
        SELEC
    End Enum

    Public Shared Function ConsisteGravar(Tipo As typeAreaServico) As String

        Select Case Tipo
            Case typeAreaServico.INSERT
                If Codigo = 0 Then
                    Return "O código da área de serviço deve ser informado."
                End If
                Try
                    Inserir()
                Catch ex As Exception
                    Return ex.Message
                End Try
            Case typeAreaServico.DELETE
                If Codigo = 0 Then
                    Return "O código da área de serviço deve ser informado."
                End If
                Try
                    Excluir()
                Catch ex As Exception
                    Return ex.Message
                End Try
            Case typeAreaServico.SELEC
                If Codigo = 0 Then
                    Return "O código da área de serviço deve ser informado."
                End If
                Try
                    Mostrar()
                Catch ex As Exception
                    Return ex.Message
                End Try
            Case typeAreaServico.UPDATE
                If Codigo = 0 Then
                    Return "O código da área de serviço deve ser informado."
                End If
                Try
                    Alterar()
                Catch ex As Exception
                    Return ex.Message
                End Try

            Case Else
                Return "Tipo de operação inválida."
        End Select

        Return String.Empty

    End Function

    Private Shared Sub Excluir()
        Try

            Using conn = New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim cmd As New MySqlCommand("DELETE FROM area WHERE codigo = @codigo", conn)
                cmd.Parameters.AddWithValue("@codigo", Codigo)
                cmd.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Throw New Exception("Erro ao apagar registro na tabela.")
        End Try
    End Sub
    Private Shared Sub Mostrar()

        Try

            Using conn = New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim cmd As New MySqlCommand("SELECT * FROM area WHERE codigo = @codigo", conn)
                cmd.Parameters.AddWithValue("@codigo", Codigo)
                Dim reader As MySqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    Descricao = If(reader("nome") IsNot DBNull.Value, reader("nome").ToString(), String.Empty)
                    Porta = If(reader("porta") IsNot DBNull.Value, reader("porta").ToString(), String.Empty)
                End If
            End Using

        Catch ex As Exception
            Throw New Exception("Erro ao mostrar registro na tabela.")
        End Try

    End Sub
    Private Shared Sub Alterar()


        Try

            Using conn = New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim cmd As New MySqlCommand("UPDATE area SET nome = @nome, porta=@porta WHERE codigo = @codigo", conn)
                cmd.Parameters.AddWithValue("@codigo", Codigo)
                cmd.Parameters.AddWithValue("@nome", Descricao)
                cmd.Parameters.AddWithValue("@porta", Porta)
                cmd.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Throw New Exception("Erro ao alterar registro na tabela.")
        End Try

    End Sub

    Private Shared Sub Inserir()

        Try

            Using conn = New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim cmd As New MySqlCommand("INSERT INTO area (codigo, nome,porta) VALUES (@codigo, @nome,@porta)", conn)
                cmd.Parameters.AddWithValue("@codigo", Codigo)
                cmd.Parameters.AddWithValue("@nome", Descricao)
                cmd.Parameters.AddWithValue("@porta", Porta)
                cmd.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Throw New Exception("Erro ao inserir registro na tabela.")
        End Try


    End Sub


End Class
