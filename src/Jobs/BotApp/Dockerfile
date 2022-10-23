#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Jobs/BotApp/BotApp.csproj", "src/Jobs/BotApp/"]
COPY ["src/Configuration/Configuration.csproj", "src/Configuration/"]
COPY ["src/Extensions/Extensions.csproj", "src/Extensions/"]
COPY ["src/Interfaces/Interfaces.csproj", "src/Interfaces/"]
COPY ["src/Services/Services.csproj", "src/Services/"]
COPY ["src/Storage/Repository/Repository.csproj", "src/Storage/Repository/"]
COPY ["src/Storage/Storage.Flats/Storage.Flats.csproj", "src/Storage/Storage.Flats/"]
COPY ["src/Storage/Storage.Users/Storage.Bot.csproj", "src/Storage/Storage.Users/"]
RUN dotnet restore "src/Jobs/BotApp/BotApp.csproj"
COPY . .
WORKDIR "/src/src/Jobs/BotApp"
RUN dotnet build "BotApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BotApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BotApp.dll"]