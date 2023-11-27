using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeachingLoadType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ITeachingLoadTypeService
    {
        Task<IEnumerable<TeachingLoadType>> GetAll(int? category = null);
        Task<IEnumerable<Select2Structs.Result>> GetSelect2ClientSide(int? category = null ,bool? enabled = null);
        Task<TeachingLoadType> Get(Guid id);
        Task<TeachingLoadType> GetByName(string name, int? category = null);
        Task Insert(TeachingLoadType teachingLoadType);
        Task Update(TeachingLoadType teachingLoadType);
        Task Delete(TeachingLoadType teachingLoadType);
        Task DeleteById(Guid id);
        Task<bool> AnyNonTeachingLoadByTerm(Guid teachingLoadTypeId, Guid termId);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachingLoadTypeDatatable(DataTablesStructs.SentParameters parameters, int? category);
        Task<List<TeachingLoadTypeReport>> GetTeacherLoadTypeReportData(Guid? teachingLoadTypeId);
        Task<List<TeachingLoadTypeReportV2Template>> GetTeacherLoadTypeReportDatatableTemplate(Guid? termId, Guid? teachingLoadTypeId,Guid? academicDepartmentId);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherLoadTypeReportDatatable(DataTablesStructs.SentParameters parameters, Guid? termId ,Guid? teachingLoadTypeId, Guid? academicDepartmentId ,string searchValue);
    }
}
