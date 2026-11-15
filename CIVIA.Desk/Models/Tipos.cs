namespace CiviaDesk.Models
{
    public enum TipoRegistro
    {
        BarreiraUrbana,
        PontoApoio
    }

    public enum PublicoAtendido
    {
        DeficienciaVisual,
        DeficienciaAuditiva,
        Ambos
    }

    public enum Prioridade
    {
        Baixa,
        Media,
        Alta
    }

    public enum StatusRegistro
    {
        Pendente,
        EmAnalise,
        Concluido
    }
}
