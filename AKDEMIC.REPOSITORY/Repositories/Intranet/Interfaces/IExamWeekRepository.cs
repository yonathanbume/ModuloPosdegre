using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IExamWeekRepository : IRepository<ExamWeek>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetExamWeekDatatable(DataTablesStructs.SentParameters parameters);
        Task<bool> AnyByTermAndType(Guid termId, byte type);
    }
}
