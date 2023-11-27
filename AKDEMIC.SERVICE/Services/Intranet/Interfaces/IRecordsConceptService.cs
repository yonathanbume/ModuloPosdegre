using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RecordConcept;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.Intranet.Interfaces
{
    public interface IRecordsConceptService
    {
        Task<string> SaveRecordsConcept(List<RecordConceptSaveTemplate> template);
        Task<Guid?> GetValueByRecordType(int recordType);

        Task<RecordConcept> GetRecordConceptByConceptId(Guid conceptId);
    }
}
