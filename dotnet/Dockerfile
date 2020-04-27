FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine

COPY bin/Release/netcoreapp3.1 /
COPY entrypoint.sh /entrypoint.sh

ENTRYPOINT ["sh", "./entrypoint.sh"]
