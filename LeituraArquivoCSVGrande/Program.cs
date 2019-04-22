using Data.Class;
using Infra.Business.Classes;
using Infra.Entidades;
using Nest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeituraArquivoCSVGrande
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine("C:\\Users\\T-Gamer\\Documents\\201901_BolsaFamilia_Pagamentos", "201901_BolsaFamilia_Pagamentos.csv");
            Type tipo = typeof(Dictionary<string, string>);
            List<Dictionary<string,string>> objetosLista = new List<Dictionary<string, string>>();

            ToDo();
            Console.ReadLine();
        }

        static async void ToDo()
        {
            await Task.Run(async () => {
                string url = "http://www.portaltransparencia.gov.br/download-de-dados/bolsa-familia-pagamentos/201901";
                var arquivoBase = new ArquivoBaseBusiness(new UnitOfWork());
                try
                {
                    //await arquivoBase.DownloadOnDiskAsync(url, "bolsaFamilia.zip");
                    await arquivoBase.CadastrarBaseAsync(url, "bolsateste");
                }
                catch(Exception erro)
                {
                    Console.WriteLine($"Exceção a frente: {erro.Message}");
                }
            });
        }
    }
}
