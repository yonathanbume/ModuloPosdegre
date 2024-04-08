using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base.Implementations;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.PosDegree
{
    public interface IMasterService
    {
        Task Insert(Master entity);
        Task Update(Master entity);
		Task<List<Master>> GetAllMaster();
        Task<DataTablesStructs.ReturnedData<object>> GetMasterDataTable(DataTablesStructs.SentParameters parameters1, string search);
        Task DownloadExcel(IXLWorksheet worksheet);
        Task DeleteMaster(Guid id);
        Task <Master> Get(Guid id);
        Task<object> GetMasterAllJson();

    }
}
