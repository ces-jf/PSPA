using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArquivoController : ControllerBase
    {
        private IArquivoBaseBusiness ArquivoBaseBusiness { get; set; }

        public ArquivoController(IArquivoBaseBusiness _arquivoBaseBusiness)
        {
            this.ArquivoBaseBusiness = _arquivoBaseBusiness;
        }

        // GET: api/Arquivo
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Arquivo/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Arquivo
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] string url, string indexName)
        {
            try
            {
                await this.ArquivoBaseBusiness.CadastrarBaseAsync(url, indexName);
                return new CreatedResult("",new { message = $"Base de dados {indexName} cadastrada com sucesso!" });
            }
            catch(Exception erro)
            {
                return new BadRequestObjectResult(new { message = $"Erro gerado no servidor: {erro.Message}" });
            }
        }

        // PUT: api/Arquivo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
