# ===============================
# 📦 Etapa 1: Build da aplicação
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todos os arquivos do projeto
COPY . .

# Restaura dependências
RUN dotnet restore "Atendimentos.Api/Atendimentos.Api.csproj"

# Compila e publica em modo Release
RUN dotnet publish "Atendimentos.Api/Atendimentos.Api.csproj" -c Release -o /app/publish


# ===============================
# 🚀 Etapa 2: Runtime (execução)
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia os arquivos da etapa de build
COPY --from=build /app/publish .

# Executa a API
ENTRYPOINT ["dotnet", "Atendimentos.Api.dll"]
