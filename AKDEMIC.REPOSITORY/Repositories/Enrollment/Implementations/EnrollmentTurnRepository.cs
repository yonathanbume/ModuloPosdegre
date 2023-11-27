using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentTurn;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class EnrollmentTurnRepository : Repository<EnrollmentTurn>, IEnrollmentTurnRepository
    {
        public EnrollmentTurnRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<Tuple<bool, string>> GenerateTurns(Guid enrollmentShiftId)
        {
            var minutesBetweenShifts = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.MINUTES_BETWEEN_SHIFTS);
            var creditsModality = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.TERM_CREDITS_MODALITY);
            var regularCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REGULAR_MAXIMUM_CREDITS));
            var onlyRegulars = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_TURN_ONLY_FOR_REGULAR));

            var enrollmentShift = await _context.EnrollmentShifts.Include(x => x.Term).FirstOrDefaultAsync(x => x.Id == enrollmentShiftId);

            var careerShifts = await _context.CareerEnrollmentShifts
                .Where(c => c.EnrollmentShiftId == enrollmentShift.Id)
                .ToListAsync();
            var careersHashSet = careerShifts.Select(x => x.CareerId).ToHashSet();

            var academicYearCourses = await _context.AcademicYearCourses
                .Where(x => careersHashSet.Contains(x.Curriculum.CareerId))
                .Select(x => new
                {
                    x.CourseId,
                    x.AcademicYear,
                    x.Course.Credits,
                    x.CurriculumId,
                    x.IsElective,
                    x.Course.AcademicProgramId,
                    AcademicProgramCode = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Code : ""
                }).ToListAsync();

            var averageGradeCredits = await _context.AverageGradeCreditConfigurations.ToListAsync();

            var extraCreditsModality = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.EXTRA_CREDITS_MODALITY));
            var unbeatenStudentCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENT_CREDITS));
            var unbeatenStudentCreditsByRange = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENTS_CREDITS_BY_RANGE));
            var extraCredisConfigurations = await _context.ExtraCreditConfigurations.ToListAsync();
            var academicYearCredits = await _context.AcademicYearCredits.ToListAsync();

            var enableLowGradeCredits = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENABLE_CREDITS_FOR_LOW_GRADE_STUDENTS));
            var lowGradeMaximumGrade = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.LOW_GRADE_STUDENTS_MAXIMUM_GRADE));
            var lowGradeCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.LOW_GRADE_STUDENTS_CREDITS));

            foreach (var careerShift in careerShifts)
            {
                var currentTurns = await _context.EnrollmentTurns
                    .Where(x => x.TermId == enrollmentShift.TermId && x.Student.CareerId == careerShift.CareerId)
                    .ToListAsync();

                var query = _context.Students
                    .Where(x => x.CareerId == careerShift.CareerId)
                    .FilterActiveStudents()
                    .AsNoTracking();

                if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNICA)
                    query = query.Where(x => x.AcademicSummaries.All(y => y.TermHasFinished));

                var allStudents = await query
                       .Select(s => new
                       {
                           s.User.UserName,
                           s.Id,
                           s.CareerId,
                           Order = s.CurrentMeritOrder,
                           s.Status,
                           s.CurriculumId,
                           Curriculum = s.Curriculum.Code,
                           s.CurrentAcademicYear,
                           s.AcademicProgramId,
                           s.UserId,
                           s.CurrentMeritType,
                           AverageGrade = s.AcademicSummaries.Where(y => y.Term.Number == "1" || y.Term.Number == "2").OrderByDescending(y => y.Term.StartDate).Select(y => y.WeightedAverageGrade).FirstOrDefault()
                       }).ToListAsync();

                var qryStudents = allStudents
                    .Where(x => x.CareerId == careerShift.CareerId);

                if (onlyRegulars) qryStudents = qryStudents.Where(x => x.Status == ConstantHelpers.Student.States.REGULAR || x.Status == ConstantHelpers.Student.States.UNBEATEN || x.Status == ConstantHelpers.Student.States.HIGH_PERFORMANCE);

                var students = qryStudents
                    .OrderBy(s => s.Order)
                    .ToList();

                var totalTicks = 1.0;
                if (minutesBetweenShifts != 0) totalTicks = careerShift.EndDateTime.Subtract(careerShift.StartDateTime).TotalMinutes / minutesBetweenShifts;

                var studentPerTick = (int)Math.Ceiling(students.Count / totalTicks);
                var studentsCount = 0;

                var academicSummaries = await _context.AcademicSummaries
                    .Where(x => x.Student.CareerId == careerShift.CareerId && !x.WasWithdrawn)
                    .Select(x => new
                    {
                        x.StudentId,
                        TermYear = x.Term.Year,
                        TermNumber = x.Term.Number,
                        x.WeightedAverageGrade,
                        x.TermId
                    }).ToListAsync();

                var academicHistories = await _context.AcademicHistories
                  .Where(x => x.Student.CareerId == careerShift.CareerId && !x.Withdraw)
                  .Select(x => new
                  {
                      x.StudentId,
                      x.CourseId,
                      x.Course.Credits,
                      termYear = x.Term.Year,
                      termNumber = x.Term.Number,
                      x.Approved,
                      x.Term.StartDate,
                      x.Try,
                      x.Course.Code,
                      x.TermId,
                      termName = x.Term.Name,
                      x.Validated
                  }).ToListAsync();

                var enrollmentTurns = new List<EnrollmentTurn>();

                for (var i = 0; i < totalTicks; i++)
                {
                    if (studentsCount >= students.Count) break;

                    var group = students.GetRange(studentsCount, Math.Min(studentPerTick, students.Count - studentsCount));
                    studentsCount += studentPerTick;
                    var time = careerShift.StartDateTime.AddMinutes(i * minutesBetweenShifts);

                    foreach (var student in group)
                    {
                        if (student.UserName == "12210701")
                        {
                            var entro = true;
                        }
                        var creditsQry = academicYearCourses
                            .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear && !x.IsElective);
                        if (student.AcademicProgramId.HasValue)
                            creditsQry = creditsQry.Where(x => !x.AcademicProgramId.HasValue || (x.AcademicProgramId.HasValue && (x.AcademicProgramId == student.AcademicProgramId || x.AcademicProgramCode == "00")));

                        var creditsBasedOnCurriculum = creditsQry.Sum(x => x.Credits);

                        var credits = creditsModality == ConstantHelpers.Term.CreditsModality.MAXIMUM_CREDITS_BASED_ON_CURRICULUM ? creditsBasedOnCurriculum : regularCredits;

                        if (creditsModality == ConstantHelpers.Term.CreditsModality.ACADEMIC_YEAR_CREDITS_CONFIGURATION)
                        {
                            var academicYearCredit = academicYearCredits
                                              .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear)
                                              .FirstOrDefault();
                            if (academicYearCredit == null)
                                return new Tuple<bool, string>(false, $"No existe configuración de créditos para el plan {student.Curriculum} y ciclo {student.CurrentAcademicYear}");

                            credits = academicYearCredit.Credits;
                        }

                        var academicSummary = academicSummaries
                            .Where(x => x.StudentId == student.Id)
                            .OrderByDescending(x => x.TermYear)
                            .ThenByDescending(x => x.TermNumber)
                            .FirstOrDefault();

                        if (creditsModality == ConstantHelpers.Term.CreditsModality.LAST_AVERAGE_GRADE_CREDITS)
                        {
                            var averageGradeCredit = averageGradeCredits
                                              .Where(x => ((x.GreaterThan && x.AverageGradeStart <= academicSummary.WeightedAverageGrade) || (!x.GreaterThan && x.AverageGradeStart < academicSummary.WeightedAverageGrade))
                                              && ((x.LessThan && academicSummary.WeightedAverageGrade <= x.AverageGradeEnd) || (!x.LessThan && academicSummary.WeightedAverageGrade < x.AverageGradeEnd)))
                                              .FirstOrDefault();

                            if (averageGradeCredit == null)
                                return new Tuple<bool, string>(false, $"No existe configuración de créditos para el prom. {academicSummary.WeightedAverageGrade}");

                            credits = averageGradeCredit.Credits;
                        }

                        if (extraCreditsModality == ConstantHelpers.Configuration.Enrollment.ExtraCreditModality.UNBEATEN_STUDENTS)
                        {
                            if (student.Status == ConstantHelpers.Student.States.UNBEATEN || student.Status == ConstantHelpers.Student.States.HIGH_PERFORMANCE)
                            {
                                if (unbeatenStudentCreditsByRange)
                                {
                                    var extraCreditsConfiguration = extraCredisConfigurations.FirstOrDefault(x => x.AverageGradeStart <= student.AverageGrade && student.AverageGrade <= x.AverageGradeEnd);
                                    if (extraCreditsConfiguration != null)
                                        credits += extraCreditsConfiguration.Credits;
                                }
                                else
                                    credits += unbeatenStudentCredits;
                            }
                        }
                        else
                        {
                            var extraCreditsConfiguration = extraCredisConfigurations.FirstOrDefault(x => x.MeritType == student.CurrentMeritType);
                            if (extraCreditsConfiguration != null)
                                credits += extraCreditsConfiguration.Credits;
                        }

                        if (enableLowGradeCredits && academicSummary != null)
                        {
                            if (academicSummary.WeightedAverageGrade <= lowGradeMaximumGrade)
                                credits = lowGradeCredits;
                        }

                        if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAB)
                        {
                            var studentCurriculumCourses = academicYearCourses.Where(x => x.CurriculumId == student.CurriculumId).Select(x => x.CourseId).ToList();

                            //validar notas del ciclo pasado
                            var studentHistories = academicHistories
                                .Where(x => x.StudentId == student.Id && studentCurriculumCourses.Contains(x.CourseId))
                                .GroupBy(x => x.CourseId)
                                .Select(x => new
                                {
                                    CourseId = x.Key,
                                    Try = x.Max(y => y.Try),
                                    Approved = x.Any(y => y.Approved)
                                })
                                .ToList();

                            if (studentHistories.Any(x => x.Try >= 2 && !x.Approved))
                                credits = 12;
                        }

                        if (currentTurns.Any(x => x.StudentId == student.Id))
                        {
                            var turn = currentTurns.Where(x => x.StudentId == student.Id).FirstOrDefault();

                            if (!turn.SpecialEnrollment)
                            {
                                turn.Time = time;
                                turn.CreditsLimit = credits;
                                turn.IsOnline = true;
                            }
                        }
                        else
                        {
                            var enrollmentTurn = new EnrollmentTurn
                            {
                                TermId = enrollmentShift.TermId,
                                StudentId = student.Id,
                                Time = time,
                                CreditsLimit = credits
                            };

                            enrollmentTurns.Add(enrollmentTurn);
                        }
                    }
                }

                if (onlyRegulars)
                {
                    var inPersonEnrollmentStudents = allStudents
                    .Where(x => x.CareerId == careerShift.CareerId
                    && x.Status != ConstantHelpers.Student.States.REGULAR
                    && x.Status != ConstantHelpers.Student.States.UNBEATEN
                    && x.Status != ConstantHelpers.Student.States.HIGH_PERFORMANCE)
                    .ToList();

                    var inPersonEnrollmentTurns = new List<EnrollmentTurn>();

                    foreach (var student in inPersonEnrollmentStudents)
                    {
                        if (student.UserName == "12210701")
                        {
                            var entro = true;
                        }
                        var credits = regularCredits;

                        if (creditsModality == ConstantHelpers.Term.CreditsModality.MAXIMUM_CREDITS_BASED_ON_CURRICULUM)
                        {
                            var creditsQry = academicYearCourses
                                              .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear && !x.IsElective);
                            if (student.AcademicProgramId.HasValue)
                            {
                                creditsQry = creditsQry.Where(x => !x.AcademicProgramId.HasValue || (x.AcademicProgramId.HasValue && (x.AcademicProgramId == student.AcademicProgramId || x.AcademicProgramCode == "00")));
                            }
                            credits = creditsQry.Sum(x => x.Credits);
                        }

                        if (creditsModality == ConstantHelpers.Term.CreditsModality.ACADEMIC_YEAR_CREDITS_CONFIGURATION)
                        {
                            var academicYearCredit = academicYearCredits
                                              .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear)
                                              .FirstOrDefault();
                            if (academicYearCredit == null)
                                return new Tuple<bool, string>(false, $"No existe configuración de créditos para el plan {student.Curriculum} y ciclo {student.CurrentAcademicYear}");

                            credits = academicYearCredit.Credits;
                        }

                        var academicSummary = academicSummaries
                           .Where(x => x.StudentId == student.Id)
                           .OrderByDescending(x => x.TermYear)
                           .ThenByDescending(x => x.TermNumber)
                           .FirstOrDefault();

                        if (creditsModality == ConstantHelpers.Term.CreditsModality.LAST_AVERAGE_GRADE_CREDITS)
                        {
                            var averageGradeCredit = averageGradeCredits
                                              .Where(x => ((x.GreaterThan && x.AverageGradeStart <= academicSummary.WeightedAverageGrade) || (!x.GreaterThan && x.AverageGradeStart < academicSummary.WeightedAverageGrade))
                                              && ((x.LessThan && academicSummary.WeightedAverageGrade <= x.AverageGradeEnd) || (!x.LessThan && academicSummary.WeightedAverageGrade < x.AverageGradeEnd)))
                                              .FirstOrDefault();

                            if (averageGradeCredit == null)
                                return new Tuple<bool, string>(false, $"No existe configuración de créditos para el prom. {academicSummary.WeightedAverageGrade}");

                            credits = averageGradeCredit.Credits;
                        }

                        if (enableLowGradeCredits && academicSummary != null)
                        {
                            if (academicSummary.WeightedAverageGrade <= lowGradeMaximumGrade)
                                credits = lowGradeCredits;
                        }

                        if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAB
                            || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNIFSLB)
                        {
                            var studentCurriculumCourses = academicYearCourses.Where(x => x.CurriculumId == student.CurriculumId).Select(x => x.CourseId).ToList();
                            var studentHistories = academicHistories.Where(x => x.StudentId == student.Id && studentCurriculumCourses.Contains(x.CourseId)).ToList();

                            var studentLastTries = studentHistories
                                .GroupBy(x => x.CourseId)
                                .Select(x => new
                                {
                                    CourseId = x.Key,
                                    Approved = x.Any(y => y.Approved),
                                    Try = x.OrderByDescending(y => y.Try).Select(y => y.Try).FirstOrDefault()
                                }).ToList();

                            if (studentLastTries.Any(x => !x.Approved && x.Try == 2))
                                credits = ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNIFSLB ? 16 : 12;
                        }

                        if (currentTurns.Any(x => x.StudentId == student.Id))
                        {
                            var turn = currentTurns.Where(x => x.StudentId == student.Id).FirstOrDefault();

                            if (!turn.SpecialEnrollment)
                            {
                                turn.Time = enrollmentShift.Term.EnrollmentStartDate.Date;
                                turn.CreditsLimit = credits;
                                turn.IsOnline = false;
                            }
                        }
                        else
                        {
                            var enrollmentTurn = new EnrollmentTurn
                            {
                                TermId = enrollmentShift.TermId,
                                StudentId = student.Id,
                                Time = enrollmentShift.Term.EnrollmentStartDate.Date,
                                CreditsLimit = credits,
                                IsOnline = false
                            };

                            inPersonEnrollmentTurns.Add(enrollmentTurn);
                        }
                    }

                    await _context.EnrollmentTurns.AddRangeAsync(inPersonEnrollmentTurns);
                }

                await _context.EnrollmentTurns.AddRangeAsync(enrollmentTurns);

                careerShift.WasExecuted = true;
                await _context.SaveChangesAsync();
            }

            enrollmentShift.WasExecuted = true;
            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, "");
        }

        public async Task<Tuple<bool, string>> GenerateTurns(Guid enrollmentShiftId, Guid careerId)
        {
            var minutesBetweenShifts = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.MINUTES_BETWEEN_SHIFTS);
            //var unbeatenStudentCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENT_CREDITS));
            var creditsModality = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.TERM_CREDITS_MODALITY);
            var regularCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REGULAR_MAXIMUM_CREDITS));
            var onlyRegulars = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_TURN_ONLY_FOR_REGULAR));

            var enableLowGradeCredits = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENABLE_CREDITS_FOR_LOW_GRADE_STUDENTS));
            var lowGradeMaximumGrade = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.LOW_GRADE_STUDENTS_MAXIMUM_GRADE));
            var lowGradeCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.LOW_GRADE_STUDENTS_CREDITS));

            var enrollmentShift = await _context.EnrollmentShifts.FindAsync(enrollmentShiftId);

            var careerShift = await _context.CareerEnrollmentShifts
                .Where(x => x.EnrollmentShiftId == enrollmentShift.Id && x.CareerId == careerId)
                .FirstOrDefaultAsync();

            var currentTurns = await _context.EnrollmentTurns
                .Where(x => x.TermId == enrollmentShift.TermId && x.Student.CareerId == careerId)
                .ToListAsync();
            //_context.EnrollmentTurns.RemoveRange(currentTurns);

            var academicYearCourses = await _context.AcademicYearCourses
                .Where(x => x.Curriculum.CareerId == careerId)
                .Select(x => new
                {
                    x.CourseId,
                    x.AcademicYear,
                    x.Course.Credits,
                    x.CurriculumId,
                    x.IsElective,
                    x.Course.AcademicProgramId,
                    AcademicProgramCode = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Code : ""
                }).ToListAsync();

            var query = _context.Students
                .FilterActiveStudents()
                .Where(x => x.CareerId == careerId)
                .AsNoTracking();

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNICA)
                query = query.Where(x => x.AcademicSummaries.All(y => y.TermHasFinished));

            var students = await query
                   .Select(s => new
                   {
                       s.Id,
                       s.CareerId,
                       Order = s.CurrentMeritOrder,
                       s.Status,
                       s.CurriculumId,
                       Curriculum = s.Curriculum.Code,
                       s.CurrentAcademicYear,
                       s.AcademicProgramId,
                       s.UserId,
                       s.User.UserName,
                       s.CurrentMeritType,
                       AverageGrade = s.AcademicSummaries.Where(y => y.Term.Number == "1" || y.Term.Number == "2").OrderByDescending(y => y.Term.StartDate).Select(y => y.WeightedAverageGrade).FirstOrDefault()
                   }).ToListAsync();

            students = students.OrderByDescending(x => x.AverageGrade).ToList();

            var onlineEnrollmentStudents = students.ToList();
            if (onlyRegulars) onlineEnrollmentStudents = students.Where(x => x.Status == ConstantHelpers.Student.States.REGULAR || x.Status == ConstantHelpers.Student.States.UNBEATEN || x.Status == ConstantHelpers.Student.States.HIGH_PERFORMANCE).ToList();

            var totalTicks = 1.0;
            if (minutesBetweenShifts != 0 && careerShift.EndDateTime != careerShift.StartDateTime)
                totalTicks = careerShift.EndDateTime.Subtract(careerShift.StartDateTime).TotalMinutes / minutesBetweenShifts;

            var studentPerTick = (int)Math.Ceiling(onlineEnrollmentStudents.Count / totalTicks);
            var studentsCount = 0;

            var enrollmentTurns = new List<EnrollmentTurn>();

            var academicYearCredits = await _context.AcademicYearCredits.ToListAsync();

            var academicSummaries = await _context.AcademicSummaries
                 .Where(x => x.Student.CareerId == careerShift.CareerId && !x.WasWithdrawn)
                 .Select(x => new
                 {
                     x.StudentId,
                     TermYear = x.Term.Year,
                     TermNumber = x.Term.Number,
                     x.WeightedAverageGrade,
                     x.TermId
                 }).ToListAsync();

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.Student.CareerId == careerId && !x.Withdraw)
                .Select(x => new
                {
                    x.StudentId,
                    x.CourseId,
                    x.Course.Credits,
                    termYear = x.Term.Year,
                    termNumber = x.Term.Number,
                    termName = x.Term.Name,
                    x.Approved,
                    x.Validated,
                    x.Term.StartDate,
                    x.Try,
                    x.Course.Code,
                    x.TermId
                }).ToListAsync();

            var averageGradeCredits = await _context.AverageGradeCreditConfigurations.ToListAsync();

            var extraCreditsModality = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.EXTRA_CREDITS_MODALITY));
            var unbeatenStudentCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENT_CREDITS));
            var unbeatenStudentCreditsByRange = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENTS_CREDITS_BY_RANGE));
            var extraCredisConfigurations = await _context.ExtraCreditConfigurations.ToListAsync();

            for (var i = 0; i < totalTicks; i++)
            {
                if (studentsCount >= onlineEnrollmentStudents.Count) break;

                var group = onlineEnrollmentStudents.GetRange(studentsCount, Math.Min(studentPerTick, onlineEnrollmentStudents.Count - studentsCount));
                studentsCount += studentPerTick;
                var time = careerShift.StartDateTime.AddMinutes(i * minutesBetweenShifts);

                foreach (var student in group)
                {
                    if (student.UserName == "1923030407")
                    {
                        var entro = true;
                    }

                    var creditsQry = academicYearCourses
                        .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear && !x.IsElective);
                    if (student.AcademicProgramId.HasValue)
                    {
                        creditsQry = creditsQry.Where(x => !x.AcademicProgramId.HasValue || (x.AcademicProgramId.HasValue && (x.AcademicProgramId == student.AcademicProgramId || x.AcademicProgramCode == "00")));
                    }
                    var creditsBasedOnCurriculum = creditsQry.Sum(x => x.Credits);

                    var credits = creditsModality == ConstantHelpers.Term.CreditsModality.MAXIMUM_CREDITS_BASED_ON_CURRICULUM ? creditsBasedOnCurriculum : regularCredits;

                    if (creditsModality == ConstantHelpers.Term.CreditsModality.ACADEMIC_YEAR_CREDITS_CONFIGURATION)
                    {
                        var academicYearCredit = academicYearCredits
                                          .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear)
                                          .FirstOrDefault();
                        if (academicYearCredit == null)
                            return new Tuple<bool, string>(false, $"No existe configuración de créditos para el plan {student.Curriculum} y ciclo {student.CurrentAcademicYear}");

                        credits = academicYearCredit.Credits;
                    }

                    var academicSummary = academicSummaries
                         .Where(x => x.StudentId == student.Id)
                         .OrderByDescending(x => x.TermYear)
                         .ThenByDescending(x => x.TermNumber)
                         .FirstOrDefault();

                    if (creditsModality == ConstantHelpers.Term.CreditsModality.LAST_AVERAGE_GRADE_CREDITS)
                    {
                        var averageGradeCredit = averageGradeCredits
                                          .Where(x => ((x.GreaterThan && x.AverageGradeStart <= academicSummary.WeightedAverageGrade) || (!x.GreaterThan && x.AverageGradeStart < academicSummary.WeightedAverageGrade))
                                          && ((x.LessThan && academicSummary.WeightedAverageGrade <= x.AverageGradeEnd) || (!x.LessThan && academicSummary.WeightedAverageGrade < x.AverageGradeEnd)))
                                          .FirstOrDefault();

                        if (averageGradeCredit == null)
                            return new Tuple<bool, string>(false, $"No existe configuración de créditos para el prom. {academicSummary.WeightedAverageGrade}");

                        credits = averageGradeCredit.Credits;
                    }

                    if (extraCreditsModality == ConstantHelpers.Configuration.Enrollment.ExtraCreditModality.UNBEATEN_STUDENTS)
                    {
                        if (student.Status == ConstantHelpers.Student.States.UNBEATEN || student.Status == ConstantHelpers.Student.States.HIGH_PERFORMANCE)
                        {
                            if (unbeatenStudentCreditsByRange)
                            {
                                var extraCreditsConfiguration = extraCredisConfigurations.FirstOrDefault(x => x.AverageGradeStart <= student.AverageGrade && student.AverageGrade <= x.AverageGradeEnd);
                                if (extraCreditsConfiguration != null)
                                    credits += extraCreditsConfiguration.Credits;
                            }
                            else
                                credits += unbeatenStudentCredits;
                        }
                    }
                    else
                    {
                        var extraCreditsConfiguration = extraCredisConfigurations.FirstOrDefault(x => x.MeritType == student.CurrentMeritType);
                        if (extraCreditsConfiguration != null)
                            credits += extraCreditsConfiguration.Credits;
                    }

                    if (enableLowGradeCredits && academicSummary != null)
                    {
                        if (academicSummary.WeightedAverageGrade <= lowGradeMaximumGrade)
                            credits = lowGradeCredits;
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAB)
                    {
                        var studentCurriculumCourses = academicYearCourses.Where(x => x.CurriculumId == student.CurriculumId).Select(x => x.CourseId).ToList();

                        //validar notas del ciclo pasado
                        var studentHistories = academicHistories
                            .Where(x => x.StudentId == student.Id && studentCurriculumCourses.Contains(x.CourseId))
                                .GroupBy(x => x.CourseId)
                                .Select(x => new
                                {
                                    CourseId = x.Key,
                                    Try = x.Max(y => y.Try),
                                    Approved = x.Any(y => y.Approved)
                                })
                            .ToList();

                        if (studentHistories.Any(x => x.Try >= 2 && !x.Approved))
                            credits = 12;
                    }

                    if (currentTurns.Any(x => x.StudentId == student.Id))
                    {
                        var turn = currentTurns.Where(x => x.StudentId == student.Id).FirstOrDefault();

                        if (!turn.SpecialEnrollment)
                        {
                            turn.Time = time;
                            turn.CreditsLimit = credits;
                        }
                    }
                    else
                    {
                        var enrollmentTurn = new EnrollmentTurn
                        {
                            TermId = enrollmentShift.TermId,
                            StudentId = student.Id,
                            Time = time,
                            CreditsLimit = credits,
                            IsOnline = true
                        };

                        enrollmentTurns.Add(enrollmentTurn);
                    }
                }
            }

            if (onlyRegulars)
            {
                var inPersonEnrollmentTurns = new List<EnrollmentTurn>();
                var term = await _context.Terms.FindAsync(enrollmentShift.TermId);
                var inpersonEnrollmentStudents = students.Where(x => x.Status != ConstantHelpers.Student.States.REGULAR && x.Status != ConstantHelpers.Student.States.UNBEATEN && x.Status != ConstantHelpers.Student.States.HIGH_PERFORMANCE).ToList();

                foreach (var student in inpersonEnrollmentStudents)
                {
                    var credits = regularCredits;

                    if (creditsModality == ConstantHelpers.Term.CreditsModality.MAXIMUM_CREDITS_BASED_ON_CURRICULUM)
                    {
                        var creditsQry = academicYearCourses
                                          .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear && !x.IsElective);
                        if (student.AcademicProgramId.HasValue)
                        {
                            creditsQry = creditsQry.Where(x => !x.AcademicProgramId.HasValue || (x.AcademicProgramId.HasValue && (x.AcademicProgramId == student.AcademicProgramId || x.AcademicProgramCode == "00")));
                        }
                        credits = creditsQry.Sum(x => x.Credits);
                    }

                    if (creditsModality == ConstantHelpers.Term.CreditsModality.ACADEMIC_YEAR_CREDITS_CONFIGURATION)
                    {
                        var academicYearCredit = academicYearCredits
                                          .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear)
                                          .FirstOrDefault();
                        if (academicYearCredit == null)
                            return new Tuple<bool, string>(false, $"No existe configuración de créditos para el plan {student.Curriculum} y ciclo {student.CurrentAcademicYear}");

                        credits = academicYearCredit.Credits;
                    }

                    var academicSummary = academicSummaries
                           .Where(x => x.StudentId == student.Id)
                           .OrderByDescending(x => x.TermYear)
                           .ThenByDescending(x => x.TermNumber)
                           .FirstOrDefault();

                    if (creditsModality == ConstantHelpers.Term.CreditsModality.LAST_AVERAGE_GRADE_CREDITS)
                    {
                        var averageGradeCredit = averageGradeCredits
                                          .Where(x => ((x.GreaterThan && x.AverageGradeStart <= academicSummary.WeightedAverageGrade) || (!x.GreaterThan && x.AverageGradeStart < academicSummary.WeightedAverageGrade))
                                          && ((x.LessThan && academicSummary.WeightedAverageGrade <= x.AverageGradeEnd) || (!x.LessThan && academicSummary.WeightedAverageGrade < x.AverageGradeEnd)))
                                          .FirstOrDefault();

                        if (averageGradeCredit == null)
                            return new Tuple<bool, string>(false, $"No existe configuración de créditos para el prom. {academicSummary.WeightedAverageGrade}");

                        credits = averageGradeCredit.Credits;
                    }

                    if (enableLowGradeCredits && academicSummary != null)
                    {
                        if (academicSummary.WeightedAverageGrade <= lowGradeMaximumGrade)
                            credits = lowGradeCredits;
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAB
                        || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNIFSLB)
                    {
                        var studentCurriculumCourses = academicYearCourses.Where(x => x.CurriculumId == student.CurriculumId).Select(x => x.CourseId).ToList();
                        var studentHistories = academicHistories.Where(x => x.StudentId == student.Id && studentCurriculumCourses.Contains(x.CourseId)).ToList();

                        var studentLastTries = studentHistories
                            .GroupBy(x => x.CourseId)
                            .Select(x => new
                            {
                                CourseId = x.Key,
                                Approved = x.Any(y => y.Approved),
                                Try = x.OrderByDescending(y => y.Try).Select(y => y.Try).FirstOrDefault()
                            }).ToList();

                        if (studentLastTries.Any(x => !x.Approved && x.Try == 2))
                            credits = ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNIFSLB ? 16 : 12;
                    }

                    if (currentTurns.Any(x => x.StudentId == student.Id))
                    {
                        var turn = currentTurns.Where(x => x.StudentId == student.Id).FirstOrDefault();

                        if (!turn.SpecialEnrollment)
                        {
                            turn.Time = term.EnrollmentStartDate.Date;
                            turn.CreditsLimit = credits;
                            turn.IsOnline = false;
                        }

                    }
                    else
                    {
                        var enrollmentTurn = new EnrollmentTurn
                        {
                            TermId = enrollmentShift.TermId,
                            StudentId = student.Id,
                            Time = term.EnrollmentStartDate.Date,
                            CreditsLimit = credits,
                            IsOnline = false
                        };

                        inPersonEnrollmentTurns.Add(enrollmentTurn);
                    }
                }

                await _context.EnrollmentTurns.AddRangeAsync(inPersonEnrollmentTurns);
            }

            await _context.EnrollmentTurns.AddRangeAsync(enrollmentTurns);

            careerShift.WasExecuted = true;
            careerShift.LastExecution = DateTime.UtcNow;

            enrollmentShift.WasExecuted = true;

            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, "");
        }

        public async Task<Tuple<bool, string>> GenerateStudentTurn(Guid termId, Guid studentId)
        {
            var minutesBetweenShifts = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.MINUTES_BETWEEN_SHIFTS);
            var creditsModality = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.TERM_CREDITS_MODALITY);
            var regularCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REGULAR_MAXIMUM_CREDITS));
            var onlyRegulars = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_TURN_ONLY_FOR_REGULAR));

            var enableLowGradeCredits = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENABLE_CREDITS_FOR_LOW_GRADE_STUDENTS));
            var lowGradeMaximumGrade = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.LOW_GRADE_STUDENTS_MAXIMUM_GRADE));
            var lowGradeCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.LOW_GRADE_STUDENTS_CREDITS));

            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.CareerId,
                    Order = x.CurrentMeritOrder,
                    x.Status,
                    x.CurriculumId,
                    Curriculum = x.Curriculum.Code,
                    x.CurrentAcademicYear,
                    x.AcademicProgramId,
                    x.UserId,
                    x.User.UserName,
                    x.CurrentMeritType,
                    AverageGrade = x.AcademicSummaries.Where(y => y.Term.Number == "1" || y.Term.Number == "2").OrderByDescending(y => y.Term.StartDate).Select(y => y.WeightedAverageGrade).FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            var careerShift = await _context.CareerEnrollmentShifts
                .Where(x => x.EnrollmentShift.TermId == termId && x.CareerId == student.CareerId)
                .FirstOrDefaultAsync();

            var academicYearCourses = await _context.AcademicYearCourses
                .Where(x => x.Curriculum.CareerId == student.CareerId)
                .Select(x => new
                {
                    x.CourseId,
                    x.AcademicYear,
                    x.Course.Credits,
                    x.CurriculumId,
                    x.IsElective,
                    x.Course.AcademicProgramId,
                    AcademicProgramCode = x.Course.AcademicProgramId.HasValue ? x.Course.AcademicProgram.Code : ""
                }).ToListAsync();

            var time = careerShift.StartDateTime;

            var academicSummaries = await _context.AcademicSummaries
               .Where(x => x.StudentId == student.Id)
               .Select(x => new
               {
                   x.StudentId,
                   TermYear = x.Term.Year,
                   TermNumber = x.Term.Number,
                   x.WeightedAverageGrade,
                   x.TermId
               }).ToListAsync();

            var lastTurn = await _context.EnrollmentTurns
                .Where(x => x.TermId == termId && x.Student.CareerId == student.CareerId)
                .OrderByDescending(x => x.Time)
                .FirstOrDefaultAsync();
            if (lastTurn != null) time = lastTurn.Time;

            var academicYearCredits = await _context.AcademicYearCredits.ToListAsync();

            var extraCreditsModality = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.EXTRA_CREDITS_MODALITY));
            var unbeatenStudentCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENT_CREDITS));
            var unbeatenStudentCreditsByRange = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENTS_CREDITS_BY_RANGE));
            var extraCredisConfigurations = await _context.ExtraCreditConfigurations.ToListAsync();

            var creditsQry = academicYearCourses.Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear && !x.IsElective);

            if (student.AcademicProgramId.HasValue)
                creditsQry = creditsQry.Where(x => !x.AcademicProgramId.HasValue || (x.AcademicProgramId.HasValue && (x.AcademicProgramId == student.AcademicProgramId || x.AcademicProgramCode == "00")));

            var creditsBasedOnCurriculum = creditsQry.Sum(x => x.Credits);

            var credits = creditsModality == ConstantHelpers.Term.CreditsModality.MAXIMUM_CREDITS_BASED_ON_CURRICULUM ? creditsBasedOnCurriculum : regularCredits;

            if (creditsModality == ConstantHelpers.Term.CreditsModality.ACADEMIC_YEAR_CREDITS_CONFIGURATION)
            {
                var academicYearCredit = academicYearCredits
                                  .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear == student.CurrentAcademicYear)
                                  .FirstOrDefault();
                if (academicYearCredit == null)
                    return new Tuple<bool, string>(false, $"No existe configuración de créditos para el plan {student.Curriculum} y ciclo {student.CurrentAcademicYear}");

                credits = academicYearCredit.Credits;
            }

            if (extraCreditsModality == ConstantHelpers.Configuration.Enrollment.ExtraCreditModality.UNBEATEN_STUDENTS)
            {
                if (student.Status == ConstantHelpers.Student.States.UNBEATEN || student.Status == ConstantHelpers.Student.States.HIGH_PERFORMANCE)
                {
                    if (unbeatenStudentCreditsByRange)
                    {
                        var extraCreditsConfiguration = extraCredisConfigurations.FirstOrDefault(x => x.AverageGradeStart <= student.AverageGrade && student.AverageGrade <= x.AverageGradeEnd);
                        if (extraCreditsConfiguration != null)
                            credits += extraCreditsConfiguration.Credits;
                    }
                    else
                        credits += unbeatenStudentCredits;
                }
            }
            else
            {
                var extraCreditsConfiguration = extraCredisConfigurations.FirstOrDefault(x => x.MeritType == student.CurrentMeritType);
                if (extraCreditsConfiguration != null)
                    credits += extraCreditsConfiguration.Credits;
            }

            var academicSummary = academicSummaries
                         .Where(x => x.StudentId == student.Id)
                         .OrderByDescending(x => x.TermYear)
                         .ThenByDescending(x => x.TermNumber)
                         .FirstOrDefault();

            if (enableLowGradeCredits && academicSummary != null)
            {
                if (academicSummary.WeightedAverageGrade <= lowGradeMaximumGrade)
                    credits = lowGradeCredits;
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAB)
            {
                //validar notas del ciclo pasado
                var curriculumCourses = academicYearCourses
                    .Where(x => x.CurriculumId == student.CurriculumId)
                    .Select(x => x.CourseId).ToHashSet();

                var academicHistories = await _context.AcademicHistories
                    .Where(x => x.StudentId == student.Id && !x.Withdraw)
                    .ToListAsync();

                var studentHistories = academicHistories
                    .Where(x => x.StudentId == student.Id && curriculumCourses.Contains(x.CourseId))
                    .GroupBy(x => x.CourseId)
                    .Select(x => new
                    {
                        CourseId = x.Key,
                        Try = x.Max(y => y.Try),
                        Approved = x.Any(y => y.Approved)
                    }).ToList();

                if (studentHistories.Any(x => x.Try >= 2 && !x.Approved))
                    credits = 12;
            }

            var enrollmentTurn = await _context.EnrollmentTurns.Where(x => x.TermId == termId && x.StudentId == student.Id).FirstOrDefaultAsync();

            if (enrollmentTurn != null)
            {
                enrollmentTurn.Time = time;
                enrollmentTurn.CreditsLimit = credits;
            }
            else
            {
                enrollmentTurn = new EnrollmentTurn
                {
                    TermId = termId,
                    StudentId = student.Id,
                    Time = time,
                    CreditsLimit = credits,
                    IsOnline = onlyRegulars
                    ? student.Status == ConstantHelpers.Student.States.REGULAR || student.Status == ConstantHelpers.Student.States.UNBEATEN || student.Status == ConstantHelpers.Student.States.HIGH_PERFORMANCE
                    : true
                };

                await _context.EnrollmentTurns.AddAsync(enrollmentTurn);
            }

            await _context.SaveChangesAsync();

            return new Tuple<bool, string>(true, "");
        }

        public override async Task<EnrollmentTurn> Get(Guid id) => await _context.EnrollmentTurns.Where(x => x.Id == id).Include(x => x.Student)
            .ThenInclude(x => x.Career).ThenInclude(x => x.Faculty).Include(x => x.Student.User).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, int? type = null, ClaimsPrincipal user = null)
        {
            Expression<Func<EnrollmentTurn, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Student.CurrentAcademicYear;
                    break;
                case "5":
                    orderByPredicate = (x) => x.CreditsLimit;
                    break;
                default:
                    //orderByPredicate = (x) => x.Student.User.UserName;
                    break;
            }

            var query = _context.EnrollmentTurns
                .Where(x => x.TermId == termId)
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();
                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);

                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(searchValue)) query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            if (type.HasValue && type != -1)
            {
                if (type == 1) query = query.Where(x => x.SpecialEnrollment);
                else query = query.Where(x => !x.SpecialEnrollment);
            }
            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    faculty = x.Student.Career.Faculty.Name,
                    career = x.Student.Career.Name,
                    id = x.Id,
                    credits = x.CreditsLimit,
                    special = x.SpecialEnrollment,
                    active = x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE,
                    x.TermId,
                    rectification = x.IsRectificationActive,
                    observations = x.Observations,
                    x.StudentId,
                    isOnline = x.IsOnline
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatableWithCredits(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null, bool? isConfirmed = null, bool? isReceived = null)
        {
            Expression<Func<EnrollmentTurn, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Student.CurrentAcademicYear;
                    break;
                case "5":
                    orderByPredicate = (x) => x.CreditsLimit;
                    break;
                default:
                    break;
            }

            var query = _context.EnrollmentTurns
                .Where(x => x.TermId == termId)
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);

                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(searchValue)) query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            if (isConfirmed.HasValue) query = query.Where(x => x.IsConfirmed == isConfirmed.Value);

            if (isReceived.HasValue) query = query.Where(x => x.IsReceived == isReceived.Value);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    career = x.Student.Career.Name,
                    faculty = x.Student.Career.Faculty.Name,
                    academicYear = x.Student.CurrentAcademicYear,
                    credits = x.Student.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    // studentSections.Where(x => x.Key == s.Id).Select(x => x.sum).FirstOrDefault(),
                    isConfirmed = x.IsConfirmed,
                    isReceived = x.IsReceived,
                    id = x.Id
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetStudentsDetailDatatable(Guid termId, Guid? careerId = null)
        {
            var query = _context.EnrollmentTurns
                .Where(x => x.TermId == termId)
                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);

            var result = await query
                .Select(e => new
                {
                    code = e.Student.User.UserName,
                    name = e.Student.User.FullName,
                    turn = e.Time.ToString(ConstantHelpers.FORMATS.DATETIME, new System.Globalization.CultureInfo("en-US")),
                    merit = e.Student.CurrentMeritOrder,
                    academicyear = e.Student.CurrentAcademicYear,
                    isOnline = e.IsOnline
                })
                .OrderBy(x => x.merit)
                .ToListAsync();

            return result;
        }

        public async Task<EnrollmentTurn> GetStudentTurn(Guid studentId, Guid termId)
        {
            return await _context.EnrollmentTurns.Where(x => x.StudentId == studentId && x.TermId == termId).FirstOrDefaultAsync();
        }
        public async Task<EnrollmentTurn> GetWithData(Guid id)
            => await _context.EnrollmentTurns
            .Include(x => x.Term)
            .Include(x => x.EnrollmentTurnHistories)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(); //2016
        public async Task<EnrollmentTurn> GetByStudentIdAndTerm(Guid studentId, Guid termId)
            => await _context.EnrollmentTurns.Where(x => x.StudentId == studentId && x.TermId == termId).FirstOrDefaultAsync();
        public async Task EnrollmentTurnsFixJob(Guid termId)
        {
            var enrollmentTurns = await _context.EnrollmentTurns.Where(x => x.TermId == termId).ToListAsync();
            var count = 0;
            var academicYearCourses = await _context.AcademicYearCourses.Include(x => x.Course).ToListAsync();
            var total = 0;
            foreach (var item in enrollmentTurns)
            {
                var studentData = await _context.Students
                    .Where(x => x.Id == item.StudentId)
                    .Select(x => new
                    {
                        x.CurriculumId,
                        x.CurrentAcademicYear
                    }).FirstOrDefaultAsync();

                var credits = academicYearCourses
                    .Where(x => x.CurriculumId == studentData.CurriculumId
                    && x.AcademicYear == studentData.CurrentAcademicYear)
                    .Sum(x => x.Course.Credits);

                item.CreditsLimit = credits;

                count++;
                total++;
                if (count > 250)
                {
                    await _context.SaveChangesAsync();
                    count = 0;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetStudentCreditsWithoutTurn(Guid studentId)
        {
            var credits = await CalculateStudentCredits(studentId);
            return credits;
        }

        public async Task ValidateReceivedEnrollments()
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null) return;

            var enrollmentTurns = await _context.EnrollmentTurns
                .Where(x => x.TermId == term.Id && !x.IsReceived)
                .ToListAsync();

            var enrollmentTurnHistories = new List<EnrollmentTurnHistory>();
            foreach (var item in enrollmentTurns)
            {
                var history = new EnrollmentTurnHistory
                {
                    ConfirmationDate = item.ConfirmationDate,
                    CreditsLimit = item.CreditsLimit,
                    FileUrl = item.FileUrl,
                    IsConfirmed = item.IsConfirmed,
                    IsReceived = item.IsReceived,
                    IsRectificationActive = item.IsRectificationActive,
                    Observations = "Matrícula invalidada por no presentar la ficha",
                    SpecialEnrollment = item.SpecialEnrollment
                };
                enrollmentTurnHistories.Add(history);

                item.IsConfirmed = false;
                item.ConfirmationDate = null;

            }

            var studentSections = await _context.StudentSections
                .Where(x => x.Student.EnrollmentTurns.Any(y => y.TermId == term.Id && !y.IsReceived))
                .ToListAsync();

            _context.StudentSections.RemoveRange(studentSections);
            await _context.EnrollmentTurnHistories.AddRangeAsync(enrollmentTurnHistories);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EnrollmentTurnTemplate>> GetSpecialEnrollmentData(Guid termId, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var query = _context.EnrollmentTurns
                .Where(x => x.TermId == termId && x.SpecialEnrollment)
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();
                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Student.Career.FacultyId == facultyId);

                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(searchValue)) query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            var data = await query
                .Select(x => new EnrollmentTurnTemplate
                {
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Faculty = x.Student.Career.Faculty.Name,
                    Career = x.Student.Career.Name,
                    Credits = x.CreditsLimit,
                    EnableRectification = x.IsRectificationActive,
                    Observations = x.Observations
                })
                .ToListAsync();

            return data;
        }

    }
}
