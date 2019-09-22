using Infra.Business.Classes.Identity;
using Infra.Business.Interfaces;
using Infra.Class;
using Infra.Entidades;
using Infra.Interfaces;
using Ionic.Zip;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using SystemHelper;

namespace Infra.Business.Classes
{
    public class ArquivoBaseBusiness : BusinessBase, IArquivoBaseBusiness
    {
        public readonly IdentityBusiness identityBusiness;
        private PedidoImportacao _importacaoWebClient { get; set; }
        private IIdentityContext _identityWebClient { get; set; }
        private IList<int> _valorPercentual { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        public ArquivoBaseBusiness(IUnitOfWork _unitOfWork, IIdentityContext _systemContext, IdentityBusiness _identityBusiness, IServiceProvider serviceProvider) : base(_unitOfWork, _systemContext)
        {
            this.identityBusiness = _identityBusiness;
            this.ServiceProvider = serviceProvider;
            this._valorPercentual = new List<int>();
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

            var newPedidoEntity = new PedidoImportacao
            {
                LogPedidoImportacao = new List<LogPedidoImportacao>(),
                Estado = "A",
                Arquivos = new List<ArquivoBase>()
                {
                    new ArquivoBase
                    {
                        Index = new Index
                        {
                            Name = _index
                        },
                        Nome = "inicial.zip",
                        UrlOrigem = _url
                    }
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

            return newPedidoEntity;
        }

        private void NewMethod(string _index, PedidoImportacao newPedidoEntity, List<Dictionary<int[], string>> errors, string[] files)
        {
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

                //var result = this.InserirArquivo(file, _index);

                //if (result.Count > 0)
                //    errors.Add(result);
                //else
                //    this.DeleteTempFiles(_file);
            }
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

                newPedidoEntity.Estado = "E";
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

        public void InserirArquivo(PedidoImportacao pedido, IIdentityContext context)
        {
            pedido.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = "Iniciando conexão com ElasticSearch...",
                IndicadorStatus = "I"
            });

            var arquivo = pedido.Arquivos.FirstOrDefault();
            var index = arquivo.Index;

            context.SaveChanges();

            var repository = this._unitOfWork.StartClient<Dictionary<string, string>>($"{index.Name}Repository", index.Name);

            bool isMemoryFull = false;

            var _file = pedido.Arquivos.Where(a => a.Nome != arquivo.Nome).FirstOrDefault();

            pedido.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = $"Iniciando leitura do arquivo {_file.Nome}...",
                IndicadorStatus = "I"
            });

            context.SaveChanges();

            var filePath = Path.Combine(Configuration.DefaultTempBaseFiles, pedido.PastaTemp, _file.Nome);

            var encoding = Functions.GetEncoding(filePath);
            var file = File.ReadLines(filePath, encoding);

            var cabecalho = file.FirstOrDefault().Split(';').Select(a => a.Replace("\"", "").Replace("\"", "")).ToArray();
            var loadObjects = new List<Dictionary<string, string>>();
            int valorTotal = 0;
            int fileSize = file.Count();

            int contador = 200000;
            int skip = 1;

            var ramErrors = new Dictionary<int[], string>(); //skip, contador

            pedido.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = $"Iniciando gravação do arquivo {_file.Nome}...",
                IndicadorStatus = "I"
            });

            context.SaveChanges();

            do
            {
                try
                {
                    var listObject = new List<Dictionary<string, string>>();

                    loadObjects.AddRange(file.Skip(skip).Take(contador).Select(a => a.Split(';').Select(t => t.Replace("\"", "").Replace("\"", "")).ToArray().Zip(cabecalho, (value, key) => new { key, value }).ToDictionary(z => z.key, b => b.value)));

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

            this.DeleteTempFiles(filePath);

            if (ramErrors.Count > 0)
            {
                foreach (var error in ramErrors)
                {
                    pedido.LogPedidoImportacao.Add(new LogPedidoImportacao()
                    {
                        Descricao = $"Erro ao gravar o arquivo {_file.Nome} - {error.Value}...",
                        IndicadorStatus = "E"
                    });
                }

                pedido.Estado = "E";

                context.SaveChanges();
                return;
            }

            pedido.LogPedidoImportacao.Add(new LogPedidoImportacao()
            {
                Descricao = $"Processo de leitura do arquivo finalizado sem erros",
                IndicadorStatus = "C"
            });

            pedido.Estado = "C";

            context.SaveChanges();
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
                newPedidoEntity.Estado = "E";
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
            this._importacaoWebClient.LogPedidoImportacao.Add(new LogPedidoImportacao
            {
                Descricao = $"Donwload Finalizado",
                IndicadorStatus = "I"
            });
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage.ToString().Length != 1 && e.ProgressPercentage.ToString().EndsWith("0"))
            {
                if (this._valorPercentual.Any(a => a == e.ProgressPercentage))
                    return;

                this._valorPercentual.Add(e.ProgressPercentage);

                var logPedidoImportacao = new LogPedidoImportacao
                {
                    Descricao = $"Progresso do Download: {e.ProgressPercentage}",
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

        public void UpdateToRegisterData(PedidoImportacao pedido, IIdentityContext context)
        {
            pedido.Estado = "AR";
            context.SaveChanges();
        }

        public string ConsultaToCSV(IPrincipal user, string indexName, IEnumerable<string> selectFilter = null, IEnumerable<Tuple<string, string, string>> filterFilter = null, int numberEntries = 1000, bool allEntries = false)
        {
            try
            {
                int from = 0;
                int size = 10000;

                var tempDownloadFolderUser = Path.Combine(Configuration.DefaultTempFolder, user.Identity.Name, "downloadTemp");
                Directory.CreateDirectory(tempDownloadFolderUser);

                var fileName = Path.Combine(tempDownloadFolderUser, $"{indexName}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");

                do
                {
                    var result = _unitOfWork.MatchAll(indexName, selectFilter, filterFilter, from, size);

                    var export = new CsvExport();

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

                    export.Dispose();

                    from += size;

                    if ((from + size) >= numberEntries)
                        size = numberEntries - from;
                }
                while (from < numberEntries);

                return fileName;
            }
            catch(Exception erro)
            {
                throw erro;
            }
        }
    }
}
