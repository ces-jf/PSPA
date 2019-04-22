using Infra.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;

namespace Infra.Interfaces
{
    public interface ISystemContext : IDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbQueryCache, IDbContextPoolable
    {
        DbSet<ArquivoBase> ArquivoBase { get; set; }
        DbSet<Cabecalho> Cabecalho { get; set; }
        DbSet<Index> Indice { get; set; }
        DbSet<LinhaPedidoImportacao> LinhaPedidoImportacao { get; set; }
        DbSet<LogPedidoImportacao> LogPedidoImportacao { get; set; }
        DbSet<PedidoImportacao> PedidoImportacao { get; set; }
        DbSet<Usuario> Usuario { get; set; }
        int SaveChanges(bool acceptAllChangesOnSuccess);
        //
        // Resumo:
        //     Saves all changes made in this context to the database.
        //
        // Devoluções:
        //     The number of state entries written to the database.
        //
        // Exceções:
        //   T:Microsoft.EntityFrameworkCore.DbUpdateException:
        //     An error is encountered while saving to the database.
        //
        //   T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException:
        //     A concurrency violation is encountered while saving to the database. A concurrency
        //     violation occurs when an unexpected number of rows are affected during save.
        //     This is usually because the data in the database has been modified since it was
        //     loaded into memory.
        //
        // Comentários:
        //     This method will automatically call Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges
        //     to discover any changes to entity instances before saving to the underlying database.
        //     This can be disabled via Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled.
        int SaveChanges();
    }
}