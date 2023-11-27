using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class InstitutionalWelfareScholarshipRepository : Repository<InstitutionalWelfareScholarship>, IInstitutionalWelfareScholarshipRepository
    {
        public InstitutionalWelfareScholarshipRepository(AkdemicContext context) : base(context)
        {

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetScholarshipDatatable(DataTablesStructs.SentParameters parameters, Guid? scholarshipTypeId, string searchValue, ClaimsPrincipal user = null)
        {
            var query = _context.InstitutionalWelfareScholarships.AsNoTracking();

            if (scholarshipTypeId.HasValue && scholarshipTypeId != Guid.Empty)
                query = query.Where(x => x.InstitutionalWelfareScholarshipTypeId == scholarshipTypeId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.Trim().ToLower().Contains(searchValue.Trim().ToLower()));
            }

            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Name,
                      applicationStart = x.ApplicationStartDate.ToDateFormat(),
                      applicationEnd = x.ApplicationEndDate.Date.ToDateFormat(),
                      scholarshipType = x.InstitutionalWelfareScholarshipType.Name,

                      //Postulante
                      IsApplicant = x.ScholarshipStudents.Any(y => y.Student.UserId == userId),
                      PostulantStatus = x.ScholarshipStudents.Where(y => y.Student.UserId == userId).Select(y => y.Status).FirstOrDefault()
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

        public async Task<List<ScholarshipStudent>> GetScholarshipStudents(Guid scholarshipId, byte status)
        {
            var result = await _context.ScholarshipStudents.Where(x => x.InstitutionalWelfareScholarshipId == scholarshipId && x.Status == status)
                .Include(x => x.Student.User)
                .Include(x => x.Student.Career)
                .ToListAsync();

            return result;
        }

        public async Task<bool> AnyStudent(Guid scholarshipId)
            => await _context.ScholarshipStudents.AnyAsync(x => x.InstitutionalWelfareScholarshipId == scholarshipId);
    }
}
