using System;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution
{
    public class ScaleSectionAnnexTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ExpeditionFormattedDate { get; set; }
        public string AnnexDocument { get; set; }
        public string Type { get; set; }
        public string Condition { get; set; }
        public int ScaleSectionNumber { get; set; }
    }
}
