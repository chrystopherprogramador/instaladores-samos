<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfBanco
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtServidor = New System.Windows.Forms.TextBox()
        Me.txtusuario = New System.Windows.Forms.TextBox()
        Me.txtSenha = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtBanco = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtPorta = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnGravar = New System.Windows.Forms.Button()
        Me.btnTestar = New System.Windows.Forms.Button()
        Me.btnCriar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Servidor:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Usuário:"
        '
        'txtServidor
        '
        Me.txtServidor.Location = New System.Drawing.Point(98, 6)
        Me.txtServidor.Name = "txtServidor"
        Me.txtServidor.Size = New System.Drawing.Size(152, 20)
        Me.txtServidor.TabIndex = 0
        Me.txtServidor.Tag = "P"
        '
        'txtusuario
        '
        Me.txtusuario.Location = New System.Drawing.Point(98, 32)
        Me.txtusuario.Name = "txtusuario"
        Me.txtusuario.Size = New System.Drawing.Size(152, 20)
        Me.txtusuario.TabIndex = 1
        Me.txtusuario.Tag = "P"
        '
        'txtSenha
        '
        Me.txtSenha.Location = New System.Drawing.Point(98, 58)
        Me.txtSenha.Name = "txtSenha"
        Me.txtSenha.Size = New System.Drawing.Size(152, 20)
        Me.txtSenha.TabIndex = 2
        Me.txtSenha.Tag = "P"
        Me.txtSenha.UseSystemPasswordChar = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Senha:"
        '
        'txtBanco
        '
        Me.txtBanco.Location = New System.Drawing.Point(98, 84)
        Me.txtBanco.Name = "txtBanco"
        Me.txtBanco.Size = New System.Drawing.Size(152, 20)
        Me.txtBanco.TabIndex = 3
        Me.txtBanco.Tag = "P"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Banco:"
        '
        'txtPorta
        '
        Me.txtPorta.Location = New System.Drawing.Point(98, 110)
        Me.txtPorta.Name = "txtPorta"
        Me.txtPorta.Size = New System.Drawing.Size(69, 20)
        Me.txtPorta.TabIndex = 4
        Me.txtPorta.Text = "3306"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 113)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Porta:"
        '
        'btnGravar
        '
        Me.btnGravar.Location = New System.Drawing.Point(292, 173)
        Me.btnGravar.Name = "btnGravar"
        Me.btnGravar.Size = New System.Drawing.Size(133, 30)
        Me.btnGravar.TabIndex = 6
        Me.btnGravar.Text = "&Gravar (Ctrl + S)"
        Me.btnGravar.UseVisualStyleBackColor = True
        '
        'btnTestar
        '
        Me.btnTestar.Location = New System.Drawing.Point(153, 173)
        Me.btnTestar.Name = "btnTestar"
        Me.btnTestar.Size = New System.Drawing.Size(133, 30)
        Me.btnTestar.TabIndex = 5
        Me.btnTestar.Text = "T&estar Conexão"
        Me.btnTestar.UseVisualStyleBackColor = True
        '
        'btnCriar
        '
        Me.btnCriar.Location = New System.Drawing.Point(12, 173)
        Me.btnCriar.Name = "btnCriar"
        Me.btnCriar.Size = New System.Drawing.Size(133, 30)
        Me.btnCriar.TabIndex = 7
        Me.btnCriar.Text = "Criar &Novo Banco"
        Me.btnCriar.UseVisualStyleBackColor = True
        '
        'ConfBanco
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(437, 215)
        Me.Controls.Add(Me.btnCriar)
        Me.Controls.Add(Me.btnTestar)
        Me.Controls.Add(Me.btnGravar)
        Me.Controls.Add(Me.txtPorta)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtBanco)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtSenha)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtusuario)
        Me.Controls.Add(Me.txtServidor)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label1)
        Me.Name = "ConfBanco"
        Me.Text = "Configuração Banco de dados"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtServidor As TextBox
    Friend WithEvents txtusuario As TextBox
    Friend WithEvents txtSenha As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtBanco As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtPorta As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents btnGravar As Button
    Friend WithEvents btnTestar As Button
    Friend WithEvents btnCriar As Button
End Class
