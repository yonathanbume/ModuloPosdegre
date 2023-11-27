using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Department : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? CountryId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public Country Country { get; set; }

        public ICollection<Province> Provinces { get; set; }
        public ICollection<Postulant> PostulantsBirthDepartment { get; set; }
        public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsBirthDepartment { get; set; }
        public ICollection<Postulant> PostulantsDepartment { get; set; }
        public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsDepartment { get; set; }
        public ICollection<Postulant> PostulantsSecondaryEducationDepartment { get; set; }
        //public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsSecondaryEducationDepartment { get; set; }
        public ICollection<WorkerLaborInformation> WorkersLaborInformationBirthDepartment { get; set; }
        public ICollection<WorkerLaborInformation> WorkersLaborInformationResidenceDepartment { get; set; }
    }
}
