using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryExamClassroomPostulantRepository : IRepository<PreuniversitaryExamClassroomPostulant>
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid preuniversitaryExamId);
        Task<Tuple<bool, string>> AssignPostulantsRandomly(Guid preuniversitaryExamId);
    }
}
