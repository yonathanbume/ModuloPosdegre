using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Degree.Implementations
{
    public class DegreeRequirementService : IDegreeRequirementService
    {
        private readonly IDegreeRequirementRepository _degreeRequirementRepository;

        public DegreeRequirementService(IDegreeRequirementRepository degreeRequirementRepository)
        {
            _degreeRequirementRepository = degreeRequirementRepository;
        }
        public async Task Delete(DegreeRequirement entity)
        {
            await _degreeRequirementRepository.Delete(entity);
        }

        public async Task<DegreeRequirement> Get(Guid id)
        {
            return await _degreeRequirementRepository.Get(id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDegreeRequirementDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _degreeRequirementRepository.GetDegreeRequirementDatatable(sentParameters, searchValue);
        }

        public async Task<IEnumerable<DegreeRequirement>> GetDegreeRequirementsByType(int type)
        {
            return await _degreeRequirementRepository.GetDegreeRequirementsByType(type);
        }



        public async Task Insert(DegreeRequirement entity)
        {
            await _degreeRequirementRepository.Insert(entity);
        }

        public async Task Update(DegreeRequirement entity)
        {
            await _degreeRequirementRepository.Update(entity);
        }
    }
}
