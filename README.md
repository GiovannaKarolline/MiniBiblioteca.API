# MiniBiblioteca.API

MiniBiblioteca.API is a lightweight ASP.NET Core Web API for managing a small book catalog. The project exposes a simple REST interface for registering and listing books, backed by SQL Server and Entity Framework Core.

## Features

- Create books with title and ISBN
- List all saved books
- ASP.NET Core Web API with controller-based routing
- Entity Framework Core integration with SQL Server
- Repository + service layer separation for cleaner architecture

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- C#

## Project Structure

```text
MiniBiblioteca.API/
├── MiniBiblioteca/
│   ├── Controllers/
│   │   └── LivroController.cs
│   ├── Data/
│   │   └── BibliotecaContext.cs
│   ├── DTOs/
│   │   └── CriarLivroDTO.cs
│   ├── Migrations/
│   ├── Models/
│   │   └── Livro.cs
│   ├── Repositories/
│   │   ├── Interfaces/
│   │   └── LivroRepository.cs
│   ├── Services/
│   │   ├── Interfaces/
│   │   └── LivroService.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── MiniBiblioteca.API.csproj
│   ├── MiniBiblioteca.http
│   ├── Program.cs
│   └── Properties/
├── MiniBiblioteca.API.slnx
├── .gitignore
├── .gitattributes
└── README.md