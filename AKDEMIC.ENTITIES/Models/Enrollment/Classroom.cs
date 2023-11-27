using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Classroom : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        public Guid ClassroomTypeId { get; set; }

        [Required]
        public int Capacity { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(255)]
        public string Description { get; set; }
        public string Floor { get; set; }
        public int Number { get; set; }
        public string IPAddress { get; set; }
        [Required]
        public byte Status { get; set; } = 1;

        public Guid? FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public Building Building { get; set; }
        public ClassroomType ClassroomType { get; set; }

        public ICollection<ClassSchedule> ClassSchedules { get; set; }
    }
}
