FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .                      # copia TUDO do context (sln + src/*)
RUN dotnet restore "Atendimentos.sln"
RUN dotnet publish "src/Atendimentos.Api/Atendimentos.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8081
COPY --from=build /app/publish .
EXPOSE 8081
ENTRYPOINT ["dotnet","Atendimentos.Api.dll"]
