using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.REPOSITORY.Repositories.AcademicExchange.Interfaces;
using AKDEMIC.SERVICE.Services.AcademicExchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.AcademicExchange.Implementations
{
    public class AcademicExchangeNewsService : IAcademicExchangeNewsService
    {
        private readonly IAcademicExchangeNewsRepository _academicExchangeNewsRepository;

        public AcademicExchangeNewsService(IAcademicExchangeNewsRepository academicExchangeNewsRepository)
        {
            _academicExchangeNewsRepository = academicExchangeNewsRepository;
        }

        public async Task<int> Count()
            => await _academicExchangeNewsRepository.Count();

        public async Task Delete(AcademicExchangeNews entity)
            => await _academicExchangeNewsRepository.Delete(entity);

        public async Task<AcademicExchangeNews> Get(Guid id)
            => await _academicExchangeNewsRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<AcademicExchangeNews>> GetAcademicExchangeNewsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => await _academicExchangeNewsRepository.GetAcademicExchangeNewsDatatable(sentParameters, searchValue);

        public async Task<IEnumerable<AcademicExchangeNews>> GetAll()
            => await _academicExchangeNewsRepository.GetAll();

        public async Task<IEnumerable<AcademicExchangeNews>> GetAllServerSide(int rowsPerPage, int page)
            => await _academicExchangeNewsRepository.GetAllServerSide(rowsPerPage, page);

        public async Task Insert(AcademicExchangeNews entity)
            => await _academicExchangeNewsRepository.Insert(entity);

        public async Task Update(AcademicExchangeNews entity)
            => await _academicExchangeNewsRepository.Update(entity);
    }
}
