using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Province : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public Department Department { get; set; }

        public ICollection<District> Districts { get; set; }
        public ICollection<Postulant> PostulantsBirthProvince { get; set; }
        public ICollection<Postulant> PostulantsProvince { get; set; }
        public ICollection<Postulant> PostulantsSecondaryEducationProvince { get; set; }
        public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsBirthProvince { get; set; }
        public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsProvince { get; set; }
        //public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsSecondaryEducationProvince { get; set; }
        public ICollection<WorkerLaborInformation> WorkersLaborInformationBirthProvince { get; set; }
        public ICollection<WorkerLaborInformation> WorkersLaborInformationResidenceProvince { get; set; }
    }
}
