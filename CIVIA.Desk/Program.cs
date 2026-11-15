using System.Windows.Forms;
using System.Globalization;
using CiviaDesk.Views;

namespace CiviaDesk
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.CurrentUICulture = new CultureInfo("pt-BR");

            ApplicationConfiguration.Initialize();

            try
            {
                Application.Run(new FrmPrincipal());
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Text.Json.JsonException)
            {
                MessageBox.Show(
                    $"Não foi possível iniciar o CIVIA Desk.\n\n{ex.Message}\n\nO arquivo local não será apagado.",
                    "Erro ao abrir os dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
