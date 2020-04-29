using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Infra.Entidades;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SystemHelper;

namespace Infra.Business.Interfaces
{
    public interface IArquivoBaseBusiness :IDisposable
    {
        Task<PedidoImportacao> CreateImportRequestAsync(string _url, string _index, IPrincipal User);
        void DownloadOnDisk(ArquivoBase arquivo, PedidoImportacao newPedidoEntity, IIdentityContext _context);
        string[] CheckFileList(string _index, PedidoImportacao newPedidoEntity, IIdentityContext _systemContext);
        void RegisterNewFiles(string[] files, PedidoImportacao pedido, IIdentityContext context);
        bool InsertFile(long order, long fileId);
        IList<Dictionary<string, string>> QueryGroupData(string indexName, IList<string> selectFilter = null, IEnumerable<Tuple<string, string, string>> filterFilter = null, long numberEntries = 1000, bool allEntries = false);
        string ConsultaToCSV(IPrincipal User, string indexName, IList<string> selectFilter = null, IEnumerable<Tuple<string, string, string>> filterFilter = null, long numberEntries = 1000, bool allEntries = false);
    }
}