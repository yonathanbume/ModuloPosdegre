using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class StudentGraduatedSurvey : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }
        public Student Student { get; set; }
        public Term Term { get; set; }

        public bool HasWorkingExperience { get; set; }

        public bool CurrentWork { get; set; }
        public int EmploymentSituation { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.STUDENTEXPERIENCE.EMPLOYMENTSITUATION.
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
        public Guid? DistrictId { get; set; }
        public District District { get; set; }
        public string Area { get; set; } //Área en que labora
        public string Position { get; set; } //Cargo que desempeña
        public string MainFunctions { get; set; } //Funciones principales

        public DateTime? StartDate { get; set; } //Fecha de inicio de sus labores 
        public DateTime? EndDate { get; set; } //Fecha de fin de sus labores

        public string BossName { get; set; } //Nombre de su jefe inmediato
        public string BossPhoneNumber { get; set; } //Número de telefono de su jefe
        public string BossEmail { get; set; } //Correo electronico de su jefe
        public string BossPosition { get; set; } //Cargo del jefe directo
        public decimal AverageSalary { get; set; } //Sueldo promedio mensual
    }
}
