using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface IHistoryReferredTutoringStudentRepository : IRepository<HistoryReferredTutoringStudent>
    {
        Task<DataTablesStructs.ReturnedData<HistoryReferredTutoringStudent>> GetAllHistoryReferredTutoringStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string userId, Guid tutoringSessionId, Guid tutoringId);
        Task<HistoryReferredTutoringStudent> GetWithData(Guid id);
    }
}
