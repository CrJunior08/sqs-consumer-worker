# AWS - SQS Consumer Worker

## Descrição
Este projeto é um worker desenvolvido para consumir mensagens de uma fila AWS SQS, utilizando **LocalStack** para simular a infraestrutura da AWS localmente. Ele faz parte do projeto API-Product-NET8, que realiza operações CRUD para criar produtos e envia mensagens para a **fila SQS**, mas também pode funcionar de forma independente, consumindo mensagens de qualquer fila SQS previamente configurada no LocalStack.

## Requisitos
- .NET 8.0
- Docker
- LocalStack (para simulação da AWS SQS)
- Redis e MongoDB (se necessário para a aplicação completa)

## Instalação

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/CrJunior08/sqs-consumer-worker.git

2. **Entre na pasta do projeto:**

     ```bash
    cd sqs-consumer-worker

## Como rodar

Opção 1: **Usando a API de Produtos**
Este worker pode ser usado em conjunto com a API de Produtos que envia mensagens para a fila SQS. Siga os passos abaixo para rodar ambas as aplicações:

1. **Montar o docker MongoDB e Redis:**

   ```bash
   docker run -d -p 27017:27017 --name mongodb mongo
   docker run -d -p 6379:6379 --name redis redis

2. **Executar o LocalStack para simular a fila SQS:**

   ```bash
   baixe e instale: https://app.localstack.cloud/getting-started
   abra o CMD e rode: localstack start
   Entre no site: https://app.localstack.cloud/sign-in
   E selecione o SQS na aba status.

3. **Criar a fila SQS no LocalStack:**
   
    ```bash
    aws --endpoint-url=http://localhost:4566 sqs create-queue --queue-name Product

4. **Clonar e rodar a API de produtos:**
   ```bash
   https://github.com/CrJunior08/API-Product-NET8.git
Após a configuração da API de Produtos, siga as instruções no repositório da API para rodá-la e começar a enviar mensagens para a fila SQS.


5. **Rodar o worker**
   
     ```bash
     Aperte no play com o nome da aplicação e execute


Opção 2: **Usando de Forma Independente**
Você pode rodar este worker para consumir qualquer fila configurada no LocalStack, sem a necessidade de rodar a API de Produtos. Precisando rodar apenas o LocalStack

**Siga os passos anteriores para rodar o localStack**

1. **Enviar mensagem para a fila**
   
     ```bash
    aws --endpoint-url=http://localhost:4566 sqs send-message --queue-url http://localhost:4566/000000000000/Product --message-body "Mensagem de Teste"


2. **Rodar o worker**
   
     ```bash
     Aperte no play com o nome da aplicação e execute
