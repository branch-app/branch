FROM node:10.10.0-alpine as builder
RUN mkdir -p /usr/local/app
RUN apk add --no-cache parallel
RUN echo | parallel --will-cite

WORKDIR /usr/local/app

COPY ./packages ./packages
WORKDIR /usr/local/app/packages
RUN ls | parallel 'cd {}; npm install --quiet'
RUN ls | parallel 'cd {}; npm test'
RUN ls | parallel 'cd {}; npm install --only=prod --quiet'

COPY ./svcs ./svcs
WORKDIR /usr/local/app/svcs
RUN yarn install --frozen-lockfile --production=false
RUN ls -d */ | cut -f1 -d'/' | grep -v '^node_modules$' | parallel 'cd {}; npm test'
RUN yarn install --frozen-lockfile --production=true

FROM node:10.10.0-alpine
ENV NODE_ENV="production"
ENV PORT="80"
EXPOSE 80
RUN mkdir -p /usr/local/app/svcs
WORKDIR /usr/local/app/svcs
ENTRYPOINT ["sh", "./run.sh"]
COPY ./svcs/run.sh .
COPY --from=builder /usr/local/app/packages ./packages
COPY --from=builder /usr/local/app/svcs ./svcs
