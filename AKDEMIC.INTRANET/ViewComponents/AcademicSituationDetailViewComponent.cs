using Microsoft.AspNetCore.Mvc;
using AKDEMIC.INTRANET.ViewModels.AcademicSituationViewModels;

namespace AKDEMIC.INTRANET.ViewComponents
{
    [ViewComponent(Name = "AcademicSituationDetail")]
    public class AcademicSituationDetailViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AcademicYearViewModel model) => View(model);
    }
}
