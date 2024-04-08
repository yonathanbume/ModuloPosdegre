﻿using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
namespace AKDEMIC.POSDEGREE.Areas.Admin.Models.MasterViewModel
{
    public class AddMasterViewModel
    {
      public Guid Id { get; set; }
        public int Nro { get; set; }
        public string? Sede { get; set; }
        public string? Curricula { get; set; }
        public string? StudyProgram { get; set; }
        public string? StudyMode { get; set; }
        public bool current { get; set; } = true;
        public bool state { get; set; } = true;

        public string? Nombre { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "La Duración debe ser un número positivo")]
        public int Duracion { get; set; } // Duración en años
        [Range(1, int.MaxValue, ErrorMessage = "Los Créditos deben ser un número positivo")]
        public int Creditos { get; set; }
        public string? Descripcion { get; set; }
    }
}
