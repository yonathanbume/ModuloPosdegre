using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;

namespace AKDEMIC.SERVICE.Services.TransparencyPortal.Interfaces
{
    public interface ITransparencyCompetitionService
    {
        Task Insert(TransparencyCompetition regulation);
        Task<TransparencyCompetition> Get(Guid id);
        Task Update(TransparencyCompetition regulation);
        Task DeleteById(Guid id);
        Task<IEnumerable<TransparencyCompetition>> GetAll();
        Task<DataTablesStructs.ReturnedData<object>> GetListCompetition(DataTablesStructs.SentParameters paginationParameter, int year);
    }
}
