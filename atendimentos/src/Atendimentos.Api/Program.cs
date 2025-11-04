using Atendimentos.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Atendimentos.Domain.Repositories;
using Atendimentos.Infrastructure.Repositories;
using Atendimentos.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Logging explícito no console
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Carrega env vars como fonte de config
builder.Configuration.AddEnvironmentVariables();

// Connection string com fallback + validação
var cs =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration["ConnectionStrings__DefaultConnection"]
    ?? throw new InvalidOperationException("A ConnectionString 'DefaultConnection' não foi encontrada.");

if (string.IsNullOrWhiteSpace(cs))
    throw new InvalidOperationException("A ConnectionString 'DefaultConnection' está vazia.");

builder.Services.AddDbContext<AtendimentosDbContext>(opt => opt.UseOracle(cs));

// DI – Repositórios & Serviços
builder.Services.AddScoped<IMesaRepository, MesaRepository>();
builder.Services.AddScoped<IMesaService, MesaService>();
builder.Services.AddScoped<IGarcomRepository, GarcomRepository>();
builder.Services.AddScoped<IGarcomService, GarcomService>();
builder.Services.AddScoped<IComandaRepository, ComandaRepository>();
builder.Services.AddScoped<IComandaService, ComandaService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// LOG: confirma que leu a connection (sem expor)
app.Logger.LogInformation("Connection string lida com sucesso. Tamanho: {Len} chars", cs.Length);

// Middleware global de captura p/ logar erro raiz no console
app.Use(async (ctx, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var root = ex;
        while (root.InnerException != null) root = root.InnerException;
        var logger = ctx.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("GlobalException");
        logger.LogError(ex, "Unhandled exception. Root: {RootMessage}", root.Message);
        throw;
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Bootstrap de schema: migra se tiver migrations; senão cria
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AtendimentosDbContext>();
    var pending = db.Database.GetPendingMigrations();
    if (pending.Any())
    {
        app.Logger.LogInformation("Aplicando {Count} migrations...", pending.Count());
        db.Database.Migrate();
    }
    else
    {
        app.Logger.LogInformation("Sem migrations pendentes. Garantindo schema existente (EnsureCreated)...");
        db.Database.EnsureCreated();
    }
}

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
