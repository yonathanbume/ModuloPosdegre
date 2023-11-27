using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.CourseSyllabus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public class CourseSyllabusRepository : Repository<CourseSyllabus>, ICourseSyllabusRepository
    {
        public CourseSyllabusRepository(AkdemicContext context) : base(context) { }

        public async Task<CourseSyllabusTemplateA> GetAsModelAByFilter(Guid? courseId = null, Guid? termId = null)
        {
            var query = _context.CourseSyllabus.AsQueryable();

            if (courseId.HasValue)
                query = query.Where(x => x.CourseId == courseId.Value);

            if (termId.HasValue)
                query = query.Where(x => x.TermId == termId.Value);

            var model = await query
                .Select(x => new CourseSyllabusTemplateA
                {
                    Id = x.Id,
                    CourseId = x.CourseId,
                    CourseCode = x.Course.Code,
                    CourseName = x.Course.Name,
                    TermName = x.Term.Name,
                    TermId = x.TermId,
                }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<CourseSyllabus> GetByCourseIdAndTermId(Guid courseId, Guid termId)
            => await _context.CourseSyllabus.Where(x => x.CourseId == courseId && x.TermId == termId).FirstOrDefaultAsync();

        public async Task<Guid> GetIdByCourseIdAndTermId(Guid courseId, Guid termId)
        {
            var sylabusId = await _context.CourseSyllabus.Where(x => x.CourseId == courseId && x.TermId == termId).Select(x => x.Id).FirstOrDefaultAsync();
            return sylabusId;
        }

        public async Task<CourseSyllabus> GetIncludingTermAndCourse(Guid courseId, Guid termId) =>
            await _context.CourseSyllabus.Include(x => x.Term).Include(x => x.Course.Area)
            .Include(x=>x.ListCourseUnit)
                .Where(x => x.TermId == termId && x.CourseId == courseId).FirstOrDefaultAsync();

        public async Task<CourseSyllabus> GetIncludingTermAndCourse(Guid syllabusId) => await _context.CourseSyllabus
            .Include(x => x.Term).Include(x => x.Course).FirstOrDefaultAsync(x => x.Id == syllabusId);
    }
}