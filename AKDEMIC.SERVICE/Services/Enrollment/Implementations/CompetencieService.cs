using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.SERVICE.Services.Enrollment.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Enrollment.Implementations
{
    public class CompetencieService : ICompetencieService
    {
        private readonly ICompetencieRepository _competencieRepository;

        public CompetencieService(ICompetencieRepository competencieRepository)
        {
            _competencieRepository = competencieRepository;
        }

        public async Task<bool> AnyByName(string name, Guid? ignoredId = null)
            => await _competencieRepository.AnyByName(name, ignoredId);

        public async Task Delete(Competencie entity)
            => await _competencieRepository.Delete(entity);

        public async Task<Competencie> Get(Guid id)
            => await _competencieRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCompetenciesDatatable(DataTablesStructs.SentParameters parameters, string searchvalue)
            => await _competencieRepository.GetCompetenciesDatatable(parameters, searchvalue);

        public async Task<Select2Structs.ResponseParameters> GetCompetenciesSelect2(Select2Structs.RequestParameters parameters, byte? type, string searchValue, Guid? curriculumId)
            => await _competencieRepository.GetCompetenciesSelect2(parameters, type, searchValue, curriculumId);

        public async Task Insert(Competencie entity)
            => await _competencieRepository.Insert(entity);

        public async Task Update(Competencie entity)
            => await _competencieRepository.Update(entity);
    }
}
