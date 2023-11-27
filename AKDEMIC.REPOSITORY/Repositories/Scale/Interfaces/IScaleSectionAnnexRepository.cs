using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleSectionAnnexRepository : IRepository<ScaleSectionAnnex>
    {
        Task<int> GetScaleSectionAnnexesQuantity(string userId, Guid sectionId);
        Task<List<ScaleSectionAnnexTemplate>> GetScaleSectionAnnexesByPaginationParameters(string userId, Guid sectionId, PaginationParameter paginationParameter);
    }
}
