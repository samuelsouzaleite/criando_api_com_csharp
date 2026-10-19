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
- **JWT** (`Microsoft.AspNetCore.Authentication.JwtBearer`) para autenticação e autorização
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
- [x] Controllers de Curso, Turma, Matrícula, Nota e Usuário, com CRUD completo
- [x] Exceções próprias do domínio (`NotFoundException`, `BadRequestException`)
- [x] Tratamento global de erros com middleware, traduzindo exceções em status HTTP e devolvendo um JSON padronizado
- [x] Autenticação com **JWT**: login, token assinado com validade de 1 hora e botão *Authorize* no Swagger
- [x] Autorização por perfil (Administrador e Aluno) declarada endpoint a endpoint
- [x] Endpoints que devolvem apenas os dados do próprio aluno, com o id lido do token

## 🚧 Em andamento

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

### 2. Configurar os segredos

As credenciais **não** ficam no repositório. O `appsettings.json` traz apenas o formato, com os valores vazios.

Para rodar localmente, use o User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "User ID=postgres;Password=SUA_SENHA;Host=localhost;Port=5432;Database=Escola;Pooling=true;Connection Lifetime=0;" --project Escola.API
```

A chave que assina os tokens JWT também fica fora do repositório. Gere uma aleatória e guarde do mesmo jeito:

```bash
$bytes = New-Object byte[] 48; [Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes); dotnet user-secrets set "Jwt:SecretKey" ([Convert]::ToBase64String($bytes)) --project Escola.API
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

Quatro entidades expõem o CRUD completo, seguindo o mesmo padrão de rotas:

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/{entidade}` | Lista todos os registros |
| `GET` | `/api/{entidade}/{id}` | Busca um registro pelo id |
| `POST` | `/api/{entidade}` | Cria um novo registro |
| `PUT` | `/api/{entidade}` | Atualiza um registro existente |
| `DELETE` | `/api/{entidade}/{id}` | Remove um registro |

Onde `{entidade}` é **Curso**, **Turma**, **Matricula** ou **Nota**.

As consultas de Turma e Matrícula devolvem os dados relacionados (o curso da turma, o usuário e a turma da matrícula).

### Autenticação

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `POST` | `/api/Usuario` | público | Cadastra um usuário. O primeiro vira **Administrador**, os demais **Aluno** |
| `POST` | `/api/Usuario/login` | público | Valida e-mail e senha e devolve o token JWT |

O token vale **1 hora** e carrega o id, o e-mail e o perfil do usuário. Para usá-lo no Swagger, clique em **Authorize** e informe o token.

### Rotas do próprio aluno

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/api/Turma/user` | Aluno, Administrador | Turmas em que o usuário logado está matriculado |
| `GET` | `/api/Nota/user/turma/{id}` | Aluno, Administrador | Notas do usuário logado em uma turma |

Nessas rotas o id do usuário é lido **do token**, nunca de um parâmetro enviado pelo cliente. Sem isso, bastaria trocar um número na requisição para acessar os dados de outro aluno.

As demais rotas exigem o perfil **Administrador**.

### Respostas de erro

Qualquer exceção é interceptada pelo middleware e devolvida em um formato único:

```json
{
  "statusCode": "404",
  "message": "Curso não encontrado.",
  "details": "..."
}
```

| Situação | Status |
|---|---|
| Registro não encontrado (`NotFoundException`) | `404` |
| Requisição inválida (`BadRequestException`) | `400` |
| Sem token, ou token inválido / expirado | `401` |
| Token válido, mas sem permissão para a rota | `403` |
| Qualquer outra exceção | `500` |

O campo `details` traz o stack trace apenas em ambiente de desenvolvimento.

---

## 📁 Estrutura de pastas

```
criando_api_com_csharp/
├── Escola.API/                    # Controllers, Program.cs, Dockerfile
│   ├── Controllers/
│   ├── Errors/                    # Contrato de resposta de erro
│   └── Middleware/                # Tratamento global de exceções
├── Escola.Application/            # Casos de uso
│   ├── DTOs/                      # Um DTO por operação
│   ├── Exceptions/                # Exceções próprias do domínio
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

Dois segredos precisam existir para a aplicação rodar: a **senha do banco** e a **chave que assina os tokens JWT**. Nenhum dos dois entra no repositório.

| Onde roda | De onde vêm os segredos |
|---|---|
| Local (`dotnet run`) | User Secrets, fora da pasta do projeto |
| Docker | arquivo `.env`, ignorado pelo Git |
| `appsettings.json` | só o formato, com os valores vazios |

A chave do JWT merece o mesmo cuidado que a senha do banco, ou mais: quem a tiver consegue **forjar um token válido** e se passar por qualquer usuário, inclusive administrador, sem precisar de senha.

---

## 📚 Créditos

Projeto desenvolvido acompanhando o curso **"Criando uma API com .NET 10"** do canal [Café com Bug](https://www.youtube.com/@cafecombug).

Um diferencial do meu caminho: o curso usa **Visual Studio**, e eu fiz tudo pelo **VS Code**, traduzindo cada passo para a linha de comando do `dotnet`. Deu mais trabalho e ensinou muito mais.

---

## 👤 Autor

[![LinkedIn](https://img.shields.io/badge/LinkedIn-conectar-0A66C2?style=flat-square&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/samuel-souza-leite-43281b252/)

