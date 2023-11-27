using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface ICareerLicensureRepository : IRepository<CareerLicensure>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCareerLicensureHistoryDatatable(DataTablesStructs.SentParameters parameters, Guid careerId, string searchValue);
    }
}
