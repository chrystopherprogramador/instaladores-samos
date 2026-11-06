Imports MySql.Data.MySqlClient

Public Class clsGeScriptBanco

    Private _ip As String
    Private _nome As String
    Private _usuario As String
    Private _senha As String
    Private _porta As Long

    Public Sub New(ip As String, nome As String, usuario As String, senha As String, Optional porta As Long = 3306)
        _ip = ip
        _nome = nome
        _usuario = usuario
        _senha = senha
        _porta = porta
    End Sub

    Public Function executaScript(Optional cria As Boolean = True) As Boolean
        Try
            Dim connStr = $"server={_ip};user={_usuario};password={_senha};port={_porta};"
            Using conn As New MySqlConnection(connStr)
                conn.Open()

                ' Verifica se o banco existe
                Dim cmdVerifica = New MySqlCommand($"SHOW DATABASES LIKE '{_nome}'", conn)
                Dim existe = cmdVerifica.ExecuteScalar() IsNot Nothing

                If Not existe AndAlso cria Then
                    Dim cmdCria = New MySqlCommand($"CREATE DATABASE `{_nome}`", conn)
                    cmdCria.ExecuteNonQuery()
                End If
            End Using

            ' Conecta ao banco
            Dim connStrBanco = $"server={_ip};database={_nome};user={_usuario};password={_senha};port={_porta};"
            Using connBanco As New MySqlConnection(connStrBanco)
                connBanco.Open()

                For Each comando In ScriptBanco
                    Dim sql = comando.Trim()
                    Try
                        If Not String.IsNullOrWhiteSpace(sql) Then
                            Using cmd As New MySqlCommand(sql, connBanco)
                                cmd.ExecuteNonQuery()
                            End Using
                        End If
                    Catch ex As Exception
                        If sql.ToUpper().Contains("DROP") Or sql.ToUpper().Contains("MODIFY") Then
                            Continue For
                        End If
                        MsgBox($"Erro ao executar script da tabela {comando}", vbInformation)
                    End Try

                Next
            End Using

            Return True
        Catch ex As Exception
            MsgBox("Erro ao criar banco ou executar script: " & ex.Message, vbCritical)
            Return False
        End Try
    End Function

    Private ReadOnly Property ScriptBanco As String()
        Get
            Return New String() {
                "CREATE TABLE IF NOT EXISTS `acesso` (`cod` SMALLINT NOT NULL, `data` DATETIME, `hora` DATETIME, `usuario` SMALLINT, PRIMARY KEY (`cod`))",
                "CREATE TABLE IF NOT EXISTS `acessorios` (`movto` INT NOT NULL, `codigo` INT NOT NULL, `hora` VARCHAR(5) NOT NULL, `descricao` VARCHAR(50), `qtde` FLOAT, PRIMARY KEY (`hora`,`codigo`,`movto`))",
                "CREATE TABLE IF NOT EXISTS `Aliquotas` (`Aliquota` FLOAT NOT NULL, `Indice` INT, PRIMARY KEY (`Aliquota`))",
                "CREATE TABLE IF NOT EXISTS `area` (`codigo` INT NOT NULL, `nome` VARCHAR(50), `porta` VARCHAR(30), PRIMARY KEY (`codigo`))",
                "CREATE TABLE IF NOT EXISTS `CartaoVip` (`Numero` INT NOT NULL, `Nome` VARCHAR(50), `Telefone` VARCHAR(20), `CfgVip` INT, `ativo` TINYINT(1), `Nascimento` DATETIME, `numeromembro` INT, `email` VARCHAR(250), `celular` VARCHAR(20), `cpf` VARCHAR(20), `digital` TINYINT(1), PRIMARY KEY (`Numero`))",
                "CREATE TABLE IF NOT EXISTS `CatSuites` (`SiglaCat` VARCHAR(4) NOT NULL, `IdCatSuites` INT, `NomeCateg` VARCHAR(35), `QtdePessoas` INT, `idGuia` INT, PRIMARY KEY (`SiglaCat`))",
                "CREATE TABLE IF NOT EXISTS `cfgimp` (`codigo` INT NOT NULL, `portares` VARCHAR(30), `portabar` VARCHAR(30), `portacoz` VARCHAR(30), PRIMARY KEY (`codigo`))",
                "CREATE TABLE IF NOT EXISTS `CfgIntegracaoCX` (`CfgCX` INT NOT NULL, `ContaCX` VARCHAR(30), PRIMARY KEY (`CfgCX`))",
                "CREATE TABLE IF NOT EXISTS `Clientes` (`CPF` VARCHAR(18) NOT NULL, `Nome` VARCHAR(50), `Endereco` VARCHAR(100), `Telefone` VARCHAR(15), `Celular` VARCHAR(15), `datanasc` DATETIME, `cidade` VARCHAR(40), `UF` VARCHAR(2), `email` VARCHAR(250), PRIMARY KEY (`CPF`))",
                "CREATE TABLE IF NOT EXISTS `ClientesVip` (`Codigo` INT NOT NULL, `CPF` VARCHAR(25), `Desconto` TINYINT(1), `PercDesconto` INT, `DescontoTotal` TINYINT(1), `LiberaAdic` TINYINT(1), `NumeroCartao` INT, `ValidadeCartao` DATETIME, PRIMARY KEY (`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `Climatizacao` (`Categoria` VARCHAR(4) NOT NULL, `QtdeFila` INT, `Horaini` DATETIME, `Horafin` DATETIME, `Ativo` TINYINT(1), PRIMARY KEY (`Categoria`))",
                "CREATE TABLE IF NOT EXISTS `compitem` (`item` INT NOT NULL, `item2` INT NOT NULL, `qtde` FLOAT, PRIMARY KEY (`item2`,`item`))",
                "CREATE TABLE IF NOT EXISTS `config` (
                  `codigo` SMALLINT NOT NULL,
                  `ECFpadrao` SMALLINT NULL DEFAULT NULL,
                  `promocao` VARCHAR(1) NULL DEFAULT NULL,
                  `hsini` VARCHAR(5) NULL DEFAULT NULL,
                  `hsfin` VARCHAR(5) NULL DEFAULT NULL,
                  `diaini` TINYINT UNSIGNED NULL DEFAULT NULL,
                  `diafin` TINYINT UNSIGNED NULL DEFAULT NULL,
                  `tpimpR` TINYINT UNSIGNED NULL DEFAULT NULL,
                  `espR` TINYINT UNSIGNED NULL DEFAULT NULL,
                  `tpimpC` TINYINT UNSIGNED NULL DEFAULT NULL,
                  `espC` TINYINT UNSIGNED NULL DEFAULT NULL,
                  `tpimpB` TINYINT UNSIGNED NULL DEFAULT NULL,
                  `espB` TINYINT UNSIGNED NULL DEFAULT NULL,
                  `tpimpRel` TINYINT UNSIGNED NULL DEFAULT NULL,
                  `espRel` TINYINT UNSIGNED NULL DEFAULT NULL,
                  `taradic` VARCHAR(1) NULL DEFAULT NULL,
                  `qtdehs` INT NULL DEFAULT NULL,
                  `perini` DATETIME NULL DEFAULT NULL,
                  `perfin` DATETIME NULL DEFAULT NULL,
                  `percas` FLOAT NULL DEFAULT NULL,
                  `totper` DATETIME NULL DEFAULT NULL,
                  `turno1` TINYINT(1) NULL DEFAULT NULL,
                  `hiturno1` DATETIME NULL DEFAULT NULL,
                  `hfturno1` DATETIME NULL DEFAULT NULL,
                  `turno2` TINYINT(1) NULL DEFAULT NULL,
                  `hiturno2` DATETIME NULL DEFAULT NULL,
                  `hfturno2` DATETIME NULL DEFAULT NULL,
                  `turno3` TINYINT(1) NULL DEFAULT NULL,
                  `hiturno3` DATETIME NULL DEFAULT NULL,
                  `hfturno3` DATETIME NULL DEFAULT NULL,
                  `turno4` TINYINT(1) NULL DEFAULT NULL,
                  `hiturno4` DATETIME NULL DEFAULT NULL,
                  `hfturno4` DATETIME NULL DEFAULT NULL,
                  `diainiper` INT NULL DEFAULT NULL,
                  `diafimper` INT NULL DEFAULT NULL,
                  `mudatabela` DATETIME NULL DEFAULT NULL,
                  `mudatabelaper` DATETIME NULL DEFAULT NULL,
                  `pernoitefs` TINYINT(1) NULL DEFAULT NULL,
                  `perinifs` DATETIME NULL DEFAULT NULL,
                  `perfinfs` DATETIME NULL DEFAULT NULL,
                  `totperfs` DATETIME NULL DEFAULT NULL,
                  `implocalremoto` TINYINT(1) NULL DEFAULT NULL,
                  `nivelbaixopdv` TINYINT(1) NULL DEFAULT NULL,
                  `nivelbaixoECF` TINYINT(1) NULL DEFAULT NULL,
                  `ecfrestaurante` TINYINT(1) NULL DEFAULT NULL,
                  `integraadm` TINYINT(1) NULL DEFAULT NULL,
                  `caminhoadm` VARCHAR(150) NULL DEFAULT NULL,
                  `nivelbaixoEstorno` TINYINT(1) NULL DEFAULT NULL,
                  `PPA` TINYINT(1) NULL DEFAULT NULL,
                  `estoqueseparado` TINYINT(1) NULL DEFAULT NULL,
                  `ToleranciaE` DATETIME NULL DEFAULT NULL,
                  `TEF` TINYINT(1) NULL DEFAULT NULL,
                  `lencois` INT NULL DEFAULT NULL,
                  `toalhas` INT NULL DEFAULT NULL,
                  `QtdeLencois` FLOAT NULL DEFAULT NULL,
                  `QtdeToalhas` FLOAT NULL DEFAULT NULL,
                  `imprimeliberacao` TINYINT(1) NULL DEFAULT NULL,
                  `ControlaLimpeza` TINYINT(1) NULL DEFAULT NULL,
                  `TempoCtrlLimpeza` INT NULL DEFAULT NULL,
                  `IgnorarSuja` TINYINT(1) NULL DEFAULT NULL,
                  `versaoenergia` INT NULL DEFAULT NULL,
                  `SituacaoGaragem` TINYINT(1) NULL DEFAULT NULL,
                  `ReservaSuite` TINYINT(1) NULL DEFAULT NULL,
                  `TempoRecebimento` DATETIME NULL DEFAULT NULL,
                  `VarreduraPlaca` TINYINT(1) NULL DEFAULT NULL,
                  `VarreduraCamareira` TINYINT(1) NULL DEFAULT NULL,
                  `ReaberturaConta` TINYINT(1) NULL DEFAULT NULL,
                  `EstornoLocacao` TINYINT(1) NULL DEFAULT NULL,
                  `nivelbaixoImpPDV` TINYINT(1) NULL DEFAULT NULL,
                  `DescontoEntrada` TINYINT(1) NULL DEFAULT NULL,
                  `BloqueaConferencia` TINYINT(1) NULL DEFAULT NULL,
                  `PernoiteFixa` TINYINT(1) NULL DEFAULT NULL,
                  `TempoPernoiteFixa` DATETIME NULL DEFAULT NULL,
                  `RecPenhora` TINYINT(1) NULL DEFAULT NULL,
                  `OutrasTaxas` TINYINT(1) NULL DEFAULT NULL,
                  `Transferencia` TINYINT(1) NULL DEFAULT NULL,
                  `ReaberturaAutomatica` TINYINT(1) NULL DEFAULT NULL,
                  `AgregaPernoite` TINYINT(1) NULL DEFAULT NULL,
                  `DescontoRapido` TINYINT(1) NULL DEFAULT NULL,
                  `ModeloPDV` INT NULL DEFAULT NULL,
                  `AgregaPSlocacao` TINYINT(1) NULL DEFAULT NULL,
                  `descontotempo` TINYINT(1) NULL DEFAULT NULL,
                  `DescontoMax` FLOAT NULL DEFAULT NULL,
                  `Diarias` TINYINT(1) NULL DEFAULT NULL,
                  `HoraDiaria` INT NULL DEFAULT NULL,
                  `ImprimeCanhoto` TINYINT(1) NULL DEFAULT NULL,
                  `TurnoCXFixo` TINYINT(1) NULL DEFAULT NULL,
                  `AdCasalDif` TINYINT(1) NULL DEFAULT NULL,
                  `PerCasF` FLOAT NULL DEFAULT NULL,
                  `indiceISS` INT NULL DEFAULT NULL,
                  `VersaoGCIS` INT NULL DEFAULT NULL,
                  `PadraoEpson` TINYINT(1) NULL DEFAULT NULL,
                  `AvaliacaoTarifacao` VARCHAR(1) NULL DEFAULT NULL,
                  `ToleranciaIni` DATETIME NULL DEFAULT NULL,
                  `impresumosimp` TINYINT(1) NULL DEFAULT NULL,
                  `impresumofechamento` TINYINT(1) NULL DEFAULT NULL,
                  `ctrl1` TINYINT(1) NULL DEFAULT NULL,
                  `ctrl1_1` INT NULL DEFAULT NULL,
                  `ctrl1_2` INT NULL DEFAULT NULL,
                  `CtrlFuncRec` TINYINT(1) NULL DEFAULT NULL,
                  `TipoFinalSemana` INT NULL DEFAULT NULL,
                  `MostrarEntrada` TINYINT(1) NULL DEFAULT NULL,
                  `QtdeEject` INT NULL DEFAULT NULL,
                  `Guilhotina` TINYINT(1) NULL DEFAULT NULL,
                  `CtrlTempoPatio` TINYINT(1) NULL DEFAULT NULL,
                  `PerIni2` DATETIME NULL DEFAULT NULL,
                  `PerFin2` DATETIME NULL DEFAULT NULL,
                  `PerIniFS2` DATETIME NULL DEFAULT NULL,
                  `PerFinFS2` DATETIME NULL DEFAULT NULL,
                  `ImpEstorno` TINYINT(1) NULL DEFAULT NULL,
                  `MensGaragem` VARCHAR(30) NULL DEFAULT NULL,
                  `impEnd2v` TINYINT(1) NULL DEFAULT NULL,
                  `ConfImpRec` TINYINT(1) NULL DEFAULT NULL,
                  `StsRevLimpezaSup` TINYINT(1) NULL DEFAULT NULL,
                  `SupNrRec` TINYINT(1) NULL DEFAULT NULL,
                  `RecebimentoUnico` TINYINT(1) NULL DEFAULT NULL,
                  `caixaunificado` TINYINT(1) NULL DEFAULT NULL,
                  `TempoCtrlLimpezaRev` INT NULL DEFAULT NULL,
                  `ControlaLimpezaRev` TINYINT(1) NULL DEFAULT NULL,
                  `SamosAPL` TINYINT(1) NULL DEFAULT NULL,
                  `SamosAplURL` VARCHAR(255) NULL DEFAULT NULL,
                  PRIMARY KEY (`codigo`)
                );
                ",
                "CREATE TABLE IF NOT EXISTS `ConfigVip` (`Cod` INT NOT NULL, `Descricao` VARCHAR(30), `Tipo` VARCHAR(1), `QtdeOcu` INT, `Percentual` FLOAT, `expiracao` INT, `saqueparcial` TINYINT(1), `fechamento` VARCHAR(1), PRIMARY KEY (`Cod`))",
                "CREATE TABLE IF NOT EXISTS `ConfigVip2` (`Cod2` INT NOT NULL, `TabelaP` INT NOT NULL, `Percentual` FLOAT, PRIMARY KEY (`TabelaP`,`Cod2`))",
                "CREATE TABLE IF NOT EXISTS `consumos` (`numero` INT NOT NULL, `item` SMALLINT NOT NULL, `hora` VARCHAR(5) NOT NULL, `descricao` VARCHAR(255), `tipo` VARCHAR(1), `qtde` FLOAT, `valor` FLOAT, `total` FLOAT, `impresso` TINYINT(1), `DataBaixa` DATETIME, `servico` TINYINT(1), `TaxaServicos` FLOAT, `ctrlpedido` INT, `HoraImp` DATETIME, PRIMARY KEY (`hora`,`item`,`numero`))",
                "CREATE TABLE IF NOT EXISTS `ConsumosBrindes` (`movto` INT NOT NULL, `item` INT NOT NULL, `hora` DATETIME NOT NULL, `qtde` FLOAT, `usuario` INT, PRIMARY KEY (`hora`,`item`,`movto`))",
                "CREATE TABLE IF NOT EXISTS `consumosopcionais` (`movto` INT NOT NULL, `item` INT NOT NULL, `composicao` INT NOT NULL, `qtde` FLOAT, PRIMARY KEY (`composicao`,`item`,`movto`))",
                "CREATE TABLE IF NOT EXISTS `contaspr` (`Numero` INT NOT NULL, `DataLacto` DATETIME, `DataVencto` DATETIME, `Clifor` INT, `Valor` FLOAT, `Baixado` TINYINT(1), `DataPagto` DATETIME, `Juros` FLOAT, `Docto` VARCHAR(20), `Tipo` VARCHAR(2), `Obs` VARCHAR(50), PRIMARY KEY (`Numero`))",
                "CREATE TABLE IF NOT EXISTS `Cores` (`Codigo` INT NOT NULL, `CorO` VARCHAR(20), `CorD` VARCHAR(20), `CorL` VARCHAR(20), `CorS` VARCHAR(20), `CorC` VARCHAR(20), `CorG` VARCHAR(20), `CorM` VARCHAR(20), `CorFO` VARCHAR(20), `CorFD` VARCHAR(20), `CorFL` VARCHAR(20), `CorFS` VARCHAR(20), `CorFC` VARCHAR(20), `CorFG` VARCHAR(20), `CorFM` VARCHAR(20), `RelD` VARCHAR(20), `RelO` VARCHAR(20), PRIMARY KEY (`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `CtrlCupom` (`Numero` INT NOT NULL, `DataEntrega` DATETIME, `HoraEntrega` DATETIME, `SuiteOrigem` INT, `PermanenciaOrigem` DATETIME, `UsuarioEntrega` INT, `ValorOrigem` FLOAT, `DataRetorno` DATETIME, `Horaretorno` DATETIME, `SuiteRetorno` INT, `PermanenciaRetorno` DATETIME, `UsuarioRecebe` INT, `ValorRetorno` FLOAT, `StsFechado` TINYINT(1), PRIMARY KEY (`Numero`))",
                "CREATE TABLE IF NOT EXISTS `CtrlReposicoes` (`suite` INT NOT NULL, `item` INT NOT NULL, `movto` INT NOT NULL, `qtde` FLOAT, `reposicao` TINYINT(1), PRIMARY KEY (`movto`,`item`,`suite`))",
                "CREATE TABLE IF NOT EXISTS `CtrlS` (`Sigla` VARCHAR(6) NOT NULL, PRIMARY KEY (`Sigla`))",
                "CREATE TABLE IF NOT EXISTS `CtrlSuite` (`Sigla` VARCHAR(6) NOT NULL, PRIMARY KEY (`Sigla`))",
                "CREATE TABLE IF NOT EXISTS `Descontos` (`Movto` INT NOT NULL, `Solicitante` VARCHAR(50), `Motivo` VARCHAR(255), `usuario` INT, `desctempo` DATETIME, `CodMotivo` INT, PRIMARY KEY (`Movto`))",
                "CREATE TABLE IF NOT EXISTS `DescontosPro` (`Codigo` INT NOT NULL, `Descricao` VARCHAR(30), `Validade` DATETIME, `Percentual` FLOAT, `TipoDesc` VARCHAR(2), `CBar` TINYINT(1), `DiaInicial` INT, `DiaFinal` INT, `HoraInicial` DATETIME, `HoraFinal` DATETIME, PRIMARY KEY (`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `Despesas` (`Controle` INT NOT NULL, `Caixa` INT, `TipoDespesas` INT, `Valor` FLOAT, PRIMARY KEY (`Controle`))",
                "CREATE TABLE IF NOT EXISTS `DetReq` (`ctrl` INT NOT NULL, `item` INT NOT NULL, `qtde` FLOAT, `qtde2` FLOAT, `unitario` FLOAT, `total` FLOAT, PRIMARY KEY (`item`,`ctrl`))",
                "CREATE TABLE IF NOT EXISTS `diareposicao` (`dia` INT NOT NULL, `hora` DATETIME, `aviso` TINYINT(1), PRIMARY KEY (`dia`))",
                "CREATE TABLE IF NOT EXISTS `ecf` (`numero` INT NOT NULL, `modelo` TINYINT UNSIGNED, `versao` VARCHAR(5), PRIMARY KEY (`numero`))",
                "CREATE TABLE IF NOT EXISTS `Especies` (`Codigo` INT NOT NULL, `Nome` VARCHAR(50), `Vendas` TINYINT(1), PRIMARY KEY (`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `EstornoConsumo` (`movto` INT NOT NULL, `item` INT NOT NULL, `horaped` DATETIME NOT NULL, `hora` DATETIME NOT NULL, `qtde` FLOAT, `usuario` INT, PRIMARY KEY (`hora`,`horaped`,`item`,`movto`))",
                "CREATE TABLE IF NOT EXISTS `Feriados` (`DataFer` DATETIME NOT NULL, `DescricaoFer` VARCHAR(50), `FerLocal` TINYINT(1), PRIMARY KEY (`DataFer`))",
                "CREATE TABLE IF NOT EXISTS `fornecedores` (`Codigo` INT NOT NULL, `Nome` VARCHAR(50), `Endereco` VARCHAR(50), `Complemento` VARCHAR(30), `Bairro` VARCHAR(30), `Cidade` VARCHAR(30), `UF` VARCHAR(2), `Telefone` VARCHAR(20), `Observacao` VARCHAR(100), `Telefone2` VARCHAR(15), PRIMARY KEY (`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `funcionarios` (`codigo` INT NOT NULL, `nome` VARCHAR(50), `funcao` VARCHAR(50), `Senha` VARCHAR(15), `emservico` TINYINT(1), `senhaponto` VARCHAR(10), `hora1` DATETIME, `hora2` DATETIME, `hora3` DATETIME, `hora4` DATETIME, `LimiteVale` FLOAT, PRIMARY KEY (`codigo`))",
                "CREATE TABLE IF NOT EXISTS `Historicos` (`codigo` INT NOT NULL, `descricao` VARCHAR(30), `tipo` VARCHAR(1), PRIMARY KEY (`codigo`))",
                "CREATE TABLE IF NOT EXISTS `itens` (`codigo` SMALLINT NOT NULL, `descricao` VARCHAR(50), `unidade` VARCHAR(3), `categoria` SMALLINT, `tributacao` SMALLINT, `aliquota` FLOAT, `venda` FLOAT, `custo` FLOAT, `tipo` SMALLINT, `vendamaxima` FLOAT, `devolucao` VARCHAR(255), `especie` INT, `minimo` FLOAT, `maximo` FLOAT, `itemadm` INT, `frigobar` FLOAT, `servico` TINYINT(1), `retornavel` TINYINT(1), `composicaoopcional` TINYINT(1), `produtobrinde` TINYINT(1), `CodGTIN` VARCHAR(12), `Consumacao` TINYINT(1), `CFOP` INT, `NCM` INT, `expromesp` TINYINT(1), `CEst` INT, `cardapioonline` TINYINT(1), `foto` VARCHAR(255), `alteraupload` TINYINT(1), `inativo` TINYINT(1), `QtdeImp1` FLOAT, `QtdeImp2` FLOAT, `QtdeImp3` FLOAT, `ialterado` TINYINT(1), PRIMARY KEY (`codigo`))",
                "CREATE TABLE IF NOT EXISTS `itenstemp` (`item` INT NOT NULL, `qtde1` FLOAT, `qtde2` FLOAT, `qtde3` FLOAT, `qtde4` FLOAT, PRIMARY KEY (`item`))",
                "CREATE TABLE IF NOT EXISTS `ItensVsSuites` (`Categoria` VARCHAR(10) NOT NULL, `Item` INT NOT NULL, `HsIni` DATETIME, `HsFin` DATETIME, PRIMARY KEY (`Item`,`Categoria`))",
                "CREATE TABLE IF NOT EXISTS `limpeza` (`limpeza` INT NOT NULL, `entrada` DATETIME, `entradahs` VARCHAR(5), `saida` DATETIME, `saidahs` VARCHAR(5), `turno` SMALLINT, `tempo` FLOAT, `limpezaini` DATETIME, `camareira` INT, `Grau` VARCHAR(1), `limpezatmp` VARCHAR(10), `CodSupervisor` INT, `camareira2` INT, `tempo1` DATETIME, `tempo2` DATETIME, `limpezaini2` DATETIME, `limpezafim` DATETIME, `limpezadataini` DATETIME, `limpezafim1` DATETIME, `Limpezafim2` DATETIME, PRIMARY KEY (`limpeza`))",
                "CREATE TABLE IF NOT EXISTS `limpezarev` (`controle` INT NOT NULL, `suite` INT, `dataini` DATETIME, `datafim` DATETIME, `horaini` DATETIME, `horafim` DATETIME, `usuario` INT, `funcionario` INT, `Motivo` INT, `Observacao` VARCHAR(50), `MovtoOrigem` INT, `temporev` DATETIME, PRIMARY KEY (`controle`))",
                "CREATE TABLE IF NOT EXISTS `LogSistema` (`Data` DATETIME NOT NULL, `Hora` DATETIME NOT NULL, `Tipo` VARCHAR(1), `Descricao` VARCHAR(255), PRIMARY KEY (`Hora`,`Data`))",
                "CREATE TABLE IF NOT EXISTS `LotesCupons` (`Lote` INT NOT NULL, `Cupomini` INT, `Cupomfin` INT, `Validade` DATETIME, `CodPromocao` INT, PRIMARY KEY (`Lote`))",
                "CREATE TABLE IF NOT EXISTS `Manutencao` (`movto` INT NOT NULL, `Suite` INT, `DataI` DATETIME, `HoraI` VARCHAR(10), `DataF` DATETIME, `HoraF` VARCHAR(10), `Corretiva` TINYINT(1), `Descricao` VARCHAR(250), `Liberacao` VARCHAR(1), `ocorrencia` INT, `funcionario` INT, `usuario` INT, `codmotivo` INT, PRIMARY KEY (`movto`))",
                "CREATE TABLE IF NOT EXISTS `MotivoDes` (`Codigo` INT NOT NULL, `Descricao` VARCHAR(50), PRIMARY KEY (`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `MotivoRev` (`Codigo` INT NOT NULL, `Descricao` VARCHAR(30), PRIMARY KEY (`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `MovPedido` (`Mov` INT, `Data` DATETIME, `hora` VARCHAR(50), `suite` INT, `prod` INT, `prodesc` VARCHAR(255), `qnt` INT)",
                "CREATE TABLE IF NOT EXISTS `movtos` (`numero` INT NOT NULL, `tipo` SMALLINT NOT NULL, `serie` VARCHAR(2), `data` DATETIME, `total` FLOAT, PRIMARY KEY (`tipo`,`numero`))",
                "CREATE TABLE IF NOT EXISTS `movtos2` (`movto` SMALLINT NOT NULL, `tipo` SMALLINT NOT NULL, `item` SMALLINT NOT NULL, `qtde` FLOAT, `valor` FLOAT, `total` FLOAT, PRIMARY KEY (`item`,`tipo`,`movto`))",
                "CREATE TABLE IF NOT EXISTS `MovtosEstoque` (`codigo` INT NOT NULL, `Data` DATETIME, `Item` INT, `Tipo` VARCHAR(1), `Qtde` FLOAT, `Valor` FLOAT, `Nf` INT, `Obs` VARCHAR(250), `usuario` INT, `area` INT, `caixa` INT, `historico` INT, `fornecedor` INT, `Agrupado` TINYINT(1), PRIMARY KEY (`codigo`))",
                "CREATE TABLE IF NOT EXISTS `movtosq` (`numero` INT NOT NULL, `suite` SMALLINT, `entrada` DATETIME, `entradahs` VARCHAR(5), `saida` DATETIME, `saidahs` VARCHAR(5), `tempo` VARCHAR(10), `valor` FLOAT, `pernoite` FLOAT, `desconto` FLOAT, `casais` FLOAT, `qcasais` SMALLINT, `frigobar` FLOAT, `bar` FLOAT, `cozinha` FLOAT, `turno` SMALLINT, `usuario` SMALLINT, `caixa` DATETIME, `pagto` VARCHAR(2), `operadora` SMALLINT, `placa` VARCHAR(10), `flag` TINYINT(1), `sinal` FLOAT, `pdv` TINYINT(1), `horaconf` VARCHAR(10), `totalDI` FLOAT, `totalCH` FLOAT, `totalCC` FLOAT, `nrcaixa` INT, `nrdias` FLOAT, `faltaimpfiscal` TINYINT(1), `usuariofec` INT, `txReserva` FLOAT, `txOutras` FLOAT, `dataredz` DATETIME, `totalpe` FLOAT, `CPF` VARCHAR(18), `condpg` VARCHAR(2), `prefechamento` TINYINT(1), `ligacoes` FLOAT, `nrreserva` INT, `stsECF` VARCHAR(255), `reducaoz` INT, `coo` INT, `redznf` INT, `NrECF` INT, `desctempo` DATETIME, `qtdepessoas` INT, `QtdeFeminino` INT, `qcasaisf` INT, `obs` VARCHAR(50), `placa2` VARCHAR(8), `placa3` VARCHAR(8), `placa4` VARCHAR(8), `placa5` VARCHAR(8), `Controle` INT, `funcrec` INT, `Resumo2via` TINYINT(1), `CupomOrigem` INT, `CupomDesconto` INT, `Veiculo` VARCHAR(15), `DescOutrasTaxas` VARCHAR(30), `TempoGaragem` DATETIME, `TempoPatio` DATETIME, `NrCaixaP` INT, `SuiteOrigem` INT, `cartaovip` INT, `TotalVale` FLOAT, `diaria` TINYINT(1), PRIMARY KEY (`numero`))",
                "CREATE TABLE IF NOT EXISTS `MovtosqECF` (`numeromecf` INT NOT NULL, `faltaimpfiscal` TINYINT(1), `flag` TINYINT(1), `dataredz` DATETIME, `reducaoz` INT, `coo` INT, `redznf` INT, `nrecf` INT, `cpfcnpj` VARCHAR(20), PRIMARY KEY (`numeromecf`))",
                "CREATE TABLE IF NOT EXISTS `movtosqR` (`numeromv` INT NOT NULL, `datamv` DATETIME NOT NULL, `horamv` DATETIME NOT NULL, `usuariomv` INT, PRIMARY KEY (`horamv`,`datamv`,`numeromv`))",
                "CREATE TABLE IF NOT EXISTS `movtosRec` (`MovtoRec` INT NOT NULL, `TotalDIRec` FLOAT, `TotalCCRec` FLOAT, `TotalCHRec` FLOAT, `TotalPERec` FLOAT, `OpeRec` INT, PRIMARY KEY (`MovtoRec`))",
                "CREATE TABLE IF NOT EXISTS `MovtoVip` (`Controle` INT NOT NULL, `Cliente` INT, `ValorConta` FLOAT, `Dataconta` DATETIME, `Status` VARCHAR(1), `DataReceb` DATETIME, PRIMARY KEY (`Controle`))",
                "CREATE TABLE IF NOT EXISTS `MRV` (`numero` INT NOT NULL, `reducoes` INT, `data` DATETIME, `cooini` INT, `coofin` INT, `gt` FLOAT, `venda` FLOAT, `cancela` FLOAT, `cancelaT` FLOAT, `cancelaISS` FLOAT, `descFIN` FLOAT, `descT` FLOAT, `descISS` FLOAT, `ISS` FLOAT, `ValCont` FLOAT, `Substituicao` FLOAT, `Isentas` FLOAT, `Outras` FLOAT, PRIMARY KEY (`numero`))",
                "CREATE TABLE IF NOT EXISTS `negativo` (`cpf` VARCHAR(20) NOT NULL, `data` DATETIME NOT NULL, `placa` VARCHAR(10), `nome` VARCHAR(50), `valor` FLOAT, `nrch` INT, `bco` INT, `usuario` INT, `datareg` DATETIME, `alinea` INT, PRIMARY KEY (`data`,`cpf`))",
                "CREATE TABLE IF NOT EXISTS `NFCe` (`NumeroNF` INT NOT NULL, `MovtoNF` INT, `ReciboNF` VARCHAR(50), `ProtocoloNF` VARCHAR(50), `ChaveNFCe` VARCHAR(50), `BaseCalculoNFCe` FLOAT, `ImpostoNFCe` FLOAT, `SubstTribNFCe` FLOAT, `IsentasNFCe` FLOAT, `TotalNFCe` FLOAT, `Dataemissao` DATETIME, `Horaemissao` DATETIME, PRIMARY KEY (`NumeroNF`))",
                "CREATE TABLE IF NOT EXISTS `NiveisAcesso` (`Nivel` INT NOT NULL, `Opcao` VARCHAR(30) NOT NULL, `Acesso` TINYINT(1), `Descricao` VARCHAR(50), `ExclusaoPedido` INT, PRIMARY KEY (`Opcao`,`Nivel`))",
                "CREATE TABLE IF NOT EXISTS `NiveisCtrl` (`Nivel` INT NOT NULL, `DescontoMaxValor` FLOAT, PRIMARY KEY (`Nivel`))",
                "CREATE TABLE IF NOT EXISTS `OcorrenciaCli` (`PlacaOc` VARCHAR(10) NOT NULL, `dataOc` DATETIME, `UsuarioOc` INT, `DescricaoOc` VARCHAR(250), PRIMARY KEY (`PlacaOc`))",
                "CREATE TABLE IF NOT EXISTS `ocorrencias` (`ctrl` INT NOT NULL, `dataoc` DATETIME, `horaoc` DATETIME, `usuario` INT, `suite` INT, `descricao` VARCHAR(250), `tipo` INT, PRIMARY KEY (`ctrl`))",
                "CREATE TABLE IF NOT EXISTS `operadoras` (`codigo` SMALLINT NOT NULL, `nome` VARCHAR(40), `ContaCX` VARCHAR(30), `Cartaodebito` TINYINT(1), `CodNFCE` INT, PRIMARY KEY (`codigo`))",
                "CREATE TABLE IF NOT EXISTS `PDV` (`codigo` INT NOT NULL, `data` DATETIME, `usuario` INT, `horafec` DATETIME, `requisicao` INT, `exportacao` TINYINT(1), `Valor` INT, `Pernoite` INT, `Descontos` INT, `Vendas` INT, `integrado` TINYINT(1), PRIMARY KEY (`codigo`))",
                "CREATE TABLE IF NOT EXISTS `PDVSLD` (`Caixa` INT NOT NULL, `Item` INT NOT NULL, `Area` INT NOT NULL, `Saldo` FLOAT, `pedidos` FLOAT, PRIMARY KEY (`Area`,`Item`,`Caixa`))",
                "CREATE TABLE IF NOT EXISTS `Pedidos` (`Id` INT NOT NULL, `Suite` INT, `Data` DATETIME, `Hora` VARCHAR(50), `Status` VARCHAR(20), `movto` INT, `item` INT, `qtd` INT, `webalt` TINYINT(1), `nwstatus` VARCHAR(255), `obs` VARCHAR(255), `imprimi` TINYINT(1), `interno` TINYINT(1), PRIMARY KEY (`Id`))",
                "CREATE TABLE IF NOT EXISTS `Penhoras` (`Numero` INT NOT NULL, `DataPen` DATETIME, `DataEnt` DATETIME, `DescPen` VARCHAR(30), `ValorPen` FLOAT, PRIMARY KEY (`Numero`))",
                "CREATE TABLE IF NOT EXISTS `PernoiteDif` (`Categ` VARCHAR(5) NOT NULL, `PernoiteIni` DATETIME, `PernoiteFin` DATETIME, `TempoPernoite` DATETIME, `pernoitefs` TINYINT(1), `pernoiteinifs` DATETIME, `pernoitefinfs` DATETIME, `tempopernoitefs` DATETIME, `PerDifFS` TINYINT(1), PRIMARY KEY (`Categ`))",
                "CREATE TABLE IF NOT EXISTS `Promocao` (`tabela` INT NOT NULL, `categoria` VARCHAR(4) NOT NULL, `Tipop` VARCHAR(1), `Tempo` DATETIME, `HoraInicial` DATETIME, `HoraFinal` DATETIME, `Tempominimo` DATETIME, `Tempomaximo` DATETIME, `Valortempo` FLOAT, `Valorperiodo` FLOAT, `CasalAdicional` TINYINT(1), `QtdePessoas` INT, `ValorPernoite` FLOAT, `Adicionalhs` FLOAT, PRIMARY KEY (`categoria`,`tabela`))",
                "CREATE TABLE IF NOT EXISTS `PromocaoItem` (`Codigo` INT NOT NULL, `Vigencia` DATETIME, `Tempo` VARCHAR(50), `PN` VARCHAR(20), `Valor` FLOAT, PRIMARY KEY (`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `PromocaoItemMov` (`Item` INT, `Descricao` VARCHAR(50), `Promocao` INT)",
                "CREATE TABLE IF NOT EXISTS `ReducoesZ` (`DataRed` DATETIME NOT NULL, `HoraRed` VARCHAR(10), `DataMovto` DATETIME, PRIMARY KEY (`DataRed`))",
                "CREATE TABLE IF NOT EXISTS `RegPonto` (`Funcionario` INT NOT NULL, `Data` DATETIME NOT NULL, `Hora1` DATETIME, `Hora2` DATETIME, `Hora3` DATETIME, `Hora4` DATETIME, `Obs` VARCHAR(50), PRIMARY KEY (`Data`,`Funcionario`))",
                "CREATE TABLE IF NOT EXISTS `Requisicoes` (`ctrl` INT NOT NULL, `data` DATETIME, `hora` DATETIME, `nrcaixa` INT, `area` INT, `fechado` TINYINT(1), `pedido` INT, `histmovto` INT, `ValorTotal` FLOAT, PRIMARY KEY (`ctrl`))",
                "CREATE TABLE IF NOT EXISTS `ReqWebService` (`id` INT NOT NULL, `status` VARCHAR(50), PRIMARY KEY (`id`))",
                "CREATE TABLE IF NOT EXISTS `Reservas2` (`numero` INT NOT NULL, `cliente` VARCHAR(20), `categoria` VARCHAR(10), `usuario` INT, `dtemissao` DATETIME, `dtreserva` DATETIME, `hsreserva` VARCHAR(10), `periodo` VARCHAR(10), `Adiantamento` FLOAT, `Servicos` FLOAT, `nrform` INT, `nrcaixa` INT, `fechado` TINYINT(1), `Formapagto` VARCHAR(2), `qtdediaria` INT, `agenciadoc` VARCHAR(10), `contadoc` VARCHAR(20), `Tolerancia` INT, `nova` INT, `Observacao` VARCHAR(255), `ValorAdP` FLOAT, `CreditoFidGuia` FLOAT, `DescontoGuia` FLOAT, `Cancelada` TINYINT(1), `Enviada` TINYINT(1), PRIMARY KEY (`numero`))",
                "CREATE TABLE IF NOT EXISTS `RetornoPortao` (`Data` DATETIME NOT NULL, `Hora` DATETIME NOT NULL, `Motivo` VARCHAR(50), `Usuario` INT, PRIMARY KEY (`Hora`,`Data`))",
                "CREATE TABLE IF NOT EXISTS `RoupasTmp` (`codigo` INT NOT NULL, `suite` INT NOT NULL, `qtde` FLOAT, PRIMARY KEY (`suite`,`codigo`))",
                "CREATE TABLE IF NOT EXISTS `slditens` (`item` SMALLINT NOT NULL, `mes` DATETIME NOT NULL, `entradas` FLOAT, `saidas` FLOAT, `saldo` FLOAT, PRIMARY KEY (`mes`,`item`))",
                "CREATE TABLE IF NOT EXISTS `slditens2` (`item` INT NOT NULL, `mes` DATETIME NOT NULL, `area` INT NOT NULL, `entradas` FLOAT, `saidas` FLOAT, `saldo` FLOAT, PRIMARY KEY (`area`,`mes`,`item`))",
                "CREATE TABLE IF NOT EXISTS `st_guia` (`idguia` INT NOT NULL, `suite` VARCHAR(40), PRIMARY KEY (`idguia`))",
                "CREATE TABLE IF NOT EXISTS `suites` (`numero` SMALLINT NOT NULL, `sigla` VARCHAR(4), `nome` VARCHAR(20), `valor` FLOAT, `adicional` FLOAT, `tempo` DATETIME, `valorp` FLOAT, `adicionalp` FLOAT, `tempop` DATETIME, `situacao` VARCHAR(1), `movto` INT, `tipo` VARCHAR(1), `pernoite` FLOAT, `pernoitep` FLOAT, `movtolimpeza` INT, `ultimodia` DATETIME, `ultimaHS` DATETIME, `area` INT, `codigoecxus` INT, `statusE` TINYINT(1), `Verificado` TINYINT(1), `flag` TINYINT UNSIGNED, `Travado` TINYINT(1), `DataLembrete` DATETIME, `HoraLembrete` DATETIME, `Lembrete` VARCHAR(50), `HoraOc` DATETIME, `DataOc` DATETIME, `CtrlCascata` TINYINT(1), `CtrlVolume` TINYINT(1), `PercAdicional` FLOAT, `qtdepessoas` INT, `PercAdicionalF` FLOAT, `aguardando` TINYINT(1), `limpeza` TINYINT(1), `prioridade` TINYINT(1), `codaut` VARCHAR(10), `Observacoes` VARCHAR(250), `Interditada` TINYINT(1), `Saquevip` TINYINT(1), `bloqueiatc` TINYINT(1), `contrato` TINYINT(1), `TempoLimpeza` DATETIME, `TempoRevLimp` DATETIME, `AtAutomatico` TINYINT(1), `Climatizada` TINYINT(1), `End2` INT, `End3` INT, `Qrcode` VARCHAR(255), `salterado` TINYINT(1), `ReservaGuia` INT, `ValidacaoGuia` TINYINT(1), `Programada` TINYINT(1), PRIMARY KEY (`numero`))",
                "CREATE TABLE IF NOT EXISTS `tabelapernoite` (`dia` INT NOT NULL, `horaini` DATETIME, `horafim` DATETIME, `tempoper` DATETIME, `horaini2` DATETIME, `horafim2` DATETIME, `TempoMax` DATETIME, PRIMARY KEY (`dia`))",
                "CREATE TABLE IF NOT EXISTS `tarifacao` (`sigla` VARCHAR(4) NOT NULL, `Tipo` VARCHAR(1), `avaliacao` VARCHAR(1), `valor` FLOAT, PRIMARY KEY (`sigla`))",
                "CREATE TABLE IF NOT EXISTS `TarifacaoCat` (`Codigo` INT NOT NULL, `Categoria` VARCHAR(5) NOT NULL, `Periodo` FLOAT, `Adicional` FLOAT, `Pernoite` FLOAT, `Tempo` DATETIME, `ES_Cat` VARCHAR(1), `prioritaria` TINYINT(1), `adicionalvariavel` TINYINT(1), `PessoaAdDif` TINYINT(1), `ValorPessoa` FLOAT, `MultiploDe` INT, `Pernoite2` FLOAT, `AdicionalPessoa` FLOAT, `ValorDiaria` FLOAT, `AdicionalHora` INT, PRIMARY KEY (`Categoria`,`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `TarifacaoCat2` (`codigo` INT NOT NULL, `categoria` VARCHAR(5) NOT NULL, `hora` INT NOT NULL, `valor` FLOAT, PRIMARY KEY (`hora`,`categoria`,`codigo`))",
                "CREATE TABLE IF NOT EXISTS `TarifacaoSuite` (`DiaT` INT NOT NULL, `HoraT` INT NOT NULL, `Codigo` INT, PRIMARY KEY (`HoraT`,`DiaT`))",
                "CREATE TABLE IF NOT EXISTS `TarifacaoTab` (`Codigo` INT NOT NULL, `Descricao` VARCHAR(50), PRIMARY KEY (`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `TipoDespesas` (`Codigo` INT NOT NULL, `Descricao` VARCHAR(50), PRIMARY KEY (`Codigo`))",
                "CREATE TABLE IF NOT EXISTS `usuarios` (`codigo` SMALLINT NOT NULL, `nome` VARCHAR(30), `senha` VARCHAR(20), `nivel` SMALLINT, `Supervisor` TINYINT(1), `inativo` TINYINT(1), PRIMARY KEY (`codigo`))",
                "CREATE TABLE IF NOT EXISTS `VerifCtrl` (`idguia` INT NOT NULL, `suite` VARCHAR(40), PRIMARY KEY (`idguia`))",
                "CREATE TABLE IF NOT EXISTS `Versao` (`Codigo` INT NOT NULL,`NrVersao` VARCHAR(5),`dataV` DATETIME, PRIMARY KEY (`Codigo`));",
                "CREATE TABLE IF NOT EXISTS Sigla (codigo INT NOT NULL AUTO_INCREMENT PRIMARY KEY,sigla VARCHAR(10) UNIQUE,txadd DECIMAL(10,2),qtdpessoa INT,bloqueiavi BOOLEAN,emissao BOOLEAN);",
                "ALTER TABLE suites DROP COLUMN valor;",
                "ALTER TABLE suites DROP COLUMN adicional;",
                "ALTER TABLE suites DROP COLUMN tempo;",
                "ALTER TABLE suites DROP COLUMN valorp;",
                "ALTER TABLE suites DROP COLUMN adicionalp;",
                "ALTER TABLE suites DROP COLUMN tempop;",
                "ALTER TABLE suites DROP COLUMN tipo;",
                "ALTER TABLE suites DROP COLUMN pernoite;",
                "ALTER TABLE suites DROP COLUMN pernoitep;",
                "ALTER TABLE suites DROP COLUMN bloqueiatc;",
                "ALTER TABLE suites DROP COLUMN contrato;",
                "ALTER TABLE suites DROP COLUMN qrcode;",
                "ALTER TABLE movtosq MODIFY COLUMN entradahs TIME;",
                "ALTER TABLE movtosq MODIFY COLUMN saidahs TIME;",
                "ALTER TABLE movtosq MODIFY COLUMN tempo TIME;",
                "ALTER TABLE movtosq MODIFY COLUMN entrada DATE;",
                "ALTER TABLE movtosq MODIFY COLUMN saida DATE;",
                "ALTER TABLE slditens2 MODIFY COLUMN mes DATE;",
                "ALTER TABLE suites MODIFY COLUMN sigla INT;"
            }
        End Get
    End Property

End Class
