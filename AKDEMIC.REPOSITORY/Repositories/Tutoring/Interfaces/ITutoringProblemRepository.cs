using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces
{
    public interface ITutoringProblemRepository : IRepository<TutoringProblem>
    {
        Task<DataTablesStructs.ReturnedData<TutoringProblem>> GetTutoringProblemsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, byte? category = null);
        Task<TutoringProblem> FindByCode(string code);
        Task<IEnumerable<TutoringProblem>> GetAllByCategory(byte category);
        Task<IEnumerable<TutoringProblem>> GetAllByCategoryNu(byte? category = null, string search = null);
        Task<DataTablesStructs.ReturnedData<TutoringProblem>> GetTutoringProblemsDatatableByTutoring(DataTablesStructs.SentParameters sentParameters, Guid tutoringId, string searchValue = null, byte? category = null);
    }
}
