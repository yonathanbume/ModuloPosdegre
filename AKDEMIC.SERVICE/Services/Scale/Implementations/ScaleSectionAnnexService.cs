using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleSectionAnnexService : IScaleSectionAnnexService
    {
        private readonly IScaleSectionAnnexRepository _scaleSectionAnnexRepository;

        public ScaleSectionAnnexService(IScaleSectionAnnexRepository scaleSectionAnnexRepository)
        {
            _scaleSectionAnnexRepository = scaleSectionAnnexRepository;
        }

        public async Task<ScaleSectionAnnex> Get(Guid scaleSectionAnnexId)
        {
            return await _scaleSectionAnnexRepository.Get(scaleSectionAnnexId);
        }

        public async Task Insert(ScaleSectionAnnex scaleSectionAnnex)
        {
            await _scaleSectionAnnexRepository.Insert(scaleSectionAnnex);
        }

        public async Task Update(ScaleSectionAnnex scaleSectionAnnex)
        {
            await _scaleSectionAnnexRepository.Update(scaleSectionAnnex);
        }

        public async Task Delete(ScaleSectionAnnex scaleSectionAnnex)
        {
            await _scaleSectionAnnexRepository.Delete(scaleSectionAnnex);
        }

        public async Task<int> GetScaleSectionAnnexesQuantity(string userId, Guid sectionId)
        {
            return await _scaleSectionAnnexRepository.GetScaleSectionAnnexesQuantity(userId, sectionId);
        }

        public async Task<List<ScaleSectionAnnexTemplate>> GetScaleSectionAnnexesByPaginationParameters(string userId, Guid sectionId, PaginationParameter paginationParameter)
        {
            return await _scaleSectionAnnexRepository.GetScaleSectionAnnexesByPaginationParameters(userId, sectionId, paginationParameter);
        }
    }
}
