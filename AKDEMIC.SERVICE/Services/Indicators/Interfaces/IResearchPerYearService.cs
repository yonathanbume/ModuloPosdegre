using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Indicators.Interfaces
{
    public interface IResearchPerYearService
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters);
        Task Insert(ResearchPerYear researchPerYear);
        Task Update(ResearchPerYear researchPerYear);
        Task<ResearchPerYear> Get(Guid id);
        Task DeleteById(Guid id);
        Task<bool> AnyByYear(int year, Guid? id = null);
        Task<object> GetReportPieChart(int year);
    }
}
