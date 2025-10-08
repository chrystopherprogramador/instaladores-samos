Public Class clsGeForm

    Private _centraliza As Boolean = False
    Private _formulario As Form = Nothing
    Private _maximizado As Boolean = True
    Private _minimizado As Boolean = True
    Private _acaoSalvar As Action = Nothing

    Public Sub New(Formulario As Form, Centraliza As Boolean, Optional Maximed As Boolean = True,
                   Optional Minimized As Boolean = True, Optional AcaoSalvar As Action = Nothing)
        ' Construtor padrão
        _centraliza = Centraliza
        _formulario = Formulario
        _maximizado = Maximed
        _minimizado = Minimized
        _acaoSalvar = AcaoSalvar
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
                                                    SendKeys.Send("{TAB}")
                                                End If
                                                If e.Control AndAlso e.KeyCode = Keys.S Then
                                                    If _acaoSalvar IsNot Nothing Then _acaoSalvar.Invoke()
                                                End If
                                            End Sub

            'Estiliza Button no Formulário
            EstilizaBotoes(_formulario)

            'Estiliza TextBox no Formulário
            EstilizaTextBoxes(_formulario)

        Catch ex As Exception
            MsgBox("Erro na configuração do formulário." & vbCrLf & ex.Message, vbCritical + vbOKOnly, "Atenção")
        End Try

    End Sub

    Private Sub EstilizaTextBoxes(container As Control)
        For Each ctrl As Control In container.Controls
            If TypeOf ctrl Is TextBox Then
                Dim txt As TextBox = CType(ctrl, TextBox)
                If txt.Tag Is Nothing OrElse txt.Tag.ToString() <> "N" Then
                    txt.BorderStyle = BorderStyle.FixedSingle
                    txt.BackColor = Color.White
                    txt.ForeColor = Color.FromArgb(30, 30, 30)
                    txt.Font = New Font("Segoe UI", 9.75, FontStyle.Regular)
                    txt.Margin = New Padding(5)
                    txt.Height = 28
                End If
            End If

            ' 🔁 Recursivamente estiliza controles filhos
            If ctrl.HasChildren Then
                EstilizaTextBoxes(ctrl)
            End If
        Next
    End Sub

    Private Sub EstilizaBotoes(container As Control)
        For Each ctrl As Control In container.Controls
            If TypeOf ctrl Is Button Then
                Dim btn As Button = CType(ctrl, Button)
                If btn.Tag Is Nothing OrElse btn.Tag.ToString() <> "N" Then
                    btn.BackColor = Color.FromArgb(0, 120, 215)
                    btn.ForeColor = Color.White
                    btn.FlatStyle = FlatStyle.Flat
                    btn.Font = New Font("Segoe UI", 9.75, FontStyle.Regular)
                    btn.FlatAppearance.BorderSize = 0
                    btn.Height = 35
                    btn.Width = 120
                End If
            End If

            ' 🔁 Se o controle tiver filhos, chama recursivamente
            If ctrl.HasChildren Then
                EstilizaBotoes(ctrl)
            End If
        Next
    End Sub


End Class
