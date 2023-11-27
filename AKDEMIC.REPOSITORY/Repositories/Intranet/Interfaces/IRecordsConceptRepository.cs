using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RecordConcept;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces
{
    public interface IRecordsConceptRepository : IRepository<RecordConcept>
    {
        Task<string> SaveRecordsConcept(List<RecordConceptSaveTemplate> template);
        Task<Guid?> GetValueByRecordType(int recordType);
        Task<RecordConcept> GetRecordConceptByConceptId(Guid conceptId);

    }
}
