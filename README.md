# Laboratório De Software
Trabalho para entrega de um projeto desenvolvido na cadeira de Laboratório de Software - UCS



# Tecnologias
Node (LTS)
JQuery
.NET 9


#Importante para executar
- Crie um arquivo chamado "appsettings.Development.json" (Se não existir), na pasta "LaboratorioDeSoftware", la copie e cole o conteúdo do appsettings normal, mas mude o campo Password na connection string para a informada externamente.
- Para executar migrations precisa instalar o dotnet ef globalmente na maquina (só pesquisar como instala), depois você vai abrir o terminal na pasta do projeto principal "LaboratorioDeSoftware" (onde esta o site/api), lá execute o comando:
  "dotnet ef migrations add NOME_DA_MIGRATION --project ..\LaboratorioDeSoftware.Core", depois execute o comando, "dotnet ef database update"
  
