FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS base
WORKDIR /app
COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet ErrorCenter.dll