<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Operadoras
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
        Me.btnGravar = New System.Windows.Forms.Button()
        Me.txtNome = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCodNfce = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chDebito = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'btnGravar
        '
        Me.btnGravar.Location = New System.Drawing.Point(285, 157)
        Me.btnGravar.Name = "btnGravar"
        Me.btnGravar.Size = New System.Drawing.Size(123, 34)
        Me.btnGravar.TabIndex = 4
        Me.btnGravar.Text = "Gravar (Ctrl + S)"
        Me.btnGravar.UseVisualStyleBackColor = True
        '
        'txtNome
        '
        Me.txtNome.Location = New System.Drawing.Point(95, 80)
        Me.txtNome.Name = "txtNome"
        Me.txtNome.Size = New System.Drawing.Size(191, 20)
        Me.txtNome.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(416, 41)
        Me.Label4.TabIndex = 28
        Me.Label4.Text = "Operadoras de Cartão"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCodigo
        '
        Me.txtCodigo.Location = New System.Drawing.Point(95, 54)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(70, 20)
        Me.txtCodigo.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 27
        Me.Label2.Text = "Nome"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 26
        Me.Label1.Text = "Código"
        '
        'txtCodNfce
        '
        Me.txtCodNfce.Location = New System.Drawing.Point(95, 106)
        Me.txtCodNfce.Name = "txtCodNfce"
        Me.txtCodNfce.Size = New System.Drawing.Size(70, 20)
        Me.txtCodNfce.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 109)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 31
        Me.Label5.Text = "Cod. NFCE"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(171, 109)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(237, 13)
        Me.Label6.TabIndex = 32
        Me.Label6.Text = "01 - Visa  02 - Mastercard  03 - Amex  99 - outros"
        '
        'chDebito
        '
        Me.chDebito.AutoSize = True
        Me.chDebito.Location = New System.Drawing.Point(95, 132)
        Me.chDebito.Name = "chDebito"
        Me.chDebito.Size = New System.Drawing.Size(109, 17)
        Me.chDebito.TabIndex = 3
        Me.chDebito.Text = "Cartão tipo débito"
        Me.chDebito.UseVisualStyleBackColor = True
        '
        'Operadoras
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(416, 212)
        Me.Controls.Add(Me.chDebito)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtCodNfce)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnGravar)
        Me.Controls.Add(Me.txtNome)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtCodigo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Operadoras"
        Me.Text = "Operadoras de Cartão"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnGravar As Button
    Friend WithEvents txtNome As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtCodNfce As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents chDebito As CheckBox
End Class
