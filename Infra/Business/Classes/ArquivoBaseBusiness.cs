using Infra.Business.Classes.Identity;
using Infra.Business.Interfaces;
using Infra.Class;
using Infra.Entidades;
using Infra.Enums;
using Infra.Interfaces;
using Ionic.Zip;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using SystemHelper;
using Index = Infra.Entidades.Index;

namespace Infra.Business.Classes
{
    public class ArquivoBaseBusiness : BusinessBase, IArquivoBaseBusiness
    {
        private readonly IdentityBusiness identityBusiness;
        private readonly IIndexBusiness indexBusiness;
        private PedidoImportacao _importacaoWebClient { get; set; }
        private IIdentityContext _identityWebClient { get; set; }
        private IList<int> _valorPercentual { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        public ArquivoBaseBusiness(IUnitOfWork _unitOfWork, IIdentityContext _systemContext, IdentityBusiness _identityBusiness, IServiceProvider serviceProvider, IIndexBusiness indexBusiness) : base(_unitOfWork, _systemContext)
        {
            this.identityBusiness = _identityBusiness;
            this.ServiceProvider = serviceProvider;
            this._valorPercentual = new List<int>();
            this.indexBusiness = indexBusiness;
        }

        public async Task<PedidoImportacao> CreateImportRequestAsync(string _url, string _index, IPrincipal User)
        {
            Usuario usuario;

            using (var identityContext = identityBusiness)
            {
                usuario = await identityContext.GetUsuarioAsync(User as ClaimsPrincipal);
                usuario = _systemContext.Usuario.Include(a => a.PedidosImportacao).FirstOrDefault(a => a.Id == usuario.Id);
            }

            if (usuario == null)
                throw new Exception("Usuario não encontrado.");

            var index = indexBusiness.GetIndex(_index);

            if (index != null)
            {
                index = _systemContext.Indice.Include(a => a.ArquivoBases).FirstOrDefault(a => a.Name == _index);
            }

            if(index == null)
            {
                index = new Index
                {
                    Name = _index,
                    ArquivoBases = new List<ArquivoBase>()
                };
            }

            var fileBase = new ArquivoBase 
            {
                Nome = "inicial.zip",
                UrlOrigem = _url
            };
            index.ArquivoBases.Add(fileBase);
            _systemContext.SaveChanges();

            var newPedidoEntity = new PedidoImportacao
            {
                LogPedidoImportacao = new List<LogPedidoImportacao>(),
                OrderState = OrderState.Waiting,
                Arquivos = new List<ArquivoBase>()
                {
                    fileBase
                },
                Usuario = usuario,
                PastaTemp = $"{_index}Temp"
            };

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = "Aguardando processo de download iniciar...",
                IndicadorStatus = "I"
            });

            usuario.PedidosImportacao.Add(newPedidoEntity);

            _systemContext.SaveChanges();

            var orderGenerated = _systemContext.PedidoImportacao.Include(inc => inc.Arquivos).ThenInclude(then => then.Index).AsNoTracking().FirstOrDefault(a => a.ID == newPedidoEntity.ID);

            return orderGenerated;
        }

        public string[] CheckFileList(string fileName, PedidoImportacao newPedidoEntity, IIdentityContext _systemContext)
        {
            try
            {
                string path = Path.Combine(Configuration.DefaultTempBaseFiles, newPedidoEntity.PastaTemp);
                var index = newPedidoEntity.Arquivos.FirstOrDefault().Index;

                newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
                {
                    Descricao = "Listando arquivos baixados no disco...",
                    IndicadorStatus = "I"
                });
                _systemContext.SaveChanges();

                var files = Directory.GetFiles(path);

                var linkListFiles = files.Where(a => a.Contains("link.txt")).FirstOrDefault();

                if (!string.IsNullOrEmpty(linkListFiles))
                    files = this.DownloadListLink(linkListFiles, ref newPedidoEntity, _systemContext);

                foreach (var file in files)
                {
                    newPedidoEntity.Arquivos.Add(new ArquivoBase
                    {
                        Index = index,
                        Nome = Path.GetFileName(file),
                        UrlOrigem = "."
                    });
                }

                newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
                {
                    Descricao = "Registrando novos arquivos para leitura...",
                    IndicadorStatus = "I"
                });
                _systemContext.SaveChanges();

