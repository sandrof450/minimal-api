# Minimal API com Docker e MySQL

Este projeto é uma aplicação ASP.NET Core utilizando Minimal APIs com autenticação por token, testes automatizados e containerização com Docker.

---

## 📁 Estrutura do Projeto

.
├── Api/ # Projeto principal da API
├── Test/ # Projeto de testes
├── obj/Debug/net9.0/ # Saída de build (ignorada pelo Git)
├── Program.cs # Ponto de entrada da API
├── appsettings.json # Configurações da aplicação
├── appsettings.Development.json
├── minimal-api.csproj # Arquivo de projeto da API
├── minimal-api.sln # Solução do Visual Studio
├── .gitignore
└── README.md # Este arquivo

---

## 🚀 Executando localmente com Docker

### 1. Criar a imagem da API

```bash
docker build -t minimal-api:1.0 .
2. Subir banco de dados com Docker Compose
bash
Copiar
Editar
docker compose up -d
Isso sobe um container MySQL na porta 3306.

3. Executar a API na mesma rede

docker run -it --rm \
  --name minimal-api \
  -p 5000:5000 \
  --network minimal_api_internal_net \
  minimal-api:1.0
🔗 Acesso à API
Ambiente de produção:
👉 http://18.223.2.85/swagger/index.html

Localmente (após rodar via Docker):
👉 http://localhost:5000/swagger

🔐 Autenticação
A API utiliza autenticação com token JWT. Ao fazer login, você recebe um token que deve ser usado nas requisições protegidas com o cabeçalho:

Authorization: Bearer <seu_token>
Exemplo de login via Swagger:

POST /administradores/login
{
  "email": "administrador@teste.com",
  "senha": "123456"
}
🧪 Testes
O projeto contém testes automatizados no diretório Test/.

Para rodar os testes:

dotnet test
📝 Licença
Este projeto está licenciado sob os termos da MIT License.

---
