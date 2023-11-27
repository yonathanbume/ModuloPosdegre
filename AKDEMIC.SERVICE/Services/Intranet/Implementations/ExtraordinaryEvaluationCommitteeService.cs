using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ExtraordinaryEvaluationCommitteeService : IExtraordinaryEvaluationCommitteeService
    {
        private readonly IExtraordinaryEvaluationCommitteeRepository _extraordinaryEvaluationCommitteeRepository;

        public ExtraordinaryEvaluationCommitteeService(IExtraordinaryEvaluationCommitteeRepository extraordinaryEvaluationCommitteeRepository)
        {
            _extraordinaryEvaluationCommitteeRepository = extraordinaryEvaluationCommitteeRepository;
        }

        public async Task DeleteCommitteeByEvalutionId(Guid extraordinaryEvalutionId)
            => await _extraordinaryEvaluationCommitteeRepository.DeleteCommitteeByEvalutionId(extraordinaryEvalutionId);

        public async Task DeleteRange(IEnumerable<ExtraordinaryEvaluationCommittee> entities)
            => await _extraordinaryEvaluationCommitteeRepository.DeleteRange(entities);

        public async Task<IEnumerable<ExtraordinaryEvaluationCommittee>> GetCommittee(Guid extraordinaryEvalutionId)
            => await _extraordinaryEvaluationCommitteeRepository.GetCommittee(extraordinaryEvalutionId);

        public async Task InsertRange(IEnumerable<ExtraordinaryEvaluationCommittee> entities)
            => await _extraordinaryEvaluationCommitteeRepository.InsertRange(entities);
    }
}
