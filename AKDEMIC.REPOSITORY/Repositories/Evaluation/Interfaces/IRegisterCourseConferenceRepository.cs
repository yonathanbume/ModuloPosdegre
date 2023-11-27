using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces
{
    public interface IRegisterCourseConferenceRepository : IRepository<RegisterCourseConference>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetRegisterConferenceDataTable(DataTablesStructs.SentParameters parameters, Guid id, string search);
        Task<bool> IsRegistered(Guid couseConferenceId, string dni, bool isId = false);
        Task<IEnumerable<MemberRegister>> GetRegisterConference(Guid id);
    }
}
