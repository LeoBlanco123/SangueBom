
# 🩸 SangueBom

**SangueBom** é uma aplicação web desenvolvida com ASP.NET Core, voltada para o gerenciamento de doações de sangue. O sistema contempla o cadastro de doadores, campanhas de doação e o registro de doações realizadas, com foco em escalabilidade, organização em camadas e boas práticas de desenvolvimento baseadas no DDD (Domain-Driven Design).

## 🎯 Objetivos do Projeto

- Digitalizar o processo de coleta e controle de doações de sangue.
- Facilitar a gestão de doadores e campanhas.
- Implementar boas práticas de desenvolvimento com C# e .NET.

## 🧰 Stack Tecnológica

- **.NET 6+ / ASP.NET Core** – Backend estruturado em camadas (Domain, Application, Infrastructure).
- **Entity Framework Core** – ORM para mapeamento objeto-relacional.
- **xUnit** – Testes automatizados de unidade.
- **Razor Pages** – Interface web leve e integrada ao ASP.NET.
- **C# 10+** – Linguagem principal do backend.


## 🧪 Testes

Os testes automatizados utilizam o framework **xUnit**, com foco em:

- Validação de regras de negócio (ex: CPF duplicado).
- Testes de Value Objects (ex: CPF).
- Repositórios in-memory para simulação.

Executar testes:

```bash
dotnet test
```

## 🚀 Executando o Projeto

### Requisitos

- [.NET SDK 6 ou superior](https://dotnet.microsoft.com/download)
- Visual Studio 2022+ ou VS Code com extensão C#

### Instruções

```bash
git clone https://github.com/LeoBlanco123/SangueBom.git
cd SangueBom
dotnet restore
dotnet build
dotnet run --project SangueBom
```

A aplicação estará disponível em `https://localhost:5001` (ou conforme configurado).

## 📌 Padrões e Boas Práticas

- **DDD (Domain-Driven Design)** básico com separação clara entre camadas.
- **Repository Pattern** para acesso a dados.
- **Value Objects** para validações ricas de domínio (ex: CPF).
- **TDD (Test-Driven Development)** para regras críticas de negócio.

---

**Desenvolvido para fins educacionais e demonstração de arquitetura limpa com ASP.NET Core.**
