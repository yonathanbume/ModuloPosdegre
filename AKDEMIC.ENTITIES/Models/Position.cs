using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models
{
    public class Position : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        public int Age { get; set; }
        public string Category { get; set; }
        public string Dedication { get; set; }
        public string AcademicDegree { get; set; }
        public string JobTitle { get; set; }
        public string FilePath { get; set; }
    }
}