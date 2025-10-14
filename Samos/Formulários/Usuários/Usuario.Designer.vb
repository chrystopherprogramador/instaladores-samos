<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Usuario
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.txtNome = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNivel = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDisNivel = New System.Windows.Forms.TextBox()
        Me.chSup = New System.Windows.Forms.CheckBox()
        Me.chInativo = New System.Windows.Forms.CheckBox()
        Me.btnGravar = New System.Windows.Forms.Button()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(422, 41)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Usuário"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Código"
        '
        'txtCodigo
        '
        Me.txtCodigo.Location = New System.Drawing.Point(12, 67)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(85, 20)
        Me.txtCodigo.TabIndex = 0
        Me.txtCodigo.Tag = "NU"
        '
        'txtNome
        '
        Me.txtNome.Location = New System.Drawing.Point(103, 67)
        Me.txtNome.Name = "txtNome"
        Me.txtNome.Size = New System.Drawing.Size(307, 20)
        Me.txtNome.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(103, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Nome"
        '
        'txtNivel
        '
        Me.txtNivel.Location = New System.Drawing.Point(12, 106)
        Me.txtNivel.Name = "txtNivel"
        Me.txtNivel.Size = New System.Drawing.Size(85, 20)
        Me.txtNivel.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 90)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 13)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Nível de Acesso"
        '
        'txtDisNivel
        '
        Me.txtDisNivel.Location = New System.Drawing.Point(188, 106)
        Me.txtDisNivel.Name = "txtDisNivel"
        Me.txtDisNivel.Size = New System.Drawing.Size(222, 20)
        Me.txtDisNivel.TabIndex = 22
        '
        'chSup
        '
        Me.chSup.AutoSize = True
        Me.chSup.Location = New System.Drawing.Point(12, 132)
        Me.chSup.Name = "chSup"
        Me.chSup.Size = New System.Drawing.Size(170, 17)
        Me.chSup.TabIndex = 3
        Me.chSup.Text = "Usuário Supervisor do Sistema"
        Me.chSup.UseVisualStyleBackColor = True
        '
        'chInativo
        '
        Me.chInativo.AutoSize = True
        Me.chInativo.Location = New System.Drawing.Point(188, 132)
        Me.chInativo.Name = "chInativo"
        Me.chInativo.Size = New System.Drawing.Size(58, 17)
        Me.chInativo.TabIndex = 4
        Me.chInativo.Text = "Inativo"
        Me.chInativo.UseVisualStyleBackColor = True
        '
        'btnGravar
        '
        Me.btnGravar.Location = New System.Drawing.Point(304, 153)
        Me.btnGravar.Name = "btnGravar"
        Me.btnGravar.Size = New System.Drawing.Size(106, 39)
        Me.btnGravar.TabIndex = 5
        Me.btnGravar.Text = "&Gravar (Ctrl + S)"
        Me.btnGravar.UseVisualStyleBackColor = True
        '
        'btnBuscar
        '
        Me.btnBuscar.Location = New System.Drawing.Point(103, 106)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(79, 20)
        Me.btnBuscar.TabIndex = 26
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'Usuario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(422, 204)
        Me.Controls.Add(Me.btnBuscar)
        Me.Controls.Add(Me.btnGravar)
        Me.Controls.Add(Me.chInativo)
        Me.Controls.Add(Me.chSup)
        Me.Controls.Add(Me.txtDisNivel)
        Me.Controls.Add(Me.txtNivel)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtNome)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtCodigo)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Name = "Usuario"
        Me.Text = "Usuário"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents txtNome As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtNivel As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtDisNivel As TextBox
    Friend WithEvents chSup As CheckBox
    Friend WithEvents chInativo As CheckBox
    Friend WithEvents btnGravar As Button
    Friend WithEvents btnBuscar As Button
End Class
