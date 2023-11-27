using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AKDEMIC.DEGREE.Controllers
{
    [AllowAnonymous]
    public class JsonController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly AkdemicContext _context;

        public JsonController(IOptions<CloudStorageCredentials> storageCredentials, AkdemicContext context)
        {
            _storageCredentials = storageCredentials;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("departamentos/get")]
        public async Task<IActionResult> GetDepartmetns()
        {
            var country = await _context.Countries.Where(x => x.Code == "PE").FirstOrDefaultAsync();
            var departments = await _context.Departments.Where(x => x.Country.Code == "PE")
                .Select(x => new
                {
                    x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return Ok(departments);
        }

        /// <summary>
        /// Método para descargar archivos
        /// </summary>
        /// <param name="path">Ruta del archivo</param>
        /// <returns>Retorna un Archivo</returns>
        [AllowAnonymous]
        [HttpGet("archivos/{*path}")]
        public async Task DownloadImage(string path)
        {
            using (var mem = new MemoryStream())
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDownload(mem, "", path);
                // Download file
                var fileName = Path.GetFileName(path);
                var text = $"inline;filename=\"{fileName.Normalize().Replace(' ', '_')}\"";
                HttpContext.Response.Headers["Content-Disposition"] = text;
                mem.Position = 0;
                mem.CopyTo(HttpContext.Response.Body);
            }
        }

        /// <summary>
        /// Méotodo para generar los Procedures relacionados a grados
        /// </summary>
        /// <returns>Retorna un Ok o BadRequest</returns>
        [AllowAnonymous]
        [HttpGet("generar-procedures")]
        public async Task<IActionResult> GenerateProcedures()
        {
            //var studentRoleId = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.STUDENTS).Select(x => x.Id).FirstOrDefaultAsync();
            //var procedures = new List<Procedure>
            //{
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.STUDYRECORD], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.STUDYRECORD, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.PROOFONINCOME], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.PROOFONINCOME,ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.ENROLLMENT], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.ENROLLMENT, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.REGULARSTUDIES], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.REGULARSTUDIES, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.EGRESS], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.EGRESS, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.MERITCHART], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.MERITCHART, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.UPPERFIFTH], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.UPPERFIFTH, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.UPPERTHIRD], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.UPPERTHIRD, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.ACADEMICRECORD], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.ACADEMICRECORD, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.ACADEMICPERFORMANCESUMMARY], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.ACADEMICPERFORMANCESUMMARY, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.JOBTITLE], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.JOBTITLE, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.CERTIFICATEOFSTUDIES], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.CERTIFICATEOFSTUDIES, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //};

            //await _context.Procedures.AddRangeAsync(procedures);
            //await _context.SaveChangesAsync();
            //var studentRoleId = await _context.Roles.Where(x => x.Name == ConstantHelpers.ROLES.STUDENTS).Select(x => x.Id).FirstOrDefaultAsync();
            //var procedures = new List<Procedure>
            //{
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR_DEGREE_APPLICATION], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.BACHELOR_DEGREE_APPLICATION, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //    new Procedure { Name = ConstantHelpers.PROCEDURES.STATIC_TYPE.VALUES[ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION], Duration = 1, Score = 1, LegalBase = "", StaticType = ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION, ProcedureRoles = new List<ProcedureRole>{ new ProcedureRole { RoleId = studentRoleId} } },
            //};
            //await _context.Procedures.AddRangeAsync(procedures);


            var procedure = await _context.Procedures.Where(x => x.StaticType == ConstantHelpers.PROCEDURES.STATIC_TYPE.TITLE_DEGREE_APPLICATION).FirstOrDefaultAsync();
            var aspnetuser = await _context.Users.Where(x => x.Dni == "71787720").FirstOrDefaultAsync();
            var student = await _context.Students.Where(x => x.UserId == aspnetuser.Id).FirstOrDefaultAsync();
            var currentTerm = await _context.Terms
             .OrderByDescending(x => x.Year)
             .ThenByDescending(x => x.Number)
             .FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            var userProcedure = new UserProcedure()
            {
                UserId = student.UserId,
                ProcedureId = procedure.Id,
                TermId = currentTerm.Id,
                DNI = student.User.Dni,
                Status = ConstantHelpers.USER_PROCEDURES.STATUS.PENDING
            };
            await _context.UserProcedures.AddAsync(userProcedure);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
