using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Models.PosDegree;
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
    [Route("admin/payment-management")]
    public class PaymentManagementController : Controller
    {
        [Route("payment-control")]
        public IActionResult PaymentControl()
        {
            return View();
        }

        [Route("payment-record")]
        public IActionResult PaymentRecord()
        {
            return View();
        }
    }
}
