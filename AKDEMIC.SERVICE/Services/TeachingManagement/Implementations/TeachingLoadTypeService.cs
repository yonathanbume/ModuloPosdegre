using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeachingLoadType;
using AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Implementations
{
    public class TeachingLoadTypeService : ITeachingLoadTypeService
    {
        private readonly ITeachingLoadTypeRepository _teachingLoadTypeRepository;

        public TeachingLoadTypeService(ITeachingLoadTypeRepository teachingLoadTypeRepository)
        {
            _teachingLoadTypeRepository = teachingLoadTypeRepository; 
        }

        public Task<TeachingLoadType> GetByName(string name, int? category = null)
            => _teachingLoadTypeRepository.GetByName(name, category);

        public Task Delete(TeachingLoadType teachingLoadType)
            => _teachingLoadTypeRepository.Delete(teachingLoadType);

        public Task DeleteById(Guid id)
            => _teachingLoadTypeRepository.DeleteById(id);

        public Task<TeachingLoadType> Get(Guid id)
            => _teachingLoadTypeRepository.Get(id);

        public Task<IEnumerable<TeachingLoadType>> GetAll(int? category = null)
            => _teachingLoadTypeRepository.GetAll(category);

        public Task<IEnumerable<Select2Structs.Result>> GetSelect2ClientSide(int? category = null, bool? enabled = null)
            => _teachingLoadTypeRepository.GetSelect2ClientSide(category, enabled);

        public Task Insert(TeachingLoadType teachingLoadType)
            => _teachingLoadTypeRepository.Insert(teachingLoadType);

        public Task Update(TeachingLoadType teachingLoadType)
            => _teachingLoadTypeRepository.Update(teachingLoadType);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeachingLoadTypeDatatable(DataTablesStructs.SentParameters parameters, int? category)
            => await _teachingLoadTypeRepository.GetTeachingLoadTypeDatatable(parameters, category);

        public async Task<DataTablesStructs.ReturnedData<object>> GetTeacherLoadTypeReportDatatable(DataTablesStructs.SentParameters parameters, Guid? termId ,Guid? teachingLoadTypeId, Guid? academicDeparmentId ,string searchValue)
            => await _teachingLoadTypeRepository.GetTeacherLoadTypeReportDatatable(parameters, termId, teachingLoadTypeId, academicDeparmentId,searchValue);

        public async Task<List<TeachingLoadTypeReport>> GetTeacherLoadTypeReportData(Guid? teachingLoadTypeId)
            => await _teachingLoadTypeRepository.GetTeacherLoadTypeReportData(teachingLoadTypeId);

        public async Task<List<TeachingLoadTypeReportV2Template>> GetTeacherLoadTypeReportDatatableTemplate(Guid? termId, Guid? teachingLoadTypeId,Guid? academicDepartmentId)
            => await _teachingLoadTypeRepository.GetTeacherLoadTypeReportDatatableTemplate(termId, teachingLoadTypeId,academicDepartmentId);

        public async Task<bool> AnyNonTeachingLoadByTerm(Guid teachingLoadTypeId, Guid termId)
            => await _teachingLoadTypeRepository.AnyNonTeachingLoadByTerm(teachingLoadTypeId, termId);
    }
}
