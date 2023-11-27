using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Building : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CampusId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(250)]
        public string Name { get; set; }

        public Campus Campus { get; set; }

        public ICollection<Classroom> Classrooms { get; set; }
    }
}
