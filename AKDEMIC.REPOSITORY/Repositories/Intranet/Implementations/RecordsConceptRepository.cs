using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.RecordConcept;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class RecordsConceptRepository : Repository<RecordConcept> , IRecordsConceptRepository
    {
        public RecordsConceptRepository(AkdemicContext context) : base(context) { }

        public async Task<RecordConcept> GetRecordConceptByConceptId(Guid conceptId)
            => await _context.RecordConcepts.Where(x => x.ConceptId == conceptId).FirstOrDefaultAsync();

        public async Task<Guid?> GetValueByRecordType(int recordType)
            => await _context.RecordConcepts.Where(x => x.RecordType == recordType).Select(x => x.ConceptId).FirstOrDefaultAsync();

        public async Task<string> SaveRecordsConcept(List<RecordConceptSaveTemplate> template)
        {
            var listRecord = new List<RecordConcept>();
            foreach (var item in template)
            {

                var recordConcept = await _context.RecordConcepts.Where(x => x.RecordType == item.RecordType).FirstOrDefaultAsync();
                if (recordConcept is null)
                {
                    recordConcept = new RecordConcept
                    {
                        ConceptId = item.ConceptId,
                        RecordType = item.RecordType
                    };

                    await _context.AddAsync(recordConcept);
                }
                recordConcept.ConceptId = item.ConceptId;
                listRecord.Add(recordConcept);
            }

            var grouped = listRecord.GroupBy(x => x.ConceptId).Select(x => new { id = x.Key, count = x.Count() }).ToList();
            var hasRepeated = grouped.Any(x=>x.count > 1);

            if (hasRepeated)
            {
                var conceptId = grouped.Where(x => x.count > 1).Select(x => x.id).FirstOrDefault();
                var concept = await _context.Concepts.Where(x => x.Id == conceptId).FirstOrDefaultAsync();
                return $"El concepto {concept.Description} esta siendo asignado mas de una vez.";
            }

            await _context.SaveChangesAsync();
            return string.Empty;
        }
    }
}
