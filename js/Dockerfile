FROM node:9.5-alpine as builder
RUN mkdir -p /usr/local/app
RUN apk add --no-cache parallel
RUN echo | parallel --will-cite
WORKDIR /usr/local/app
COPY ./package.json .
COPY ./yarn.lock .
COPY ./packages ./packages
COPY ./apps ./apps
RUN yarn install --frozen-lockfile --ignore-engines --production=false
RUN ls ./packages | parallel 'cd ./packages/{}; npm test'
RUN ls ./apps | grep ^service\- | parallel 'cd ./apps/{}; npm test'
RUN ls ./apps | grep ^service\- | parallel 'cd ./apps/{}; npm run build'
RUN yarn install --frozen-lockfile --ignore-engines --production=true
RUN mkdir ./build
RUN ls ./apps | grep ^service\- | parallel mkdir -p ./apps/{}/data
RUN ls ./apps | grep ^service\- | parallel mkdir -p ./build/{}
RUN ls ./apps | grep ^service\- | parallel cp ./apps/{}/package.json ./build/{}
RUN ls ./apps | grep ^service\- | parallel cp -R ./apps/{}/node_modules ./build/{}
RUN ls ./apps | grep ^service\- | parallel cp -R ./apps/{}/build ./build/{}
RUN ls ./apps | grep ^service\- | parallel cp -R ./apps/{}/data ./build/{}

FROM node:9.5-alpine
ENV NODE_ENV="production"
ENV PORT="80"
EXPOSE 80
RUN mkdir -p /usr/local/app
WORKDIR /usr/local/app
ENTRYPOINT ["sh", "./run.sh"]
COPY ./run.sh .
COPY --from=builder /usr/local/app/node_modules ./node_modules
COPY --from=builder /usr/local/app/packages ./packages
COPY --from=builder /usr/local/app/build ./apps
