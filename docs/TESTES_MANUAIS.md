# Roteiro de testes manuais — CIVIA Desk

Execute estes testes no Windows após abrir o projeto no Visual Studio. **Marque apenas os testes que realmente executar.**

| ID | Cenário | Procedimento | Resultado esperado | Executado? |
|---|---|---|---|---|
| T01 | Iniciar aplicação | Executar com F5 | Exibe visão geral, sem exigir internet | [ ] |
| T02 | Campo obrigatório | Abrir cadastro e deixar local vazio | Mensagem de validação; nada é salvo | [ ] |
| T03 | Descrição curta | Escrever descrição com menos de 10 caracteres | Mensagem de validação | [ ] |
| T04 | Criar barreira | Preencher e salvar | Registro aparece na tabela como Pendente | [ ] |
| T05 | Criar ponto de apoio | Selecionar o tipo e salvar | Novo registro aparece no filtro Pontos de apoio | [ ] |
| T06 | Pesquisa | Digitar nome do local | Lista apresenta registros correspondentes | [ ] |
| T07 | Alterar status | Selecionar registro e clicar Concluir | Status muda para Resolvido ou Verificado | [ ] |
| T08 | Persistência | Fechar e abrir novamente | Registro e status permanecem salvos | [ ] |
| T09 | Exportação CSV | Abrir Registros > Exportar CSV | Arquivo é salvo com cabeçalhos e dados | [ ] |
| T10 | Exclusão | Selecionar registro e clicar Excluir | Exibe confirmação antes de apagar | [ ] |
| T11 | Exemplos fictícios | Clicar Inserir exemplos duas vezes | Não duplica os exemplos existentes | [ ] |
| T12 | Arquivo inválido | Em ambiente de testes, corromper uma cópia de registros.json | O app informa erro e não apaga o arquivo | [ ] |

**Evidências:** cole as capturas reais da aplicação no README ou no PR após executar os testes. A imagem `preview_conceitual.png` é apenas uma ilustração e não substitui evidência.
