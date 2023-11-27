using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.INTRANET.Areas.Admin.Models.InstitutionalWelfareViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.INTRANET.Helpers;
using AKDEMIC.CORE.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AKDEMIC.CORE.Services;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.CORE.Structs;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Implementations;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.ROLES.SUPERADMIN + "," + ConstantHelpers.ROLES.INTRANET_ADMIN + "," + ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE + "," + ConstantHelpers.ROLES.GENERAL_ACADEMIC_DIRECTORATE)]
    [Area("Admin")]
    [Route("admin/bienestar_institucional")]
    public class InstitutionalWelfareController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConverter _dinkConverter;
        private readonly IViewRenderService _viewRenderService;

        public InstitutionalWelfareController(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostingEnvironment,
            IConverter dinkConverter,
            IViewRenderService viewRenderService,
            ITermService termService,
            IDataTablesService dataTablesService,
            IStudentService studentService) : base(context, userManager, termService, dataTablesService)
        {
            _studentService = studentService;
            _hostingEnvironment = hostingEnvironment;
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
        }

        /// <summary>
        /// Obtiene la vista inicial de bienestar institucional
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Obtiene los estudiantes segun el filtro
        /// </summary>
        /// <param name="searchValue">Texto de busqueda</param>
        /// <returns>Retorna un objeto con la estructura de la tabla</returns>
        [HttpGet("students-datatable")]
        public async Task<IActionResult> GetStudents(Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? admissionTypeId = null, string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _studentService.GetInstitutionalWelfareStudentDatatable(sentParameters, termId, facultyId, careerId, admissionTypeId, searchValue);
            return Ok(result);
        }

        [HttpGet("constancia/{studentId}/periodo/{termId}")]
        public async Task<IActionResult> PrintSocioEconomicRecord(Guid studentId, Guid termId)
        {

            var data = await _studentService.GetStudentConstancy(studentId, termId);

            if (data == null)
                return BadRequest("No se ha encontrado una constancia");

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Views/InstitutionalWelfare/PDF/SocioEconomicConstancy.cshtml", data);

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 0, Left = 10, Right = 10 }
            };

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8"},
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            HttpContext.Response.Headers["Content-Disposition"] = $"attachment; filename= {data.UserName}-constancia.pdf";
            return File(fileByte, "application/octet-stream");
        }

        [HttpGet("reporte-excel/get")]
        public async Task<IActionResult> GetExcelReport(Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? admissionTypeId = null, string searchValue = null)
        {
            var term = await _context.Terms
                .Where(x => x.Id == termId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            if (term == null)
                return BadRequest("Sucedio un error");

            var query = _context.Students
                            .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                            .AsNoTracking();

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (admissionTypeId != null)
                query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                var trimSearch = searchValue.Trim();
                query = query.Where(x => x.User.UserName.ToUpper().Contains(trimSearch.ToUpper())
                        || x.User.FullName.ToUpper().Contains(trimSearch.ToUpper())
                        || x.User.PaternalSurname.ToUpper().Contains(trimSearch.ToUpper())
                        || x.User.MaternalSurname.ToUpper().Contains(trimSearch.ToUpper())
                        || x.User.Name.ToUpper().Contains(trimSearch.ToUpper()));
            }


            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    names = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    maternalSurname = x.User.MaternalSurname,
                    userName = x.User.UserName,
                    fullname = x.User.FullName,
                    email = x.User.Email,
                    faculty = x.Career.Faculty.Name,
                    career = x.Career.Name,
                    phoneNumber = x.User.PhoneNumber,
                    hasStudentInformationTerm = x.StudentInformations.Any(y => y.TermId == termId && y.StudentId == x.Id),
                    hasOldStudentInformationTerm = x.StudentInformations.Any(y => y.TermId != termId && y.StudentId == x.Id),
                }).ToListAsync();

            var dt = new DataTable
            {
                TableName = $"ReporteDeFicha-{term.Name}"
            };

            dt.Columns.Add("CodigoEstudiante");
            dt.Columns.Add("Paterno");
            dt.Columns.Add("Materno");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Correo");
            dt.Columns.Add("Escuela Profesional");
            dt.Columns.Add("Periodo Académico");
            dt.Columns.Add("Lleno la ficha este periodo");
            dt.Columns.Add("Lleno la ficha otro periodo");

            foreach (var item in data)
            {
                dt.Rows.Add(item.userName, item.paternalSurname, item.maternalSurname, item.names, item.email, item.career, term.Name, item.hasStudentInformationTerm ? "SI" : "NO", item.hasOldStudentInformationTerm ? "SI" : "NO");
            }


            dt.AcceptChanges();

            var fileName = $"ReporteMatriculadosFichas-{term.Name}.xlsx";
            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                var ws = wb.Worksheet(dt.TableName);

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("ficha-estudiante/{studentId}/periodo/{termId}")]
        public async Task<IActionResult> StudentInformationTerm(Guid studentId, Guid termId)
        {
            var student = await _studentService.Get(studentId);
            var term = await _termService.Get(termId);
            if (student == null || term == null)
                return RedirectToAction("Index");

            var model = new StudentInformationTermViewModel
            {
                StudentId = student.Id,
                TermId = term.Id
            };

            var studentInformation = await _context.StudentInformations
                .Where(x => x.StudentId == studentId && x.TermId == termId)
                .Select(x => new
                {

                    OriginDepartmentId = x.OriginDistrictId == null ? null : (Guid?)x.OriginDistrict.Province.DepartmentId,
                    OriginProvinceId = x.OriginDistrictId == null ? null : (Guid?)x.OriginDistrict.ProvinceId,
                    x.OriginDistrictId,
                    CurrentDepartmentId = x.Student.User.DepartmentId,
                    CurrentProvinceId = x.Student.User.ProvinceId,
                    CurrentDistrictId = x.Student.User.DistrictId
                })
                .FirstOrDefaultAsync();

            if (studentInformation != null)
            {
                model.OriginDistrictIdDefault = studentInformation.OriginDistrictId;
                model.OriginProvinceIdDefault = studentInformation.OriginProvinceId;
                model.OriginDepartmentIdDefault = studentInformation.OriginDepartmentId;

                model.CurrentDepartmentIdDefault = studentInformation.CurrentDepartmentId;
                model.CurrentProvinceIdDefault = studentInformation.CurrentProvinceId;
                model.CurrentDistrictIdDefault = studentInformation.CurrentDistrictId;
            }

            return View(model);
        }

        #region FichaEstudiante

        [HttpGet("ficha-estudiante/datos-personales/{studentId}/periodo/{termId}")]
        public async Task<IActionResult> PersonalInformationGet(Guid studentId, Guid termId)
        {
            var model = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new PersonalInformationViewModel
                {
                    StudentId = x.Id,
                    UserName = x.User.UserName,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    Dni = x.User.Dni,
                    CareerName = x.Career.Name,
                    FacultyName = x.Career.Faculty.Name,
                    BirthDate = x.User.BirthDate.ToLocalDateFormat(),
                    Age = DateTime.UtcNow.Year - x.User.BirthDate.Year,
                    Email = x.User.Email,
                    Sex = x.User.Sex,
                    CivilStatus = x.User.CivilStatus,
                    CurrentAcademicYear = x.CurrentAcademicYear,
                    CurrentAddress = x.User.Address,
                    CurrentPhoneNumber = x.User.PhoneNumber,
                })
                .FirstOrDefaultAsync();

            if (model == null)
                return BadRequest("No se pudo cargar la data del estudiante");

            var term = await _context.Terms
                .Where(x => x.Id == termId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            if (term == null)
                return BadRequest("El periodo indicado no existe");

            var studentInformation = await _context.StudentInformations
                .Where(x => x.StudentId == studentId && x.TermId == termId)
                .Select(x => new
                {
                    x.OriginAddress,
                    x.OriginPhoneNumber,
                    x.FullNameExternalPerson,
                    x.AddressExternalPerson,
                    x.EmailExternalPerson,
                    x.PhoneExternalPerson
                })
                .FirstOrDefaultAsync();

            model.TermId = term.Id;
            model.TermName = term.Name;

            if (studentInformation != null)
            {
                model.OriginAddress = studentInformation.OriginAddress;
                model.OriginPhoneNumber = studentInformation.OriginPhoneNumber;

                model.FullNameExternalPerson = studentInformation.FullNameExternalPerson;
                model.AddressExternalPerson = studentInformation.AddressExternalPerson;
                model.EmailExternalPerson = studentInformation.EmailExternalPerson;
                model.PhoneExternalPerson = studentInformation.PhoneExternalPerson;
            }

            return Ok(model);
        }

        /// <summary>
        /// Guarda los datos personales de la ficha del estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene la informacion para modificar de la ficha del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("ficha-estudiante/datos-personales/post")]
        public async Task<IActionResult> PersonalInformationSave(PersonalInformationViewModel model)
        {
            var studentInformation = await _context.StudentInformations
                .Where(x => x.StudentId == model.StudentId && x.TermId == model.TermId)
                .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == model.TermId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            var student = await _context.Students
                .Include(x => x.User)
                .Where(x => x.Id == model.StudentId)
                .FirstOrDefaultAsync();

            if (student == null || term == null)
                return BadRequest("Sucedio un error");

            if (studentInformation == null)
            {
                studentInformation = new StudentInformation
                {
                    TermId = model.TermId,
                    StudentId = model.StudentId
                };
                await _context.StudentInformations.AddAsync(studentInformation);
            }

            student.User.Sex = model.Sex;
            student.User.CivilStatus = (byte)model.CivilStatus;
            student.User.DistrictId = model.CurrentDistrictId;
            student.User.ProvinceId = model.CurrentProvinceId;
            student.User.DepartmentId = model.CurrentDepartmentId;
            student.User.Address = model.CurrentAddress;
            student.User.PhoneNumber = model.CurrentPhoneNumber;



            studentInformation.CurrentPhoneNumber = model.CurrentPhoneNumber;
            studentInformation.OriginDistrictId = model.OriginDistrictId;
            studentInformation.OriginAddress = model.OriginAddress;
            studentInformation.OriginPhoneNumber = model.OriginPhoneNumber;
            studentInformation.FullNameExternalPerson = model.FullNameExternalPerson;
            studentInformation.AddressExternalPerson = model.AddressExternalPerson;
            studentInformation.EmailExternalPerson = model.EmailExternalPerson;
            studentInformation.PhoneExternalPerson = model.PhoneExternalPerson;

            await _context.SaveChangesAsync();
            return Ok();

        }


        [HttpGet("ficha-estudiante/antecedentes-academicos/{studentId}/periodo/{termId}")]
        public async Task<IActionResult> AcademicBackgroundGet(Guid studentId, Guid termId)
        {
            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.CareerId
                })
                .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == termId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            if (term == null || student == null)
                return BadRequest("Sucedio un error");

            var studentInformation = await _context.StudentInformations
                .Where(x => x.StudentId == studentId && x.TermId == termId)
                .Select(x => new
                {
                    x.OriginSchool,
                    x.OriginSchoolPlace,
                    x.SchoolType,
                    x.UniversityPreparationId,
                })
                .FirstOrDefaultAsync();
            var model = new AcademicBackgroundViewModel();

            if (studentInformation != null)
            {
                model.OriginSchool = studentInformation.OriginSchool;
                model.OriginSchoolPlace = studentInformation.OriginSchoolPlace;

                model.SchoolType = studentInformation.SchoolType;
                model.UniversityPreparationId = studentInformation.UniversityPreparationId;
            }

            return Ok(model);
        }

        /// <summary>
        /// Guarda los antecedentes academicos en la ficha del estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene los antecedentes academicos del estudiante a editar</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("ficha-estudiante/antecedentes-academicos/post")]
        public async Task<IActionResult> AcademicBackgroundSave(AcademicBackgroundViewModel model)
        {
            var studentInformation = await _context.StudentInformations
                .Where(x => x.StudentId == model.StudentId && x.TermId == model.TermId)
                .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == model.TermId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            var student = await _context.Students
                .Where(x => x.Id == model.StudentId)
                .FirstOrDefaultAsync();

            if (student == null || term == null)
                return BadRequest("Sucedio un error");

            if (studentInformation == null)
            {
                studentInformation = new StudentInformation
                {
                    TermId = model.TermId,
                    StudentId = model.StudentId
                };
                await _context.StudentInformations.AddAsync(studentInformation);
            }


            studentInformation.OriginSchool = model.OriginSchool;
            studentInformation.OriginSchoolPlace = model.OriginSchoolPlace;
            studentInformation.SchoolType = (byte)model.SchoolType;
            studentInformation.UniversityPreparationId = model.UniversityPreparationId;


            await _context.SaveChangesAsync();

            return Ok();

        }

        [HttpGet("ficha-estudiante/economia/{studentId}/periodo/{termId}")]
        public async Task<IActionResult> EconomyGet(Guid studentId, Guid termId)
        {
            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.CareerId
                })
                .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == termId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            if (term == null || student == null)
                return BadRequest("Sucedio un error");

            var studentInformation = await _context.StudentInformations
                .Where(x => x.StudentId == studentId && x.TermId == termId)
                .Select(x => new
                {
                    x.PrincipalPerson,
                    x.EconomicMethodFatherTutor,
                    x.DSectorFatherTutor,
                    x.DWorkConditionFatherTutor,
                    x.DEspecificActivityFatherTutor,
                    x.DBusyFatherTutor,
                    x.ISectorFatherTutor,
                    x.IWorkConditionFatherTutor,
                    x.IEspecificActivityFatherTutor,
                    x.EconomicMethodMother,
                    x.DSectorMother,
                    x.DWorkConditionMother,
                    x.DEspecificActivityMother,
                    x.DBusyMother,
                    x.ISectorMother,
                    x.IWorkConditionMother,
                    x.IEspecificActivityMother,
                    x.EconomicExpensesFeeding,
                    x.EconomicExpensesBasicServices,
                    x.EconomicExpensesEducation,
                    x.EconomicExpensesOthers,
                    x.FatherRemuneration,
                    x.MotherRemuneration,
                    x.StudentRemuneration,
                    x.OtherRemuneration,
                    x.TotalRemuneration,
                    x.StudentDependency,
                    x.StudentCoexistence,
                    x.FamilyRisk,
                    x.StudentWorkDedication,
                    x.StudentWorkDescription,
                    x.StudentWorkCondition,
                    x.AuthorizeCheck,
                    x.AuthorizedPersonFullName,
                    x.AuthorizedPersonAddress,
                    x.AuthorizedPersonPhone,
                })
                .FirstOrDefaultAsync();
            var model = new EconomyViewModel();

            if (studentInformation != null)
            {
                model.PrincipalPerson = studentInformation.PrincipalPerson;
                model.EconomicMethodFatherTutor = studentInformation.EconomicMethodFatherTutor;
                model.DSectorFatherTutor = studentInformation.DSectorFatherTutor;
                model.DWorkConditionFatherTutor = studentInformation.DWorkConditionFatherTutor;
                model.DEspecificActivityFatherTutor = studentInformation.DEspecificActivityFatherTutor;
                model.DBusyFatherTutor = studentInformation.DBusyFatherTutor;
                model.ISectorFatherTutor = studentInformation.ISectorFatherTutor;
                model.IWorkConditionFatherTutor = studentInformation.IWorkConditionFatherTutor;
                model.IEspecificActivityFatherTutor = studentInformation.IEspecificActivityFatherTutor;
                model.EconomicMethodMother = studentInformation.EconomicMethodMother;
                model.DSectorMother = studentInformation.DSectorMother;
                model.DWorkConditionMother = studentInformation.DWorkConditionMother;
                model.DEspecificActivityMother = studentInformation.DEspecificActivityMother;
                model.DBusyMother = studentInformation.DBusyMother;
                model.ISectorMother = studentInformation.ISectorMother;
                model.IWorkConditionMother = studentInformation.IWorkConditionMother;
                model.IEspecificActivityMother = studentInformation.IEspecificActivityMother;
                model.EconomicExpensesFeeding = studentInformation.EconomicExpensesFeeding;
                model.EconomicExpensesBasicServices = studentInformation.EconomicExpensesBasicServices;
                model.EconomicExpensesEducation = studentInformation.EconomicExpensesEducation;
                model.EconomicExpensesOthers = studentInformation.EconomicExpensesOthers;
                model.FatherRemuneration = studentInformation.FatherRemuneration;
                model.MotherRemuneration = studentInformation.MotherRemuneration;
                model.StudentRemuneration = studentInformation.StudentRemuneration;
                model.OtherRemuneration = studentInformation.OtherRemuneration;
                model.TotalRemuneration = studentInformation.TotalRemuneration;
                model.StudentDependency = studentInformation.StudentDependency;
                model.StudentCoexistence = studentInformation.StudentCoexistence;
                model.FamilyRisk = studentInformation.FamilyRisk;
                model.StudentWorkDedication = studentInformation.StudentWorkDedication;
                model.StudentWorkDescription = studentInformation.StudentWorkDescription;
                model.StudentWorkCondition = studentInformation.StudentWorkCondition;
                model.AuthorizeCheck = studentInformation.AuthorizeCheck;
                model.AuthorizedPersonFullName = studentInformation.AuthorizedPersonFullName;
                model.AuthorizedPersonAddress = studentInformation.AuthorizedPersonAddress;
                model.AuthorizedPersonPhone = studentInformation.AuthorizedPersonPhone;
            }

            return Ok(model);
        }

        /// <summary>
        /// Guarda la parte economica de la ficha del estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene la ficha del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("ficha-estudiante/economia/post")]
        public async Task<IActionResult> EconomySave(EconomyViewModel model)
        {
            var studentInformation = await _context.StudentInformations
                            .Where(x => x.StudentId == model.StudentId && x.TermId == model.TermId)
                            .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == model.TermId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            var student = await _context.Students
                .Where(x => x.Id == model.StudentId)
                .FirstOrDefaultAsync();

            if (student == null || term == null)
                return BadRequest("Sucedio un error");

            if (studentInformation == null)
            {
                studentInformation = new StudentInformation
                {
                    TermId = model.TermId,
                    StudentId = model.StudentId
                };
                await _context.StudentInformations.AddAsync(studentInformation);
            }

            studentInformation.PrincipalPerson = model.PrincipalPerson;
            studentInformation.EconomicMethodFatherTutor = model.EconomicMethodFatherTutor;
            studentInformation.DSectorFatherTutor = model.DSectorFatherTutor;
            studentInformation.DWorkConditionFatherTutor = model.DWorkConditionFatherTutor;
            studentInformation.DEspecificActivityFatherTutor = model.DEspecificActivityFatherTutor;
            studentInformation.DBusyFatherTutor = model.DBusyFatherTutor;
            studentInformation.ISectorFatherTutor = model.ISectorFatherTutor;
            studentInformation.IWorkConditionFatherTutor = model.IWorkConditionFatherTutor;
            studentInformation.IEspecificActivityFatherTutor = model.IEspecificActivityFatherTutor;
            studentInformation.EconomicMethodMother = model.EconomicMethodMother;
            studentInformation.DSectorMother = model.DSectorMother;
            studentInformation.DWorkConditionMother = model.DWorkConditionMother;
            studentInformation.DEspecificActivityMother = model.DEspecificActivityMother;
            studentInformation.DBusyMother = model.DBusyMother;
            studentInformation.ISectorMother = model.ISectorMother;
            studentInformation.IWorkConditionMother = model.IWorkConditionMother;
            studentInformation.IEspecificActivityMother = model.IEspecificActivityMother;
            studentInformation.EconomicExpensesFeeding = model.EconomicExpensesFeeding;
            studentInformation.EconomicExpensesBasicServices = model.EconomicExpensesBasicServices;
            studentInformation.EconomicExpensesEducation = model.EconomicExpensesEducation;
            studentInformation.EconomicExpensesOthers = model.EconomicExpensesOthers;
            studentInformation.FatherRemuneration = model.FatherRemuneration;
            studentInformation.MotherRemuneration = model.MotherRemuneration;
            studentInformation.StudentRemuneration = model.StudentRemuneration;
            studentInformation.OtherRemuneration = model.OtherRemuneration;
            studentInformation.TotalRemuneration = model.TotalRemuneration;
            studentInformation.StudentDependency = model.StudentDependency;
            studentInformation.StudentCoexistence = model.StudentCoexistence;
            studentInformation.FamilyRisk = model.FamilyRisk;
            studentInformation.StudentWorkDedication = model.StudentWorkDedication;
            studentInformation.StudentWorkDescription = model.StudentWorkDescription;
            studentInformation.StudentWorkCondition = model.StudentWorkCondition;
            studentInformation.AuthorizeCheck = model.AuthorizeCheck;
            studentInformation.AuthorizedPersonFullName = model.AuthorizedPersonFullName;
            studentInformation.AuthorizedPersonAddress = model.AuthorizedPersonAddress;
            studentInformation.AuthorizedPersonPhone = model.AuthorizedPersonPhone;
            await _context.SaveChangesAsync();
            return Ok();

        }


        /// <summary>
        /// Obtiene la informacion familiar del estudiante
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("familiar/{studentId}/datatable")]
        public async Task<IActionResult> GetStudentFamilyDatatable(Guid studentId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            var data = await _context.StudentFamilies
                .Where(x => x.StudentId == studentId)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    paternalName = x.PaternalName,
                    maternalName = x.MaternalName,
                    birthday = x.Birthday.ToLocalDateFormat(),
                    relationship = ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.TYPE.ContainsKey(x.RelationshipInt) ?
                        ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.TYPE[x.RelationshipInt] : "",
                    civilStatus = ConstantHelpers.CIVIL_STATUS.VALUES.ContainsKey(x.CivilStatusInt) ?
                        ConstantHelpers.CIVIL_STATUS.VALUES[x.CivilStatusInt] : "",
                    degreeInstruction = ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.TYPE.ContainsKey(x.DegreeInstructionInt) ?
                        ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.TYPE[x.DegreeInstructionInt] : "",
                    certificated = x.Certificated,
                    occupation = x.Occupation,
                    workcenter = x.WorkCenter,
                    location = x.Location,
                }).ToListAsync();

            return Ok(new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            });
        }

        /// <summary>
        /// Obtiene la informacion familiar del estudiante
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("familiar-salud/{studentId}/datatable")]
        public async Task<IActionResult> GetStudentFamilyHealthDatatable(Guid studentId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            var data = await _context.StudentFamilies
                .Where(x => x.StudentId == studentId)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    paternalName = x.PaternalName,
                    maternalName = x.MaternalName,
                    relationship = ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.TYPE.ContainsKey(x.RelationshipInt) ?
                        ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.TYPE[x.RelationshipInt] : "",
                    x.IsSick,
                    x.DiseaseType,
                    x.SurgicalIntervention
                }).ToListAsync();

            return Ok(new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = data.Count,
                RecordsTotal = data.Count
            });
        }

        /// <summary>
        /// Agrega una informacion familiar a un estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene la informacion familiar del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("agregar/familiar")]
        public async Task<IActionResult> AddStudentFamily(StudentFamilyViewModel model)
        {
            var student = await _context.Students.Where(x => x.Id == model.StudentId).FirstOrDefaultAsync();

            if (!ModelState.IsValid) return BadRequest("Revise el formulario");

            if (student == null)
                return BadRequest("Sucedio un error");

            var studentFamily = new StudentFamily
            {
                StudentId = student.Id,
                Name = model.Name,
                PaternalName = model.PaternalName,
                MaternalName = model.MaternalName,
                Birthday = ConvertHelpers.DatepickerToUtcDateTime(model.Birthday),
                RelationshipInt = model.RelationshipInt,
                CivilStatusInt = model.CivilStatusInt,
                DegreeInstructionInt = model.DegreeInstructionInt,
                Certificated = model.Certificated,
                Occupation = model.Occupation,
                WorkCenter = model.WorkCenter,
                Location = model.Location
            };

            _context.StudentFamilies.Add(studentFamily);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Edita la informacion familiar del estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene los campos de la informacion familiar del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("editar/familiar")]
        public async Task<IActionResult> EditStudentFamily(StudentFamilyEditViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Revise el formulario");

            var studentFamily = await _context.StudentFamilies.Where(x => x.Id == model.StudentFamilyId).FirstOrDefaultAsync();

            if (studentFamily == null)
                return BadRequest("Sucedio un error");

            studentFamily.Name = model.Name;
            studentFamily.PaternalName = model.PaternalName;
            studentFamily.MaternalName = model.MaternalName;
            studentFamily.Birthday = ConvertHelpers.DatepickerToUtcDateTime(model.Birthday);
            studentFamily.RelationshipInt = model.RelationshipInt;
            studentFamily.CivilStatusInt = model.CivilStatusInt;
            studentFamily.DegreeInstructionInt = model.DegreeInstructionInt;
            studentFamily.Certificated = model.Certificated;
            studentFamily.Occupation = model.Occupation;
            studentFamily.WorkCenter = model.WorkCenter;
            studentFamily.Location = model.Location;

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Obtiene la informacion familiar del estudiante
        /// </summary>
        /// <param name="Id">Identificador de la relacion estudiante-familia</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("familiar/get/{id}")]
        public async Task<IActionResult> GetStudentFamily(Guid id)
        {
            var result = await _context.StudentFamilies
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    paternalname = x.PaternalName,
                    maternalname = x.MaternalName,
                    birthday = x.Birthday.ToLocalDateFormat(),
                    relationship = x.RelationshipInt,
                    civilstatus = x.CivilStatusInt,
                    degreeinstruction = x.DegreeInstructionInt,
                    certificated = x.Certificated,
                    occupation = x.Occupation,
                    workcenter = x.WorkCenter,
                    location = x.Location,
                }).FirstOrDefaultAsync();

            return Ok(result);
        }

        /// <summary>
        /// Elimina la informacion familiar del estudiante
        /// </summary>
        /// <param name="Id">Identificador de la relacion familia-estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("eliminar/familiar/{id}")]
        public async Task<IActionResult> DeleteStudentFamily(Guid id)
        {
            var studentFamily = await _context.StudentFamilies.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (studentFamily == null)
                return BadRequest("Sucedio un error");

            _context.StudentFamilies.Remove(studentFamily);
            await _context.SaveChangesAsync();
            return Ok();
        }


        /// <summary>
        /// Edita la informacion familiar del estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene la informacion familiar del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("editar/familiar-salud")]
        public async Task<IActionResult> EditStudentFamilyHealth(StudentFamilyHealEditViewModel model)
        {
            var studentFamily = await _context.StudentFamilies.Where(x => x.Id == model.StudentFamilyId).FirstOrDefaultAsync();

            if (studentFamily == null)
                return BadRequest("Sucedio un error");

            studentFamily.IsSick = model.IsSick;
            studentFamily.SurgicalIntervention = model.SurgicalIntervention;
            studentFamily.DiseaseType = model.DiseaseType;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("familiar-salud/get/{id}")]
        public async Task<IActionResult> GetStudentFamilyHealth(Guid id)
        {
            var result = await _context.StudentFamilies
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    paternalname = x.PaternalName,
                    maternalname = x.MaternalName,
                    diseaseType = x.DiseaseType,
                    isSick = x.IsSick,
                    surgicalIntervention = x.SurgicalIntervention
                }).FirstOrDefaultAsync();

            return Ok(result);
        }

        [HttpPost("ficha-estudiante/salud/post")]
        public async Task<IActionResult> HealthSave(HealthViewModel model)
        {
            var studentInformation = await _context.StudentInformations
                                        .Where(x => x.StudentId == model.StudentId && x.TermId == model.TermId)
                                        .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == model.TermId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            var student = await _context.Students
                .Where(x => x.Id == model.StudentId)
                .FirstOrDefaultAsync();

            if (student == null || term == null)
                return BadRequest("Sucedio un error");

            if (studentInformation == null)
            {
                studentInformation = new StudentInformation
                {
                    TermId = model.TermId,
                    StudentId = model.StudentId
                };
                await _context.StudentInformations.AddAsync(studentInformation);
            }

            studentInformation.IsSick = model.IsSick;
            studentInformation.TypeParentIllness = model.TypeParentIllness;
            studentInformation.HaveInsurance = model.HaveInsurance;
            studentInformation.InsuranceDescription = model.InsuranceDescription;

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Guarda la parte de la salud de la ficha del estudiante
        /// </summary>
        /// <param name="model">Modelo que contieen los parametros de la salud a editar</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpGet("ficha-estudiante/salud/{studentId}/periodo/{termId}")]
        public async Task<IActionResult> HealthGet(Guid studentId, Guid termId)
        {
            var student = await _context.Students
                            .Where(x => x.Id == studentId)
                            .Select(x => new
                            {
                                x.Id,
                                x.CareerId
                            })
                            .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == termId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            if (term == null || student == null)
                return BadRequest("Sucedio un error");

            var studentInformation = await _context.StudentInformations
                .Where(x => x.StudentId == studentId && x.TermId == termId)
                .Select(x => new
                {
                    x.IsSick,
                    x.TypeParentIllness,
                    x.HaveInsurance,
                    x.InsuranceDescription,
                })
                .FirstOrDefaultAsync();
            var model = new HealthViewModel();

            if (studentInformation != null)
            {
                model.IsSick = studentInformation.IsSick;
                model.TypeParentIllness = studentInformation.TypeParentIllness;
                model.HaveInsurance = studentInformation.HaveInsurance;
                model.InsuranceDescription = studentInformation.InsuranceDescription;
            }

            return Ok(model);
        }

        [HttpGet("ficha-estudiante/alimentacion/{studentId}/periodo/{termId}")]
        public async Task<IActionResult> FeedingGet(Guid studentId, Guid termId)
        {
            var student = await _context.Students
                            .Where(x => x.Id == studentId)
                            .Select(x => new
                            {
                                x.Id,
                                x.CareerId
                            })
                            .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == termId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            if (term == null || student == null)
                return BadRequest("Sucedio un error");

            var studentInformation = await _context.StudentInformations
                .Where(x => x.StudentId == studentId && x.TermId == termId)
                .Select(x => new
                {
                    x.BreakfastHome,
                    x.BreakfastPension,
                    x.BreakfastRelativeHome,
                    x.BreakfastOther,

                    x.LunchHome,
                    x.LunchPension,
                    x.LunchRelativeHome,
                    x.LunchOther,

                    x.DinnerHome,
                    x.DinnerPension,
                    x.DinnerRelativeHome,
                    x.DinnerOther,
                })
                .FirstOrDefaultAsync();
            var model = new FeedViewModel();

            if (studentInformation != null)
            {
                model.BreakfastHome = studentInformation.BreakfastHome;
                model.BreakfastPension = studentInformation.BreakfastPension;
                model.BreakfastRelativeHome = studentInformation.BreakfastRelativeHome;
                model.BreakfastOther = studentInformation.BreakfastOther;
                model.LunchHome = studentInformation.LunchHome;
                model.LunchPension = studentInformation.LunchPension;
                model.LunchRelativeHome = studentInformation.LunchRelativeHome;
                model.LunchOther = studentInformation.LunchOther;
                model.DinnerHome = studentInformation.DinnerHome;
                model.DinnerPension = studentInformation.DinnerPension;
                model.DinnerRelativeHome = studentInformation.DinnerRelativeHome;
                model.DinnerOther = studentInformation.DinnerOther;
            }

            return Ok(model);
        }


        /// <summary>
        /// Guarda la parte de la alimentacion de la ficha del estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene los parametros de la ficha del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("ficha-estudiante/alimentacion/post")]
        public async Task<IActionResult> FeedingSave(FeedViewModel model)
        {
            var studentInformation = await _context.StudentInformations
                                                    .Where(x => x.StudentId == model.StudentId && x.TermId == model.TermId)
                                                    .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == model.TermId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            var student = await _context.Students
                .Where(x => x.Id == model.StudentId)
                .FirstOrDefaultAsync();

            if (student == null || term == null)
                return BadRequest("Sucedio un error");

            if (studentInformation == null)
            {
                studentInformation = new StudentInformation
                {
                    TermId = model.TermId,
                    StudentId = model.StudentId
                };
                await _context.StudentInformations.AddAsync(studentInformation);
            }

            studentInformation.BreakfastHome = model.BreakfastHome;
            studentInformation.BreakfastPension = model.BreakfastPension;
            studentInformation.BreakfastRelativeHome = model.BreakfastRelativeHome;
            studentInformation.BreakfastOther = model.BreakfastOther;
            studentInformation.LunchHome = model.LunchHome;
            studentInformation.LunchPension = model.LunchPension;
            studentInformation.LunchRelativeHome = model.LunchRelativeHome;
            studentInformation.LunchOther = model.LunchOther;
            studentInformation.DinnerHome = model.DinnerHome;
            studentInformation.DinnerPension = model.DinnerPension;
            studentInformation.DinnerRelativeHome = model.DinnerRelativeHome;
            studentInformation.DinnerOther = model.DinnerOther;

            await SaveChangesAsync();
            return Ok();
        }

        [HttpGet("ficha-estudiante/vivienda/{studentId}/periodo/{termId}")]
        public async Task<IActionResult> LivingPlaceGet(Guid studentId, Guid termId)
        {
            var student = await _context.Students
                            .Where(x => x.Id == studentId)
                            .Select(x => new
                            {
                                x.Id,
                                x.CareerId
                            })
                            .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == termId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            if (term == null || student == null)
                return BadRequest("Sucedio un error");

            var studentInformation = await _context.StudentInformations
                .Where(x => x.StudentId == studentId && x.TermId == termId)
                .Select(x => new
                {
                    x.Tenure,
                    x.ContructionType,
                    x.ZoneType,
                    x.BuildType,
                    x.OtherTypeLivingPlace,
                    x.NumberFloors,
                    x.NumberRooms,
                    x.NumberKitchen,
                    x.NumberBathroom,
                    x.NumberLivingRoom,
                    x.NumberDinningRoom,
                    x.Water,
                    x.Drain,
                    x.LivingPlacePhone,
                    x.Light,
                    x.Internet,
                    x.TV,
                    x.HasPhone,
                    x.Radio,
                    x.Stereo,
                    x.Iron,
                    x.EquipPhone,
                    x.Laptop,
                    x.Closet,
                    x.Fridge,
                    x.PersonalLibrary,
                    x.EquipComputer
                })
                .FirstOrDefaultAsync();
            var model = new LivingPlaceViewModel();

            if (studentInformation != null)
            {
                model.Tenure = studentInformation.Tenure;
                model.ContructionType = studentInformation.ContructionType;
                model.ZoneType = studentInformation.ZoneType;
                model.BuildType = studentInformation.BuildType;
                model.OtherTypeLivingPlace = studentInformation.OtherTypeLivingPlace;
                model.NumberFloors = studentInformation.NumberFloors;
                model.NumberRooms = studentInformation.NumberRooms;
                model.NumberKitchen = studentInformation.NumberKitchen;
                model.NumberBathroom = studentInformation.NumberBathroom;
                model.NumberLivingRoom = studentInformation.NumberLivingRoom;
                model.NumberDinningRoom = studentInformation.NumberDinningRoom;
                model.Water = studentInformation.Water;
                model.Drain = studentInformation.Drain;
                model.LivingPlacePhone = studentInformation.LivingPlacePhone;
                model.Light = studentInformation.Light;
                model.Internet = studentInformation.Internet;
                model.TV = studentInformation.TV;
                model.HasPhone = studentInformation.HasPhone;
                model.Radio = studentInformation.Radio;
                model.Stereo = studentInformation.Stereo;
                model.Iron = studentInformation.Iron;
                model.EquipPhone = studentInformation.EquipPhone;
                model.Laptop = studentInformation.Laptop;
                model.Closet = studentInformation.Closet;
                model.Fridge = studentInformation.Fridge;
                model.PersonalLibrary = studentInformation.PersonalLibrary;
                model.EquipComputer = studentInformation.EquipComputer;
            }

            return Ok(model);
        }



        /// <summary>
        /// Guarda los datos de la vivienta de la ficha del estudiante
        /// </summary>
        /// <param name="model">Modelo que contiene la parte de la vivienda de la ficha del estudiante</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("ficha-estudiante/vivienda/post")]
        public async Task<IActionResult> LivingPlaceSave(LivingPlaceViewModel model)
        {
            var studentInformation = await _context.StudentInformations
                .Where(x => x.StudentId == model.StudentId && x.TermId == model.TermId)
                .FirstOrDefaultAsync();

            var term = await _context.Terms
                .Where(x => x.Id == model.TermId)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync();

            var student = await _context.Students
                .Where(x => x.Id == model.StudentId)
                .FirstOrDefaultAsync();

            if (student == null || term == null)
                return BadRequest("Sucedio un error");

            if (studentInformation == null)
            {
                studentInformation = new StudentInformation
                {
                    TermId = model.TermId,
                    StudentId = model.StudentId
                };
                await _context.StudentInformations.AddAsync(studentInformation);
            }

            studentInformation.Tenure = model.Tenure;
            studentInformation.ContructionType = model.ContructionType;
            studentInformation.ZoneType = model.ZoneType;
            studentInformation.BuildType = model.BuildType;
            studentInformation.OtherTypeLivingPlace = model.OtherTypeLivingPlace;
            studentInformation.NumberFloors = model.NumberFloors;
            studentInformation.NumberRooms = model.NumberRooms;
            studentInformation.NumberKitchen = model.NumberKitchen;
            studentInformation.NumberBathroom = model.NumberBathroom;
            studentInformation.NumberLivingRoom = model.NumberLivingRoom;
            studentInformation.NumberDinningRoom = model.NumberDinningRoom;
            studentInformation.Water = model.Water;
            studentInformation.Drain = model.Drain;
            studentInformation.LivingPlacePhone = model.LivingPlacePhone;
            studentInformation.Light = model.Light;
            studentInformation.Internet = model.Internet;
            studentInformation.TV = model.TV;
            studentInformation.HasPhone = model.HasPhone;
            studentInformation.Radio = model.Radio;
            studentInformation.Stereo = model.Stereo;
            studentInformation.Iron = model.Iron;
            studentInformation.EquipPhone = model.EquipPhone;
            studentInformation.Laptop = model.Laptop;
            studentInformation.Closet = model.Closet;
            studentInformation.Fridge = model.Fridge;
            studentInformation.PersonalLibrary = model.PersonalLibrary;
            studentInformation.EquipComputer = model.EquipComputer;

            await _context.SaveChangesAsync();
            return Ok();
        }

        #endregion

        private byte[] GenerarQv2(Guid studentId)
        {
            QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            var URLAbsolute = HttpContext.Request.Host + "/ficha-socioeconomica/ver/" + studentId;
            QRCoder.QRCodeData qrCodeData = qrGenerator.CreateQrCode(URLAbsolute, QRCoder.QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(5);
        }

        private string GetImageQR(Guid studentId)
        {
            var bitMap = GenerarQv2(studentId);
            var finalQR = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitMap));
            return finalQR;
        }
    }
}
