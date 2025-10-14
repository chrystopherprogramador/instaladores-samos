Public Class Produtos

    Public Property CodigoAtual As Long = 0

    Dim crud As crProdutos
    Public Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        Dim form As New clsGeForm(Me, True, False, False, Sub()
                                                              btnGravar.PerformClick()
                                                          End Sub)
        form.ConfiguraMe()



    End Sub
    Private Sub Produtos_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            If CodigoAtual = 0 Then
                txtCodigo.Text = clsGeBanco.IncrementarCampo("itens", "codigo")
            Else
                crud = New crProdutos With {
                    .codigo = CodigoAtual
                }
                crud.Executar(TabelaBase.Operacao.SELEC)
            End If

        Catch ex As Exception
            MsgBox("Erro ao carregar Formulário" & vbCrLf & ex.Message, vbCritical)
            Close()
        End Try

    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click

        Dim dtEspecie As DataTable = clsGeBanco.dataTable("SELECT Codigo,nome FROM especies ORDER BY nome")

        Dim frm As New Busca With {
        .Dados = dtEspecie,
        .ColunasVisuais = New Dictionary(Of String, String) From {
            {"codigo", "Código"},
            {"nome", "Descrição"}
        },
        .CampoRetorno = "codigo",
        .CamposFiltro = New List(Of String) From {"nome"}
        }

        frm.Inicializar()

        If frm.ShowDialog() = DialogResult.OK Then
            Dim codigoSelecionado = frm.Resultado
            txtCategoria.Text = codigoSelecionado
            txtCategoria.Select()
        End If
    End Sub

    Private Sub txtCategoria_TextChanged(sender As Object, e As EventArgs) Handles txtCategoria.TextChanged

        If IsNumeric(txtCategoria.Text) Then


            Dim DtEspecie As DataTable = clsGeBanco.dataTable($"SELECT nome FROM especies WHERE Codigo = {CLng(txtCategoria.Text)}")

            If DtEspecie.Rows.Count > 0 Then
                lblCategoria.Visible = True
                lblCategoria.Text = DtEspecie.Rows(0)("nome").ToString()
            Else
                lblCategoria.Visible = False
            End If

        End If

    End Sub

    Private Sub Produtos_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If CodigoAtual <> 0 Then
            Try
                txtCodigo.Enabled = False


                With crud
                    txtCodigo.Text = .codigo
                    txtNome.Text = .descricao
                    txtEan.Text = .CodGTIN
                    txtCategoria.Text = .especie
                    txtVenda.Text = FormatNumber(.venda, 2)
                    cmbTributacao.SelectedIndex = FormatNumber(.tributacao, 2)
                    txtAlicota.Text = FormatNumber(.aliquota, 2)
                    txtCusto.Text = FormatNumber(.custo, 2)
                    txtUnd.Text = .unidade
                    txtCFOP.Text = .CFOP
                    txtNCM.Text = .NCM
                    txtCEst.Text = .CEst
                    cmbLocalPadrao.SelectedIndex = .categoria
                    cmbCtrlEstoque.SelectedIndex = .tipo
                    txtDigMax.Text = .vendamaxima
                    txtEstMaximo.Text = .maximo
                    txtEstMinimo.Text = .minimo
                    txtQtdFribobar.Text = .frigobar

                    If System.IO.File.Exists(.foto) Then
                        PtImagem.Image = Image.FromFile(.foto)
                    End If

                    chServico.Checked = .servico
                    chBrinde.Checked = .produtobrinde
                    chOpcional.Checked = .composicaoopcional
                    chConsumacao.Checked = .Consumacao
                    chRetornavel.Checked = .retornavel
                    chCancela.Checked = .itemadm
                    chCardapio.Checked = .cardapioonline
                    chInativo.Checked = .inativo

                End With
            Catch ex As Exception
                MsgBox("Erro ao carregar Formulário" & vbCrLf & ex.Message, vbCritical)
                Close()
            End Try
        End If
    End Sub

    Private Sub btnGravar_Click(sender As Object, e As EventArgs) Handles btnGravar.Click
        If CLng(txtCodigo.Text) = 0 Then
            MsgBox("Código inválido.", vbCritical, "Aviso.")
            txtCodigo.Select()
            Exit Sub
        ElseIf Len(Trim(txtNome.Text)) = 0 Then
            MsgBox("Descrição inválida.", vbCritical, "Aviso")
            txtNome.Select()
            Exit Sub
        ElseIf Len(Trim(txtUnd.Text)) = 0 Then
            MsgBox("Unidade inválida.", vbCritical, "Aviso")
            txtUnd.Select()
            Exit Sub
        ElseIf Len(Trim(cmbLocalPadrao.Text)) = 0 Then
            MsgBox("Categoria inválida.", vbCritical, "Aviso")
            cmbLocalPadrao.Select()
            Exit Sub
        ElseIf Len(Trim(cmbTributacao.Text)) = 0 Then
            MsgBox("Tipo de tributação inválida", vbCritical, "Aviso.")
            cmbTributacao.Select()
            Exit Sub
        ElseIf Len(Trim(cmbCtrlEstoque.Text)) = 0 Then
            MsgBox("Tipo de fabricação inválida.", vbCritical, "Aviso.")
            cmbCtrlEstoque.Select()
            Exit Sub
        ElseIf Mid(cmbTributacao.Text, 1, 1) = "T" Then
            If CSng(txtAlicota.Text) = 0 Then
                MsgBox("Mercadoria Tributada deverá ser informada.", vbCritical, "Aviso")
                txtAlicota.Select()
                Exit Sub
            End If
        ElseIf Val(txtCategoria.Text) = 0 Then
            MsgBox("Espécie não informada...", vbInformation)
            txtCategoria.Select()
            Exit Sub
        End If


        Dim crus As New crProdutos With {
            .codigo = CLng(txtCodigo.Text),
            .descricao = UCase(Trim(txtNome.Text)),
            .unidade = UCase(Trim(txtUnd.Text)),
            .categoria = cmbLocalPadrao.SelectedIndex,
            .tributacao = cmbTributacao.SelectedIndex,
            .aliquota = CDec(txtAlicota.Text),
            .venda = CDec(txtVenda.Text),
            .custo = CDec(txtCusto.Text),
            .tipo = cmbCtrlEstoque.SelectedIndex,
            .vendamaxima = CDec(txtDigMax.Text),
            .devolucao = "N",
            .especie = CLng(txtCategoria.Text),
            .minimo = CDec(txtEstMinimo.Text),
            .maximo = CDec(txtEstMaximo.Text),
            .itemadm = IIf(chCancela.Checked, 1, 0),
            .frigobar = CDec(txtQtdFribobar.Text),
            .servico = chServico.Checked,
            .retornavel = chRetornavel.Checked,
            .composicaoopcional = chOpcional.Checked,
            .produtobrinde = chBrinde.Checked,
            .CodGTIN = Trim(txtEan.Text),
            .Consumacao = chConsumacao.Checked,
            .CFOP = CLng(IIf(Len(Trim(txtCFOP.Text)) = 0, 0, txtCFOP.Text)),
            .NCM = CLng(IIf(Len(Trim(txtNCM.Text)) = 0, 0, txtNCM.Text)),
            .CEst = CLng(IIf(Len(Trim(txtCEst.Text)) = 0, 0, txtCEst.Text)),
            .expromesp = False,
            .cardapioonline = chCardapio.Checked,
            .foto = PtImagem.Tag,
            .alteraupload = True,
            .inativo = chInativo.Checked
        }
        With crus
            If CodigoAtual = 0 Then
                Dim exe As String = .Executar(TabelaBase.Operacao.INSERT)
                If exe = String.Empty Then
                    MsgBox("Registro gravado com sucesso!", MsgBoxStyle.Information)
                Else
                    MsgBox("Falha ao gravar registro" & vbCrLf & exe, vbCritical)
                End If
            Else
                Dim exe As String = .Executar(TabelaBase.Operacao.UPDATE)
                If exe = String.Empty Then
                    MsgBox("Registro alterado com sucesso!", MsgBoxStyle.Information)
                Else
                    MsgBox("Falha ao alterar registro" & vbCrLf & exe, vbCritical)
                End If
            End If
        End With

        Close()

    End Sub

    Private Sub cmbTributacao_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTributacao.SelectedIndexChanged
        With cmbTributacao
            Select Case .SelectedIndex
                Case 0
                    txtCFOP.Text = 5102
                Case 1
                    txtCFOP.Text = 5405
                Case 2
                    txtCFOP.Text = 5910
            End Select
        End With
    End Sub

    Private Sub PtImagem_Click(sender As Object, e As EventArgs) Handles PtImagem.Click
        Using dlg As New OpenFileDialog()
            dlg.Title = "Selecionar imagem do produto"
            dlg.Filter = "Imagens (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)

            If dlg.ShowDialog() = DialogResult.OK Then
                Dim caminhoImagem As String = dlg.FileName

                ' Armazena o caminho na Tag
                PtImagem.Tag = caminhoImagem

                ' Exibe a imagem no controle
                Try
                    Using imgTemp = Image.FromFile(caminhoImagem)
                        PtImagem.Image = New Bitmap(imgTemp)
                    End Using
                Catch ex As Exception
                    MsgBox("Erro ao carregar imagem: " & ex.Message, vbCritical)
                    PtImagem.Image = Nothing
                    PtImagem.Tag = Nothing
                End Try
            End If
        End Using
    End Sub

    Private Sub btnBuscaNCM_Click(sender As Object, e As EventArgs) Handles btnBuscaNCM.Click
        Dim descricaoProduto As String = txtNome.Text.Trim()

        If String.IsNullOrWhiteSpace(descricaoProduto) Then
            MsgBox("Informe a descrição do produto para buscar o NCM.", vbExclamation)
            Exit Sub
        End If

        Dim ncmEncontrado As String = BuscarNCMOnline(descricaoProduto)

        If Not String.IsNullOrWhiteSpace(ncmEncontrado) Then
            txtNCM.Text = ncmEncontrado
        Else
            MsgBox("NCM não encontrado para esta descrição.", vbInformation)
        End If
    End Sub

    Private Function BuscarNCMOnline(descricao As String) As String
        Dim urlBusca = "https://agilize.com.br/consultar-ncm/?q=" & Uri.EscapeDataString(descricao)
        Process.Start(urlBusca)
        Return "" ' Ou implementar scraping/API se disponível
    End Function

End Class