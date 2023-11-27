using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface IHistoryReferredTutoringStudentService
    {
        Task InsertHistoryReferredTutoringStudent(HistoryReferredTutoringStudent historyReferredTutoringStudent);
        Task UpdateHistoryReferredTutoringStudent(HistoryReferredTutoringStudent historyReferredTutoringStudent);
        Task DeleteHistoryReferredTutoringStudent(HistoryReferredTutoringStudent historyReferredTutoringStudent);
        Task<HistoryReferredTutoringStudent> GetHistoryReferredTutoringStudentById(Guid id);
        Task<IEnumerable<HistoryReferredTutoringStudent>> GetAllHistoryReferredTutoringStudents();
        Task<DataTablesStructs.ReturnedData<HistoryReferredTutoringStudent>> GetAllHistoryReferredTutoringStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string userId, Guid tutoringSessionId, Guid tutoringId);
        Task<HistoryReferredTutoringStudent> GetWithData(Guid id);
    }
}
