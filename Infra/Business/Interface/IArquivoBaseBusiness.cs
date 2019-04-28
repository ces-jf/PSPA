using System.Threading.Tasks;
using Infra.Entidades;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infra.Business.Interfaces
{
    public interface IArquivoBaseBusiness
    {
        Task CadastrarBaseAsync(string _url, string _index);
        void DownloadOnDisk(string _url, string _nameBase, ref EntityEntry<LogPedidoImportacao> logFileEntity);
    }
}