<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UsuariosSelecao
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
        Me.GridUsuarios = New System.Windows.Forms.DataGridView()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.btnExcluir = New System.Windows.Forms.Button()
        Me.btnAlterar = New System.Windows.Forms.Button()
        Me.btnNovo = New System.Windows.Forms.Button()
        CType(Me.GridUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GridUsuarios
        '
        Me.GridUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridUsuarios.Location = New System.Drawing.Point(12, 12)
        Me.GridUsuarios.Name = "GridUsuarios"
        Me.GridUsuarios.Size = New System.Drawing.Size(492, 244)
        Me.GridUsuarios.TabIndex = 17
        '
        'btnImprimir
        '
        Me.btnImprimir.Location = New System.Drawing.Point(317, 272)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(135, 35)
        Me.btnImprimir.TabIndex = 22
        Me.btnImprimir.Text = "Imprimir (Ctrl + P)"
        Me.btnImprimir.UseVisualStyleBackColor = True
        '
        'btnExcluir
        '
        Me.btnExcluir.Location = New System.Drawing.Point(216, 272)
        Me.btnExcluir.Name = "btnExcluir"
        Me.btnExcluir.Size = New System.Drawing.Size(95, 35)
        Me.btnExcluir.TabIndex = 21
        Me.btnExcluir.Text = "Excluir (F4)"
        Me.btnExcluir.UseVisualStyleBackColor = True
        '
        'btnAlterar
        '
        Me.btnAlterar.Location = New System.Drawing.Point(115, 272)
        Me.btnAlterar.Name = "btnAlterar"
        Me.btnAlterar.Size = New System.Drawing.Size(95, 35)
        Me.btnAlterar.TabIndex = 19
        Me.btnAlterar.Text = "A&lterar (F3)"
        Me.btnAlterar.UseVisualStyleBackColor = True
        '
        'btnNovo
        '
        Me.btnNovo.Location = New System.Drawing.Point(14, 272)
        Me.btnNovo.Name = "btnNovo"
        Me.btnNovo.Size = New System.Drawing.Size(95, 35)
        Me.btnNovo.TabIndex = 18
        Me.btnNovo.Text = "&Novo (F2)"
        Me.btnNovo.UseVisualStyleBackColor = True
        '
        'UsuariosSelecao
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(517, 318)
        Me.Controls.Add(Me.GridUsuarios)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.btnExcluir)
        Me.Controls.Add(Me.btnAlterar)
        Me.Controls.Add(Me.btnNovo)
        Me.Name = "UsuariosSelecao"
        Me.Text = "Usuários"
        CType(Me.GridUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GridUsuarios As DataGridView
    Friend WithEvents btnImprimir As Button
    Friend WithEvents btnExcluir As Button
    Friend WithEvents btnAlterar As Button
    Friend WithEvents btnNovo As Button
End Class
