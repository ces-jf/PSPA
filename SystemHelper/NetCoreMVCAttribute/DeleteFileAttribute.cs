using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;

namespace SystemHelper.NetCoreMVCAttribute
{
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //filterContext.HttpContext.Response.Flush();
            filterContext.HttpContext.Response.Body.Flush();

            //convert the current filter context to file and get the file path
            string filePath = ((filterContext.Result as FileStreamResult).FileStream as FileStream).Name;

            //delete the file after download
            System.IO.File.Delete(filePath);
        }
    }
}
