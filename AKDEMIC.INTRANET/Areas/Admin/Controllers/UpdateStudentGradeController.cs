using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.UpdateStudentGradeViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles =
     ConstantHelpers.ROLES.SUPERADMIN
    )]
    [Area("Admin")]
    [Route("admin/actualizar-nota-estudiante")]
    public class UpdateStudentGradeController : BaseController
    {
        private readonly AkdemicContext _context;
        private readonly IConfigurationService _configurationService;
        private readonly IActionContextAccessor _accessor;
        private readonly IAcademicSummariesService _academicSummariesService;
        private readonly IStudentSectionService _studentSectionService;
        private readonly IDataTablesService _dataTablesService;
        private readonly ICloudStorageService _cloudStorageService;

        public UpdateStudentGradeController(
            AkdemicContext context,
            IActionContextAccessor accessor,
            IAcademicSummariesService academicSummariesService,
            IStudentSectionService studentSectionService,
            IDataTablesService dataTablesService,
            ICloudStorageService cloudStorageService,
            IConfigurationService configurationService
            )
        {
            _context = context;
            _configurationService = configurationService;
            _accessor = accessor;
            _academicSummariesService = academicSummariesService;
            _studentSectionService = studentSectionService;
            _dataTablesService = dataTablesService;
            _cloudStorageService = cloudStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var confi_ip_address = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IP_ADDRESS_UPDATE_STUDENT_GRADE);

            if (string.IsNullOrEmpty(confi_ip_address))
            {
                ErrorToastMessage("No se configuró la dirección IP.");
                return RedirectToAction("Index", "Home");
            }

            var iPAddress = GetRequestIP();

            if (iPAddress.Trim() != confi_ip_address.Trim())
            {
                ErrorToastMessage($"{iPAddress} no tiene acceso a esta vista.");
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet("get-courses-datatable")]
        public async Task<IActionResult> CourseStudentDatatable(Guid termId, Guid studentId, byte type)
        {
            var parameters = _dataTablesService.GetSentParameters();

            var query = _context.AcademicHistories.Where(x => x.TermId == termId && x.StudentId == studentId && x.Type == type && !x.Validated).AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    CourseCode = x.Course.Code,
                    CourseName = x.Course.Name,
                    Section = x.Section.Code,
                    isRegular = x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR,
                    Grade = x.Grade
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return Ok(new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            });
        }

        [HttpGet("get-notas-datatable")]
        public async Task<IActionResult> GradesDatatable(Guid academicHistoryId)
        {
            var parameters = _dataTablesService.GetSentParameters();
            var academicHistory = await _context.AcademicHistories.Where(x => x.Id == academicHistoryId).FirstOrDefaultAsync();

            var query = _context.Grades.Where(x => x.StudentSection.SectionId == academicHistory.SectionId && x.StudentSection.StudentId == academicHistory.StudentId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.Evaluation.CourseUnit.Number)
                .ThenBy(x => x.Evaluation.Week)
               .Skip(parameters.PagingFirstRecord)
               .Take(parameters.RecordsPerDraw)
               .Select(x => new
               {
                   x.StudentSection.Student.User.FullName,
                   x.StudentSection.Student.User.UserName,
                   x.Id,
                   x.Evaluation.Name,
                   x.Value
               })
               .ToListAsync();

            int recordsTotal = data.Count;

            return Ok(new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            });
        }

        [HttpPost("actualizar-nota-estudiante")]
        public async Task<IActionResult> UpdateStudentGrade(UpdateGradeViewModel model)
        {
            var confi_ip_address = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IP_ADDRESS_UPDATE_STUDENT_GRADE);

            var iPAddress = GetRequestIP();

            if (iPAddress.Trim() != confi_ip_address.Trim())
            {
                return BadRequest("No tiene acceso a actualizar esta información.");
            }

            var grade = await _context.Grades.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            var evaluation = await _context.Evaluations.Where(x => x.Id == grade.EvaluationId).FirstOrDefaultAsync();
            var studentSection = await _context.StudentSections.Where(x => x.Id == grade.StudentSectionId)
                .Select(x => new
                {
                    x.StudentId,
                    x.FinalGrade
                })
                .FirstOrDefaultAsync();

            var entity = new StudentGradeChangeHistory
            {
                StudentId = studentSection.StudentId,
                IpAddress = iPAddress,
                AcademicHistoryId = model.AcademicHistoryId,
                Description = $"Evaluación : {evaluation.Name}, con nota {grade.Value} se actualizó a {model.Grade}."
            };

            if (model.Resolution != null)
            {
                entity.ResolutionFile = await _cloudStorageService.UploadFile(
                    model.Resolution.OpenReadStream(),
                    ConstantHelpers.CONTAINER_NAMES.STUDENT_OBSERVATION,
                    Path.GetExtension(model.Resolution.FileName)
                    );
            }

            grade.Value = model.Grade;

            await _context.StudentGradeChangeHistories.AddAsync(entity);
            await _context.SaveChangesAsync();

            await _studentSectionService.RecalculateFinalGrade(grade.StudentSectionId);
            await _academicSummariesService.ReCreateStudentAcademicSummaries(studentSection.StudentId);
            return Ok();
        }

        [HttpPost("actualizar-historial-academico")]
        public async Task<IActionResult> UpdateAcademicHistory(UpdateGradeViewModel model)
        {
            var confi_ip_address = await _configurationService.GetValueByKey(ConstantHelpers.Configuration.IntranetManagement.IP_ADDRESS_UPDATE_STUDENT_GRADE);

            var iPAddress = GetRequestIP();

            if (iPAddress.Trim() != confi_ip_address.Trim())
            {
                return BadRequest("No tiene acceso a actualizar esta información.");
            }

            var academicHistory = await _context.AcademicHistories.Where(x => x.Id == model.AcademicHistoryId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == academicHistory.TermId).FirstOrDefaultAsync();

            var entity = new StudentGradeChangeHistory
            {
                StudentId = academicHistory.StudentId,
                IpAddress = iPAddress,
                AcademicHistoryId = model.AcademicHistoryId,
                Description = $"Curso con nota {academicHistory.Grade} se actualizó a {model.Grade}."
            };

            if (model.Resolution != null)
            {
                entity.ResolutionFile = await _cloudStorageService.UploadFile(
                    model.Resolution.OpenReadStream(),
                    ConstantHelpers.CONTAINER_NAMES.STUDENT_OBSERVATION,
                    Path.GetExtension(model.Resolution.FileName)
                    );
            }

            academicHistory.Grade = (int)model.Grade;
            academicHistory.Approved = term.MinGrade <= academicHistory.Grade;

            if (academicHistory.Type == ConstantHelpers.AcademicHistory.Types.DEFERRED)
            {
                var deferredExamStudent = await _context.DeferredExamStudents
                    .Where(x => x.StudentId == academicHistory.StudentId && x.DeferredExam.Section.CourseTerm.CourseId == academicHistory.CourseId && x.DeferredExam.Section.CourseTerm.TermId == academicHistory.TermId)
                    .FirstOrDefaultAsync();

                if (deferredExamStudent != null)
                {
                    deferredExamStudent.Grade = (int)model.Grade;
                }
            }

            await _context.SaveChangesAsync();
            await _academicSummariesService.ReCreateStudentAcademicSummaries(academicHistory.StudentId);
            return Ok();
        }

        private string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
            {
                var csvList = GetHeaderValueAs<string>("X-Forwarded-For");
                ip = SplitCsv(csvList).FirstOrDefault();
            }

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && _accessor.ActionContext.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _accessor.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (string.IsNullOrWhiteSpace(ip))
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        private T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (_accessor.ActionContext.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        private static List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
    }
}
