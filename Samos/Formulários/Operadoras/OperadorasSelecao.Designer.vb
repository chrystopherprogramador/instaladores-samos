<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OperadorasSelecao
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
        Me.GridOperadoras = New System.Windows.Forms.DataGridView()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.btnExcluir = New System.Windows.Forms.Button()
        Me.btnAlterar = New System.Windows.Forms.Button()
        Me.btnNovo = New System.Windows.Forms.Button()
        CType(Me.GridOperadoras, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtBusca
        '
        Me.txtBusca.Location = New System.Drawing.Point(75, 9)
        Me.txtBusca.Name = "txtBusca"
        Me.txtBusca.Size = New System.Drawing.Size(292, 20)
        Me.txtBusca.TabIndex = 22
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 26
        Me.Label1.Text = "Descrição"
        '
        'GridOperadoras
        '
        Me.GridOperadoras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridOperadoras.Location = New System.Drawing.Point(12, 54)
        Me.GridOperadoras.Name = "GridOperadoras"
        Me.GridOperadoras.Size = New System.Drawing.Size(492, 198)
        Me.GridOperadoras.TabIndex = 23
        '
        'btnImprimir
        '
        Me.btnImprimir.Location = New System.Drawing.Point(317, 268)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(135, 35)
        Me.btnImprimir.TabIndex = 28
        Me.btnImprimir.Text = "Imprimir (Ctrl + P)"
        Me.btnImprimir.UseVisualStyleBackColor = True
        '
        'btnExcluir
        '
        Me.btnExcluir.Location = New System.Drawing.Point(216, 268)
        Me.btnExcluir.Name = "btnExcluir"
        Me.btnExcluir.Size = New System.Drawing.Size(95, 35)
        Me.btnExcluir.TabIndex = 27
        Me.btnExcluir.Text = "Excluir (F4)"
        Me.btnExcluir.UseVisualStyleBackColor = True
        '
        'btnAlterar
        '
        Me.btnAlterar.Location = New System.Drawing.Point(115, 268)
        Me.btnAlterar.Name = "btnAlterar"
        Me.btnAlterar.Size = New System.Drawing.Size(95, 35)
        Me.btnAlterar.TabIndex = 25
        Me.btnAlterar.Text = "A&lterar (F3)"
        Me.btnAlterar.UseVisualStyleBackColor = True
        '
        'btnNovo
        '
        Me.btnNovo.Location = New System.Drawing.Point(14, 268)
        Me.btnNovo.Name = "btnNovo"
        Me.btnNovo.Size = New System.Drawing.Size(95, 35)
        Me.btnNovo.TabIndex = 24
        Me.btnNovo.Text = "&Novo (F2)"
        Me.btnNovo.UseVisualStyleBackColor = True
        '
        'OperadorasSelecao
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(517, 313)
        Me.Controls.Add(Me.txtBusca)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GridOperadoras)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.btnExcluir)
        Me.Controls.Add(Me.btnAlterar)
        Me.Controls.Add(Me.btnNovo)
        Me.Name = "OperadorasSelecao"
        Me.Text = "Operadoras de Cartão"
        CType(Me.GridOperadoras, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtBusca As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GridOperadoras As DataGridView
    Friend WithEvents btnImprimir As Button
    Friend WithEvents btnExcluir As Button
    Friend WithEvents btnAlterar As Button
    Friend WithEvents btnNovo As Button
End Class
