' AccessToMySQL_Converter.vb
' Conversor simples de Microsoft Access (.mdb) -> MySQL
' Feito em VB.NET (Framework) - esqueleto para adaptar ao seu sistema "Samos Chef"
' Observações:
' - Requer o pacote NuGet MySql.Data (Oracle MySQL Connector/NET).
' - Se usar provedor Jet (Microsoft.Jet.OLEDB.4.0) execute a app em x86; alternativamente instale o ACE (Microsoft.ACE.OLEDB.12.0) em 64-bit.
' - Teste em ambiente local antes de rodar em produção. Faça backup do MDB.
'
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Text
Imports MySql.Data.MySqlClient

Module Program

    Sub Main()
        Console.WriteLine("Converter Access (.mdb) -> MySQL")

        Console.WriteLine("Digite o IP do banco mysql")
        Dim ipNovoBanco As String = Console.ReadLine()
        Console.WriteLine("Digite o nome do banco mysql")
        Dim nomeNovoBanco As String = Console.ReadLine()
        Console.WriteLine("Digite o usuario do banco mysql")
        Dim usuarioNovoBanco As String = Console.ReadLine()
        Console.WriteLine("Digite a senha do banco mysql")
        Dim senhaNovoBanco As String = Console.ReadLine()

        Console.WriteLine("Digite o caminho do banco Access")
        ' CONFIGURA AQUI
        Dim accessFile As String = Console.ReadLine  ' <- altere para o caminho do arquivo .mdb
        Dim accessProvider As String = "Microsoft.ACE.OLEDB.12.0" ' ou "Microsoft.ACE.OLEDB.12.0"
        Dim mysqlConnString As String = $"Server={ipNovoBanco};Database={nomeNovoBanco};User ID={usuarioNovoBanco};Password={senhaNovoBanco};Charset=utf8mb4;" ' <- altere

        Try
            Using accConn As New OleDbConnection($"Provider={accessProvider};Data Source={accessFile};Persist Security Info=False;")
                accConn.Open()
                Console.WriteLine("Conectado ao Access: " & accessFile)

                Dim tableNames = GetAccessTableNames(accConn)
                Console.WriteLine("Tabelas encontradas: " & String.Join(", ", tableNames))

                Using myConn As New MySqlConnection(mysqlConnString)
                    myConn.Open()
                    Console.WriteLine("Conectado ao MySQL: " & myConn.Database)

                    Dim estruturaSql As New StringBuilder()

                    For Each tName In tableNames
                        Console.WriteLine($"Processando tabela: {tName}")

                        If tName.Contains("Erros de conversão") Then
                            Continue For
                        End If

                        Dim columns = GetTableColumns(accConn, tName)
                        Dim pkColumns = GetPrimaryKeyColumns(accConn, tName)

                        Dim createSql = BuildCreateTableMySql(tName, columns, pkColumns)
                        Console.WriteLine("SQL de criação: " & createSql)
                        estruturaSql.AppendLine(createSql)
                        estruturaSql.AppendLine()
                        'Dim cleanSql = createSql.Replace("", vbCrLf)
                        ' Cria tabela (se já existir, opcionalmente dropar/ignorar)
                        Using cmd As New MySqlCommand(createSql, myConn)
                            cmd.ExecuteNonQuery()
                        End Using

                        ' Copia dados
                        CopyDataTable(accConn, myConn, tName, columns)

                        Console.WriteLine($"Tabela {tName} migrada com sucesso.")
                    Next

                    Dim estruturaPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "estrutura_motel.sql")
                    IO.File.WriteAllText(estruturaPath, estruturaSql.ToString(), Encoding.UTF8)
                    Console.WriteLine(vbCrLf & "Arquivo de estrutura salvo em: " & estruturaPath)
                End Using

                accConn.Close()


            End Using

            Console.WriteLine("Migração finalizada.")
        Catch ex As Exception
            Console.WriteLine("Erro: " & ex.Message)
        End Try

        Console.WriteLine("Pressione Enter para sair...")
        Console.ReadLine()
    End Sub

    ' Retorna lista de tabelas de usuário (ignora system tables)
    Function GetAccessTableNames(conn As OleDbConnection) As List(Of String)
        Dim tables As New List(Of String)()
        Dim dt = conn.GetSchema("Tables")
        For Each r As DataRow In dt.Rows
            Dim tableType = r("TABLE_TYPE").ToString()
            If tableType.Equals("TABLE", StringComparison.OrdinalIgnoreCase) Then
                Dim name = r("TABLE_NAME").ToString()
                If Not name.StartsWith("MSys") Then
                    tables.Add(name)
                End If
            End If
        Next
        Return tables
    End Function

    ' Obtem colunas e metadados
    Function GetTableColumns(conn As OleDbConnection, tableName As String) As List(Of ColumnInfo)
        Dim cols As New List(Of ColumnInfo)()

        Dim restrictions() As String = {Nothing, Nothing, tableName}
        Dim dt = conn.GetSchema("Columns", restrictions)

        For Each r As DataRow In dt.Rows
            Dim col As New ColumnInfo()
            col.Name = r("COLUMN_NAME").ToString()
            col.DataType = r("DATA_TYPE") ' inteiro OLEDB data type
            col.TypeName = If(r.Table.Columns.Contains("DATA_TYPE"), r("DATA_TYPE").ToString(), String.Empty)
            If r.Table.Columns.Contains("CHARACTER_MAXIMUM_LENGTH") AndAlso Not IsDBNull(r("CHARACTER_MAXIMUM_LENGTH")) Then
                col.Size = Convert.ToInt32(r("CHARACTER_MAXIMUM_LENGTH"))
            End If
            If r.Table.Columns.Contains("ORDINAL_POSITION") Then
                col.Ordinal = Convert.ToInt32(r("ORDINAL_POSITION"))
            End If
            cols.Add(col)
        Next

        ' Ordena por posição
        cols.Sort(Function(a, b) a.Ordinal.CompareTo(b.Ordinal))
        Return cols
    End Function

    ' Tenta obter chaves primarias
    Function GetPrimaryKeyColumns(conn As OleDbConnection, tableName As String) As List(Of String)
        Dim pks As New List(Of String)()
        Try
            Dim restrictions() As Object = {Nothing, Nothing, tableName}
            Dim dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, restrictions)
            If dt IsNot Nothing Then
                For Each r As DataRow In dt.Rows
                    pks.Add(r("COLUMN_NAME").ToString())
                Next
            End If
        Catch ex As Exception
            ' Alguns drivers/provedores não retornam esse schema; então ignoramos
        End Try
        Return pks
    End Function

    ' Mapeamento simples de tipos Access -> MySQL
    Function MapAccessTypeToMySql(column As ColumnInfo) As String
        Dim typeCode As Integer = Convert.ToInt32(column.DataType)
        Dim size As Integer = If(column.Size <= 0, 255, column.Size)

        Select Case typeCode
            Case 2 ' SmallInt
                Return "SMALLINT"
            Case 3 ' Integer / Long
                Return "INT"
            Case 4 ' Single
                Return "FLOAT"
            Case 5 ' Double
                Return "DOUBLE"
            Case 6 ' Currency
                Return "DECIMAL(18,2)"
            Case 7 ' Date/Time
                Return "DATETIME"
            Case 11 ' Yes/No
                Return "TINYINT(1)"
            Case 17 ' Byte
                Return "TINYINT UNSIGNED"
            Case 130 ' Text
                Return $"VARCHAR({size})"
            Case 131 ' Decimal / Numeric
                Return "DECIMAL(18,4)"
            Case 128 ' OLE Object
                Return "LONGBLOB"
            Case 203 ' Memo / Long Text
                Return "TEXT"
            Case 204 ' Binary
                Return "BLOB"
            Case 205 ' VarBinary
                Return "MEDIUMBLOB"
            Case Else
                Return $"VARCHAR({size})"
        End Select
    End Function

    ' Constrói SQL CREATE TABLE; tenta usar PKs quando disponíveis
    Function BuildCreateTableMySql(tableName As String, columns As List(Of ColumnInfo), pkColumns As List(Of String)) As String
        Dim sb As New StringBuilder()
        sb.AppendLine($"DROP TABLE IF EXISTS `{tableName}`;")
        sb.AppendLine($"CREATE TABLE IF NOT EXISTS `{tableName}` (")


        Dim colDefs As New List(Of String)()
        For Each c In columns
            Dim colType = MapAccessTypeToMySql(c)
            Dim def = $" `{c.Name}` {colType}"


            If pkColumns.Contains(c.Name) Then
                def &= " NOT NULL"
            End If


            colDefs.Add(def)
        Next


        sb.AppendLine(String.Join("," & vbCrLf, colDefs))


        If pkColumns IsNot Nothing AndAlso pkColumns.Count > 0 Then
            sb.AppendLine($", PRIMARY KEY (`{String.Join("`,`", pkColumns)}`)")
        End If


        sb.AppendLine(");")
        Return sb.ToString()
    End Function

    ' Copia dados da tabela Access para MySQL
    Sub CopyDataTable(accConn As OleDbConnection, myConn As MySqlConnection, tableName As String, columns As List(Of ColumnInfo))
        Dim colNames = String.Join(",", columns.ConvertAll(Function(c) "`" & c.Name & "`"))
        Dim paramNames = String.Join(",", columns.ConvertAll(Function(c) "@" & c.Name))

        Dim selectSql = $"SELECT {String.Join(",", columns.ConvertAll(Function(c) "[" & c.Name & "]"))} FROM [{tableName}]"
        Using accCmd As New OleDbCommand(selectSql, accConn)
            Using reader = accCmd.ExecuteReader()
                Dim rowCount As Long = 0
                Using trans = myConn.BeginTransaction()
                    While reader.Read()
                        Dim insertSql = $"INSERT INTO `{tableName}` ({colNames}) VALUES ({paramNames})"
                        Using myCmd As New MySqlCommand(insertSql, myConn, trans)
                            For i As Integer = 0 To columns.Count - 1
                                Dim cname = columns(i).Name
                                Dim val = If(reader.IsDBNull(i), DBNull.Value, reader.GetValue(i))
                                myCmd.Parameters.AddWithValue("@" & cname, val)
                            Next
                            myCmd.ExecuteNonQuery()
                        End Using
                        rowCount += 1
                        If rowCount Mod 100 = 0 Then
                            Console.Write($"{rowCount} ")
                        End If
                    End While
                    trans.Commit()
                End Using
                Console.WriteLine($"{rowCount} registros inseridos em {tableName}.")
            End Using
        End Using
    End Sub

    ' Classe auxiliar para metadados de coluna
    Class ColumnInfo
        Public Property Name As String
        Public Property DataType As Object
        Public Property TypeName As String
        Public Property Size As Integer = 0
        Public Property Ordinal As Integer = 0
    End Class

End Module
