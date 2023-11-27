using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class CatalogActivity : ITimestamp
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int SectionEjec { get; set; }
        public int Type { get; set; }
        public int General { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public char Status { get; set; }
        public string NameGroup { get; set; }
        public char Level { get; set; }
        public int TypePpto { get; set; }
        public char TypeUse { get; set; }
        public string IdActPoi { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
