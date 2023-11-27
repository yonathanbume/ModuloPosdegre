using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using AKDEMIC.SERVICE.Services.Admission.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Admission.Implementations
{
    public class EntityLoadFormatService : IEntityLoadFormatService
    {
        private readonly IEntityLoadFormatRepository _entityLoadFormatRepository;

        public EntityLoadFormatService(IEntityLoadFormatRepository entityLoadFormatRepository)
        {
            _entityLoadFormatRepository = entityLoadFormatRepository;
        }

        public async Task Activate(Guid entityLoadFormatId)
        {
            await _entityLoadFormatRepository.Activate(entityLoadFormatId);
        }

        public async Task Delete(EntityLoadFormat entityLoadFormat)
        {
            await _entityLoadFormatRepository.Delete(entityLoadFormat);
        }

        public async Task DeleteById(Guid entityLoadFormatId)
        {
            await _entityLoadFormatRepository.DeleteById(entityLoadFormatId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> EntityLoadFormatDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await _entityLoadFormatRepository.EntityLoadFormatDatatable(sentParameters, searchValue);
        }

        public async Task<object> EntityLoadSelect2(bool? onlyActive = false)
        {
            return await _entityLoadFormatRepository.EntityLoadSelect2(onlyActive);
        }

        public async Task<EntityLoadFormat> Get(Guid entityLoadFormatId)
        {
            return await _entityLoadFormatRepository.Get(entityLoadFormatId);
        }

        public async Task<EntityLoadFormat> GetActive() => await _entityLoadFormatRepository.GetActive();

        public async Task Insert(EntityLoadFormat entityLoadFormat)
        {
            await _entityLoadFormatRepository.Insert(entityLoadFormat);
        }

        public async Task Update(EntityLoadFormat entityLoadFormat)
        {
            await _entityLoadFormatRepository.Update(entityLoadFormat);
        }
    }
}
