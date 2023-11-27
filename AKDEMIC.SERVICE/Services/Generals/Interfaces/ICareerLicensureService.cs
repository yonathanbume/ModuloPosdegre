using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface ICareerLicensureService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetCareerLicensureHistoryDatatable(DataTablesStructs.SentParameters parameters, Guid careerId, string searchValue);
        Task Insert(CareerLicensure entity);
        Task Update(CareerLicensure entity);
        Task<CareerLicensure> Get(Guid id);
    }
}
