using Infra.Entidades;
using Infra.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.States
{
    public class RequestAdjustImport
    {
        public List<HeaderViewModel> Headers { get; set; }
        public bool CanEdit { get; set; }
    }

    public class HeaderViewModel: Header
    {
        public string FileName
        {
            get
            {
                return this.ArquivoBase.Nome;
            }
        }

        public Action StateHasChanged { get; set; }
    }

    public class HeaderTypeViewModel
    {
        public HeaderType Type { get; set; }
        public string DisplayText { get; set; }
    }
}
