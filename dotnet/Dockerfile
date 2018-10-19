FROM microsoft/dotnet:2.1.402-sdk-alpine as builder
RUN mkdir -p /usr/local/app
RUN apk add --no-cache parallel
RUN echo | parallel --will-cite
WORKDIR /usr/local/app
COPY ./Branch.sln .
COPY ./Tests ./Tests
COPY ./Clients ./Clients
COPY ./LambdaFunctions ./LambdaFunctions
COPY ./Packages ./Packages
COPY ./Apps ./Apps
RUN dotnet restore
RUN dotnet test ./Tests/Clients/JsonTests/JsonTests.csproj -c Release --no-restore
RUN dotnet build --no-restore -c Release
RUN ls ./Apps | parallel dotnet publish ./Apps/{} -o Build -c Release --no-build
RUN ls ./Apps | parallel mkdir -p ./Build/{}
RUN ls ./Apps | parallel cp -R ./Apps/{}/Build/. ./Build/{}

FROM microsoft/dotnet:2.1.4-aspnetcore-runtime-alpine
ENV ASPNETCORE_ENVIRONMENT="production"
ENV ASPNETCORE_URLS="http://*:80"
EXPOSE 80
RUN mkdir -p /usr/local/app
WORKDIR /usr/local/app
ENTRYPOINT [ "sh", "./run.sh" ]
COPY ./run.sh .
COPY --from=builder /usr/local/app/Build ./Apps
