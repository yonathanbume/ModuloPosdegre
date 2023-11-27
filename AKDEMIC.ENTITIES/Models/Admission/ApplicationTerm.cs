using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class ApplicationTerm : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid TermId { get; set; }

        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ExtraEndDate { get; set; }
        public DateTime? ExtraStartDate { get; set; }
        public DateTime InscriptionEndDate { get; set; }
        public DateTime InscriptionStartDate { get; set; }
        public string Name { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Status { get; set; }
        public int ReinscriptionDays { get; set; }
        public bool WasPublished { get; set; } = false;

        public Term Term { get; set; }

        public ICollection<AdmissionExamApplicationTerm> AdmissionExamApplicationTerms { get; set; }
        public ICollection<ApplicationTermAdmissionType> ApplicationTermAdmissionTypes { get; set; }
        public ICollection<ApplicationTermCampus> ApplicationTermCampuses { get; set; }
        public ICollection<ApplicationTermManager> ApplicationTermManagers { get; set; }
        public ICollection<Postulant> Postulants { get; set; }
        public ICollection<ApplicationTermAdmissionFile> ApplicationTermAdmissionFiles { get; set; }
    }
}
