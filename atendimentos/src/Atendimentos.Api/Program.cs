using Microsoft.EntityFrameworkCore;
using Atendimentos.Infrastructure.Context; // DbContext

var builder = WebApplication.CreateBuilder(args);

// ==============================
// 🔧 Banco Oracle (connection em appsettings.json -> "DefaultConnection")
// ==============================
builder.Services.AddDbContext<AtendimentosDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========================================
// 🧩 DI: Repositories + Services (nomes totalmente qualificados)
// ========================================

// MESA
builder.Services.AddScoped<
    Atendimentos.Domain.Repositories.IMesaRepository,
    Atendimentos.Infrastructure.Repositories.MesaRepository>();

builder.Services.AddScoped<
    Atendimentos.Application.Services.IMesaService,
    Atendimentos.Application.Services.MesaService>();

// GARÇOM
builder.Services.AddScoped<
    Atendimentos.Domain.Repositories.IGarcomRepository,
    Atendimentos.Infrastructure.Repositories.GarcomRepository>();

builder.Services.AddScoped<
    Atendimentos.Application.Services.IGarcomService,
    Atendimentos.Application.Services.GarcomService>();

// COMANDA
builder.Services.AddScoped<
    Atendimentos.Domain.Repositories.IComandaRepository,
    Atendimentos.Infrastructure.Repositories.ComandaRepository>();

builder.Services.AddScoped<
    Atendimentos.Application.Services.IComandaService,
    Atendimentos.Application.Services.ComandaService>();

// ========================================
// 🧱 ASP.NET
// ========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ========================================
// 🚀 Pipeline
// ========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
