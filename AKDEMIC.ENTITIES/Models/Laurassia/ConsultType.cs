using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class ConsultType : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}
