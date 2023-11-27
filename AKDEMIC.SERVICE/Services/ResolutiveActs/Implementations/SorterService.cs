using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.ResolutiveActs;
using AKDEMIC.REPOSITORY.Repositories.ResolutiveActs.Interfaces;
using AKDEMIC.SERVICE.Services.ResolutiveActs.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.ResolutiveActs.Implementations
{
    public class SorterService : ISorterService
    {
        private readonly ISorterRepository _sorterRepository;

        public SorterService(ISorterRepository sorterRepository)
        {
            _sorterRepository = sorterRepository;
        }

        public async Task<bool> AnyByName(string name,Guid? ignoredId = null)
            => await _sorterRepository.AnyByName(name,ignoredId);

        public async Task Delete(Sorter sorter)
            => await _sorterRepository.Delete(sorter);

        public async Task<Sorter> Get(Guid id)
            => await _sorterRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<Sorter>> GetSorterDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _sorterRepository.GetSorterDatatable(sentParameters, search);

        public async Task<IEnumerable<Select2Structs.Result>> GetSorterSelect2ClientSide()
            => await _sorterRepository.GetSorterSelect2ClientSide();

        public async Task Insert(Sorter sorter)
            => await _sorterRepository.Insert(sorter);

        public async Task Update(Sorter sorter)
            => await _sorterRepository.Update(sorter);
    }
}
