using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class AcademicRecordDepartmentRepository : Repository<AcademicRecordDepartment>, IAcademicRecordDepartmentRepository 
    {
        public AcademicRecordDepartmentRepository(AkdemicContext context) : base(context) { }

        public async Task<List<AcademicRecordDepartment>> GetByUserId(string userId)
            => await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).ToListAsync();
       
        public async Task<object> GetCareersSelect2ClientSide(ClaimsPrincipal user, Guid? facultyId, bool? hasAll = null)
        {
            var query = _context.Careers.AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                query = query.Where(x => careers.Contains(x.Id));
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.FacultyId == facultyId);

            var result = await query
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToListAsync();

            if(hasAll.HasValue && hasAll.Value)
            {
                result.Insert(0, new Select2Structs.Result
                {
                    Id = Guid.Empty,
                    Text = "Todos"
                });
            }

            return result;
        }

        public async Task<object> GetFacultiesSelect2ClientSide(ClaimsPrincipal user)
        {
            var query = _context.Faculties.AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var faculties = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.FacultyId).ToArrayAsync();
                query = query.Where(x => faculties.Contains(x.Id));
            }

            var result = await query
                .Select(x => new
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToListAsync();

            return result;
        }
    }
}
