Public Class ProdutosSelecao
    Private Sub ProdutosSelecao_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim form As New clsGeForm(Me, True, True, True)
        form.ConfiguraMe()
    End Sub
End Class