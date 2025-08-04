# Minimal API com Docker e MySQL

Este projeto é uma aplicação ASP.NET Core Minimal API configurada para rodar com Docker e MySQL via Docker Compose.

---

## 🛠 Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

---

## 🚀 Como rodar

### 1. Clonar o repositório

```bash
git clone https://github.com/seu-usuario/minimal-api.git
cd minimal-api
2. Gerar a imagem da API
bash
Copiar
Editar
docker build -t minimal-api:1.0 .
3. Subir os containers com Docker Compose
bash
Copiar
Editar
docker compose up -d
Isso irá:

Subir um container MySQL (database) na porta 3306

Subir sua API (api) na porta 5000

4. Acessar a API
Abra no navegador ou com curl:

arduino
Copiar
Editar
http://localhost:5000
🧪 Testando conexão com o banco
A string de conexão no app deve usar o nome do serviço MySQL como host:

pgsql
Copiar
Editar
Server=database;Port=3306;Database=minimal_api;User=root;Password=root;
📂 Estrutura do projeto
bash
Copiar
Editar
.
├── Api/                    # Projeto .NET
├── Dockerfile             # Imagem da API
├── docker-compose.yml     # Orquestração da API + MySQL
├── .dockerignore
└── README.md
🧹 Parar e limpar os containers
bash
Copiar
Editar
docker compose down
📝 Observações
Certifique-se de que a porta 5000 (API) e 3306 (MySQL) estejam livres no seu sistema.

A imagem é baseada em .NET 9.0 e usa ASPNETCORE_URLS=http://+:5000 para escutar conexões externas no container.

📄 Licença
Este projeto está sob a licença MIT.

yaml
Copiar
Editar

---

Se quiser, posso ajustar esse `README` para incluir detalhes específicos do seu projeto (como endpoints, auth, migrations etc). Quer que eu personalize com base no que já existe na sua API?








Perguntar ao ChatGPT
