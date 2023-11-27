using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.ViewComponents
{
    public class Schedule : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
