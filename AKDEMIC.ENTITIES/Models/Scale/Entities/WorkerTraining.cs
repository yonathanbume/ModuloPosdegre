using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerTraining
    {
        public Guid Id { get; set; }
        public decimal TotalHours { get; set; } //Horas
        public decimal Credits { get; set; } //Creditos
        public byte Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Institution { get; set; }
        public string TrainingDocumentUrl { get; set; } //opcional
        public string RegisterNumber { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
