using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class TeacherInformationRepository : Repository<TeacherInformation>, ITeacherInformationRepository
    {
        public TeacherInformationRepository(AkdemicContext context) : base(context) { }
    }
}