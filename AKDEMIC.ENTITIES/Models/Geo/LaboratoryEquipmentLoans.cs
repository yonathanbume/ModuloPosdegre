using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Geo
{
    public class LaboratoryEquipmentLoans
    {
        [Key]
        public Guid Id { get; set; }

        public string Observation { get; set; }

        public string ReturnObservation { get; set; }

        public string TeacherId { get; set; }

        public string UserId { get; set; }

        public string LaboratoyRequestId { get; set; }

        public Guid LaboratoryEquipmentsId { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public bool OnLoan { get; set; }

        public Teacher Teacher { get; set; }

        public ApplicationUser User { get; set; }

        public LaboratoryEquipments LaboratoryEquipments { get; set; }

        public LaboratoyRequest LaboratoyRequest { get; set; }
    }
}
