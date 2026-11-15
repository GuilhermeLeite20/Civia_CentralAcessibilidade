using System.Drawing;
using System.Windows.Forms;

namespace CiviaDesk.Views
{
    // Elementos visuais reaproveitados nas telas do sistema.
    internal static class CiviaUI
    {
        public static readonly Color Fundo = Color.FromArgb(245, 247, 250);
        public static readonly Color Branco = Color.White;
        public static readonly Color Escuro = Color.FromArgb(23, 29, 43);
        public static readonly Color Texto = Color.FromArgb(32, 42, 60);
        public static readonly Color Secundario = Color.FromArgb(104, 117, 137);
        public static readonly Color Destaque = Color.FromArgb(224, 16, 104);
        public static readonly Color Borda = Color.FromArgb(222, 226, 232);
        public static readonly Color Verde = Color.FromArgb(17, 126, 96);

        public static Label TextoLabel(string texto, float tamanho = 10, bool negrito = false)
        {
            return new Label
            {
                Text = texto,
                AutoSize = true,
                ForeColor = Texto,
                Font = new Font("Segoe UI", tamanho,
                    negrito ? FontStyle.Bold : FontStyle.Regular)
            };
        }

        public static Button Botao(string texto, bool destaque = false, int largura = 140)
        {
            var botao = new Button
            {
                Text = texto,
                Width = largura,
                Height = 40,
                BackColor = destaque ? Destaque : Branco,
                ForeColor = destaque ? Branco : Texto,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                UseVisualStyleBackColor = false,
                Margin = new Padding(5, 4, 5, 4)
            };

            botao.FlatAppearance.BorderColor = destaque ? Destaque : Borda;
            botao.FlatAppearance.BorderSize = destaque ? 0 : 1;
            return botao;
        }

        public static Panel Cartao()
        {
            var cartao = new Panel
            {
                BackColor = Branco,
                Padding = new Padding(18),
                Margin = new Padding(0, 0, 14, 0)
            };

            cartao.Paint += (_, e) =>
            {
                using var caneta = new Pen(Borda);
                e.Graphics.DrawRectangle(caneta, 0, 0, cartao.Width - 1, cartao.Height - 1);
            };
            return cartao;
        }

        public static ComboBox Lista(params string[] opcoes)
        {
            var caixa = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                BackColor = Branco,
                ForeColor = Texto,
                FlatStyle = FlatStyle.Standard,
                Height = 34,
                IntegralHeight = false
            };
            caixa.Items.AddRange(opcoes);
            caixa.SelectedIndex = 0;
            return caixa;
        }
    }
}
