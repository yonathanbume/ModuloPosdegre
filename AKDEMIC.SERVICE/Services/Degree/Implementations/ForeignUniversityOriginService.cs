using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.REPOSITORY.Repositories.Degree.Interfaces;
using AKDEMIC.SERVICE.Services.Degree.Interfaces;
using System;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Degree.Implementations
{
    public class ForeignUniversityOriginService: IForeignUniversityOriginService
    {
        private readonly IForeignUniversityOriginRepository _foreignUniversityOriginRepository;

        public ForeignUniversityOriginService(IForeignUniversityOriginRepository foreignUniversityOriginRepository)
        {
            _foreignUniversityOriginRepository = foreignUniversityOriginRepository;
        }

        public async Task Delete(ForeignUniversityOrigin foreignUniversityOrigin)
            => await _foreignUniversityOriginRepository.Delete(foreignUniversityOrigin);

        public async Task<ForeignUniversityOrigin> Get(Guid id)
            => await _foreignUniversityOriginRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetForeignUniveristyOriginDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _foreignUniversityOriginRepository.GetForeignUniveristyOriginDatatable(sentParameters,searchValue);

        public async Task<object> GetSelect2()
        {
            return await _foreignUniversityOriginRepository.GetSelect2();
        }

        public async Task Insert(ForeignUniversityOrigin foreignUniversityOrigin)
            => await _foreignUniversityOriginRepository.Insert(foreignUniversityOrigin);

        public async Task Update(ForeignUniversityOrigin foreignUniversityOrigin)
            => await _foreignUniversityOriginRepository.Update(foreignUniversityOrigin);
    }
}
