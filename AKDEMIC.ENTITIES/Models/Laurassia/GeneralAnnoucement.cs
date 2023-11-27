using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class GeneralAnnouncement : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string Area { get; set; }
        public string Content { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }
}
