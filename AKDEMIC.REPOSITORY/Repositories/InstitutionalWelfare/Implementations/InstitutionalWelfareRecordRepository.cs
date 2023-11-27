using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareRecordRepository : Repository<InstitutionalWelfareRecord>, IInstitutionalWelfareRecordRepository
    {
        public InstitutionalWelfareRecordRepository(AkdemicContext akdemicContext) : base(akdemicContext)
        {

        }

        public async Task<bool> ExistRecordWithTerm(Guid termId)
        {
            return await _context.InstitutionalWelfareAnswerByStudents.AnyAsync(x => x.TermId == termId);
        }

        public async Task<InstitutionalWelfareRecord> GetActive()
        {
            return await _context.InstitutionalWelfareRecords.Include(x=>x.CategorizationLevelHeader.CategorizationLevels).Where(x => x.IsActive).FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareRecordDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {

            Expression<Func<InstitutionalWelfareRecord, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.IsActive);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.InstitutionalWelfareRecords.AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
            }
            Expression<Func<InstitutionalWelfareRecord, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                x.IsActive,
                x.Name,
                //x.IsEvaluated
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDetailDatatable(DataTablesStructs.SentParameters sentParameters, Guid recordId, Guid termId, bool toEvaluate = false, string searchValue = null)
        {
            var query = _context.InstitutionalRecordCategorizationByStudents        
                        .Include(x=>x.Student.User)
                        .Where(x=>x.InstitutionalWelfareRecordId== recordId && x.TermId == termId)   
                        .AsQueryable();
            if (toEvaluate)
            {
                query = query.Where(x => x.IsEvaluated == false);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(searchValue.ToLower()) || x.Student.User.UserName.Contains(searchValue));
            }          

            int recordsFiltered = query.Count();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new {
                    x.StudentId,
                    x.Student.User.FullName,
                    x.Student.User.UserName,
                    Sum = x.StudentScore,
                    //x.IsEvaluated
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<InstitutionalWelfareRecord> GetWithIncludes(Guid recordId)
        {
            return await _context.InstitutionalWelfareRecords               
                .Include(x => x.CategorizationLevelHeader.CategorizationLevels)
                .Where(x => x.Id == recordId).FirstOrDefaultAsync();
        }

        public async Task<bool> HaveAnswerStudents(Guid recordId)
        {
            return await _context.InstitutionalWelfareAnswerByStudents.AnyAsync(x => x.InstitutionalWelfareQuestion.InstitutionalWelfareSection.InstitutionalWelfareRecordId == recordId);
        }

        public async Task<List<StudentTextAnswerTemplate>> StudentTextAnswersByRecord(Guid recordId, Guid studentId, Guid termId)
        {    
            var result = await _context.InstitutionalWelfareAnswerByStudents
                .Where(x =>
                        x.InstitutionalWelfareQuestion.InstitutionalWelfareSection.InstitutionalWelfareRecordId == recordId
                        && x.InstitutionalWelfareQuestion.Type == 1
                        && x.StudentId == studentId
                        && x.TermId == termId)                        
                .Select(x=> new StudentTextAnswerTemplate
                {         
                    Id = x.Id,
                    AnswerResponse = x.AnswerDescription,
                    Question = x.InstitutionalWelfareQuestion.Description,
                    MaxScore = x.InstitutionalWelfareQuestion.Score  ,
                    WasEvaluated = x.Score != 0
                }).ToListAsync();
            return result;
        }

        public new async Task Update(InstitutionalWelfareRecord record)
        {
            if (record.IsActive)
            {
                var records = _context.InstitutionalWelfareRecords.Where(x => x.Id != record.Id).AsQueryable();
                foreach (var item in records)
                {
                    item.IsActive = false;
                }
            }

            await base.Update(record);
        }
    }
}
