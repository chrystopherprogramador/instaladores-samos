<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Sigla
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
        Me.chcBloqueiaTerminal = New System.Windows.Forms.CheckBox()
        Me.btnGravar = New System.Windows.Forms.Button()
        Me.txtNome = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtTxAdd = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtQtdPessoa = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chcEmissaoContrato = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'chcBloqueiaTerminal
        '
        Me.chcBloqueiaTerminal.AutoSize = True
        Me.chcBloqueiaTerminal.Location = New System.Drawing.Point(14, 131)
        Me.chcBloqueiaTerminal.Name = "chcBloqueiaTerminal"
        Me.chcBloqueiaTerminal.Size = New System.Drawing.Size(161, 17)
        Me.chcBloqueiaTerminal.TabIndex = 3
        Me.chcBloqueiaTerminal.Text = "Bloqueia Terminal de Contas"
        Me.chcBloqueiaTerminal.UseVisualStyleBackColor = True
        '
        'btnGravar
        '
        Me.btnGravar.Location = New System.Drawing.Point(233, 200)
        Me.btnGravar.Name = "btnGravar"
        Me.btnGravar.Size = New System.Drawing.Size(123, 34)
        Me.btnGravar.TabIndex = 5
        Me.btnGravar.Text = "Gravar (Ctrl + S)"
        Me.btnGravar.UseVisualStyleBackColor = True
        '
        'txtNome
        '
        Me.txtNome.Location = New System.Drawing.Point(131, 53)
        Me.txtNome.Name = "txtNome"
        Me.txtNome.Size = New System.Drawing.Size(191, 20)
        Me.txtNome.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(368, 41)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Categoria de Suítes"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Nome"
        '
        'txtTxAdd
        '
        Me.txtTxAdd.Location = New System.Drawing.Point(131, 79)
        Me.txtTxAdd.Name = "txtTxAdd"
        Me.txtTxAdd.Size = New System.Drawing.Size(96, 20)
        Me.txtTxAdd.TabIndex = 1
        Me.txtTxAdd.Tag = "MO"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 82)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(114, 13)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "Tx Adicional p/Pessoa"
        '
        'txtQtdPessoa
        '
        Me.txtQtdPessoa.Location = New System.Drawing.Point(131, 105)
        Me.txtQtdPessoa.Name = "txtQtdPessoa"
        Me.txtQtdPessoa.Size = New System.Drawing.Size(96, 20)
        Me.txtQtdPessoa.TabIndex = 2
        Me.txtQtdPessoa.Tag = "NU"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 108)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(82, 13)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "Qtd de Pessoas"
        '
        'chcEmissaoContrato
        '
        Me.chcEmissaoContrato.AutoSize = True
        Me.chcEmissaoContrato.Location = New System.Drawing.Point(14, 154)
        Me.chcEmissaoContrato.Name = "chcEmissaoContrato"
        Me.chcEmissaoContrato.Size = New System.Drawing.Size(183, 17)
        Me.chcEmissaoContrato.TabIndex = 4
        Me.chcEmissaoContrato.Text = "Emissão de Contrato de Locação"
        Me.chcEmissaoContrato.UseVisualStyleBackColor = True
        '
        'Sigla
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(368, 250)
        Me.Controls.Add(Me.chcEmissaoContrato)
        Me.Controls.Add(Me.txtQtdPessoa)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtTxAdd)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chcBloqueiaTerminal)
        Me.Controls.Add(Me.btnGravar)
        Me.Controls.Add(Me.txtNome)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Name = "Sigla"
        Me.Text = "Categoria de Suítes"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents chcBloqueiaTerminal As CheckBox
    Friend WithEvents btnGravar As Button
    Friend WithEvents txtNome As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtTxAdd As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtQtdPessoa As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents chcEmissaoContrato As CheckBox
End Class
