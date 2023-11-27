using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class StudentExperience : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public string Position { get; set; }

        public string Area { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool CurrentWork { get; set; }

        public Guid? CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string ContactName { get; set; }

        public string ContactNumber { get; set; }

        public string Description { get; set; } //Funciones principales

        public bool IsPrivate { get; set; } //Es privado o publico

        public bool HasReportConfirmation { get; set; } = false;

        public Company Company { get; set; }

        public Student Student { get; set; }

        //Campos adicionales Para encuesta UNSM
        public Guid? DistrictId { get; set; }
        public District District { get; set; }
        public int EmploymentSituation { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.STUDENTEXPERIENCE.EMPLOYMENTSITUATION.
        public string BossName { get; set; } //Nombre de su jefe inmediato
        public string BossPhoneNumber { get; set; } //Número de telefono de su jefe
        public string BossEmail { get; set; } //Correo electronico de su jefe
        public string BossPosition { get; set; } //Cargo del jefe directo
        public decimal AverageSalary { get; set; } //Sueldo promedio mensual

    }
}
