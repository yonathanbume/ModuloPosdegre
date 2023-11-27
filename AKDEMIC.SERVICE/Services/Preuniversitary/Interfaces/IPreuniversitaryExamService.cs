using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Preuniversitary.Interfaces
{
    public interface IPreuniversitaryExamService
    {
        Task<object> GetDatatable(DataTablesStructs.SentParameters sentParameters, Guid? preuniversitaryTermId);
        Task<bool> AnyByCode(string code, Guid preuniversitaryTermId, Guid? ignoredId = null);
        Task Insert(PreuniversitaryExam entity);
        Task Update(PreuniversitaryExam entity);
        Task<PreuniversitaryExam> Get(Guid id);
        Task Delete(PreuniversitaryExam entity);
    }
}
