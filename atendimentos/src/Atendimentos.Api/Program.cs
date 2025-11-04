using Atendimentos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Atendimentos.Domain.Repositories;
using Atendimentos.Infrastructure.Repositories;
using Atendimentos.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================
// Força leitura de variáveis de ambiente
// =============================
builder.Configuration.AddEnvironmentVariables();

// =============================
// Recupera Connection String (com fallback)
// =============================
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    builder.Configuration["ConnectionStrings__DefaultConnection"] ??
    throw new InvalidOperationException(" A ConnectionString 'DefaultConnection' não foi encontrada nas configurações.");

// =============================
// Configuração do banco de dados Oracle
// =============================
builder.Services.AddDbContext<AtendimentosDbContext>(options =>
    options.UseOracle(connectionString));

// =============================
// Registro de Repositórios e Serviços
// =============================
builder.Services.AddScoped<IMesaRepository, MesaRepository>();
builder.Services.AddScoped<IMesaService, MesaService>();

builder.Services.AddScoped<IGarcomRepository, GarcomRepository>();
builder.Services.AddScoped<IGarcomService, GarcomService>();

builder.Services.AddScoped<IComandaRepository, ComandaRepository>();
builder.Services.AddScoped<IComandaService, ComandaService>();

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
// Ambiente de desenvolvimento
// =============================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// =============================
// Migrations automáticas
// =============================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AtendimentosDbContext>();
    db.Database.EnsureCreated();
}

// =============================
// Swagger e Middlewares
// =============================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Atendimentos API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
