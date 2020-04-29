using Infra.Entidades;
using Infra.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SystemHelper;

namespace Infra.EntityExtension
{
    public static class ArquivoBaseExtension
    {
        public static ICollection<Header> MakeHeader(this ArquivoBase fileBase)
        {
            var encoding = Functions.GetEncoding(fileBase.UrlOrigem);
            var file = File.ReadLines(fileBase.UrlOrigem, encoding);
            var cabecalho = file.FirstOrDefault().Split(';').Select(a => a.Replace("\"", "").Replace("\"", "")).Select(z => new Header {
                Name = z,
                HeaderType = HeaderType.Text
            });

            return cabecalho.ToList();
        }
    }
}
