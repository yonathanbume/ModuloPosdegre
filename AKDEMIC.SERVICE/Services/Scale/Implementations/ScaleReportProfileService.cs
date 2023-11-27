using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Repositories.Scale.Implementations;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.SERVICE.Services.Scale.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Implementations
{
    public class ScaleReportProfileService : IScaleReportProfileService
    {
        private readonly IScaleReportProfileRepository _scaleReportProfileRepository;

        public ScaleReportProfileService(IScaleReportProfileRepository scaleReportProfileRepository)
        {
            _scaleReportProfileRepository = scaleReportProfileRepository;
        }

        public Task<bool> AnyByName(string name, Guid? id = null)
            => _scaleReportProfileRepository.AnyByName(name,id);

        public Task Delete(ScaleReportProfile scaleReportProfile)
            => _scaleReportProfileRepository.Delete(scaleReportProfile);

        public Task DeleteById(Guid id)
            => _scaleReportProfileRepository.DeleteById(id);

        public Task<ScaleReportProfile> Get(Guid id)
            => _scaleReportProfileRepository.Get(id);

        public Task<IEnumerable<ScaleReportProfile>> GetAll()
            => _scaleReportProfileRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetReportProfileDataTable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
            => _scaleReportProfileRepository.GetReportProfileDataTable(sentParameters,searchValue);

        public Task Insert(ScaleReportProfile scaleReportProfile)
            => _scaleReportProfileRepository.Insert(scaleReportProfile);

        public Task Update(ScaleReportProfile scaleReportProfile)
            => _scaleReportProfileRepository.Update(scaleReportProfile);
    }
}
