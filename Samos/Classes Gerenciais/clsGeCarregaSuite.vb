Imports MySql.Data.MySqlClient

''' <summary>
''' Monta e mantém o mapa de suítes (Grade Simples ou Kanban) com painel lateral de ações.
''' Otimizações: cache de cores (1 consulta a cada 30s em vez de 1 por botão), índice de
''' botões para atualização O(1) e proteções contra erros de layout e de banco.
''' </summary>
Public Class clsGeCarregaSuite

    Public Enum ModoLayoutSuite
        GradeSimples
        Kanban
    End Enum

#Region "Constantes e cache compartilhado"

    Private Const NomePainelLateral As String = "painelLateralSuite"
    Private Const LarguraPainelLateral As Integer = 280
    Private Const CoresCacheSegundos As Integer = 30

    Private Const SqlSuites As String =
        "SELECT s.numero, s.situacao, si.sigla " &
        "FROM suites s INNER JOIN sigla si ON si.codigo = s.sigla " &
        "ORDER BY s.numero"

    Private Shared ReadOnly CoresPadrao As Tuple(Of Color, Color) = Tuple.Create(Color.Gainsboro, Color.Black)

    ' Cores de referência do sistema legado (fundo, texto) usadas como fallback
    ' quando a tabela "cores" não traz um valor válido para o status.
    Private Shared ReadOnly CoresPadraoPorStatus As New Dictionary(Of String, Tuple(Of Color, Color)) From {
        {"D", Tuple.Create(Color.Lime, Color.Black)},
        {"O", Tuple.Create(Color.Red, Color.White)},
        {"L", Tuple.Create(Color.Blue, Color.White)},
        {"S", Tuple.Create(Color.FromArgb(64, 64, 64), Color.White)},
        {"C", Tuple.Create(Color.Yellow, Color.Black)},
        {"G", Tuple.Create(Color.Magenta, Color.White)},
        {"M", Tuple.Create(Color.Black, Color.White)},
        {"X", Tuple.Create(Color.FromArgb(0, 0, 64), Color.White)}
    }

    Private Shared ReadOnly DescricoesSituacao As New Dictionary(Of String, String) From {
        {"D", "Disponível"}, {"O", "Ocupada"}, {"L", "Limpeza"}, {"S", "Suja"},
        {"C", "Check-out"}, {"G", "Garagem"}, {"M", "Manutenção"}, {"X", "Reservada"}
    }

    Private Shared ReadOnly CoresLock As New Object()
    Private Shared _coresCache As Dictionary(Of String, Tuple(Of Color, Color))
    Private Shared _coresCacheExpira As Date = Date.MinValue

#End Region

#Region "Campos de instância"

    Private ReadOnly _panel As FlowLayoutPanel
    Private ReadOnly _form As Form

    ' Índices para atualização O(1) — substituem a varredura recursiva de controles.
    Private ReadOnly _botoesPorNumero As New Dictionary(Of Integer, Button)()
    Private ReadOnly _estadoPorNumero As New Dictionary(Of Integer, Tuple(Of String, String))()

    Private painelLateral As Panel
    Private _animTimer As Timer

    Public lblInfoSuite As New Label()
    Public btnFecharPainel As New Button()

#End Region

    Public Sub New(Painel As FlowLayoutPanel, formPrincipal As Form)
        Painel.Controls.Clear()
        _panel = Painel
        _form = formPrincipal
    End Sub

#Region "Painel lateral"

    Public Sub AbrirPainel()
        If painelLateral Is Nothing OrElse painelLateral.IsDisposed Then ConstrutorPainel()

        painelLateral.Height = _form.ClientSize.Height
        If Not painelLateral.Visible Then painelLateral.Left = _form.ClientSize.Width
        painelLateral.Visible = True
        painelLateral.BringToFront()

        AnimarPainelAte(_form.ClientSize.Width - painelLateral.Width, -40, Nothing)
    End Sub

    Public Sub FecharPainel()
        If painelLateral Is Nothing OrElse painelLateral.IsDisposed Then Exit Sub
        AnimarPainelAte(_form.ClientSize.Width, 40, Sub() painelLateral.Visible = False)
    End Sub

    Private Sub AnimarPainelAte(destinoX As Integer, passo As Integer, aoConcluir As Action)
        PararAnimacao()

        _animTimer = New Timer() With {.Interval = 10}
        AddHandler _animTimer.Tick,
            Sub()
                If painelLateral Is Nothing OrElse painelLateral.IsDisposed Then
                    PararAnimacao()
                    Exit Sub
                End If

                Dim proximo As Integer = painelLateral.Left + passo
                Dim chegou As Boolean = If(passo < 0, proximo <= destinoX, proximo >= destinoX)

                If chegou Then
                    painelLateral.Left = destinoX
                    PararAnimacao()
                    aoConcluir?.Invoke()
                Else
                    painelLateral.Left = proximo
                End If
            End Sub
        _animTimer.Start()
    End Sub

    Private Sub PararAnimacao()
        If _animTimer Is Nothing Then Exit Sub
        _animTimer.Stop()
        _animTimer.Dispose()
        _animTimer = Nothing
    End Sub
    Public Sub ConstrutorPainel()
        PararAnimacao()
        RemoverPainelLateralAnterior()

        painelLateral = New Panel() With {
            .Name = NomePainelLateral,
            .Width = LarguraPainelLateral,
            .Height = _form.ClientSize.Height,
            .BackColor = Color.FromArgb(230, 240, 240, 240),
            .Location = New Point(_form.ClientSize.Width, 0),
            .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Right,
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

        ' Botão fechar painel (Dock = Bottom)
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
            .Padding = New Padding(0, 48, 0, 0)
        }
        painelLateral.Controls.Add(painelBotoes)

        painelBotoes.Controls.AddRange({
            CriarBotaoAcao("Ocupar", "F1", AddressOf OcuparSuite),
            CriarBotaoAcao("Fechar", "F2", AddressOf OcuparSuite),
            CriarBotaoAcao("Consumos", "F3", AddressOf OcuparSuite),
            CriarBotaoAcao("Recebimento", "F6", AddressOf OcuparSuite),
            CriarBotaoAcao("Início Limpeza", "F4", AddressOf OcuparSuite),
            CriarBotaoAcao("Chaves", "F5", AddressOf OcuparSuite),
            CriarBotaoAcao("Manutenção", "F7", AddressOf OcuparSuite)
        })
    End Sub

    Private Sub RemoverPainelLateralAnterior()
        ' Evita acúmulo de painéis órfãos quando a classe é recriada (troca de layout no MDI).
        For Each antigo As Panel In _form.Controls.OfType(Of Panel)().
                Where(Function(p) p.Name = NomePainelLateral).ToList()
            _form.Controls.Remove(antigo)
            antigo.Dispose()
        Next
    End Sub

    Private Function CriarBotaoAcao(texto As String, tecla As String, acao As EventHandler) As Button
        Dim btn As New Button() With {
            .Text = $"{texto} {tecla}",
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .Width = 230,
            .Height = 40,
            .BackColor = Color.LightSteelBlue,
            .FlatStyle = FlatStyle.Flat
        }
        btn.FlatAppearance.BorderSize = 0
        AddHandler btn.Click, acao
        Return btn
    End Function

    Private Sub OcuparSuite(sender As Object, e As EventArgs)
        Dim myForm As New Ocupação With {.codigoAtual = _form.Tag}
        FecharPainel()
        myForm.ShowDialog()
    End Sub

#End Region


#Region "Carga das suítes"

    Public Sub CarregaSuite(Optional modo As ModoLayoutSuite = ModoLayoutSuite.Kanban, Optional filtroSituacao As String = "")
        If modo = ModoLayoutSuite.GradeSimples Then
            CarregaSuiteGradeSimples()
        Else
            CarregaSuiteKanban(filtroSituacao)
        End If
    End Sub

    Private Sub CarregaSuiteGradeSimples()
        Try
            Dim dtSuites As DataTable = CarregarSuitesDoBanco()

            _panel.SuspendLayout()
            Try
                LimparMapa()

                _panel.AutoScroll = True
                _panel.WrapContents = True
                _panel.FlowDirection = FlowDirection.LeftToRight
                _panel.HorizontalScroll.Enabled = False

                Dim colunas As Integer = ColunasParaTotal(dtSuites.Rows.Count)
                Dim linhas As Integer = Math.Max(1, CInt(Math.Ceiling(dtSuites.Rows.Count / CDbl(colunas))))

                Const espacoEntre As Integer = 20
                Dim larguraBotao As Integer = (_panel.Width \ colunas) - espacoEntre
                Dim alturaBotaoMinima As Integer = 100
                Dim alturaBotaoCalculada As Integer = (_panel.Height \ linhas) - espacoEntre
                Dim alturaBotaoFinal As Integer = Math.Max(alturaBotaoCalculada, alturaBotaoMinima)

                For Each row As DataRow In dtSuites.Rows
                    Dim numero As Integer = Convert.ToInt32(row("numero"))
                    Dim sigla As String = row("sigla").ToString()
                    Dim situacao As String = row("situacao").ToString()

                    Dim btn As Button = CriarBotaoSuite(numero, situacao, larguraBotao, alturaBotaoFinal, sigla)
                    btn.Margin = New Padding(5)

                    RegistrarBotao(numero, situacao, sigla, btn)
                    _panel.Controls.Add(btn)
                Next
            Finally
                _panel.ResumeLayout()
            End Try

        Catch ex As Exception
            MsgBox("Erro ao carregar as suítes: " & ex.Message, vbCritical + vbOKOnly, "Erro")
        End Try
    End Sub

    Private Shared Function ColunasParaTotal(totalSuites As Integer) As Integer
        If totalSuites > 54 Then Return 10
        If totalSuites > 40 Then Return 9
        If totalSuites > 24 Then Return 8
        Return 6
    End Function

    Private Sub LimparMapa()
        _panel.Controls.Clear()
        _botoesPorNumero.Clear()
        _estadoPorNumero.Clear()
    End Sub

    Private Sub RegistrarBotao(numero As Integer, situacao As String, sigla As String, btn As Button)
        _botoesPorNumero(numero) = btn
        _estadoPorNumero(numero) = Tuple.Create(situacao, sigla)
        AddHandler btn.Click, Sub() AoClicarSuite(numero)
    End Sub

    Private Sub AoClicarSuite(numero As Integer)
        ' Lê o estado do índice: o painel lateral mostra sempre a situação vigente.
        Dim estado As Tuple(Of String, String) = Nothing
        Dim situacao As String = If(_estadoPorNumero.TryGetValue(numero, estado), estado.Item1, String.Empty)

        lblInfoSuite.Text = $"Suíte {numero}" & vbCrLf & $"Situação: {DescricaoSituacao(situacao)}"
        _form.Tag = numero
        AbrirPainel()
    End Sub

    Public Sub CarregaSuiteKanban(Optional filtroSituacao As String = "")
        Try
            Dim dtSuites As DataTable = CarregarSuitesDoBanco()

            _panel.SuspendLayout()
            Try
                LimparMapa()

                _panel.AutoScroll = True
                _panel.WrapContents = False
                _panel.FlowDirection = FlowDirection.LeftToRight

                Dim siglas = dtSuites.AsEnumerable().
                    Select(Function(r) r.Field(Of String)("sigla")).
                    Distinct().
                    OrderBy(Function(s) s).
                    ToList()

                Const larguraBotao As Integer = 100
                Const alturaBotao As Integer = 100
                Const alturaTotalBotao As Integer = alturaBotao + 10

                Dim maxLinhasPorColuna As Integer = Math.Max(1, (_panel.ClientSize.Height - 60) \ alturaTotalBotao)

                For Each sigla In siglas
                    Dim filtro As String = $"sigla = '{sigla.Replace("'", "''")}'"
                    If filtroSituacao <> "" Then filtro &= $" AND situacao = '{filtroSituacao}'"

                    Dim suitesDaSigla As DataRow() = dtSuites.Select(filtro)
                    Dim total As Integer = suitesDaSigla.Length
                    Dim disponiveis As Integer = suitesDaSigla.Count(Function(r) r("situacao").ToString() = "D")

                    Dim colunasNecessarias As Integer = Math.Max(1, CInt(Math.Ceiling(total / CDbl(maxLinhasPorColuna))))

                    Dim colunaBase As New Panel() With {
                        .Width = (larguraBotao + 12) * colunasNecessarias,
                        .Height = Math.Max(140, _panel.ClientSize.Height - 20),
                        .AutoScroll = False,
                        .Margin = New Padding(4),
                        .BackColor = Color.Transparent
                    }

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

                    Dim painelHorizontal As New FlowLayoutPanel() With {
                        .Dock = DockStyle.Fill,
                        .FlowDirection = FlowDirection.LeftToRight,
                        .WrapContents = False,
                        .AutoScroll = False
                    }

                    For i As Integer = 0 To colunasNecessarias - 1
                        Dim subColuna As New FlowLayoutPanel() With {
                            .Width = larguraBotao + 12,
                            .Height = colunaBase.Height - titulo.Height,
                            .FlowDirection = FlowDirection.TopDown,
                            .WrapContents = False,
                            .AutoScroll = False
                        }

                        For j As Integer = 0 To maxLinhasPorColuna - 1
                            Dim index As Integer = (i * maxLinhasPorColuna) + j
                            If index >= total Then Exit For

                            Dim row As DataRow = suitesDaSigla(index)
                            Dim numero As Integer = Convert.ToInt32(row("numero"))
                            Dim situacao As String = row("situacao").ToString()

                            Dim btn As Button = CriarBotaoSuite(numero, situacao, larguraBotao, alturaBotao, tamanhoFonte:=12)
                            btn.Margin = New Padding(6)

                            RegistrarBotao(numero, situacao, sigla, btn)
                            subColuna.Controls.Add(btn)
                        Next

                        painelHorizontal.Controls.Add(subColuna)
                    Next

                    colunaBase.Controls.Add(painelHorizontal)
                    _panel.Controls.Add(colunaBase)
                Next
            Finally
                _panel.ResumeLayout()
            End Try

        Catch ex As Exception
            MsgBox("Erro ao carregar as suítes: " & ex.Message, vbCritical + vbOKOnly, "Erro")
        End Try
    End Sub

#End Region

#Region "Fábrica de botões"

    Private Shared Function CriarBotaoSuite(numero As Integer, situacao As String, largura As Integer, altura As Integer,
                                            Optional siglaTopo As String = Nothing,
                                            Optional tamanhoFonte As Integer = 0) As Button
        Dim cores As Tuple(Of Color, Color) = ObterCores(situacao)
        If tamanhoFonte <= 0 Then tamanhoFonte = Math.Max(9, Math.Min(26, largura \ 6))

        Dim btn As New Button() With {
            .Tag = numero,
            .Width = largura,
            .Height = altura,
            .Text = numero.ToString(),
            .Font = New Font("Segoe UI", tamanhoFonte, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = cores.Item1,
            .ForeColor = cores.Item2
        }
        btn.FlatAppearance.BorderSize = 0

        ' Sigla (MAST, LUXO, ...) como label no topo do quadrado.
        If siglaTopo IsNot Nothing Then
            btn.Controls.Add(New Label() With {
                .Name = "lblSigla",
                .Text = siglaTopo,
                .Font = New Font("Segoe UI", 10, FontStyle.Bold),
                .ForeColor = cores.Item2,
                .BackColor = Color.Transparent,
                .AutoSize = False,
                .Width = largura,
                .Height = 20,
                .TextAlign = ContentAlignment.MiddleCenter,
                .Location = New Point(0, 4)
            })
        End If

        ' Status extra (Manutenção/Reservada) no rodapé.
        If situacao = "M" OrElse situacao = "X" Then
            btn.Controls.Add(New Label() With {
                .Name = "lblExtra",
                .Text = DescricaoSituacao(situacao),
                .Font = New Font("Segoe UI", 8, FontStyle.Bold),
                .ForeColor = cores.Item2,
                .BackColor = Color.Transparent,
                .AutoSize = False,
                .Width = largura,
                .Height = 18,
                .TextAlign = ContentAlignment.MiddleCenter,
                .Location = New Point(0, altura - 20)
            })
        End If

        Return btn
    End Function

#End Region

#Region "Atualização em tempo real"

    ''' <summary>
    ''' Atualiza somente os botões cuja situação mudou. Seguro para thread de fundo:
    ''' a consulta roda fora da UI e a aplicação é enfileirada via BeginInvoke.
    ''' O parâmetro modo é mantido por compatibilidade — o índice atende os dois layouts.
    ''' </summary>
    Public Sub AtualizaSuite(Optional modo As ModoLayoutSuite = ModoLayoutSuite.Kanban)
        Try
            Dim dtSuites As DataTable = CarregarSuitesDoBanco()

            If _panel Is Nothing OrElse _panel.IsDisposed OrElse Not _panel.IsHandleCreated Then Exit Sub
            _panel.BeginInvoke(Sub() AplicarAtualizacao(dtSuites))

        Catch ex As Exception
            ' Sem MsgBox: este método roda em timer e uma falha transitória geraria
            ' diálogos em série. Loga e tenta de novo no próximo ciclo.
            Diagnostics.Debug.WriteLine($"[clsGeCarregaSuite] Falha ao atualizar suítes: {ex.Message}")
        End Try
    End Sub

    Private Sub AplicarAtualizacao(dtSuites As DataTable)
        Try
            For Each row As DataRow In dtSuites.Rows
                Dim numero As Integer = Convert.ToInt32(row("numero"))
                Dim sigla As String = row("sigla").ToString()
                Dim situacao As String = row("situacao").ToString()

                Dim btn As Button = Nothing
                If Not _botoesPorNumero.TryGetValue(numero, btn) OrElse btn.IsDisposed Then Continue For

                Dim estadoAnterior As Tuple(Of String, String) = Nothing
                If _estadoPorNumero.TryGetValue(numero, estadoAnterior) AndAlso
                   estadoAnterior.Item1 = situacao AndAlso estadoAnterior.Item2 = sigla Then
                    Continue For ' nada mudou, não repinta
                End If

                AtualizaBotao(btn, sigla, situacao)
                _estadoPorNumero(numero) = Tuple.Create(situacao, sigla)
            Next
        Catch ex As Exception
            Diagnostics.Debug.WriteLine($"[clsGeCarregaSuite] Falha ao aplicar atualização: {ex.Message}")
        End Try
    End Sub

    Private Sub AtualizaBotao(btn As Button, sigla As String, situacao As String)
        Dim cores As Tuple(Of Color, Color) = ObterCores(situacao)
        btn.BackColor = cores.Item1
        btn.ForeColor = cores.Item2

        Dim lblSigla As Label = TryCast(btn.Controls("lblSigla"), Label)
        If lblSigla IsNot Nothing Then
            lblSigla.Text = sigla
            lblSigla.ForeColor = cores.Item2
        End If

        Dim lblExtra As Label = TryCast(btn.Controls("lblExtra"), Label)
        If situacao = "M" OrElse situacao = "X" Then
            If lblExtra Is Nothing Then
                btn.Controls.Add(New Label() With {
                    .Name = "lblExtra",
                    .Text = DescricaoSituacao(situacao),
                    .Font = New Font("Segoe UI", 8, FontStyle.Bold),
                    .ForeColor = cores.Item2,
                    .BackColor = Color.Transparent,
                    .AutoSize = False,
                    .Width = btn.Width,
                    .Height = 18,
                    .TextAlign = ContentAlignment.MiddleCenter,
                    .Location = New Point(0, btn.Height - 20)
                })
            Else
                lblExtra.Text = DescricaoSituacao(situacao)
                lblExtra.ForeColor = cores.Item2
            End If
        ElseIf lblExtra IsNot Nothing Then
            btn.Controls.Remove(lblExtra)
            lblExtra.Dispose()
        End If
    End Sub

