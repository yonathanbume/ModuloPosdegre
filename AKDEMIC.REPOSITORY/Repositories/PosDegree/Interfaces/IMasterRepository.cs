using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Base;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces
{
      public interface IMasterRepository:IRepository<Master>
    {
		Task<List<Master>> GetAll();
        /*implementacion para  filatrado de datos*/
        Task<DataTablesStructs.ReturnedData<object>> GetMasterDataTable(DataTablesStructs.SentParameters parameters1, string search);
        /*prueba*/
        Task DownloadExcel(IXLWorksheet worksheet);
        Task UpdateAsync(Master entity);
        Task SaveChangesAsync();


    }
}
