Public Class clsGeForm

    Private _centraliza As Boolean = False
    Private _formulario As Form = Nothing
    Private _maximizado As Boolean = True
    Private _minimizado As Boolean = True
    Private _acaoSalvar As Action = Nothing
    Private _f1Action As Action = Nothing
    Private _f2Action As Action = Nothing
    Private _f3Action As Action = Nothing
    Private _f4Action As Action = Nothing
    Private _f5Action As Action = Nothing
    Private _acaoImprimir As Action = Nothing

    Private ReadOnly fontePadrao As New Font("Segoe UI", 9.75, FontStyle.Regular)

    Public Sub New(Formulario As Form, Centraliza As Boolean, Optional Maximed As Boolean = True,
                   Optional Minimized As Boolean = True, Optional AcaoSalvar As Action = Nothing,
                   Optional F1Acao As Action = Nothing, Optional F2Acao As Action = Nothing, Optional F3Acao As Action = Nothing,
                   Optional F4Acao As Action = Nothing, Optional AcaoImprimir As Action = Nothing)
        ' Construtor padrão
        _centraliza = Centraliza
        _formulario = Formulario
        _maximizado = Maximed
        _minimizado = Minimized
        _acaoSalvar = AcaoSalvar
        _f1Action = F1Acao
        _f2Action = F2Acao
        _f3Action = F3Acao
        _f4Action = F4Acao
        _acaoImprimir = AcaoImprimir
    End Sub


    Public Sub ConfiguraMe()

        Try

            _formulario.FormBorderStyle = FormBorderStyle.FixedSingle
            _formulario.MaximizeBox = _maximizado
            _formulario.MinimizeBox = _minimizado

            If _centraliza Then
                _formulario.StartPosition = FormStartPosition.CenterScreen
            End If

            _formulario.KeyPreview = True
            _formulario.BackColor = Color.FromArgb(249, 250, 251)
            _formulario.Font = New Font("Segoe UI", 9.75, FontStyle.Regular)
            _formulario.AutoScaleMode = AutoScaleMode.Font
            _formulario.AutoSizeMode = AutoSizeMode.GrowAndShrink
            _formulario.ShowIcon = True
            _formulario.ShowInTaskbar = True
            If _maximizado Then
                _formulario.WindowState = FormWindowState.Maximized
            Else
                _formulario.WindowState = FormWindowState.Normal
            End If
            _formulario.Icon = My.Resources.logo

            AddHandler _formulario.KeyDown, Sub(s As Object, e As KeyEventArgs)
                                                If e.KeyCode = Keys.Escape Then
                                                    _formulario.Close()
                                                End If
                                                If e.KeyCode = Keys.Enter Then
                                                    e.Handled = True
                                                    e.SuppressKeyPress = True
                                                    SendKeys.Send("{TAB}")
                                                End If
                                                If e.Control AndAlso e.KeyCode = Keys.S Then
                                                    If _acaoSalvar IsNot Nothing Then _acaoSalvar.Invoke()
                                                End If
                                                If e.KeyCode = Keys.F1 Then
                                                    If _f1Action IsNot Nothing Then _f1Action.Invoke()
                                                End If
                                                If e.KeyCode = Keys.F2 Then
                                                    If _f2Action IsNot Nothing Then _f2Action.Invoke()
                                                End If
                                                If e.KeyCode = Keys.F3 Then
                                                    If _f3Action IsNot Nothing Then _f3Action.Invoke()
                                                End If
                                                If e.KeyCode = Keys.F4 Then
                                                    If _f4Action IsNot Nothing Then _f4Action.Invoke()
                                                End If
                                                If e.Control AndAlso e.KeyCode = Keys.P Then
                                                    If _acaoImprimir IsNot Nothing Then _acaoImprimir.Invoke()
                                                End If
                                            End Sub
            _formulario.SuspendLayout()
            'Estiliza Button no Formulário
            EstilizaBotoes(_formulario)

            'Estiliza TextBox no Formulário
            EstilizaTextBoxes(_formulario)
            _formulario.ResumeLayout()

        Catch ex As Exception
            MsgBox("Erro na configuração do formulário." & vbCrLf & ex.Message, vbCritical + vbOKOnly, "Atenção")
        End Try

    End Sub
    Public Sub CentralizaNaTela(formulario As Form)
        Dim tela As Rectangle = Screen.PrimaryScreen.WorkingArea

        Dim posX As Integer = (tela.Width - formulario.Width) \ 2
        Dim posY As Integer = (tela.Height - formulario.Height) \ 2

        formulario.StartPosition = FormStartPosition.Manual
        formulario.Location = New Point(posX, posY)
    End Sub
    Private Sub EstilizaTextBoxes(container As Control)
        For Each ctrl As Control In container.Controls
            If TypeOf ctrl Is TextBox Then
                Dim txt As TextBox = CType(ctrl, TextBox)

                ' Estilo visual padrão
                If txt.Tag Is Nothing OrElse txt.Tag.ToString() <> "N" Then
                    txt.BorderStyle = BorderStyle.FixedSingle
                    txt.BackColor = Color.White
                    txt.ForeColor = Color.FromArgb(30, 30, 30)
                    txt.Font = fontePadrao
                    txt.Margin = New Padding(5)
                    If txt.Tag <> "P" Then
                        txt.CharacterCasing = CharacterCasing.Upper
                    End If
                    txt.Height = 28
                End If

                ' Formatação monetária
                If txt.Tag IsNot Nothing AndAlso txt.Tag.ToString() = "MO" Then
                    txt.Text = "0,00"
                    AddHandler txt.TextChanged, Sub(sender, e)
                                                    FormatarMoeda(DirectCast(sender, TextBox))
                                                End Sub
                End If

                ' Apenas números
                If txt.Tag IsNot Nothing AndAlso txt.Tag.ToString() = "NU" Then
                    txt.Text = "0"
                    AddHandler txt.KeyPress, Sub(sender, e)
                                                 If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
                                                     e.Handled = True
                                                 End If
                                             End Sub
                End If
            End If

            ' 🔁 Recursivamente estiliza controles filhos
            If ctrl.HasChildren Then
                EstilizaTextBoxes(ctrl)
            End If
        Next
    End Sub

    Private Sub FormatarMoeda(txt As TextBox)
        Dim n As String = String.Empty
        Dim v As Double = 0

        Try
            n = txt.Text.Replace(",", "").Replace(".", "")
            If n.Equals("") Then n = "000"
            n = n.PadLeft(3, "0")
            If n.Length > 3 AndAlso n.Substring(0, 1) = "0" Then n = n.Substring(1)
            v = Convert.ToDouble(n) / 100
            txt.Text = String.Format("{0:N}", v)
            txt.SelectionStart = txt.Text.Length
        Catch ex As Exception
            MessageBox.Show(ex.Message, "TextBoxMoeda")
        End Try
    End Sub
    Private Sub EstilizaBotoes(container As Control)
        For Each ctrl As Control In container.Controls
            If TypeOf ctrl Is Button Then
                Dim btn As Button = CType(ctrl, Button)
                If btn.Tag Is Nothing OrElse btn.Tag.ToString() <> "N" Then
                    btn.BackColor = Color.FromArgb(0, 120, 215)
                    btn.ForeColor = Color.White
                    btn.FlatStyle = FlatStyle.Flat
                    btn.Font = fontePadrao
                    btn.FlatAppearance.BorderSize = 0
                    'btn.Height = 35
                    'btn.Width = 120
                End If
            End If

            ' 🔁 Se o controle tiver filhos, chama recursivamente
            If ctrl.HasChildren Then
                EstilizaBotoes(ctrl)
            End If
        Next
    End Sub


End Class
