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
    public class InstitutionalWelfareQuestionService : IInstitutionalWelfareQuestionService
    {
        private readonly IInstitutionalWelfareQuestionRepository _institutionalWelfareQuestionRepository;

        public InstitutionalWelfareQuestionService(IInstitutionalWelfareQuestionRepository institutionalWelfareQuestionRepository)
        {
            _institutionalWelfareQuestionRepository = institutionalWelfareQuestionRepository;
        }

        public async Task<bool> AnyByDescription(Guid sectionId, string description, Guid? ignoredId = null)
        {
            return await _institutionalWelfareQuestionRepository.AnyByDescription(sectionId, description, ignoredId);
        }

        public async Task Delete(InstitutionalWelfareQuestion entity)
        {
            await _institutionalWelfareQuestionRepository.Delete(entity);
        }

        public async Task DeleteRange(IEnumerable<InstitutionalWelfareQuestion> entities)
        {
            await _institutionalWelfareQuestionRepository.DeleteRange(entities);
        }

        public async Task<InstitutionalWelfareQuestion> Get(Guid id)
        {
            return await _institutionalWelfareQuestionRepository.Get(id);
        }

        public async Task<IEnumerable<InstitutionalWelfareQuestion>> GetAllByRecordId(Guid recordId)
        {
            return await _institutionalWelfareQuestionRepository.GetAllByRecordId(recordId);
        }

        public async Task<IEnumerable<InstitutionalWelfareQuestion>> GetAllBySectionId(Guid sectionId)
        {
            return await _institutionalWelfareQuestionRepository.GetAllBySectionId(sectionId);
        }

        public async Task<List<RecordQuestionExcelTemplate>> GetQuestionForExcelByRecord(Guid recordId)
        {
            return await _institutionalWelfareQuestionRepository.GetQuestionForExcelByRecord(recordId);
        }

        public async Task<InstitutionalWelfareQuestion> GetWithIncludes(Guid id)
        {
            return await _institutionalWelfareQuestionRepository.GetWithIncludes(id);
        }

        //public async Task<InstitutionalWelfareQuestion> GetByDescriptionAndSectionTitle(string description, string sectionTitle, Guid scholarshipId)
        //{
        //    return await _institutionalWelfareQuestionRepository.GetByDescriptionAndSectionTitle(description, sectionTitle, scholarshipId);
        //}

        public async Task Insert(InstitutionalWelfareQuestion entity)
        {
            await _institutionalWelfareQuestionRepository.Insert(entity);
        }

        public async Task Update(InstitutionalWelfareQuestion entity)
        {
            await _institutionalWelfareQuestionRepository.Update(entity);
        }
    }
}
