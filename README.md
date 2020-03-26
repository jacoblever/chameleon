# chameleon

Powers [https://chameleon.jacoblever.dev/](https://chameleon.jacoblever.dev/)


## Setup instructions

Install:
* .NET Core 2.1 https://dotnet.microsoft.com/download/dotnet-core/2.1
* Python3
* Node.js https://nodejs.org/en/
* Docker (and login)

Setup:
* pip install aws-sam-cli
* pip install awscli
* aws configure (here you need aws credentials)
* cd chameleon-webapp
* npm install

Run backend:
* sam build
* sam local start-api

Run frontend:
* cd chameleon-webapp
* npm start
