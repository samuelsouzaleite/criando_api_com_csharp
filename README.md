# 🎓 Escola.API

API REST de gestão escolar construída com **.NET 10** e **PostgreSQL**, seguindo os princípios da **Arquitetura Limpa**.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-13-239120?style=flat-square&logo=csharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?style=flat-square&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?style=flat-square&logo=docker&logoColor=white)
![Status](https://img.shields.io/badge/status-em%20desenvolvimento-yellow?style=flat-square)

---

## 📌 Sobre o projeto

O domínio é a gestão de uma escola: cursos, turmas, alunos, matrículas e notas.

O objetivo não é só fazer funcionar, e sim praticar **separação de responsabilidades**. Cada camada conhece apenas o que precisa, e a API não sabe que existe banco de dados.

---

## 🛠️ Tecnologias

- **.NET 10** / ASP.NET Core Web API
- **Entity Framework Core 10** com abordagem **Code First**
- **PostgreSQL** (provider Npgsql)
- **Swagger / OpenAPI** (Swashbuckle)
- **Docker** e Docker Compose
- **User Secrets** e `.env` para gerenciamento de credenciais

---

## 🧱 Arquitetura

A solução é dividida em cinco projetos, com as dependências sempre apontando para dentro, na direção do domínio:

```
Escola.API ──► Escola.Infra.Ioc ──┬──► Escola.Application ──► Escola.Domain
                                  ├──► Escola.Infra.Data ────► Escola.Domain
                                  └──► Escola.Domain
```

| Projeto | Responsabilidade |
|---|---|
| **Escola.Domain** | Entidades e interfaces dos repositórios. É o núcleo, não depende de ninguém. |
| **Escola.Application** | Casos de uso, services e DTOs. As regras de negócio da aplicação. |
| **Escola.Infra.Data** | DbContext, configurações das entidades, repositories e migrations. |
| **Escola.Infra.Ioc** | Injeção de dependência. Liga as interfaces às implementações. |
| **Escola.API** | Controllers e configuração do pipeline HTTP. |

A API referencia **apenas** o `Infra.Ioc`. Trocar o banco de dados amanhã não exigiria mudar nenhum controller.

---

## 📊 Modelo de dados

```
Curso ──1:N──► Turma ──1:N──► Matricula ──1:N──► Nota
                                   ▲
                                   │ N:1
                                Usuario
```

- Um **Curso** tem várias **Turmas**
- Uma **Turma** tem várias **Matrículas**
- Um **Usuário** (aluno) pode ter várias **Matrículas**
- Cada **Matrícula** acumula várias **Notas**, com aprovação calculada automaticamente

Exclusão é feita por **soft delete**: o registro recebe a marca de excluído em vez de sair do banco.

---

## ✅ O que já está implementado

- [x] Estrutura da solução em camadas
- [x] Entidades e mapeamento com Fluent API (`IEntityTypeConfiguration`)
- [x] Migrations e banco gerado por Code First
- [x] Repositories das cinco entidades
- [x] Services e DTOs separados por operação (Post, Put, Get)
- [x] Injeção de dependência centralizada no `Infra.Ioc`
- [x] Soft delete
- [x] Hash de senha com HMACSHA512 e salt individual por usuário
- [x] Documentação com Swagger
- [x] Container Docker
- [x] Credenciais fora do repositório

## 🚧 Em andamento

- [ ] Controllers das demais entidades (Curso, Usuário, Matrícula, Nota)
- [ ] Tratamento global de erros com middleware
- [ ] Autenticação e autorização com **JWT**
- [ ] Controle de acesso por perfil (aluno, professor, administrador)
- [ ] Paginação nas consultas
- [ ] Testes automatizados

---

## 🚀 Como executar

### Pré-requisitos

- [SDK do .NET 10](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/products/docker-desktop/) (opcional)

### 1. Clonar

```bash
git clone https://github.com/samuelsouzaleite/criando_api_com_csharp.git
cd criando_api_com_csharp
```

### 2. Configurar a connection string

As credenciais **não** ficam no repositório. O `appsettings.json` traz apenas o formato, com a senha vazia.

Para rodar localmente, use o User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "User ID=postgres;Password=SUA_SENHA;Host=localhost;Port=5432;Database=Escola;Pooling=true;Connection Lifetime=0;" --project Escola.API
```

### 3. Criar o banco

O banco é gerado pelas migrations. Não precisa criar as tabelas na mão:

```bash
dotnet ef database update --project Escola.Infra.Data --startup-project Escola.API
```

> Se o `dotnet ef` não estiver instalado: `dotnet tool install --global dotnet-ef`

### 4. Executar

```bash
dotnet run --project Escola.API
```

Swagger em **https://localhost:7256/swagger**

---

## 🐳 Executando com Docker

Crie um arquivo `.env` na raiz do projeto:

```env
DB_PASSWORD=sua_senha
```

E suba o container:

```bash
docker compose up --build
```

Swagger em **http://localhost:5160/swagger**

> Dentro do container, o host do banco é `host.docker.internal`, porque `localhost` apontaria para o próprio container.

---

## 📡 Endpoints

Implementados até aqui:

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/Turma` | Lista todas as turmas com o curso relacionado |
| `GET` | `/api/Turma/{id}` | Busca uma turma pelo id |
| `POST` | `/api/Turma` | Cria uma nova turma |
| `PUT` | `/api/Turma` | Atualiza uma turma existente |
| `DELETE` | `/api/Turma/{id}` | Remove uma turma |

Os services e repositories de Curso, Usuário, Matrícula e Nota já existem. Os controllers vêm nas próximas etapas.

---

## 📁 Estrutura de pastas

```
criando_api_com_csharp/
├── Escola.API/                    # Controllers, Program.cs, Dockerfile
│   └── Controllers/
├── Escola.Application/            # Casos de uso
│   ├── DTOs/                      # Um DTO por operação
│   ├── Interfaces/
│   └── Services/
├── Escola.Domain/                 # Núcleo
│   ├── Entities/
│   └── Interfaces/
├── Escola.Infra.Data/             # Acesso a dados
│   ├── Context/
│   ├── EntitiesConfiguration/     # Fluent API
│   ├── Migrations/
│   └── Repositories/
├── Escola.Infra.Ioc/              # Injeção de dependência
├── compose.yaml
└── Escola.slnx
```

---

## 🔐 Sobre as credenciais

Nenhuma senha entra no repositório, nem no histórico do Git:

| Onde roda | De onde vem a senha |
|---|---|
| Local (`dotnet run`) | User Secrets, fora da pasta do projeto |
| Docker | arquivo `.env`, ignorado pelo Git |
| `appsettings.json` | só o formato, com o valor vazio |

---

## 📚 Créditos

Projeto desenvolvido acompanhando o curso **"Criando uma API com .NET 10"** do canal [Café com Bug](https://www.youtube.com/@cafecombug).

Um diferencial do meu caminho: o curso usa **Visual Studio**, e eu fiz tudo pelo **VS Code**, traduzindo cada passo para a linha de comando do `dotnet`. Deu mais trabalho e ensinou muito mais.

---

## 👤 Autor

[![LinkedIn](https://img.shields.io/badge/LinkedIn-conectar-0A66C2?style=flat-square&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/samuel-souza-leite-43281b252/)

