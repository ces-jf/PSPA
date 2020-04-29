using Infra.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using Index = Infra.Entidades.Index;

namespace Infra.Interfaces
{
    public interface IIdentityContext: IDisposable
    {
        DbSet<ArquivoBase> ArquivoBase { get; set; }
        DbSet<Index> Indice { get; set; }
        DbSet<LogPedidoImportacao> LogPedidoImportacao { get; set; }
        DbSet<PedidoImportacao> PedidoImportacao { get; set; }
        DbSet<Usuario> Usuario { get; set; }
        DbSet<Header> Header { get; set; }

        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
    }
}