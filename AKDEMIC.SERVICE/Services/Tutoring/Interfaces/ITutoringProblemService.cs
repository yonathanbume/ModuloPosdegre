using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Interfaces
{
    public interface ITutoringProblemService
    {
        Task<DataTablesStructs.ReturnedData<TutoringProblem>> GetTutoringProblemsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, byte? category = null);
        Task<TutoringProblem> Get(Guid tutoringProblemId);
        Task Insert(TutoringProblem tutoringProblem);
        Task Update(TutoringProblem tutoringProblem);
        Task<IEnumerable<TutoringProblem>> GetAll();
        Task DeleteById(Guid tutoringProblemId);
        Task<IEnumerable<TutoringProblem>> GetAllByCategory(byte category);
        Task<IEnumerable<TutoringProblem>> GetAllByCategoryNu(byte? category = null, string search = null);
        Task<TutoringProblem> FindByCode(string code);
        Task<DataTablesStructs.ReturnedData<TutoringProblem>> GetTutoringProblemsDatatableByTutoring(DataTablesStructs.SentParameters sentParameters, Guid tutoringId, string searchValue = null, byte? category = null);
        
    }
}
