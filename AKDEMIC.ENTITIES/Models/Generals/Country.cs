using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Country : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public ICollection<Department> Departments { get; set; }
        public ICollection<Postulant> PostulantsNationalityCountry { get; set; }
        public ICollection<Postulant> PostulantsBirthCountry { get; set; }
        public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsNationalityCountry { get; set; }
        public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsBirthCountry { get; set; }
        public ICollection<WorkerLaborInformation> WorkersLaborInformationBirthCountry { get; set; }
        public ICollection<WorkerLaborInformation> WorkersLaborInformationResidenceCountry { get; set; }
    }
}
