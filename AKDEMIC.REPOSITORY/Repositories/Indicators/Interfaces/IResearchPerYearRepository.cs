using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Indicators;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Indicators.Interfaces
{
    public interface IResearchPerYearRepository : IRepository<ResearchPerYear>
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters);
        Task<bool> AnyByYear(int year, Guid? id = null);
        Task<object> GetReportPieChart(int year);
    }
}
