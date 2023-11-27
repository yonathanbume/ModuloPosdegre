using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class DigitalDocument : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int Type { get; set; }
        public int Class { get; set; }

        public string ResolutionNumber { get; set; }
        public string ResolutionUrl { get; set; }
        public string FileUrl { get; set; }

        public Guid DependencyId { get; set; }
        public Dependency Dependency { get; set; }

        public Guid CareerId { get; set; }
        public Career Career { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}