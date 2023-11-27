using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyCompetitionFilesService
    {
        Task Insert(TransparencyCompetitionFile regulation);
        Task<TransparencyCompetitionFile> Get(Guid id);
        Task Update(TransparencyCompetitionFile regulation);
        Task DeleteById(Guid id);
        Task<List<TransparencyCompetitionFile>> GetByTransparencyCompetitionId(Guid transparencyCompetitionId);
    }
}
