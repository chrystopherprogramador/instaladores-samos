Imports MySql.Data.MySqlClient

Public Class clsGeCarregaSuite


    Private _panel As FlowLayoutPanel
    Private _form As MDI
    Private painelLateral As Panel
    Public lblInfoSuite As New Label()
    Public btnFecharPainel As New Button()
    Public Enum ModoLayoutSuite
        GradeSimples
        Kanban
    End Enum
    Public Sub New(Painel As FlowLayoutPanel, formPrincipal As Form)
        Painel.Controls.Clear()
        _panel = Painel
        _form = formPrincipal
    End Sub
    Public Sub AbrirPainel()
        painelLateral.Visible = True
        painelLateral.BringToFront() ' 👈 isso garante que ele fique acima de todos

        Dim destinoX = MDI.ClientSize.Width - painelLateral.Width
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
                               If painelLateral.Left < MDI.Width Then
                                   painelLateral.Left += 20
                               Else
                                   painelLateral.Visible = False
                                   t.Stop()
                               End If
                           End Sub
        t.Start()
    End Sub
    Public Sub ConstrutorPainel()
        ' Painel lateral
        painelLateral = New Panel() With {
        .Name = "painelLateral",
        .Width = 280,
        .Height = _form.ClientSize.Height,
        .BackColor = Color.FromArgb(230, 240, 240, 240),
        .Location = New Point(_form.ClientSize.Width, 0),
        .Visible = False,
        .Padding = New Padding(12)
    }
        _form.Controls.Add(painelLateral)

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
        Dim criarBotao = Function(texto As String, tecla As String, ativo As Boolean, acao As EventHandler) As Button
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
                             AddHandler btn.Click, acao
                             Return btn
                         End Function

        ' Adiciona os botões
        painelBotoes.Controls.Add(criarBotao("Ocupar", "F1", True, AddressOf OcuparSuite))
        painelBotoes.Controls.Add(criarBotao("Fechar", "F2", True, AddressOf OcuparSuite))
        painelBotoes.Controls.Add(criarBotao("Consumos", "F3", True, AddressOf OcuparSuite))
        painelBotoes.Controls.Add(criarBotao("Recebimento", "F6", True, AddressOf OcuparSuite))
        painelBotoes.Controls.Add(criarBotao("Início Limpeza", "F4", True, AddressOf OcuparSuite))
        painelBotoes.Controls.Add(criarBotao("Chaves", "F5", True, AddressOf OcuparSuite))
        painelBotoes.Controls.Add(criarBotao("Manutenção", "F7", True, AddressOf OcuparSuite))


    End Sub

    Private Sub OcuparSuite(sender As Object, e As EventArgs)
        Dim myForm As New Ocupação With {.codigoAtual = _form.Tag}
        FecharPainel()
        myForm.ShowDialog()
    End Sub

    Private Sub FecharSuite(sender As Object, e As EventArgs)
        ' Código para fechar a suíte
    End Sub


    Private Sub CarregaSuiteGradeSimples()
        Try

            _panel.AutoScroll = True
            _panel.WrapContents = True
            _panel.FlowDirection = FlowDirection.LeftToRight
            _panel.HorizontalScroll.Enabled = False

            Dim dtSuites As New DataTable()
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim sql As String = "SELECT numero, situacao, sigla FROM suites ORDER BY numero"
                Using da As New MySqlDataAdapter(sql, conn)
                    da.Fill(dtSuites)
                End Using
            End Using

            _panel.Controls.Clear()

            Dim colunas As Integer

            Dim linhas As Integer

            If dtSuites.Rows.Count > 54 Then
                colunas = 10
                linhas = Math.Ceiling(dtSuites.Rows.Count / colunas)
            ElseIf dtSuites.Rows.Count > 40 Then
                colunas = 9
                linhas = 6
            ElseIf dtSuites.Rows.Count > 24 Then
                colunas = 8
                linhas = 5
            Else
                colunas = 6
                linhas = 4
            End If
            Dim espacoEntre As Integer = 20
            Dim larguraBotao As Integer = (_panel.Width \ colunas) - espacoEntre
            Dim alturaBotaoMinima As Integer = 100

            Dim alturaBotaoCalculada As Integer = (_panel.Height \ linhas) - espacoEntre
            Dim alturaBotaoFinal As Integer = Math.Max(alturaBotaoCalculada, alturaBotaoMinima)

            Dim x As Integer = 0
            Dim y As Integer = 0
            Dim contador As Integer = 0

            For Each row As DataRow In dtSuites.Rows
                Dim numero As Integer = Convert.ToInt32(row("numero"))
                Dim sigla As String = row("sigla").ToString()
                Dim situacao As String = row("situacao").ToString()

                Dim btn = CreateSuiteButtonSimple(numero, sigla, situacao, larguraBotao, alturaBotaoFinal)
                btn.Location = New Point(x, y)
                btn.Margin = New Padding(5)


                Dim situacaoDesc As String = String.Empty

                Select Case situacao
                    Case "D"
                        situacaoDesc = "Disponível"
                    Case "O"
                        situacaoDesc = "Ocupada"
                    Case "L"
                        situacaoDesc = "Limpeza"
                    Case "S"
                        situacaoDesc = "Suja"
                    Case "C"
                        situacaoDesc = "Check-out"
                    Case "G"
                        situacaoDesc = "Garagem"
                    Case "M"
                        situacaoDesc = "Manutenção"
                    Case "X"
                        situacaoDesc = "Reservada"
                End Select

                AddHandler btn.Click, Sub()
                                          lblInfoSuite.Text = $"Suíte {numero}" & vbCrLf & $"Situação: {situacaoDesc}"
                                          _form.Tag = numero ' Armazena o número da suíte no Tag do formulário principal
                                          AbrirPainel() ' 👈 chamada direta e segura
                                      End Sub
                _panel.Controls.Add(btn)

                contador += 1
                If contador Mod colunas = 0 Then
                    x = 0
                    y += alturaBotaoFinal + espacoEntre
                Else
                    x += larguraBotao + espacoEntre
                End If
            Next

        Catch ex As Exception
            MsgBox("Erro ao carregar as suítes: " & ex.Message, vbCritical + vbOKOnly, "Erro")
        End Try

    End Sub

    Private Function CreateSuiteButtonSimple(numero As Integer, sigla As String, situacao As String, largura As Integer, altura As Integer) As Button

        Dim tamFonte As Integer = largura / 6

        Dim btn As New Button()
        btn.Tag = numero
        btn.Width = largura
        btn.Height = altura
        btn.Font = New Font("Segoe UI", tamFonte, FontStyle.Bold)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        Dim coresDict = CarregaCoresDoBanco()
        Dim cores = coresDict(situacao)
        btn.BackColor = cores.Item1
        btn.ForeColor = cores.Item2

        ' Texto principal (número da suíte)
        btn.Text = $"{numero}"

        ' Label da sigla no canto superior
        Dim lblSigla As New Label() With {
            .Text = sigla,
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .ForeColor = cores.Item2,
            .BackColor = Color.Transparent,
            .AutoSize = False,
            .Width = btn.Width,
            .Height = 20,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Location = New Point(0, 4) ' Pequeno espaçamento do topo
        }

        btn.Controls.Add(lblSigla)

        If situacao = "M" OrElse situacao = "X" Then
            Dim lblStatusExtra As New Label() With {
                .Text = If(situacao = "M", "Manutenção", "Reservada"),
                .Font = New Font("Segoe UI", 10, FontStyle.Bold),
                .ForeColor = cores.Item2,
                .BackColor = Color.Transparent,
                .AutoSize = False,
                .Width = btn.Width,
                .Height = 20,
                .TextAlign = ContentAlignment.MiddleCenter,
                .Location = New Point(0, btn.Height - 24)
            }
            btn.Controls.Add(lblStatusExtra)
        End If

        Return btn
    End Function


    Public Sub AtualizaSuite(Optional modo As ModoLayoutSuite = ModoLayoutSuite.Kanban)
        Try
            Dim dtSuites As New DataTable()
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim sql As String = "SELECT numero, situacao, sigla FROM suites ORDER BY numero"
                Using da As New MySqlDataAdapter(sql, conn)
                    da.Fill(dtSuites)
                End Using
            End Using

            _panel.Invoke(Sub()
                              For Each row As DataRow In dtSuites.Rows
                                  Dim numero As Integer = Convert.ToInt32(row("numero"))
                                  Dim sigla As String = row("sigla").ToString()
                                  Dim situacao As String = row("situacao").ToString()

                                  Dim btnExistente As Button = Nothing

                                  If modo = ModoLayoutSuite.GradeSimples Then
                                      btnExistente = _panel.Controls.OfType(Of Button)().FirstOrDefault(Function(b) CInt(b.Tag) = numero)
                                  Else
                                      For Each colunaBase As Control In _panel.Controls
                                          For Each painelHorizontal As Control In colunaBase.Controls
                                              For Each subColuna As Control In painelHorizontal.Controls
                                                  For Each controle As Control In subColuna.Controls
                                                      If TypeOf controle Is Button AndAlso controle.Tag IsNot Nothing Then
                                                          If CInt(controle.Tag) = numero Then
                                                              btnExistente = DirectCast(controle, Button)
                                                              Exit For
                                                          End If
                                                      End If
                                                  Next
                                                  If btnExistente IsNot Nothing Then Exit For
                                              Next
                                              If btnExistente IsNot Nothing Then Exit For
                                          Next
                                          If btnExistente IsNot Nothing Then Exit For
                                      Next
                                  End If

                                  If btnExistente IsNot Nothing Then
                                      AtualizaBotao(btnExistente, sigla, situacao)
                                  End If
                              Next
                          End Sub)

        Catch ex As Exception
            MsgBox("Erro ao atualizar as suítes: " & ex.Message, vbCritical)
        End Try
    End Sub






    Private Sub AtualizaBotao(btn As Button, sigla As String, situacao As String)
        Dim _coresDict = CarregaCoresDoBanco()
        Dim cores = _coresDict(situacao)
        btn.BackColor = cores.Item1
        btn.ForeColor = cores.Item2

        ' Atualiza sigla (label superior)
        Dim lblSigla = btn.Controls.OfType(Of Label)().FirstOrDefault(Function(l) l.Location.Y = 4)
        If lblSigla IsNot Nothing Then lblSigla.Text = sigla

        ' Atualiza label extra para M ou X
        Dim lblExtra = btn.Controls.OfType(Of Label)().FirstOrDefault(Function(l) l.Location.Y > btn.Height - 30)
        If situacao = "M" OrElse situacao = "X" Then
            If lblExtra Is Nothing Then
                lblExtra = New Label() With {
                .Text = If(situacao = "M", "Manutenção", "Reservada"),
                .Font = New Font("Segoe UI", 10, FontStyle.Bold),
                .ForeColor = cores.Item2,
                .BackColor = Color.Transparent,
                .AutoSize = False,
                .Width = btn.Width,
                .Height = 20,
                .TextAlign = ContentAlignment.MiddleCenter,
                .Location = New Point(0, btn.Height - 24)
            }
                btn.Controls.Add(lblExtra)
            Else
                lblExtra.Text = If(situacao = "M", "Manutenção", "Reservada")
            End If
        ElseIf lblExtra IsNot Nothing Then
            btn.Controls.Remove(lblExtra)
        End If
    End Sub
    Public Sub CarregaSuite(Optional modo As ModoLayoutSuite = ModoLayoutSuite.Kanban, Optional filtroSituacao As String = "")
        If modo = ModoLayoutSuite.GradeSimples Then
            CarregaSuiteGradeSimples()
        Else
            CarregaSuiteKanban(filtroSituacao)
        End If
    End Sub
    Public Sub CarregaSuiteKanban(Optional filtroSituacao As String = "")
        Try
            _panel.Controls.Clear()
            _panel.AutoScroll = True
            _panel.WrapContents = False
            _panel.FlowDirection = FlowDirection.LeftToRight

            Dim dtSuites As New DataTable()
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim sql As String = "SELECT numero, situacao, sigla FROM suites ORDER BY sigla, numero"
                Using da As New MySqlDataAdapter(sql, conn)
                    da.Fill(dtSuites)
                End Using
            End Using

            Dim coresDict = CarregaCoresDoBanco()
            Dim siglas = dtSuites.AsEnumerable().
            Select(Function(r) r.Field(Of String)("sigla")).
            Distinct().
            OrderBy(Function(s) s).ToList()

            Dim X As Integer = 1

            For Each sigla In siglas
                Dim suitesDaSigla = dtSuites.Select($"sigla = '{sigla}'" & If(filtroSituacao <> "", $" AND situacao = '{filtroSituacao}'", ""))
                Dim total = suitesDaSigla.Length
                Dim disponiveis = suitesDaSigla.Count(Function(r) r("situacao").ToString() = "D")

                ' Tamanho fixo dos botões
                Dim larguraBotao As Integer = 100
                Dim alturaBotao As Integer = 100
                Dim margemVertical As Integer = 10
                Dim alturaTotalBotao As Integer = alturaBotao + margemVertical

                ' Quantas linhas cabem na altura da coluna
                Dim maxLinhasPorColuna As Integer = (_panel.Height - 60) \ alturaTotalBotao
                Dim colunasNecessarias As Integer = Math.Ceiling(suitesDaSigla.Length / maxLinhasPorColuna)

                ' Painel base da coluna
                Dim larguraColunaFinal As Integer = (larguraBotao + 12) * colunasNecessarias
                Dim colunaBase As New Panel() With {
                .Width = larguraColunaFinal,
                .Height = _panel.Height - 20,
                .AutoScroll = False,
                .Margin = New Padding(4),
                .BackColor = Color.Transparent
            }

                ' Título da sigla com contador
                Dim titulo As New Label() With {
                .Text = $"{sigla} ({disponiveis}/{total})",
                .Font = New Font("Segoe UI", 10, FontStyle.Regular),
                .ForeColor = Color.White,
                .BackColor = Color.DarkBlue,
                .TextAlign = ContentAlignment.MiddleCenter,
                .Dock = DockStyle.Top,
                .Height = 28,
                .Padding = New Padding(0, 4, 0, 4)
            }
                colunaBase.Controls.Add(titulo)

                ' Painel horizontal que agrupa subcolunas verticais
                Dim painelHorizontal As New FlowLayoutPanel() With {
                .Dock = DockStyle.Fill,
                .FlowDirection = FlowDirection.LeftToRight,
                .WrapContents = False,
                .AutoScroll = False
            }

                ' Criar subcolunas verticais
                For i = 0 To colunasNecessarias - 1
                    Dim subColuna As New FlowLayoutPanel() With {
                    .Width = larguraBotao + 12,
                    .Height = colunaBase.Height - titulo.Height,
                    .FlowDirection = FlowDirection.TopDown,
                    .WrapContents = False,
                    .AutoScroll = False
                }

                    ' Adicionar botões nesta subcoluna
                    For j = 0 To maxLinhasPorColuna - 1
                        Dim index = i * maxLinhasPorColuna + j
                        If index >= suitesDaSigla.Length Then Exit For

                        Dim row = suitesDaSigla(index)
                        Dim numero = Convert.ToInt32(row("numero"))
                        Dim situacao = row("situacao").ToString()
                        Dim btn = CreateSuiteButton(numero, situacao, coresDict, larguraBotao, alturaBotao, j)
                        btn.Margin = New Padding(6)
                        Dim situacaoDesc As String = String.Empty

                        Select Case situacao
                            Case "D"
                                situacaoDesc = "Disponível"
                            Case "O"
                                situacaoDesc = "Ocupada"
                            Case "L"
                                situacaoDesc = "Limpeza"
                            Case "S"
                                situacaoDesc = "Suja"
                            Case "C"
                                situacaoDesc = "Check-out"
                            Case "G"
                                situacaoDesc = "Garagem"
                            Case "M"
                                situacaoDesc = "Manutenção"
                            Case "X"
                                situacaoDesc = "Reservada"
                        End Select

                        ' Clique abre painel lateral com animação
                        AddHandler btn.Click, Sub()
                                                  lblInfoSuite.Text = $"Suíte {numero}" & vbCrLf & $"Situação: {situacaoDesc}"
                                                  _form.Tag = numero ' Armazena o número da suíte no Tag do formulário principal
                                                  AbrirPainel() ' 👈 chamada direta e segura
                                              End Sub

                        subColuna.Controls.Add(btn)
                    Next

                    painelHorizontal.Controls.Add(subColuna)
                Next

                colunaBase.Controls.Add(painelHorizontal)
                _panel.Controls.Add(colunaBase)
            Next

        Catch ex As Exception
            MsgBox("Erro ao carregar as suítes: " & ex.Message, vbCritical)
        End Try
    End Sub


    Private Function CreateSuiteButton(numero As Integer, situacao As String, coresDict As Dictionary(Of String, Tuple(Of Color, Color)), largura As Integer, altura As Integer, linhaIndex As Integer) As Button
        Dim btn As New Button()
        btn.Tag = numero
        btn.Width = largura
        btn.Height = altura
        btn.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        If linhaIndex = 0 Then
            btn.Margin = New Padding(0, 20, 0, 6)
        Else
            btn.Margin = New Padding(0, 6, 0, 6)
        End If
        Dim cores = coresDict(situacao)
        btn.BackColor = cores.Item1
        btn.ForeColor = cores.Item2
        btn.Text = $"{numero}"

        ' Só adiciona label extra se for modo Kanban
        If situacao = "M" OrElse situacao = "X" Then
            Dim lblStatusExtra As New Label() With {
            .Text = If(situacao = "M", "Manutenção", "Reservada"),
            .Font = New Font("Segoe UI", 8, FontStyle.Bold),
            .ForeColor = cores.Item2,
            .BackColor = Color.Transparent,
            .AutoSize = False,
            .Width = btn.Width,
            .Height = 16,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Location = New Point(0, btn.Height - 18)
        }
            btn.Controls.Add(lblStatusExtra)
        End If

        Return btn
    End Function

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


End Class
