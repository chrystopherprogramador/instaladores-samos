Public Class clsGeGeneral


    Public Shared Sub OpenForm(Of T As {Form, New})()
        Try
            Using frm As New T()
                frm.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show("Erro ao abrir formulário: " & ex.Message,
                            "Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function ListarImpressorasInstaladas() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("Nome", GetType(String))
        dt.Columns.Add("Padrão", GetType(Boolean))
        dt.Columns.Add("Status", GetType(String))

        Try
            For Each impressora As String In Printing.PrinterSettings.InstalledPrinters
                Dim ps As New Printing.PrinterSettings With {.PrinterName = impressora}
                Dim linha = dt.NewRow()
                linha("Nome") = impressora
                linha("Padrão") = ps.IsDefaultPrinter
                linha("Status") = If(ps.IsValid, "Disponível", "Indisponível")
                dt.Rows.Add(linha)
            Next
        Catch ex As Exception
            MsgBox("Erro ao listar impressoras: " & ex.Message, MsgBoxStyle.Critical)
        End Try

        Return dt
    End Function

    Public Shared Function VB6Color(vb6ColorValue As Object) As Color
        Try
            If vb6ColorValue Is DBNull.Value OrElse vb6ColorValue Is Nothing Then Return Color.Empty

            Dim s As String = vb6ColorValue.ToString().Trim()
            If s = String.Empty Then Return Color.Empty

            ' Aceita formatos: inteiro decimal, &HHHHHH, 0xHHHHHH, #RRGGBB ou "R,G,B"
            If s.StartsWith("&H", StringComparison.OrdinalIgnoreCase) Then
                s = s.Substring(2)
            ElseIf s.StartsWith("0x", StringComparison.OrdinalIgnoreCase) Then
                s = s.Substring(2)
            ElseIf s.StartsWith("#") Then
                ' #RRGGBB
                Dim hex = s.Substring(1)
                If hex.Length = 6 Then
                    Dim r = Convert.ToInt32(hex.Substring(0, 2), 16)
                    Dim g = Convert.ToInt32(hex.Substring(2, 2), 16)
                    Dim b = Convert.ToInt32(hex.Substring(4, 2), 16)
                    Return Color.FromArgb(r, g, b)
                End If
            ElseIf s.Contains(",") Then
                Dim parts = s.Split(","c)
                If parts.Length >= 3 Then
                    Dim r, g, b As Integer
                    If Integer.TryParse(parts(0), r) AndAlso Integer.TryParse(parts(1), g) AndAlso Integer.TryParse(parts(2), b) Then
                        Return Color.FromArgb(r, g, b)
                    End If
                End If
            End If

            Dim valor As Integer
            ' tenta parsear hex ou decimal
            If Integer.TryParse(s, valor) Then
                Dim r = valor And &HFF
                Dim g = (valor >> 8) And &HFF
                Dim b = (valor >> 16) And &HFF
                Return Color.FromArgb(r, g, b)
            End If

            ' tentativa de parse hex explícita
            If Integer.TryParse(s, Globalization.NumberStyles.HexNumber, Globalization.CultureInfo.InvariantCulture, valor) Then
                Dim r = valor And &HFF
                Dim g = (valor >> 8) And &HFF
                Dim b = (valor >> 16) And &HFF
                Return Color.FromArgb(r, g, b)
            End If
        Catch ex As Exception
            Diagnostics.Debug.WriteLine("[VB6Color] Parse falhou: " & ex.Message)
        End Try

        Return Color.Empty
    End Function

    Public Shared Function CripSenhaUsuario(Senha As String) As String
        Dim X As Integer
        Dim Y As Integer

        CripSenhaUsuario = ""

        If Senha <> "" Then
            X = 1
            Y = Len(Senha)
            For X = 1 To Y
                CripSenhaUsuario = CripSenhaUsuario & Format(Asc(Mid(Senha, X, 1)), "000")
            Next
        End If
    End Function
    Public Shared Function DescSenhaUsuario(Senha As String) As String
        Dim X As Integer
        Dim Y As Integer

        DescSenhaUsuario = ""

        If Senha <> "" Then
            X = 1
            Y = Len(DescSenhaUsuario)
            For X = 1 To Y Step 3
                DescSenhaUsuario = DescSenhaUsuario & Chr(Mid(Senha, X, 3))
            Next
        End If
    End Function
End Class
