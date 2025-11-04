using Atendimentos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Atendimentos.Domain.Repositories;
using Atendimentos.Infrastructure.Repositories;
using Atendimentos.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================
// Leitura de variáveis de ambiente
// =============================
builder.Configuration.AddEnvironmentVariables();

// =============================
// Connection String (com fallback + validação)
// =============================
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    builder.Configuration["ConnectionStrings__DefaultConnection"] ??
    throw new InvalidOperationException("A ConnectionString 'DefaultConnection' não foi encontrada nas configurações.");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("A ConnectionString 'DefaultConnection' está vazia.");

// =============================
// EF Core + Oracle
// =============================
builder.Services.AddDbContext<AtendimentosDbContext>(options =>
    options.UseOracle(connectionString));

// =============================
// DI – Repositórios & Serviços
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
// API Base
// =============================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// =============================
// Bootstrap do schema
// =============================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AtendimentosDbContext>();
    var pending = db.Database.GetPendingMigrations();
    if (pending.Any()) db.Database.Migrate(); else db.Database.EnsureCreated();
}

// =============================
// Swagger & Middlewares
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
