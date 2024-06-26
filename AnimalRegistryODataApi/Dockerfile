# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
    
# Copy csproj and restore as distinct layers
COPY ../*.sln ./
COPY ../AnimalRegistryODataApi/*.csproj ./AnimalRegistryODataApi/
COPY ../Domain/*.csproj ./Domain/
COPY ../DataAccess/*.csproj ./DataAccess/
COPY ../Application/*.csproj ./Application/
COPY ../Tests/Api.Tests/*.csproj ./Tests/Api.Tests/
COPY ../Tests/Application.Tests/*.csproj ./Tests/Application.Tests/
RUN dotnet restore
    
# Copy everything else and build
COPY ../AnimalRegistryODataApi/ ./AnimalRegistryODataApi/
COPY ../Domain/ ./Domain/
COPY ../DataAccess/ ./DataAccess/
COPY ../Application/ ./Application/
COPY ../Tests/Api.Tests/. /Tests/Api.Tests/
COPY ../Tests/Application.Tests/. /Tests/Application.Tests/
WORKDIR /app/AnimalRegistryODataApi
RUN dotnet publish -c Release -o /app/out
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AnimalRegistryODataApi.dll"]