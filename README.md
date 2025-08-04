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

yaml
Copiar
Editar

---

## 🚀 Executando com Docker

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
bash
Copiar
Editar
docker run -it --rm \
  --name minimal-api \
  -p 5000:5000 \
  --network minimal_api_internal_net \
  minimal-api:1.0
🔗 Acessar a API
Depois que estiver rodando, acesse via navegador ou Postman:

arduino
Copiar
Editar
http://localhost:5000
🔐 Autenticação
A API utiliza autenticação com token JWT. Ao fazer login, você recebe um token que deve ser usado nas requisições protegidas com o cabeçalho:

http
Copiar
Editar
Authorization: Bearer <seu_token>
🧪 Testes
O projeto contém testes automatizados no diretório Test/.

Para rodar os testes:

bash
Copiar
Editar
dotnet test
📝 Licença
Este projeto está licenciado sob os termos da MIT License.

yaml
Copiar
Editar

---

Se quiser, posso gerar esse arquivo e te passar o conteúdo pronto para colar ou até te orientar sobre como fazer o commit no GitHub diretamente. Deseja isso?








Perguntar ao ChatGPT
