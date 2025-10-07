using Atendimentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Atendimentos.Infrastructure.Configurations;


public class MesaConfiguration : IEntityTypeConfiguration<Mesa>
{
public void Configure(EntityTypeBuilder<Mesa> b)
{
b.ToTable("mesas");
b.HasKey(x => x.Id);
b.Property(x => x.Numero).IsRequired();
b.HasIndex(x => x.Numero).IsUnique();
b.Property(x => x.Status).IsRequired();
b.Property(x => x.Capacidade);
b.Property(x => x.Localizacao).HasMaxLength(80);
b.Property(x => x.QrCode).HasMaxLength(256);
b.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
b.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
b.Property<byte[]>("RowVersion").IsRowVersion();
}
}