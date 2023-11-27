using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutorTutoringStudentRepository : Repository<TutorTutoringStudent>, ITutorTutoringStudentRepository
    {
        public TutorTutoringStudentRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<bool> AnyByTutorIdAndTutoringStudentId(string tutorId, Guid tutoringStudentId, Guid term)
            => await _context.TutorTutoringStudents.AnyAsync(x => x.TutorId == tutorId && x.TutoringStudentStudentId == tutoringStudentId && x.TutoringStudentTermId == term);

        public async Task<bool> AnyByStudentId(Guid studentId)
            => await _context.TutorTutoringStudents.AnyAsync(x => x.TutoringStudent.StudentId == studentId);
        public async Task<bool> AnyByCoordinator(string userId)
            => await _context.TutorTutoringStudents.AnyAsync(x => x.Tutor.UserId == userId);
        public async Task<bool> AnyByTutor(string tutorId)
    => await _context.TutorTutoringStudents.AnyAsync(x => x.TutorId == tutorId);
        public async Task DeleteByTutorIdAndTutoringStudentId(string tutorId, Guid tutoringStudentId, Guid term)
        {
            var tutorTutoringStudent = await _context.TutorTutoringStudents.Where(x => x.TutorId == tutorId && x.TutoringStudentStudentId == tutoringStudentId && x.TutoringStudentTermId == term).FirstOrDefaultAsync();
            await base.Delete(tutorTutoringStudent);
        }

        public async Task<IEnumerable<TutorTutoringStudent>> GetByTutorId(string tutorId)
            => await _context.TutorTutoringStudents
                .Include(x => x.TutoringStudent.Student.User)
                .Where(x => x.TutorId == tutorId)
                .ToListAsync();

        public async Task<TutorTutoringStudent> GetByTutorIdAndTutoringStudentId(string tutorId, Guid tutoringStudentId)
            => await _context.TutorTutoringStudents.FindAsync(tutorId, tutoringStudentId);

        public async Task<IEnumerable<TutorTutoringStudent>> GetByTutoringStudentId(Guid tutoringStudentId)
            => await _context.TutorTutoringStudents
                .Include(x => x.Tutor.Career)
                .Include(x => x.Tutor.User)
                .Where(x => x.TutoringStudentStudentId == tutoringStudentId)
                .ToListAsync();

        public async Task<TutorTutoringStudent> GetByStudentId(Guid studentId)
            => await _context.TutorTutoringStudents
            .Include(x => x.Tutor).ThenInclude(x => x.User)
            .Where(x => x.TutoringStudent.StudentId == studentId).FirstOrDefaultAsync();

        public async Task<object> GetAllTutoringsChart(Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TutorTutoringStudents.AsNoTracking();
            var careersQuery = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Tutor.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            if (termId != null)
                query = query.Where(x => x.TutoringStudent.TermId == termId);

            var data = await careersQuery
                .Select(x => new
                {
                    x.Name,
                    Count = x.Tutors.Count(y => query.Any(z => z.TutorId == y.UserId))
                }).ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Name).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTutorByTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TutorTutoringStudents.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Tutor.Career.QualityCoordinatorId == userId);
                }
            }

            //TutoringStudentTermId = TermId , es lo mismo el problema esta en la config de la tabla
            if (termId != null)
            {
                query = query.Where(x => x.TutoringStudentTermId == termId);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new 
                {
                    x.TutorId,
                    x.TutoringStudentTermId,
                    TermName = x.TutoringStudent.Term.Name,
                })
                .Distinct()
                .GroupBy(x => new { x.TutoringStudentTermId , x.TermName })
                .Select(x => new
                {
                    TermName = x.Key.TermName,
                    Tutors = x.Count()
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

        public async Task<object> GetTutorByTermChart(Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.TutorTutoringStudents.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Tutor.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null) query = query.Where(x => x.TutoringStudentTermId == termId);

            var data = await _context.Terms
                .Select(x => new
                {
                    Name = x.Name,
                    Count = query.Where(y => y.TutoringStudentTermId == x.Id).Select(y => y.TutorId).Distinct().Count()
                }).ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Name).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<object> GetTutorsByTutoringStudent(Guid studentId, Guid termId)
        {
            var result = await _context.TutorTutoringStudents
                .Where(x => x.TutoringStudent.TermId == termId && x.TutoringStudent.StudentId == studentId)
                .Select(x => new
                {
                    id = x.TutorId,
                    text = x.Tutor.User.FullName
                })
                .ToListAsync();

            return result;
        }
    }
}
