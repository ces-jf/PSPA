using System;
using System.Collections.Generic;
using Infra.Entidades;
using Infra.Enums;
using Infra.States;

namespace Infra.Business.Interfaces
{
    public interface IPedidoImportacaoBusiness: IDisposable
    {
        IEnumerable<PedidoImportacao> GetPendingOrders();
        IEnumerable<PedidoImportacao> GetPedidoByUser(Usuario usuario);
        IEnumerable<LogPedidoImportacao> GetLogPedidoImportacao(long id);
        IEnumerable<HeaderViewModel> GetHeaders(long id);
        void UpdateHeader(Header header);
        void SaveToImport(long id);
        void SetStatus(long id, OrderState orderState);
    }
}