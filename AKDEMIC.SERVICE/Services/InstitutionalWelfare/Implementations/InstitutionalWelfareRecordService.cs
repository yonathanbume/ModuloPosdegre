using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareRecordService : IInstitutionalWelfareRecordService
    {
        private readonly IInstitutionalWelfareRecordRepository _institutionalWelfareRecordRepository;

        public InstitutionalWelfareRecordService(IInstitutionalWelfareRecordRepository institutionalWelfareRecordRepository)
        {
            _institutionalWelfareRecordRepository = institutionalWelfareRecordRepository;
        }

        public async Task Delete(InstitutionalWelfareRecord entity)
        {
            await _institutionalWelfareRecordRepository.Delete(entity);
        }

        public async Task<bool> ExistRecordWithTerm(Guid termId)
        {
            return await _institutionalWelfareRecordRepository.ExistRecordWithTerm(termId);
        }

        public async Task<InstitutionalWelfareRecord> Get(Guid id)
        {
            return await _institutionalWelfareRecordRepository.Get(id);
        }

        public async Task<InstitutionalWelfareRecord> GetActive()
        {
            return await _institutionalWelfareRecordRepository.GetActive();
        }

        public async Task<IEnumerable<InstitutionalWelfareRecord>> GetAll()
        {
            return await _institutionalWelfareRecordRepository.GetAll();
            }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareRecordDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _institutionalWelfareRecordRepository.GetInstitutionalWelfareRecordDatatable(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid recordId, Guid termId, bool ToEvaluate = false, string searchValue = null)
        {
            return await _institutionalWelfareRecordRepository.GetStudentDetailDatatable(sentParameters, recordId, termId, ToEvaluate, searchValue);
        }

        public async Task<InstitutionalWelfareRecord> GetWithIncludes(Guid recordId)
        {
            return await _institutionalWelfareRecordRepository.GetWithIncludes(recordId);
        }

        public async Task<bool> HaveAnswerStudents(Guid recordId)
        {
            return await _institutionalWelfareRecordRepository.HaveAnswerStudents(recordId);
        }

        public async Task Insert(InstitutionalWelfareRecord model)
        {
            await _institutionalWelfareRecordRepository.Insert(model);
        }

        public async Task<List<StudentTextAnswerTemplate>> StudentTextAnswersByRecord(Guid recordId, Guid studentId, Guid termId)
        {
            return await _institutionalWelfareRecordRepository.StudentTextAnswersByRecord(recordId, studentId, termId);
        }

        public async Task Update(InstitutionalWelfareRecord entity)
        {
            await _institutionalWelfareRecordRepository.Update(entity);
        }
    }
}
