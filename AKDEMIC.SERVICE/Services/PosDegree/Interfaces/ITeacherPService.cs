using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Interfaces
{
    public interface ITeacherPService
    {
        Task Insert(PosdegreeTeacher entity);
        Task DeleteTeacher(Guid id);
        Task<PosdegreeTeacher> Get(Guid id);
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherDataTable(DataTablesStructs.SentParameters parameters1, string search);

    }
}
