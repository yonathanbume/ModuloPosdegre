using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class InstitutionalRecordCategorizationByStudentService : IInstitutionalRecordCategorizationByStudentService
    {
        private readonly IInstitutionalRecordCategorizationByStudentRepository _institutionalRecordCategorizationByStudentRepository;

        public InstitutionalRecordCategorizationByStudentService(IInstitutionalRecordCategorizationByStudentRepository institutionalRecordCategorizationByStudentRepository)
        {
            _institutionalRecordCategorizationByStudentRepository = institutionalRecordCategorizationByStudentRepository;
        }
        public async Task Delete(InstitutionalRecordCategorizationByStudent institutionalRecordCategorizationByStudent)
        {
            await _institutionalRecordCategorizationByStudentRepository.Delete(institutionalRecordCategorizationByStudent);
        }

        public async Task<InstitutionalRecordCategorizationByStudent> Get(Guid id)
        {
            return await _institutionalRecordCategorizationByStudentRepository.Get(id);
        }

        public async Task<InstitutionalRecordCategorizationByStudent> GetByStudentAndRecord(Guid recordId, Guid studentId)
        {
            return await _institutionalRecordCategorizationByStudentRepository.GetByStudentAndRecord(recordId, studentId);
        }

        public async Task<object> GetStudentReport(Guid id, Guid termId, byte? sisfohClasification = 0, Guid? categorizationLevelId = null, Guid? careerId = null)
        {
            return await _institutionalRecordCategorizationByStudentRepository.GetStudentReport(id,termId, sisfohClasification, categorizationLevelId, careerId);
        }

        public async Task Insert(InstitutionalRecordCategorizationByStudent institutionalRecordCategorizationByStudent)
        {
            await _institutionalRecordCategorizationByStudentRepository.Insert(institutionalRecordCategorizationByStudent);
        }

        public async Task Update(InstitutionalRecordCategorizationByStudent institutionalRecordCategorizationByStudent)
        {
            await _institutionalRecordCategorizationByStudentRepository.Update(institutionalRecordCategorizationByStudent);
        }
    }
}
