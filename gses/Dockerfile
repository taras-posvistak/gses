#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GSES2 BTC application/GSES2 BTC application.csproj", "GSES2 BTC application/"]
RUN dotnet restore "GSES2 BTC application/GSES2 BTC application.csproj"
COPY . .
WORKDIR "/src/GSES2 BTC application"
RUN dotnet build "GSES2 BTC application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GSES2 BTC application.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GSES2 BTC application.dll"]