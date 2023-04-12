FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore

RUN dotnet build --no-restore

RUN dotnet test --no-build