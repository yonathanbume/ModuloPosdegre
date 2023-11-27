using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template.CafobeRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.SERVICE.Services.InstitutionalWelfare.Interfaces
{
    public interface IWelfareAlertService
    {
        Task<WelfareAlert> Get(Guid id);
        Task<IEnumerable<WelfareAlert>> GetAll();
        Task Insert(WelfareAlert welfareAlert);
        Task Update(WelfareAlert welfareAlert);
        Task Delete(WelfareAlert welfareAlert);
        Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, int? status = null, string searchValue = null);

    }
}
