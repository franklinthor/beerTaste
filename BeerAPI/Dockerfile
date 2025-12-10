# ====== BASE RUNTIME IMAGE ======
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

# App will listen on 8080 inside the container
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose HTTP port
EXPOSE 8080

# Create data directory and non-root user
RUN mkdir -p /data \
    && groupadd -r appgroup \
    && useradd -r -g appgroup appuser \
    && chown -R appuser:appgroup /app /data

USER appuser


# ====== BUILD IMAGE ======
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["BeerAPI/BeerAPI.csproj", "BeerAPI/"]
RUN dotnet restore "BeerAPI/BeerAPI.csproj"

# Copy the rest of the source
COPY . .
WORKDIR "/src/BeerAPI"

# Build
RUN dotnet build "./BeerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build


# ====== PUBLISH IMAGE ======
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BeerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# ====== FINAL RUNTIME IMAGE ======
FROM base AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# user is already appuser from base
ENTRYPOINT ["dotnet", "BeerAPI.dll"]
