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
