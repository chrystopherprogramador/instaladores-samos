Imports System.IO

Public Class RelatorioGenerico

    Public Property DataSource As DataTable
    Public Property Titulo As String

    Private Sub RelatorioGenerico_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim form As New clsGeForm(Me, True, False, False,,, Sub()
                                                                ExportarParaPDF(DataSource, Titulo)
                                                            End Sub, Sub()
                                                                         ExportarParaExcel(DataSource, Titulo)
                                                                     End Sub,, Sub()
                                                                                   ImprimirRelatorio(DataSource, Titulo)
                                                                               End Sub)
        form.ConfiguraMe()

    End Sub
    Private Sub ExportarParaPDF(dt As DataTable, titulo As String)
        Dim salvarDialog As New SaveFileDialog With {
        .Filter = "Arquivo PDF|*.pdf",
        .Title = "Salvar relatório como PDF",
        .FileName = "relatorio.pdf"
    }

        If salvarDialog.ShowDialog() = DialogResult.OK Then
            Dim doc As New iTextSharp.text.Document()
            iTextSharp.text.pdf.PdfWriter.GetInstance(doc, New FileStream(salvarDialog.FileName, FileMode.Create))
            doc.Open()

            doc.Add(New iTextSharp.text.Paragraph(titulo) With {.Alignment = iTextSharp.text.Element.ALIGN_CENTER})
            doc.Add(New iTextSharp.text.Paragraph(" "))

            Dim tabela As New iTextSharp.text.pdf.PdfPTable(dt.Columns.Count)

            For Each col As DataColumn In dt.Columns
                tabela.AddCell(New iTextSharp.text.Phrase(col.ColumnName))
            Next

            For Each row As DataRow In dt.Rows
                For Each item In row.ItemArray
                    tabela.AddCell(New iTextSharp.text.Phrase(item.ToString()))
                Next
            Next

            doc.Add(tabela)
            doc.Close()
            Process.Start(salvarDialog.FileName)
        End If
    End Sub
    Private Sub ExportarParaExcel(dt As DataTable, titulo As String)
        Dim salvarDialog As New SaveFileDialog With {
        .Filter = "Arquivo Excel|*.xlsx",
        .Title = "Salvar relatório como Excel",
        .FileName = "relatorio.xlsx"
    }

        If salvarDialog.ShowDialog() = DialogResult.OK Then
            Dim wb = New ClosedXML.Excel.XLWorkbook()
            Dim ws = wb.Worksheets.Add(titulo)

            For i = 0 To dt.Columns.Count - 1
                ws.Cell(1, i + 1).Value = dt.Columns(i).ColumnName
            Next

            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    ws.Cell(i + 2, j + 1).Value = dt.Rows(i)(j).ToString()
                Next
            Next

            wb.SaveAs(salvarDialog.FileName)
            Process.Start(salvarDialog.FileName)
        End If
    End Sub

    Public Sub ImprimirRelatorio(dt As DataTable, titulo As String)
        Dim doc As New Printing.PrintDocument()
        AddHandler doc.PrintPage, Sub(sender, e)
                                      Dim fonte As New Font("Segoe UI", 10)
                                      Dim y As Integer = 50

                                      ' Título
                                      e.Graphics.DrawString(titulo, New Font("Segoe UI", 12, FontStyle.Bold), Brushes.Black, 50, y)
                                      y += 40

                                      ' Cabeçalhos
                                      Dim cabecalho As String = String.Join(" | ", dt.Columns.Cast(Of DataColumn).Select(Function(c) c.ColumnName))
                                      e.Graphics.DrawString(cabecalho, fonte, Brushes.Black, 50, y)
                                      y += 30

                                      ' Dados
                                      For Each row As DataRow In dt.Rows
                                          Dim linha As String = String.Join(" | ", row.ItemArray.Select(Function(v) v.ToString()))
                                          e.Graphics.DrawString(linha, fonte, Brushes.Black, 50, y)
                                          y += 20
                                      Next
                                  End Sub

        Dim preview As New PrintPreviewDialog With {.Document = doc}
        preview.ShowDialog()
    End Sub
    Private Sub btnPDF_Click(sender As Object, e As EventArgs) Handles btnPDF.Click
        ExportarParaPDF(DataSource, Titulo)
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        ImprimirRelatorio(DataSource, Titulo)
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        ExportarParaExcel(DataSource, Titulo)
    End Sub
End Class