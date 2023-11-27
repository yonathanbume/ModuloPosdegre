using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class GradeRecoveryService : IGradeRecoveryService
    {
        private readonly IGradeRecoveryRepository _gradeRecoveryRepository;

        public GradeRecoveryService(IGradeRecoveryRepository gradeRecoveryRepository)
        {
            _gradeRecoveryRepository = gradeRecoveryRepository;
        }

        public async Task Delete(GradeRecovery entity)
            => await _gradeRecoveryRepository.Delete(entity);

        public async Task DeleteRange(IEnumerable<GradeRecovery> entities)
            => await _gradeRecoveryRepository.DeleteRange(entities);

        public async Task<object> GetAssignedStudentsExecuted(Guid gradeRecoveryExamId)
            => await _gradeRecoveryRepository.GetAssignedStudentsExecuted(gradeRecoveryExamId);

        public async Task<object> GetAssignedStudentsWithData(Guid gradeRecoveryExamId)
            => await _gradeRecoveryRepository.GetAssignedStudentsWithData(gradeRecoveryExamId);

        public async Task<IEnumerable<GradeRecovery>> GetByGradeRecoveryExamId(Guid gradeRecoveryExamId)
            => await _gradeRecoveryRepository.GetByGradeRecoveryExamId(gradeRecoveryExamId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeRecoveryDatatable(DataTablesStructs.SentParameters parameters, Guid gradeRecoveryExamId, string searchValue)
            => await _gradeRecoveryRepository.GetGradeRecoveryDatatable(parameters, gradeRecoveryExamId, searchValue);

        public async Task Insert(GradeRecovery entity)
            => await _gradeRecoveryRepository.Insert(entity);

        public async Task InsertRange(IEnumerable<GradeRecovery> entity)
            => await _gradeRecoveryRepository.InsertRange(entity);

        public async Task Update(GradeRecovery entity)
            => await _gradeRecoveryRepository.Update(entity);

        public async Task Updaterange(IEnumerable<GradeRecovery> entities)
            => await _gradeRecoveryRepository.UpdateRange(entities);
    }
}
