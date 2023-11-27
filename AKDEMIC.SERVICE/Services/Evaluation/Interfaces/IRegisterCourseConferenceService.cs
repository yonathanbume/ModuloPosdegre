using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;

namespace AKDEMIC.SERVICE.Services.Evaluation.Interfaces
{
    public interface IRegisterCourseConferenceService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetRegisterConferenceDataTable(DataTablesStructs.SentParameters parameters, Guid id, string search);
        Task<bool> IsRegistered(Guid couseConferenceId , string dni,bool isId=false);
        Task Insert(RegisterCourseConference register);
        Task<IEnumerable<MemberRegister>> GetRegisterConference(Guid id);
    }
}
