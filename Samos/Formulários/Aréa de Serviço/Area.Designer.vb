<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Area
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtDescricao = New System.Windows.Forms.TextBox()
        Me.cmbImpressora = New System.Windows.Forms.ComboBox()
        Me.btnGravar = New System.Windows.Forms.Button()
        Me.ProgressoAtualizaSaldo = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 62)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Código"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 88)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Descrição"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 114)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Impressora"
        '
        'txtCodigo
        '
        Me.txtCodigo.Location = New System.Drawing.Point(93, 59)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(70, 20)
        Me.txtCodigo.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(392, 41)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Área de Serviços"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDescricao
        '
        Me.txtDescricao.Location = New System.Drawing.Point(93, 85)
        Me.txtDescricao.Name = "txtDescricao"
        Me.txtDescricao.Size = New System.Drawing.Size(191, 20)
        Me.txtDescricao.TabIndex = 5
        '
        'cmbImpressora
        '
        Me.cmbImpressora.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbImpressora.FormattingEnabled = True
        Me.cmbImpressora.Location = New System.Drawing.Point(93, 111)
        Me.cmbImpressora.Name = "cmbImpressora"
        Me.cmbImpressora.Size = New System.Drawing.Size(191, 21)
        Me.cmbImpressora.TabIndex = 6
        '
        'btnGravar
        '
        Me.btnGravar.Location = New System.Drawing.Point(257, 147)
        Me.btnGravar.Name = "btnGravar"
        Me.btnGravar.Size = New System.Drawing.Size(123, 34)
        Me.btnGravar.TabIndex = 7
        Me.btnGravar.Text = "Gravar (Ctrl + S)"
        Me.btnGravar.UseVisualStyleBackColor = True
        '
        'ProgressoAtualizaSaldo
        '
        Me.ProgressoAtualizaSaldo.Location = New System.Drawing.Point(15, 187)
        Me.ProgressoAtualizaSaldo.Name = "ProgressoAtualizaSaldo"
        Me.ProgressoAtualizaSaldo.Size = New System.Drawing.Size(365, 25)
        Me.ProgressoAtualizaSaldo.TabIndex = 8
        Me.ProgressoAtualizaSaldo.Visible = False
        '
        'Area
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(392, 224)
        Me.Controls.Add(Me.ProgressoAtualizaSaldo)
        Me.Controls.Add(Me.btnGravar)
        Me.Controls.Add(Me.cmbImpressora)
        Me.Controls.Add(Me.txtDescricao)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtCodigo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Area"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Área de Serviços"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtDescricao As TextBox
    Friend WithEvents cmbImpressora As ComboBox
    Friend WithEvents btnGravar As Button
    Friend WithEvents ProgressoAtualizaSaldo As ProgressBar
End Class
