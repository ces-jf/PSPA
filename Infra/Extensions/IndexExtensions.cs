using Data.Class;
using Infra.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.Extensions
{
    public static class IndexExtensions
    {
        private static readonly UnitOfWork UnitOfWork = new UnitOfWork();

        public static bool IsCreated(this Index _index, string _indexName)
        {
            return UnitOfWork.IndexExists(_indexName);
        }
    }
}
