using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserDependency
    {
        public Guid Id { get; set; }

        public Guid DependencyId { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public Dependency Dependency { get; set; }
    }
}
