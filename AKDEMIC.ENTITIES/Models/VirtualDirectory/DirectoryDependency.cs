using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;

namespace AKDEMIC.ENTITIES.Models.VirtualDirectory
{
    public class DirectoryDependency
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Annex { get; set; }

        public byte Charge { get; set; }

        public Guid DependencyId { get; set; }

        public virtual Dependency Dependency { get; set; }
    }
}
