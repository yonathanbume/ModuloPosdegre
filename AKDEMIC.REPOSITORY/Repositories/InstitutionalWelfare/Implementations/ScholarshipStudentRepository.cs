using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class ScholarshipStudentRepository : Repository<ScholarshipStudent>,IScholarshipStudentRepository
    {
        public ScholarshipStudentRepository(AkdemicContext context) : base(context)
        {
            
        }

        public async Task<ScholarshipStudent> GetScholarshipStudent(Guid studentId, Guid scholarshipId)
            => await _context.ScholarshipStudents.Where(x => x.StudentId == studentId && x.InstitutionalWelfareScholarshipId == scholarshipId).FirstOrDefaultAsync();

        public async Task<List<ScholarshipStudentRequirement>> GetScholarshipStudentRequirements(Guid id)
            => await _context.ScholarshipStudentRequirements.Where(x => x.ScholarshipStudentId == id).ToListAsync();

        public async Task<bool> IsApplicant(Guid studentId, Guid scholarshipId)
            => await _context.ScholarshipStudents.AnyAsync(y => y.StudentId == studentId && y.InstitutionalWelfareScholarshipId == scholarshipId);

        public async Task<DataTablesStructs.ReturnedData<object>> GetScholarshipStudentDatatable(DataTablesStructs.SentParameters parameters, Guid scholarshipId, string searchValue, byte? status = null)
        {
            var query = _context.ScholarshipStudents.Where(x=>x.InstitutionalWelfareScholarshipId == scholarshipId).AsNoTracking();

            if (status.HasValue && status != 0)
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.FullName.Trim().ToLower().Contains(searchValue.Trim().ToLower()));
            }


            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Student.User.UserName,
                      x.Student.User.FullName,
                      x.Student.User.Document,
                      career = x.Student.Career.Name,
                      status = x.Status
                  })
                  .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
