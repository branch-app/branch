FROM node:7
MAINTAINER Alex Forbes-Reed <dockerfile@alx.red>

ENV NODE_ENV="production" \
		PORT="3000"

RUN mkdir -p /usr/local/app
WORKDIR /usr/local/app

COPY package.json /usr/local/app
RUN npm install --production=false --silent

COPY . /usr/local/app
RUN npm run build

EXPOSE 3000
CMD ["npm", "start"]
