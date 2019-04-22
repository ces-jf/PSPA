using Infra.Class;
using Infra.Interfaces;
using System;
using System.Security.Principal;

namespace Infra.Business
{
    public class BusinessBase : IDisposable, IBusinessBase
    {
        public IUnitOfWork _unitOfWork { get; set; }
        public ISystemContext _systemContext { get; set; }
        public IPrincipal User { get; set; }

        public BusinessBase(IPrincipal User)
        {
            this.User = User;
        }

        public BusinessBase(IUnitOfWork unitOfWork, IPrincipal User) : this(User)
        {
            this._unitOfWork = unitOfWork;
        }

        public BusinessBase(ISystemContext systemContext, IPrincipal User) : this(User)
        {
            this._systemContext = systemContext;
        }

        public BusinessBase(IUnitOfWork unitOfWork, ISystemContext systemContext, IPrincipal User) : this(User)
        {
            this._unitOfWork = unitOfWork;
            this._systemContext = systemContext;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: descartar estado gerenciado (objetos gerenciados).
                }

                // TODO: liberar recursos não gerenciados (objetos não gerenciados) e substituir um finalizador abaixo.
                // TODO: definir campos grandes como nulos.

                disposedValue = true;
            }
        }

        // TODO: substituir um finalizador somente se Dispose(bool disposing) acima tiver o código para liberar recursos não gerenciados.
        // ~BusinessBase() {
        //   // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
        //   Dispose(false);
        // }

        // Código adicionado para implementar corretamente o padrão descartável.
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
            Dispose(true);
            // TODO: remover marca de comentário da linha a seguir se o finalizador for substituído acima.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
