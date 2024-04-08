namespace AKDEMIC.POSDEGREE.Areas.Admin.Models.AsignaturaViewModel
{
    public class AddAsignaturaViewModel
    {
        public Guid Id { get; set; }
        public string codigo { get; set; }
        public string nameAsignatura { get; set; }
        public decimal credito { get; set; } = 1.0M;
        public byte hteoricas { get; set; } = 0;
        public byte hpracticas { get; set; } = 0;
        public byte totalhoras { get; set; } = 0;
        public string requisito { get; set; }
    }
}
