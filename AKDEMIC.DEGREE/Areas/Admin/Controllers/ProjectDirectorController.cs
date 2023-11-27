using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DEGREE.Areas.Admin.Models.ProjectDirectorViewModels;
using AKDEMIC.DEGREE.Controllers;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.DEGREE.Areas.Admin.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.DEGREE_ADMIN)]
    [Area("Admin")]
    [Route("admin/director-proyecto")]
    public class ProjectDirectorController : BaseController
    {
        private readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;

        public ProjectDirectorController(
            AkdemicContext context,
            IDataTablesService dataTablesService
            )
        {
            _context = context;
            _dataTablesService = dataTablesService;
        }

        public IActionResult Index()
            => View();

        [HttpGet("get-datatable")]
        public async Task<IActionResult> GetDatatable(string search)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var query = _context.DegreeProjectDirectors.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Surnames,
                    x.IdentificationCard,
                    x.PhoneNumber,
                    x.Sex,
                    x.CountryId,
                    x.DepartmentId,
                    x.CivilStatus,
                    x.Address,
                    x.Email
                })
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count();

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return Ok(result);
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> Add(ProjectDirectorViewModel model)
        {
            var country = await _context.Countries.Where(x => x.Code == "PE").FirstOrDefaultAsync();

            var entity = new DegreeProjectDirector
            {
                Name = model.Name,
                Surnames = model.Surnames,
                IdentificationCard = model.IdentificationCard,
                PhoneNumber = model.PhoneNumber,
                Sex = model.Sex,
                CountryId = country.Id,
                DepartmentId = model.DepartmentId,
                CivilStatus = model.CivilStatus,
                Address = model.Address,
                Email = model.Email
            };

            await _context.DegreeProjectDirectors.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("editar")]
        public async Task<IActionResult> Edit(ProjectDirectorViewModel model)
        {
            var entity = await _context.DegreeProjectDirectors.Where(x => x.Id == model.Id.Value).FirstOrDefaultAsync();
            entity.Name = model.Name;
            entity.Surnames = model.Surnames;
            entity.IdentificationCard = model.IdentificationCard;
            entity.PhoneNumber = model.PhoneNumber;
            entity.Sex = model.Sex;
            entity.DepartmentId = model.DepartmentId;
            entity.CivilStatus = model.CivilStatus;
            entity.Address = model.Address;
            entity.Email = model.Email;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _context.DegreeProjectDirectors.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.DegreeProjectDirectors.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
