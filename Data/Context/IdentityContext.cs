using System;
using Infra.Entidades;
using Infra.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class IdentityContext : IdentityDbContext<Usuario>, IIdentityContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        public DbSet<ArquivoBase> ArquivoBase { get; set; }
        public DbSet<Index> Indice { get; set; }
        public DbSet<PedidoImportacao> PedidoImportacao { get; set; }
        public DbSet<LogPedidoImportacao> LogPedidoImportacao { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        public IIdentityContext Clone()
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>(b => {

                b.HasKey(a => a.Id);
                b.Property(a => a.Id).ValueGeneratedOnAdd();
                b.HasIndex(a => a.Email).IsUnique();
                b.HasIndex(a => a.UserName).IsUnique();

                b.ToTable("Usuario", "PSPABase");

            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Role", "PSPABase");
            });

            builder.Entity<IdentityUserRole<string>>(b => {
                b.Property(a => a.UserId).HasColumnName("UsuarioId");

                b.ToTable("UsuarioRole", "PSPABase");
            });

            builder.Entity<LogPedidoImportacao>(b => {
                b.ToTable("LogPedidoImportacao", "PSPABase");
            });

            builder.Entity<ArquivoBase>(b => {
                b.ToTable("ArquivoBase", "PSPABase");
            });

            builder.Entity<Index>(b => {
                b.ToTable("Indice", "PSPABase");
            });

            builder.Entity<PedidoImportacao>(b => {
                b.HasKey(a => a.ID);
                b.Property(a => a.ID).ValueGeneratedOnAdd();


                b.ToTable("PedidoImportacao", "PSPABase");
            });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
