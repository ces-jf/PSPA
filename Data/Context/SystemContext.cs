using Infra.Entidades;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class SystemContext : DbContext, ISystemContext
    {
        public SystemContext(DbContextOptions<SystemContext> options) : base(options) { }

        public DbSet<ArquivoBase> ArquivoBase { get; set; }
        public DbSet<Index> Indice { get; set; }
        public DbSet<Cabecalho> Cabecalho { get; set; }
        public DbSet<LinhaPedidoImportacao> LinhaPedidoImportacao { get; set; }
        public DbSet<PedidoImportacao> PedidoImportacao { get; set; }
        public DbSet<LogPedidoImportacao> LogPedidoImportacao { get; set; }
    }
}
