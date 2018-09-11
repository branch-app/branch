FROM microsoft/dotnet:2.1-sdk-alpine as builder
RUN mkdir -p /usr/local/app
RUN apk add --no-cache parallel
RUN echo | parallel --will-cite
WORKDIR /usr/local/app
COPY ./Branch.sln .
COPY ./Apps ./Apps
COPY ./Clients ./Clients
COPY ./Packages ./Packages
COPY ./Tests ./Tests
RUN dotnet restore
RUN dotnet build --no-restore -c Release -r alpine-x64
RUN dotnet test ./Tests/Clients/JsonTests --no-build -c Release
RUN ls ./Apps | parallel dotnet publish -o Build -c Release -r alpine-x64 --no-build ./Apps/{}
RUN ls ./Apps | parallel mkdir -p ./Build/{}
RUN ls ./Apps | parallel cp -R ./Apps/{}/Build ./Build/{}

FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine
ENV ASPNETCORE_ENVIRONMENT="production"
ENV ASPNETCORE_URLS="http://*:80"
EXPOSE 80
RUN mkdir -p /usr/local/app
WORKDIR /usr/local/app
ENTRYPOINT [ "sh", "./run.sh" ]
COPY ./run.sh .
COPY --from=builder /usr/local/app/Build ./Apps
