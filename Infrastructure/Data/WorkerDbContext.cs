using Core.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Infrastructure.Data
{
    public partial class WorkerDbContext : DbContext
    {
        private readonly string connectionString;

        public WorkerDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public WorkerDbContext(DbContextOptions<WorkerDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Producto> Productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("code")
                    .IsFixedLength(true);

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("currency")
                    .IsFixedLength(true);

                entity.Property(e => e.Ii)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("ii");

                entity.Property(e => e.Iva)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("iva");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("sku")
                    .IsFixedLength(true);

                entity.Property(e => e.Stock)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("stock");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
