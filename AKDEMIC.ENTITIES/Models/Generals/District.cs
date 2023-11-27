using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class District : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid ProvinceId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public Province Province { get; set; }

        public ICollection<Postulant> PostulantsBirthDistrict { get; set; }
        public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsBirthDistrict { get; set; }
        public ICollection<Postulant> PostulantsDistrict { get; set; }
        public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsDistrict { get; set; }
        public ICollection<Postulant> PostulantsSecondaryEducationDistrict { get; set; }
        //public ICollection<Preuniversitary.PreuniversitaryPostulant> PreUniversityPostulantsSecondaryEducationDistrict { get; set; }
        public ICollection<WorkerLaborInformation> WorkersLaborInformationBirthDistrict { get; set; }
        public ICollection<WorkerLaborInformation> WorkersLaborInformationResidenceDistrict { get; set; }
    }
}
