# chameleon

Powers [https://chameleon.jacoblever.dev/](https://chameleon.jacoblever.dev/)

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
