using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.ViewComponents
{
    [ViewComponent(Name = "AcademicSituationElective")]
    public class AcademicSituationElectiveViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
