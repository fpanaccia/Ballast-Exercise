#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Aircraft/Aircraft.Api/Aircraft.Api.csproj", "src/Aircraft/Aircraft.Api/"]
COPY ["src/Aircraft/Aircraft.Domain/", "src/Aircraft/Aircraft.Domain/"]
COPY ["src/Aircraft/Aircraft.Infrastructure/", "src/Aircraft/Aircraft.Infrastructure/"]
RUN dotnet restore "./src/Aircraft/Aircraft.Api/Aircraft.Api.csproj"
COPY . .
WORKDIR "/src/src/Aircraft/Aircraft.Api"
RUN dotnet build "Aircraft.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Aircraft.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aircraft.Api.dll"]