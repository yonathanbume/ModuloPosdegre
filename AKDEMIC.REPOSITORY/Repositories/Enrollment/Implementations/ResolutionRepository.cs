using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public sealed class ResolutionRepository : Repository<Resolution>, IResolutionRepository
    {
        public ResolutionRepository(AkdemicContext context) : base(context) { }
    }
}