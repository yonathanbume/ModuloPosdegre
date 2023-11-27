using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IGradeService
    {
        Task<int> CountByFilter(Guid? evaluationId = null);
        Task<IEnumerable<Grade>> GetAll();
        Task<IEnumerable<Grade>> GetAll(Guid? studentSectionId = null, Guid? studentId = null, Guid? sectionId = null);
        Task<Grade> Get(Guid id);
        Task Insert(Grade grade);
        Task Update(Grade grade);
        Task DeleteById(Grade id);
        Task<List<Grade>> GetGradesByStudentSectionId(Guid studentSectionId);
        Task<List<Grade>> GetGradesByStudentAndTerm(Guid studentId, Guid termId);
        Task<List<Grade>> GetGradesBySectionId(Guid sectionId);
        Task<Select2Structs.ResponseParameters> GetStudentsBySectionAndEvaluation(Select2Structs.RequestParameters requestParameters, Guid sectionId, Guid? evaluationId = null, string searchValue = null);
        Task<object> GetStudentGradesDatatable(Guid studentSectionId);
    }
}