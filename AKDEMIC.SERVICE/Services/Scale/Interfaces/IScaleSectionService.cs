using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Scale.Entities;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IScaleSectionService
    {
        Task<bool> Any(Guid id);
        Task<IEnumerable<ScaleSection>> GetAll();
        Task<ScaleSection> Get(Guid scaleSectionId);
        Task<ScaleSection> GetScaleSectionByNumber(byte sectionNumber);
        Task<Tuple<int, List<Tuple<string, int>>>> GetScaleSectionQuantityReportByPaginationParameters(PaginationParameter paginationParameter);
        Task<List<Tuple<string, int>>> GetScaleSectionQuantityReport(string search);
    }
}
