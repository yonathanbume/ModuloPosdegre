using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface IRegisterCulturalActivityService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetParticipantsByCulturalActivityIdDataTable(DataTablesStructs.SentParameters parameters, Guid id,string search);
        Task<bool> IsRegistered(Guid activityId ,string dni,bool isId=false);
        Task Insert(RegisterCulturalActivity register);
        Task<IEnumerable<MemberRegister>> GetParticipantsByCulturalActivityId(Guid id);
    }
}
