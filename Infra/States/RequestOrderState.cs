using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infra.States
{
    public class RequestOrderState
    {
        public RequestOrderViewModel Input { get; set; } = new RequestOrderViewModel();
        public string SuccessReturn { get; set; }
        public IList<string> ErrorReturn { get; set; } = new List<string>();
    }

    public class RequestOrderViewModel
    {
        [Required]
        [Display(Name = "File Address")]
        public string Url { get; set; }
        [Required]
        [Display(Name = "Database Name")]
        public string Index { get; set; }
    }
}
