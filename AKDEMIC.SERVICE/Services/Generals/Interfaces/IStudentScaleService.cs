using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Generals.Interfaces
{
    public interface IStudentScaleService
    {
        Task<object> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string search = null);

        Task<object> GetStudentScalesSelect();

        Task Insert(StudentScale studentScale);

        Task Update(StudentScale studentScale);

        Task Delete(StudentScale studentScale);

        Task DeleteById(Guid id);

        Task<StudentScale> Get(Guid id);

        Task<IEnumerable<StudentScale>> GetAll();
    }
}
