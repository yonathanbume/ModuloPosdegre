using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Scale.Interfaces
{
    public interface IBenefitTypeService
    {
        Task<BenefitType> Get(Guid id);
        Task<IEnumerable<BenefitType>> GetAll();
        Task Insert(BenefitType benefitType);
        Task Update(BenefitType benefitType);
        Task Delete(BenefitType benefitType);
        Task<bool> AnyByName(string name, Guid? id = null);

        Task<DataTablesStructs.ReturnedData<object>> GetAllBenefitsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);
    }
}
