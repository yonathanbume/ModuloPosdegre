using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Controllers
{
    public class ViewComponentController : Controller
    {
        public IActionResult Schedule()
        {
            return ViewComponent("Schedule");
        }
    }
}