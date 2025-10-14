<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Produtos
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
        Me.chServico = New System.Windows.Forms.CheckBox()
        Me.btnGravar = New System.Windows.Forms.Button()
        Me.txtNome = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtEan = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCategoria = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbLocalPadrao = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbCtrlEstoque = New System.Windows.Forms.ComboBox()
        Me.txtVenda = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbTributacao = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtAlicota = New System.Windows.Forms.TextBox()
        Me.txtCFOP = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.PtImagem = New System.Windows.Forms.PictureBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtNCM = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtCEst = New System.Windows.Forms.TextBox()
        Me.txtCusto = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtUnd = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtDigMax = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtEstMaximo = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtEstMinimo = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtQtdFribobar = New System.Windows.Forms.TextBox()
        Me.chOpcional = New System.Windows.Forms.CheckBox()
        Me.chRetornavel = New System.Windows.Forms.CheckBox()
        Me.chBrinde = New System.Windows.Forms.CheckBox()
        Me.chConsumacao = New System.Windows.Forms.CheckBox()
        Me.chCancela = New System.Windows.Forms.CheckBox()
        Me.chCardapio = New System.Windows.Forms.CheckBox()
        Me.chInativo = New System.Windows.Forms.CheckBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.grpBasic = New System.Windows.Forms.GroupBox()
        Me.lblCategoria = New System.Windows.Forms.Label()
        Me.grpTax = New System.Windows.Forms.GroupBox()
        Me.grpInventory = New System.Windows.Forms.GroupBox()
        Me.grpIdentification = New System.Windows.Forms.GroupBox()
        Me.grpFlags = New System.Windows.Forms.GroupBox()
        Me.btnBuscaNCM = New System.Windows.Forms.Button()
        CType(Me.PtImagem, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpBasic.SuspendLayout()
        Me.grpTax.SuspendLayout()
        Me.grpInventory.SuspendLayout()
        Me.grpIdentification.SuspendLayout()
        Me.grpFlags.SuspendLayout()
        Me.SuspendLayout()
        '
        'chServico
        '
        Me.chServico.AutoSize = True
        Me.chServico.Location = New System.Drawing.Point(10, 20)
        Me.chServico.Name = "chServico"
        Me.chServico.Size = New System.Drawing.Size(107, 17)
        Me.chServico.TabIndex = 0
        Me.chServico.Text = "Serviço Prestado"
        Me.chServico.UseVisualStyleBackColor = True
        '
        'btnGravar
        '
        Me.btnGravar.Location = New System.Drawing.Point(698, 405)
        Me.btnGravar.Name = "btnGravar"
        Me.btnGravar.Size = New System.Drawing.Size(123, 34)
        Me.btnGravar.TabIndex = 5
        Me.btnGravar.Text = "Gravar (Ctrl + S)"
        Me.btnGravar.UseVisualStyleBackColor = True
        '
        'txtNome
        '
        Me.txtNome.Location = New System.Drawing.Point(220, 20)
        Me.txtNome.Name = "txtNome"
        Me.txtNome.Size = New System.Drawing.Size(240, 20)
        Me.txtNome.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(824, 41)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Produtos"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtCodigo
        '
        Me.txtCodigo.Location = New System.Drawing.Point(80, 20)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(70, 20)
        Me.txtCodigo.TabIndex = 0
        Me.txtCodigo.Tag = "NU"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(160, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Descrição"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Código"
        '
        'txtEan
        '
        Me.txtEan.Location = New System.Drawing.Point(55, 49)
        Me.txtEan.Name = "txtEan"
        Me.txtEan.Size = New System.Drawing.Size(130, 20)
        Me.txtEan.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 52)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "EAN"
        '
        'txtCategoria
        '
        Me.txtCategoria.Location = New System.Drawing.Point(260, 49)
        Me.txtCategoria.Name = "txtCategoria"
        Me.txtCategoria.Size = New System.Drawing.Size(58, 20)
        Me.txtCategoria.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(200, 52)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(52, 13)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "Categoria"
        '
        'btnBuscar
        '
        Me.btnBuscar.Location = New System.Drawing.Point(324, 48)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(92, 20)
        Me.btnBuscar.TabIndex = 25
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 22)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 13)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "Local Padrão"
        '
        'cmbLocalPadrao
        '
        Me.cmbLocalPadrao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLocalPadrao.FormattingEnabled = True
        Me.cmbLocalPadrao.Items.AddRange(New Object() {"Frigobar", "Bar", "Cozinha", "Produto p/ preparo", "Outros"})
        Me.cmbLocalPadrao.Location = New System.Drawing.Point(90, 19)
        Me.cmbLocalPadrao.Name = "cmbLocalPadrao"
        Me.cmbLocalPadrao.Size = New System.Drawing.Size(112, 21)
        Me.cmbLocalPadrao.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(210, 23)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(64, 13)
        Me.Label7.TabIndex = 28
        Me.Label7.Text = "Ctrl Estoque"
        '
        'cmbCtrlEstoque
        '
        Me.cmbCtrlEstoque.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCtrlEstoque.FormattingEnabled = True
        Me.cmbCtrlEstoque.Items.AddRange(New Object() {"Simple", "Composto"})
        Me.cmbCtrlEstoque.Location = New System.Drawing.Point(280, 19)
        Me.cmbCtrlEstoque.Name = "cmbCtrlEstoque"
        Me.cmbCtrlEstoque.Size = New System.Drawing.Size(74, 21)
        Me.cmbCtrlEstoque.TabIndex = 1
        '
        'txtVenda
        '
        Me.txtVenda.Location = New System.Drawing.Point(60, 19)
        Me.txtVenda.Name = "txtVenda"
        Me.txtVenda.Size = New System.Drawing.Size(74, 20)
        Me.txtVenda.TabIndex = 0
        Me.txtVenda.Tag = "MO"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(10, 22)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(38, 13)
        Me.Label8.TabIndex = 31
        Me.Label8.Text = "Venda"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(150, 22)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(82, 13)
        Me.Label9.TabIndex = 33
        Me.Label9.Text = "Tipo Tributação"
        '
        'cmbTributacao
        '
        Me.cmbTributacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTributacao.FormattingEnabled = True
        Me.cmbTributacao.Items.AddRange(New Object() {"Tributado", "Subs. Trib", "Isento", "Outros"})
        Me.cmbTributacao.Location = New System.Drawing.Point(240, 19)
        Me.cmbTributacao.Name = "cmbTributacao"
        Me.cmbTributacao.Size = New System.Drawing.Size(112, 21)
        Me.cmbTributacao.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(360, 22)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(39, 13)
        Me.Label10.TabIndex = 35
        Me.Label10.Text = "Alicota"
        '
        'txtAlicota
        '
        Me.txtAlicota.Location = New System.Drawing.Point(405, 19)
        Me.txtAlicota.Name = "txtAlicota"
        Me.txtAlicota.Size = New System.Drawing.Size(58, 20)
        Me.txtAlicota.TabIndex = 2
        Me.txtAlicota.Tag = "MO"
        '
        'txtCFOP
        '
        Me.txtCFOP.Location = New System.Drawing.Point(290, 47)
        Me.txtCFOP.Name = "txtCFOP"
        Me.txtCFOP.Size = New System.Drawing.Size(74, 20)
        Me.txtCFOP.TabIndex = 5
        Me.txtCFOP.Tag = "NU"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(250, 50)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(35, 13)
        Me.Label11.TabIndex = 38
        Me.Label11.Text = "CFOP"
        '
        'PtImagem
        '
        Me.PtImagem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PtImagem.Location = New System.Drawing.Point(710, 47)
        Me.PtImagem.Name = "PtImagem"
        Me.PtImagem.Size = New System.Drawing.Size(111, 100)
        Me.PtImagem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PtImagem.TabIndex = 39
        Me.PtImagem.TabStop = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(10, 22)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(31, 13)
        Me.Label12.TabIndex = 41
        Me.Label12.Text = "NCM"
        '
        'txtNCM
        '
        Me.txtNCM.Location = New System.Drawing.Point(50, 19)
        Me.txtNCM.Name = "txtNCM"
        Me.txtNCM.Size = New System.Drawing.Size(120, 20)
        Me.txtNCM.TabIndex = 0
        Me.txtNCM.Tag = "NU"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(10, 48)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(28, 13)
        Me.Label13.TabIndex = 43
        Me.Label13.Text = "Cest"
        '
        'txtCEst
        '
        Me.txtCEst.Location = New System.Drawing.Point(50, 45)
        Me.txtCEst.Name = "txtCEst"
        Me.txtCEst.Size = New System.Drawing.Size(120, 20)
        Me.txtCEst.TabIndex = 1
        Me.txtCEst.Tag = "NU"
        '
        'txtCusto
        '
        Me.txtCusto.Location = New System.Drawing.Point(50, 47)
        Me.txtCusto.Name = "txtCusto"
        Me.txtCusto.Size = New System.Drawing.Size(74, 20)
        Me.txtCusto.TabIndex = 3
        Me.txtCusto.Tag = "MO"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(10, 50)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(34, 13)
        Me.Label14.TabIndex = 45
        Me.Label14.Text = "Custo"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(140, 50)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(31, 13)
        Me.Label15.TabIndex = 47
        Me.Label15.Text = "UND"
        '
        'txtUnd
        '
        Me.txtUnd.Location = New System.Drawing.Point(180, 47)
        Me.txtUnd.Name = "txtUnd"
        Me.txtUnd.Size = New System.Drawing.Size(55, 20)
        Me.txtUnd.TabIndex = 4
        Me.txtUnd.Tag = ""
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(10, 50)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(62, 13)
        Me.Label16.TabIndex = 49
        Me.Label16.Text = "Dig Máximo"
        '
        'txtDigMax
        '
        Me.txtDigMax.Location = New System.Drawing.Point(90, 47)
        Me.txtDigMax.Name = "txtDigMax"
        Me.txtDigMax.Size = New System.Drawing.Size(55, 20)
        Me.txtDigMax.TabIndex = 2
        Me.txtDigMax.Tag = "NU"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(160, 50)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(61, 13)
        Me.Label17.TabIndex = 51
        Me.Label17.Text = "Est Máximo"
        '
        'txtEstMaximo
        '
        Me.txtEstMaximo.Location = New System.Drawing.Point(230, 47)
        Me.txtEstMaximo.Name = "txtEstMaximo"
        Me.txtEstMaximo.Size = New System.Drawing.Size(55, 20)
        Me.txtEstMaximo.TabIndex = 3
        Me.txtEstMaximo.Tag = "NU"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(10, 77)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(60, 13)
        Me.Label18.TabIndex = 53
        Me.Label18.Text = "Est Mínimo"
        '
        'txtEstMinimo
        '
        Me.txtEstMinimo.Location = New System.Drawing.Point(90, 74)
        Me.txtEstMinimo.Name = "txtEstMinimo"
        Me.txtEstMinimo.Size = New System.Drawing.Size(55, 20)
        Me.txtEstMinimo.TabIndex = 4
        Me.txtEstMinimo.Tag = "NU"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(150, 77)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(65, 13)
        Me.Label19.TabIndex = 55
        Me.Label19.Text = "Qtd Frigobar"
        '
        'txtQtdFribobar
        '
        Me.txtQtdFribobar.Location = New System.Drawing.Point(230, 74)
        Me.txtQtdFribobar.Name = "txtQtdFribobar"
        Me.txtQtdFribobar.Size = New System.Drawing.Size(55, 20)
        Me.txtQtdFribobar.TabIndex = 5
        Me.txtQtdFribobar.Tag = "NU"
        '
        'chOpcional
        '
        Me.chOpcional.AutoSize = True
        Me.chOpcional.Location = New System.Drawing.Point(10, 40)
        Me.chOpcional.Name = "chOpcional"
        Me.chOpcional.Size = New System.Drawing.Size(108, 17)
        Me.chOpcional.TabIndex = 2
        Me.chOpcional.Text = "Produto Opcional"
        Me.chOpcional.UseVisualStyleBackColor = True
        '
        'chRetornavel
        '
        Me.chRetornavel.AutoSize = True
        Me.chRetornavel.Location = New System.Drawing.Point(10, 60)
        Me.chRetornavel.Name = "chRetornavel"
        Me.chRetornavel.Size = New System.Drawing.Size(118, 17)
        Me.chRetornavel.TabIndex = 4
        Me.chRetornavel.Text = "Produto Retornável"
        Me.chRetornavel.UseVisualStyleBackColor = True
        '
        'chBrinde
        '
        Me.chBrinde.AutoSize = True
        Me.chBrinde.Location = New System.Drawing.Point(130, 20)
        Me.chBrinde.Name = "chBrinde"
        Me.chBrinde.Size = New System.Drawing.Size(96, 17)
        Me.chBrinde.TabIndex = 1
        Me.chBrinde.Text = "Produto Brinde"
        Me.chBrinde.UseVisualStyleBackColor = True
        '
        'chConsumacao
        '
        Me.chConsumacao.AutoSize = True
        Me.chConsumacao.Location = New System.Drawing.Point(130, 40)
        Me.chConsumacao.Name = "chConsumacao"
        Me.chConsumacao.Size = New System.Drawing.Size(88, 17)
        Me.chConsumacao.TabIndex = 3
        Me.chConsumacao.Text = "Consumação"
        Me.chConsumacao.UseVisualStyleBackColor = True
        '
        'chCancela
        '
        Me.chCancela.AutoSize = True
        Me.chCancela.Location = New System.Drawing.Point(130, 60)
        Me.chCancela.Name = "chCancela"
        Me.chCancela.Size = New System.Drawing.Size(182, 17)
        Me.chCancela.TabIndex = 5
        Me.chCancela.Text = "Produto Cancela promoção suíte"
        Me.chCancela.UseVisualStyleBackColor = True
        '
        'chCardapio
        '
        Me.chCardapio.AutoSize = True
        Me.chCardapio.Location = New System.Drawing.Point(10, 80)
        Me.chCardapio.Name = "chCardapio"
        Me.chCardapio.Size = New System.Drawing.Size(101, 17)
        Me.chCardapio.TabIndex = 6
        Me.chCardapio.Text = "Cardápio Online"
        Me.chCardapio.UseVisualStyleBackColor = True
        '
        'chInativo
        '
        Me.chInativo.AutoSize = True
        Me.chInativo.Location = New System.Drawing.Point(130, 80)
        Me.chInativo.Name = "chInativo"
        Me.chInativo.Size = New System.Drawing.Size(58, 17)
        Me.chInativo.TabIndex = 7
        Me.chInativo.Text = "Inativo"
        Me.chInativo.UseVisualStyleBackColor = True
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(240, 88)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(45, 13)
        Me.Label20.TabIndex = 64
        Me.Label20.Text = "Label20"
        '
        'grpBasic
        '
        Me.grpBasic.Controls.Add(Me.lblCategoria)
        Me.grpBasic.Controls.Add(Me.Label1)
        Me.grpBasic.Controls.Add(Me.txtCodigo)
        Me.grpBasic.Controls.Add(Me.Label2)
        Me.grpBasic.Controls.Add(Me.txtNome)
        Me.grpBasic.Controls.Add(Me.Label3)
        Me.grpBasic.Controls.Add(Me.txtEan)
        Me.grpBasic.Controls.Add(Me.Label5)
        Me.grpBasic.Controls.Add(Me.txtCategoria)
        Me.grpBasic.Controls.Add(Me.btnBuscar)
        Me.grpBasic.Location = New System.Drawing.Point(10, 47)
        Me.grpBasic.Name = "grpBasic"
        Me.grpBasic.Size = New System.Drawing.Size(480, 100)
        Me.grpBasic.TabIndex = 0
        Me.grpBasic.TabStop = False
        Me.grpBasic.Text = "Informações Básicas"
        '
        'lblCategoria
        '
        Me.lblCategoria.AutoSize = True
        Me.lblCategoria.Location = New System.Drawing.Point(257, 72)
        Me.lblCategoria.Name = "lblCategoria"
        Me.lblCategoria.Size = New System.Drawing.Size(45, 13)
        Me.lblCategoria.TabIndex = 26
        Me.lblCategoria.Text = "Label21"
        Me.lblCategoria.Visible = False
        '
        'grpTax
        '
        Me.grpTax.Controls.Add(Me.Label8)
        Me.grpTax.Controls.Add(Me.txtVenda)
        Me.grpTax.Controls.Add(Me.Label9)
        Me.grpTax.Controls.Add(Me.cmbTributacao)
        Me.grpTax.Controls.Add(Me.Label10)
        Me.grpTax.Controls.Add(Me.txtAlicota)
        Me.grpTax.Controls.Add(Me.Label14)
        Me.grpTax.Controls.Add(Me.txtCusto)
        Me.grpTax.Controls.Add(Me.Label15)
        Me.grpTax.Controls.Add(Me.txtUnd)
        Me.grpTax.Controls.Add(Me.Label11)
        Me.grpTax.Controls.Add(Me.txtCFOP)
        Me.grpTax.Location = New System.Drawing.Point(10, 153)
        Me.grpTax.Name = "grpTax"
        Me.grpTax.Size = New System.Drawing.Size(480, 120)
        Me.grpTax.TabIndex = 1
        Me.grpTax.TabStop = False
        Me.grpTax.Text = "Tributação / Preços"
        '
        'grpInventory
        '
        Me.grpInventory.Controls.Add(Me.Label6)
        Me.grpInventory.Controls.Add(Me.cmbLocalPadrao)
        Me.grpInventory.Controls.Add(Me.Label7)
        Me.grpInventory.Controls.Add(Me.cmbCtrlEstoque)
        Me.grpInventory.Controls.Add(Me.Label19)
        Me.grpInventory.Controls.Add(Me.Label16)
        Me.grpInventory.Controls.Add(Me.txtQtdFribobar)
        Me.grpInventory.Controls.Add(Me.txtDigMax)
        Me.grpInventory.Controls.Add(Me.Label17)
        Me.grpInventory.Controls.Add(Me.txtEstMaximo)
        Me.grpInventory.Controls.Add(Me.Label18)
        Me.grpInventory.Controls.Add(Me.txtEstMinimo)
        Me.grpInventory.Location = New System.Drawing.Point(380, 279)
        Me.grpInventory.Name = "grpInventory"
        Me.grpInventory.Size = New System.Drawing.Size(360, 120)
        Me.grpInventory.TabIndex = 3
        Me.grpInventory.TabStop = False
        Me.grpInventory.Text = "Estoque / Controle"
        '
        'grpIdentification
        '
        Me.grpIdentification.Controls.Add(Me.btnBuscaNCM)
        Me.grpIdentification.Controls.Add(Me.Label12)
        Me.grpIdentification.Controls.Add(Me.txtNCM)
        Me.grpIdentification.Controls.Add(Me.Label13)
        Me.grpIdentification.Controls.Add(Me.txtCEst)
        Me.grpIdentification.Location = New System.Drawing.Point(10, 279)
        Me.grpIdentification.Name = "grpIdentification"
        Me.grpIdentification.Size = New System.Drawing.Size(360, 120)
        Me.grpIdentification.TabIndex = 2
        Me.grpIdentification.TabStop = False
        Me.grpIdentification.Text = "Identificação"
        '
        'grpFlags
        '
        Me.grpFlags.Controls.Add(Me.chServico)
        Me.grpFlags.Controls.Add(Me.chOpcional)
        Me.grpFlags.Controls.Add(Me.chRetornavel)
        Me.grpFlags.Controls.Add(Me.chBrinde)
        Me.grpFlags.Controls.Add(Me.chConsumacao)
        Me.grpFlags.Controls.Add(Me.chCancela)
        Me.grpFlags.Controls.Add(Me.chCardapio)
        Me.grpFlags.Controls.Add(Me.chInativo)
        Me.grpFlags.Location = New System.Drawing.Point(500, 153)
        Me.grpFlags.Name = "grpFlags"
        Me.grpFlags.Size = New System.Drawing.Size(321, 120)
        Me.grpFlags.TabIndex = 4
        Me.grpFlags.TabStop = False
        Me.grpFlags.Text = "Opções"
        '
        'btnBuscaNCM
        '
        Me.btnBuscaNCM.Location = New System.Drawing.Point(180, 14)
        Me.btnBuscaNCM.Name = "btnBuscaNCM"
        Me.btnBuscaNCM.Size = New System.Drawing.Size(95, 30)
        Me.btnBuscaNCM.TabIndex = 44
        Me.btnBuscaNCM.Text = "Buscar NCM"
        Me.btnBuscaNCM.UseVisualStyleBackColor = True
        '
        'Produtos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(824, 447)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.grpBasic)
        Me.Controls.Add(Me.grpTax)
        Me.Controls.Add(Me.grpIdentification)
        Me.Controls.Add(Me.grpInventory)
        Me.Controls.Add(Me.grpFlags)
        Me.Controls.Add(Me.PtImagem)
        Me.Controls.Add(Me.btnGravar)
        Me.Controls.Add(Me.Label20)
        Me.Name = "Produtos"
        Me.Text = "Produtos"
        CType(Me.PtImagem, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpBasic.ResumeLayout(False)
        Me.grpBasic.PerformLayout()
        Me.grpTax.ResumeLayout(False)
        Me.grpTax.PerformLayout()
        Me.grpInventory.ResumeLayout(False)
        Me.grpInventory.PerformLayout()
        Me.grpIdentification.ResumeLayout(False)
        Me.grpIdentification.PerformLayout()
        Me.grpFlags.ResumeLayout(False)
        Me.grpFlags.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents chServico As CheckBox
    Friend WithEvents btnGravar As Button
    Friend WithEvents txtNome As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtEan As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtCategoria As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents btnBuscar As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents cmbLocalPadrao As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents cmbCtrlEstoque As ComboBox
    Friend WithEvents txtVenda As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents cmbTributacao As ComboBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtAlicota As TextBox
    Friend WithEvents txtCFOP As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents PtImagem As PictureBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtNCM As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtCEst As TextBox
    Friend WithEvents txtCusto As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents txtUnd As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents txtDigMax As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents txtEstMaximo As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents txtEstMinimo As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents txtQtdFribobar As TextBox
    Friend WithEvents chOpcional As CheckBox
    Friend WithEvents chRetornavel As CheckBox
    Friend WithEvents chBrinde As CheckBox
    Friend WithEvents chConsumacao As CheckBox
    Friend WithEvents chCancela As CheckBox
    Friend WithEvents chCardapio As CheckBox
    Friend WithEvents chInativo As CheckBox
    Friend WithEvents Label20 As Label

    'Novos grupos para organizar UX sem alterar nomes e padrões visuais
    Friend WithEvents grpBasic As GroupBox
    Friend WithEvents grpTax As GroupBox
    Friend WithEvents grpInventory As GroupBox
    Friend WithEvents grpIdentification As GroupBox
    Friend WithEvents grpFlags As GroupBox
    Friend WithEvents lblCategoria As Label
    Friend WithEvents btnBuscaNCM As Button
End Class
