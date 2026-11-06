Imports DocumentFormat.OpenXml.Spreadsheet

Public Class Suites

    Public Property CodigoAtual As Long = 0
    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().
        Dim form As New clsGeForm(Me, True, False, False, Sub()
                                                              btnGravar.PerformClick()
                                                          End Sub)
        form.ConfiguraMe()
    End Sub
    Private Sub btnBuscaSigla_Click(sender As Object, e As EventArgs) Handles btnBuscaSigla.Click
        Dim frm As New Busca With {
       .Dados = clsGeBanco.dataTable("SELECT codigo,sigla,txadd,qtdpessoa FROM sigla ORDER BY sigla "),
       .ColunasVisuais = New Dictionary(Of String, String) From {
           {"Código", "Código"},
           {"sigla", "Sigla"},
           {"txadd", "Tx Adicional"},
           {"qtdpessoa", "Qtd. Pessoa"}
       },
       .CampoRetorno = "codigo",
       .CamposFiltro = New List(Of String) From {"sigla"}
       }

        frm.Inicializar()

        If frm.ShowDialog() = DialogResult.OK Then
            Dim codigoSelecionado = frm.Resultado
            txtSigla.Text = codigoSelecionado
            txtArea.Select()
        End If
    End Sub

    Private Sub Suites_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        disSigla.Text = String.Empty
        disArea.Text = String.Empty
    End Sub
    Private Sub txtNumero_LostFocus(sender As Object, e As EventArgs) Handles txtNumero.LostFocus
        If CodigoAtual = 0 Then
            If Not IsNumeric(txtNumero.Text) Then txtNumero.Text = clsGeBanco.IncrementarCampo("suites", "numero")
            Dim DtSuite As DataTable = clsGeBanco.dataTable("SELECT 
                                                        s.numero as numero,si.sigla as sigla,s.nome as nome,
                                                        si.qtdpessoa,si.txadd
                                                        FROM suites s
                                                        INNER JOIN sigla si on si.codigo = s.sigla
                                                        WHERE s.numero = " & CLng(txtNumero.Text))
            If DtSuite.Rows.Count > 0 Then
                If MsgBox("Já existe uma suíte cadastrada com este código!" & vbCrLf &
                       "Nome: " & DtSuite.Rows(0)("nome") & vbCrLf &
                       "Sigla: " & DtSuite.Rows(0)("sigla") & vbCrLf &
                       "Deseja editar esta suíte?", vbQuestion + vbYesNo) = MsgBoxResult.Yes Then

                Else
                    txtNumero.Text = clsGeBanco.IncrementarCampo("suites", "numero")
                End If
            End If
        End If
    End Sub
End Class