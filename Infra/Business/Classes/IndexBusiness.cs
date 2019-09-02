using Infra.Business.Interfaces;
using Infra.Class;
using Infra.Entidades;
using Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Business.Classes
{
    public class IndexBusiness : BusinessBase, IIndexBusiness
    {
        public IndexBusiness(IUnitOfWork _unitOfWork, IIdentityContext _identityContext) : base(_unitOfWork, _identityContext)
        {

        }

        public IEnumerable<Index> Listar()
        {
            try
            {
                return _unitOfWork.ListarIndices();
            }
            catch(Exception erro)
            {
                throw erro;
            }
        }

        public IEnumerable<Cabecalho> Colunas(string nomeIndex)
        {
            nomeIndex = nomeIndex ?? "";

            var nomeIndexLower = nomeIndex.ToLowerInvariant();
            try
            {
                return _unitOfWork.Colunas(nomeIndexLower);
            }
            catch(Exception erro)
            {
                throw erro;
            }
        }
    }
}
