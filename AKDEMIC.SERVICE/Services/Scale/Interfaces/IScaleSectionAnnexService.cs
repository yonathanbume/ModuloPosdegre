using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleSectionAnnexService
    {
        Task<ScaleSectionAnnex> Get(Guid scaleSectionAnnexId);
        Task Insert(ScaleSectionAnnex scaleSectionAnnex);
        Task Update(ScaleSectionAnnex scaleSectionAnnex);
        Task Delete(ScaleSectionAnnex scaleSectionAnnex);
        Task<int> GetScaleSectionAnnexesQuantity(string userId, Guid sectionId);
        Task<List<ScaleSectionAnnexTemplate>> GetScaleSectionAnnexesByPaginationParameters(string userId, Guid sectionId, PaginationParameter paginationParameter);
    }
}
