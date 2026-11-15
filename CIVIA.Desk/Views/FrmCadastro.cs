using System.Drawing;
using System.Windows.Forms;
using CiviaDesk.Models;

namespace CiviaDesk.Views
{
    public class FrmCadastro : Form
    {
        private readonly ComboBox _cmbTipo;
        private readonly ComboBox _cmbPublico;
        private readonly ComboBox _cmbPrioridade;
        private readonly TextBox _txtLocal;
        private readonly TextBox _txtBairro;
        private readonly TextBox _txtDescricao;

        public RegistroAcessibilidade? NovoRegistro { get; private set; }

        public FrmCadastro()
        {
            Text = "CIVIA Desk | Novo registro";
            ClientSize = new Size(610, 694);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = CiviaUI.Branco;
            Font = new Font("Segoe UI", 10);

            var topo = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = CiviaUI.Escuro
            };
            Controls.Add(topo);

            var titulo = CiviaUI.TextoLabel("Novo registro de acessibilidade", 17, true);
            titulo.ForeColor = Color.White;
            titulo.Location = new Point(28, 19);
            topo.Controls.Add(titulo);

            var dica = CiviaUI.TextoLabel("Descreva uma barreira ou um ponto de apoio à comunidade.", 9);
            dica.ForeColor = Color.FromArgb(210, 215, 225);
            dica.Location = new Point(29, 58);
            topo.Controls.Add(dica);

            _cmbTipo = CiviaUI.Lista("Barreira urbana", "Ponto de apoio");
            _cmbPublico = CiviaUI.Lista("Pessoa com deficiência visual", "Pessoa com deficiência auditiva", "Ambos os públicos");
            _cmbPrioridade = CiviaUI.Lista("Baixa", "Média", "Alta");
            _txtLocal = CriarTexto(100);
            _txtBairro = CriarTexto(70);
            _txtDescricao = CriarTexto(500, true);

            AdicionarCampo("Tipo de registro", _cmbTipo, 123);
            AdicionarCampo("Nome do local", _txtLocal, 189);
            AdicionarCampo("Bairro", _txtBairro, 255);
            AdicionarCampo("Público beneficiado", _cmbPublico, 321);
            AdicionarCampo("Prioridade", _cmbPrioridade, 387);
            AdicionarCampo("Descrição da situação", _txtDescricao, 453);
            _txtDescricao.Height = 85;

            var nota = CiviaUI.TextoLabel("Use apenas dados simulados ou públicos. Não informe dados pessoais.", 8.5f);
            nota.ForeColor = CiviaUI.Secundario;
            nota.Location = new Point(28, 582);
            Controls.Add(nota);

            var cancelar = CiviaUI.Botao("Cancelar", largura: 120);
            cancelar.Location = new Point(315, 626);
            cancelar.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(cancelar);

            var salvar = CiviaUI.Botao("Salvar registro", destaque: true, largura: 145);
            salvar.Location = new Point(441, 626);
            salvar.Click += (_, _) => Salvar();
            Controls.Add(salvar);

            AcceptButton = salvar;
            CancelButton = cancelar;
            ActiveControl = _txtLocal;
        }

        private TextBox CriarTexto(int maximo, bool multilinha = false)
        {
            return new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White,
                ForeColor = CiviaUI.Texto,
                MaxLength = maximo,
                Multiline = multilinha,
                ScrollBars = multilinha ? ScrollBars.Vertical : ScrollBars.None
            };
        }

        private void AdicionarCampo(string legenda, Control campo, int y)
        {
            var label = CiviaUI.TextoLabel(legenda, 9, true);
            label.Location = new Point(28, y);
            Controls.Add(label);

            campo.Location = new Point(28, y + 23);
            campo.Width = 554;
            if (campo is TextBox && !((TextBox)campo).Multiline)
                campo.Height = 31;
            Controls.Add(campo);
        }

        private void Salvar()
        {
            try
            {
                TipoRegistro tipo = _cmbTipo.SelectedIndex == 0
                    ? TipoRegistro.BarreiraUrbana : TipoRegistro.PontoApoio;

                PublicoAtendido publico = (PublicoAtendido)_cmbPublico.SelectedIndex;
                Prioridade prioridade = (Prioridade)_cmbPrioridade.SelectedIndex;

                NovoRegistro = new RegistroAcessibilidade(
                    tipo, _txtLocal.Text, _txtBairro.Text,
                    _txtDescricao.Text, publico, prioridade);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Verifique as informações",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
