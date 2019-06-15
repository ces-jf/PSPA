using System.Security.Principal;
using System.Threading.Tasks;
using Infra.Entidades;
using Infra.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infra.Business.Interfaces
{
    public interface IArquivoBaseBusiness
    {
        Task CadastrarBaseAsync(string _url, string _index, IPrincipal User);
        void DownloadOnDisk(string _url, string _nameBase, ref PedidoImportacao newPedidoEntity, IIdentityContext _systemContext);
    }
}