# ===== ETAP 1: BUILD (ciężki obraz z SDK) =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Najpierw kopiujemy tylko .csproj i robimy restore (cache!)
COPY *.csproj ./
RUN dotnet restore

# Potem reszta kodu i publikacja
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# ===== ETAP 2: RUNTIME (lekki obraz, tylko do uruchomienia) =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "WarehouseApi.dll"]