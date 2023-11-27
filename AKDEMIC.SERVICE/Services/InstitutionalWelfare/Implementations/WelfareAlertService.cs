using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Implementations
{
    public class WelfareAlertService : IWelfareAlertService
    {
        private readonly IWelfareAlertRepository _welfareAlertRepository;
        public WelfareAlertService(IWelfareAlertRepository welfareAlertRepository)
        {
            _welfareAlertRepository = welfareAlertRepository;
        }
        public Task Delete(WelfareAlert welfareAlert)
            => _welfareAlertRepository.Delete(welfareAlert);
        public Task<WelfareAlert> Get(Guid id)
            => _welfareAlertRepository.Get(id);

        public Task<IEnumerable<WelfareAlert>> GetAll()
            => _welfareAlertRepository.GetAll();

        public Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, int? status = null, string searchValue = null)
            => _welfareAlertRepository.GetAllDatatable(sentParameters, status, searchValue);

        public Task Insert(WelfareAlert welfareAlert)
            => _welfareAlertRepository.Insert(welfareAlert);

        public Task Update(WelfareAlert welfareAlert)
            => _welfareAlertRepository.Update(welfareAlert);
    }
}
