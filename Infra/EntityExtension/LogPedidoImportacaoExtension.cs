using Infra.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.EntityExtension
{
    public static class LogPedidoImportacaoExtension
    {
        public static string TraduzirEstado(this LogPedidoImportacao logPedidoImportacao)
        {
            switch (logPedidoImportacao.IndicadorStatus)
            {
                case "A":
                    return "Aguardando";
                case "C":
                    return "Concluido";
                case "I":
                    return "Informativo";

                default:
                    return "Status não encontrado";
            }
        }
    }
}
