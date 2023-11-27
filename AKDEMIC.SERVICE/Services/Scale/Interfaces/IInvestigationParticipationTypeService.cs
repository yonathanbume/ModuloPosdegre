using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IInvestigationParticipationTypeService
    {
        Task<InvestigationParticipationType> Get(Guid id);
        Task<bool> AnyByName(string name, Guid? id = null);
        Task<IEnumerable<InvestigationParticipationType>> GetAll();
        Task Insert(InvestigationParticipationType investigationParticipationType);
        Task Update(InvestigationParticipationType investigationParticipationType);
        Task Delete(InvestigationParticipationType investigationParticipationType);

        Task<DataTablesStructs.ReturnedData<object>> GetAllParticipationTypeDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
