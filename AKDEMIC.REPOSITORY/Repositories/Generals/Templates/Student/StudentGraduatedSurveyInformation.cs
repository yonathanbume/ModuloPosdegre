using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentGraduatedSurveyInformation
    {
        public bool HasGraduatedSurveyCompleted { get; set; }

        public string StudentPhoneNumber { get; set; }
        public string StudentEmail { get; set; }
        public string StudentCurrentAddress { get; set; }
        public string StudentAddressUbigeo { get; set; }

        //Campos de BD, no editables
        public string FullName { get; set; }
        public string Dni { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string GraduatedTerm { get; set; }
        public string StudentSuneduStatus { get; set; }

        //Campos por Api de SUNEDU, no editables
        public bool HasWorkingExperience { get; set; }
        public string StudentStatus { get; set; }
        public bool CurrentWork { get; set; }
        public int EmploymentSituation { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.STUDENTEXPERIENCE.EMPLOYMENTSITUATION.
        public string CompanyRuc { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public Guid? CompanySizeId { get; set; }
        public Guid? CompanyTypeId { get; set; }
        public Guid? EconomicActivityId { get; set; } //Actividad Económica por clase
        public Guid? DistrictId { get; set; }
        public string WorkerArea { get; set; } //Area en la que labora
        public string WorkerPosition { get; set; } //Cargo que desempeña
        public string MainFunctions { get; set; } //Funciones principales
        public decimal AverageSalary { get; set; } //Sueldo promedio mensual
        public string WorkerStartDate { get; set; } //Inicio de sus labores
        public string WorkerEndDate { get; set; } //Fin de sus labores
        public string BossName { get; set; } //Nombre de su jefe inmediato
        public string BossPhoneNumber { get; set; } //Número de telefono de su jefe
        public string BossEmail { get; set; } //Correo electronico de su jefe
    }
}
