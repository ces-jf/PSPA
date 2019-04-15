using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SystemHelper;

namespace Infra.Business.Classes
{
    public class ArquivoBaseBusiness : BusinessBase
    {
        public ArquivoBaseBusiness():base(new Data.Class.UnitOfWork()) { }

        public async Task CadastrarBaseAsync(string _url, string _index)
       {
            //await this.DownloadOnDiskAsync(_url, _index);

            string path = Path.Combine(Configuration.DefaultTempBaseFiles, _index);

            var files = Directory.GetFiles(path);
            foreach(var _file in files)
            {
                var file = File.ReadLines(_file);

                this.InserirArquivo(file, _index);
            }
        }

        private void InserirArquivo(IEnumerable<string> file, string _nameBase)
        {
            var repository = this._unitOfWork.StartClient<Dictionary<string, string>>($"{_nameBase}Repository", _nameBase);

            var maxMemoryUsing = Configuration.MaxMemoryUsing;
            long memoryUsing = 0;
            bool isMemoryFull = false;

            var cabecalho = file.FirstOrDefault().Split(';');
            var loadObjects = new List<Dictionary<string, string>>();
            int valorTotal = 0;
            int fileSize = file.Count();

            int contador = 200000;
            int skip = 1;

            var ramErrors = new Dictionary<int[], string>(); //skip, contador
            

            do
            {
                try
                {
                    var listObject = new List<Dictionary<string, string>>();

                    loadObjects.AddRange(file.Skip(skip).Take(contador).Select(a => a.Split(';').Zip(cabecalho, (value, key) => new { key, value }).ToDictionary(z => z.key, b => b.value)));

                    valorTotal += loadObjects.Count;

                    //using (var process = Process.GetCurrentProcess())
                    //{
                    //    memoryUsing = process.PrivateMemorySize64;
                    //    isMemoryFull = (memoryUsing >= maxMemoryUsing);
                    //}

                    ////if (isMemoryFull)
                    this.WorkWithObjMemory(ref loadObjects, ref repository, ref isMemoryFull, cabecalho);

                    skip += contador;
                }
                catch (Exception erro)
                {
                    ramErrors.Add(new int[] { skip, contador }, erro.Message);
                }
            }
            while (valorTotal < fileSize - 1);
            // });
        }

        private void WorkWithObjMemory(ref List<Dictionary<string,string>> linha, ref IRepository<Dictionary<string, string>> repository, ref bool isMemoryFull, string[] cabecalho)
        {
            try
            {
                repository.BulkInsert(linha);
                linha.RemoveRange(0, linha.Count);
            }
            catch(Exception erro)
            {
                throw erro;
            }
            finally
            {
                isMemoryFull = false;
            }
        }

        public async Task DownloadOnDiskAsync(string _url, string _nameBase)
        {
            var destineBase = Path.Combine(Configuration.DefaultTempBaseFiles, _nameBase);
            var destineExtract = Path.Combine(Configuration.DefaultTempBaseFiles, _nameBase.Substring(0, _nameBase.Length - 4));

            Directory.CreateDirectory(destineExtract);

            var uri = new Uri(_url);

            var webClient = new WebClient();
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            await webClient.DownloadFileTaskAsync(uri, destineBase);

            await Task.Run(() => {

                ZipFile.ExtractToDirectory(destineBase, destineExtract);

            });
        }
    
        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.Clear();
            Console.WriteLine($"Download Concluido.");
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Clear();
            Console.WriteLine($"Progresso do Download: {e.ProgressPercentage}");
            Console.WriteLine($"Progresso em Byes: {e.BytesReceived} / {e.TotalBytesToReceive}");
        }
    }
}
