using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface ICulturalActivityFileRepository : IRepository<CulturalActivityFile>
    {
        Task<IEnumerable<CulturalActivityFile>> GetFilesByActivity(Guid id);
    }
}
