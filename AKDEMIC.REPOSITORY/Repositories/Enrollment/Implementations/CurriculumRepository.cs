using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Curriculum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class CurriculumRepository : Repository<Curriculum>, ICurriculumRepository
    {
        public CurriculumRepository(AkdemicContext context) : base(context) { }

        public async Task ActivateCurriculum(Guid careerId, Guid curriculumId)
        {
            //var curriculums = await _context.Curriculums.Where(c => c.CareerId == careerId).ToListAsync();

            //foreach (var item in curriculums)
            //{
            //    item.IsActive = false;

            //    if (item.Id == curriculumId)
            //    {
            //        item.IsNew = false;
            //        item.IsActive = true;
            //    }
            //}

            var curriculum = await _context.Curriculums.FindAsync(curriculumId);
            curriculum.IsNew = false;
            curriculum.IsActive = true;

            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> CurriculumsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null, Guid? termId = null)
        {
            var query = _context.Curriculums.AsQueryable();

            if (termId.HasValue && termId != Guid.Empty)
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.Course.CourseTerms.Any(z => z.TermId == termId)));

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY)
                    )
                {
                    var careers = await _context.Careers.Where(x =>
                    x.CareerDirectorId == userId ||
                    x.AcademicCoordinatorId == userId ||
                    x.AcademicSecretaryId == userId).Select(x => x.Id).ToArrayAsync();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_COORDINATOR))
                {
                    var careers = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId || x.AcademicDepartmentCoordinatorId == userId).Select(x => x.CareerId).ToArrayAsync();
                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            Expression<Func<Curriculum, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Career.Faculty.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Career.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Code); break;
                case "3":
                    orderByPredicate = ((x) => x.ApprovedResolutionDate); break;
                case "4":
                    orderByPredicate = ((x) => x.ApprovedResolutionDate); break;
                case "5":
                    orderByPredicate = ((x) => x.IsActive); break;
                default:
                    query = query.OrderBy(x => x.Career.Faculty.Name).ThenBy(x => x.Code);
                    break;
            }

            var pagedList = query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(c => new
                {
                    faculty = c.Career.Faculty.Name,
                    career = c.Career.Name,
                    academicProgram = c.AcademicProgram.Name,
                    year = c.Code,
                    creationresolution = c.CreationResolutionNumber,
                    aproberesolution = c.ApprovedResolutionNumber ?? "---",
                    isActive = c.IsActive,
                    isNew = c.IsNew,
                    id = c.Id,
                    isUnique = c.Career.Curriculums.Count == 1
                });

            return await pagedList.ToDataTables<object>(sentParameters);
        }

        public async Task<Curriculum> GetActiveByCareer(Guid careerId)
            => await _context.Curriculums.Where(x => x.CareerId == careerId && x.IsActive)
            .OrderByDescending(x => x.Year).ThenByDescending(x => x.Code).FirstOrDefaultAsync();

        public async Task<Curriculum> GetCareerLastCurriculum(Guid id)
            => await _context.Curriculums.Where(x => x.CareerId == id)
            .OrderByDescending(cu => cu.Year)
            .ThenByDescending(x => x.ValidityStart)
            .ThenByDescending(x => x.Code).FirstOrDefaultAsync();

        public async Task<Curriculum> GetCareerPreviousCurriculum(Guid id)
        {
            return await _context.Curriculums.Where(x => x.CareerId == id).OrderByDescending(cu => cu.Year).ThenByDescending(x => x.ValidityStart).ThenByDescending(x => x.Code).Skip(1).FirstOrDefaultAsync();
        }

        public async Task<Curriculum> GetCurriculumPrevCurriculum(Guid curriculumId)
        {
            var curriculum = await _context.Curriculums.FindAsync(curriculumId);
            return await _context.Curriculums.Where(x => x.CareerId == curriculum.CareerId && x.Year < curriculum.Year).OrderByDescending(cu => cu.Year).ThenByDescending(x => x.ValidityStart).ThenByDescending(x => x.Code).FirstOrDefaultAsync();
        }
        public async Task ProcessStudent(Guid studentId, Guid newCurriculumId)
        {
            var student = await _context.Students.FindAsync(studentId);
            var enableMultipleGrades = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENABLE_MULTIPLE_GRADES_EQUIVALENCE));
            var enableDisapprovedCourseEquivalence = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENABLE_DISAPPROVED_COURSE_EQUIVALENCE));

            var academicHistoriesQry = _context.AcademicHistories
                .Where(x => x.StudentId == studentId)
                .AsNoTracking();

            //if (!enableDisapprovedCourseEquivalence)
            //    academicHistoriesQry = academicHistoriesQry.Where(x => x.Approved);

            var academicHistories = await academicHistoriesQry
                .Select(x => new
                {
                    x.CourseId,
                    x.Grade,
                    x.TermId,
                    TermYear = x.Term.Year,
                    TermNumber = x.Term.Number,
                    x.EvaluationReportId,
                    x.Course.Credits,
                    x.Approved,
                    x.Try
                })
                .ToListAsync();

            var courseEquivalences = await _context.CourseEquivalences
                .Where(x => x.OldAcademicYearCourse.CurriculumId == student.CurriculumId
                && x.NewAcademicYearCourse.CurriculumId == newCurriculumId)
                .Select(x => new
                {
                    x.NewAcademicYearCourseId,
                    NewCourseId = x.NewAcademicYearCourse.CourseId,
                    x.OldAcademicYearCourseId,
                    OldCourseId = x.OldAcademicYearCourse.CourseId,
                    x.IsOptional,
                    x.ReplaceGrade,
                    OldCourseCode = x.OldAcademicYearCourse.Course.Code
                }).ToListAsync();

            var courses = courseEquivalences.Select(x => new { x.NewAcademicYearCourseId, x.NewCourseId }).Distinct().ToList();

            var curriculum = await _context.Curriculums.FindAsync(student.CurriculumId);
            var newAcademicHistories = new List<AcademicHistory>();

            foreach (var course in courses)
            {
                var equivalences = courseEquivalences.Where(x => x.NewAcademicYearCourseId == course.NewAcademicYearCourseId).ToList();

                if (equivalences.Count == 1)
                {
                    var equivalence = equivalences.FirstOrDefault();
                    var courseHistories = academicHistories.Where(ah => equivalence.OldCourseId == ah.CourseId && ah.Approved).ToList();

                    if (enableDisapprovedCourseEquivalence)
                        courseHistories = academicHistories.Where(ah => equivalence.OldCourseId == ah.CourseId).ToList();

                    var validate = courseHistories.Any();
                    var wasConvalidated = academicHistories.Any(x => x.CourseId == course.NewCourseId);

                    if (validate && !wasConvalidated)
                    {
                        foreach (var item in courseHistories)
                        {
                            var academicHistory = new AcademicHistory
                            {
                                CourseId = course.NewCourseId,
                                Approved = item.Approved,
                                Validated = true,
                                TermId = item.TermId,
                                Observations = $"Convalidado por llevar: {string.Join(", ", equivalence.OldCourseCode)} plan: {curriculum.Code}",
                                StudentId = student.Id,
                                Try = item.Try,
                                Type = ConstantHelpers.AcademicHistory.Types.REGULAR,
                                Grade = !item.Approved ? item.Grade : -1,
                                CurriculumId = student.CurriculumId
                            };

                            if (equivalence.ReplaceGrade)
                                academicHistory.Grade = item.Grade;

                            academicHistory.EvaluationReportId = item.EvaluationReportId;
                            newAcademicHistories.Add(academicHistory);
                        }
                    }
                }
                else
                {
                    var validate = equivalences.All(x => academicHistories.Any(ah => ah.CourseId == x.OldCourseId && ah.Approved));
                    if (equivalences.Any(x => x.IsOptional))
                        validate = equivalences.Any(x => academicHistories.Any(ah => ah.CourseId == x.OldCourseId && ah.Approved));

                    var termId = academicHistories.Where(x => equivalences.Any(y => y.OldCourseId == x.CourseId) && x.Approved)
                        .OrderByDescending(x => x.TermYear).OrderByDescending(x => x.TermNumber).Select(x => x.TermId).FirstOrDefault();


                    if (validate)
                    {
                        var wasConvalidated = academicHistories.Any(x => x.CourseId == course.NewCourseId);

                        if (!wasConvalidated)
                        {
                            var codes = equivalences.Where(x => academicHistories.Any(ah => ah.CourseId == x.OldCourseId && ah.Approved)).Select(x => x.OldCourseCode).ToList();
                            var academicHistory = new AcademicHistory
                            {
                                CourseId = course.NewCourseId,
                                Approved = true,
                                Validated = true,
                                TermId = termId,
                                Observations = $"Convalidado por llevar: {string.Join(", ", codes)} plan: {curriculum.Code}",
                                StudentId = student.Id,
                                Try = 1,
                                Type = ConstantHelpers.AcademicHistory.Types.REGULAR,
                                Grade = -1,
                                CurriculumId = student.CurriculumId
                            };

                            var replaceEquivalences = equivalences.Where(x => x.ReplaceGrade && academicHistories.Any(ah => ah.CourseId == x.OldCourseId && ah.Approved)).ToList();
                            if (replaceEquivalences.Any())
                            {
                                if (enableMultipleGrades)
                                {
                                    var histories = academicHistories.Where(x => x.Grade > 0 && replaceEquivalences.Any(y => y.OldCourseId == x.CourseId) && x.Approved).ToList();
                                    if (histories.Any())
                                    {
                                        academicHistory.Grade = (int)Math.Round(histories.Sum(x => x.Grade * x.Credits) / histories.Sum(x => x.Credits), 0, MidpointRounding.AwayFromZero);
                                        academicHistory.EvaluationReportId = histories.OrderByDescending(x => x.Grade).FirstOrDefault().EvaluationReportId;
                                    }
                                    else
                                    {
                                        var oldAcademicHistory = academicHistories.Where(x => replaceEquivalences.Any(y => y.OldCourseId == x.CourseId) && x.Approved).OrderByDescending(x => x.Grade).FirstOrDefault();
                                        academicHistory.EvaluationReportId = oldAcademicHistory.EvaluationReportId;
                                    }
                                }
                                else
                                {
                                    var replaceEquivalence = replaceEquivalences.FirstOrDefault();

                                    var oldAcademicHistory = academicHistories.Where(x => x.CourseId == replaceEquivalence.OldCourseId && x.Approved).FirstOrDefault();
                                    academicHistory.Grade = oldAcademicHistory.Grade;
                                    academicHistory.EvaluationReportId = oldAcademicHistory.EvaluationReportId;
                                }
                            }
                            else
                            {
                                academicHistory.EvaluationReportId = academicHistories
                                  .Where(x => equivalences.Any(y => y.OldCourseId == x.CourseId) && x.Approved)
                                  .Select(x => x.EvaluationReportId).FirstOrDefault();
                            }

                            newAcademicHistories.Add(academicHistory);
                        }
                    }
                }
            }

            student.CurriculumId = newCurriculumId;

            await _context.AcademicHistories.AddRangeAsync(newAcademicHistories);
            await _context.SaveChangesAsync();

            #region actualiza ciclo

            var academicYearCredits = await _context.AcademicYearCredits.Where(x => x.CurriculumId == student.CurriculumId).ToListAsync();
            if (academicYearCredits.Any())
            {
                var studentHistories = _context.AcademicHistories
                          .Where(x => x.StudentId == student.Id)
                          .Select(x => new
                          {
                              x.StudentId,
                              x.CourseId,
                              x.Course.Credits,
                              termYear = x.Term.Year,
                              termNumber = x.Term.Number,
                              x.Approved
                          })
                          .ToList();

                var academicYearCourses = await _context.AcademicYearCourses
                   .Where(x => x.CurriculumId == student.CurriculumId)
                   .Select(x => new
                   {
                       x.AcademicYear,
                       x.CourseId,
                       x.IsElective,
                       x.CurriculumId
                   }).ToListAsync();

                var studentCurriculumCourses = academicYearCourses.Where(x => x.CurriculumId == student.CurriculumId).ToList();
                var totalApprovedCredits = studentHistories
                    .Where(x => studentCurriculumCourses.Any(y => y.CourseId == x.CourseId) && x.Approved)
                    .Distinct().Sum(x => x.Credits);

                var summaries = studentHistories
                    .Where(x => x.termNumber == "1" || x.termNumber == "2")
                    .GroupBy(x => new { x.termYear, x.termNumber })
                    .Select(x => new
                    {
                        Year = x.Key.termYear,
                        Number = x.Key.termNumber,
                        Courses = x.ToList(),
                        DisapprovedCredits = x.Where(y => !y.Approved).Sum(y => y.Credits)
                    }).ToList();
                var lastSummary = summaries.OrderBy(x => x.Year).ThenBy(x => x.Number).LastOrDefault();
                var disapprovedCredits = lastSummary == null ? 0
                    : lastSummary.Courses.Where(x => !studentHistories.Any(y => y.CourseId == x.CourseId && y.Approved)).Sum(x => x.Credits);

                var aditionalCredits = 0.0M;
                var curriculumCredits = await _context.CurriculumCredits.FirstOrDefaultAsync(x => x.CurriculumId == student.CurriculumId);
                if (curriculumCredits != null)
                    aditionalCredits = curriculumCredits.CreditsDisapproved <= disapprovedCredits ? curriculumCredits.CreditsObservation : curriculumCredits.MaxCredits;

                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAB || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAH)
                {
                    var prevAcademicYearCredit = academicYearCredits
                    .FirstOrDefault(x => x.CurriculumId == student.CurriculumId && x.StartCredits <= totalApprovedCredits && totalApprovedCredits <= x.EndCredits);
                    if (prevAcademicYearCredit == null)
                    {
                        if (academicYearCredits.Any(x => x.CurriculumId == student.CurriculumId && totalApprovedCredits >= x.StartCredits))
                        {
                            prevAcademicYearCredit = academicYearCredits.Where(x => x.CurriculumId == student.CurriculumId).OrderBy(x => x.AcademicYear).LastOrDefault();
                        }
                    }

                    if (prevAcademicYearCredit != null)
                        aditionalCredits = prevAcademicYearCredit.Credits;
                }

                var totalCredits = totalApprovedCredits + aditionalCredits;

                var academicYearCredit = academicYearCredits.FirstOrDefault(x => x.StartCredits <= totalCredits && totalCredits <= x.EndCredits);
                if (academicYearCredit == null)
                {
                    if (academicYearCredits.Any(x => x.CurriculumId == student.CurriculumId))
                    {
                        academicYearCredit = academicYearCredits.Where(x => x.CurriculumId == student.CurriculumId).OrderBy(x => x.AcademicYear).LastOrDefault();
                    }
                }

                student.CurrentAcademicYear = academicYearCredit != null ? academicYearCredit.AcademicYear : 1;

                await _context.SaveChangesAsync();
            }

            #endregion
        }

        public async Task ProcessStudents(Guid newCurriculumId, ClaimsPrincipal user = null)
        {
            var newCurriculum = await _context.Curriculums.FindAsync(newCurriculumId);
            var prevCurriculum = await GetCurriculumPrevCurriculum(newCurriculumId);
            var enableMultipleGrades = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENABLE_MULTIPLE_GRADES_EQUIVALENCE));

            var query = _context.Students
                .Where(x => x.CurriculumId == prevCurriculum.Id)
                .AsQueryable();

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNICA) query = query.Where(x => x.CurrentAcademicYear <= 8);

            var students = await query
                .ToListAsync();

            var allAcademicHistories = await _context.AcademicHistories
                .Where(x => x.Student.CurriculumId == prevCurriculum.Id && x.Approved)
                .Select(x => new
                {
                    x.StudentId,
                    x.CourseId,
                    x.Grade,
                    x.TermId,
                    TermYear = x.Term.Year,
                    TermNumber = x.Term.Number,
                    x.EvaluationReportId,
                    x.Course.Credits
                })
                .ToListAsync();

            var courseEquivalences = await _context.CourseEquivalences
              .Where(x => x.OldAcademicYearCourse.CurriculumId == prevCurriculum.Id)
              .Select(x => new
              {
                  x.NewAcademicYearCourseId,
                  NewCourseId = x.NewAcademicYearCourse.CourseId,
                  x.OldAcademicYearCourseId,
                  OldCourseId = x.OldAcademicYearCourse.CourseId,
                  x.IsOptional,
                  x.ReplaceGrade,
                  OldCourseCode = x.OldAcademicYearCourse.Course.Code
              }).ToListAsync();

            var courses = courseEquivalences.Select(x => new { x.NewAcademicYearCourseId, x.NewCourseId }).Distinct().ToList();

            var convalidatedCourses = new List<AcademicHistory>();
            var observations = new List<StudentObservation>();

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            var userId = user != null ? user.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;

            foreach (var student in students)
            {
                var academicHistories = allAcademicHistories.Where(x => x.StudentId == student.Id).ToList();
                var newAcademicHistories = new List<AcademicHistory>();

                foreach (var course in courses)
                {
                    var equivalences = courseEquivalences.Where(x => x.NewAcademicYearCourseId == course.NewAcademicYearCourseId).ToList();
                    var validate = equivalences.All(x => academicHistories.Any(ah => ah.CourseId == x.OldCourseId));
                    var termId = academicHistories.Where(x => equivalences.Any(y => y.OldCourseId == x.CourseId))
                        .OrderByDescending(x => x.TermYear).OrderByDescending(x => x.TermNumber).Select(x => x.TermId).FirstOrDefault();

                    if (equivalences.Any(x => x.IsOptional)) validate = equivalences.Any(x => academicHistories.Any(ah => ah.CourseId == x.OldCourseId));

                    if (validate)
                    {
                        var wasConvalidated = academicHistories.Any(x => x.StudentId == student.Id && x.CourseId == course.NewCourseId)
                            || convalidatedCourses.Any(x => x.StudentId == student.Id && x.CourseId == course.NewCourseId && x.Approved);

                        if (!wasConvalidated)
                        {
                            //var grade = -1;
                            //var replaceEquivalence = equivalences
                            //    .Where(x => x.ReplaceGrade && academicHistories.Any(ah => ah.CourseId == x.OldCourseId))
                            //    .FirstOrDefault();

                            //if (replaceEquivalence != null) grade = academicHistories.Where(x => x.CourseId == replaceEquivalence.OldCourseId).First().Grade;

                            var academicHistory = new AcademicHistory
                            {
                                CourseId = course.NewCourseId,
                                Approved = true,
                                Validated = true,
                                TermId = termId,
                                Observations = $"Convalidado por llevar los cursos correspondientes de la malla anterior",
                                StudentId = student.Id,
                                Try = 1,
                                Type = ConstantHelpers.AcademicHistory.Types.REGULAR,
                                Grade = -1,
                                CurriculumId = student.CurriculumId
                            };

                            var replaceEquivalences = equivalences.Where(x => x.ReplaceGrade && academicHistories.Any(ah => ah.CourseId == x.OldCourseId)).ToList();
                            if (replaceEquivalences.Any())
                            {
                                if (enableMultipleGrades)
                                {
                                    var histories = academicHistories.Where(x => x.Grade > 0 && replaceEquivalences.Any(y => y.OldCourseId == x.CourseId)).ToList();
                                    if (histories.Any())
                                    {
                                        academicHistory.Grade = (int)Math.Round(histories.Sum(x => x.Grade * x.Credits) / histories.Sum(x => x.Credits), 0, MidpointRounding.AwayFromZero);
                                        academicHistory.EvaluationReportId = histories.OrderByDescending(x => x.Grade).FirstOrDefault().EvaluationReportId;
                                    }
                                    else
                                    {
                                        var oldAcademicHistory = academicHistories.Where(x => replaceEquivalences.Any(y => y.OldCourseId == x.CourseId)).OrderByDescending(x => x.Grade).FirstOrDefault();
                                        academicHistory.EvaluationReportId = oldAcademicHistory.EvaluationReportId;
                                    }
                                }
                                else
                                {
                                    var replaceEquivalence = replaceEquivalences.FirstOrDefault();

                                    var oldAcademicHistory = academicHistories.Where(x => x.CourseId == replaceEquivalence.OldCourseId).FirstOrDefault();
                                    academicHistory.Grade = oldAcademicHistory.Grade;
                                    academicHistory.EvaluationReportId = oldAcademicHistory.EvaluationReportId;
                                }
                            }
                            else
                            {
                                academicHistory.EvaluationReportId = academicHistories
                                  .Where(x => equivalences.Any(y => y.OldCourseId == x.CourseId))
                                  .Select(x => x.EvaluationReportId).FirstOrDefault();
                            }

                            newAcademicHistories.Add(academicHistory);
                            convalidatedCourses.Add(academicHistory);

                        }
                    }
                }

                await _context.AcademicHistories.AddRangeAsync(newAcademicHistories);

                student.CurriculumId = newCurriculum.Id;

                var observation = new StudentObservation
                {
                    StudentId = student.Id,
                    Observation = "Convalidación de la malla anterior por tabla de equivalencia",
                    UserId = userId,
                    Type = ConstantHelpers.OBSERVATION_TYPES.EQUIVALENCE,
                    TermId = term != null ? term.Id : (Guid?)null
                };
                observations.Add(observation);
            }

            await _context.StudentObservations.AddRangeAsync(observations);
            await _context.SaveChangesAsync();

            prevCurriculum.IsActive = false;

            newCurriculum.IsNew = false;
            newCurriculum.IsActive = true;

            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumDatatable(DataTablesStructs.SentParameters sentParameters, Guid faculty, Guid career, Guid academicProgram, string searchValue, ClaimsPrincipal user = null)
        {
            Expression<Func<Curriculum, dynamic>> orderByPredicate = null;


            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Career.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.AcademicProgram.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Code;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.Curriculums.AsTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

            if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);

            if (academicProgram != Guid.Empty) query = query.Where(x => x.AcademicProgramId == academicProgram);

            if (!string.IsNullOrEmpty(searchValue)) query = query.Where(x => x.Career.Name.ToUpperInvariant().Contains(searchValue.ToUpperInvariant()) || x.Career.Code.ToUpperInvariant().Contains(searchValue.ToUpperInvariant()));

            query = query.AsQueryable();
            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    academicProgram = x.AcademicProgram.Name,
                    curriculum = $"{x.Year}-{x.Code}",
                    curriculumCode = x.Code,
                    isActive = x.IsActive
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<Curriculum> GetCurriculumByCareerId(Guid careerId)
        {
            var curriculum = await _context.Curriculums.FirstOrDefaultAsync(x => x.CareerId == careerId && x.IsActive);
            return curriculum;
        }

        public async Task<int> CountAsync(Guid? careerId = null)
        {
            var query = _context.Curriculums.AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId.Value);

            return await query.CountAsync();
        }

        public async Task<CurriculumTemplateA> GetAsModelA(Guid? id = null)
        {
            var entity = await _context.Curriculums
                .Where(x => x.Id == id)
                .Select(x => new CurriculumTemplateA
                {
                    Faculty = x.Career.Faculty.Name ?? "-",
                    Title = x.Career.Name ?? "-",
                    Year = x.Year,
                    Code = x.Code
                })
                //.Select(x => new CurriculumTemplateA
                //{
                //    //Faculty = faculties.Any(y => y.Id == x.ca.FacultyId) ? faculties.First(y => y.Id == x.ca.FacultyId).Name : "--",
                //    Title = x.ca.Name,
                //    Year = x.cu.Year,
                //})
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<object> GetAllAsModelB(Guid? facultyId = null, Guid? careerId = null, string coordinatorId = null)
        {
            var query = _context.Curriculums.AsQueryable();

            if (facultyId.HasValue)
                query = query.Where(x => x.CareerId == careerId.Value);

            if (careerId.HasValue)
                query = query.Where(x => x.Career.FacultyId == facultyId.Value);


            if (!string.IsNullOrEmpty(coordinatorId))
            {
                var careers = GetCoordinatorCareers(coordinatorId);
                query = query.Where(x => careers.Any(y => y == x.CareerId));
            }


            var result = await query
                .Select(c => new
                {
                    faculty = c.Career.Faculty.Name,
                    career = c.Career.Name,
                    year = $"{c.Year.ToString()} - {c.Code}",
                    creationresolution = c.CreationResolutionNumber,
                    aproberesolution = c.ApprovedResolutionNumber,
                    isActive = c.IsActive,
                    isNew = c.IsNew,
                    isUnique = c.Career.Curriculums.Count == 1,
                    id = c.Id
                }).ToListAsync();

            return result;
        }

        public async Task<CurriculumTemplateC> GetAsModelC(Guid? id = null)
        {
            var query = _context.Curriculums.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id);

            var model = await query
                .Select(x => new CurriculumTemplateC
                {
                    Id = x.Id,
                    Career = x.Career.Name,
                    Term = x.Year.ToString(),
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<CurriculumTemplateD> GetAsModelD(Guid? id = null)
        {

            var query = _context.Curriculums.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id);

            var model = await query
                .Select(x => new CurriculumTemplateD
                {
                    Id = x.Id,
                    AcademicProgramId = x.AcademicProgramId,
                    FacultyId = x.Career.FacultyId,
                    CareerId = x.CareerId,
                    Career = x.Career.Name,
                    Year = x.Year,
                    ValidityStart = x.ValidityStart.ToLocalDateFormat(),
                    Code = x.Code,
                    ValidityEnd = x.ValidityEnd.ToLocalDateFormat(),
                    IsActive = x.IsActive,
                    IsNew = x.IsNew,
                    StudyRegime = x.StudyRegime,
                    CurriculumName = x.Code,
                    CreationResolution = x.CreationResolutionNumber,
                    CreationResolutionDate = x.CreationResolutionDate.ToLocalDateFormat(),
                    AprobeResolution = x.ApprovedResolutionNumber,
                    AprobeResolutionDate = x.ApprovedResolutionDate.ToLocalDateFormat(),
                    UniversityAssemblyResolution = x.UniversityAssemblyResolution,
                    UniversityAssemblyResolutionDate = x.UniversityAssemblyResolutionDate.ToLocalDateFormat(),
                    UniversityAssemblyResolutionFile = x.UniversityAssemblyResolutionFile,
                    //GeneralSkills = x.GeneralSkills,
                    //SpecificSkills = x.SpecificSkills,
                    AcademicDegreeProfessionalTitle = x.AcademicDegreeProfessionalTitle,
                    AcademicDegreeBachelor = x.AcademicDegreeBachelor,
                    RequiredCredits = x.RequiredCredits,
                    ElectiveCredits = x.ElectiveCredits,
                    ExtracurricularCredits = x.ExtracurricularCredits,
                    HasCompetencies = x.CurriculumCompetencies.Any(),
                    ProfessionalPracticeHours = x.ProfessionalPracticeHours,
                    CurricularDesingFileUrl = x.CurricularDesignFile
                }).FirstAsync();

            return model;
        }

        public Task<bool> AnyNewByCareerId(Guid? careerId = null)
            => _context.Curriculums.Where(x => x.CareerId == careerId).AnyAsync(x => x.IsNew);

        public async Task<List<Curriculum>> GetAllCurriculumsByCareer(Guid? careerId = null, Guid? academicProgramId = null)
        {
            var query = _context.Curriculums.AsQueryable();

            if (careerId != null) query = query.Where(x => x.CareerId == careerId);

            if (academicProgramId.HasValue) query = query.Where(x => x.AcademicYearCourses.Any(y => y.Course.AcademicProgramId == academicProgramId));

            return await query.ToListAsync();
        }

        public async Task<object> GetCareerCurriculumJson(Guid id, bool? onlyActive = null)
        {
            var query = _context.Curriculums.AsQueryable();

            if (id != Guid.Empty)
                query = query.Where(x => x.CareerId == id);

            if (onlyActive.HasValue && onlyActive.Value)
                query = query.Where(x => x.IsActive);

            var result = await query
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Code)
                .Select(ay => new
                {
                    id = ay.Id,
                    text = $"{ay.Year}-{ay.Code}"
                })
                .ToListAsync();

            return result;
        }

        public async Task<object> GetAcademicProgramsCurriculumJson(Guid id, bool? onlyActive = false)
        {
            //var query_courses = _context.Courses.AsQueryable();
            //if (id != Guid.Empty)
            //    query_courses = query_courses.Where(x => x.AcademicProgramId == id);

            //var courses = await query_courses.Select(x => x.Id).ToArrayAsync();
            //var query = _context.AcademicYearCourses.Include(y => y.Curriculum).Where(x => courses.Any(y => y == x.CourseId)).AsNoTracking();
            //var query = _context.AcademicYearCourses.Include(y=>y.Curriculum).Where(y => query_courses.Select(x => x.Id).ToList().Contains(y.CourseId));

            var query = _context.Curriculums
                .Where(x => x.AcademicProgramId == id)
                .AsNoTracking();

            if (onlyActive.HasValue && onlyActive.Value)
                query = query.Where(x => x.IsActive);

            var result = await query
                .Select(ay => new
                {
                    //id = ay.Curriculum.Id,
                    //text = $"{ay.Curriculum.Year}-{ay.Curriculum.Code}",
                    id = ay.Id,
                    text = $"{ay.Year}-{ay.Code}"
                })
                //.Distinct()
                .ToListAsync();

            var query2 = _context.AcademicYearCourses
                .Where(x => x.Course.AcademicProgramId == id)
                .AsNoTracking();

            if (onlyActive.HasValue && onlyActive.Value)
                query2 = query2.Where(x => x.Curriculum.IsActive);

            var result2 = await query2
                 .Select(ay => new
                 {
                     id = ay.Curriculum.Id,
                     text = $"{ay.Curriculum.Year}-{ay.Curriculum.Code}"
                 })
                .Distinct()
                .ToListAsync();

            result.AddRange(result2);

            result = result.Distinct().ToList();

            return result;
        }

        public async Task<object> GetAllNumberPlusCodeByCareerId(Guid careerId, bool onlyActive = false)
        {
            var query = _context.Curriculums.Where(x => x.CareerId == careerId)
                .AsNoTracking();

            if (onlyActive)
                query = query.Where(x => x.IsActive);

            var alll = await query
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Code)
                .Select(
                    x => new
                    {
                        x.Id,
                        Text = $"{x.Year} - {x.Code}"
                    }
                ).ToListAsync();
            return alll;
        }

        public async Task<List<byte>> GetAllAcademicYears(Guid? curriculumId = null)
        {
            var query = _context.AcademicYearCourses
                .AsNoTracking();

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CurriculumId == curriculumId);

            var courses = await query.Select(x => x.AcademicYear).ToListAsync();

            var years = courses.OrderBy(x => x).Distinct().ToList();

            return years;
        }

        public async Task UpdateCurriculumsAcademicProgramJob()
        {
            var curriculums = await _context.Curriculums.ToListAsync();
            var academicPrograms = await _context.AcademicPrograms.ToListAsync();

            foreach (var item in curriculums)
            {
                item.AcademicProgramId = academicPrograms.Where(x => x.CareerId == item.CareerId).Select(x => x.Id).FirstOrDefault();
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Curriculum> GetFirstByCareerAndCurriculumCode(string careerCode, string curriculumCode)
        {
            return await _context.Curriculums.FirstOrDefaultAsync(x => x.Career.Code.ToUpper() == careerCode.ToUpper() && (x.Code.ToUpper() == curriculumCode.ToUpper() || x.RelationId.ToUpper() == curriculumCode.ToUpper()));
        }

        public async Task<object> GetAllByAcademicProgramOrCareerJson(Guid careerId, Guid? academicProgramId = null, ClaimsPrincipal user = null, bool? onlyActive = false)
        {
            var query = _context.Curriculums
               .AsNoTracking();

            if (careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                query = query.Where(x => x.AcademicYearCourses.Any(y => y.Course.AcademicProgramId == academicProgramId));

            if (onlyActive.HasValue && onlyActive.Value)
                query = query.Where(x => x.IsActive);

            //if (academicProgramId == null || academicProgramId == Guid.Empty)
            //{
            //    query = query.Where(x => x.CareerId == careerId);
            //}
            //else
            //{
            //    //query = query.Where(x => x.AcademicProgramId == academicProgramId);
            //}

            if (user != null)
            {
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL))
                {
                    var firstCareerId = await _context.Careers.Select(x => x.Id).FirstOrDefaultAsync();
                    query = query.Where(x => x.CareerId == firstCareerId);
                }
            }

            var result = await query
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Code)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Year}-{x.Code}"
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<Guid>> GetCoursesWithSyllabusId(Guid termId, Guid careerId, Guid curriculumId)
        {
            var result = await _context.CourseTerms
                .Where(x => x.TermId == termId &&
                x.Course.CareerId == careerId &&
                x.SyllabusTeachers.Any() &&
                x.Course.AcademicYearCourses.Any(y => y.CurriculumId == curriculumId))
                .Select(x => x.CourseId).ToListAsync();

            return result;
        }

        public async Task<List<Competencie>> GetCurriculumCompetencies(Guid curriculumId, byte? type)
        {
            var query = _context.CurriculumCompetencies
                .AsNoTracking();

            if (type.HasValue)
                query = query.Where(x => x.Competencie.Type == type);

            var data = await query
                .Where(x => x.CurriculumId == curriculumId)
                .Select(x => new Competencie
                {
                    Id = x.CompetencieId,
                    Type = x.Competencie.Type,
                    Name = x.Competencie.Name
                })
                .ToListAsync();

            return data;
        }

        public async Task<Tuple<string, bool>> UpdateCurriculumCompetence(Guid curriculumId, List<Guid> competences)
        {
            var componentsUsed = await _context.AcademicYearCourses.Where(x => x.CurriculumId == curriculumId && x.CompetencieId.HasValue).Select(x => x.CompetencieId.Value).Distinct().ToListAsync();

            if (componentsUsed.Any())
            {
                var rest = componentsUsed.Where(x => !competences.Contains(x)).ToList();

                if (rest.Any())
                {
                    var competencies = await _context.Competencies.Where(x => x.Id == rest.FirstOrDefault()).Select(x => x.Name).FirstOrDefaultAsync();
                    return new Tuple<string, bool>($"La competencia '{competencies}' esta relacionada con los cursos del plan y no puede ser eliminada.", false);
                }
            }

            var currentCompentences = await _context.CurriculumCompetencies.Where(x => x.CurriculumId == curriculumId).ToListAsync();
            _context.CurriculumCompetencies.RemoveRange(currentCompentences);

            var entities = competences
                .Select(x => new CurriculumCompetencie
                {
                    CompetencieId = x,
                    CurriculumId = curriculumId,
                    Description = currentCompentences.Where(y => y.CompetencieId == x).Select(y => y.Description).FirstOrDefault()
                })
                .ToList();

            await _context.CurriculumCompetencies.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return new Tuple<string, bool>(string.Empty, true);
        }

        public async Task<Curriculum> GetFirstByCourse(Guid courseId)
        {
            return await _context.AcademicYearCourses
                .Where(x => x.CourseId == courseId)
                .Select(x => x.Curriculum)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<List<Curriculum>> GetCurriculumsWithCareer()
            => await _context.Curriculums.Include(x => x.Career).Include(x => x.AcademicProgram).ToListAsync();

        public async Task<object> GetCurriculumsLastYearActiveByCareerSelect2(Guid careerId)
        {
            int year = 0;
            year = await _context.Curriculums
                .Where(x => x.CareerId == careerId && x.IsActive)
                .OrderByDescending(x => x.Year)
                .Select(x => x.Year)
                .FirstOrDefaultAsync();

            var result = await _context.Curriculums
                .Where(x => x.CareerId == careerId && x.IsActive && x.Year == year)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Year}-{x.Code}"
                })
                .ToListAsync();

            return result;
        }
    }
}