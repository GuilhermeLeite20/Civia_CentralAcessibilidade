using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CiviaDesk.Models;

namespace CiviaDesk.Services
{
    // Salva e carrega os registros em uma pasta local do usuário no Windows.
    public class RepositorioJson
    {
        private readonly string _caminho;
        private readonly JsonSerializerOptions _opcoes;

        public string Caminho => _caminho;

        public RepositorioJson()
        {
            string pasta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CIVIA", "Desk");

            Directory.CreateDirectory(pasta);
            _caminho = Path.Combine(pasta, "registros.json");

            _opcoes = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public List<RegistroAcessibilidade> Carregar()
        {
            if (!File.Exists(_caminho))
                return new List<RegistroAcessibilidade>();

            string json = File.ReadAllText(_caminho, Encoding.UTF8);

            if (string.IsNullOrWhiteSpace(json))
                throw new JsonException("O arquivo de registros está vazio ou inválido.");

            return JsonSerializer.Deserialize<List<RegistroAcessibilidade>>(json, _opcoes)
                ?? throw new JsonException("Não foi possível interpretar os registros salvos.");
        }

        public void Salvar(List<RegistroAcessibilidade> registros)
        {
            string temporario = _caminho + ".tmp";
            string json = JsonSerializer.Serialize(registros, _opcoes);

            // Primeiro grava um arquivo temporário, evitando sobrescrever um dado incompleto.
            try
            {
                File.WriteAllText(temporario, json, new UTF8Encoding(false));
                File.Move(temporario, _caminho, true);
            }
            finally
            {
                if (File.Exists(temporario))
                {
                    try { File.Delete(temporario); }
                    catch (IOException) { /* O próximo salvamento tentará novamente. */ }
                    catch (UnauthorizedAccessException) { }
                }
            }
        }
    }
}
