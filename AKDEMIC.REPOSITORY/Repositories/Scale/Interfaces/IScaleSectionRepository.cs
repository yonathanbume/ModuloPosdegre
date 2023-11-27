using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces
{
    public interface IScaleSectionRepository : IRepository<ScaleSection>
    {
        Task<ScaleSection> GetScaleSectionByNumber(byte sectionNumber);
        Task<Tuple<int, List<Tuple<string, int>>>> GetScaleSectionQuantityReportByPaginationParameters(PaginationParameter paginationParameter);
        Task<List<Tuple<string, int>>> GetScaleSectionQuantityReport(string search);
    }
}
