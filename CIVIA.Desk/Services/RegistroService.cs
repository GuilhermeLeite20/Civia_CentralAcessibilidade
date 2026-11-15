using System.Text;
using CiviaDesk.Models;

namespace CiviaDesk.Services
{
    // Centraliza as regras: a interface não grava os arquivos diretamente.
    public class RegistroService
    {
        private readonly RepositorioJson _repositorio;
        private List<RegistroAcessibilidade> _registros;

        public string CaminhoDados => _repositorio.Caminho;

        public RegistroService()
        {
            _repositorio = new RepositorioJson();
            _registros = _repositorio.Carregar();
        }

        public List<RegistroAcessibilidade> Listar()
        {
            return _registros.OrderByDescending(r => r.CriadoEm).ToList();
        }

        public void Cadastrar(RegistroAcessibilidade registro)
        {
            var copia = new List<RegistroAcessibilidade>(_registros) { registro };
            SalvarAlteracoes(copia);
        }

        public bool AlterarStatus(Guid id, StatusRegistro status)
        {
            var copia = _registros.Select(r => r.Copiar()).ToList();
            var registro = copia.FirstOrDefault(r => r.Id == id);
            if (registro == null) return false;

            registro.AlterarStatus(status);
            SalvarAlteracoes(copia);
            return true;
        }

        public bool Excluir(Guid id)
        {
            var copia = _registros.Where(r => r.Id != id).ToList();
            if (copia.Count == _registros.Count) return false;

            SalvarAlteracoes(copia);
            return true;
        }

        public int InserirExemplos()
        {
            if (_registros.Any(r => r.Descricao.StartsWith("[DEMO]", StringComparison.OrdinalIgnoreCase)))
                return 0;

            // Situações inteiramente fictícias. NÃO representam incidentes reais.
            var exemplos = new List<RegistroAcessibilidade>
            {
                new(TipoRegistro.BarreiraUrbana, "Estação Central (fictícia)", "Centro",
                    "[DEMO] Placa de orientação com fonte muito pequena para pessoas com baixa visão.",
                    PublicoAtendido.DeficienciaVisual, Prioridade.Media),

                new(TipoRegistro.BarreiraUrbana, "Terminal Comunitário (fictício)", "Bairro Estação",
                    "[DEMO] Aviso de mudança de plataforma comunicado somente pelo alto-falante.",
                    PublicoAtendido.DeficienciaAuditiva, Prioridade.Alta),

                new(TipoRegistro.PontoApoio, "Balcão de apoio (fictício)", "Centro",
                    "[DEMO] Ponto com orientações acessíveis e atendimento presencial disponível.",
                    PublicoAtendido.Ambos, Prioridade.Baixa)
            };

            var copia = new List<RegistroAcessibilidade>(_registros);
            copia.AddRange(exemplos);
            SalvarAlteracoes(copia);
            return exemplos.Count;
        }

        public void ExportarCsv(string caminho)
        {
            var linhas = new List<string>
            {
                "ID;Data;Tipo;Local;Bairro;Publico;Prioridade;Status;Descricao"
            };

            foreach (var r in Listar())
            {
                linhas.Add(string.Join(";", new[]
                {
                    Csv(r.Id.ToString()), Csv(r.CriadoEm.ToString("dd/MM/yyyy HH:mm")),
                    Csv(r.ObterTipo()), Csv(r.Local), Csv(r.Bairro), Csv(r.ObterPublico()),
                    Csv(r.ObterPrioridade()), Csv(r.ObterStatus()), Csv(r.Descricao)
                }));
            }

            File.WriteAllLines(caminho, linhas, new UTF8Encoding(true));
        }

        private void SalvarAlteracoes(List<RegistroAcessibilidade> novaLista)
        {
            // Só confirma a alteração na memória depois de salvar o arquivo.
            _repositorio.Salvar(novaLista);
            _registros = novaLista;
        }

        private static string Csv(string valor)
        {
            // Evita fórmulas interpretadas por planilhas nos campos digitados.
            string seguro = valor.TrimStart().StartsWith('=') || valor.TrimStart().StartsWith('+') ||
                            valor.TrimStart().StartsWith('-') || valor.TrimStart().StartsWith('@')
                ? "'" + valor
                : valor;
            return "\"" + seguro.Replace("\"", "\"\"") + "\"";
        }
    }
}
