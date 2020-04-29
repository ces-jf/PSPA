using Infra.Entidades;
using System.Collections.Generic;

namespace Infra.Business.Interfaces
{
    public interface IIndexBusiness
    {
        IEnumerable<Index> Listar();
        Index GetIndex(string index);
        IEnumerable<Header> Colunas(string nomeIndex);
    }
}