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
    public class CareerLicensureService : ICareerLicensureService
    {
        private readonly ICareerLicensureRepository _careerLicensureRepository;

        public CareerLicensureService(ICareerLicensureRepository careerLicensureRepository)
        {
            _careerLicensureRepository = careerLicensureRepository;
        }

        public async Task<CareerLicensure> Get(Guid id)
            => await _careerLicensureRepository.Get(id);

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareerLicensureHistoryDatatable(DataTablesStructs.SentParameters parameters, Guid careerId, string searchValue)
            => await _careerLicensureRepository.GetCareerLicensureHistoryDatatable(parameters, careerId, searchValue);

        public async Task Insert(CareerLicensure entity)
            => await _careerLicensureRepository.Insert(entity);

        public async Task Update(CareerLicensure entity)
            => await _careerLicensureRepository.Update(entity);

    }
}
