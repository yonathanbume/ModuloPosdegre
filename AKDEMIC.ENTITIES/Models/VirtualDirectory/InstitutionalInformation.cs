using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;

namespace AKDEMIC.ENTITIES.Models.VirtualDirectory
{
    public class InstitutionalInformation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte Type { get; set; }

        public DateTime PublishDate { get; set; }

        public string FileUrl { get; set; }


        public Guid DependencyId { get; set; }

        public virtual Dependency Dependency { get; set; }


    }
}
