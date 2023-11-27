using AKDEMIC.ENTITIES.Models.Generals;
using System.Collections.Generic;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IPerformanceInfoService
    {
        List<PerformanceInfo> GetPerformanceInfo();
    }
}
