using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class TeacherDedicationService : ITeacherDedicationService
    {
        private readonly ITeacherDedicationRepository _teacherDedicationRepository;

        public TeacherDedicationService(ITeacherDedicationRepository teacherDedicationRepository)
        {
            _teacherDedicationRepository = teacherDedicationRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDedicationDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _teacherDedicationRepository.GetAllDedicationDatatable(sentParameters,searchValue);

        public async Task<object> GetTeacherDedicationChart(Guid? id = null)
            => await _teacherDedicationRepository.GetTeacherDedicationChart(id);

        Task ITeacherDedicationService.DeleteAsync(TeacherDedication teacherDedication)
            => _teacherDedicationRepository.Delete(teacherDedication);

        Task<IEnumerable<TeacherDedication>> ITeacherDedicationService.GetAll()
            => _teacherDedicationRepository.GetAll();

        public async Task<IEnumerable<TeacherDedication>> GetAllWithIncludes(int? status = null)
            => await _teacherDedicationRepository.GetAllWithIncludes(status);

        public async Task<object> GetAllAsSelect2ClientSideAsync(bool includeTitle = false)
            => await _teacherDedicationRepository.GetAllAsSelect2ClientSideAsync(includeTitle);

        Task<TeacherDedication> ITeacherDedicationService.GetByIdAsync(Guid id)
            => _teacherDedicationRepository.Get(id);

        Task<double> ITeacherDedicationService.GetTeacherLessonsHours(string teacherId, Guid termId, TimeSpan startTime, TimeSpan endTime)
            => _teacherDedicationRepository.GetTeacherLessonsHours(teacherId, termId, startTime, endTime);

        Task ITeacherDedicationService.InsertAsync(TeacherDedication teacherDedication)
            => _teacherDedicationRepository.Insert(teacherDedication);

        Task ITeacherDedicationService.UpdateAsync(TeacherDedication teacherDedication)
            => _teacherDedicationRepository.Update(teacherDedication);

        public async Task<IEnumerable<TeacherDedication>> GetAll(string search, bool? onlyActive = false)
            => await _teacherDedicationRepository.GetAll(search, onlyActive);

        public Task<object> GetTeacherDedicationSelect()
            => _teacherDedicationRepository.GetTeacherDedicationSelect();

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherDedicationReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null)
            => await _teacherDedicationRepository.GetTeacherDedicationReportDatatable(sentParameters, termId, searchValue);
    }
}