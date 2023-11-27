using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.INTRANET.Areas.Admin.Models.StudentViewModels;
using AKDEMIC.INTRANET.Controllers;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using iTextSharp.text.pdf.qrcode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConstantHelpers = AKDEMIC.CORE.Helpers.ConstantHelpers;
using ConvertHelpers = AKDEMIC.CORE.Helpers.ConvertHelpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Controllers
{
    [Authorize(Roles =
        ConstantHelpers.ROLES.SUPERADMIN + "," +
        ConstantHelpers.ROLES.INTRANET_ADMIN + "," +
        ConstantHelpers.ROLES.DEAN + "," +
        ConstantHelpers.ROLES.DEAN_SECRETARY)]
    [Area("Admin")]
    [Route("admin/alumnos")]
    public class StudentController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IStudentService _studentService;

        public StudentController(IUserService userService,
            UserManager<ApplicationUser> userManager,
            IStudentService studentService,
            IDataTablesService dataTablesService,
            IOptions<CloudStorageCredentials> storageCredentials) : base(userManager, userService, dataTablesService)
        {
            _storageCredentials = storageCredentials;
            _studentService = studentService;
        }

        /// <summary>
        /// Obtiene el listado de alumnos para ser usado en tablas
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="programId">Identificador del programa académico</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de alumnos</returns>
        [HttpGet("get")]
        public async Task<IActionResult> Get(Guid? facultyId = null, Guid? careerId = null, Guid? programId = null, string search = null, Guid? curriculumId = null, Guid? campusId = null)
        {
            var sentParameters = GetSentParameters();
            var result = await _studentService.GetStudentsDatatable(sentParameters, search, facultyId, careerId, academicProgramId: programId, curriculumId: curriculumId, campusId: campusId);
            return Ok(result);
        }

        #region --- JSON ---

        /// <summary>
        /// Obtiene el listado de alumnos para ser usado en tablas
        /// </summary>
        /// <param name="facultyId">Identificador de la facultad</param>
        /// <param name="careerId">Identificador de la escuela profesional</param>
        /// <param name="programId">Identificador del programa académico</param>
        /// <param name="search">Texto de búsqueda</param>
        /// <returns>Objeto que contiene el listado de alumnos</returns>
        [Route("get")]
        public async Task<IActionResult> GetStudents(Guid? fid = null, Guid? cid = null, Guid? programId = null, string search = null)
        {
            var sentParameters = GetSentParameters();
            var result = await _studentService.GetStudentsDatatable(sentParameters, search, fid, cid, academicProgramId: programId);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene los datos del estudiante
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Objeto que contiene los datos del estudiante</returns>
        [Route("{id}/carrera/get")]
        public async Task<IActionResult> GetStudentCareer(Guid id)
        {
            var student = await _studentService.Get(id);
            var result = new
            {
                id = student.Career.Id,
                name = student.Career.Name
            };


            return Ok(result);
        }
        #endregion

        #region --- ACTIONS ---

        /// <summary>
        /// Vista principal donde se listan los estudiantes
        /// </summary>
        /// <returns>Vista principal del controlador</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Vista para agregar un estudiante
        /// </summary>
        /// <returns>Vista</returns>
        [Route("agregar")]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Vista para editar los datos del estudiante
        /// </summary>
        /// <param name="id">Identificador del estudiante</param>
        /// <returns>Vista edición</returns>
        [Route("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {

            if (id == Guid.Empty)
                throw new ApplicationException($"No se pudo encontrar el usuario con el id {id}.");
            var student = await _studentService.GetStudentWithCareerAndUser(id);
            var model = new StudentViewModel()
            {
                Id = student.Id,
                Email = student.User.Email,
                PhoneNumber = student.User.PhoneNumber,
                UserName = student.User.UserName,
                PaternalSurname = student.User.PaternalSurname,
                MaternalSurname = student.User.MaternalSurname,
                Name = student.User.Name,
                Address = student.User.Address,
                Dni = student.User.Dni,
                PictureUrl = student.User.Picture,
                Sex = student.User.Sex,
                BirthDate = $"{student.User.BirthDate:dd/MM/yyyy}",
                FacultyId = student.Career.FacultyId,
                SelectedCareer = student.CareerId,
                PhoneNumber2 = student.User.PhoneNumber2
            };

            return View(model);
        }

        /// <summary>
        /// Método para editar los datos del alumno
        /// </summary>
        /// <param name="model">Objeto que contiene los datos actualizados del alumno</param>
        /// <param name="id">Identificador del alumno</param>
        /// <returns>Código de estado HTTP</returns>
        [HttpPost("editar/{id}")]
        public async Task<IActionResult> Edit(StudentViewModel model, Guid id)
        {
            var ids = ModelState.Where(x => x.Key.Equals("Name") || x.Key.Equals("MaternalSurname") || x.Key.Equals("PaternalSurname") || x.Key.Equals("UserName"));
            foreach (var item in ids)
            {
                ModelState[item.Key].Errors.Clear();
                ModelState[item.Key].ValidationState = ModelValidationState.Valid;
            }

            if (!ModelState.IsValid)
                return BadRequest("Por favor, revise la información ingresada");

            if (CORE.Helpers.ConvertHelpers.DatepickerToUtcDateTime(model.BirthDate) > DateTime.UtcNow)
                return BadRequest("La Fecha de Nacimiento ingresada no es válida.");

            var passwordIsValid = false;

            if (!string.IsNullOrEmpty(model.Password))
            {
                if (model.Password.Length < 6)
                    return BadRequest("La contraseña debe tener 6 caracteres como mínimo.");
                else
                {
                    var passwordValidator = new PasswordValidator<ApplicationUser>();
                    passwordIsValid = passwordValidator
                        .ValidateAsync(_userManager, new ApplicationUser(), model.Password).Result.Succeeded;
                    if (!passwordIsValid)
                        return BadRequest("La contraseña debe contener al menos 1 letra mayúscula, 1 letra minúscula, 1 dígito y un caracter no alfanumérico.");
                }
            }

            var student = await _studentService.Get(model.Id);
            var user = await _userService.Get(student.UserId);

            var studentUsername = await _userService.GetByUserName(user.UserName);
            if (studentUsername != null && studentUsername.Id != user.Id)
                return BadRequest("El usuario especificado ya se encuentra registrado.");

            var anyWithEmail = await _userService.AnyWithSameEmail(user.Id, model.Email);
            if (anyWithEmail) return BadRequest("El correo electrónico especificado ya se encuentra registrado.");

            FillApplicationUser(ref user, model);

            if (passwordIsValid)
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);

            student.UserId = user.Id;
            // Upload Picture
            if (model.Picture != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                if (!string.IsNullOrEmpty(user.Picture))
                    await storage.TryDelete(user.Picture, ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE);

                user.Picture = await storage.UploadFile(model.Picture.OpenReadStream(), ConstantHelpers.CONTAINER_NAMES.USER_PROFILE_PICTURE,
                    Path.GetExtension(model.Picture.FileName), ConstantHelpers.FileStorage.SystemFolder.INTRANET);
            }

            await _userService.Update(user);
            return Ok();
        }

        /// <summary>
        /// Método para colocar los nuevos datos del alumno en la entidad
        /// </summary>
        /// <param name="user">Entidad alumno</param>
        /// <param name="model">Objeto que contiene los datos actualizados del alumno</param>
        private void FillApplicationUser(ref ApplicationUser user, StudentViewModel model)
        {
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.PhoneNumber = model.PhoneNumber;
            user.PhoneNumber2 = model.PhoneNumber2;
            //user.UserName = model.UserName;
            //user.NormalizedUserName = model.UserName.ToUpper();
            //user.Name = model.Name;
            //user.MaternalSurname = model.MaternalSurname;
            //user.PaternalSurname = model.PaternalSurname;
            user.Address = model.Address;
            user.BirthDate = ConvertHelpers.DatepickerToDatetime(model.BirthDate);
            user.Sex = model.Sex;
            user.Dni = model.Dni;
            user.Document = model.Dni;
            user.Type = ConstantHelpers.USER_TYPES.STUDENT;
        }
        #endregion
    }
}
