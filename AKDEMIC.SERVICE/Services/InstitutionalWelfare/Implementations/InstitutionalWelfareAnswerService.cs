using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareAnswerService : IInstitutionalWelfareAnswerService
    {
        private readonly IInstitutionalWelfareAnswerRepository _institutionalWelfareAnswerRepository;

        public InstitutionalWelfareAnswerService(IInstitutionalWelfareAnswerRepository institutionalWelfareAnswerRepository)
        {
            _institutionalWelfareAnswerRepository = institutionalWelfareAnswerRepository;
        }

        public async Task Delete(InstitutionalWelfareAnswer entity)
        {
            await _institutionalWelfareAnswerRepository.Delete(entity);
        }

        public async Task DeleteRange(IEnumerable<InstitutionalWelfareAnswer> entites)
        {
            await _institutionalWelfareAnswerRepository.DeleteRange(entites);
        }

        public async Task<InstitutionalWelfareAnswer> Get(Guid selection)
        {
            return await _institutionalWelfareAnswerRepository.Get(selection);
        }

        public async Task<IEnumerable<InstitutionalWelfareAnswer>> GetAllByQuestionId(Guid questionId)
        {
            return await _institutionalWelfareAnswerRepository.GetAllByQuestionId(questionId);
        }

        public async Task<IEnumerable<InstitutionalWelfareAnswer>> GetAllBySectionId(Guid sectionId)
        {
            return await _institutionalWelfareAnswerRepository.GetAllBySectionId(sectionId);
        }
    }
}
