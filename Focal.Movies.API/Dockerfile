FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20 AS build
WORKDIR /src

COPY ["Focal.Movies.API/Focal.Movies.API.csproj", "Focal.Movies.API/"]
RUN dotnet restore "Focal.Movies.API/Focal.Movies.API.csproj"

COPY . .
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Focal.Movies.API/Focal.Movies.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.20 AS final
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p /app/Csvs \
    && chown -R appuser:appgroup /app/Csvs \
    && chmod -R 755 /app/Csvs

USER appuser
EXPOSE 8080
ENTRYPOINT ["dotnet", "Focal.Movies.API.dll"]