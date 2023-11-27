using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ITeacherTermInformService
    {
        Task<TeacherTermInform> Get(Guid id);
        Task<object> GetChartReportDataByStateCourseTermAndTerm(int? state, Guid? termId, Guid? courseTermId);
        Task<DataTablesStructs.ReturnedData<object>> GetTermInformsDatatable(DataTablesStructs.SentParameters parameters, int? type = null, ClaimsPrincipal user = null);
        Task<object> GetTermInformsChart(int? type = null, ClaimsPrincipal user = null);
        Task<object> GetChartReportDatatableByStateCourseTermAndTerm(int? state, Guid? termId, Guid? courseTermId);
        Task InsertAsync(TeacherTermInform teacherTermInform);
        Task Delete(TeacherTermInform entity);
    }
}