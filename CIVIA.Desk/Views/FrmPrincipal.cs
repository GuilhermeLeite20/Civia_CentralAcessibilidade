using System.Drawing;
using System.Windows.Forms;
using CiviaDesk.Models;
using CiviaDesk.Services;

namespace CiviaDesk.Views
{
    public class FrmPrincipal : Form
    {
        private readonly RegistroService _servico;
        private readonly Panel _areaPrincipal;
        private readonly Label _lblTitulo;
        private readonly Label _lblDescricao;
        private readonly Label _lblRodape;
        private readonly List<Button> _menu = new();

        private DataGridView? _gridRegistros;
        private TextBox? _txtBusca;
        private ComboBox? _cmbFiltro;
        private Label? _lblQuantidade;

        public FrmPrincipal()
        {
            _servico = new RegistroService();

            Text = "CIVIA Desk | Central de Acessibilidade";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1220, 770);
            MinimumSize = new Size(1020, 680);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = CiviaUI.Fundo;
            Font = new Font("Segoe UI", 10);

            var estrutura = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            estrutura.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 232));
            estrutura.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            Controls.Add(estrutura);

            var barraLateral = CriarBarraLateral();
            estrutura.Controls.Add(barraLateral, 0, 0);

            var corpo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = CiviaUI.Fundo,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            corpo.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            corpo.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            corpo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            estrutura.Controls.Add(corpo, 1, 0);

            var cabecalho = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            corpo.Controls.Add(cabecalho, 0, 0);

            _lblTitulo = CiviaUI.TextoLabel("Visão geral", 21, true);
            _lblTitulo.Location = new Point(28, 16);
            cabecalho.Controls.Add(_lblTitulo);

            _lblDescricao = CiviaUI.TextoLabel("Central comunitária de acessibilidade urbana", 9.5f);
            _lblDescricao.ForeColor = CiviaUI.Secundario;
            _lblDescricao.Location = new Point(29, 61);
            cabecalho.Controls.Add(_lblDescricao);

            var novo = CiviaUI.Botao("+ Novo registro", destaque: true, largura: 170);
            novo.Top = 29;
            novo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            novo.Click += (_, _) => AbrirCadastro();
            cabecalho.Controls.Add(novo);
            cabecalho.Resize += (_, _) => novo.Left = cabecalho.ClientSize.Width - novo.Width - 28;
            novo.Left = 690;

            _areaPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CiviaUI.Fundo,
                Padding = new Padding(26, 22, 26, 22)
            };
            corpo.Controls.Add(_areaPrincipal, 0, 1);

            var rodape = new Panel { Dock = DockStyle.Fill, BackColor = CiviaUI.Branco };
            _lblRodape = CiviaUI.TextoLabel("Dados armazenados apenas neste computador.", 8.5f);
            _lblRodape.ForeColor = CiviaUI.Secundario;
            _lblRodape.Location = new Point(28, 9);
            rodape.Controls.Add(_lblRodape);
            corpo.Controls.Add(rodape, 0, 2);

            MostrarVisaoGeral();
        }

        private Panel CriarBarraLateral()
        {
            var barra = new Panel { Dock = DockStyle.Fill, BackColor = CiviaUI.Escuro };

            var marca = CiviaUI.TextoLabel("CIVIA", 25, true);
            marca.ForeColor = Color.White;
            marca.Location = new Point(23, 30);
            barra.Controls.Add(marca);

            var edicao = CiviaUI.TextoLabel("DESK  /  PROTÓTIPO", 8, true);
            edicao.ForeColor = Color.FromArgb(236, 102, 153);
            edicao.Location = new Point(25, 78);
            barra.Controls.Add(edicao);

            var divisoria = new Panel
            {
                Height = 1,
                Width = 188,
                Left = 22,
                Top = 126,
                BackColor = Color.FromArgb(67, 73, 88)
            };
            barra.Controls.Add(divisoria);

            var inicio = CriarBotaoMenu("Visão geral", 150, MostrarVisaoGeral);
            var registros = CriarBotaoMenu("Registros", 204, MostrarRegistros);
            var sobre = CriarBotaoMenu("Sobre o CIVIA", 258, MostrarSobre);
            barra.Controls.Add(inicio);
            barra.Controls.Add(registros);
            barra.Controls.Add(sobre);

            var info = new Label
            {
                Text = "CIVIA  •  CitizenTech\nVersão acadêmica 0.1.0",
                Font = new Font("Segoe UI", 8.3f),
                ForeColor = Color.FromArgb(176, 184, 201),
                Location = new Point(23, 0),
                Size = new Size(190, 48),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            barra.Controls.Add(info);
            barra.Resize += (_, _) => info.Top = barra.ClientSize.Height - 72;
            return barra;
        }

        private Button CriarBotaoMenu(string titulo, int y, Action aoClicar)
        {
            var botao = new Button
            {
                Text = titulo,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Location = new Point(16, y),
                Size = new Size(200, 44),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                BackColor = CiviaUI.Escuro,
                ForeColor = Color.FromArgb(225, 229, 238),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            botao.FlatAppearance.BorderSize = 0;
            botao.Click += (_, _) =>
            {
                DestacarMenu(botao);
                aoClicar();
            };
            _menu.Add(botao);
            return botao;
        }

        private void DestacarMenu(int indice)
        {
            if (_menu.Count > indice)
                DestacarMenu(_menu[indice]);
        }

        private void DestacarMenu(Button selecionado)
        {
            foreach (var b in _menu)
            {
                b.BackColor = b == selecionado ? Color.FromArgb(57, 44, 65) : CiviaUI.Escuro;
                b.ForeColor = b == selecionado ? Color.White : Color.FromArgb(225, 229, 238);
                b.Font = new Font("Segoe UI", 10,
                    b == selecionado ? FontStyle.Bold : FontStyle.Regular);
            }
        }

        private void ExibirPagina(Control pagina, string titulo, string descricao)
        {
            _lblTitulo.Text = titulo;
            _lblDescricao.Text = descricao;
            _areaPrincipal.Controls.Clear();
            pagina.Dock = DockStyle.Fill;
            _areaPrincipal.Controls.Add(pagina);
        }

        private void MostrarVisaoGeral()
        {
            DestacarMenu(0);
            _gridRegistros = null;

            var container = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                BackColor = CiviaUI.Fundo,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            container.RowStyles.Add(new RowStyle(SizeType.Absolute, 144));
            container.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            container.RowStyles.Add(new RowStyle(SizeType.Absolute, 154));

            var todos = _servico.Listar();
            var estatisticas = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = new Padding(0, 0, 0, 14)
            };
            for (int i = 0; i < 4; i++)
                estatisticas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            estatisticas.Controls.Add(CriarIndicador("REGISTROS", todos.Count, CiviaUI.Texto), 0, 0);
            estatisticas.Controls.Add(CriarIndicador("BARREIRAS PENDENTES", todos.Count(r =>
                r.Tipo == TipoRegistro.BarreiraUrbana && r.Status != StatusRegistro.Concluido), CiviaUI.Destaque), 1, 0);
            estatisticas.Controls.Add(CriarIndicador("PONTOS DE APOIO", todos.Count(r =>
                r.Tipo == TipoRegistro.PontoApoio), CiviaUI.Verde), 2, 0);
            estatisticas.Controls.Add(CriarIndicador("CONCLUÍDOS", todos.Count(r =>
                r.Status == StatusRegistro.Concluido), CiviaUI.Texto), 3, 0);
            container.Controls.Add(estatisticas, 0, 0);

            var recentes = CiviaUI.Cartao();
            recentes.Dock = DockStyle.Fill;
            recentes.Margin = new Padding(0, 0, 0, 14);
            var estruturaRecentes = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Margin = Padding.Empty
            };
            estruturaRecentes.RowStyles.Add(new RowStyle(SizeType.Absolute, 43));
            estruturaRecentes.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            var tituloTabela = CiviaUI.TextoLabel("Registros recentes", 13, true);
            tituloTabela.AutoSize = false;
            tituloTabela.Dock = DockStyle.Fill;
            estruturaRecentes.Controls.Add(tituloTabela, 0, 0);
            var tabela = CriarTabela();
            tabela.Dock = DockStyle.Fill;
            estruturaRecentes.Controls.Add(tabela, 0, 1);
            recentes.Controls.Add(estruturaRecentes);
            PreencherTabela(tabela, todos.Take(6));
            container.Controls.Add(recentes, 0, 1);

            var convite = CiviaUI.Cartao();
            convite.Dock = DockStyle.Fill;
            convite.Margin = Padding.Empty;
            var tituloConvite = CiviaUI.TextoLabel("Uma cidade mais acessível começa pela informação.", 13, true);
            tituloConvite.Location = new Point(22, 22);
            convite.Controls.Add(tituloConvite);
            var textoConvite = new Label
            {
                Text = "Cadastre barreiras urbanas e pontos de apoio, acompanhe a situação e consulte indicadores. " +
                       "Esta é uma simulação local do painel cidadão da CIVIA.",
                Location = new Point(22, 57),
                Size = new Size(640, 49),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = CiviaUI.Secundario
            };
            convite.Controls.Add(textoConvite);
            var exemplos = CiviaUI.Botao("Inserir exemplos fictícios", largura: 218);
            exemplos.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            convite.Controls.Add(exemplos);
            convite.Resize += (_, _) =>
            {
                exemplos.Location = new Point(convite.ClientSize.Width - 242, convite.ClientSize.Height - 66);
                textoConvite.Width = Math.Max(200, convite.ClientSize.Width - 310);
            };
            exemplos.Click += (_, _) => InserirExemplos();
            container.Controls.Add(convite, 0, 2);

            ExibirPagina(container, "Visão geral", "Registros locais que apoiam uma cidade mais acessível.");
        }

        private Panel CriarIndicador(string descricao, int valor, Color cor)
        {
            var cartao = CiviaUI.Cartao();
            cartao.Dock = DockStyle.Fill;
            var faixa = new Panel { BackColor = cor, Size = new Size(4, 77), Location = new Point(14, 22) };
            cartao.Controls.Add(faixa);
            var numero = CiviaUI.TextoLabel(valor.ToString(), 29, true);
            numero.Location = new Point(30, 24);
            cartao.Controls.Add(numero);
            var legenda = new Label
            {
                Text = descricao,
                Location = new Point(31, 84),
                Size = new Size(220, 30),
                ForeColor = CiviaUI.Secundario,
                Font = new Font("Segoe UI", 8.3f, FontStyle.Bold),
                AutoEllipsis = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top
            };
            cartao.Controls.Add(legenda);
            cartao.Resize += (_, _) => legenda.Width = Math.Max(75, cartao.Width - 45);
            return cartao;
        }

        private void MostrarRegistros()
        {
            DestacarMenu(1);
            var container = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = CiviaUI.Fundo,
                Margin = Padding.Empty
            };
            container.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
            container.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            container.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));

            var filtros = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 53));
            filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24));
            filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23));
            _txtBusca = new TextBox
            {
                PlaceholderText = "Buscar por local, bairro ou descrição...",
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 13, 14, 18)
            };
            _txtBusca.TextChanged += (_, _) => AplicarFiltros();
            filtros.Controls.Add(_txtBusca, 0, 0);

            _cmbFiltro = CiviaUI.Lista("Todos os tipos", "Barreiras urbanas", "Pontos de apoio");
            _cmbFiltro.Dock = DockStyle.Fill;
            _cmbFiltro.Margin = new Padding(0, 13, 12, 18);
            _cmbFiltro.SelectedIndexChanged += (_, _) => AplicarFiltros();
            filtros.Controls.Add(_cmbFiltro, 1, 0);

            var exportar = CiviaUI.Botao("Exportar CSV", largura: 140);
            exportar.Dock = DockStyle.Fill;
            exportar.Margin = new Padding(0, 9, 0, 14);
            exportar.Click += (_, _) => ExportarCsv();
            filtros.Controls.Add(exportar, 2, 0);
            container.Controls.Add(filtros, 0, 0);

            var blocoTabela = CiviaUI.Cartao();
            blocoTabela.Margin = Padding.Empty;
            blocoTabela.Dock = DockStyle.Fill;
            var estruturaTabela = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Margin = Padding.Empty
            };
            estruturaTabela.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            estruturaTabela.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _lblQuantidade = CiviaUI.TextoLabel("Registros cadastrados", 11, true);
            _lblQuantidade.AutoSize = false;
            _lblQuantidade.Dock = DockStyle.Fill;
            estruturaTabela.Controls.Add(_lblQuantidade, 0, 0);

            _gridRegistros = CriarTabela();
            _gridRegistros.Dock = DockStyle.Fill;
            _gridRegistros.CellDoubleClick += (_, _) => ExibirDetalhes();
            estruturaTabela.Controls.Add(_gridRegistros, 0, 1);
            blocoTabela.Controls.Add(estruturaTabela);
            container.Controls.Add(blocoTabela, 0, 1);

            var botoes = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(0, 15, 0, 0),
                Margin = Padding.Empty,
                BackColor = CiviaUI.Fundo
            };
            AdicionarAcao(botoes, "Ver detalhes", 123, ExibirDetalhes);
            AdicionarAcao(botoes, "Em análise", 125, () => AtualizarSelecionado(StatusRegistro.EmAnalise));
            AdicionarAcao(botoes, "Concluir", 115, () => AtualizarSelecionado(StatusRegistro.Concluido));
            AdicionarAcao(botoes, "Reabrir", 110, () => AtualizarSelecionado(StatusRegistro.Pendente));
            AdicionarAcao(botoes, "Excluir", 105, ExcluirSelecionado);
            container.Controls.Add(botoes, 0, 2);

            ExibirPagina(container, "Registros", "Consulte, acompanhe e atualize as informações cadastradas.");
            AplicarFiltros();
        }

        private void AdicionarAcao(FlowLayoutPanel painel, string texto, int largura, Action acao)
        {
            var botao = CiviaUI.Botao(texto, largura: largura);
            botao.Click += (_, _) => acao();
            painel.Controls.Add(botao);
        }

        private DataGridView CriarTabela()
        {
            var tabela = new DataGridView
            {
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeight = 43,
                RowTemplate = { Height = 46 },
                Font = new Font("Segoe UI", 9.4f),
                GridColor = Color.FromArgb(237, 239, 243),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };

            tabela.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 252);
            tabela.ColumnHeadersDefaultCellStyle.ForeColor = CiviaUI.Texto;
            tabela.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            tabela.DefaultCellStyle.BackColor = Color.White;
            tabela.DefaultCellStyle.ForeColor = CiviaUI.Texto;
            tabela.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 232, 241);
            tabela.DefaultCellStyle.SelectionForeColor = CiviaUI.Texto;
            tabela.DefaultCellStyle.Padding = new Padding(5, 2, 5, 2);

            tabela.Columns.Add("Data", "DATA");
            tabela.Columns.Add("Tipo", "TIPO");
            tabela.Columns.Add("Local", "LOCAL");
            tabela.Columns.Add("Bairro", "BAIRRO");
            tabela.Columns.Add("Prioridade", "PRIORIDADE");
            tabela.Columns.Add("Status", "STATUS");
            float[] pesos = { 13, 17, 25, 18, 12, 15 };
            for (int i = 0; i < tabela.Columns.Count; i++)
            {
                tabela.Columns[i].FillWeight = pesos[i];
                tabela.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            return tabela;
        }

        private void PreencherTabela(DataGridView tabela, IEnumerable<RegistroAcessibilidade> registros)
        {
            tabela.Rows.Clear();
            foreach (var registro in registros)
            {
                int indice = tabela.Rows.Add(
                    registro.CriadoEm.ToString("dd/MM/yyyy"),
                    registro.ObterTipo(), registro.Local, registro.Bairro,
                    registro.ObterPrioridade(), registro.ObterStatus());
                var linha = tabela.Rows[indice];
                linha.Tag = registro.Id;
                linha.Cells[4].Style.ForeColor = registro.Prioridade == Prioridade.Alta
                    ? CiviaUI.Destaque : CiviaUI.Secundario;
                linha.Cells[5].Style.ForeColor = registro.Status == StatusRegistro.Concluido
                    ? CiviaUI.Verde : CiviaUI.Texto;
            }
            tabela.ClearSelection();
        }

        private void AplicarFiltros()
        {
            if (_gridRegistros == null || _txtBusca == null || _cmbFiltro == null) return;
            string busca = _txtBusca.Text.Trim();
            IEnumerable<RegistroAcessibilidade> filtrados = _servico.Listar();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                filtrados = filtrados.Where(r =>
                    r.Local.Contains(busca, StringComparison.CurrentCultureIgnoreCase) ||
                    r.Bairro.Contains(busca, StringComparison.CurrentCultureIgnoreCase) ||
                    r.Descricao.Contains(busca, StringComparison.CurrentCultureIgnoreCase));
            }
            if (_cmbFiltro.SelectedIndex == 1)
                filtrados = filtrados.Where(r => r.Tipo == TipoRegistro.BarreiraUrbana);
            if (_cmbFiltro.SelectedIndex == 2)
                filtrados = filtrados.Where(r => r.Tipo == TipoRegistro.PontoApoio);

            var lista = filtrados.ToList();
            PreencherTabela(_gridRegistros, lista);
            if (_lblQuantidade != null)
                _lblQuantidade.Text = lista.Count == 0
                    ? "Nenhum registro encontrado. Use '+ Novo registro' para começar."
                    : $"{lista.Count} registro(s) encontrado(s) • Duplo clique para ver os detalhes";
        }

        private RegistroAcessibilidade? RegistroSelecionado()
        {
            if (_gridRegistros == null || _gridRegistros.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um registro na tabela.", "CIVIA Desk",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            Guid? id = _gridRegistros.SelectedRows[0].Tag is Guid guid ? guid : null;
            return _servico.Listar().FirstOrDefault(r => r.Id == id);
        }

        private void ExibirDetalhes()
        {
            var registro = RegistroSelecionado();
            if (registro == null) return;

            MessageBox.Show(
                $"Local: {registro.Local}\nBairro: {registro.Bairro}\n" +
                $"Tipo: {registro.ObterTipo()}\nPúblico: {registro.ObterPublico()}\n" +
                $"Prioridade: {registro.ObterPrioridade()}\nStatus: {registro.ObterStatus()}\n" +
                $"Criado em: {registro.CriadoEm:dd/MM/yyyy HH:mm}\n\n" +
                $"Descrição:\n{registro.Descricao}",
                "Detalhes do registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AtualizarSelecionado(StatusRegistro status)
        {
            var registro = RegistroSelecionado();
            if (registro == null) return;

            try
            {
                if (_servico.AlterarStatus(registro.Id, status))
                {
                    _lblRodape.Text = "Status atualizado e salvo no computador.";
                    AplicarFiltros();
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                MostrarErroArquivo(ex);
            }
        }

        private void ExcluirSelecionado()
        {
            var registro = RegistroSelecionado();
            if (registro == null) return;

            if (MessageBox.Show($"Excluir o registro de '{registro.Local}'?\nEsta ação não poderá ser desfeita.",
                "Confirmar exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _servico.Excluir(registro.Id);
                _lblRodape.Text = "Registro excluído.";
                AplicarFiltros();
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                MostrarErroArquivo(ex);
            }
        }

        private void AbrirCadastro()
        {
            using var cadastro = new FrmCadastro();
            if (cadastro.ShowDialog(this) != DialogResult.OK || cadastro.NovoRegistro == null)
                return;

            try
            {
                _servico.Cadastrar(cadastro.NovoRegistro);
                _lblRodape.Text = "Registro salvo com sucesso.";
                MostrarRegistros();
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                MostrarErroArquivo(ex);
            }
        }

        private void InserirExemplos()
        {
            if (MessageBox.Show(
                "Inserir três situações totalmente fictícias para demonstração?\n" +
                "Elas não representam ocorrências reais.",
                "Dados de demonstração", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                int quantidade = _servico.InserirExemplos();
                _lblRodape.Text = quantidade > 0
                    ? $"{quantidade} registros de demonstração inseridos."
                    : "Os registros de demonstração já foram inseridos.";
                MostrarVisaoGeral();
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                MostrarErroArquivo(ex);
            }
        }

        private void ExportarCsv()
        {
            if (!_servico.Listar().Any())
            {
                MessageBox.Show("Não há registros para exportar.", "CIVIA Desk",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialogo = new SaveFileDialog
            {
                Title = "Exportar registros",
                Filter = "Arquivo CSV (*.csv)|*.csv",
                FileName = "civia-registros.csv",
                AddExtension = true,
                DefaultExt = "csv",
                OverwritePrompt = true
            };
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                _servico.ExportarCsv(dialogo.FileName);
                MessageBox.Show("Relatório exportado com sucesso.", "CIVIA Desk",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                MostrarErroArquivo(ex);
            }
        }

        private void MostrarSobre()
        {
            DestacarMenu(2);
            _gridRegistros = null;
            var cartao = CiviaUI.Cartao();
            cartao.Dock = DockStyle.Fill;
            cartao.Margin = Padding.Empty;
            cartao.Padding = new Padding(28);

            var titulo = CiviaUI.TextoLabel("CIVIA Desk", 24, true);
            titulo.Location = new Point(28, 30);
            cartao.Controls.Add(titulo);

            var texto = new Label
            {
                Text =
                    "Uma central comunitária de acessibilidade, criada como protótipo " +
                    "da Global Solution CitizenTech.\n\n" +
                    "O sistema permite cadastrar barreiras urbanas e pontos de apoio, " +
                    "consultar informações, acompanhar o status e exportar registros. " +
                    "Os dados são armazenados localmente em JSON.\n\n" +
                    "Cidadãos beneficiados: pessoas cegas, com baixa visão, " +
                    "surdas e com perda auditiva. O atendimento pode ser realizado " +
                    "por um operador em posto comunitário.\n\n" +
                    "Este sistema NÃO faz reconhecimento por câmera, não controla óculos, " +
                    "não envia SOS real e não oferece navegação segura. É uma parte administrativa " +
                    "e demonstrativa do conceito CIVIA, independente do aplicativo principal.\n\n" +
                    "ODS 10: redução das desigualdades.  ODS 11: cidades e comunidades sustentáveis.",
                Location = new Point(29, 99),
                Size = new Size(790, 390),
                Font = new Font("Segoe UI", 11),
                ForeColor = CiviaUI.Texto,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            cartao.Controls.Add(texto);
            cartao.Resize += (_, _) => texto.Width = Math.Max(420, cartao.ClientSize.Width - 68);
            var caminho = new Label
            {
                Text = "Armazenamento: " + _servico.CaminhoDados,
                Location = new Point(29, 520),
                Size = new Size(760, 55),
                ForeColor = CiviaUI.Secundario,
                Font = new Font("Segoe UI", 8.8f),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            cartao.Controls.Add(caminho);
            cartao.Resize += (_, _) =>
            {
                caminho.Top = cartao.ClientSize.Height - 70;
                caminho.Width = Math.Max(400, cartao.ClientSize.Width - 66);
            };
            ExibirPagina(cartao, "Sobre o CIVIA", "O papel deste protótipo desktop no ecossistema CIVIA.");
        }

        private static void MostrarErroArquivo(Exception ex)
        {
            MessageBox.Show("Não foi possível salvar ou acessar o arquivo local.\n\n" + ex.Message,
                "Erro de armazenamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
