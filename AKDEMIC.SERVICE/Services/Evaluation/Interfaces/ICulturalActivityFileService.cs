using AKDEMIC.ENTITIES.Models.Evaluation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface ICulturalActivityFileService
    {
        Task<IEnumerable<CulturalActivityFile>> GetFilesByActivity(Guid id);
        Task DeleteRange(IEnumerable<CulturalActivityFile> entities);
        Task<CulturalActivityFile> Get(Guid id);
    }
}
