using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
namespace AKDEMIC.POSDEGREE.Areas.Admin.Models.MasterViewModel
{
    public class AddMasterViewModel
    {
      public Guid Id { get; set; }
        public string? Nombre { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "La Duración debe ser un número positivo")]
        public int Duracion { get; set; } // Duración en años
        [Range(1, int.MaxValue, ErrorMessage = "Los Créditos deben ser un número positivo")]
        public int Creditos { get; set; }
        public string? Descripcion { get; set; }
    }
}
