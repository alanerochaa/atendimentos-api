# ==============================
# 🧱 Etapa 1: Build
# ==============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo para dentro da imagem
COPY . .

# Restaura dependências do projeto principal
RUN dotnet restore "src/Atendimentos.Api/Atendimentos.Api.csproj"

# Compila e publica em modo Release
RUN dotnet publish "src/Atendimentos.Api/Atendimentos.Api.csproj" -c Release -o /app/publish

# ==============================
# 🚀 Etapa 2: Execução
# ==============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8081
ENTRYPOINT ["dotnet", "Atendimentos.Api.dll"]

