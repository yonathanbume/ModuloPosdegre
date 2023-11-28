using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Areas.Admin.Models.MasterViewModel;
using AKDEMIC.POSDEGREE.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.DocumentaryProcedure.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Flurl.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/enrollment-management")]
    public class EnrollmentManagementController : Controller
    {
        [Route("register-enrollment")]
        public IActionResult RegisterEnrollment()
        {
            return View();
        }

        [Route("history-enrollment")]
        public IActionResult HistoryEnrollment()
        {
            return View();
        }

        [Route("list-enrollment")]
        public IActionResult ListEnrollment()
        {
            return View();
        }
    }
}
