using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Reservations
{
    public class Environment 
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        [Required]
        [StringLength(50)]
        public string Tusne { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int MaxReservationAdmin { get; set; } = 1;
        public int MaxReservationTeacher { get; set; } = 1;
        public int MaxReservationStudent { get; set; } = 1;
        public int MaxReservationExternal { get; set; } = 1;
        public string ImageURL { get; set; }
        public decimal Price { get; set; } = 0.00M;
        public bool IsActive { get; set; } = true;
    }
}
