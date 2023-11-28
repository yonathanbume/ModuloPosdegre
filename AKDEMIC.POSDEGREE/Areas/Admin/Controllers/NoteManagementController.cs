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
    [Route("admin/note-management")]
    public class NoteManagementController : Controller
    {
        [Route("note-record")]
        public IActionResult NoteRecord()
        {
            return View();
        }
        [Route("note-control")]
        public IActionResult NoteControl()
        {
            return View();
        }
        [Route("note-list")]
        public IActionResult NoteList()
        {
            return View();
        }

    }
}
