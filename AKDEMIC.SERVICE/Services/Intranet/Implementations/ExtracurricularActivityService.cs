using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.SERVICE.Services.Intranet.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Implementations
{
    public class ExtracurricularActivityService : IExtracurricularActivityService
    {
        private readonly IExtracurricularActivityRepository _extracurricularActivityRepository;

        public ExtracurricularActivityService(IExtracurricularActivityRepository extracurricularActivityRepository)
        {
            _extracurricularActivityRepository = extracurricularActivityRepository;
        }

        public Task DeleteById(Guid id)
            => _extracurricularActivityRepository.DeleteById(id);

        public Task<ExtracurricularActivity> Get(Guid id)
            => _extracurricularActivityRepository.Get(id);

        public Task<IEnumerable<ExtracurricularActivity>> GetAll()
            => _extracurricularActivityRepository.GetAll();

        public Task<ExtracurricularActivity> GetByCode(string code)
            => _extracurricularActivityRepository.GetByCode(code);

        public Task<ExtracurricularActivity> GetByName(string name)
            => _extracurricularActivityRepository.GetByName(name);

        public async Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
            => await _extracurricularActivityRepository.GetDataDatatable(sentParameters, search);


        public async Task<IEnumerable<Select2Structs.Result>> GetExtracurricularActivitiesSelect2ClientSide()
            => await _extracurricularActivityRepository.GetExtracurricularActivitiesSelect2ClientSide();

        public Task Insert(ExtracurricularActivity extracurricularActivity)
            => _extracurricularActivityRepository.Insert(extracurricularActivity);

        public Task Update(ExtracurricularActivity extracurricularActivity)
            => _extracurricularActivityRepository.Update(extracurricularActivity);
    }
}
