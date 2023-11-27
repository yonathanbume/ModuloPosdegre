using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Overrides;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class GeneralDataViewModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Nombres", Prompt = "Nombres")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Paterno", Prompt = "Apellido Paterno")]
        public string PaternalSurname { get; set; }

        //[Required(AllowEmptyStrings = true, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Materno", Prompt = "Apellido Materno")]
        public string MaternalSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "{0} no válido")]
        [Display(Name = "Correo electrónico", Prompt = "Correo Electrónico")]
        public string Email { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "{0} no válido")]
        [Display(Name = "Correo Alternativo", Prompt = "Correo Alternativo")]
        public string PersonalEmail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [MinLength(7, ErrorMessage = "El campo '{0}' debe tener {1} dígitos")]
        //[RegularExpression("[1-9][0-9]*", ErrorMessage = "{0} no válido")]
        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string PhoneNumber { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Usuario", Prompt = "Usuario")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Fecha de Nacimiento", Prompt = "Fecha de Nacimiento")]
        public string BirthDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [StringLength(maximumLength: 8, MinimumLength = 8, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.STRING_LENGTH)]
        [RegularExpression("[0-9]*", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REGULAR_EXPRESSION)]
        [Display(Name = "Documento de Identidad", Prompt = "Documento")]
        public string Dni { get; set; }

        public string DocumentType { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Range(1, 2, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.RANGE)]
        [Display(Name = "Sexo", Prompt = "Sexo")]
        public int Sex { get; set; }

        [Display(Name = "Dirección", Prompt = "Dirección")]
        public string Address { get; set; }

        [DataType(DataType.Upload)]
        [Extensions(ConstantHelpers.DOCUMENTS.FILE_EXTENSION_GROUP.IMAGES, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.FILE_EXTENSIONS)]
        [Display(Name = "Foto", Prompt = "Foto")]
        public IFormFile Picture { get; set; }

        public string PictureUrl { get; set; }

        [DataType(DataType.Password)]
        [StringLength(maximumLength: 256, MinimumLength = 6, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.STRING_LENGTH)]
        [Display(Name = "Contraseña", Prompt = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.COMPARE)]
        [Display(Name = "Confirmar contraseña", Prompt = "Confirmar Contraseña")]
        public string ConfirmedPassword { get; set; }

        [Display(Name = "Facultad", Prompt = "Facultad")]
        public Guid FacultyId { get; set; }

        [Display(Name = "Escuela", Prompt = "Escuela")]
        public Guid SelectedCareer { get; set; }

        [Display(Name = "Departamento", Prompt = "Departamento")]
        public Guid? DepartmentId { get; set; }

        [Display(Name = "Provincia", Prompt = "Provincia")]
        public Guid? ProvinceId { get; set; }

        [Display(Name = "Distrito", Prompt = "Distrito")]
        public Guid? DistrictId { get; set; }

        [Display(Name = "Programa Academico", Prompt = "Programa Academico")]
        public string AcademicProgramName { get; set; }

        public string UrlPhotoCropImg { get; set; }
        [Display(Name = "Usuario Web", Prompt = "Usuario Web")]
        public string UserWeb { get; set; }

        [Display(Name = "Identidad Étnica")]
        public byte RacialIdentity { get; set; } = ConstantHelpers.Student.RacialIdentity.OTHER;

        //[Display(Name = "Escala de Pagos", Prompt = "Escala")]
        //public Guid? StudentScaleId { get; set; }

        [Display(Name = "Pensión de Matrícula", Prompt = "Pensión")]
        public Guid? EnrollmentFeeId { get; set; }

        public bool EnableFees { get; set; }
    }
}
