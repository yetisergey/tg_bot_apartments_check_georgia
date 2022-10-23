#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Jobs/LoadJob/LoadJob.csproj", "src/Jobs/LoadJob/"]
COPY ["src/Extensions/Extensions.csproj", "src/Extensions/"]
COPY ["src/Configuration/Configuration.csproj", "src/Configuration/"]
COPY ["src/Interfaces/Interfaces.csproj", "src/Interfaces/"]
COPY ["src/RabbitMQ/RabbitMQ.Contracts/RabbitMQ.Contracts.csproj", "src/RabbitMQ/RabbitMQ.Contracts/"]
COPY ["src/RabbitMQ/RabbitMQ/RabbitMQ.csproj", "src/RabbitMQ/RabbitMQ/"]
COPY ["src/Services/Services.csproj", "src/Services/"]
COPY ["src/Storage/Repository/Repository.csproj", "src/Storage/Repository/"]
COPY ["src/Storage/Storage.Flats/Storage.Flats.csproj", "src/Storage/Storage.Flats/"]
COPY ["src/Storage/Storage.Users/Storage.Bot.csproj", "src/Storage/Storage.Users/"]
RUN dotnet restore "src/Jobs/LoadJob/LoadJob.csproj"
COPY . .
WORKDIR "/src/src/Jobs/LoadJob"
RUN dotnet build "LoadJob.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoadJob.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoadJob.dll"]