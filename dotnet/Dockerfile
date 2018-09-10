FROM microsoft/dotnet:sdk as builder
RUN mkdir -p /usr/local/app
RUN apk add --no-cache parallel
RUN echo | parallel --will-cite
WORKDIR /usr/local/app
COPY ./Branch.sln .
COPY ./Apps ./Apps
COPY ./Clients ./Clients
COPY ./Packages ./Packages
COPY ./Tests ./Tests

# Restore packages
RUN dotnet restore

# Check we can build
RUN dotnet build --no-restore

# Do tests (workaround until I think of a better way)
RUN dotnet test ./Tests/Clients/JsonTests --no-build

# Publish projects
RUN ls ./Apps | parallel dotnet publish ./Apps/{} -o Build -c Release --no-build
RUN ls ./Apps | parallel mkdir -p ./Build
RUN ls ./Apps | parallel cp ./Apps/{}/Build ./Build/{}


FROM microsoft/dotnet:aspnetcore-runtime
ENV ASPNETCORE_ENVIRONMENT="production"
ENV ASPNETCORE_URLS="http://*:80"
EXPOSE 80
RUN mkdir =p /use/local/app
WORKDIR /use/local/app
ENTRYPOINT [ "sh", "./run.sh" ]
COPY ./run.sh .
COPY --from=builder /usr/local/app/build ./apps
