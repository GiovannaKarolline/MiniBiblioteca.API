# MiniBiblioteca.API

API REST leve em ASP.NET Core para gerenciar um pequeno catálogo de livros. Permite cadastrar e listar livros, com persistência em SQL Server via Entity Framework Core.

O projeto é pequeno de propósito: serve como exemplo de uma arquitetura em camadas (Controller → Service → Repository) fácil de ler e de estender.

## Sumário

- [Funcionalidades](#funcionalidades)
- [Tecnologias](#tecnologias)
- [Pré-requisitos](#pré-requisitos)
- [Como executar](#como-executar)
- [Como usar a API](#como-usar-a-api)
- [Como funciona](#como-funciona)
- [Estrutura do projeto](#estrutura-do-projeto)
- [Migrations](#migrations)
- [Limitações conhecidas e próximos passos](#limitações-conhecidas-e-próximos-passos)

## Funcionalidades

- Cadastro de livros com título e ISBN
- Listagem de todos os livros cadastrados
- Validação automática do corpo da requisição (campos obrigatórios)
- Separação em camadas (Controller, Service, Repository) com injeção de dependência
- Documento OpenAPI gerado automaticamente em ambiente de desenvolvimento

## Tecnologias

- .NET 10 / C#
- ASP.NET Core Web API (controllers)
- Entity Framework Core 10 (SQL Server)
- SQL Server (Express ou superior)

## Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server ou SQL Server Express acessível na sua máquina
- Ferramenta de linha de comando do EF Core:

```bash
dotnet tool install --global dotnet-ef
```

## Como executar

**1. Clone o repositório**

```bash
git clone <url-do-repositorio>
cd MiniBiblioteca.API
```

**2. Configure a connection string**

Edite `MiniBiblioteca/appsettings.json` e ajuste a chave `Default` para a sua instância do SQL Server:

```json
{
  "ConnectionStrings": {
    "Default": "Server=SEU_SERVIDOR\\SQLEXPRESS;Database=mini_biblioteca_db;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Dica: para não versionar dados da sua máquina, deixe sua connection string em `appsettings.Development.json` ou nos [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) (`dotnet user-secrets set "ConnectionStrings:Default" "..."`).

**3. Crie o banco de dados**

```bash
cd MiniBiblioteca
dotnet ef database update
```

**4. Rode a API**

```bash
dotnet run --launch-profile https
```

A API ficará disponível em:

| Perfil | URL |
|--------|-----|
| http   | `http://localhost:5230` |
| https  | `https://localhost:7027` |

Em ambiente de desenvolvimento, o documento OpenAPI fica em `/openapi/v1.json`.

## Como usar a API

Rota base: `/api/livro`

| Método | Rota         | Descrição                  | Sucesso            |
|--------|--------------|----------------------------|--------------------|
| GET    | `/api/livro` | Lista todos os livros      | `200 OK`           |
| POST   | `/api/livro` | Cadastra um novo livro     | `201 Created`      |

### Modelo do livro

```json
{
  "id": "3f2b8c1e-5a47-4c0e-9d51-7b6a1f0e2c11",
  "titulo": "Dom Casmurro",
  "isbn": "978-85-359-0277-5"
}
```

| Campo    | Tipo   | Observação                              |
|----------|--------|-----------------------------------------|
| `id`     | Guid   | Gerado automaticamente pela API         |
| `titulo` | string | Obrigatório                             |
| `isbn`   | string | Obrigatório                             |

### Listar livros

```bash
curl http://localhost:5230/api/livro
```

Resposta `200 OK`:

```json
[
  {
    "id": "3f2b8c1e-5a47-4c0e-9d51-7b6a1f0e2c11",
    "titulo": "Dom Casmurro",
    "isbn": "978-85-359-0277-5"
  }
]
```

Se não houver livros cadastrados, a resposta é uma lista vazia (`[]`).

### Cadastrar um livro

```bash
curl -X POST http://localhost:5230/api/livro \
  -H "Content-Type: application/json" \
  -d '{ "titulo": "Dom Casmurro", "isbn": "978-85-359-0277-5" }'
```

Resposta `201 Created`, com o livro criado (incluindo o `id` gerado):

```json
{
  "id": "3f2b8c1e-5a47-4c0e-9d51-7b6a1f0e2c11",
  "titulo": "Dom Casmurro",
  "isbn": "978-85-359-0277-5"
}
```

### Erros de validação

Se `titulo` ou `isbn` estiverem ausentes ou vazios, a API responde `400 Bad Request` com os detalhes da validação:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Titulo": ["The Titulo field is required."]
  }
}
```

### Usando o arquivo `.http`

O arquivo `MiniBiblioteca/MiniBiblioteca.http` pode ser usado no Visual Studio ou no VS Code (extensão REST Client) para testar a API sem sair do editor. Exemplo de conteúdo:

```http
@MiniBiblioteca_HostAddress = http://localhost:5230

### Listar livros
GET {{MiniBiblioteca_HostAddress}}/api/livro
Accept: application/json

### Cadastrar livro
POST {{MiniBiblioteca_HostAddress}}/api/livro
Content-Type: application/json

{
  "titulo": "Dom Casmurro",
  "isbn": "978-85-359-0277-5"
}
```

## Como funciona

Cada requisição atravessa três camadas, e cada uma tem uma única responsabilidade:

```text
Cliente HTTP
    │
    ▼
LivroController   → recebe a requisição, valida o DTO e devolve o status HTTP
    │
    ▼
LivroService      → regras de negócio: converte o DTO em entidade Livro
    │
    ▼
LivroRepository   → acesso a dados via BibliotecaContext (EF Core)
    │
    ▼
SQL Server        → tabela "Livros"
```

**Fluxo do `POST /api/livro`**

1. O ASP.NET Core desserializa o JSON em um `CriarLivroDTO`. Por causa do atributo `[ApiController]`, se o `[Required]` falhar, o `400` é devolvido automaticamente, antes de o código do controller executar.
2. O `LivroController` chama `ILivroService.Criar(dto)`.
3. O `LivroService` monta um objeto `Livro` com `Titulo` e `ISBN` e o entrega ao repositório.
4. O `LivroRepository` adiciona a entidade ao `DbContext` e chama `SaveChangesAsync()`. O `Id` (Guid) é gerado pelo EF Core nesse momento.
5. O controller responde `201 Created` com o livro salvo.

**Fluxo do `GET /api/livro`**: o controller chama `ILivroService.Listar()`, que delega ao repositório. O repositório executa `ToListAsync()` sobre `Livros`.

**Por que DTO e entidade são separados?** O `CriarLivroDTO` define só o que o cliente pode enviar (`Titulo` e `ISBN`). Assim o cliente nunca controla campos internos, como o `Id`.

**Injeção de dependência** (configurada em `Program.cs`): `ILivroRepository` e `ILivroService` são registrados como `Scoped`, ou seja, uma instância por requisição, o mesmo ciclo de vida do `BibliotecaContext`.

## Estrutura do projeto

```text
MiniBiblioteca.API/
├── MiniBiblioteca/
│   ├── Controllers/
│   │   └── LivroController.cs       # Endpoints HTTP
│   ├── Data/
│   │   └── BibliotecaContext.cs     # DbContext do EF Core
│   ├── DTOs/
│   │   └── CriarLivroDTO.cs         # Contrato de entrada do POST
│   ├── Migrations/                  # Histórico do schema do banco
│   ├── Models/
│   │   └── Livro.cs                 # Entidade
│   ├── Repositories/
│   │   ├── Interfaces/
│   │   │   └── ILivroRepository.cs
│   │   └── LivroRepository.cs       # Acesso a dados
│   ├── Services/
│   │   ├── Interfaces/
│   │   │   └── ILivroService.cs
│   │   └── LivroService.cs          # Regras de negócio
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── MiniBiblioteca.API.csproj
│   ├── MiniBiblioteca.http
│   ├── Program.cs                   # Configuração e DI
│   └── Properties/
│       └── launchSettings.json
├── MiniBiblioteca.API.slnx
├── .gitignore
├── .gitattributes
├── .editorconfig
└── README.md
```

## Migrations

Rode os comandos a partir da pasta `MiniBiblioteca/` (onde está o `.csproj`).

```bash
# Criar uma nova migration após alterar o modelo
dotnet ef migrations add NomeDaMigration

# Aplicar migrations pendentes ao banco
dotnet ef database update

# Reverter a última migration (ainda não aplicada)
dotnet ef migrations remove
```

## Limitações conhecidas e próximos passos

Pontos que ainda não existem e que seriam boas evoluções:

- Buscar, atualizar e remover livros (`GET /api/livro/{id}`, `PUT`, `DELETE`). Observação: o `POST` já devolve o cabeçalho `Location` apontando para `api/livro/{id}`, mas esse endpoint ainda não foi implementado.
- Validação do formato do ISBN e unicidade por ISBN
- Limites de tamanho para `Titulo` e `ISBN` (hoje as colunas são `nvarchar(max)`)
- Paginação na listagem
- Interface visual para a documentação (Swagger UI ou Scalar) sobre o documento OpenAPI
- Testes automatizados (unitários para o service, de integração para os endpoints)
- Retornar um DTO de resposta em vez da entidade `Livro` diretamente