# Fluxo simplificado da aplicação

```text
CIVIA Desk
  |
  +-- Visão geral
  |     +-- Indicadores
  |     +-- Registros recentes
  |     +-- Inserir dados demonstrativos (opcional)
  |
  +-- Novo registro
  |     +-- Tipo, local, bairro, público, prioridade, descrição
  |     +-- Validação dos campos
  |     +-- RegistroService.Cadastrar()
  |     +-- RepositorioJson.Salvar()
  |
  +-- Registros
  |     +-- Buscar e filtrar
  |     +-- Abrir detalhes
  |     +-- Atualizar status / excluir
  |     +-- Exportar CSV
  |
  +-- Sobre o CIVIA
        +-- Escopo e limitações do protótipo
```

A aplicação funciona inteiramente no computador do operador, sem backend, login, banco de dados ou envio à nuvem. Isso é uma decisão de escopo para o entregável de C#, não uma característica obrigatória do produto CIVIA completo.
