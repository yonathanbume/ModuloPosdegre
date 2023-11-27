using AKDEMIC.ENTITIES.Models.Generals;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IPerformanceInfoRepository
    {
        List<PerformanceInfo> GetPerformanceInfo();
    }
}
