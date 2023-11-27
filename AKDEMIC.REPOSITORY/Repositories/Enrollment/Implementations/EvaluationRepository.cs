using AKDEMIC.CORE.Helpers;
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
    public class EvaluationRepository : Repository<ENTITIES.Models.Enrollment.Evaluation>, IEvaluationRepository
    {
        public EvaluationRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetAllAsModelA(Guid? termId = null, Guid? courseId = null)
        {
            var query = _context.Evaluations.AsQueryable();

            if (termId.HasValue)
                query = query.Where(x => x.CourseTerm.TermId == termId.Value);

            if (courseId.HasValue)
                query = query.Where(x => x.CourseTerm.CourseId == courseId.Value);

            var dataDB = await query
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    percentage = x.Percentage,
                    unitName = x.CourseUnit.Name,
                    week = x.Week
                }).ToListAsync();

            var result = dataDB
                .Select(x => new
                {
                    x.id,
                    x.name,
                    percentage = $"{x.percentage}%",
                    x.unitName,
                    week = x.week.HasValue ? $"Semana {x.week}" : "No asignado"
                }).ToList();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationsDatatable(DataTablesStructs.SentParameters parameters, Guid termId, Guid courseId)
        {
            var query = _context.Evaluations.Where(x => x.CourseTerm.CourseId == courseId && x.CourseTerm.TermId == termId).AsQueryable();
            int recordsFiltered = await query.CountAsync();
            var equivalentUnits = await query.AllAsync(x => x.CourseUnit.AcademicProgressPercentage == 0);
            var data = await query
                .OrderBy(x => x.CourseUnit.Number).ThenBy(x => x.CourseUnit.Name).ThenBy(x => x.Week)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Name,
                      percentage = $"{x.Percentage}%",
                      unitName = x.CourseUnit.Name,
                      week = x.Week.HasValue ? $"Semana {x.Week}" : "No Asignado",
                      group = new
                      {
                          name = x.CourseUnitId.HasValue ? (!equivalentUnits ? $"{x.CourseUnit.Name} - PORCENTAJE : {x.CourseUnit.AcademicProgressPercentage}%" : $"{x.CourseUnit.Name}") : "EVALUACIONES DISPONIBLES",
                          id = x.CourseUnitId
                      }
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetEvaluationsDatatableByTeacherConfiguration(DataTablesStructs.SentParameters parameters, Guid termId, Guid courseId, Guid sectionId)
        {
            var query = _context.Evaluations.Where(x => x.CourseTerm.CourseId == courseId && x.CourseTerm.TermId == termId).AsQueryable();
            int recordsFiltered = await query.CountAsync();
            var equivalentUnits = await query.AllAsync(x => x.CourseUnit.AcademicProgressPercentage == 0);
            var data = await query
                .OrderBy(x => x.CourseUnit.Number).ThenBy(x => x.CourseUnit.Name).ThenBy(x => x.Week)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                  .Select(x => new
                  {
                      x.Id,
                      x.Name,
                      percentage = _context.SectionEvaluations.Any(y => y.EvaluationId == x.Id && y.SectionId == sectionId) ? _context.SectionEvaluations.Where(y => y.EvaluationId == x.Id && y.SectionId == sectionId).Select(y => y.Percentage.ToString()).FirstOrDefault() : $"Sin configurar",
                      unitName = x.CourseUnit.Name,
                      week = x.Week.HasValue ? $"Semana {x.Week}" : "No Asignado",
                      sectionId,
                      group = new
                      {
                          name = x.CourseUnitId.HasValue ? (!equivalentUnits ? $"{x.CourseUnit.Name} - PORCENTAJE : {x.CourseUnit.AcademicProgressPercentage}%" : $"{x.CourseUnit.Name}") : "EVALUACIONES DISPONIBLES",
                          id = x.CourseUnitId
                      }
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

        public async Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseAndTerm(Guid courseId, Guid termId) =>
            await _context
            .Evaluations
            .Include(x => x.CourseUnit)
            .Include(x => x.Grades)
            .Where(x => x.CourseTerm.CourseId == courseId && x.CourseTerm.TermId == termId)
            .ToListAsync();

        public async Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseAndTermWithTaken(Guid courseId, Guid termId,
            Guid sectionId)
        {

            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var evaluationCount = await _context.Evaluations.Where(x => x.CourseTerm.CourseId == courseId && x.CourseTerm.TermId == termId).CountAsync();

            var classStudents = await _context.ClassStudents.Where(x => x.Class.SectionId == sectionId)
                .Select(x => new
                {
                    x.StudentId,
                    x.IsAbsent,
                    x.Class.ClassSchedule.SectionGroupId
                })
                .ToListAsync();

            var classesBySubGroup = await _context.Classes.Where(x => x.ClassSchedule.SectionId == sectionId)
                .GroupBy(x => x.ClassSchedule.SectionGroupId)
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .ToListAsync();

            var studentSectionsData = await _context.StudentSections.Where(x => x.SectionId == sectionId && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
                .Select(x => new
                {
                    x.Id,
                    x.StudentId,
                    grades = x.Grades.Select(y => new
                    {
                        y.EvaluationId,
                        y.Value
                    }).ToList(),
                    x.SectionGroupId
                })
                .ToListAsync();

            var studentSections = studentSectionsData
                .Select(ss => new
                {
                    ss.Id,
                    ss.StudentId,
                    ss.grades,
                    DPI = classesBySubGroup.Where(x => !x.Key.HasValue || x.Key == ss.SectionGroupId).Sum(x => x.count) == 0 ?
                        false :
                        ((decimal)classStudents.Where(x => x.StudentId == ss.StudentId && (!x.SectionGroupId.HasValue || x.SectionGroupId == x.SectionGroupId) && x.IsAbsent).Count() / (decimal)classesBySubGroup.Where(x => !x.Key.HasValue || x.Key == ss.SectionGroupId).Sum(x => x.count)) * 100M > (decimal)term.AbsencePercentage,
                })
                .ToList();

            var result = await _context.Evaluations
            .Where(x => x.CourseTerm.CourseId == courseId && x.CourseTerm.TermId == termId)
            .Select(x => new ENTITIES.Models.Enrollment.Evaluation
            {
                Id = x.Id,
                Name = x.Name,
                Percentage = x.Percentage,
                Retrievable = x.Retrievable,
                //Taken = studentSections.Where(y=>!y.DPI).All(y=>y.grades.Any(z=>z.EvaluationId == x.Id)),
                Week = x.Week,
                Description = x.Description,
                CourseUnitId = x.CourseUnitId,
                CourseUnit = x.CourseUnit != null ? new CourseUnit
                {
                    Id = x.CourseUnit.Id,
                    Name = x.CourseUnit.Name,
                    Number = x.CourseUnit.Number
                } : null
            }).OrderBy(x => x.Week).ThenBy(x => x.Name).ToListAsync();

            foreach (var item in result)
                item.Taken = studentSections.Where(y => !y.DPI).All(y => y.grades.Any(z => z.EvaluationId == item.Id));

            return result;
        }



        public async Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseTerm(Guid courseTermId)
            => await _context
            .Evaluations.Where(x => x.CourseTerm.Id == courseTermId)
            .Select(x => new ENTITIES.Models.Enrollment.Evaluation
            {
                Id = x.Id,
                Name = x.Name,
                Percentage = x.Percentage,
                Retrievable = x.Retrievable,
                CourseUnitId = x.CourseUnitId,
                Week = x.Week,
                CourseUnit = x.CourseUnit != null ? new CourseUnit
                {
                    Id = x.CourseUnit.Id,
                    Name = x.CourseUnit.Name,
                    Number = x.CourseUnit.Number,
                    AcademicProgressPercentage = x.CourseUnit.AcademicProgressPercentage,
                    WeekNumberStart = x.CourseUnit.WeekNumberStart,
                    WeekNumberEnd = x.CourseUnit.WeekNumberEnd
                } : null,
                Grades = x.Grades.Select(y => new ENTITIES.Models.Intranet.Grade
                {
                    Attended = y.Attended,
                    StudentSection = new StudentSection
                    {
                        Id = y.StudentSectionId,
                        FinalGrade = y.StudentSection.FinalGrade,
                        SectionId = y.StudentSection.SectionId
                    }
                })
                .ToList()
            }).ToListAsync();

        public async Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetAllByCourseUnit(Guid courseUnitId)
            => await _context.Evaluations.Where(x => x.CourseUnitId == courseUnitId).ToListAsync();

        public async Task<object> GetAsModelB(Guid? id = null)
        {
            var query = _context.Evaluations.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id.Value);

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    unitName = x.CourseUnit.Name,
                    unitId = x.CourseUnitId,
                    name = x.Name,
                    percentage = x.Percentage,
                    retrievable = x.Retrievable,
                    week = x.Week.HasValue ? $"Semana {x.Week}" : "No asignado",
                    weekNumber = x.Week,
                    evaluationTypeId = x.EvaluationTypeId,
                    evaluationType = x.EvaluationType.Name,
                    x.Description
                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<int> GenerateUnitsAndEvaluationsJob()
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            var courses = await _context.Courses.Include(x => x.CourseComponent).ToListAsync();
            var syllabuses = await _context.CourseSyllabus.Where(x => x.TermId == term.Id).Include(x => x.ListCourseUnit).ToListAsync();

            var count = 0;

            foreach (var course in courses)
            {
                if (syllabuses.Any(x => x.CourseId == course.Id)) continue;

                var syllabus = new CourseSyllabus
                {
                    CourseId = course.Id,
                    TermId = term.Id
                };

                if (course.CourseComponent.QuantityOfUnits == 2)
                {
                    syllabus.ListCourseUnit = new List<CourseUnit>
                    {
                        new CourseUnit { Name = "Unidad 1", Number = 1, WeekNumberStart = 1, WeekNumberEnd = 2 },
                        new CourseUnit { Name = "Unidad 2", Number = 2, WeekNumberStart = 3, WeekNumberEnd = 4 }
                    };
                }

                if (course.CourseComponent.QuantityOfUnits == 3)
                {
                    syllabus.ListCourseUnit = new List<CourseUnit>
                    {
                        new CourseUnit { Name = "Unidad 1", Number = 1, WeekNumberStart = 1, WeekNumberEnd = 2 },
                        new CourseUnit { Name = "Unidad 2", Number = 2, WeekNumberStart = 3, WeekNumberEnd = 4 },
                        new CourseUnit { Name = "Unidad 3", Number = 3, WeekNumberStart = 5, WeekNumberEnd = 6 }
                    };
                }

                await _context.CourseSyllabus.AddAsync(syllabus);
                await _context.SaveChangesAsync();

                var competenceEvaluations = await _context.CompetenceEvaluations.ToListAsync();

                var courseTerm = await _context.CourseTerms.FirstOrDefaultAsync(x => x.CourseId == course.Id && x.TermId == term.Id);
                if (courseTerm == null)
                {
                    courseTerm = new CourseTerm
                    {
                        CourseId = course.Id,
                        Temary = "",
                        TermId = term.Id,
                        WeekHours = 6
                    };

                    await _context.CourseTerms.AddAsync(courseTerm);
                    await _context.SaveChangesAsync();
                }

                var exams = new List<ENTITIES.Models.Enrollment.Evaluation>();
                foreach (var unit in syllabus.ListCourseUnit)
                {
                    foreach (var evaluation in competenceEvaluations)
                    {
                        var exam = new ENTITIES.Models.Enrollment.Evaluation
                        {
                            CourseTermId = courseTerm.Id,
                            Name = $"{unit.Name} {evaluation.Name}",
                            Percentage = evaluation.Percentage,
                            Retrievable = false,
                            CourseUnitId = unit.Id,
                            EvaluationTypeId = evaluation.EvaluationTypeId
                        };
                        exams.Add(exam);
                    }
                }
                await _context.Evaluations.AddRangeAsync(exams);
                await _context.SaveChangesAsync();

                count++;
            }

            await _context.SaveChangesAsync();
            return count;
        }

        public async Task<IEnumerable<ENTITIES.Models.Enrollment.Evaluation>> GetEvaluationsByClass(Guid classId)
        {
            var @class = await _context.Classes.Where(x => x.Id == classId)
                .Select(x => new
                {
                    x.Section.CourseTermId,
                    x.WeekNumber,
                    x.SectionId
                })
                .FirstOrDefaultAsync();

            var classEvaluationsInWeek = await _context.Classes.Where(x => x.Id != classId && x.SectionId == @class.SectionId && x.WeekNumber == @class.WeekNumber && x.EvaluationId.HasValue)
                .Select(x => x.EvaluationId).ToListAsync();

            var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == @class.CourseTermId && x.Week == @class.WeekNumber && !classEvaluationsInWeek.Contains(x.Id))
                .ToListAsync();

            return evaluations;
        }

    }
}