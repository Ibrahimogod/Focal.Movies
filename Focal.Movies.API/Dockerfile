FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5021
EXPOSE 7242

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Focal.Movies.API/Focal.Movies.API.csproj", "Focal.Movies.API/"]
RUN dotnet restore "Focal.Movies.API/Focal.Movies.API.csproj"
COPY . .
WORKDIR "/src/Focal.Movies.API"
RUN dotnet build "Focal.Movies.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Focal.Movies.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Focal.Movies.API.dll"]
