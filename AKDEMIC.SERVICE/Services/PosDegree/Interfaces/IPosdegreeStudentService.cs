using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Interfaces
{
    public interface IPosdegreeStudentService
    {
        Task Insert(PosdegreeStudent entity);
        Task<DataTablesStructs.ReturnedData<object>> GetStudentDataTable(DataTablesStructs.SentParameters parameters1, string search);
        Task Delete(Guid id);
        Task<PosdegreeStudent> Get(Guid id);
    }
}
