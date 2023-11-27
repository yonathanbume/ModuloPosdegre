using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleSectionService : IScaleSectionService
    {
        private readonly IScaleSectionRepository _scaleSectionRepository;

        public ScaleSectionService(IScaleSectionRepository scaleSectionRepository)
        {
            _scaleSectionRepository = scaleSectionRepository;
        }

        public async Task<bool> Any(Guid id)
        {
            return await _scaleSectionRepository.Any(id);
        }

        public async Task<IEnumerable<ScaleSection>> GetAll()
        {
            return await _scaleSectionRepository.GetAll();
        }

        public async Task<ScaleSection> Get(Guid scaleSectionId)
        {
            return await _scaleSectionRepository.Get(scaleSectionId);
        }

        public async Task<ScaleSection> GetScaleSectionByNumber(byte sectionNumber)
        {
            return await _scaleSectionRepository.GetScaleSectionByNumber(sectionNumber);
        }

        public async Task<Tuple<int, List<Tuple<string, int>>>> GetScaleSectionQuantityReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            return await _scaleSectionRepository.GetScaleSectionQuantityReportByPaginationParameters(paginationParameter);
        }

        public async Task<List<Tuple<string, int>>> GetScaleSectionQuantityReport(string search)
        {
            return await _scaleSectionRepository.GetScaleSectionQuantityReport(search);
        }
    }
}
