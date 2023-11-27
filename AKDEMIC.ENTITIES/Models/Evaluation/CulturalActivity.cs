using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Evaluation
{
    public class CulturalActivity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? TypeId { get; set; }
        public CulturalActivityType Type { get; set; }
        public Guid? CareerId { get; set; }
        public Career Career { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }
        public string To { get; set; }
        public decimal? Price { get; set; }
        public string Objective { get; set; }
        public string Strategy { get; set; }
        public string Activities { get; set; }
        public string Users { get; set; }
        public string Competencies { get; set; }
        public bool IsPrivate { get; set; }
        public ICollection<CulturalActivityFile> CulturalActivityFiles { get; set; }
        public ICollection<RegisterCulturalActivity> Registers { get; set; }
    }
}