#End Region

#Region "Acesso a dados"

    Private Shared Function CarregarSuitesDoBanco() As DataTable
        Dim dt As New DataTable()
        Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
            conn.Open()
            Using da As New MySqlDataAdapter(SqlSuites, conn)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    Private Shared Function DescricaoSituacao(situacao As String) As String
        Dim descricao As String = Nothing
        If DescricoesSituacao.TryGetValue(If(situacao, String.Empty), descricao) Then Return descricao
        Return "Indefinida"
    End Function

    Private Shared Function ObterCores(situacao As String) As Tuple(Of Color, Color)
        Dim chave As String = If(situacao, String.Empty)

        Dim padrao As Tuple(Of Color, Color) = Nothing
        If Not CoresPadraoPorStatus.TryGetValue(chave, padrao) Then padrao = CoresPadrao

        Dim cores As Tuple(Of Color, Color) = Nothing
        If Not CarregaCoresDoBanco().TryGetValue(chave, cores) Then Return padrao

        ' Se o banco não trouxe cor válida (parsing falhou -> Color.Empty),
        ' usa o fallback do status para preservar o visual do legado.
        Dim fundo As Color = If(cores.Item1.IsEmpty, padrao.Item1, cores.Item1)
        Dim texto As Color = If(cores.Item2.IsEmpty, padrao.Item2, cores.Item2)
        Return Tuple.Create(fundo, texto)
    End Function

    Private Shared Function CarregaCoresDoBanco() As Dictionary(Of String, Tuple(Of Color, Color))
                    SyncLock CoresLock
                        If _coresCache IsNot Nothing AndAlso Date.Now < _coresCacheExpira Then Return _coresCache

                        Try
                            Dim coresDict As New Dictionary(Of String, Tuple(Of Color, Color))()

                            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                                conn.Open()
                                Using cmd As New MySqlCommand("SELECT * FROM cores WHERE Codigo = 1", conn)
                                    Using reader = cmd.ExecuteReader()
                                        If reader.Read() Then
                                            coresDict("D") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorD")), clsGeGeneral.VB6Color(reader("CorFD")))
                                            coresDict("O") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorO")), clsGeGeneral.VB6Color(reader("CorFO")))
                                            coresDict("L") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorL")), clsGeGeneral.VB6Color(reader("CorFL")))
                                            coresDict("S") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorS")), clsGeGeneral.VB6Color(reader("CorFS")))
                                            coresDict("C") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorC")), clsGeGeneral.VB6Color(reader("CorFC")))
                                            coresDict("G") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorG")), clsGeGeneral.VB6Color(reader("CorFG")))
                                            coresDict("M") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorM")), clsGeGeneral.VB6Color(reader("CorFM")))
                                            coresDict("X") = Tuple.Create(clsGeGeneral.VB6Color(reader("CorM")), clsGeGeneral.VB6Color(reader("CorFM"))) ' Reservada usa as cores de Manutenção
                                        End If
                                    End Using
                                End Using
                            End Using

                            If coresDict.Count > 0 Then
                                _coresCache = coresDict
                                _coresCacheExpira = Date.Now.AddSeconds(CoresCacheSegundos)
                            End If
                        Catch ex As Exception
                            ' Mantém o cache anterior, se existir; cores padrão cobrem o restante.
                            Diagnostics.Debug.WriteLine($"[clsGeCarregaSuite] Falha ao carregar cores: {ex.Message}")
                        End Try

                        If _coresCache Is Nothing Then _coresCache = New Dictionary(Of String, Tuple(Of Color, Color))()
                        Return _coresCache
                    End SyncLock
                End Function

            #End Region

            End Class
