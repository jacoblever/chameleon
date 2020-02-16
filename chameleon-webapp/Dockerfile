FROM node:10.15.3-alpine as production

ENV PORT=6000
ARG ENV_FILE_POSTFIX=production

WORKDIR /home/chameleon/chameleon-webapp

COPY package.json package-lock.json ./
RUN npm install --no-color --silent

COPY . .

RUN export $(egrep -v '^#' .env.${ENV_FILE_POSTFIX} | xargs) && npm run build
