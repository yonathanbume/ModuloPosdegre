using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class AcademicDepartment : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? CareerId { get; set; }
        public Guid FacultyId { get; set; }
        
        [StringLength(255)]
        public string Name { get; set; }
        public byte Status { get; set; }
        public string AcademicDepartmentDirectorId { get; set; }
        public ApplicationUser AcademicDepartmentDirector { get; set; }
        public string AcademicDepartmentSecretaryId { get; set; }
        public ApplicationUser AcademicDepartmentSecretary { get; set; }

        public string AcademicDepartmentCoordinatorId { get; set; }
        public ApplicationUser AcademicDepartmentCoordinator { get; set; }

        public Career Career { get; set; }
        public Faculty Faculty { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}
