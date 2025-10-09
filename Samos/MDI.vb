Imports MySql.Data.MySqlClient

Public Class MDI

    'Timers

    Private WithEvents atualizaTimer As New Timer()

    Public painelLateral As New Panel()
    Public lblInfoSuite As New Label()
    Public btnFecharPainel As New Button()

    Dim clsSuite As clsGeCarregaSuite
    Private Sub MDI_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Dupla verificação de carregamento

        If clsGeVariaveis._systemLoaded = False Then
            Dim frmCarregando As New Carregando
            frmCarregando.ShowDialog()
            If clsGeVariaveis._systemLoaded = False Then
                Application.Exit()
                Exit Sub
            End If
        End If


        Dim config As New clsGeForm(Me, True, True, True)
        config.ConfiguraMe()

        clsSuite = New clsGeCarregaSuite(pnSuite, Me)
        clsSuite.CarregaSuite()

        atualizaTimer.Interval = 3000 ' 3000 ms = 3 segundos
        atualizaTimer.Start()

        ConstrutorPainel()

        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is Button OrElse
            TypeOf ctrl Is Label OrElse
            TypeOf ctrl Is Panel OrElse
            TypeOf ctrl Is PictureBox OrElse
            TypeOf ctrl Is FlowLayoutPanel Then

                AddHandler ctrl.Click, AddressOf DetectaCliqueFora
            End If
        Next

        ' Também adiciona ao próprio formulário
        AddHandler Me.Click, AddressOf DetectaCliqueFora

    End Sub

    Private Sub ConstrutorPainel()
        ' Painel lateral
        painelLateral = New Panel() With {
        .Name = "painelLateral",
        .Width = 280,
        .Height = Me.ClientSize.Height,
        .BackColor = Color.FromArgb(230, 240, 240, 240),
        .Location = New Point(Me.ClientSize.Width, 0),
        .Visible = False,
        .Padding = New Padding(12)
    }
        Me.Controls.Add(painelLateral)

        ' Título da suíte
        lblInfoSuite = New Label() With {
        .Name = "lblInfoSuite",
        .Text = "Suíte",
        .Font = New Font("Segoe UI Semibold", 12),
        .ForeColor = Color.DarkSlateGray,
        .Dock = DockStyle.Top,
        .Height = 40,
        .TextAlign = ContentAlignment.MiddleLeft
    }
        painelLateral.Controls.Add(lblInfoSuite)

        ' Botão fechar painel (adicionado antes para respeitar Dock = Bottom)
        btnFecharPainel = New Button() With {
        .Text = "Fechar Painel",
        .Dock = DockStyle.Bottom,
        .Height = 40,
        .BackColor = Color.LightCoral,
        .FlatStyle = FlatStyle.Flat
    }
        btnFecharPainel.FlatAppearance.BorderSize = 0
        AddHandler btnFecharPainel.Click, Sub() FecharPainel()
        painelLateral.Controls.Add(btnFecharPainel)

        ' Painel de botões (entre o topo e o rodapé)
        Dim painelBotoes As New FlowLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.TopDown,
            .WrapContents = False,
            .AutoScroll = True,
            .Padding = New Padding(0, 48, 0, 0) ' 👈 espaço abaixo do título
        }
        painelLateral.Controls.Add(painelBotoes)

        ' Função auxiliar para criar botões
        Dim criarBotao = Function(texto As String, tecla As String, ativo As Boolean) As Button
                             Dim btn As New Button() With {
                             .Text = $"{texto} {tecla}",
                             .Font = New Font("Segoe UI", 10, FontStyle.Bold),
                             .Width = 230,
                             .Height = 40,
                             .Enabled = ativo,
                             .BackColor = If(ativo, Color.LightSteelBlue, Color.Gainsboro),
                             .FlatStyle = FlatStyle.Flat
                         }
                             btn.FlatAppearance.BorderSize = 0
                             Return btn
                         End Function

        ' Adiciona os botões
        painelBotoes.Controls.Add(criarBotao("Ocupar", "F1", True))
        painelBotoes.Controls.Add(criarBotao("Fechar", "F2", True))
        painelBotoes.Controls.Add(criarBotao("Consumos", "F3", True))
        painelBotoes.Controls.Add(criarBotao("Recebimento", "F6", True))
        painelBotoes.Controls.Add(criarBotao("Início Limpeza", "F4", True))
        painelBotoes.Controls.Add(criarBotao("Chaves", "F5", True))
        painelBotoes.Controls.Add(criarBotao("Manutenção", "F7", True))


    End Sub
    Public Sub AbrirPainel()
        painelLateral.Visible = True
        painelLateral.BringToFront() ' 👈 isso garante que ele fique acima de todos

        Dim destinoX = Me.ClientSize.Width - painelLateral.Width
        Dim t As New Timer() With {.Interval = 5}
        AddHandler t.Tick, Sub()
                               If painelLateral.Left > destinoX Then
                                   painelLateral.Left -= 40
                               Else
                                   painelLateral.Left = destinoX
                                   t.Stop()
                               End If
                           End Sub
        t.Start()
    End Sub
    Public Sub FecharPainel()
        Dim t As New Timer() With {.Interval = 10}
        AddHandler t.Tick, Sub()
                               If painelLateral.Left < Me.Width Then
                                   painelLateral.Left += 20
                               Else
                                   painelLateral.Visible = False
                                   t.Stop()
                               End If
                           End Sub
        t.Start()
    End Sub
    Private Sub atualizaTimer_Tick(sender As Object, e As EventArgs) Handles atualizaTimer.Tick
        Task.Run(Sub()
                     clsSuite.AtualizaSuite()
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

        End If
    End Sub

    Private Function CarregaCoresDoBanco() As Dictionary(Of String, Tuple(Of Color, Color))
        Dim coresDict As New Dictionary(Of String, Tuple(Of Color, Color))()

        Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
            conn.Open()
            Dim cmd As New MySqlCommand("SELECT * FROM cores WHERE Codigo = 1", conn)
            Using reader = cmd.ExecuteReader()
                If reader.Read() Then
                    coresDict("D") = Tuple.Create(VB6Color(reader("CorD")), VB6Color(reader("CorFD")))
                    coresDict("O") = Tuple.Create(VB6Color(reader("CorO")), VB6Color(reader("CorFO")))
                    coresDict("L") = Tuple.Create(VB6Color(reader("CorL")), VB6Color(reader("CorFL")))
                    coresDict("S") = Tuple.Create(VB6Color(reader("CorS")), VB6Color(reader("CorFS")))
                    coresDict("C") = Tuple.Create(VB6Color(reader("CorC")), VB6Color(reader("CorFC")))
                    coresDict("G") = Tuple.Create(VB6Color(reader("CorG")), VB6Color(reader("CorFG")))
                    coresDict("M") = Tuple.Create(VB6Color(reader("CorM")), VB6Color(reader("CorFM")))
                    coresDict("X") = Tuple.Create(VB6Color(reader("CorM")), VB6Color(reader("CorFM"))) ' Reservado usa mesma cor de Ocupada
                End If
            End Using
        End Using

        Return coresDict
    End Function
    Private Function VB6Color(vb6ColorValue As Object) As Color
        If vb6ColorValue Is DBNull.Value Then Return Color.Gray

        Dim valor As Integer
        If Integer.TryParse(vb6ColorValue.ToString(), valor) Then
            Dim r = valor And &HFF
            Dim g = (valor >> 8) And &HFF
            Dim b = (valor >> 16) And &HFF
            Return Color.FromArgb(r, g, b)
        End If

        Return Color.Gray
    End Function


    Private Sub DetectaCliqueFora(sender As Object, e As EventArgs)
        Dim mousePos = Me.PointToClient(MousePosition)
        If painelLateral.Visible AndAlso Not painelLateral.Bounds.Contains(mousePos) Then
            FecharPainel()
        End If
    End Sub
End Class
