# FCG – FIAP Cloud Games

API REST em .NET 8 para gerenciamento de usuários e jogos adquiridos, atendendo aos requisitos da primeira fase do Tech Challenge

---

## Visão Geral

Este projeto implementa o **mínimo produto viável (MVP)** de uma plataforma de venda de jogos e gestão de usuários, voltada para a comunidade FIAP/Alura/PM3.

## Funcionalidades

1. **Cadastro de Usuários**  
   - Criação de conta com nome, e-mail e senha (mínimo de 8 caracteres, incluindo letras, números e caractere especial).  
   - Validação de formato de e-mail e regra de senha segura.  
   - Dois níveis de perfil:  
     - **UserApp** (usuário comum) – acesso à própria biblioteca de jogos.  
     - **Admin** – permissão para criar/editar/deletar usuários e jogos, gerenciar futuras funcionalidades (promoções, etc.).  

2. **Autenticação e Autorização**  
   - Login via JWT (JSON Web Token).  
   - Rotas protegidas por roles (“Admin” para operações sensíveis; “UserApp” para operações básicas).  

3. **Gestão de Jogos**  
   - CRUD completo de jogos (título, descrição, preço, categoria, etc.).  
   - Consulta de todos os jogos disponíveis.  
   - Associação de jogos a usuários (biblioteca).  

4. **Persistência de Dados com MongoDB**  
   - Conexão a um banco MongoDB hospedado localmente ou remoto.  
   - Repositórios genéricos para entidades **User** e **Game**.  
   - Migração de “seed” (MongoSeeder) que verifica se existe pelo menos um usuário Admin; caso não exista, insere um Admin padrão.  

5. **Qualidade de Software**  
   - Projetado seguindo princípios de **Domain-Driven Design (DDD)**:  
     - Entidades, DTOs, Services e Repositórios bem separados.  
   - Middleware de tratamento global de exceções e logs estruturados.  
   - Testes unitários (xUnit) para **UserService** e **GameService**.  

6. **Documentação Swagger**  
   - Gerada automaticamente em ambiente de desenvolvimento, com descrições de todos os endpoints protegidos.  

---

## Tecnologias Utilizadas

- **Linguagem**: C#  
- **Framework**: .NET 8 (ASP.NET Core)  
- **Banco de Dados**: MongoDB (via MongoDB.Driver)  
- **Autenticação/Autorização**: JWT (Microsoft.AspNetCore.Authentication.JwtBearer)  
- **Documentação de API**: Swagger (Swashbuckle.AspNetCore)  
- **Testes**: xUnit, Moq  
- **Injeção de Dependência**: Microsoft.Extensions.DependencyInjection  
- **Modelagem de Domínio**: Pastas separadas em Domain, Application, Infraestrutura  
- **Logs e Middleware**: Serilog  
- **Containerização (opcional)**: Docker, Docker Compose  

---

## Pré-requisitos

Antes de rodar o projeto, você deverá ter instalado em sua máquina:

- **.NET SDK 8.x**  
- **MongoDB Community Server** (instalado localmente ou via Docker)  
- (Opcional) **Docker e Docker Compose**  
- Editor de código (VS Code, Visual Studio, Rider etc.)  

---

## Instalação e Configuração

### 1. Clone do Repositório

``` bash
git clone https://github.com/AstSanX2/FCGTech.git
cd FCGTech/FCG.API
```

> Observação: este README assume que o arquivo FCGTech.sln está na raiz do diretório clonado.

### 2. Configuração do MongoDB

- Se você já possui MongoDB instalado localmente, garanta que o serviço esteja rodando em mongodb://localhost:27017.  
- Caso queira usar Docker, execute o seguinte comando (tudo em uma única linha):  
  docker run -d --name mongodb-fcg -p 27017:27017 mongo:6.0

### 3. Variáveis de Configuração (appsettings.json)

Em FCG.API/appsettings.json, configure:
```json
{  
  "Logging": {  
    "LogLevel": {  
      "Default": "Information",  
      "Microsoft.AspNetCore": "Warning"  
    }  
  },  
  "AllowedHosts": "*",  

  "MongoDB": {  
    // Exemplo: mongodb://localhost:27017/fcg  
    "ConnectionString": "mongodb://localhost:27017/fcg"  
  },  

  "JwtOptions": {  
    // Gere uma chave aleatória (pelo menos 32 caracteres)  
    "Key": "suaChaveMuitoSecretaAquiCom32CaracteresMinimo",  
    "Audience": "FCGapi-audience-exemplo",  
    "Issuer": "FCGapi-issuer-exemplo"  
  }  
}
```

