Public Class MDI
    Private Sub MDI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim config As New clsGeForm(Me, True, True, True)
        config.ConfiguraMe()
    End Sub

    Private Sub ProdToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProdToolStripMenuItem.Click

    End Sub
End Class
