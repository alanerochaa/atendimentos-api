using Atendimentos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Atendimentos.Domain.Repositories;
using Atendimentos.Infrastructure.Repositories;
using Atendimentos.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================
// Configuração do banco de dados Oracle
// =============================
builder.Services.AddDbContext<AtendimentosDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("Default")));

// =============================
// Registro de Repositórios e Serviços
// =============================

// MESA
builder.Services.AddScoped<IMesaRepository, MesaRepository>();
builder.Services.AddScoped<IMesaService, MesaService>();

// GARÇOM
builder.Services.AddScoped<IGarcomRepository, GarcomRepository>();
builder.Services.AddScoped<IGarcomService, GarcomService>();

// COMANDA
builder.Services.AddScoped<IComandaRepository, ComandaRepository>();
builder.Services.AddScoped<IComandaService, ComandaService>();

// CLIENTE
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// =============================
// Configurações básicas da API
// =============================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =============================
// Configuração do Swagger (habilitado em qualquer ambiente)
// =============================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Atendimentos API v1");
    c.RoutePrefix = "swagger";
});

// =============================
// Middlewares padrão
// =============================
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// =============================
// Execução da aplicação
// =============================
app.Run();
