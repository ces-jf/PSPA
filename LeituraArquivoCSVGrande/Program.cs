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
            //int numeroTake = 10000000;
            int contador = 1;
            int skip = 1;
            Type tipo = typeof(Dictionary<string, string>);
            List<Dictionary<string,string>> objetosLista = new List<Dictionary<string, string>>();

            ToDo();
            Console.ReadLine();

            //var unitOfWork = new UnitOfWork();
            //var repository = unitOfWork.StartClient<Dictionary<string, string>>("bolsaFamiliaRepository","bolsafamilia");

            //Console.WriteLine("Procurando!");

            //var arquivo = File.ReadLines(path);
            //var cabecalho = arquivo.FirstOrDefault().Split(';');

            //do
            //{
            //    //linhas.AddRange(arquivo.Skip(skip).Take(contador));
                
            //    var linha = arquivo.Skip(skip).Take(contador).FirstOrDefault().Split(';');
            //    var objeto = new Dictionary<string, string>();

            //    for(var i = 0; i < linha.Count(); i++ )
            //    {
            //        objeto.Add(cabecalho[i], linha[i]);
            //    }

            //    repository.Insert(objeto);

            //    skip = contador;
            //    contador += 1;
            //}
            //while (contador < arquivo.Count());

            ////workspace
            ////int tamanho = arquivo.Count();


            //Console.WriteLine("Lida!");
        }

        static async void ToDo()
        {
            await Task.Run(async () => {
                string url = "http://www.portaltransparencia.gov.br/download-de-dados/bolsa-familia-pagamentos/201901";
                var arquivoBase = new ArquivoBaseBusiness();
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
