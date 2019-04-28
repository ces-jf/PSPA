using Infra.Business.Interfaces;
using Infra.Class;
using Infra.Entidades;
using Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using SystemHelper;

namespace Infra.Business.Classes
{
    public class ArquivoBaseBusiness : BusinessBase, IArquivoBaseBusiness
    {
        public ArquivoBaseBusiness(IUnitOfWork _unitOfWork, IPrincipal User) : base(_unitOfWork, User) { }
        public ArquivoBaseBusiness(IUnitOfWork _unitOfWork, ISystemContext _systemContext, IPrincipal User) : base(_unitOfWork, _systemContext, User) { }

        public async Task CadastrarBaseAsync(string _url, string _index)
        {
            var newPedido = new PedidoImportacao()
            {
                Usuario = User as Usuario
            };

            var newPedidoEntity = await _systemContext.PedidoImportacao.AddAsync(newPedido);

            var logFile = new LogPedidoImportacao()
            {
                Descricao = "Baixando Arquivos do Disco...",
                IndicadorStatus = "I",
                PedidoImportacao = newPedidoEntity.Entity
            };

            var logFileEntity = await _systemContext.LogPedidoImportacao.AddAsync(logFile);

            this.DownloadOnDisk(_url, _index, ref logFileEntity);

            var errors = new List<Dictionary<int[], string>>();

            string path = Path.Combine(Configuration.DefaultTempBaseFiles, _index);

            logFileEntity.Entity.Descricao = "Listando arquivos baixados no disco...";
            logFileEntity = _systemContext.LogPedidoImportacao.Update(logFileEntity.Entity);
            _systemContext.SaveChanges();

            var files = Directory.GetFiles(path);

            var linkListFiles = files.Where(a => a.Contains("link.txt")).FirstOrDefault();

            if (!string.IsNullOrEmpty(linkListFiles))
                files = this.DownloadListLink(linkListFiles, _index, ref logFileEntity);

            logFileEntity.Entity.Descricao = $"Iniciando leitura dos arquivos...";
            logFileEntity = _systemContext.LogPedidoImportacao.Update(logFileEntity.Entity);
            _systemContext.SaveChanges();

            foreach (var _file in files)
            {
                var logSingleFile = new LogPedidoImportacao
                {
                    Descricao = $"Iniciando leitura do arquivo {_file}...",
                    IndicadorStatus = "I",
                    PedidoImportacao = newPedidoEntity.Entity
                };

                _systemContext.LogPedidoImportacao.Add(logSingleFile);

                var file = File.ReadLines(_file);

                logSingleFile.Descricao = $"Gravando no banco ElasticSearch os dados do arquivo {_file}...";
                _systemContext.LogPedidoImportacao.Add(logSingleFile);

                var result = this.InserirArquivo(file, _index);

                if (result.Count > 0)
                    errors.Add(result);
                else
                    this.DeleteTempFiles(_file);
            }
        }

        private void DeleteTempFiles(string _filePath)
        {
            File.Delete(_filePath);
            var path = Path.GetDirectoryName(_filePath);
            Directory.Delete(path);
        }

        private string[] DownloadListLink(string _filepath, string _index, ref Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<LogPedidoImportacao> logFileEntity)
        {
            var context = logFileEntity.Context;
            logFileEntity.Entity.Descricao = "Encontrado arquivo de listagem de links...";
            logFileEntity = context.Update(logFileEntity.Entity);

            logFileEntity.Entity.Descricao = "Lendo Links para download...";
            logFileEntity = context.Update(logFileEntity.Entity);

            var links = File.ReadAllLines(_filepath);
            List<string> files = new List<string>() { };

            logFileEntity.Entity.Descricao = "Realizando Download dos arquivos listados na lista de links...";
            logFileEntity = context.Update(logFileEntity.Entity);
            for (var i = 0; i < links.Count(); i++)
            {
                logFileEntity.Entity.Descricao = $"Realizando download do link {links[i]} da lista de links...";
                logFileEntity = context.Update(logFileEntity.Entity);

                this.DownloadOnDisk(links[i], _index, ref logFileEntity);
                string path = Path.Combine(Path.GetDirectoryName(links[i]), _index);

                var newFiles = Directory.GetFiles(path);

                files.AddRange(newFiles);
            }

            return files.ToArray();
        }

        private Dictionary<int[], string> InserirArquivo(IEnumerable<string> file, string _nameBase)
        {
            var repository = this._unitOfWork.StartClient<Dictionary<string, string>>($"{_nameBase}Repository", _nameBase);

            var maxMemoryUsing = Configuration.MaxMemoryUsing;
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

                    this.WorkWithObjMemory(ref loadObjects, ref repository, ref isMemoryFull, cabecalho);

                    skip += contador;
                }
                catch (Exception erro)
                {
                    ramErrors.Add(new int[] { skip, contador }, $"[ERRO] - {erro.Message}");
                }
            }
            while (valorTotal < fileSize - 1);

            return ramErrors;
        }

        private void WorkWithObjMemory(ref List<Dictionary<string, string>> linha, ref IRepository<Dictionary<string, string>> repository, ref bool isMemoryFull, string[] cabecalho)
        {
            try
            {
                repository.BulkInsert(linha);
                linha.RemoveRange(0, linha.Count);
            }
            catch (Exception erro)
            {
                throw erro;
            }
            finally
            {
                isMemoryFull = false;
            }
        }

        public void DownloadOnDisk(string _url, string _nameBase, ref Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<LogPedidoImportacao> logFileEntity)
        {
            var destineBase = Path.Combine(Configuration.DefaultTempBaseFiles, _nameBase);
            var destineExtract = Path.Combine(Configuration.DefaultTempBaseFiles, _nameBase.Substring(0, _nameBase.Length - 4));
            var context = logFileEntity.Context;

            logFileEntity.Entity.Descricao = "Criando diretorio de extração...";
            logFileEntity = context.Update(logFileEntity.Entity);
            _systemContext.SaveChanges();

            Directory.CreateDirectory(destineExtract);

            var uri = new Uri(_url);

            logFileEntity.Entity.Descricao = $"Realizando download do arquivo {_nameBase}...";
            logFileEntity = context.Update(logFileEntity.Entity);
            _systemContext.SaveChanges();

            var webClient = new WebClient();
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadFile(uri, destineBase);

            logFileEntity.Entity.Descricao = $"Extraindo arquivo {_nameBase}...";
            logFileEntity = context.Update(logFileEntity.Entity);
            _systemContext.SaveChanges();
            ZipFile.ExtractToDirectory(destineBase, destineExtract);
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
