using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Controllers;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/TeachingLoad")]
    public class TeachingLoadController : BaseController
    {
        public readonly IAsignaturaService _asignaturaService;
        public readonly IMasterService _masterService;
        public readonly ISemestreService _semestreService;
        public readonly ITeacherPService _teacherPService;
        public TeachingLoadController(IAsignaturaService asignaturaService, IMasterService masterService, ISemestreService semestreService,
         ITeacherPService teacherPService)
        {
            _asignaturaService = asignaturaService;
            _masterService = masterService;
            _semestreService = semestreService;
            _teacherPService = teacherPService;
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}

