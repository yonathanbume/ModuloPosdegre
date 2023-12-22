using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces
{
    public  interface ITeacherPRepository:IRepository<PosdegreeTeacher>
    {
        Task<DataTablesStructs.ReturnedData<object>> GetTeacherDataTable(DataTablesStructs.SentParameters parameters1, string search);
      
    }
}
