using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.TeachingManagement.Interfaces
{
    public interface ITeacherPortfolioService
    {
        Task<object> GetPortfolioDatatable(DataTablesStructs.SentParameters parameters, string teacherId, byte folder, string search);
        Task<TeacherPortfolio> Get(Guid id);
        Task Insert(TeacherPortfolio entity);
        Task Delete(TeacherPortfolio entity);
        Task<object> GetPortfolioCurriculumDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search);
        Task<object> GetPortfolioCourseSyllabusDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search);

        Task<object> GetPortfolioCurricularDesignDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search);
    }
}
