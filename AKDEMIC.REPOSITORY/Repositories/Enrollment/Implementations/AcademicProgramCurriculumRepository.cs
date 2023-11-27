using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public sealed class AcademicProgramCurriculumRepository : Repository<AcademicProgramCurriculum>, IAcademicProgramCurriculumRepository
    {
        public AcademicProgramCurriculumRepository(AkdemicContext context) : base(context) { }

        Task<bool> IAcademicProgramCurriculumRepository.AnyByAcademicProgramAndCurriculum(Guid academicProgramId, Guid curriculumId)
        {
            return _context.AcademicProgramCurriculums.AnyAsync(x => x.AcademicProgramId == academicProgramId && x.CurriculumId == curriculumId);
        }

        public async Task<AcademicProgramCurriculum> GetByFilter(Guid academicProgramId, Guid curriculumId)
        {
            var program = await _context.AcademicProgramCurriculums.Where(x => x.AcademicProgramId == academicProgramId && x.CurriculumId == curriculumId).FirstOrDefaultAsync();
            return program;
        }
        public async Task<IEnumerable<Select2Structs.Result>> GetByAcademicProgramIdSelect2ClientSide(Guid careerId, Guid academicProgramId)
        {
            var query = _context.AcademicProgramCurriculums.AsQueryable();

            if (academicProgramId != Guid.Empty)
            {
                query = query.Where(x => x.AcademicProgramId == academicProgramId);
            }
            if (careerId != Guid.Empty)
            {
                query = query.Where(x => x.AcademicProgram.CareerId == careerId);
            }

            var program =
                await query
                .Select(x => new Select2Structs.Result
                {
                    Id = x.CurriculumId,
                    Text = x.Curriculum.Name
                })
                .OrderBy(x => x.Text)
                .ToArrayAsync();
            return program;
        }
        public async Task LoadAcademicProgramsJob()
        {
            var currentAcademicProgramCurriculums = await _context.AcademicProgramCurriculums.ToListAsync();

            _context.AcademicProgramCurriculums.RemoveRange(currentAcademicProgramCurriculums);

            var curriculums = await _context.Curriculums
                .Select(x => x.Id).ToListAsync();


            var academicProgramCurriculums = new List<AcademicProgramCurriculum>();

            foreach (var curriculum in curriculums)
            {
                var programs = await _context.AcademicYearCourses
                    .Where(x => x.CurriculumId == curriculum && x.Course.AcademicProgramId.HasValue)
                    .Select(x => x.Course.AcademicProgramId.Value)
                    .ToListAsync();

                foreach (var program in programs)
                {
                    if (!academicProgramCurriculums.Any(x => x.AcademicProgramId == program && x.CurriculumId == curriculum))
                    {
                        academicProgramCurriculums.Add(new AcademicProgramCurriculum
                        {
                            AcademicProgramId = program,
                            CurriculumId = curriculum
                        });
                    }
                }
            }

            await _context.AcademicProgramCurriculums.AddRangeAsync(academicProgramCurriculums);

            await _context.SaveChangesAsync();
        }

    }
}