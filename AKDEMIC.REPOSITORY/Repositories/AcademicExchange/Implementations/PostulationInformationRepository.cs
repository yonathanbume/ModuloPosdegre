using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;

namespace AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Implementations
{
    public class PostulationInformationRepository : Repository<PostulationInformation>, IPostulationInformationRepository
    {
        public PostulationInformationRepository(AkdemicContext context) : base(context) { }
    }
}
