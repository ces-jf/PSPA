using Infra.Entidades;
using System.Collections.Generic;

namespace Infra.Business.Interfaces
{
    public interface IIndexBusiness
    {
        IEnumerable<Index> Listar();
        IEnumerable<Cabecalho> Colunas(string nomeIndex);
    }
}