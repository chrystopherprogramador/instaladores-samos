Imports System.ComponentModel
Imports MySql.Data.MySqlClient

Public Class Carregando

    Private WithEvents carregaSistema As New System.ComponentModel.BackgroundWorker()

    Public Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        Dim clsGeForm As New clsGeForm(Me, True, False, False)
        clsGeForm.ConfiguraMe()

    End Sub
    Private Sub Carregando_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        carregaSistema.WorkerReportsProgress = True
        carregaSistema.WorkerSupportsCancellation = False
        carregaSistema.RunWorkerAsync()

    End Sub

    Private Sub carregaSistema_DoWork(sender As Object, e As DoWorkEventArgs) Handles carregaSistema.DoWork
        Try
            ' Etapa 1: Simulação inicial
            carregaSistema.ReportProgress(10)
            System.Threading.Thread.Sleep(500)

            ' Etapa 2: Leitura do arquivo INI
            carregaSistema.ReportProgress(30)
            Dim arquivoIni As String = IO.Path.Combine(Application.StartupPath, "AdmMotel.ini")
            Dim ini As New IniReader(arquivoIni)

            clsGeVariaveis._ipServidor = ini.ReadValue("CONFIG", "IP_SERVIDOR", String.Empty)
            clsGeVariaveis._nomeBanco = ini.ReadValue("CONFIG", "NOME", String.Empty)
            clsGeVariaveis._usuarioBanco = ini.ReadValue("CONFIG", "USUARIO", String.Empty)
            clsGeVariaveis._senhaBanco = ini.ReadValue("CONFIG", "SENHA", String.Empty)
            clsGeVariaveis._portaBanco = ini.ReadValue("CONFIG", "PORTA", "3306")

            ' Etapa 3: Validação básica
            carregaSistema.ReportProgress(50)
            If String.IsNullOrWhiteSpace(clsGeVariaveis._ipServidor) OrElse
               String.IsNullOrWhiteSpace(clsGeVariaveis._nomeBanco) OrElse
               String.IsNullOrWhiteSpace(clsGeVariaveis._usuarioBanco) Then

                Dim myForm As New ConfBanco
                myForm.ShowDialog()
                Application.Exit()
            End If

            clsGeVariaveis._strPathBanco = $"Server={clsGeVariaveis._ipServidor};Port={clsGeVariaveis._portaBanco};Database={clsGeVariaveis._nomeBanco};Uid={clsGeVariaveis._usuarioBanco};Pwd={clsGeVariaveis._senhaBanco};"

            ' Etapa 4: Teste de conexão
            carregaSistema.ReportProgress(70)
            If Not TestaConexao(clsGeVariaveis._strPathBanco) Then
                Dim myForm As New ConfBanco
                myForm.ShowDialog()
                Me.Close()
            Else
                Dim script As New clsGeScriptBanco(clsGeVariaveis._ipServidor,
                                                    clsGeVariaveis._nomeBanco,
                                                        clsGeVariaveis._usuarioBanco,
                                                            clsGeVariaveis._senhaBanco,
                                                                clsGeVariaveis._portaBanco)
                script.executaScript(False)
            End If


            'Carrega configurações gerais

            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Using cmd As New MySqlCommand("SELECT * FROM config LIMIT 1", conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            clsGeVariaveis.EstoqueSeparado = reader.GetBoolean("EstoqueSeparado")
                        End If
                    End Using
                End Using
            End Using

            clsGeVariaveis.ConfMenu = ini.ReadValue("CONFIG", "MENU", 0)

            'Verifica Usuário 999

            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Using cmd As New MySqlCommand("SELECT COUNT(*) as registros FROM usuarios WHERE codigo = 999", conn)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        reader.Read()
                        Dim X As Long = CLng(reader("registros"))

                        reader.Close()
                        If X = 0 Then

                            Dim cmdIn As New MySqlCommand("INSERT INTO usuarios (codigo,nome,senha,nivel,Supervisor,inativo)
                                                        VALUES (@codigo,@nome,@senha,@nivel,@supervisor,@inativo)", conn)

                            With cmdIn.Parameters
                                .AddWithValue("@codigo", 999)
                                .AddWithValue("@nome", "Supervisor")
                                .AddWithValue("@senha", clsGeGeneral.CripSenhaUsuario("1"))
                                .AddWithValue("@nivel", 1)
                                .AddWithValue("@supervisor", True)
                                .AddWithValue("@inativo", False)
                            End With

                            cmdIn.ExecuteNonQuery()

                        End If
                    End Using

                End Using
            End Using

                ' Etapa 5: Finalização
                carregaSistema.ReportProgress(100)
            clsGeVariaveis._systemLoaded = True

        Catch ex As Exception
            e.Result = "Erro: " & ex.Message
            MsgBox("Falha ao inicializar o sistema, por tanto ele será fechado!" & vbCrLf & "Erro apresentado:" &
                   vbCrLf & ex.Message, vbCritical, "ERRO CRÍTICO!")
        End Try
    End Sub
    Private Function TestaConexao(connectionString As String) As Boolean
        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()
                Return True
            End Using
        Catch
            Return False
        End Try
    End Function
    Private Sub carregaSistema_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles carregaSistema.ProgressChanged
        Barra.Value = e.ProgressPercentage
    End Sub

    Private Sub carregaSistema_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles carregaSistema.RunWorkerCompleted
        ' Se o sistema foi carregado com sucesso, fecha o formulário
        If clsGeVariaveis._systemLoaded Then
            Me.Close()
        End If
    End Sub
End Class