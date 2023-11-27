using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Reservations
{
    public class EnvironmentReservation
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }  
        public bool IsExternalUser { get; set; }
        public DateTime ReservatedAt { get; set; }
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
        [Required]
        public int State { get; set; } = ConstantHelpers.RESERVATION.STATUS.PENDING;
        public Guid? PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }
        public Guid EnvironmentId { get; set; }
        [ForeignKey("EnvironmentId")]
        public Environment Environment { get; set; }
    }
}
