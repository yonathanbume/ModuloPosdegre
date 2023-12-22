using AKDEMIC.ENTITIES.Models.PosDegree;

namespace AKDEMIC.POSDEGREE.Areas.Admin.Models.TeacherViewModel
{
    public class AddTeacherViewModel
    {
        public Guid id { get; set; }
        public string? Nombre { get; set; }
        public string? APaterno { get; set; }
        public string? AMaterno { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Departamento { get; set; }
        public string? Especialidad{ get; set; }
        public ICollection<RegistroNotas> RegistroNotas { get; set; }
    }
}
