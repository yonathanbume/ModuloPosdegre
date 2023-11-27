using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.CourseEquivalence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CourseEquivalenceRepository : Repository<CourseEquivalence>, ICourseEquivalenceRepository
    {
        public CourseEquivalenceRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<List<CourseEquivalence>> GetByCurriculumId(Guid curriculumId)
        {
           return await _context.CourseEquivalences
                     .Where(x => x.OldAcademicYearCourse.CurriculumId == curriculumId)
                     .Include(x => x.OldAcademicYearCourse)
                     .Include(x => x.NewAcademicYearCourse)
                     .ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEquivalenceDatatable(DataTablesStructs.SentParameters sentParameters, Guid curriculumId, Guid? programId = null)
        {
            var data = await GetEquivalenceData(curriculumId, programId);

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<CourseTemplate>> GetEquivalenceData(Guid curriculumId, Guid? programId)
        {
            var data = await (from c in _context.AcademicYearCourses
                              where c.CurriculumId == curriculumId
                              from ce in _context.CourseEquivalences.Where(ce =>
                                  ce.NewAcademicYearCourseId == c.Id).DefaultIfEmpty()
                              select new CourseTemplate
                              {
                                  Id = c.Id,
                                  Credits = c.Course.Credits,
                                  Code = c.Course.Code,
                                  AcademicProgramId = c.Course.AcademicProgramId.HasValue ? c.Course.AcademicProgramId.Value : Guid.Empty,
                                  AcademicProgram = c.Course.AcademicProgramId.HasValue ? c.Course.AcademicProgram.Name : "---",
                                  Name = c.Course.Name,
                                  AcademicYear = c.AcademicYear,
                                  OldCode = ce != null ? ce.OldAcademicYearCourse.Course.Code : "---",
                                  OldCredits = ce != null ? ce.OldAcademicYearCourse.Course.Credits : -1,
                                  OldName = ce != null ? ce.OldAcademicYearCourse.Course.Name : "---",
                                  OldAcademicYear = ce != null ? ce.OldAcademicYearCourse.AcademicYear : -1,
                                  EquivalenceId = ce != null ? ce.Id : new Guid(),
                                  Duplicated = false,
                                  Replace = ce != null ? ce.ReplaceGrade : false
                              }).OrderBy(x => x.AcademicYear).ThenBy(x => x.Name).ToListAsync();

            if (programId.HasValue && programId != Guid.Empty)
            {
                data = data.Where(x => x.AcademicProgramId == programId).ToList();
            }

            var filter = new List<CourseTemplate>();

            data.ForEach(x =>
            {
                if (filter.Any(y => y.Id == x.Id))
                {
                    x.Duplicated = true;
                }
                else
                {
                    filter.Add(x);
                }
            });
            return data;
        }

        public async Task<Guid> GetNewCurriculumId(Guid id)
        {
          return  await _context.CourseEquivalences
                     .Where(x => x.Id == id)
                     .Select(x => x.NewAcademicYearCourse.CurriculumId)
                     .FirstOrDefaultAsync();
        }

        public override async Task Insert(CourseEquivalence courseEquivalence)
        {
            var exist = await _context.CourseEquivalences.AnyAsync(x => x.NewAcademicYearCourseId == courseEquivalence.NewAcademicYearCourseId 
            && x.OldAcademicYearCourseId == courseEquivalence.OldAcademicYearCourseId);

            if (!exist)
            {
                var enableMultipleGrades = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENABLE_MULTIPLE_GRADES_EQUIVALENCE));

                if (courseEquivalence.ReplaceGrade && !enableMultipleGrades)
                {
                    var equivalences = await _context.CourseEquivalences.Where(x => x.NewAcademicYearCourseId == courseEquivalence.NewAcademicYearCourseId).ToListAsync();

                    foreach (var item in equivalences)
                    {
                        item.ReplaceGrade = false;
                    }
                }

                await _context.CourseEquivalences.AddAsync(courseEquivalence);
                await _context.SaveChangesAsync();
            }            
        }
    }
}
