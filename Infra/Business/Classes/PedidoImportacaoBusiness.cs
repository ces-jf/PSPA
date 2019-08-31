using Infra.Business.Interfaces;
using Infra.Class;
using Infra.Entidades;
using Infra.EntityExtension;
using Infra.Interfaces;
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

        public IEnumerable<PedidoImportacao> GetPedidosAguardando()
        {
            var pedidos = _systemContext.PedidoImportacao.Where(a => a.Estado == "A").Include(a => a.Usuario).Include(a => a.Arquivos).Include("Arquivos.Index").ToList();

            if (pedidos == null)
                return new List<PedidoImportacao>();
            else
                return pedidos;
        }

        public IEnumerable<PedidoImportacao> GetPedidoByUser(Usuario usuario)
        {
            var pedidos = _systemContext.PedidoImportacao.Where(a => a.Usuario.Id == usuario.Id).Include(a => a.Usuario).OrderByDescending(a => a.Estado).ToList();

            if (pedidos == null)
                return new List<PedidoImportacao>();

            if(pedidos != null)
                pedidos.ForEach(a => a.Estado = a.TraduzirEstado());

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

    }
}
