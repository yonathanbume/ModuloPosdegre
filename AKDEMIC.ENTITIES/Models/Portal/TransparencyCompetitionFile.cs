using System;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class TransparencyCompetitionFile
    {
        public Guid Id { get; set; }
        public string FileUrl { get; set; }
        public string FileExtension { get; set; }
        public int Type { get; set; }

        public Guid TransparencyCompetitionId { get; set; }
        public TransparencyCompetition TransparencyCompetition { get; set; }
    }
}
