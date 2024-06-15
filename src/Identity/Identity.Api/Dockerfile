#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Identity/Identity.Api/Identity.Api.csproj", "src/Identity/Identity.Api/"]
COPY ["src/Identity/Identity.Domain/", "src/Identity/Identity.Domain/"]
COPY ["src/Identity/Identity.Infrastructure/", "src/Identity/Identity.Infrastructure/"]
RUN dotnet restore "./src/Identity/Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/Identity/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]