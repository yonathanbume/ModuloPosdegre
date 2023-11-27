using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces
{
    public interface ITeacherPortfolioRepository : IRepository<TeacherPortfolio>
    {
        Task<object> GetPortfolioDatatable(DataTablesStructs.SentParameters parameters, string teacherId, byte folder, string search);
        Task<object> GetPortfolioCurriculumDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search);
        Task<object> GetPortfolioCourseSyllabusDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search);
        Task<object> GetPortfolioCurricularDesignDatatable(DataTablesStructs.SentParameters parameters, Guid termId, string teacherId, string search);
    }
}
