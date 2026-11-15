# feat: protótipo desktop CIVIA Desk

## Objetivo
Construir uma aplicação Windows Forms simples para cadastrar e acompanhar barreiras urbanas e pontos de apoio relacionados à acessibilidade.

## Alterações
- Interface com visão geral, registros e tela de cadastro
- Validações e domínio orientado a objetos
- Persistência local em JSON e exportação CSV
- Busca, filtros, consulta, alteração de status e exclusão
- README, equipe, ficha CitizenTech e integração aos ODS 10 e 11

## Como testar
1. Abrir `CIVIA.Desk.sln` no Visual Studio 2022 com .NET 8.
2. Executar com F5.
3. Cadastrar uma barreira e conferir os indicadores.
4. Fechar/reabrir para verificar persistência.
5. Alterar o status e exportar um CSV.

## Checklist de revisão
- [ ] A aplicação compilou em Windows com .NET 8.
- [ ] Os campos obrigatórios são validados.
- [ ] Os registros são preservados entre execuções.
- [ ] A atualização de status funciona.
- [ ] Não foram utilizados dados pessoais reais.
- [ ] Prints foram incluídos no PR ou entrega, se solicitados.

**Observação:** todos os exemplos pré-cadastrados são fictícios.
