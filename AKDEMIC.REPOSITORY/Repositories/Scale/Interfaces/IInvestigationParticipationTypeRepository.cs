using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IInvestigationParticipationTypeRepository:IRepository<InvestigationParticipationType>
    {
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<DataTablesStructs.ReturnedData<object>> GetAllParticipationTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
