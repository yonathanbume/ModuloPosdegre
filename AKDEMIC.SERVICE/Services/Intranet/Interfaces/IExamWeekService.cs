using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IExamWeekService
    {
        Task Insert(ExamWeek entity);
        Task<DataTablesStructs.ReturnedData<object>> GetExamWeekDatatable(DataTablesStructs.SentParameters parameters);
        Task<bool> AnyByTermAndType(Guid termId, byte type);
    }
}
