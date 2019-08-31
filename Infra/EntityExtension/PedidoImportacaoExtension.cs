using Infra.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.EntityExtension
{
    public static class PedidoImportacaoExtension
    {
        public static string TraduzirEstado(this PedidoImportacao pedidoImportacao)
        {
            switch (pedidoImportacao.Estado)
            {
                case "A":
                    return "Aguardando";
                case "C":
                    return "Concluido";

                default:
                    return "Status não encontrado";
            }
        }
    }
}
