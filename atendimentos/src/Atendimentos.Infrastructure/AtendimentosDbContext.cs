using Atendimentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Atendimentos.Infrastructure;


public class AtendimentosDbContext : DbContext
{
public DbSet<Mesa> Mesas => Set<Mesa>();


public AtendimentosDbContext(DbContextOptions<AtendimentosDbContext> options)
: base(options) { }


protected override void OnModelCreating(ModelBuilder modelBuilder)
{
modelBuilder.ApplyConfigurationsFromAssembly(typeof(AtendimentosDbContext).Assembly);
base.OnModelCreating(modelBuilder);
}
}