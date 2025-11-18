Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports Samos.clsGeCarregaSuite

Public Class MDI

    'Timers

    Private WithEvents atualizaTimer As New Timer()

    Private WithEvents timerHora As New Timer()



    Dim clsSuite As clsGeCarregaSuite


    Public Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        'Dupla verificação de carregamento



        If clsGeVariaveis._systemLoaded = False Then
            Dim frmCarregando As New Carregando
            frmCarregando.ShowDialog()
            If clsGeVariaveis._systemLoaded = False Then
                Application.Exit()
                Exit Sub
            End If
        End If

        If clsGeVariaveis.vUserLogado = False Then
            Application.Exit()
            Exit Sub
        End If




    End Sub
    Private Sub MDI_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim config As New clsGeForm(Me, True, True, True)
        config.ConfiguraMe()

        ToolTipCmdMenu.SelectedIndex = 0


        clsSuite = New clsGeCarregaSuite(pnSuite, Me)
        clsSuite.CarregaSuite()

        atualizaTimer.Interval = 3000
        atualizaTimer.Start()
        timerHora.Interval = 1000
        timerHora.Start()



        AtualizaLabelsDeSituacao()

        clsSuite.ConstrutorPainel()

        ToolTipCmdMenu.SelectedIndex = clsGeVariaveis.ConfMenu

    End Sub

    Private Sub timerHora_Tick(sender As Object, e As EventArgs) Handles timerHora.Tick
        disHora.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
    End Sub

    Private Sub atualizaTimer_Tick(sender As Object, e As EventArgs) Handles atualizaTimer.Tick
        Dim modoSelecionado As ModoLayoutSuite

        Select Case ToolTipCmdMenu.SelectedIndex
            Case 0
                modoSelecionado = ModoLayoutSuite.Kanban
            Case 1
                modoSelecionado = ModoLayoutSuite.GradeSimples
            Case Else
                modoSelecionado = ModoLayoutSuite.Kanban
        End Select

        Task.Run(Sub()
                     clsSuite.AtualizaSuite(modoSelecionado)
                     Me.Invoke(Sub()
                                   AtualizaLabelsDeSituacao()
                               End Sub)
                 End Sub)
    End Sub
    Private Sub AtualizaLabelsDeSituacao()
        Try
            Dim coresDict = CarregaCoresDoBanco()
            Dim contadores As New Dictionary(Of String, Integer)

            ' Consulta ao banco para contar suítes por situação
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim cmd As New MySqlCommand("SELECT situacao, COUNT(*) AS total FROM suites GROUP BY situacao", conn)
                Using reader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim situacao As String = reader("situacao").ToString()
                        Dim total As Integer = Convert.ToInt32(reader("total"))
                        contadores(situacao) = total
                    End While
                End Using
            End Using

            ' Atualiza os labels dentro do painel PNSTATUS
            For Each ctrl As Control In PNSTATUS.Controls
                If TypeOf ctrl Is Label AndAlso ctrl.Name.StartsWith("dis") Then
                    Dim status = ctrl.Name.Substring(3) ' Ex: "O", "D", etc.

                    ' Aplica cor
                    If coresDict.ContainsKey(status) Then
                        Dim cor = coresDict(status)
                        ctrl.BackColor = cor.Item1
                        ctrl.ForeColor = cor.Item2
                    End If

                    ' Aplica contador
                    If contadores.ContainsKey(status) Then
                        ctrl.Text = contadores(status).ToString()
                    Else
                        ctrl.Text = "0"
                    End If
                End If
            Next

        Catch ex As Exception
            MsgBox("Erro ao atualizar os labels de situação: " & ex.Message, vbCritical)
        End Try
    End Sub


    Private Sub MDI_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If clsGeVariaveis._systemLoaded Then
            If clsGeVariaveis.vUserLogado = False Then
                Dim myForm As New Acesso
                myForm.ShowDialog()
            Else
                lblUsuario.Text = $"Usuário logado {clsGeVariaveis.vNomeUsuario}"
            End If

            Bloqueio()
        End If
    End Sub

    Private Function CarregaCoresDoBanco() As Dictionary(Of String, Tuple(Of Color, Color))
        Dim coresDict As New Dictionary(Of String, Tuple(Of Color, Color))()

        Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
            conn.Open()
            Dim cmd As New MySqlCommand("SELECT * FROM cores WHERE Codigo = 1", conn)
            Using reader = cmd.ExecuteReader()
                If reader.Read() Then
                    coresDict("D") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorD")), clsGeGeneral.VB6Color(reader("CorFD")))
                    coresDict("O") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorO")), clsGeGeneral.VB6Color(reader("CorFO")))
                    coresDict("L") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorL")), clsGeGeneral.VB6Color(reader("CorFL")))
                    coresDict("S") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorS")), clsGeGeneral.VB6Color(reader("CorFS")))
                    coresDict("C") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorC")), clsGeGeneral.VB6Color(reader("CorFC")))
                    coresDict("G") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorG")), clsGeGeneral.VB6Color(reader("CorFG")))
                    coresDict("M") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorM")), clsGeGeneral.VB6Color(reader("CorFM")))
                    coresDict("X") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorM")), clsGeGeneral.VB6Color(reader("CorFM"))) ' Reservado usa mesma cor de Ocupada
                End If
            End Using
        End Using

        Return coresDict
    End Function



    Private Sub ToolStripComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolTipCmdMenu.SelectedIndexChanged

        Dim Ini As New IniReader(Application.StartupPath & "\AdmMotel.ini")

        Dim modoSelecionado As ModoLayoutSuite

        Select Case ToolTipCmdMenu.SelectedIndex
            Case 0
                modoSelecionado = ModoLayoutSuite.Kanban
            Case 1
                modoSelecionado = ModoLayoutSuite.GradeSimples
            Case Else
                Exit Sub
        End Select

        Ini.WriteValue("CONFIG", "MENU", ToolTipCmdMenu.SelectedIndex)

        clsSuite = New clsGeCarregaSuite(pnSuite, Me)
        clsSuite.CarregaSuite(modoSelecionado)
        clsSuite.AtualizaSuite(modoSelecionado)
        clsSuite.ConstrutorPainel()

    End Sub

    Private Sub InformaTarifaçãoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InformaTarifaçãoToolStripMenuItem.Click
        clsGeGeneral.OpenForm(Of InformaTarifacao)()
    End Sub

    Private Sub ÁreaDeServiçosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÁreaDeServiçosToolStripMenuItem.Click
        clsGeGeneral.OpenForm(Of AreaSelecao)()
    End Sub

    Private Sub TrocaDeUsuáriosToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles TrocaDeUsuáriosToolStripMenuItem1.Click
        clsGeGeneral.OpenForm(Of Acesso)()
    End Sub

    Private Sub CategoriasDeMercadoriasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CategoriasDeMercadoriasToolStripMenuItem.Click
        clsGeGeneral.OpenForm(Of EspecieSelecao)()
    End Sub

    Private Sub ProdToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProdToolStripMenuItem.Click
        clsGeGeneral.OpenForm(Of ProdutosSelecao)()
    End Sub

    Private Sub SairToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SairToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub MDI_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If MsgBox("Deseja realmente sair do sistema?", vbQuestion + vbYesNo, "Atenção") = vbNo Then
            e.Cancel = True
        Else
            Application.Exit()
        End If
    End Sub

    Private Sub UsuáriosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UsuáriosToolStripMenuItem.Click
        clsGeGeneral.OpenForm(Of UsuariosSelecao)()
    End Sub

    Private Sub LicençaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LicençaToolStripMenuItem.Click
        clsGeGeneral.OpenForm(Of Licenca)()
    End Sub

    Private Sub CategoriasDeSuítesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CategoriasDeSuítesToolStripMenuItem.Click
        clsGeGeneral.OpenForm(Of SiglaSelecao)()
    End Sub

    Private Sub SuítesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SuítesToolStripMenuItem.Click

    End Sub

    Private Sub HistóricoDeMovtosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HistóricoDeMovtosToolStripMenuItem.Click
        clsGeGeneral.OpenForm(Of HistMovtSelecao)()
    End Sub

    Private Sub OperadorasDeCartãoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OperadorasDeCartãoToolStripMenuItem.Click
        clsGeGeneral.OpenForm(Of OperadorasSelecao)()
    End Sub
End Class
