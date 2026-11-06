<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Ocupação
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
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.PtFotoPlaca = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.disMovto = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblPlaca = New System.Windows.Forms.Label()
        Me.txtPlaca = New System.Windows.Forms.TextBox()
        CType(Me.PtFotoPlaca, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitulo
        '
        Me.lblTitulo.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.lblTitulo.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitulo.ForeColor = System.Drawing.Color.White
        Me.lblTitulo.Location = New System.Drawing.Point(0, 0)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(800, 41)
        Me.lblTitulo.TabIndex = 16
        Me.lblTitulo.Text = "Ocupação de Suíte"
        Me.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PtFotoPlaca
        '
        Me.PtFotoPlaca.Location = New System.Drawing.Point(593, 56)
        Me.PtFotoPlaca.Name = "PtFotoPlaca"
        Me.PtFotoPlaca.Size = New System.Drawing.Size(195, 187)
        Me.PtFotoPlaca.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PtFotoPlaca.TabIndex = 17
        Me.PtFotoPlaca.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 25)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Movto"
        '
        'disMovto
        '
        Me.disMovto.AutoSize = True
        Me.disMovto.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.disMovto.Location = New System.Drawing.Point(13, 81)
        Me.disMovto.Name = "disMovto"
        Me.disMovto.Size = New System.Drawing.Size(89, 20)
        Me.disMovto.TabIndex = 19
        Me.disMovto.Text = "00000000"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(609, 258)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(179, 38)
        Me.Button1.TabIndex = 20
        Me.Button1.Text = "Capturar OCR"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lblPlaca
        '
        Me.lblPlaca.AutoSize = True
        Me.lblPlaca.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlaca.Location = New System.Drawing.Point(606, 305)
        Me.lblPlaca.Name = "lblPlaca"
        Me.lblPlaca.Size = New System.Drawing.Size(48, 19)
        Me.lblPlaca.TabIndex = 21
        Me.lblPlaca.Text = "Placa"
        '
        'txtPlaca
        '
        Me.txtPlaca.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPlaca.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPlaca.Location = New System.Drawing.Point(609, 327)
        Me.txtPlaca.MaxLength = 8
        Me.txtPlaca.Name = "txtPlaca"
        Me.txtPlaca.Size = New System.Drawing.Size(179, 39)
        Me.txtPlaca.TabIndex = 22
        Me.txtPlaca.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Ocupação
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.txtPlaca)
        Me.Controls.Add(Me.lblPlaca)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.disMovto)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PtFotoPlaca)
        Me.Controls.Add(Me.lblTitulo)
        Me.Name = "Ocupação"
        Me.Text = "Ocupação"
        CType(Me.PtFotoPlaca, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitulo As Label
    Friend WithEvents PtFotoPlaca As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents disMovto As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents lblPlaca As Label
    Friend WithEvents txtPlaca As TextBox
End Class
