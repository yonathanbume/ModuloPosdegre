using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IGradeRepository : IRepository<Grade>
    {
        Task<int> CountByFilter(Guid? evaluationId = null);
        Task<IEnumerable<Grade>> GetAll(Guid? studentSectionId = null, Guid? studentId = null, Guid? sectionId = null);
        Task<List<Grade>> GetGradesByStudentSectionId(Guid studentSectionId);
        Task<List<Grade>> GetGradesByStudentAndTerm(Guid studentId, Guid termId);
        Task<List<Grade>> GetGradesBySectionId(Guid sectionId);
        Task<Select2Structs.ResponseParameters> GetStudentsBySectionAndEvaluation(Select2Structs.RequestParameters requestParameters, Guid sectionId, Guid? evaluationId = null, string searchValue = null);
        Task<object> GetStudentGradesDatatable(Guid studentSectionId);
    }
}