using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ITeacherTermInformRepository : IRepository<TeacherTermInform>
    {
        Task<object> GetChartReportDataByStateCourseTermAndTerm(int? state, Guid? termId, Guid? courseTermId);
        Task<object> GetChartReportDatatableByStateCourseTermAndTerm(int? state, Guid? termId, Guid? courseTermId);
        Task<DataTablesStructs.ReturnedData<object>> GetTermInformsDatatable(DataTablesStructs.SentParameters parameters, int? type = null, ClaimsPrincipal user = null);
        Task<object> GetTermInformsChart(int? type = null, ClaimsPrincipal user = null);
    }
}