using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Classifier : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public string Name { get; set; }

        public Classifier Parent { get; set; }
        public ICollection<Concept> Concepts { get; set; }
        public ICollection<Classifier> Classifiers { get; set; }
    }
}