- ConnectionString: URI de conexão ao MongoDB + nome do banco (ex: mongodb://localhost:27017/fcg).  
- JwtOptions:Key: chave secreta usada para assinar os tokens JWT.  
- JwtOptions:Audience e JwtOptions:Issuer: identidades que você define para seu token.  

---

## Como Executar a Aplicação

### 1. Rodando localmente

1. Acesse a pasta da solução:  
   cd FCGTech

2. Restaure pacotes e compile:  
   dotnet restore  
   dotnet build

3. Navegue até o projeto API e rode:  
   cd FCG.API  
   dotnet run

   - Em ambiente de Development, a aplicação escuta em https://localhost:5001 e http://localhost:5000 (conforme launchSettings.json).

4. Verifique se o MongoDB está rodando em localhost:27017.  
   - Nos primeiros logs, o MongoSeeder verifica se existe um usuário com role = Admin. Se não houver, insere um Admin “padrão” (altere credenciais manualmente ou via código).

5. Acesse o Swagger para documentação e testes:  
   https://localhost:5001/swagger/index.html

---

### 2. Docker (Opcional)

1. Crie uma imagem Docker do projeto:  
   cd FCG.API  
   docker build -t fcg-api:latest .

2. Execute um container para o MongoDB (caso necessário):  
   docker run -d --name mongodb-fcg -p 27017:27017 mongo:6.0

3. Execute o container da API (ajuste o caminho do appsettings.json e a ConnectionString para apontar ao MongoDB correto):  
   docker run -d --name fcg-api -p 5000:80 -e ASPNETCORE_ENVIRONMENT=Production -v /caminho/para/appsettings.json:/app/appsettings.json fcg-api:latest

4. A API ficará acessível em:  
   http://localhost:5000/swagger/index.html

> Dica: crie um docker-compose.yml para orquestrar MongoDB + API, definindo variáveis de ambiente de conexão.

---

## Testes Automatizados

Este projeto inclui testes de unidade (xUnit) que validam as regras de negócio dos services. Para executar:

1. Navegue até a solução principal:  
   cd FCGTech

2. Execute todos os testes:  
   dotnet test

- Os testes estão em FCG.Tests/ServiceTests, incluindo UserServiceTests.cs e GameServiceTests.cs.  
- Não é necessário nenhum servidor em execução, pois usam repositórios mockados via Moq.

---

## Arquitetura e Organização do Código

O projeto segue princípios de Domain-Driven Design (DDD), com camadas bem definidas:

FCGTech/  
├─ FCG.API/                  ← Projeto principal (.NET 8)  
│   ├─ Controllers/          ← Controladores (API Endpoints)  
│   │   ├─ AuthenticationController.cs  
│   │   ├─ UsersController.cs  
│   │   └─ GameController.cs  
│   │  
│   ├─ Domain/               ← Entidades, DTOs, Enums e Interfaces  
│   │   ├─ Entities/         ← Classes de domínio (User, Game, BaseEntity)  
│   │   ├─ DTO/              ← Objetos de transferência (Create, Update, Project, Filter)  
│   │   ├─ Enums/            ← Enumeração de UserRole, GameCategory etc.  
│   │   └─ Interfaces/       ← Interfaces de repositórios (IUserRepository, IGameRepository),  
│   │                         serviços (IUserService, IGameService, IAuthenticationService)  
│   │  
│   ├─ Application/          ← Implementações de serviços de domínio  
│   │   └─ Services/  
│   │       ├─ AuthenticationService.cs  
│   │       ├─ UserService.cs  
│   │       └─ GameService.cs  
│   │  
│   ├─ Infraestrutura/       ← Acesso a dados e configuração (MongoDB)  
│   │   ├─ Repositories/     ← Implementação de CRUD em MongoDB (UserRepository.cs, GameRepository.cs, BaseRepository.cs)  
│   │   ├─ Migration/        ← Seeder para criação de Admin padrão (MongoSeeder.cs)  
│   │   ├─ Options/          ← Opções de configuração (JwtOptions.cs)  
│   │   └─ Security/         ← Arquivos relacionados a políticas de autorização (caso necessário)  
│   │  
│   ├─ Middlewares/          ← Middleware global de log e tratamento de erro (GlobalRequestMiddleware.cs)  
│   ├─ Helpers/              ← Classes utilitárias (Conversores de ObjectId, extensões etc.)  
│   ├─ Program.cs            ← Ponto de entrada (configuração de DI, JWT, Swagger, pipeline)  
│   ├─ appsettings.json      ← Configurações de conexão MongoDB e JWT  
│   └─ Dockerfile            ← Definição da imagem Docker (opcional)  
│  
├─ FCG.Tests/                ← Projeto de testes xUnit (mock usando Moq)  
│   └─ ServiceTests/         ← Testes de UserService e GameService  
│  
├─ FCG.sln                   ← Solução .NET que referencia FCG.API e FCG.Tests  
├─ LICENSE.txt               ← Licença do projeto (ex: MIT)  
└─ README.md                 ← Este arquivo  

### Padrões e Boas Práticas

- **Injeção de Dependência (DI)**  
  - Program.cs registra IMongoClient, repositórios (IUserRepository, IGameRepository), serviços (IUserService, IGameService, IAuthenticationService) e middleware.  
  - Facilita trocas de implementação (por exemplo, trocar MongoDB por outro banco).  

- **Repositories Genéricos**  
  - BaseRepository<T> define operações comuns (GetById, GetAll, Create, Update, Delete).  
  - GameRepository e UserRepository herdam de BaseRepository<T> e adicionam métodos específicos quando necessário.  

- **Services**  
  - UserService e GameService contêm lógica de negócio (ex.: validação de e-mail único, regras de senha, relação usuário–jogos).  
  - AuthenticationService trata fluxo de login e geração de JWT.  

- **DTOs (Data Transfer Objects)**  
  - Objetos específicos para Create, Update, Filter e Projection de User e Game.  
  - Garantem que apenas os campos necessários sejam expostos nos endpoints.  

- **Middleware**  
  - GlobalRequestMiddleware captura exceções não tratadas e retorna resposta JSON padronizada.  
  - Também registra logs estruturados de cada requisição e resposta.  

- **Seed de Admin**  
  - MongoSeeder (implementado como IHostedService) executa ao iniciar a aplicação e verifica se já existe um usuário Admin. Se não houver, cria um Admin “padrão”.

---

## Documentação Swagger

Em **ambiente de desenvolvimento** (ASPNETCORE_ENVIRONMENT=Development), o Swagger é habilitado automaticamente. Após executar dotnet run, abra no navegador:

https://localhost:5001/swagger/index.html

- Explore todos os schemas de request/response.  
- Teste cada rota diretamente.  
- Use o botão “Authorize” para inserir o Bearer Token e testar rotas protegidas.
  
---

## Documentação MongoDB

### User
Campos:
- _id (ObjectId): chave primária gerada automaticamente pelo MongoDB.
- Name (String): nome do usuário. Exemplo: "Guilherme".
- Email (String): e-mail do usuário. Exemplo: "guilherme@gmail.com".
- Password (String): hash da senha do usuário .Exemplo: "466c7a2f84f0acda77081a4ae5572c52486577b05e3ee25f8c69b26dcdab".
- Role (NumberInt): nível de acesso do usuário. Exemplo: 2 (inteiro). Pode corresponder a perfis como:
  - 1 = Admin (administrador)
  - 2 = UserApp (usuário comum)

Exemplo de documento real na coleção Users:
``` json
{
  "_id" : ObjectId("683e2f259b0f3a4dd9778bc7"),
  "Name" : "Guilherme",
  "Email" : "guilherme@gmail.com",
  "Password" : "466c7a2f84f0acda77081a4ae5572c52486577b05e3ee25f8c69b26dcdab",
  "Role" : NumberInt(2)
}
```

### Game

Campos:
- _id (ObjectId): chave primária gerada automaticamente pelo MongoDB.
- Name (String): nome do jogo.
- Description (String): descrição breve do jogo.
- Category (String): categoria do jogo.
- ReleaseDate (Date - ISODate): data de lançamento do jogo.
- LastUpdateDate (Date - ISODate): data da última atualização do registro do jogo. 
- Price (NumberDecimal): preço do jogo.

Exemplo de documentos reais na coleção Games:
``` json
{
  "_id" : ObjectId("683b64ae483edd08343a5645"),
  "Name" : "GTA",
  "Description" : "CJ Retorna para sua cidade...",
  "Category" : "Ação",
  "ReleaseDate" : ISODate("2025-05-31T20:20:38.990+0000"),
  "LastUpdateDate" : ISODate("2025-05-31T20:20:38.990+0000"),
  "Price" : NumberDecimal("100.59")
},
{
  "_id" : ObjectId("683e2ed59b0f3a4dd9778bc6"),
  "Name" : "Homem Aranha",
  "Description" : "O Rei do crime está de volta...",
  "Category" : "Aventura",
  "ReleaseDate" : ISODate("2025-06-02T23:07:22.129+0000"),
  "LastUpdateDate" : ISODate("2025-06-02T23:07:22.129+0000"),
  "Price" : NumberDecimal("50.10")
}
```

### Índices (Indexes)

- Users:
  - { Email: 1 } (unique: true)  
    - Para garantir que não haja dois usuários com o mesmo e-mail.

- Games:
  - { Name: 1 }  
    - Para acelerar buscas por nome do jogo.
  - { Category: 1 }  
    - Para filtragem por categoria.
  - { ReleaseDate: -1 }  
    - Para ordenação rápida por data de lançamento.


### Justificativas de Modelagem

- Na coleção Users:
  - O campo Email é indexado como único para impedir duplicidade de cadastros.
  - A Role é armazenada como NumberInt para facilitar comparações numéricas de permissão.

- Na coleção Games:
  - Os campos ReleaseDate e LastUpdateDate utilizam ISODate para manter padrão de data/hora compatível com o driver MongoDB.
  - O uso de NumberDecimal em Price garante precisão ao armazenar valores monetários.
  - Não há embedding de informações de usuário dentro de Games, pois a relação principal é gerenciada pela lógica da aplicação (por exemplo, uma tabela de associação ou conservação de IDs em arrays se necessário em fases futuras).

