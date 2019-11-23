using Infra.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Infra.Interfaces
{
    public interface IIdentityContext
    {
        DbSet<ArquivoBase> ArquivoBase { get; set; }
        DbSet<Index> Indice { get; set; }
        DbSet<LogPedidoImportacao> LogPedidoImportacao { get; set; }
        DbSet<PedidoImportacao> PedidoImportacao { get; set; }
        DbSet<Usuario> Usuario { get; set; }

        DatabaseFacade Database { get; }
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        void Dispose();
    }
}