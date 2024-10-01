using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lab9
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult =
           HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case (int)HttpStatusCode.NotFound:
                    ViewBag.ErrorMessage = "Извините, страница не найдена";
                    ViewBag.RouteOfException = statusCodeResult?.OriginalPath;
                    return View("NotFound");
                case (int)HttpStatusCode.InternalServerError:
                    ViewBag.ErrorMessage = "Внутренняя ошибка сервера";
                    ViewBag.RouteOfException = statusCodeResult?.OriginalPath;
                    return View("ServerError");
                default:
                    ViewBag.ErrorMessage = "Произошла ошибка";
                    ViewBag.RouteOfException = statusCodeResult?.OriginalPath;
                    return View("Error");
            }
        }
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionDetails =
           HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ErrorMessage = exceptionDetails?.Error.Message;
            ViewBag.RouteOfException = exceptionDetails?.Path;
            if (exceptionDetails?.Error is System.Exception)
            {
                ViewBag.ErrorMessage = "ошибка сервера, нужная";
                return View("ServerError");
            }
            return View("Error");
        }
    }

}
