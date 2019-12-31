# Template API ASP.NET Core
Template de API desenvolvido em ASP.NET Core 3.1.

# Recursos
- Conceitos de DDD - Domain-Driven Design;
- Utilizado Entity Framework, com "Code First" para gerar os scripts de banco de dados(Migrations);
- Injeção de dependência utilizando recurso nativo do .NET Core;
- Padrão Unit of Work para controle de transações;
- Padrão de Notificações para validações em conjunto com o Fluent Validation;
- Filtro de exceção para gravar o stack trace. Iniciamente gravando em banco de dados, utilizando contexto separado para os logs, mas também abre também a possibilidade de implementar para gravar em serviço em nuvem como por exemplo o Elmah;
- Controle de usuários e permissões utilizando o ASP.NET Identity e controle de token utilizando o JwT(JSON Web Tokens);
- Repositório genérico com possibilidade de especializar para cada entidade;

# Configurações
- Necessário criar o arquivo "appsettings.json", dentro do projeto "RBTemplate.Services.Api", para configurar:
  - Chave secreta para a geração de token. Formato:  "SecretKeyJwT": "SUA_CHAVE_AQUI";
  - Conexão com banco de dados. Formato: "ConnectionStrings": { "DefaultConnection": "STRING DE CONEXÃO AQUI" }

# Divisão da Solução
RBTemplate.Services.Api
  - Projeto de API

RBTemplate.Domain
  - Projeto onde são mapeadas as entidades e são criadas as classes de negócios

RBTemplate.Infra.Data
  - Onde são criados:
    - Contextos;
    - Mapeamentos para migrations;
    - Unit Of Work;
    - Repositórios;

RBTemplate.Infra.CrossCutting.AspNetFilters
  - Projeto para os filtros.(Filtro de exceção implementado)

RBTemplate.Infra.CrossCutting.Identity
  - Projeto com as configurações do Identity;

RBTemplate.Infra.CrossCutting.IoC
  - Projeto de injeção de dependências;
  
# Autor
Rafael Bocute
