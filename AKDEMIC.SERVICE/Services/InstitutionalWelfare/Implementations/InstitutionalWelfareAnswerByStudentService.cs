using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareAnswerByStudentService : IInstitutionalWelfareAnswerByStudentService
    {
        private readonly IInstitutionalWelfareAnswerByStudentRepository _institutionalWelfareAnswerByStudentRepository;

        public InstitutionalWelfareAnswerByStudentService(IInstitutionalWelfareAnswerByStudentRepository institutionalWelfareAnswerByStudentRepository)
        {
            _institutionalWelfareAnswerByStudentRepository = institutionalWelfareAnswerByStudentRepository;
        }

        public async Task Delete(InstitutionalWelfareAnswerByStudent institutionalWelfareAnswerByStudent)
        {
            await _institutionalWelfareAnswerByStudentRepository.Delete(institutionalWelfareAnswerByStudent);
        }

        public async Task<bool> ExistAnswerByStudent(Guid recordId, Guid studentId, Guid termId)
        {
           var result = await _institutionalWelfareAnswerByStudentRepository.ExistAnswerByStudent(recordId, studentId, termId);
            return result;
        }

        public async Task<bool> ExistAnswerByUserName(Guid recordId, string userName, Guid termId)
        {
            return await _institutionalWelfareAnswerByStudentRepository.ExistAnswerByUserName(recordId, userName, termId);
        }

        public async Task<InstitutionalWelfareAnswerByStudent> Get(Guid id)
        {
            return await _institutionalWelfareAnswerByStudentRepository.Get(id);
        }

        public async Task<List<RecordUserReportTemplate>> GetUserAnswersByRecord(Guid recordId, Guid termId)
        {
            return await _institutionalWelfareAnswerByStudentRepository.GetUserAnswersByRecord(recordId , termId);
        }

        public async Task Insert(InstitutionalWelfareAnswerByStudent institutionalWelfareAnswerByStudent)
        {
            await _institutionalWelfareAnswerByStudentRepository.Insert(institutionalWelfareAnswerByStudent);
        }

        public async Task InsertRange(List<InstitutionalWelfareAnswerByStudent> answers)
        {
            await _institutionalWelfareAnswerByStudentRepository.InsertRange(answers);
        }

        public async Task Update(InstitutionalWelfareAnswerByStudent institutionalWelfareAnswerByStudent)
        {
            await _institutionalWelfareAnswerByStudentRepository.Update(institutionalWelfareAnswerByStudent);
        }
    }
}
