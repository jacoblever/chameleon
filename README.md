# chameleon

Powers [https://chameleon.jacoblever.dev/](https://chameleon.jacoblever.dev/)

## What is chameleon?

Players join the same room by entering the room code, then start a game. You need 3 or more people to play.

Everyone in the game is given the same word, apart from the chameleon. The players take it in turns to say a single word that relates to the word given. The chameleon tries to blend in and hide the fact that they don't know the word. At the end of the round, players then guess who they think the chameleon is.

## Infrastructure

The app is made up of a C# backend (in the src/ directory) and a JavaScript React frontend (in chameleon-webapp/ directory). 
The backend consists of an AWS Lambda Function with DynamoDB for persistence and is managed by an [AWS SAM template](https://github.com/jacoblever/chameleon/blob/master/template.yaml).

## Installation instructions
Install the following:
* .NET Core 2.1 https://dotnet.microsoft.com/download/dotnet-core/2.1
* Python3
* Node.js (https://nodejs.org/en/)
* Docker and make sure you open it and login (https://docs.docker.com/install/)

Run the following from the root of the repo
* `pip install aws-sam-cli`
* `pip install awscli`
* `aws configure` (here you need aws credentials)
* `cd chameleon-webapp`
* `npm install`

## Running the app locally
In one Terminal window - run the backend (or run `make back`):
* `sam build`
* `sam local start-api --port 3001`

In another Terminal window - run the frontend (or run `make front`):
* `cd chameleon-webapp`
* `npm start`
