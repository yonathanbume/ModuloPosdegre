using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using AKDEMIC.SERVICE.Services.Tutoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Tutoring.Implementations
{
    public class TutoringProblemService : ITutoringProblemService
    {
        private readonly ITutoringProblemRepository _tutoringProblemRepository;

        public TutoringProblemService(ITutoringProblemRepository tutoringProblemRepository)
        {
            _tutoringProblemRepository = tutoringProblemRepository;
        }

        public async Task DeleteById(Guid tutoringProblemId)
            => await _tutoringProblemRepository.DeleteById(tutoringProblemId);

        public async Task<TutoringProblem> FindByCode(string code)
            => await _tutoringProblemRepository.FindByCode(code);

        public async Task<TutoringProblem> Get(Guid tutoringProblemId)
            => await _tutoringProblemRepository.Get(tutoringProblemId);

        public async Task<IEnumerable<TutoringProblem>> GetAll()
            => await _tutoringProblemRepository.GetAll();

        public async Task<IEnumerable<TutoringProblem>> GetAllByCategory(byte category)
            => await _tutoringProblemRepository.GetAllByCategory(category);
        public async Task<IEnumerable<TutoringProblem>> GetAllByCategoryNu(byte? category = null, string search = null)
            => await _tutoringProblemRepository.GetAllByCategoryNu(category, search);
        public async Task<DataTablesStructs.ReturnedData<TutoringProblem>> GetTutoringProblemsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, byte? category = null)
            => await _tutoringProblemRepository.GetTutoringProblemsDatatable(sentParameters, searchValue, category);

        public async Task Insert(TutoringProblem tutoringProblem)
            => await _tutoringProblemRepository.Insert(tutoringProblem);

        public async Task Update(TutoringProblem tutoringProblem)
            => await _tutoringProblemRepository.Update(tutoringProblem);
        public async Task<DataTablesStructs.ReturnedData<TutoringProblem>> GetTutoringProblemsDatatableByTutoring(DataTablesStructs.SentParameters sentParameters, Guid tutoringId, string searchValue = null, byte? category = null)
        => await _tutoringProblemRepository.GetTutoringProblemsDatatableByTutoring(sentParameters, tutoringId, searchValue, category);
    }
}
