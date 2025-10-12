Imports MySql.Data.MySqlClient

Public Class InformaTarifacao

    Dim txtTarifacao(7, 23) As TextBox
    Private lblLegenda(15) As Label
    Private tarifacoesDisponiveis As DataTable

    Private coresFundo As List(Of Color) = New List(Of Color) From {
        Color.LightBlue, Color.LightGreen, Color.LightSalmon, Color.LightPink,
        Color.LightGoldenrodYellow, Color.LightGray, Color.MistyRose, Color.PaleTurquoise,
        Color.Thistle, Color.Honeydew, Color.Lavender, Color.LemonChiffon,
        Color.Azure, Color.Beige, Color.BlanchedAlmond, Color.Cornsilk,
        Color.Gainsboro, Color.Moccasin, Color.OldLace, Color.PowderBlue
    }
    Private coresTexto As List(Of Color) = New List(Of Color) From {
        Color.DarkBlue, Color.DarkGreen, Color.DarkRed, Color.DarkMagenta,
        Color.DarkGoldenrod, Color.Black, Color.Maroon, Color.Teal,
        Color.Indigo, Color.Olive, Color.MidnightBlue, Color.SaddleBrown,
        Color.Navy, Color.DarkSlateGray, Color.Firebrick, Color.Chocolate,
        Color.DimGray, Color.Brown, Color.DarkCyan, Color.DarkOrchid
    }
    Sub PopularLegenda(flpLegenda As FlowLayoutPanel, tarifacoes As DataTable)
        flpLegenda.Controls.Clear()

        For i As Integer = 0 To Math.Min(tarifacoes.Rows.Count - 1, 15)
            Dim linha = tarifacoes.Rows(i)
            Dim corIndex As Integer = i Mod coresFundo.Count

            Dim lbl As New Label() With {
            .Text = $"{linha("codigo"):000} - {linha("descricao")}",
            .Tag = linha("codigo"),
            .BackColor = coresFundo(corIndex),
            .ForeColor = coresTexto(corIndex),
            .Font = New Font("Segoe UI", 9.75F, FontStyle.Regular),
            .AutoSize = False,
            .Width = flpLegenda.Width - 20,
            .Height = 28,
            .Margin = New Padding(2),
            .TextAlign = ContentAlignment.MiddleLeft
        }

            lblLegenda(i) = lbl
            flpLegenda.Controls.Add(lbl)
        Next
    End Sub
    Private Sub selecao(campo As TextBox)
        campo.SelectAll()
    End Sub
    Private Sub VerificaTarifacao(campo As TextBox)
        Dim codigo = Val(campo.Text)
        Dim linha = tarifacoesDisponiveis.Select($"codigo = {codigo}").FirstOrDefault()

        If linha IsNot Nothing Then
            Dim index = tarifacoesDisponiveis.Rows.IndexOf(linha)
            campo.BackColor = coresFundo(index Mod coresFundo.Count)
            campo.ForeColor = coresTexto(index Mod coresTexto.Count)
        Else
            campo.Text = "0"
            campo.BackColor = Color.White
            campo.ForeColor = Color.Black
        End If
    End Sub
    Sub PopularGradeTarifacao(tbl As TableLayoutPanel, tarifacoesSalvas As DataTable)
        tbl.SuspendLayout()
        tbl.Controls.Clear()
        tbl.CellBorderStyle = TableLayoutPanelCellBorderStyle.None

        ' Configuração da grade
        tbl.ColumnCount = 9 ' 1 coluna para horas + 8 dias
        tbl.RowCount = 25   ' 1 linha para cabeçalho + 24 horas

        tbl.ColumnStyles.Clear()
        tbl.RowStyles.Clear()

        ' Coluna de horas
        tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 65)) ' legenda de hora

        ' Colunas dos dias
        For i = 1 To 8
            tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 55)) ' cada dia
        Next

        ' Linha de cabeçalho
        tbl.RowStyles.Add(New RowStyle(SizeType.Absolute, 26)) ' legenda dos dias

        ' Linhas das horas
        For i = 1 To 24
            tbl.RowStyles.Add(New RowStyle(SizeType.Absolute, 18)) ' cada hora
        Next

        ' Legenda dos dias (linha 0)
        Dim dias() As String = {"Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Fer"}
        For col As Integer = 1 To 8
            Dim lblDia As New Label() With {
            .Text = dias(col - 1),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Font = New Font("Segoe UI", 9.75F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(30, 30, 30),
            .BackColor = Color.Gainsboro,
            .Margin = Padding.Empty,
            .AutoSize = False,
            .Size = New Size(55, 18)
        }
            tbl.Controls.Add(lblDia, col, 0)
        Next

        ' Legenda das horas (coluna 0)
        For row As Integer = 1 To 24
            Dim lblHora As New Label() With {
            .Text = $"{row - 1:00}:00",
            .TextAlign = ContentAlignment.MiddleRight,
            .Font = New Font("Segoe UI", 9.75F, FontStyle.Regular),
            .ForeColor = Color.FromArgb(30, 30, 30),
            .BackColor = Color.Gainsboro,
            .Margin = Padding.Empty,
            .AutoSize = False,
            .Size = New Size(65, 18)
        }
            tbl.Controls.Add(lblHora, 0, row)
        Next

        ' TextBoxes: 8 dias × 24 horas
        Dim tabIndexCounter As Integer = 0
        For col As Integer = 1 To 8
            For row As Integer = 1 To 24
                Dim diaIndex = col - 1
                Dim horaIndex = row - 1

                Dim txt As New TextBox() With {
                .Name = $"txt_{diaIndex}_{horaIndex}",
                .Text = "0",
                .TextAlign = HorizontalAlignment.Center,
                .Tag = New Point(diaIndex, horaIndex),
                .TabIndex = tabIndexCounter,
                .BorderStyle = BorderStyle.FixedSingle,
                .BackColor = Color.White,
                .ForeColor = Color.FromArgb(30, 30, 30),
                .Font = New Font("Segoe UI", 9.75F, FontStyle.Regular),
                .Margin = Padding.Empty,
                .AutoSize = False,
                .Size = New Size(55, 18)
            }
                tabIndexCounter += 1

                ' Preencher com dados salvos
                Dim linha = tarifacoesSalvas.Select($"DiaT = {diaIndex + 1} AND HoraT = {horaIndex}").FirstOrDefault()
                If linha IsNot Nothing Then
                    txt.Text = linha("Codigo").ToString()
                    VerificaTarifacao(txt)
                End If

                AddHandler txt.GotFocus, Sub(s, e) selecao(DirectCast(s, TextBox))
                AddHandler txt.LostFocus, Sub(s, e) VerificaTarifacao(DirectCast(s, TextBox))

                txtTarifacao(diaIndex, horaIndex) = txt
                tbl.Controls.Add(txt, col, row)
            Next
        Next

        tbl.ResumeLayout()
    End Sub


    Private Sub InformaTarifacao_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try


            Dim formC As New clsGeForm(Me, True, False, False, Sub()
                                                                   cmbGravar.PerformClick()
                                                               End Sub)
            formC.ConfiguraMe()

            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim da As New MySqlDataAdapter("SELECT * FROM tarifacaotab ORDER BY codigo", conn)
                tarifacoesDisponiveis = New DataTable()
                da.Fill(tarifacoesDisponiveis)
            End Using

            Dim tarifacoesSalvas As New DataTable()
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim da As New MySqlDataAdapter("SELECT DiaT, HoraT, Codigo FROM tarifacaosuite", conn)
                da.Fill(tarifacoesSalvas)
            End Using

            Dim configTarifacao As New DataTable()
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()
                Dim da As New MySqlDataAdapter("SELECT * FROM config WHERE codigo = 1 LIMIT 1", conn)
                da.Fill(configTarifacao)
            End Using

            Dim linhaConfig = configTarifacao.Rows(0)
            Select Case linhaConfig("avaliacaotarifacao")
                Case "S"
                    cbTipo.SelectedIndex = 1
                Case "P"
                    cbTipo.SelectedIndex = 2
                Case Else
                    cbTipo.SelectedIndex = 0
            End Select

            PopularGradeTarifacao(tblTarifacao, tarifacoesSalvas)
            PopularLegenda(flpLegenda, tarifacoesDisponiveis)



        Catch ex As Exception
            MsgBox("Erro ao carregar tarifas: " & ex.Message, MsgBoxStyle.Critical)
        End Try


    End Sub

    Sub GravarTarifacoes()
        Try
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()

                Dim cmdDelete As New MySqlCommand("DELETE FROM tarifacaosuite", conn)
                cmdDelete.ExecuteNonQuery()

                Dim cmdInsert As New MySqlCommand("INSERT INTO tarifacaosuite (DiaT, HoraT, Codigo) VALUES (@dia, @hora, @codigo)", conn)
                cmdInsert.Parameters.Add("@dia", MySqlDbType.Int32)
                cmdInsert.Parameters.Add("@hora", MySqlDbType.Int32)
                cmdInsert.Parameters.Add("@codigo", MySqlDbType.Int32)

                For dia As Integer = 0 To 7
                    For hora As Integer = 0 To 23
                        Dim txt = txtTarifacao(dia, hora)
                        Dim codigo = Val(txt.Text)

                        If codigo > 0 Then
                            cmdInsert.Parameters("@dia").Value = dia + 1
                            cmdInsert.Parameters("@hora").Value = hora
                            cmdInsert.Parameters("@codigo").Value = codigo
                            cmdInsert.ExecuteNonQuery()
                        End If
                    Next
                Next

                GravarConfiguracaoTarifacao()

                MsgBox("Tarifações gravadas com sucesso!", MsgBoxStyle.Information)
            End Using
        Catch ex As Exception
            MsgBox("Erro ao gravar tarifações: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Sub GravarConfiguracaoTarifacao()
        Try
            Using conn As New MySqlConnection(clsGeVariaveis._strPathBanco)
                conn.Open()

                Dim tipoSelecionado As String = Mid(cbTipo.Text, 1, 1)
                Dim cmd As New MySqlCommand("UPDATE config SET avaliacaotarifacao = @tipo", conn)
                cmd.Parameters.AddWithValue("@tipo", tipoSelecionado)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            MsgBox("Erro ao gravar configuração de avaliação: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub cmbGravar_Click(sender As Object, e As EventArgs) Handles cmbGravar.Click
        GravarTarifacoes()
    End Sub
End Class