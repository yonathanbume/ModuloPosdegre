using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.POSDEGREE.Helpers;
using Bogus.DataSets;
using IdentityServer4.Models;
//using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Models.PosdegreeStudentViewModel
{
    public class AddPosdegreeStudentViewModel
    {
        public Guid Id { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage =ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        public string Codigo { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoP { get; set; }
        public string? ApellidoM { get; set; }
        public string? Dni { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage =ConstantHelpers.MESSAGES.VALIDATION.EMAIL_ADDRESS)]
        public string? email { get; set; }
        public string? telefono { get; set; }
        public string? direccion { get; set; }
        public IFormFile File { get; set; }
        public ICollection<PosdegreeDetailsPayment> posdegreeDetailsPayments { get; set; }

    }
}
