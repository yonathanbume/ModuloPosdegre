using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface IRegisterCulturalActivityRepository : IRepository<RegisterCulturalActivity>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetParticipantsByCulturalActivityIdDataTable(DataTablesStructs.SentParameters parameters, Guid id, string search);
        Task<bool> IsRegistered(Guid activityId ,string dni,bool IsId);
        Task<IEnumerable<MemberRegister>> GetParticipantsByCulturalActivityId(Guid id);
    }
}
