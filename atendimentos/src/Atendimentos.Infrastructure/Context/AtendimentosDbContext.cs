using Atendimentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Atendimentos.Infrastructure.Context
{
    public class AtendimentosDbContext : DbContext
    {
        public AtendimentosDbContext(DbContextOptions<AtendimentosDbContext> options)
            : base(options)
        {
        }

        // ==============================
        // 🧩 ENTIDADES DO DOMÍNIO
        // ==============================
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Garcom> Garcons { get; set; }
        public DbSet<Comanda> Comandas { get; set; }

        // ==============================
        // ⚙️ CONFIGURAÇÕES DO MODELO
        // ==============================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === MESA ===
            modelBuilder.Entity<Mesa>(entity =>
            {
                entity.ToTable("MESAS");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Numero)
                      .IsRequired();

                entity.HasIndex(e => e.Numero)
                      .IsUnique();

                entity.Property(e => e.Status)
                      .IsRequired();

                entity.Property(e => e.QrCode)
                      .HasMaxLength(256);

                entity.Property(e => e.Localizacao)
                      .HasMaxLength(80);

                entity.Property(e => e.CreatedAt)
                      .HasColumnType("TIMESTAMP")
                      .IsRequired();

                entity.Property(e => e.UpdatedAt)
                      .HasColumnType("TIMESTAMP")
                      .IsRequired();

                entity.Property(e => e.RowVersion)
                      .IsRowVersion()
                      .IsRequired();
            });

            // === GARÇOM ===
            modelBuilder.Entity<Garcom>(entity =>
            {
                entity.ToTable("GARCONS");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nome)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Matricula)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(e => e.Telefone)
                      .HasMaxLength(20);

                entity.Property(e => e.DataContratacao)
                      .HasColumnType("TIMESTAMP")
                      .IsRequired();

                entity.Property(e => e.Ativo)
                      .IsRequired();
            });

            // === COMANDA ===
            modelBuilder.Entity<Comanda>(entity =>
            {
                entity.ToTable("COMANDAS");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Status)
                      .IsRequired();

                entity.Property(e => e.ValorTotal)
                      .HasPrecision(10, 2);

                entity.Property(e => e.DataHoraAbertura)
                      .HasColumnType("TIMESTAMP")
                      .IsRequired();

                entity.Property(e => e.DataHoraFechamento)
                      .HasColumnType("TIMESTAMP");

                // Relacionamentos
                entity.HasOne<Mesa>()
                      .WithMany()
                      .HasForeignKey(e => e.MesaId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Garcom>()
                      .WithMany()
                      .HasForeignKey(e => e.GarcomId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
