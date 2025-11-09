## 🏍️ MottuVisualizer

API RESTful desenvolvida em ASP.NET Core para o gerenciamento e rastreamento de motocicletas nos pátios da Mottu.
A aplicação oferece operações completas de CRUD, integração com banco de dados Oracle via Entity Framework Core e documentação automatizada com Swagger UI.

## 👥 Integrante

- Diego Bassalo Canals Silva – RM558710 | Turma 2TDSPG
- Giovanni de Souza Lima – RM556536 | Turma 2TDSPH
- Vitor Tadeu Soares de Sousa – RM559105 | Turma 2TDSPH

## 🎯 Propósito do Sistema

A Mottu enfrenta dificuldades na organização e localização rápida das motos em seus pátios, o que afeta diretamente a eficiência das operações diárias.
Para resolver esse cenário, o MottuVisualizer foi criado com foco em controle, automação e rastreabilidade.

A API possibilita:

Cadastro de motos em seus respectivos setores;

Gestão dos setores físicos do pátio;

Registro das movimentações internas através da leitura de QR Codes.

Com isso, a empresa obtém mais agilidade, precisão e transparência na gestão dos veículos.

## ⚙️ Passos para Execução
1. Clone o repositório:
   ```bash
   git clone https://github.com/DGMMX/MottuVisualizer_Api.git
   cd MottuVisualizer_Api
   ```

2. Configure a connection string do Oracle no arquivo `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "OracleConnection": "User Id=seu_usuario;Password=sua_senha;Data Source=seu_host:porta/seu_servico"
   }
   ```

3. Crie o banco de dados e aplique as migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. Execute a aplicação:
   ```bash
   dotnet run
   ```


Após iniciar, acesse a interface do Swagger para visualizar e testar os endpoints:

🔗  ```
     http://localhost/swagger/index.html
     ```

## 🧪 Testando as Rotas
➤ 1. Criar um setor

Crie primeiro um setor para estruturar o pátio:

 ```
POST /api/setores
{
  "nome": "Disponível"
}
   ```

➤ 2. Cadastrar uma moto

Adicione uma moto associando-a ao setor desejado:

 ```
POST /api/motos
{
  "placa": "ABC-1234",
  "setorId": 1
}
   ```
➤ 3. Movimentar a moto entre setores

Registre uma movimentação informando o setor antigo e o novo:

```
POST /api/movimentacoes/movimentacoes?motoId={id}&novoSetorId={id}
 ```

## 🚀 Tecnologias Utilizadas

ASP.NET Core 9

Entity Framework Core

Oracle Database

Swagger (Swashbuckle)

C# 12

## 💡 Principais Benefícios

Rastreabilidade total de motos em tempo real;

Controle centralizado das informações de pátio;

Facilidade de uso e integração via Swagger;

Arquitetura limpa, escalável e de fácil manutenção.

## 📌 Considerações Finais

O projeto MottuVisualizer foi desenvolvido com o objetivo de simular uma solução corporativa real, aplicando boas práticas de desenvolvimento e organização de código.
Sua arquitetura permite expansões futuras, como dashboards de visualização e integração com sistemas externos de monitoramento.
