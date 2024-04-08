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
    [Route("admin/master")]
    public class MasterController : BaseController
    {
     
        private readonly IMasterService _masterService;
        private readonly IUserService _userService;
		private readonly IDataTablesService _dataTablesService;
        private readonly AkdemicContext _context;
		public MasterController(AkdemicContext context,IMasterService masterService,IUserService userService, IDataTablesService dataTablesService)
        {
            _masterService = masterService;
            _userService = userService;
			_dataTablesService = dataTablesService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
          return View();
        }
		[HttpGet("getallmaster")]
		public async Task<IActionResult> GetAllMaster(string search)
		{
            var parameters = _dataTablesService.GetSentParameters();
			var result = await _masterService.GetMasterDataTable(parameters,search);
			return Ok(result);
		}
        [HttpPost("matricula/{id}")]
        public async Task<IActionResult> Matricula(Guid id)
        {
            var maestria = _context.Masters.FirstOrDefault(u => u.id == id);
            if (maestria == null)
            {
                return NotFound();
            }
           
            return Ok(maestria);
        }
        [HttpGet("matricula")]
        public async Task<IActionResult> Matricula()
        { 
            return View();
        }
        [HttpPost("Agregar")]
        public async Task<IActionResult> AddPost(AddMasterViewModel model)
        {
            var entity = new Master
            {
                Nombre = model.Nombre,
                Duracion = model.Duracion,
                Creditos = model.Creditos,
                Descripcion = model.Descripcion,
                Campus = model.Sede,
                MallaCuricular = model.Curricula,
                StudyProgram= model.StudyProgram,
                StudyMode=model.StudyMode,
                current =model.current,
                state=model.state
            };
            await _masterService.Insert(entity);
            return RedirectToAction("Index");
   
        }

        [HttpPost("editar")]
        public async Task<IActionResult> Edit(AddMasterViewModel model)
        {
            
            var entity =  await _masterService.Get(model.Id);

            entity.Nombre = model.Nombre;
            entity.Duracion = model.Duracion;
            entity.Creditos = model.Creditos;
            entity.Descripcion = model.Descripcion;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _masterService.DeleteMaster(id);
            return RedirectToAction("Index");
        }
        [HttpGet("reporte-excel")]
        public async Task<IActionResult> GetExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte");
                try
                {
                    await _masterService.DownloadExcel(worksheet);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

                worksheet.RangeUsed().SetAutoFilter();

                HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";


                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Padron de registro.xlsx");
                }
            }
        }

    }
}
