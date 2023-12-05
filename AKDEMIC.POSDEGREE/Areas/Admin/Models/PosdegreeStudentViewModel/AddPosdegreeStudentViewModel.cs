using AKDEMIC.ENTITIES.Models.PosDegree;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Models.PosdegreeStudentViewModel
{
    public class AddPosdegreeStudentViewModel
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoP { get; set; }
        public string? ApellidoM { get; set; }
        public string? Dni { get; set; }
        public string? email { get; set; }
        public string? telefono { get; set; }
        public string? direccion { get; set; }

        public IFormFile File { get; set; }
        public ICollection<PosdegreeDetailsPayment> posdegreeDetailsPayments { get; set; }

    }
}
