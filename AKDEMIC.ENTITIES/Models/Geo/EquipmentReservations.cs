using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Geo
{
    public class EquipmentReservations
    {
        public Guid Id { get; set; }

        public DateTime ReservationDate { get; set; }

        public string TeacherId { get; set; }

        public Guid LaboratoryEquipmentsId { get; set; }

        public Teacher Teacher { get; set; }

        public LaboratoryEquipments LaboratoryEquipments { get; set; }
    }
}
