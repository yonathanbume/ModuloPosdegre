using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExtraordinaryEvaluationCommitteeRepository : IRepository<ExtraordinaryEvaluationCommittee>
    {
        Task<IEnumerable<ExtraordinaryEvaluationCommittee>> GetCommittee(Guid extraordinaryEvalutionId);
        Task DeleteCommitteeByEvalutionId(Guid extraordinaryEvalutionId);
    }
}
