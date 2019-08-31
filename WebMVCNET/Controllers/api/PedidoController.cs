using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Infra.Business.Classes.Identity;
using Infra.Business.Interfaces;
using Infra.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace WebMVCNET.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private IPedidoImportacaoBusiness PedidoImportacaoBusiness { get; set; }
        private IdentityBusiness IdentityBusiness { get; set; }

        public PedidoController(IPedidoImportacaoBusiness _pedidoImportacaoBusiness, IdentityBusiness _identityBusiness)
        {
            this.PedidoImportacaoBusiness = _pedidoImportacaoBusiness;
            this.IdentityBusiness = _identityBusiness;
        }

        [HttpPost]
        [Route("log/{id}")]
        public IActionResult Log(long id)
        {
            try
            {
                var itens = this.PedidoImportacaoBusiness.GetLogPedidoImportacao(id);
                var requestFormData = Request.Form;

                if (itens.Count() <= 0)
                {
                    dynamic responseItem = new
                    {
                        Data = itens,
                        Draw = "1",
                        RecordsFiltered = itens.Count(),
                        RecordsTotal = itens.Count()
                    };

                    return Ok(responseItem);
                }

                var castedItens = this.ProcessarDadosForm<LogPedidoImportacao>(itens.ToList(), requestFormData).Select(a => new { a.ID, a.Descricao, a.IndicadorStatus, a.DataCriacao});

                dynamic response = new
                {
                    Data = castedItens,
                    Draw = requestFormData["draw"],
                    RecordsFiltered = itens.Count(),
                    RecordsTotal = itens.Count()
                };

                return Ok(response);
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                var usuario = this.IdentityBusiness.GetUsuarioAsync(User).GetAwaiter().GetResult();
                var itens = this.PedidoImportacaoBusiness.GetPedidoByUser(usuario);
                var requestFormData = Request.Form;
                
                if(itens.Count() <= 0)
                {
                    dynamic responseItem = new
                    {
                        Data = itens,
                        Draw = "1",
                        RecordsFiltered = itens.Count(),
                        RecordsTotal = itens.Count()
                    };

                    return Ok(responseItem);
                }
                
                var castedItens = this.ProcessarDadosForm<PedidoImportacao>(itens.ToList(), requestFormData).Select(a => new { a.ID, a.PastaTemp, a.Estado, a.DataTermino });

                dynamic response = new
                {
                    Data = castedItens,
                    Draw = requestFormData["draw"],
                    RecordsFiltered = itens.Count(),
                    RecordsTotal = itens.Count()
                };

                return Ok(response);
            }
            catch(Exception erro)
            {
                throw erro;
            }
        }

        private IList<TEntity> ProcessarDadosForm<TEntity>(IList<TEntity> listElements, IFormCollection requestFormData) where TEntity:class
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            StringValues tempOrder = new[] { "" };

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();

                tempOrder = new[] { "" };

                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = this.GetProperty<TEntity>(columName);
                        if (sortDirection == "asc")
                        {
                            return listElements.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }
                        else
                            return listElements.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                    }
                    else
                        return listElements;
                }
            }

            return null;
        }

        private PropertyInfo GetProperty<TEntity>(string name) where TEntity: class
        {
            var properties = typeof(TEntity).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLower().Equals(name.ToLower()))
                {
                    prop = item;
                    break;
                }
            }

            return prop;
        }
    }
}