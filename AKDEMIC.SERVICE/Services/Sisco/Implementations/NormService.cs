using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using AKDEMIC.SERVICE.Services.Sisco.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Sisco.Implementations
{
    public class NormService : INormService
    {
        private readonly INormRepository _normRepository;

        public NormService(INormRepository normRepository)
        {
            _normRepository = normRepository;
        }

        public async Task InsertNorm(Norm norm) =>
            await _normRepository.Insert(norm);

        public async Task UpdateNorm(Norm norm) =>
            await _normRepository.Update(norm);

        public async Task DeleteNorm(Norm norm) =>
            await _normRepository.Delete(norm);

        public async Task<Norm> GetNormById(Guid id) =>
            await _normRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<NormTemplate>> GetAllNormDatatable(DataTablesStructs.SentParameters sentParameters, SearchNormTemplate search) =>
            await _normRepository.GetAllNormDatatable(sentParameters, search);

        public async Task<NormTemplate> GetNormTemplateById(Guid id) =>
            await _normRepository.GetNormById(id);
    }
}
