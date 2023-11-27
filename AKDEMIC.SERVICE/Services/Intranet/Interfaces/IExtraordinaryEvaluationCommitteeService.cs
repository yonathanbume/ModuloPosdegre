using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExtraordinaryEvaluationCommitteeService
    {
        Task<IEnumerable<ExtraordinaryEvaluationCommittee>> GetCommittee(Guid extraordinaryEvalutionId);
        Task DeleteRange(IEnumerable<ExtraordinaryEvaluationCommittee> entities);
        Task DeleteCommitteeByEvalutionId(Guid extraordinaryEvalutionId);
        Task InsertRange(IEnumerable<ExtraordinaryEvaluationCommittee> entities);
    }

}
