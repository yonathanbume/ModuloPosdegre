using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class AcademicSecretaryRepository : Repository<AcademicSecretary>, IAcademicSecretaryRepository
    {
        public AcademicSecretaryRepository(AkdemicContext context) : base(context) { }

        Task<bool> IAcademicSecretaryRepository.AnyByTeacherIdAndFacultyId(Guid teacherId, Guid facultyId)
            => _context.AcademicSecretaries.AnyAsync(x => x.TeacherId == teacherId && x.FacultyId == facultyId);

        Task<int> IAcademicSecretaryRepository.CountByTeacherId(Guid teacherId)
            => _context.AcademicSecretaries.CountAsync(x => x.TeacherId == teacherId);

        async Task<object> IAcademicSecretaryRepository.GetAllAsModelA(Guid? facultyId)
        {
            var academicSecretariesQuery = _context.AcademicSecretaries.AsQueryable();

            if (facultyId.HasValue)
                academicSecretariesQuery = academicSecretariesQuery.Where(x => x.FacultyId == facultyId.Value);

            var academicSecretaries = await academicSecretariesQuery
                .Select(
                    x => new
                    {
                        id = x.Id,
                        tid = x.TeacherId.ToString().ToUpper(),
                        teacherName = _context.Users.FirstOrDefault(y => y.Id == "{" + $"{x.TeacherId}".ToUpper() + "}").FullName,
                        facultyName = x.Faculty.Name
                    }
                )
                .ToListAsync();

            return academicSecretaries;
        }

        async Task<object> IAcademicSecretaryRepository.GetAsModelB(Guid? id)
        {
            var query = _context.AcademicSecretaries.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id.Value);

            var academicSecretary = await query
                .Select(
                    x => new
                    {
                        id = x.Id,
                        teacherName = (_context.Users.FirstOrDefault(y => y.Id == "{" + $"{x.TeacherId}".ToUpper() + "}")).FullName,
                        teacherId = x.TeacherId,
                        facultyName = x.Faculty.Name,
                        facultyId = x.FacultyId
                    }
                )
                .FirstOrDefaultAsync();

            return academicSecretary;
        }
    }
}