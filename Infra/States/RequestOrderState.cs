using Microsoft.AspNetCore.Mvc.Rendering;
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
        public StatusRequest StatusRequest { get; set; } = StatusRequest.ToUpload;
        public StatusDownloading StatusDownloading { get; set; }
        public IList<string> ExistingIndex { get; set; }
        public double DownloadValue { get; set; }
        public double WorkingValue { get; set; }
        public short ExistingValue { get; set; } = 0;
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

    public enum StatusRequest
    {
        Downloading,
        Uploading,
        ToUpload
    }

    public enum StatusDownloading
    {
        Extracting,
        Finished
    }
}
