FROM node:10.15.3-alpine as production

ENV PORT=6000

WORKDIR /home/chameleon/chameleon-webapp

COPY package.json package-lock.json ./
RUN npm install --no-color --silent

COPY . .

RUN npm run build
