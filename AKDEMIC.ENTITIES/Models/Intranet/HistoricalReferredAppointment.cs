using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class HistoricalReferredAppointment  
    {
        public Guid Id { get; set; }
        public string ReferredByDoctorId { get; set; }
        public string ReferredToDoctorId { get; set; }
        public ApplicationUser ReferredByDoctor { get; set; }
        public ApplicationUser ReferredToDoctor { get; set; }
        public DateTime DateReferred { get; set; }
        public Guid MedicalAppointmentId { get; set; }
        public MedicalAppointment MedicalAppointment { get; set; }
    }
}
