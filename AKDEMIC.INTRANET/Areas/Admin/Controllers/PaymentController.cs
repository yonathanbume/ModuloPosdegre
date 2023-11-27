using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    public class PaymentController : Controller
    {
        /// <summary>
        /// Vista principal donde se listan los trámites
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
