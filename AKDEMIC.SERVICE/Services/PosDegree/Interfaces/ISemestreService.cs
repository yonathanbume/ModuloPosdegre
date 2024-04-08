using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Interfaces
{
    public  interface ISemestreService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetSemestreDataTable(DataTablesStructs.SentParameters parameters1, string search);
        Task Insert(Semestre entity);
        Task DeleteSemestre(Guid id);
        Task<Semestre> Get(Guid id);
        Task<object> GetSemestreAllJson();
    }
}
