Imports MySql.Data.MySqlClient

Public Class clsGeCarregaSuite


    Private _panel As FlowLayoutPanel
    Private _form As MDI

    Public Sub New(Painel As FlowLayoutPanel, formPrincipal As Form)
        Painel.Controls.Clear()
        _panel = Painel
        _form = formPrincipal
    End Sub

    Public Sub AtualizaSuite()
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

    Public Sub CarregaSuite(Optional filtroSituacao As String = "")
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
                .ForeColor = Color.DimGray,
                .BackColor = Color.WhiteSmoke,
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
                        Dim btn = CreateSuiteButton(numero, situacao, coresDict, larguraBotao, alturaBotao)
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
                                                  _form.lblInfoSuite.Text = $"Suíte {numero}" & vbCrLf & $"Situação: {situacaoDesc}"
                                                  _form.AbrirPainel() ' 👈 chamada direta e segura
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








    Private Function CreateSuiteButton(numero As Integer, situacao As String, coresDict As Dictionary(Of String, Tuple(Of Color, Color)), largura As Integer, altura As Integer) As Button
        Dim btn As New Button()
        btn.Tag = numero
        btn.Width = largura
        btn.Height = altura
        btn.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0

        Dim cores = coresDict(situacao)
        btn.BackColor = cores.Item1
        btn.ForeColor = cores.Item2
        btn.Text = $"Suíte {numero}"

        ' Label extra para M ou X
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


End Class
