using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces
{
    public interface IStudentScaleRepository : IRepository<StudentScale>
    {
        Task<object> GetStudentScalesSelect();

        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);
    }
}
