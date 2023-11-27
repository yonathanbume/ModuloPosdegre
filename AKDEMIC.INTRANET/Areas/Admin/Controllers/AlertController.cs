using AKDEMIC.INTRANET.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    public class AlertController : BaseController
    {
        public AlertController() : base() { }

        public IActionResult Index() => View();
    }
}