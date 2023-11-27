using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class ClassroomType : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }
    }
}
