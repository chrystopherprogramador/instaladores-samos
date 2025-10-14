Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Module ModLicenca

    Public LICENCA As Boolean
    Public pValidade As String
    Public Validade As Date

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto)>
    Public Function GetVolumeInformation(
        ByVal lpRootPathName As String,
        ByVal lpVolumeNameBuffer As StringBuilder,
        ByVal nVolumeNameSize As Integer,
        ByRef lpVolumeSerialNumber As Integer,
        ByRef lpMaximumComponentLength As Integer,
        ByRef lpFileSystemFlags As Integer,
        ByVal lpFileSystemNameBuffer As StringBuilder,
        ByVal nFileSystemNameSize As Integer
    ) As Boolean
    End Function

    <DllImport("user32.dll")>
    Public Function SetForegroundWindow(ByVal hwnd As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll")>
    Public Sub Sleep(ByVal dwMilliseconds As Integer)
    End Sub

    Public Serial As String
    Public Licenciado As String
    Public Senha As String
    Public DriveSys As String = "C:\" 'padrão

    Public Function SoNumero(Vlr As String) As String
        If String.IsNullOrWhiteSpace(Vlr) Then Return ""
        Dim apenasNumeros = New String(Vlr.Where(Function(c) Char.IsDigit(c)).ToArray())
        Return If(String.IsNullOrEmpty(apenasNumeros), "0", apenasNumeros)
    End Function

    Public Sub Bloqueio()
        Try
            LICENCA = False
            Dim endWin As String = Environment.GetFolderPath(Environment.SpecialFolder.Windows)
            Dim arquivo As String = Path.Combine(endWin, "Controlecd.sys")

            If Not File.Exists(arquivo) Then
                Dim Linha As String = GeraSerial(DriveSys, DateAdd(DateInterval.Day, 10, Date.Today))
                File.WriteAllText(arquivo, Linha.PadRight(50) & "@_#")
            End If
            Dim conteudo As String = File.ReadAllText(arquivo).Trim()
            Dim valorNumerico As String = Mid(conteudo, 1, 8)

            If Not IsNumeric(valorNumerico) Then
                Throw New Exception("Arquivo de licença inválido: valor de data corrompido.")
            End If

            ' Divide por 2 e tenta remontar como DD/MM/YYYY
            Dim dataTexto As String = (CDbl(valorNumerico) / 2).ToString("00000000")

            Dim dia As Integer = CInt(Mid(dataTexto, 1, 2))
            Dim mes As Integer = CInt(Mid(dataTexto, 3, 2))
            Dim ano As Integer = CInt(Mid(dataTexto, 5, 4))

            If mes < 1 Or mes > 12 Or dia < 1 Or dia > 31 Then
                Throw New Exception("Arquivo de licença inválido: data fora do intervalo.")
            End If

            Validade = New Date(ano, mes, dia)
            pValidade = $"{DateDiff(DateInterval.Day, Validade, Date.Today):000} dias"

            If Validade >= Date.Today AndAlso Mid(conteudo, 51, 3) = "@_#" Then
                LICENCA = True
            Else
                LICENCA = False
                File.WriteAllText(arquivo, conteudo.PadRight(50) & "@_#")
            End If
        Catch ex As Exception
            If Not ex.Message.Contains("type mismatch") Then
                MsgBox($"Erro: {ex.Message}", MsgBoxStyle.Critical, Application.ProductName)
            End If
            LICENCA = False
        End Try
    End Sub
    Public Function DigitoVerificador(Codigo As String) As String
        Dim cCodigo As String = Codigo.Replace("A", "65")
        Dim cSoma As Long = 0

        For i As Integer = cCodigo.Length To 1 Step -1
            If Char.IsDigit(cCodigo(i - 1)) Then
                cSoma += Val(cCodigo(i - 1)) * i
            End If
        Next

        Dim Dig = cSoma Mod 11
        Return If(Dig >= 10, "0", Dig.ToString())
    End Function

    Public Function PegaNumero(Und As String) As String
        Dim volName As New StringBuilder(255)
        Dim fsName As New StringBuilder(255)
        Dim serial As Integer
        Dim maxCompLen As Integer
        Dim flags As Integer

        GetVolumeInformation(Und, volName, volName.Capacity, serial, maxCompLen, flags, fsName, fsName.Capacity)
        Return serial.ToString()
    End Function
    Public Sub DesBloqueio(Codigo As String, Hd As String)
        Try
            LICENCA = False
            Dim appFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Samos")
            If Not Directory.Exists(appFolder) Then
                Directory.CreateDirectory(appFolder)
            End If
            Dim arquivo As String = Path.Combine(appFolder, "Controlecd.sys")
            Dim Linha As String = Left(Codigo, 8) & "A" & Hd
            File.WriteAllText(arquivo, Linha.PadRight(50) & "@_#")

            LICENCA = True
        Catch ex As Exception
            MsgBox($"Erro: {ex.Message}", MsgBoxStyle.Critical, "Nosso Programa - Desbloqueador")
        End Try
    End Sub

    Public Function GeraSerial(DriveBd As String, DataVal As Date) As String
        Dim Hd As String = PegaNumero(DriveBd)
        Dim t As Integer = If(Hd.StartsWith("-"), -1, 1)

        Dim VALORX As String = (2 * CDbl(SoNumero(Format(DataVal, "ddMMyyyy")))).ToString("00000000")
        Dim MES As Integer = CInt(Mid(VALORX, 1, 2))
        Dim Password As String = Math.Round((((CDbl(Hd) * 2) / MES) * 2 * t), 0).ToString()

        Return VALORX & "A" & Password
    End Function

    Public Sub Main()
        If Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1 Then
            MsgBox("O aplicativo já se encontra em execução.", MsgBoxStyle.Critical, "Atenção")
            End
        End If

        Serial = GetSetting("Gcon", "Autenticacao", "Serial", "")
        Senha = GetSetting("Gcon", "Autenticacao", "Senha", "")
        Licenciado = GetSetting("Gcon", "Autenticacao", "Licenciado", "")

        If String.IsNullOrWhiteSpace(Serial) Then
            ' FrmSerial.ShowDialog()
        Else
            If Not DesCriptografia(Serial) Then
                MsgBox("Autenticação do sistema não confere.", MsgBoxStyle.Information, "Atenção")
                DeleteSetting("Gcon", "Autenticacao")
                ' FrmSerial.ShowDialog()
            Else
                ' FrmSplash.ShowDialog()
            End If
        End If
    End Sub

    Public Function Criptografia(Codigo As Integer, Serial As Integer) As String
        Dim DecodeKey As Integer = Codigo Mod 11
        Dim hexSerial As String = Hex(Serial)
        Dim Pwd As String = ""

        For Each ch In hexSerial
            Pwd &= Chr(DecodeKey + Asc(ch))
        Next

        Dim X As String = $"{Year(Today)}{Len(StrReverse(hexSerial))}{Pwd}{StrReverse(hexSerial)}{StrReverse(Codigo.ToString())}{Len(StrReverse(Codigo.ToString())):00}{Today:MMdd}"
        Return X
    End Function

    Private Function DesCriptografia(Autenticacao As String) As Boolean
        Try
            Dim Reverse As String = StrReverse(Autenticacao)
            Dim CodigoLen As Integer = CInt(Mid(Autenticacao, 5, 1))
            Dim Codigo As String = Mid(Autenticacao, 6, CodigoLen)

            Dim DecodeKey As Integer = 10
            Dim Pwd As String = ""
            For Each ch In Codigo
                Pwd &= Chr(Asc(ch) - DecodeKey)
            Next

            Dim Hd As String = PegaNumero("C:\")
            Dim t As Integer = If(Hd.StartsWith("-"), -1, 1)
            Dim X As Double = Math.Round((((CDbl(Hd) * 2) / 14) * 2 * t), 0)

            Return (X.ToString() = Mid(Reverse, 7, CInt(StrReverse(Mid(Reverse, 5, 2)))))
        Catch ex As Exception
            DeleteSetting("Gcon", "Autenticacao")
            ' FrmSerial.ShowDialog()
            Return False
        End Try
    End Function
End Module
