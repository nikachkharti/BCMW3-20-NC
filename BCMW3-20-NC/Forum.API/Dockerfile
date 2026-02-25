FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Forum.API/Forum.API.csproj", "Forum.API/"]
COPY ["Forum.Application/Forum.Application.csproj", "Forum.Application/"]
COPY ["Forum.Domain/Forum.Domain.csproj", "Forum.Domain/"]
COPY ["Forum.Infrastructure/Forum.Infrastructure.csproj", "Forum.Infrastructure/"]
RUN dotnet restore "./Forum.API/Forum.API.csproj"
COPY . .
WORKDIR "/src/Forum.API"
RUN dotnet build "./Forum.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Forum.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Forum.API.dll"]