using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareSectionService: IInstitutionalWelfareSectionService
    {
        private readonly IInstitutionalWelfareSectionRepository _institutionalWelfareSectionRepository;

        public InstitutionalWelfareSectionService(IInstitutionalWelfareSectionRepository institutionalWelfareSectionRepository)
        {
            _institutionalWelfareSectionRepository = institutionalWelfareSectionRepository;
        }

        public async Task<bool> AnyByTitle(Guid recordId, string title, Guid? ignoredId = null)
        {
            return await _institutionalWelfareSectionRepository.AnyByTitle(recordId, title, ignoredId);
        }

        public async Task Delete(InstitutionalWelfareSection entity)
        {
            await _institutionalWelfareSectionRepository.Delete(entity);
        }

        public async Task DeleteRange(IEnumerable<InstitutionalWelfareSection> entites)
        {
            await _institutionalWelfareSectionRepository.DeleteRange(entites);
        }

        public async Task<InstitutionalWelfareSection> Get(Guid id)
        {
            return await _institutionalWelfareSectionRepository.Get(id);
        }

        public async Task<IEnumerable<InstitutionalWelfareSection>> GetDetailsByRecordId(Guid recordId)
        {
            return await _institutionalWelfareSectionRepository.GetDetailsByRecordId(recordId);
        }

        public async Task<IEnumerable<InstitutionalWelfareSection>> GetInstitutionalWelfareSectionsByRecordId(Guid institutionalWelfareRecordId, byte? sisfohClasification = 0, Guid? categorizationLevelId = null, Guid? careerId = null)
        {
            return await _institutionalWelfareSectionRepository.GetInstitutionalWelfareSectionsByRecordId(institutionalWelfareRecordId, sisfohClasification, categorizationLevelId, careerId);
        }

        public async Task<InstitutionalWelfareSection> GetWithIncludes(Guid id)
        {
            return await _institutionalWelfareSectionRepository.GetWithIncludes(id);
        }

        public async Task Insert(InstitutionalWelfareSection entity)
        {
            await _institutionalWelfareSectionRepository.Insert(entity);
        }

        public async Task Update(InstitutionalWelfareSection entity)
        {
            await _institutionalWelfareSectionRepository.Update(entity);
        }
    }
}
