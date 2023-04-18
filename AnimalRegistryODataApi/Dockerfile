# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
    
# Copy csproj and restore as distinct layers
COPY ../*.sln ./
COPY ../AnimalRegistryODataApi/*.csproj ./AnimalRegistryODataApi/
COPY ../Core/*.csproj ./Core/
COPY ../DataAccess/*.csproj ./DataAccess/
COPY ../Application/*.csproj ./Application/
RUN dotnet restore
    
# Copy everything else and build
COPY ../AnimalRegistryODataApi/ ./AnimalRegistryODataApi/
COPY ../Core/ ./Core/
COPY ../DataAccess/ ./DataAccess/
COPY ../Application/ ./Application/
WORKDIR /app/AnimalRegistryODataApi
RUN dotnet publish -c Release -o /app/out
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AnimalRegistryODataApi.dll"]