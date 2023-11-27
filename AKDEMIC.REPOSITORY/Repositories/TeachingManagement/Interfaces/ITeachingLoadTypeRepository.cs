using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeachingLoadType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ITeachingLoadTypeRepository : IRepository<TeachingLoadType>
    {
        Task<IEnumerable<Select2Structs.Result>> GetSelect2ClientSide(int? category = null, bool? enabled = null);
        Task<IEnumerable<TeachingLoadType>> GetAll(int? category = null);
        Task<TeachingLoadType> GetByName(string name, int? category = null);
        Task<DataTablesStructs.ReturnedData<object>> GetTeachingLoadTypeDatatable(DataTablesStructs.SentParameters parameters, int? category);
        Task<List<TeachingLoadTypeReport>> GetTeacherLoadTypeReportData(Guid? teachingLoadTypeId);
        Task<List<TeachingLoadTypeReportV2Template>> GetTeacherLoadTypeReportDatatableTemplate(Guid? termId, Guid? teachingLoadTypeId,Guid? academicDeparmentId);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherLoadTypeReportDatatable(DataTablesStructs.SentParameters parameters, Guid? termId ,Guid? teachingLoadTypeId, Guid? academicDepartmentId,string searchValue);
        Task<bool> AnyNonTeachingLoadByTerm(Guid teachingLoadTypeId, Guid termId);
    }
}
