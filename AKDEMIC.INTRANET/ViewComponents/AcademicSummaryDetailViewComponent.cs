using AKDEMIC.INTRANET.ViewModels.AcademicSummaryViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.ViewComponents
{
    [ViewComponent(Name = "AcademicSummaryDetail")]
    public class AcademicSummaryDetailViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(StudentTermViewModel model) => View(model);
    }
}
