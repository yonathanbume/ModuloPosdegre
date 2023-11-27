using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Implementations
{
    public class YearInformationService: IYearInformationService
    {
        private readonly IYearInformationRepository _yearInformationRepository;
        public YearInformationService(IYearInformationRepository yearInformationRepository)
        {
            _yearInformationRepository = yearInformationRepository;
        }

        public Task<bool> AnyByYear(int year, Guid? id = null)
            => _yearInformationRepository.AnyByYear(year,id);

        public Task Delete(YearInformation yearInformation)
            => _yearInformationRepository.Delete(yearInformation);

        public Task<YearInformation> Get(Guid id)
            => _yearInformationRepository.Get(id);

        public Task<IEnumerable<YearInformation>> GetAll()
            => _yearInformationRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllYearInformationDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _yearInformationRepository.GetAllYearInformationDatatable(sentParameters,searchValue);

        public Task<string> GetNameByYear(int year)
            => _yearInformationRepository.GetNameByYear(year);

        public Task Insert(YearInformation yearInformation)
            => _yearInformationRepository.Insert(yearInformation);

        public Task Update(YearInformation yearInformation)
            => _yearInformationRepository.Update(yearInformation);
    }
}
