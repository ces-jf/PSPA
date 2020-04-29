using Infra.Business.Interfaces;
using Infra.Class;
using Infra.Entidades;
using Infra.EntityExtension;
using Infra.Enums;
using Infra.Interfaces;
using Infra.States;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Infra.Business.Classes
{
    public class PedidoImportacaoBusiness : BusinessBase, IPedidoImportacaoBusiness
    {
        public PedidoImportacaoBusiness(IUnitOfWork unitOfWork, IIdentityContext identityContext) : base(unitOfWork, identityContext) { }

        public IEnumerable<PedidoImportacao> GetPendingOrders()
        {
            var pedidos = _systemContext.PedidoImportacao.AsNoTracking().Where(a => a.OrderState == OrderState.WaitingToImport).Include(a => a.Usuario).Include(a => a.Arquivos).Include("Arquivos.Index").ToList();

            if (pedidos == null)
                return new List<PedidoImportacao>();
            else
                return pedidos;
        }

        public IEnumerable<PedidoImportacao> GetPedidoByUser(Usuario usuario)
        {
            var pedidos = _systemContext.PedidoImportacao.Where(a => a.Usuario.Id == usuario.Id).Include(a => a.Usuario).OrderByDescending(a => a.OrderState).ToList();

            if (pedidos == null)
                return new List<PedidoImportacao>();

            return pedidos;
        }

        public IEnumerable<LogPedidoImportacao> GetLogPedidoImportacao(long id)
        {
            var pedidos = _systemContext.PedidoImportacao.Where(a => a.ID == id).Include(a => a.LogPedidoImportacao).OrderByDescending(a => a.DataTermino).FirstOrDefault();

            if (pedidos == null)
                pedidos = new PedidoImportacao { LogPedidoImportacao = new List<LogPedidoImportacao>() };

            var logs = pedidos.LogPedidoImportacao;

            if (logs != null)
                logs.ToList().ForEach(a => a.IndicadorStatus = a.TraduzirEstado());

            return logs;
        }

        public IEnumerable<HeaderViewModel> GetHeaders(long id)
        {
            var headers = _systemContext.Header.Include(a => a.ArquivoBase).AsNoTracking().Where(a => a.ArquivoBase.PedidoImportacao.ID == id).Select(a => new HeaderViewModel
            {
                ArquivoBase = a.ArquivoBase,
                HeaderType = a.HeaderType,
                ID = a.ID,
                Name = a.Name                
            }).ToList();
            return headers;
        }

        public void UpdateHeader(Header header)
        {
            try
            {
                _systemContext.Header.Update(header);
                _systemContext.SaveChanges();
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public void SaveToImport(long id)
        {
            var order = _systemContext.PedidoImportacao.FirstOrDefault(a => a.ID == id);
            order.OrderState = OrderState.WaitingToImport;
            _systemContext.SaveChanges();
        }

        public void SetStatus(long id, OrderState orderState)
        {
            var order = _systemContext.PedidoImportacao.FirstOrDefault(a => a.ID == id);
            order.OrderState = orderState;
            _systemContext.SaveChanges();
        }
    }
}