                return files;
            }
            catch (Exception erro)
            {
                newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
                {
                    Descricao = $"Erro ocorrido ao checar arquivos: {erro.Message}",
                    IndicadorStatus = "E"
                });

                newPedidoEntity.OrderState = OrderState.Error;
                _systemContext.SaveChanges();

                throw erro;
            }
        }

        private void DeleteTempFiles(string _filePath)
        {
            var path = Path.GetDirectoryName(_filePath);
            var filesIn = Directory.GetFiles(path);

            foreach (var file in filesIn)
            {
                File.Delete(file);
            }

            Directory.Delete(path);
        }

        private string[] DownloadListLink(string _filepath, ref PedidoImportacao newPedidoEntity, IIdentityContext _systemContext)
        {
            var index = newPedidoEntity.Arquivos.FirstOrDefault().Index;
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
                var arquivo = new ArquivoBase
                {
                    Index = index,
                    Nome = $"Arquivo{i}",
                    UrlOrigem = links[i]
                };

                newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
                {
                    Descricao = $"Realizando download do link {links[i]} da lista de links...",
                    IndicadorStatus = "I"
                });
                newPedidoEntity.Arquivos.Add(arquivo);
                _systemContext.SaveChanges();

                this.DownloadOnDisk(arquivo, newPedidoEntity, _systemContext);
            }

            var path = Path.Combine(Configuration.DefaultTempBaseFiles, newPedidoEntity.PastaTemp);
            var filesList = Directory.GetFiles(path);
            files.AddRange(filesList);

            return files.ToArray();
        }

        public bool InsertFile(long order, long fileId)
        {
            using var serviceScope = ServiceProvider.CreateScope();
            using var scopedContext = serviceScope.ServiceProvider.GetRequiredService<IIdentityContext>();
            var orderEntity = scopedContext.PedidoImportacao.FirstOrDefault(a => a.ID == order);
            var fileToImport = scopedContext.ArquivoBase.Include(a => a.Index).Include(z => z.Headers).FirstOrDefault(a => a.ID == fileId);
            var index = fileToImport.Index;
            var headers = fileToImport.Headers;

            orderEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = "Iniciando conexão com ElasticSearch...",
                IndicadorStatus = "I"
            });
            scopedContext.SaveChanges();

            var repository = this._unitOfWork.StartClient<Dictionary<string, string>>($"{index.Name}Repository", index.Name);

            var _file = fileToImport;

            orderEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = $"Iniciando leitura do arquivo {_file.Nome}...",
                IndicadorStatus = "I"
            });
            scopedContext.SaveChanges();

            var encoding = Functions.GetEncoding(_file.UrlOrigem);
            var file = File.ReadLines(_file.UrlOrigem, encoding);

            var cabecalho = file.FirstOrDefault().Split(';').Select(a => a.Replace("\"", "").Replace("\"", "")).ToArray();
            int skip = 1;

            var ramErrors = new Dictionary<Dictionary<string, string>, string>(); //skip, contador

            orderEntity.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = $"Iniciando gravação do arquivo {_file.Nome}...",
                IndicadorStatus = "I"
            });
            scopedContext.SaveChanges();

            var fileQuery = file.Skip(skip);

            Parallel.ForEach(fileQuery, (item) =>
            {
                var itemTemp = item.Split(';').Select(t => t.Replace("\"", "").Replace("\"", "")).ToArray().Zip(cabecalho, (value, key) => new { key, value }).ToDictionary(z => z.key, b => b.value);
                try
                {
                    repository.Insert(itemTemp);
                }
                catch (Exception erro)
                {
                    ramErrors.Add(itemTemp, $"[ERRO] - {erro.Message}");
                }
            });

            if (ramErrors.Count > 0)
            {
                foreach (var ramError in ramErrors)
                {
                    string lineError = $"[{string.Join(";", ramError.Key.Select(a => $"({a.Key}) => {a.Value}"))}]";
                    string errorMessage = $"Erro ao importar arquivo {_file.Nome} - {lineError} <=> {ramError.Value}";

                    orderEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
                    {
                        Descricao = errorMessage,
                        IndicadorStatus = "E"
                    });
                    scopedContext.SaveChanges();
                }

                return false;
            }

            orderEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = $"Arquivo {_file.Nome} importado com sucesso!",
                IndicadorStatus = "I"
            });
            scopedContext.SaveChanges();

            return true;
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

        public void DownloadOnDisk(ArquivoBase arquivo, PedidoImportacao newPedidoEntity, IIdentityContext _context)
        {
            var index = arquivo.Index;

            var destineBase = Path.Combine(Configuration.DefaultTempBaseFiles, index.Name);
            var destineExtract = Path.Combine(Configuration.DefaultTempBaseFiles, newPedidoEntity.PastaTemp);

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = "Criando diretorio de extração...",
                IndicadorStatus = "I"
            });

            try
            {
                _context.SaveChanges();
            }
            catch (Exception erro)
            {
                throw erro;
            }

            //LinuxHelpers.Exec($"chmod 777 {Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")}");
            Directory.CreateDirectory(destineExtract);
            Directory.CreateDirectory(destineBase);
            var fileBaseName = Path.Combine(destineBase, arquivo.Nome);

            var uri = new Uri(arquivo.UrlOrigem);

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = $"Realizando download do arquivo {arquivo.Nome}...",
                IndicadorStatus = "I"
            });

            try
            {
                _context.SaveChanges();
            }
            catch (Exception erro)
            {
                throw erro;
            }

            var webClient = new WebClient();

            Debug.WriteLine("--------------------------------------------------");
            Debug.WriteLine("Iniciando Download");
            this._importacaoWebClient = newPedidoEntity;
            this._identityWebClient = _context;

            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            try
            {
                webClient.DownloadFileTaskAsync(uri, $"{fileBaseName}").GetAwaiter().GetResult();
                newPedidoEntity = this._importacaoWebClient;
                _context = this._identityWebClient;
            }
            catch (Exception erro)
            {
                newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
                {
                    Descricao = $"Erro ao registrar a importação: {erro.Message}...",
                    IndicadorStatus = "E"
                });
                newPedidoEntity.OrderState = OrderState.Error;
                _context.SaveChanges();

                throw erro;
            }

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = $"Extraindo arquivo {arquivo.Nome}...",
                IndicadorStatus = "I"
            });
            _context.SaveChanges();

            //if(Directory.Exists(destineExtract))
            //{
            //    Directory.Delete()
            //}

            using (ZipFile zip1 = ZipFile.Read(fileBaseName))
            {
                foreach (ZipEntry zip in zip1)
                {
                    zip.Extract(destineExtract, ExtractExistingFileAction.OverwriteSilently);
                }
            }

            //     ZipFile.ExtractToDirectory(fileBaseName, destineExtract);

            newPedidoEntity.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = $"Arquivos extraidos e sendo encaminhados para a fase de analise",
                IndicadorStatus = "I"
            });
            _context.SaveChanges();
        }


        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var logLine = $"Donwload Finalizado";

            this._importacaoWebClient.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = logLine,
                IndicadorStatus = "I"
            });
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Thread.Sleep(10000);

            if (e.ProgressPercentage.ToString().Length != 1 && e.ProgressPercentage.ToString().EndsWith("0"))
            {
                if (this._valorPercentual.ToList().Any(a => a == e.ProgressPercentage))
                    return;

                this._valorPercentual.Add(e.ProgressPercentage);

                var logLine = $"Progresso do Download: {e.ProgressPercentage}";

                var logPedidoImportacao = new LogPedidoImportacao
                {
                    Descricao = logLine,
                    IndicadorStatus = "I",
                    PedidoImportacao = this._importacaoWebClient
                };

                this._identityWebClient.LogPedidoImportacao.Add(logPedidoImportacao);

                this._identityWebClient.SaveChanges();
            }
        }

        public void RegisterNewFiles(string[] files, PedidoImportacao pedido, IIdentityContext context)
        {
            var index = pedido.Arquivos.FirstOrDefault().Index;

            for (var i = 0; i < files.Length; i++)
            {
                var fileName = Path.GetFileName(files[i]);

                pedido.Arquivos.Add(new ArquivoBase
                {
                    Index = index,
                    Nome = fileName
                });
            }

            context.SaveChanges();
        }

        public IList<Dictionary<string, string>> QueryGroupData(string indexName, IList<string> selectFilter = null, IEnumerable<Tuple<string, string, string>> filterFilter = null, long numberEntries = 1000, bool allEntries = false)
        {
            try
            {
                int from = 0;
                int size = 10000;

                var resultFinal = new List<Dictionary<string, string>>();

                if (allEntries)
                    numberEntries = _unitOfWork.TotalDocuments(indexName: indexName);

                do
                {
                    var result = this._unitOfWork.MatchAll(indexName: indexName, columnGroup: true, selectFilter: selectFilter, filterFilter: filterFilter, from: from, size: size);

                    resultFinal.AddRange(result);

                    result.Clear();

                    from += size;

                    if ((from + size) >= numberEntries)
                        size = (int)(numberEntries - from);

                }
                while (from < numberEntries);

                return resultFinal;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public string ConsultaToCSV(IPrincipal user, string indexName, IList<string> selectFilter = null, IEnumerable<Tuple<string, string, string>> filterFilter = null, long numberEntries = 1000, bool allEntries = false)
        {
            try
            {
                int from = 0;
                int size = 10000;

                var tempDownloadFolderUser = Path.Combine(Configuration.DefaultTempFolder, user.Identity.Name, "downloadTemp");
                Directory.CreateDirectory(tempDownloadFolderUser);

                var fileName = Path.Combine(tempDownloadFolderUser, $"{indexName}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");

                if (allEntries)
                    numberEntries = _unitOfWork.TotalDocuments(indexName: indexName);

                do
                {
                    using (var export = new CsvExport())
                    {
                        var result = this._unitOfWork.MatchAll(indexName: indexName, selectFilter: selectFilter, filterFilter: filterFilter, from: from, size: size);

                        foreach (var item in result)
                        {
                            export.AddRow();

                            foreach (var key in item.Keys)
                            {
                                export[key] = item[key];
                            }
                        }

                        result.Clear();

                        if (from == 0)
                            export.ExportToFile(fileName, includeHeader: true);
                        else
                            export.AddLinesToFile(fileName);
                    }

                    from += size;

                    if ((from + size) >= numberEntries)
                        size = (int)(numberEntries - from);

                }
                while (from < numberEntries);

                return fileName;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
    }
}
