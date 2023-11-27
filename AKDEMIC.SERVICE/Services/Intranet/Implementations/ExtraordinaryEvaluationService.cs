using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ExtraordinaryEvaluationService : IExtraordinaryEvaluationService
    {
        private readonly IExtraordinaryEvaluationRepository _extraordinaryEvaluationRepository;

        public ExtraordinaryEvaluationService(IExtraordinaryEvaluationRepository extraordinaryEvaluationRepository)
        {
            _extraordinaryEvaluationRepository = extraordinaryEvaluationRepository;
        }

        public async Task<bool> AnyByCourseAndTermId(Guid courseId, Guid termId)
            => await _extraordinaryEvaluationRepository.AnyByCourseAndTermId(courseId, termId);

        public async Task Delete(ExtraordinaryEvaluation entity)
            => await _extraordinaryEvaluationRepository.Delete(entity);

        public async Task<ExtraordinaryEvaluation> Get(Guid id)
            => await _extraordinaryEvaluationRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationsDatatable(DataTablesStructs.SentParameters parameters, string searchValue, Guid? careerId, string teacherId, Guid? termId, ClaimsPrincipal user,bool? toEvaluationReport = null, byte? type = null)
            => await _extraordinaryEvaluationRepository.GetExtraordinaryEvaluationsDatatable(parameters, searchValue, careerId, teacherId, termId, user, toEvaluationReport, type);


        public async Task<DataTablesStructs.ReturnedData<object>> GetExtraordinaryEvaluationsToTeacherDatatable(DataTablesStructs.SentParameters parameters, string searchValue, string teacherId)
            => await _extraordinaryEvaluationRepository.GetExtraordinaryEvaluationsToTeacherDatatable(parameters, searchValue, teacherId);

        public async Task Insert(ExtraordinaryEvaluation extraordinaryEvaluation)
            => await _extraordinaryEvaluationRepository.Insert(extraordinaryEvaluation);

        public async Task Update(ExtraordinaryEvaluation extraordinaryEvaluation)
            => await _extraordinaryEvaluationRepository.Update(extraordinaryEvaluation);
    }
}
