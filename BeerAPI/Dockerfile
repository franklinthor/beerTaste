FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

# App will listen on 8080 inside the container
ENV ASPNETCORE_URLS=http://+:8080

# Expose ports (8080 for HTTP; keep 8081 if you actually use it)
EXPOSE 8080
EXPOSE 8081

# Create a non-root user using groupadd/useradd (works on Debian/Mariner base)
RUN groupadd -r appgroup && useradd -r -g appgroup appuser

USER appuser


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["BeerAPI/BeerAPI.csproj", "BeerAPI/"]
RUN dotnet restore "BeerAPI/BeerAPI.csproj"

COPY . .
WORKDIR "/src/BeerAPI"
RUN dotnet build "./BeerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BeerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# user is already appuser from base
ENTRYPOINT ["dotnet", "BeerAPI.dll"]
