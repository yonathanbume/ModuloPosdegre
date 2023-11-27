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
    public class SubstituteExamCorrectionService : ISubstituteExamCorrectionService
    {
        private readonly ISubstituteExamCorrectionRepository _substituteExamCorrectionRepository;
        public SubstituteExamCorrectionService(ISubstituteExamCorrectionRepository substituteExamCorrectionRepository)
        {
            _substituteExamCorrectionRepository = substituteExamCorrectionRepository;
        }

        public Task DeleteById(Guid id)
            => _substituteExamCorrectionRepository.DeleteById(id);

        public Task<SubstituteExamCorrection> Get(Guid id)
            => _substituteExamCorrectionRepository.Get(id);

        public Task<IEnumerable<SubstituteExamCorrection>> GetAll(string teacherId = null, Guid? termId = null)
            => _substituteExamCorrectionRepository.GetAll(teacherId, termId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, string teacherId = null, Guid? termId = null, string searchValue = null, int? state = null)
            => await _substituteExamCorrectionRepository.GetAllDatatable(sentParameters, teacherId, termId, searchValue, state);

        public async Task<SubstituteExamCorrection> GetByTeacherStudent(string teacherId, Guid studentId)
            => await _substituteExamCorrectionRepository.GetByTeacherStudent(teacherId, studentId);

        public Task Insert(SubstituteExamCorrection substituteExamCorrection)
            => _substituteExamCorrectionRepository.Insert(substituteExamCorrection);

        public Task Update(SubstituteExamCorrection substituteExamCorrection)
            => _substituteExamCorrectionRepository.Update(substituteExamCorrection);
    }
}
