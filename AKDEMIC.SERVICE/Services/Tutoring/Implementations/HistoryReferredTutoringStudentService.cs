using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class HistoryReferredTutoringStudentService : IHistoryReferredTutoringStudentService
    {
        private readonly IHistoryReferredTutoringStudentRepository _historyReferredTutoringStudentRepository;

        public HistoryReferredTutoringStudentService(IHistoryReferredTutoringStudentRepository historyReferredTutoringStudentRepository)
        {
            _historyReferredTutoringStudentRepository = historyReferredTutoringStudentRepository;
        }

        public async Task InsertHistoryReferredTutoringStudent(HistoryReferredTutoringStudent historyReferredTutoringStudent) =>
            await _historyReferredTutoringStudentRepository.Insert(historyReferredTutoringStudent);

        public async Task UpdateHistoryReferredTutoringStudent(HistoryReferredTutoringStudent historyReferredTutoringStudent) =>
            await _historyReferredTutoringStudentRepository.Update(historyReferredTutoringStudent);

        public async Task DeleteHistoryReferredTutoringStudent(HistoryReferredTutoringStudent historyReferredTutoringStudent) =>
            await _historyReferredTutoringStudentRepository.Delete(historyReferredTutoringStudent);

        public async Task<HistoryReferredTutoringStudent> GetHistoryReferredTutoringStudentById(Guid id) =>
            await _historyReferredTutoringStudentRepository.Get(id);

        public async Task<IEnumerable<HistoryReferredTutoringStudent>> GetAllHistoryReferredTutoringStudents() =>
            await _historyReferredTutoringStudentRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<HistoryReferredTutoringStudent>> GetAllHistoryReferredTutoringStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string userId, Guid tutoringSessionId, Guid tutoringId) =>
            await _historyReferredTutoringStudentRepository.GetAllHistoryReferredTutoringStudentDatatable(sentParameters, termId, userId, tutoringSessionId, tutoringId);
        public async Task<HistoryReferredTutoringStudent> GetWithData(Guid id)
                => await _historyReferredTutoringStudentRepository.GetWithData(id);
    }
}
