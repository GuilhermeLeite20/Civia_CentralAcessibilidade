# Guia rápido de Git, versionamento e Pull Request

O enunciado de C# exige **versionamento semântico com Pull Request**. O ZIP contém o projeto, mas o PR deve ser feito no GitHub depois que um repositório for criado pela equipe.

## 1. Crie o repositório

No GitHub, crie um repositório vazio (por exemplo, `CIVIA-Desk`) na conta ou organização da equipe, **sem gerar README, licença ou .gitignore automaticamente**, pois os arquivos já estão no ZIP.

No terminal, dentro da pasta extraída (substitua `URL_DO_REPOSITORIO` pela URL REAL):

```bash
git init
git branch -M main
git add README.md .gitignore integrantes.txt
git commit -m "chore: inicia documentacao do CIVIA Desk"
git remote add origin URL_DO_REPOSITORIO
git push -u origin main
```

## 2. Envie o código em branch de funcionalidade

```bash
git switch -c feat/civia-desk-winforms
git add .
git commit -m "feat: implementa painel e cadastro de acessibilidade"
git push -u origin feat/civia-desk-winforms
```

## 3. Abra o Pull Request

No GitHub, abra **Compare & pull request**, comparando a branch `feat/civia-desk-winforms` com `main`. Use o texto de `PR_DESCRICAO.md` como modelo. Peça que um integrante revise antes do merge.

Depois do merge, crie a marcação de versão:

```bash
git switch main
git pull origin main
git tag -a v0.1.0 -m "Primeira versao academica do CIVIA Desk"
git push origin v0.1.0
```

## Padrão de mensagens

- `feat:` nova funcionalidade
- `fix:` correção de erro
- `docs:` documentação
- `refactor:` melhoria interna sem alterar comportamento
- `chore:` organização do repositório

A versão `0.1.0` indica um protótipo inicial, anterior à versão pública `1.0.0`.

## Entrega no Teams

1. Comprima a pasta do projeto **sem** `bin/`, `obj/`, `.vs/` e sem dados pessoais.
2. Abra `entrega-teams.txt` e substitua o placeholder pelo link REAL do repositório.
3. Anexe o ZIP e o TXT na atividade.
4. Guarde o link do PR e as capturas de tela como evidências.

> O repositório e o Pull Request só passarão a existir após a publicação pela equipe. Não informe links que ainda não foram criados.
