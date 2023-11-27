using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Indicators;
using AKDEMIC.REPOSITORY.Repositories.Indicators.Interfaces;
using AKDEMIC.SERVICE.Services.Indicators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Indicators.Implementations
{
    public class ResearchPerYearService : IResearchPerYearService
    {
        private readonly IResearchPerYearRepository _researchPerYearRepository;
        public ResearchPerYearService(IResearchPerYearRepository researchPerYearRepository)
        {
            _researchPerYearRepository = researchPerYearRepository;
        }

        public async Task<bool> AnyByYear(int year, Guid? id = null)
            => await _researchPerYearRepository.AnyByYear(year, id);

        public async Task DeleteById(Guid id)
            => await _researchPerYearRepository.DeleteById(id);

        public async Task<ResearchPerYear> Get(Guid id)
            => await _researchPerYearRepository.Get(id);

        public async Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters)
            => await _researchPerYearRepository.GetDatatable(sentParameters);

        public async Task<object> GetReportPieChart(int year)
            => await _researchPerYearRepository.GetReportPieChart(year);

        public async Task Insert(ResearchPerYear researchPerYear)
            => await _researchPerYearRepository.Insert(researchPerYear);

        public async Task Update(ResearchPerYear researchPerYear)
            => await _researchPerYearRepository.Update(researchPerYear);
    }
}
