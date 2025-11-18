<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProdutosSelecao
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
        Me.GridProduto = New System.Windows.Forms.DataGridView()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.btnExcluir = New System.Windows.Forms.Button()
        Me.btnAlterar = New System.Windows.Forms.Button()
        Me.btnNovo = New System.Windows.Forms.Button()
        Me.btnVincular = New System.Windows.Forms.Button()
        CType(Me.GridProduto, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtBusca
        '
        Me.txtBusca.Location = New System.Drawing.Point(75, 18)
        Me.txtBusca.Name = "txtBusca"
        Me.txtBusca.Size = New System.Drawing.Size(292, 20)
        Me.txtBusca.TabIndex = 15
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Descrição"
        '
        'GridProduto
        '
        Me.GridProduto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridProduto.Location = New System.Drawing.Point(12, 63)
        Me.GridProduto.Name = "GridProduto"
        Me.GridProduto.Size = New System.Drawing.Size(776, 334)
        Me.GridProduto.TabIndex = 17
        '
        'btnImprimir
        '
        Me.btnImprimir.Location = New System.Drawing.Point(453, 403)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(135, 35)
        Me.btnImprimir.TabIndex = 22
        Me.btnImprimir.Text = "Imprimir (Ctrl + P)"
        Me.btnImprimir.UseVisualStyleBackColor = True
        '
        'btnExcluir
        '
        Me.btnExcluir.Location = New System.Drawing.Point(214, 403)
        Me.btnExcluir.Name = "btnExcluir"
        Me.btnExcluir.Size = New System.Drawing.Size(95, 35)
        Me.btnExcluir.TabIndex = 21
        Me.btnExcluir.Text = "Excluir (F4)"
        Me.btnExcluir.UseVisualStyleBackColor = True
        '
        'btnAlterar
        '
        Me.btnAlterar.Location = New System.Drawing.Point(113, 403)
        Me.btnAlterar.Name = "btnAlterar"
        Me.btnAlterar.Size = New System.Drawing.Size(95, 35)
        Me.btnAlterar.TabIndex = 19
        Me.btnAlterar.Text = "A&lterar (F3)"
        Me.btnAlterar.UseVisualStyleBackColor = True
        '
        'btnNovo
        '
        Me.btnNovo.Location = New System.Drawing.Point(12, 403)
        Me.btnNovo.Name = "btnNovo"
        Me.btnNovo.Size = New System.Drawing.Size(95, 35)
        Me.btnNovo.TabIndex = 18
        Me.btnNovo.Text = "&Novo (F2)"
        Me.btnNovo.UseVisualStyleBackColor = True
        '
        'btnVincular
        '
        Me.btnVincular.Location = New System.Drawing.Point(315, 403)
        Me.btnVincular.Name = "btnVincular"
        Me.btnVincular.Size = New System.Drawing.Size(132, 35)
        Me.btnVincular.TabIndex = 23
        Me.btnVincular.Text = "Vincula Suíte (F1)"
        Me.btnVincular.UseVisualStyleBackColor = True
        '
        'ProdutosSelecao
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnVincular)
        Me.Controls.Add(Me.txtBusca)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GridProduto)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.btnExcluir)
        Me.Controls.Add(Me.btnAlterar)
        Me.Controls.Add(Me.btnNovo)
        Me.Name = "ProdutosSelecao"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Produtos"
        CType(Me.GridProduto, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtBusca As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GridProduto As DataGridView
    Friend WithEvents btnImprimir As Button
    Friend WithEvents btnExcluir As Button
    Friend WithEvents btnAlterar As Button
    Friend WithEvents btnNovo As Button
    Friend WithEvents btnVincular As Button
End Class
