using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System.Collections.Generic;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class PerformanceInfoService : IPerformanceInfoService
    {
        private readonly IPerformanceInfoRepository _performanceInfoRepository;

        public PerformanceInfoService(IPerformanceInfoRepository performanceInfoRepository)
        {
            _performanceInfoRepository = performanceInfoRepository;
        }

        public List<PerformanceInfo> GetPerformanceInfo()
        {
            return _performanceInfoRepository.GetPerformanceInfo();
        }
    }
}
