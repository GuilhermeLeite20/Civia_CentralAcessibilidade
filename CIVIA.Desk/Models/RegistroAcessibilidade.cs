using System.Text.Json.Serialization;

namespace CiviaDesk.Models
{
    // Representa uma barreira urbana ou um ponto de apoio para a comunidade.
    public class RegistroAcessibilidade
    {
        [JsonInclude] public Guid Id { get; private set; }
        [JsonInclude] public TipoRegistro Tipo { get; private set; }
        [JsonInclude] public string Local { get; private set; } = "";
        [JsonInclude] public string Bairro { get; private set; } = "";
        [JsonInclude] public string Descricao { get; private set; } = "";
        [JsonInclude] public PublicoAtendido Publico { get; private set; }
        [JsonInclude] public Prioridade Prioridade { get; private set; }
        [JsonInclude] public StatusRegistro Status { get; private set; }
        [JsonInclude] public DateTime CriadoEm { get; private set; }
        [JsonInclude] public DateTime AtualizadoEm { get; private set; }

        // Necessário para leitura do arquivo JSON pelo System.Text.Json.
        public RegistroAcessibilidade() { }

        public RegistroAcessibilidade(TipoRegistro tipo, string local, string bairro,
            string descricao, PublicoAtendido publico, Prioridade prioridade)
        {
            if (string.IsNullOrWhiteSpace(local) || local.Trim().Length < 3)
                throw new ArgumentException("Informe um local com pelo menos 3 caracteres.");

            if (string.IsNullOrWhiteSpace(bairro) || bairro.Trim().Length < 2)
                throw new ArgumentException("Informe o bairro.");

            if (string.IsNullOrWhiteSpace(descricao) || descricao.Trim().Length < 10)
                throw new ArgumentException("Descreva a situação com pelo menos 10 caracteres.");

            Id = Guid.NewGuid();
            Tipo = tipo;
            Local = local.Trim();
            Bairro = bairro.Trim();
            Descricao = descricao.Trim();
            Publico = publico;
            Prioridade = prioridade;
            Status = StatusRegistro.Pendente;
            CriadoEm = DateTime.Now;
            AtualizadoEm = CriadoEm;
        }

        // O status só deve ser alterado por uma ação explícita do usuário.
        public void AlterarStatus(StatusRegistro novoStatus)
        {
            Status = novoStatus;
            AtualizadoEm = DateTime.Now;
        }

        public RegistroAcessibilidade Copiar()
        {
            return new RegistroAcessibilidade
            {
                Id = Id,
                Tipo = Tipo,
                Local = Local,
                Bairro = Bairro,
                Descricao = Descricao,
                Publico = Publico,
                Prioridade = Prioridade,
                Status = Status,
                CriadoEm = CriadoEm,
                AtualizadoEm = AtualizadoEm
            };
        }

        public string ObterTipo() => Tipo == TipoRegistro.BarreiraUrbana ? "Barreira urbana" : "Ponto de apoio";

        public string ObterPublico() => Publico switch
        {
            PublicoAtendido.DeficienciaVisual => "Deficiência visual",
            PublicoAtendido.DeficienciaAuditiva => "Deficiência auditiva",
            _ => "Ambos"
        };

        public string ObterPrioridade() => Prioridade switch
        {
            Prioridade.Alta => "Alta",
            Prioridade.Media => "Média",
            _ => "Baixa"
        };

        public string ObterStatus() => Status switch
        {
            StatusRegistro.EmAnalise => "Em análise",
            StatusRegistro.Concluido when Tipo == TipoRegistro.PontoApoio => "Verificado",
            StatusRegistro.Concluido => "Resolvido",
            _ => "Pendente"
        };
    }
}
