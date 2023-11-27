using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class InstitutionService: IInstitutionService
    {
        private readonly IInstitutionRepository _institutionRepository;
        public InstitutionService(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _institutionRepository.AnyByName(name , id);

        public async Task Delete(Institution institution)
            => await _institutionRepository.Delete(institution);

        public async Task<Institution> Get(Guid id)
            => await _institutionRepository.Get(id);

        public async Task<IEnumerable<Institution>> GetAll()
            => await _institutionRepository.GetAll();

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllInstitutionsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _institutionRepository.GetAllInstitutionsDatatable(sentParameters,searchValue);

        public async Task Insert(Institution institution)
            => await _institutionRepository.Insert(institution);

        public async Task Update(Institution institution)
            => await _institutionRepository.Update(institution);
    }
}
