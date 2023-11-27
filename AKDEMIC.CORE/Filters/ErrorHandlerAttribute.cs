using AKDEMIC.CORE.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace AKDEMIC.CORE.Filters
{
    public class ErrorHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //filterContext.HttpContext.Response.ContentType = "application/json; charset=utf-8";
                if (filterContext.Exception.GetType() == typeof(XMLHttpRequestException))
                {
                    filterContext.Result = new ObjectResult(filterContext.Exception.Message);
                }
                else
                {
                    filterContext.Result = new ObjectResult("Ocurrió un problema en el servidor.");
                }
            }
            else
            {
                base.OnException(filterContext);
            }
        }
    }
}