<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Carregando
    Inherits System.Windows.Forms.Form

    'Descartar substituições de formulário para limpar a lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Exigido pelo Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'OBSERVAÇÃO: o procedimento a seguir é exigido pelo Windows Form Designer
    'Pode ser modificado usando o Windows Form Designer.  
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Barra = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'Barra
        '
        Me.Barra.Location = New System.Drawing.Point(12, 52)
        Me.Barra.Name = "Barra"
        Me.Barra.Size = New System.Drawing.Size(633, 40)
        Me.Barra.TabIndex = 0
        '
        'Carregando
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(657, 104)
        Me.Controls.Add(Me.Barra)
        Me.Name = "Carregando"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Carregando"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Barra As ProgressBar
End Class
