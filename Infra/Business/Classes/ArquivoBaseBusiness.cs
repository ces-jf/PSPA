using Infra.Business.Classes.Identity;
using Infra.Business.Interfaces;
using Infra.Class;
using Infra.Entidades;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using SystemHelper;

namespace Infra.Business.Classes
{
    public class ArquivoBaseBusiness : BusinessBase, IArquivoBaseBusiness
    {
        public readonly IdentityBusiness identityBusiness;
        private PedidoImportacao _importacaoWebClient { get; set; }

        public ArquivoBaseBusiness(IUnitOfWork _unitOfWork, IIdentityContext _systemContext, IdentityBusiness _identityBusiness) : base(_unitOfWork, _systemContext) { this.identityBusiness = _identityBusiness; }

        public async Task CadastrarBaseAsync(string _url, string _index, IPrincipal User)
        {
            Usuario usuario;

            using (var identityContext = identityBusiness)
            {
                usuario = await identityContext.GetUsuarioAsync(User as ClaimsPrincipal);
                usuario = _systemContext.Usuario.Include(a => a.PedidosImportacao).FirstOrDefault(a => a.Id == usuario.Id);
            }

            if (usuario == null)
                throw new Exception("Usuario não encontrado.");

            var newPedidoEntity = new PedidoImportacao
            {
                LogPedidoImportacao = new List<LogPedidoImportacao>()
            };

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = "Baixando Arquivos do Disco...",
                IndicadorStatus = "I"
            });

            usuario.PedidosImportacao.Add(newPedidoEntity);

            _systemContext.SaveChanges();
            _systemContext.PedidoImportacao.Attach(newPedidoEntity);

            this.DownloadOnDisk(_url, _index, ref newPedidoEntity, _systemContext);

            var errors = new List<Dictionary<int[], string>>();

            string path = Path.Combine(Configuration.DefaultTempBaseFiles, _index);

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = "Listando arquivos baixados no disco...",
                IndicadorStatus = "I"
            });
            _systemContext.SaveChanges();

            var files = Directory.GetFiles(path);

            var linkListFiles = files.Where(a => a.Contains("link.txt")).FirstOrDefault();

            if (!string.IsNullOrEmpty(linkListFiles))
                files = this.DownloadListLink(linkListFiles, _index, ref newPedidoEntity, _systemContext);

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = $"Iniciando leitura dos arquivos...",
                IndicadorStatus = "I"
            });
            _systemContext.SaveChanges();

            foreach (var _file in files)
            {
                newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
                {
                    Descricao = $"Iniciando leitura do arquivo {_file}...",
                    IndicadorStatus = "I"
                });

                _systemContext.SaveChanges();

                var file = File.ReadLines(_file);

                newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
                {
                    Descricao = $"Gravando no banco ElasticSearch os dados do arquivo {_file}...",
                    IndicadorStatus = "I"
                });
                _systemContext.SaveChanges();

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

        private string[] DownloadListLink(string _filepath, string _index, ref PedidoImportacao newPedidoEntity, IIdentityContext _systemContext)
        {
            var context = _systemContext;

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = "Encontrado arquivo de listagem de links...",
                IndicadorStatus = "I"
            });
            _systemContext.SaveChanges();

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = "Lendo Links para download...",
                IndicadorStatus = "I"
            });
            _systemContext.SaveChanges();

            var links = File.ReadAllLines(_filepath);
            List<string> files = new List<string>() { };

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = "Realizando Download dos arquivos listados na lista de links...",
                IndicadorStatus = "I"
            });
            _systemContext.SaveChanges();

            for (var i = 0; i < links.Count(); i++)
            {
                newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
                {
                    Descricao = $"Realizando download do link {links[i]} da lista de links...",
                    IndicadorStatus = "I"
                });
                _systemContext.SaveChanges();

                //this.DownloadOnDisk(links[i], _index, ref logFileEntity);
                string path = Path.Combine(Path.GetDirectoryName(links[i]), _index);

                var newFiles = Directory.GetFiles(path);

                files.AddRange(newFiles);
            }

            return files.ToArray();
        }

        private Dictionary<int[], string> InserirArquivo(IEnumerable<string> file, string _nameBase)
        {
            var repository = this._unitOfWork.StartClient<Dictionary<string, string>>($"{_nameBase}Repository", _nameBase);

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

        public void DownloadOnDisk(string _url, string _nameBase, ref PedidoImportacao newPedidoEntity, IIdentityContext _systemContext)
        {
            var destineBase = Path.Combine(Configuration.DefaultTempBaseFiles, _nameBase);
            var destineExtract = Path.Combine(Configuration.DefaultTempBaseFiles, $"{_nameBase}Temp");
            var context = _systemContext;

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = "Criando diretorio de extração...",
                IndicadorStatus = "I"
            });

            _systemContext.SaveChanges();

            Directory.CreateDirectory(destineExtract);
            Directory.CreateDirectory(destineBase);

            var uri = new Uri(_url);

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = $"Realizando download do arquivo {_nameBase}...",
                IndicadorStatus = "I"
            });

            _systemContext.SaveChanges();

            var webClient = new WebClient();

            Debug.WriteLine("--------------------------------------------------");
            Debug.WriteLine("Iniciando Download");
            this._importacaoWebClient = newPedidoEntity;

            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadFile(uri, destineBase);

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = $"Extraindo arquivo {_nameBase}...",
                IndicadorStatus = "I"
            });
            _systemContext.SaveChanges();
            ZipFile.ExtractToDirectory(destineBase, destineExtract);
        }


        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this._importacaoWebClient.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = $"Donwload Finalizado",
                IndicadorStatus = "I"
            });

            _systemContext.SaveChanges();
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage.ToString().Contains("5"))
            {
                this._importacaoWebClient.LogPedidoImportacao.Add(new LogPedidoImportacao
                {
                    Descricao = $"Progresso do Download: {e.ProgressPercentage}",
                    IndicadorStatus = "I"
                });
            }

            _systemContext.SaveChanges();
        }
    }
}
