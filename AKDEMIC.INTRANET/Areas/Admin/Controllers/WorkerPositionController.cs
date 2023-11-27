using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.WorkerPositionViewModels;
using AKDEMIC.INTRANET.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.ENTITIES.Models;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("admin/cargos")]
    public class WorkerPositionController : BaseController
    {
        public WorkerPositionController(AkdemicContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {

        }

        /// <summary>
        /// Vista donde se gestionan los cargos
        /// </summary>
        /// <returns>Vista principal del sistema</returns>
        [HttpGet]
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene el listado de cargos
        /// </summary>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de cargos</returns>
        [HttpGet("listar")]
        public async Task<IActionResult> GetApplicationWorkerPositions(string search)
        {
            try
            {
                var query = _context.WorkerPositions
                    .Where(x => (string.IsNullOrWhiteSpace(search) || x.Description.Contains(search))).AsQueryable();

                var filterRecords = await query.CountAsync();

                var sortOrder = GetDataTableSortOrder();
                var sortField = GetDataTableSortField();

                switch (sortField)
                {
                    case "0":
                        query = sortOrder.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Description) : query.OrderBy(q => q.Description);
                        break;
                    default:
                        query = sortOrder.Equals(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION) ? query.OrderByDescending(q => q.Description) : query.OrderBy(q => q.Description);
                        break;
                }


                var currentNumber = GetDataTableCurrentNumber();
                var recordsPerPage = GetDataTableRecordsPerPage();

                var pageList = await query.Skip(currentNumber).Take(recordsPerPage)
                    .Select(x => new
                    {
                        id = x.Id,
                        description = x.Description,
                        age = x.Age,
                        category = x.Category
                    }).ToListAsync();

                var result = GetDataTablePaginationObject(filterRecords, pageList);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("ERROR EN EL CATCH"); //TODO: Cambien esto porfa
            }
        }

        /// <summary>
        /// Vista creación de cargos
        /// </summary>
        /// <returns>Vista</returns>
        [HttpGet("crear")]
        public IActionResult Add() => View();

        /// <summary>
        /// Método para crear un cargo
        /// </summary>
        /// <param name="viewModel">Objeto que contiene los datos del nuevo cargo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("crear")]
        public async Task<IActionResult> AddPost(WorkerPositionViewModel viewModel)
        {
            var workerPosition = new WorkerPosition
            {
                Description = viewModel.Description,
                Age = viewModel.Age,
                Category = viewModel.Category,
                Dedication = viewModel.Dedication,
                AcademicDegree = viewModel.AcademicDegree,
                JobTitle = viewModel.JobTitle,
                Document = viewModel.Document
            };

            await _context.WorkerPositions.AddAsync(workerPosition);
            await _context.SaveChangesAsync();

            SuccessToastMessage("Se agrego correctamente el cargo");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Vista para editar el cargo
        /// </summary>
        /// <param name="id">Identificador del cargo</param>
        /// <returns>Vista edición</returns>
        [HttpGet("{id}/editar")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var model = await _context.WorkerPositions
                    .Select(x => new WorkerPositionViewModel
                    {
                        Id = x.Id,
                        Description = x.Description,
                        Age = x.Age,
                        Category = x.Category,
                        Dedication = x.Dedication,
                        AcademicDegree = x.AcademicDegree,
                        JobTitle = x.JobTitle,
                        Document = x.Document
                    }).FirstOrDefaultAsync(x => x.Id == id);

                if (model == null)
                {
                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Método para editar el cargo
        /// </summary>
        /// <param name="viewModel">Objeto que contiene los datos actualizados del cargo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar")]
        public async Task<IActionResult> EditPost(WorkerPositionViewModel viewModel)
        {
            try
            {
                var workerPosition = await _context.WorkerPositions.FirstOrDefaultAsync(x => x.Id == viewModel.Id);

                if (workerPosition == null)
                {
                    return RedirectToAction("Index");
                }
                workerPosition.Age = viewModel.Age;
                workerPosition.Category = viewModel.Category;
                workerPosition.Dedication = viewModel.Dedication;
                workerPosition.AcademicDegree = viewModel.AcademicDegree;
                workerPosition.JobTitle = viewModel.JobTitle;
                workerPosition.Document = viewModel.Document;

                await SaveChangesAsync();
                SuccessToastMessage("Se guardo correctamente los datos");
                return Redirect("/admin/cargos");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Método para eliminar el cargo
        /// </summary>
        /// <param name="id">Identificador del cargo</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("{id}/eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var entity = await _context.WorkerPositions.FindAsync(id);
                _context.WorkerPositions.Remove(entity);
                await SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Obtiene el detalle del cargo
        /// </summary>
        /// <param name="id">Identificador del cargo</param>
        /// <returns>Objeto que contiene los datos del cargo</returns>
        [HttpGet("{id}/detalle")]
        public async Task<IActionResult> Detail(Guid id)
        {
            try
            {
                var entity = await _context.WorkerPositions.FindAsync(id);
                var model = new WorkerPositionViewModel()
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    Category = entity.Category,
                    JobTitle = entity.JobTitle,
                    Document = entity.Document,
                    Dedication = entity.Dedication,
                    Age = entity.Age,
                    AcademicDegree = entity.AcademicDegree
                };
                return View(model);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
