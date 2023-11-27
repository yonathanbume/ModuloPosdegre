using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.CORE.Helpers;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Holiday
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public byte Type { get; set; } = ConstantHelpers.Generals.Holiday.Type.National;
        public bool NeedReschedule { get; set; }
    }
}
