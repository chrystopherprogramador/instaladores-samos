<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SiglaSelecao
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
        Me.txtBusca = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GridSigla = New System.Windows.Forms.DataGridView()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.btnAlterar = New System.Windows.Forms.Button()
        Me.btnNovo = New System.Windows.Forms.Button()
        CType(Me.GridSigla, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtBusca
        '
        Me.txtBusca.Location = New System.Drawing.Point(75, 16)
        Me.txtBusca.Name = "txtBusca"
        Me.txtBusca.Size = New System.Drawing.Size(292, 20)
        Me.txtBusca.TabIndex = 15
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Descrição"
        '
        'GridSigla
        '
        Me.GridSigla.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridSigla.Location = New System.Drawing.Point(12, 61)
        Me.GridSigla.Name = "GridSigla"
        Me.GridSigla.Size = New System.Drawing.Size(492, 198)
        Me.GridSigla.TabIndex = 17
        '
        'btnImprimir
        '
        Me.btnImprimir.Location = New System.Drawing.Point(216, 275)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(135, 35)
        Me.btnImprimir.TabIndex = 22
        Me.btnImprimir.Text = "Imprimir (Ctrl + P)"
        Me.btnImprimir.UseVisualStyleBackColor = True
        '
        'btnAlterar
        '
        Me.btnAlterar.Location = New System.Drawing.Point(115, 275)
        Me.btnAlterar.Name = "btnAlterar"
        Me.btnAlterar.Size = New System.Drawing.Size(95, 35)
        Me.btnAlterar.TabIndex = 19
        Me.btnAlterar.Text = "A&lterar (F3)"
        Me.btnAlterar.UseVisualStyleBackColor = True
        '
        'btnNovo
        '
        Me.btnNovo.Location = New System.Drawing.Point(14, 275)
        Me.btnNovo.Name = "btnNovo"
        Me.btnNovo.Size = New System.Drawing.Size(95, 35)
        Me.btnNovo.TabIndex = 18
        Me.btnNovo.Text = "&Novo (F2)"
        Me.btnNovo.UseVisualStyleBackColor = True
        '
        'SiglaSelecao
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(515, 321)
        Me.Controls.Add(Me.txtBusca)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GridSigla)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.btnAlterar)
        Me.Controls.Add(Me.btnNovo)
        Me.Name = "SiglaSelecao"
        Me.Text = "Categoria de Suítes"
        CType(Me.GridSigla, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtBusca As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GridSigla As DataGridView
    Friend WithEvents btnImprimir As Button
    Friend WithEvents btnAlterar As Button
    Friend WithEvents btnNovo As Button
End Class
