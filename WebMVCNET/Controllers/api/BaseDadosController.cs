using Infra.Business.Classes.Identity;
using Infra.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebMVCNET.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseDadosController : ControllerBase
    {
        private IIndexBusiness IndexBusiness { get; set; }
        private IdentityBusiness IdentityBusiness { get; set; }

        public BaseDadosController(IIndexBusiness _indexBusiness, IdentityBusiness _identityBusiness)
        {
            this.IndexBusiness = _indexBusiness;
            this.IdentityBusiness = _identityBusiness;
        }

        [HttpGet]
        public IActionResult Get(long id)
        {
            try
            {
                var itens = this.IndexBusiness.Listar();

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

                var castedItens = itens.ToList().Select(a => new { a.Name });

                dynamic response = new
                {
                    Data = castedItens,
                    RecordsFiltered = itens.Count(),
                    RecordsTotal = itens.Count()
                };

                return Ok(response);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        [HttpGet]
        [Route("Colunas/{nomeBase}")]
        public IActionResult Colunas(string nomeBase)
        {
            try
            {
                var itens = this.IndexBusiness.Colunas(nomeIndex: nomeBase);

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

                var castedItens = itens.ToList().Select(a => new { a.Descricao });

                dynamic response = new
                {
                    Data = castedItens,
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

        private IList<TEntity> ProcessarDadosForm<TEntity>(IList<TEntity> listElements, IFormCollection requestFormData) where TEntity : class
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

        private PropertyInfo GetProperty<TEntity>(string name) where TEntity : class
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