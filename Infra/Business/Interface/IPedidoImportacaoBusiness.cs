using System.Collections.Generic;
using Infra.Entidades;

namespace Infra.Business.Interfaces
{
    public interface IPedidoImportacaoBusiness
    {
        IEnumerable<PedidoImportacao> GetPedidosAguardando();
        IEnumerable<PedidoImportacao> GetPedidoByUser(Usuario usuario);
        IEnumerable<LogPedidoImportacao> GetLogPedidoImportacao(long id);
    }
}