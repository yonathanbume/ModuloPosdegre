using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.JOBEXCHANGE.Areas.Admin.ViewModels.ReportViewModel;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EnrolledStudent;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.SubstituteExam;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using static AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates.ProfileDetailTemplate;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<Student> GetStudentByUser(string userId)
        {
            var query = _context.Students
                .Include(x => x.User)
                .Include(x => x.Career.Faculty)
                .Include(x => x.Curriculum)
                .Include(x => x.AcademicProgram)
                .Include(x => x.Observations)
                .Include(x => x.MedicalRecord)
                .Include(x => x.StudentInformation.Term)
                .AsQueryable();

            var result = await query.FirstOrDefaultAsync(x => x.UserId == userId);

            return result;
        }

        public async Task<IEnumerable<Student>> GetAllBySectionId(Guid sectionId)
        {
            return await _context.Students
                    .Where(x => x.StudentSections.Any(ss => ss.SectionId == sectionId))
                    .Select(x => new Student
                    {
                        User = new ApplicationUser
                        {
                            Name = x.User.Name,
                            MaternalSurname = x.User.MaternalSurname,
                            PaternalSurname = x.User.PaternalSurname,
                            UserName = x.User.UserName,
                            FullName = x.User.FullName
                        },
                        CareerId = x.CareerId,
                        Career = new Career
                        {
                            Name = x.Career.Name
                        }
                    })
                    .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetEnrolledStudentBytermId(Guid termId, Guid? facultyId = null, Guid? careerId = null, int? studentAcademicYear = null, int? status = null, Guid? campusId = null)
        {
            var query = _context.Students
                .Include(x => x.User)
                .Include(x => x.Career)
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId
                && x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN))
                .AsQueryable();

            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (campusId.HasValue && campusId != Guid.Empty) query = query.Where(x => x.CampusId == campusId);

            if (studentAcademicYear.HasValue)
            {
                var term = await _context.Terms.FindAsync(termId);

                if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
                    query = query.Where(x => x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) == studentAcademicYear || (!x.AcademicSummaries.Where(y => y.TermId == termId).Any() && x.CurrentAcademicYear == studentAcademicYear));
                else
                    query = query.Where(x => x.CurrentAcademicYear == studentAcademicYear);
            }

            if (status.HasValue) query = query.Where(x => x.Status == status);

            var result = await query.ToArrayAsync();

            return result.OrderBy(x => x.User.FullName);
        }

        public async Task<IEnumerable<Student>> GetEnrolledTutoringStudentBytermId(ClaimsPrincipal User, Guid termId, Guid? careerId = null, int? studentAcademicYear = null)
        {
            var query = _context.Students
                .Include(x => x.User)
                .Include(x => x.Career)
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId)).AsQueryable();

            if (User.IsInRole(ConstantHelpers.ROLES.TUTORING_COORDINATOR))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var coordinatorCareerId = await _context.TutoringCoordinators
                        .Where(x => x.UserId == userId)
                        .Select(x => x.CareerId)
                        .ToListAsync();

                    query = query = query.Where(x => coordinatorCareerId.Contains(x.CareerId));
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (studentAcademicYear.HasValue && studentAcademicYear != 0)
            {
                var term = await _context.Terms.FindAsync(termId);

                if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
                    query = query.Where(x => x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) == studentAcademicYear || (!x.AcademicSummaries.Where(y => y.TermId == termId).Any() && x.CurrentAcademicYear == studentAcademicYear));
                else
                    query = query.Where(x => x.CurrentAcademicYear == studentAcademicYear);
            }

            var result = await query.ToArrayAsync();

            return result.OrderBy(x => x.User.FullName);
        }
        public async Task<IEnumerable<Student>> GetDisapprovedByFacultyAndTerm(Guid termId, Guid? facultyId = null)
        {
            var query = _context.Students
                .Where(x => !x.AcademicHistories.Where(ah => ah.TermId == termId).Select(ah => ah.Approved).FirstOrDefault())
                .AsQueryable();
            if (facultyId.HasValue)
                query = query.Where(x => x.Career.FacultyId == facultyId.Value);

            var result = await query
                .Select(x => new Student
                {
                    Id = x.Id,
                    Career = new Career
                    {
                        Id = x.CareerId,
                        Name = x.Career.Name
                    },
                    User = new ApplicationUser
                    {
                        Name = x.User.FullName,
                        Email = x.User.Email,
                        Dni = x.User.Dni,
                        BirthDate = x.User.BirthDate,
                        FullName = x.User.FullName
                    }
                }).ToListAsync();
            return result;
        }

        public async Task<Student> GetStudentWithCareerAndUser(Guid id)
        {
            return await _context.Students
                           .Where(x => x.Id == id)
                           .Include(x => x.AdmissionType)
                           .Include(x => x.AdmissionTerm)
                           .Include(x => x.Career)
                           .Include(x => x.Career.Faculty)
                            .Include(x => x.Curriculum)
                            .Include(x => x.User)
                            .Include(x => x.AcademicProgram)
                            .Include(x => x.Postulants)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentRankingByTerm(Guid termId, Guid? careerId = null, Guid? campusId = null)
        {
            var query = _context.Students
                .Where(x => x.AcademicSummaries.Any(asm => asm.TermId == termId))
                .AsQueryable();
            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);
            if (campusId.HasValue)
                query = query.Where(x => x.CampusId == campusId);
            var result = await query
                .Select(x => new Student
                {
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        MaternalSurname = x.User.MaternalSurname,
                        PaternalSurname = x.User.PaternalSurname,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        FullName = x.User.FullName
                    },
                    Career = !careerId.HasValue ? new Career
                    {
                        Id = x.CareerId,
                        Name = x.Career.Name
                    } : null,
                    Campus = !campusId.HasValue && x.CampusId.HasValue ? new Campus
                    {
                        Id = x.Campus.Id,
                        Name = x.Campus.Name
                    } : null,
                    AcademicSummaries = x.AcademicSummaries
                    .Where(asm => asm.TermId == termId)
                    .Select(asm => new ENTITIES.Models.Intranet.AcademicSummary
                    {
                        MeritOrder = asm.MeritOrder,
                        StudentAcademicYear = asm.StudentAcademicYear,
                        TotalCredits = asm.TotalCredits,
                        MeritType = asm.MeritType,
                        WeightedAverageGrade = asm.WeightedAverageGrade
                    }).ToList()
                }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Student>> GetGraduatedsRankingByTerms(Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null)
        {
            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED)
                .AsQueryable();
            if (admissionTermId.HasValue)
                query = query.Where(x => x.AdmissionTermId == admissionTermId);
            if (graduationTermId.HasValue)
                query = query.Where(x => x.GraduationTermId == graduationTermId);
            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);
            var result = await query
                .Select(x => new Student
                {
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        MaternalSurname = x.User.MaternalSurname,
                        PaternalSurname = x.User.PaternalSurname,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        FullName = x.User.FullName
                    },
                    Career = !careerId.HasValue ? new Career
                    {
                        Id = x.CareerId,
                        Name = x.Career.Name
                    } : null,
                    AdmissionTerm = !admissionTermId.HasValue ? new Term
                    {
                        Id = x.AdmissionTermId,
                        Name = x.AdmissionTerm.Name,
                        Number = x.AdmissionTerm.Number,
                        Year = x.AdmissionTerm.Year
                    } : null,
                    GraduationTerm = !graduationTermId.HasValue ? new Term
                    {
                        Id = x.GraduationTerm.Id,
                        Name = x.GraduationTerm.Name,
                        Number = x.GraduationTerm.Number,
                        Year = x.GraduationTerm.Year
                    } : null,
                    AcademicSummaries = x.AcademicSummaries
                    .Where(asm => graduationTermId.HasValue ? asm.TermId == graduationTermId : asm.TermId == x.GraduationTermId)
                    .Select(asm => new ENTITIES.Models.Intranet.AcademicSummary
                    {
                        MeritOrder = asm.MeritOrder,
                        MeritType = asm.MeritType,
                        WeightedAverageGrade = asm.WeightedAverageGrade
                    }).ToList()
                }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Student>> GetNewStudentsRankingByTerm(Guid? admissionTermId = null, Guid? careerId = null, int? status = null)
        {
            var query = _context.Students
                .AsQueryable();
            if (admissionTermId.HasValue)
                query = query.Where(x => x.AdmissionTermId == admissionTermId);
            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);
            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            var client = await query
                .Select(x => new
                {
                    Status = x.Status,
                    UserName = x.User.Name,
                    UserMaternalSurname = x.User.MaternalSurname,
                    UserPaternalSurname = x.User.PaternalSurname,
                    UserDni = x.User.Dni,
                    UserUserName = x.User.UserName,
                    UserFullName = x.User.FullName,
                    CareerId = x.CareerId,
                    CareerName = x.Career.Name,
                    CareerCode = x.Career.Code,
                    CurriculumCode = x.Curriculum.Code,
                    CampusName = x.Campus.Name ?? "-",
                    AdmissionTermName = x.AdmissionTerm.Name,
                    AdmissionTermNumber = x.AdmissionTerm.Number,
                    AdmissionTermYear = x.AdmissionTerm.Year,
                    GraduationTerm = x.GraduationTermId.HasValue ? new
                    {
                        x.GraduationTerm.Name,
                        x.GraduationTerm.Number,
                        x.GraduationTerm.Year,
                    } : null,
                    AcademicSummaries = x.AcademicSummaries
                    .OrderByDescending(asm => asm.Term.StartDate)
                    .Select(asm => new
                    {
                        AcademicSummaryMeritOrder = asm.MeritOrder,
                        AcademicSummaryMeritType = asm.MeritType,
                        AcademicSummaryWeightedAverageGrade = asm.WeightedAverageGrade,
                        AcademicSummaryName = asm.Term.Name,
                        AcademicSummaryNumber = asm.Term.Number,
                        AcademicSummaryYear = asm.Term.Year,
                    })
                })
                    .ToListAsync();
            var result = client
                 .Select(x => new Student
                 {
                     Status = x.Status,
                     User = new ApplicationUser
                     {
                         Name = x.UserName,
                         MaternalSurname = x.UserMaternalSurname,
                         PaternalSurname = x.UserPaternalSurname,
                         Dni = x.UserDni,
                         UserName = x.UserUserName,
                         FullName = x.UserFullName
                     },
                     Career = new Career
                     {
                         Id = x.CareerId,
                         Name = x.CareerName,
                         Code = x.CareerCode
                     },
                     Curriculum = new Curriculum
                     {
                         Code = x.CurriculumCode,
                     },
                     Campus = new Campus
                     {
                         Name = x.CampusName
                     },
                     AdmissionTerm = admissionTermId.HasValue ? new Term
                     {
                         Name = x.AdmissionTermName,
                         Number = x.AdmissionTermNumber,
                         Year = x.AdmissionTermYear
                     } : null,
                     GraduationTerm = x.GraduationTerm != null ? new Term
                     {
                         Name = x.GraduationTerm.Name,
                         Number = x.GraduationTerm.Number,
                         Year = x.GraduationTerm.Year
                     } : null,
                     LastAcademicSummary = x.AcademicSummaries
                                            .Select(asm => new ENTITIES.Models.Intranet.AcademicSummary
                                            {
                                                MeritOrder = asm.AcademicSummaryMeritOrder,
                                                MeritType = asm.AcademicSummaryMeritType,
                                                WeightedAverageGrade = asm.AcademicSummaryWeightedAverageGrade,
                                                Term = new Term
                                                {
                                                    Name = asm.AcademicSummaryName,
                                                    Number = asm.AcademicSummaryNumber,
                                                    Year = asm.AcademicSummaryYear
                                                }
                                            }).FirstOrDefault()
                 }).ToList();
            return result;
        }

        public async Task<(IEnumerable<Student> pagedList, int count)> GetStudentRankingWithCreditsByTermAndPaginationParameter(PaginationParameter paginationParameter, Guid termId, Guid? careerId = null, Guid? campusId = null)
        {
            var query = _context.Students
                .Where(x => x.AcademicSummaries.Any(asm => asm.TermId == termId))
                .AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);
            if (campusId.HasValue)
                query = query.Where(x => x.CampusId == campusId);


            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    paginationParameter.SearchValue = $"\"{paginationParameter.SearchValue}*\"";
                    query = query.Where(x =>
                        EF.Functions.Contains(x.User.FullName, paginationParameter.SearchValue) ||
                        EF.Functions.Contains(x.User.UserName, paginationParameter.SearchValue) ||
                        EF.Functions.Contains(x.User.Dni, paginationParameter.SearchValue) ||
                        (x.AcademicSummaries != null && x.AcademicSummaries.Count() != 0 && (
                                       x.AcademicSummaries.First().WeightedAverageGrade.ToString().Contains(paginationParameter.SearchValue)
                                       || x.AcademicSummaries.First().TotalCredits.ToString().Contains(paginationParameter.SearchValue))) ||
                        (x.EnrollmentTurns != null && x.EnrollmentTurns.Count() != 0 &&
                        x.EnrollmentTurns.First().CreditsLimit.ToString().Contains(paginationParameter.SearchValue))
                        );
                }
                else
                    query = query.Where(q => q.User.UserName.Contains(paginationParameter.SearchValue)
                                   || q.User.Dni.Contains(paginationParameter.SearchValue)
                                   || q.User.RawFullName.Contains(paginationParameter.SearchValue)
                                   || (q.AcademicSummaries != null && q.AcademicSummaries.Count() != 0 && (
                                      q.AcademicSummaries.First().WeightedAverageGrade.ToString().Contains(paginationParameter.SearchValue)
                                      || q.AcademicSummaries.First().TotalCredits.ToString().Contains(paginationParameter.SearchValue)))
                                   || (q.EnrollmentTurns != null && q.EnrollmentTurns.Count() != 0 &&
                                       q.EnrollmentTurns.First().CreditsLimit.ToString().Contains(paginationParameter.SearchValue)));
            }

            var count = await query.CountAsync();

            var result = await query
                .Select(x => new Student
                {
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        MaternalSurname = x.User.MaternalSurname,
                        PaternalSurname = x.User.PaternalSurname,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        FullName = x.User.FullName
                    },
                    Career = !careerId.HasValue ? new Career
                    {
                        Id = x.CareerId,
                        Name = x.Career.Name
                    } : null,
                    Campus = !campusId.HasValue && x.CampusId.HasValue ? new Campus
                    {
                        Id = x.Campus.Id,
                        Name = x.Campus.Name
                    } : null,
                    AcademicSummaries = x.AcademicSummaries
                    .Where(asm => asm.TermId == termId)
                    .Select(asm => new ENTITIES.Models.Intranet.AcademicSummary
                    {
                        MeritOrder = asm.MeritOrder,
                        StudentAcademicYear = asm.StudentAcademicYear,
                        TotalCredits = asm.TotalCredits,
                        MeritType = asm.MeritType,
                        WeightedAverageGrade = asm.WeightedAverageGrade
                    }).ToList(),
                    EnrollmentTurns = x.EnrollmentTurns
                    .Where(er => er.TermId == termId)
                    .Select(er => new EnrollmentTurn
                    {
                        CreditsLimit = er.CreditsLimit
                    }).ToList()
                }).ToListAsync();

            var result2 = result
                .OrderByDescending(x => x.EnrollmentTurns.Count() == 0 ? decimal.MinValue : x.EnrollmentTurns.First().CreditsLimit)
                .OrderByDescending(x => x.AcademicSummaries.Count() == 0 ? decimal.MinValue : x.AcademicSummaries.First().WeightedAverageGrade)
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage);

            return (result2, count);
        }

        public async Task<(IEnumerable<Student> pagedList, int count)> GetStudentRankingByTermAndPaginationParameter(PaginationParameter paginationParameter, Guid termId, Guid? careerId = null, Guid? campusId = null)
        {
            var query = _context.Students
                .Where(x => x.AcademicSummaries.Any(asm => asm.TermId == termId))
                .AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);
            if (campusId.HasValue)
                query = query.Where(x => x.CampusId == campusId);

            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    paginationParameter.SearchValue = $"\"{paginationParameter.SearchValue}*\"";
                    query = query.Where(q =>
                        EF.Functions.Contains(q.User.FullName, paginationParameter.SearchValue) ||
                        EF.Functions.Contains(q.User.UserName, paginationParameter.SearchValue) ||
                        EF.Functions.Contains(q.User.Dni, paginationParameter.SearchValue) ||
                       (q.AcademicSummaries != null && q.AcademicSummaries.Count() != 0 && (
                                      q.AcademicSummaries.First().WeightedAverageGrade.ToString().Contains(paginationParameter.SearchValue)
                                      || q.AcademicSummaries.First().TotalCredits.ToString().Contains(paginationParameter.SearchValue)))
                                   || (q.EnrollmentTurns != null && q.EnrollmentTurns.Count() != 0 &&
                                       q.EnrollmentTurns.First().CreditsLimit.ToString().Contains(paginationParameter.SearchValue))
                        );
                }
                else
                    query = query.Where(q => q.User.UserName.Contains(paginationParameter.SearchValue)
                                        || q.User.Dni.Contains(paginationParameter.SearchValue)
                                        || q.User.RawFullName.Contains(paginationParameter.SearchValue)
                                        || (q.AcademicSummaries != null && q.AcademicSummaries.Count() != 0 && (
                                           q.AcademicSummaries.First().WeightedAverageGrade.ToString().Contains(paginationParameter.SearchValue)
                                           || q.AcademicSummaries.First().TotalCredits.ToString().Contains(paginationParameter.SearchValue))));
            }

            var count = await query.CountAsync();

            var result = await query
                .Select(x => new Student
                {
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        MaternalSurname = x.User.MaternalSurname,
                        PaternalSurname = x.User.PaternalSurname,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        FullName = x.User.FullName
                    },
                    Career = !careerId.HasValue ? new Career
                    {
                        Id = x.CareerId,
                        Name = x.Career.Name
                    } : null,
                    Campus = !campusId.HasValue && x.CampusId.HasValue ? new Campus
                    {
                        Id = x.Campus.Id,
                        Name = x.Campus.Name
                    } : null,
                    AcademicSummaries = x.AcademicSummaries
                    .Where(asm => asm.TermId == termId)
                    .Select(asm => new ENTITIES.Models.Intranet.AcademicSummary
                    {
                        MeritOrder = asm.MeritOrder,
                        StudentAcademicYear = asm.StudentAcademicYear,
                        TotalCredits = asm.TotalCredits,
                        MeritType = asm.MeritType,
                        WeightedAverageGrade = asm.WeightedAverageGrade
                    }).ToList()
                }).ToListAsync();

            var result2 = result
                .OrderByDescending(x => x.AcademicSummaries.Count() == 0 ? decimal.MinValue : x.AcademicSummaries.First().WeightedAverageGrade)
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage);

            return (result2, count);
        }

        public async Task<(IEnumerable<Student> pagedList, int count)> GetGraduatedsRankingByTermsAndPaginationParameter(PaginationParameter paginationParameter, Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null)
        {
            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED)
                .AsQueryable();

            if (admissionTermId.HasValue)
                query = query.Where(x => x.AdmissionTermId == admissionTermId);
            if (graduationTermId.HasValue)
                query = query.Where(x => x.GraduationTermId == graduationTermId);
            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    paginationParameter.SearchValue = $"\"{paginationParameter.SearchValue}*\"";
                    query = query.Where(q =>
                        EF.Functions.Contains(q.User.FullName, paginationParameter.SearchValue) ||
                        EF.Functions.Contains(q.User.UserName, paginationParameter.SearchValue) ||
                        EF.Functions.Contains(q.User.Dni, paginationParameter.SearchValue) ||
                        EF.Functions.Contains(q.AdmissionTerm.Name, paginationParameter.SearchValue) ||
                        EF.Functions.Contains(q.GraduationTerm.Name, paginationParameter.SearchValue) ||
                        q.AcademicSummaries.Select(asm => asm.WeightedAverageGrade.ToString().Contains(paginationParameter.SearchValue)).Any())
                        ;
                }
                else
                    query = query.Where(q => q.User.UserName.Contains(paginationParameter.SearchValue)
                                                        || q.User.Dni.Contains(paginationParameter.SearchValue)
                                                        || q.User.RawFullName.Contains(paginationParameter.SearchValue)
                                                        || q.AdmissionTerm.Name.Contains(paginationParameter.SearchValue)
                                                        || q.GraduationTerm.Name.Contains(paginationParameter.SearchValue)
                                                        || q.AcademicSummaries.Select(asm => asm.WeightedAverageGrade.ToString().Contains(paginationParameter.SearchValue)).Any());
            }

            var count = await query.CountAsync();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.AcademicSummaries.Select(asm => asm.WeightedAverageGrade).FirstOrDefault())
                        : query.OrderBy(q => q.AcademicSummaries.Select(asm => asm.WeightedAverageGrade).FirstOrDefault());
                    break;
                case "1":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.User.UserName)
                        : query.OrderBy(q => q.User.UserName);
                    break;
                case "2":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.User.Dni)
                        : query.OrderBy(q => q.User.Dni);
                    break;
                case "3":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.User.FullName)
                        : query.OrderBy(q => q.User.FullName);
                    break;
                case "4":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.AdmissionTerm.StartDate)
                        : query.OrderBy(q => q.AdmissionTerm.StartDate);
                    break;
                case "5":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.GraduationTerm.StartDate)
                        : query.OrderBy(q => q.GraduationTerm.StartDate);
                    break;
                case "6":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.AcademicSummaries.Select(asm => asm.WeightedAverageGrade).FirstOrDefault())
                        : query.OrderBy(q => q.AcademicSummaries.Select(asm => asm.WeightedAverageGrade).FirstOrDefault());
                    break;
                case "7":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.AcademicSummaries.Select(asm => asm.MeritType).FirstOrDefault())
                        : query.OrderBy(q => q.AcademicSummaries.Select(asm => asm.MeritType).FirstOrDefault());
                    break;
            }

            var result = await query
                .Select(x => new Student
                {
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        MaternalSurname = x.User.MaternalSurname,
                        PaternalSurname = x.User.PaternalSurname,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        FullName = x.User.FullName
                    },
                    Career = new Career
                    {
                        Id = x.CareerId,
                        Name = x.Career.Name
                    },
                    AdmissionTerm = new Term
                    {
                        Id = x.AdmissionTermId,
                        Name = x.AdmissionTerm.Name,
                        Number = x.AdmissionTerm.Number,
                        Year = x.AdmissionTerm.Year
                    },
                    GraduationTerm = new Term
                    {
                        Id = x.GraduationTerm.Id,
                        Name = x.GraduationTerm.Name,
                        Number = x.GraduationTerm.Number,
                        Year = x.GraduationTerm.Year
                    },
                    AcademicSummaries = x.AcademicSummaries
                    .Where(asm => asm.TermId == (graduationTermId ?? x.GraduationTermId)) //graduationTermId.HasValue ? asm.TermId == graduationTermId : asm.TermId == x.GraduationTermId)
                    .Select(asm => new ENTITIES.Models.Intranet.AcademicSummary
                    {
                        MeritOrder = asm.MeritOrder,
                        MeritType = asm.MeritType,
                        WeightedAverageGrade = asm.WeightedAverageGrade
                    }).ToList()
                }).ToListAsync();

            var result2 = result
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .OrderByDescending(q => q.AcademicSummaries.Count() == 0 ? Decimal.MinValue : q.AcademicSummaries.First().WeightedAverageGrade);

            return (result2, count);
        }

        public async Task<(IEnumerable<Student> pagedList, int count)> GetNewStudentsRankingByTermAndPaginationParameter(PaginationParameter paginationParameter, Guid? admissionTermId = null, Guid? careerId = null, int? status = null)
        {
            var query = _context.Students.AsQueryable();

            if (admissionTermId.HasValue)
                query = query.Where(x => x.AdmissionTermId == admissionTermId);
            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);
            if (status.HasValue)
                query = query.Where(x => x.Status == status);
            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(q => q.User.UserName.Contains(paginationParameter.SearchValue)
                                    || q.User.Dni.Contains(paginationParameter.SearchValue)
                                    || q.User.RawFullName.Contains(paginationParameter.SearchValue)
                                    || q.Career.Code.Contains(paginationParameter.SearchValue)
                                    || q.Curriculum.Code.Contains(paginationParameter.SearchValue)
                                    // First Campus
                                    // Current Campus
                                    || q.Campus.Name.Contains(paginationParameter.SearchValue)
                                    || q.AdmissionTerm.Name.Contains(paginationParameter.SearchValue)
                                    // Last Term and Graduation Term
                                    || q.LastAcademicSummary.Term.Name.Contains(paginationParameter.SearchValue)
                                    || q.LastAcademicSummary.WeightedAverageGrade.ToString().Contains(paginationParameter.SearchValue)
                                    || q.GraduationTerm.Name.Contains(paginationParameter.SearchValue)
                                    || ConstantHelpers.Student.States.VALUES.GetValueOrDefault(q.Status).Contains(paginationParameter.SearchValue)
                                    || q.CurrentAcademicYear.ToString().Contains(paginationParameter.SearchValue)
                                    || ConstantHelpers.ACADEMIC_ORDER.VALUES.GetValueOrDefault(q.LastAcademicSummary.MeritType).Contains(paginationParameter.SearchValue));

            var count = await query.CountAsync();

            switch (paginationParameter.SortField)
            {
                case "1":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.User.UserName)
                        : query.OrderBy(q => q.User.UserName);
                    break;
                case "2":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.User.Dni)
                        : query.OrderBy(q => q.User.Dni);
                    break;
                case "3":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.User.RawFullName)
                        : query.OrderBy(q => q.User.RawFullName);
                    break;
                case "4":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.Career.Code)
                        : query.OrderBy(q => q.Career.Code);
                    break;
                case "5":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.Curriculum.Code)
                        : query.OrderBy(q => q.Curriculum.Code);
                    break;
                case "6": // First Campus
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.Campus.Name)
                        : query.OrderBy(q => q.Campus.Name);
                    break;
                case "7": // Current Campus
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.Campus.Name)
                        : query.OrderBy(q => q.Campus.Name);
                    break;
                case "8":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.AdmissionTerm.StartDate)
                        : query.OrderBy(q => q.AdmissionTerm.StartDate);
                    break;
                case "9":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.LastAcademicSummary.Term.StartDate)
                        : query.OrderBy(q => q.LastAcademicSummary.Term.StartDate);
                    break;
                case "10":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.LastAcademicSummary.WeightedAverageGrade)
                        : query.OrderBy(q => q.LastAcademicSummary.WeightedAverageGrade);
                    break;
                case "11":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.GraduationTerm.StartDate)
                        : query.OrderBy(q => q.GraduationTerm.StartDate);
                    break;
                case "12":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.LastAcademicSummary.WeightedAverageGrade)
                        : query.OrderBy(q => q.LastAcademicSummary.WeightedAverageGrade);
                    break;
                case "13":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.LastAcademicSummary.WeightedAverageGrade)
                        : query.OrderBy(q => q.LastAcademicSummary.WeightedAverageGrade);
                    break;
                case "14":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => ConstantHelpers.Student.States.VALUES.GetValueOrDefault(q.Status))
                        : query.OrderBy(q => ConstantHelpers.Student.States.VALUES.GetValueOrDefault(q.Status));
                    break;
                case "15":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => q.CurrentAcademicYear)
                        : query.OrderBy(q => q.CurrentAcademicYear);
                    break;
                case "16":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(q => ConstantHelpers.ACADEMIC_ORDER.VALUES.GetValueOrDefault(q.LastAcademicSummary.MeritType))
                        : query.OrderBy(q => ConstantHelpers.ACADEMIC_ORDER.VALUES.GetValueOrDefault(q.LastAcademicSummary.MeritType));
                    break;
            }

            var result = await query
                .Select(x => new Student
                {
                    Status = x.Status,
                    CurrentAcademicYear = x.CurrentAcademicYear,
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        MaternalSurname = x.User.MaternalSurname,
                        PaternalSurname = x.User.PaternalSurname,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        FullName = x.User.FullName
                    },
                    Career = new Career
                    {
                        Id = x.CareerId,
                        Name = x.Career.Name,
                        Code = x.Career.Code
                    },
                    Curriculum = new Curriculum
                    {
                        Code = x.Curriculum.Code,
                    },
                    Campus = new Campus
                    {
                        Name = x.Campus.Name
                    },
                    AdmissionTerm = new Term
                    {
                        Name = x.AdmissionTerm.Name,
                        Number = x.AdmissionTerm.Number,
                        Year = x.AdmissionTerm.Year
                    },
                    GraduationTerm = new Term
                    {
                        Name = x.GraduationTerm.Name,
                        Number = x.AdmissionTerm.Number,
                        Year = x.AdmissionTerm.Year
                    },
                    LastAcademicSummary = x.AcademicSummaries
                    .OrderByDescending(asm => asm.Term.StartDate)
                    .Select(asm => new ENTITIES.Models.Intranet.AcademicSummary
                    {
                        MeritOrder = asm.MeritOrder,
                        MeritType = asm.MeritType,
                        WeightedAverageGrade = asm.WeightedAverageGrade,
                        Term = new Term
                        {
                            Name = asm.Term.Name,
                            Number = x.AdmissionTerm.Number,
                            Year = x.AdmissionTerm.Year
                        }
                    }).FirstOrDefault()
                }).ToListAsync();

            var result2 = result
                .OrderByDescending(q => q.LastAcademicSummary?.WeightedAverageGrade ?? decimal.MinValue) // q.LastAcademicSummary != null ? q.LastAcademicSummary.WeightedAverageGrade : Decimal.MinValue)
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage);

            return (result2, count);
        }

        public async Task<IEnumerable<Student>> GetAllWithUserAndCareerAndCampusAndAdmissionTerm()
        {
            return await _context.Students
                           .Include(x => x.Career)
                           .Include(x => x.Campus)
                           .Include(x => x.AdmissionTerm)
                           .Include(x => x.User)
                           .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetAllWithUser()
        {
            return await _context.Students
                           .Include(x => x.User).ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetRegularStudents()
        {
            return await _context.Students
                           .Include(x => x.Career)
                           .Include(x => x.User)
                           .Where(x => x.Status == ConstantHelpers.Student.States.REGULAR)
                           .ToListAsync();
        }

        public async Task<object> GetAllPostulants(Guid termId, Guid careerId, int status)
        {
            var result = await _context.Students
                .Where(s => s.Status == status)
                .Where(s => s.CareerId == careerId)
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    faculty = s.Career.Faculty.Name,
                    academicYear = s.CurrentAcademicYear,
                    credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    id = s.Id
                }).ToListAsync();

            return result;
        }

        public async Task<Select2Structs.ResponseParameters> GetStudentSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Student, Select2Structs.Result>> selectPredicate = null, Func<Student, string[]> searchValuePredicate = null, string searchValue = null, int? status = null, Guid? careerId = null, string coordinatorId = null)
        {
            var query = _context.Students
                .Include(x => x.User)
                .AsNoTracking();

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue) || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.ToLower().Contains(searchValue) || x.User.UserName.ToLower().Contains(searchValue));
            }

            if (!string.IsNullOrEmpty(coordinatorId))
            {
                var careers = GetCoordinatorCareers(coordinatorId);
                query = query.Where(x => careers.Any(y => y == x.CareerId));
            }

            query = query.OrderBy(x => x.User.UserName).ThenBy(x => x.User.PaternalSurname);

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string academicCoordinatorId = null, string academicRecordStaffId = null, Guid? admissionTypeId = null,
            Guid? curriculumId = null, Guid? campusId = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Career.Name);

                    break;
                case "5":
                    orderByPredicate = ((x) => x.User.UserName);

                    break;
                case "6":
                    orderByPredicate = ((x) => x.User.PhoneNumber);

                    break;
                default:
                    //orderByPredicate = ((x) => x.User.FullName);
                    break;
            }

            var query = _context.Students
               .AsNoTracking();

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
            //    {
            //        searchValue = $"\"{searchValue}*\"";
            //        query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue) || EF.Functions.Contains(x.User.UserName, searchValue));
            //    }
            //    else
            //        query = query
            //                    .Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue) || x.User.Dni.Contains(searchValue));
            //}

            if (!string.IsNullOrEmpty(academicCoordinatorId))
                query = query.Where(x => x.Career.AcademicCoordinatorId == academicCoordinatorId || x.Career.CareerDirectorId == academicCoordinatorId);

            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(q => q.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(q => q.CareerId == careerId);

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty) query = query.Where(q => q.AcademicProgramId == academicProgramId);

            if (admissionTypeId.HasValue && admissionTypeId != Guid.Empty)
                query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CurriculumId == curriculumId);

            if (campusId.HasValue && campusId != Guid.Empty)
                query = query.Where(x => x.CampusId == campusId);

            if (!string.IsNullOrEmpty(academicRecordStaffId))
            {
                var careers = _context.AcademicRecordDepartments.Where(x => x.UserId == academicRecordStaffId).Select(x => x.AcademicDepartment.CareerId).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }

            Expression<Func<Student, dynamic>> searchFilter = (x) => new //Student
            {
                Name = x.User.Name,
                PaternalSurname = x.User.PaternalSurname,
                MaternalSurname = x.User.MaternalSurname,
                FullName = x.User.FullName,
                Dni = x.User.Dni,
                UserName = x.User.UserName,
                x.Id,
                x.User.Picture,
            };
            var recordsFiltered = query
                 .Select(x => new
                 {
                     //User = new
                     //{
                     x.Id,
                     x.User.Picture,
                     Name = x.User.Name,
                     PaternalSurname = x.User.PaternalSurname,
                     MaternalSurname = x.User.MaternalSurname,
                     FullName = x.User.FullName,
                     Dni = x.User.Dni,
                     UserName = x.User.UserName,
                     //}
                 }, searchValue, searchFilter)
                .Count();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    Id = x.Id,
                    Career = new
                    {
                        Name = x.Career.Name,
                        Faculty = new
                        {
                            Name = x.Career.Faculty.Name
                        }
                    },
                    UserId = x.UserId,
                    CurrentAcademicYear = x.CurrentAcademicYear,
                    User = new
                    {
                        Name = x.User.Name,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        FullName = x.User.FullName,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        Picture = x.User.Picture,
                        x.User.PhoneNumber
                    },
                    AcademicProgram = new
                    {
                        Name = x.AcademicProgram.Name
                    },
                    ///
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    FullName = x.User.FullName,
                    Dni = x.User.Dni,
                    Document = x.User.Document,
                    UserName = x.User.UserName,
                    Picture = x.User.Picture,
                    AdmissionType = x.AdmissionType.Name,
                    AdmissionTerm = x.AdmissionTerm.Name
                }, searchValue, searchFilter)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
        public async Task<DataTablesStructs.ReturnedData<Student>> GetLockedOutStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string academicCoordinatorId = null, string academicRecordStaffId = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Career.Name);

                    break;
                case "5":
                    orderByPredicate = ((x) => x.User.UserName);

                    break;
                case "6":
                    orderByPredicate = ((x) => x.User.PhoneNumber);

                    break;
                default:
                    //orderByPredicate = ((x) => x.User.FullName);
                    break;
            }

            var query = _context.Students
                .Where(x => x.User.IsLockedOut)
               .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue) || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query
                                .Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue) || x.User.Dni.Contains(searchValue));
            }

            if (!string.IsNullOrEmpty(academicCoordinatorId))
                query = query.Where(x => x.Career.AcademicCoordinatorId == academicCoordinatorId || x.Career.CareerDirectorId == academicCoordinatorId);

            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(q => q.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(q => q.CareerId == careerId);

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty) query = query.Where(q => q.AcademicProgramId == academicProgramId);

            if (!string.IsNullOrEmpty(academicRecordStaffId))
            {
                var careers = _context.AcademicRecordDepartments.Where(x => x.UserId == academicRecordStaffId).Select(x => x.AcademicDepartment.CareerId).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }

            var recordsFiltered = query.Count();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new Student
                {
                    Id = x.Id,
                    Career = new Career
                    {
                        Name = x.Career.Name,
                        Faculty = new Faculty
                        {
                            Name = x.Career.Faculty.Name
                        }
                    },
                    UserId = x.UserId,
                    CurrentAcademicYear = x.CurrentAcademicYear,
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        FullName = x.User.FullName,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        Picture = x.User.Picture,
                        Address = x.Observations.Where(y => y.Type == ConstantHelpers.OBSERVATION_TYPES.LOCK_OUT).OrderByDescending(y => y.CreatedAt).Select(y => y.CreatedAt.Value).FirstOrDefault().ToLocalDateTimeFormat() ?? ""
                    },
                    AcademicProgram = new AcademicProgram
                    {
                        Name = x.AcademicProgram.Name
                    },
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<Student>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<DataTablesStructs.ReturnedData<Student>> GetAgreementStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string academicCoordinatorId = null, string academicRecordStaffId = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.Career.Name);

                    break;
                case "5":
                    orderByPredicate = ((x) => x.User.UserName);

                    break;
                case "6":
                    orderByPredicate = ((x) => x.User.PhoneNumber);

                    break;
                default:
                    //orderByPredicate = ((x) => x.User.FullName);
                    break;
            }

            var query = _context.Students
                .Where(x => x.AcademicAgreementId.HasValue)
               .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue) || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query
                                .Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue) || x.User.Dni.Contains(searchValue));
            }

            if (!string.IsNullOrEmpty(academicCoordinatorId))
                query = query.Where(x => x.Career.AcademicCoordinatorId == academicCoordinatorId || x.Career.CareerDirectorId == academicCoordinatorId);

            if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(q => q.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(q => q.CareerId == careerId);

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty) query = query.Where(q => q.AcademicProgramId == academicProgramId);

            if (!string.IsNullOrEmpty(academicRecordStaffId))
            {
                var careers = _context.AcademicRecordDepartments.Where(x => x.UserId == academicRecordStaffId).Select(x => x.AcademicDepartment.CareerId).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }

            var recordsFiltered = query.Count();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new Student
                {
                    Id = x.Id,
                    Career = new Career
                    {
                        Name = x.Career.Name,
                        Faculty = new Faculty
                        {
                            Name = x.Career.Faculty.Name
                        }
                    },
                    UserId = x.UserId,
                    CurrentAcademicYear = x.CurrentAcademicYear,
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        FullName = x.User.FullName,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        Picture = x.User.Picture
                    },
                    AcademicProgram = new AcademicProgram
                    {
                        Name = x.AcademicProgram.Name
                    },
                    AcademicAgreementId = x.AcademicAgreementId,
                    AcademicAgreement = new AcademicAgreement
                    {
                        Name = x.AcademicAgreement.Name,
                    }
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<Student>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<Student>> GetUnbeatenStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, string academicCoordinatorId = null, bool isDean = false)
        {
            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.UNBEATEN)
                .Include(x => x.User)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(academicCoordinatorId) && !isDean)
            {
                var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == academicCoordinatorId || x.CareerDirectorId == academicCoordinatorId)
                    .Select(x => x.Id)
                    .ToListAsync();
                query = query.Where(X => careers.Contains(X.CareerId));
            }
            if (!string.IsNullOrEmpty(academicCoordinatorId) && isDean)
            {
                query = query.Where(x => x.Career.Faculty.DeanId == academicCoordinatorId || x.Career.Faculty.SecretaryId == academicCoordinatorId);
            }

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.AcademicProgram.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new Student
                {
                    Id = x.Id,
                    Career = new Career
                    {
                        Name = x.Career.Name
                    },
                    UserId = x.UserId,
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        UserName = x.User.UserName,
                        FullName = x.User.FullName
                    },
                    AcademicProgram = new AcademicProgram
                    {
                        Name = x.AcademicProgram.Name
                    }
                }, searchValue)
                .ToArrayAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<Student>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurriculumStudentsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? curriculumId = null)
        {
            var query = _context.Students
               .Where(x => x.GraduationTermId == null)
               .AsNoTracking();

            if (curriculumId.HasValue && curriculumId != Guid.Empty) query = query.Where(x => x.CurriculumId == curriculumId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.User.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.User.PaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.User.MaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.CurrentAcademicYear);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.User.UserName,
                    name = x.User.FullName,
                    academicYear = x.CurrentAcademicYear,
                    curriculum = $"{x.Curriculum.Year}-{x.Curriculum.Code}"
                })
                .ToArrayAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        #region STUDENT FILTER DATA TABLE 
        public async Task<DataTablesStructs.ReturnedData<StudentFilterTemplate>> GetStudentFilterDatatable(DataTablesStructs.SentParameters parameters, Guid? cid, Guid? fid, Guid? pid, string search, ClaimsPrincipal user = null)
        {
            return await GetStudentFilterDatatable(parameters, cid, fid, pid, search, null, GetStudentFilterDatatableOrderByPredicate(parameters), GetStudentFilterDatatableSearchValuePredicate(), user);
        }
        private async Task<DataTablesStructs.ReturnedData<StudentFilterTemplate>> GetStudentFilterDatatable(
             DataTablesStructs.SentParameters sentParameters, Guid? cid, Guid? fid, Guid? pid, string search,
             Expression<Func<StudentFilterTemplate, StudentFilterTemplate>> selectPredicate = null,
             Expression<Func<StudentFilterTemplate, dynamic>> orderByPredicate = null,
             Func<StudentFilterTemplate, string[]> searchValuePredicate = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x => x.Career.CareerDirectorId == userId || x.Career.AcademicCoordinatorId == userId || x.Career.AcademicSecretaryId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
            }

            if (fid.HasValue)
                query = query.Where(x => x.Career.FacultyId == fid);

            if (cid.HasValue)
                query = query.Where(x => x.CareerId == cid);

            if (pid.HasValue)
                query = query.Where(x => x.AcademicProgramId == pid);

            var result = query
                .Select(x => new StudentFilterTemplate
                {
                    Id = x.Id,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    Names = x.User.FullName,
                    UserName = x.User.UserName,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber
                });

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    result = result.Where(x => EF.Functions.Contains(x.Names, search) || EF.Functions.Contains(x.UserName, search));
                }
                else
                {
                    search = search.Trim().ToLower();
                    result = result.Where(x => x.Names.ToLower().Contains(search) || x.MaternalSurname.ToLower().Contains(search) || x.PaternalSurname.ToLower().Contains(search));
                }
            }

            result = result.WhereSearchValue(searchValuePredicate)
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }

        private Expression<Func<StudentFilterTemplate, dynamic>> GetStudentFilterDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Names);
                //case "1":
                //    return ((x) => x.Code);
                //case "2":
                //    return ((x) => x.Career);
                //case "3":
                //    return ((x) => x.AcademicProgram);
                //case "4":
                //    return ((x) => x.Intents);
                //case "5":
                //    return ((x) => x.Grade);
                //case "6":
                //    return ((x) => x.Approbed);
                default:
                    return ((x) => x.Names);
            }
        }
        private Func<StudentFilterTemplate, string[]> GetStudentFilterDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Id  +"",
                x.PaternalSurname+"",
                x.MaternalSurname+"",
                x.Names+"",
                x.UserName+"",
                x.Email +"",
                x.PhoneNumber+"",
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUndefeatedStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                default:
                    orderByPredicate = (x) => x.User.UserName;
                    break;
            }

            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.UNBEATEN).AsNoTracking();


            if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

            if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    academicYear = x.CurrentAcademicYear
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
        #endregion

        #region STUDENT RANKING DATA TABLE
        public async Task<DataTablesStructs.ReturnedData<StudentRankingByTermTemplate>> GetStudentRankingByTermDataTable(DataTablesStructs.SentParameters parameters, string userId, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? campusId, string search, ClaimsPrincipal user = null)
        {
            return await GetStudentRankingByTermDataTable(parameters, userId, termId, careerId, academicProgramId, campusId, search, null, GetStudentRankingByTermDatatableOrderByPredicate(parameters), GetStudentRankingByTermDatatableSearchValuePredicate(), user);
        }

        private async Task<DataTablesStructs.ReturnedData<StudentRankingByTermTemplate>> GetStudentRankingByTermDataTable(
             DataTablesStructs.SentParameters sentParameters, string userId, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? campusId, string search,
             Expression<Func<StudentRankingByTermTemplate, StudentRankingByTermTemplate>> selectPredicate = null,
             Expression<Func<StudentRankingByTermTemplate, dynamic>> orderByPredicate = null,
             Func<StudentRankingByTermTemplate, string[]> searchValuePredicate = null,
             ClaimsPrincipal user = null)
        {
            var query = _context.AcademicSummaries
                .Where(x => x.TermId == termId)
                 //.Where(x => x.AcademicSummaries.Any(asm => asm.TermId == termId))
                 //.WhereSearchValue(searchValuePredicate, search)
                 .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.Student.User.FullName, search) ||
                    EF.Functions.Contains(x.Student.User.UserName, search) ||
                    EF.Functions.Contains(x.Student.User.Dni, search)
                    );
                }
                else
                    query = query.Where(x => x.Student.User.Name.Contains(search) ||
                     x.Student.User.MaternalSurname.Contains(search) ||
                     x.Student.User.PaternalSurname.Contains(search) ||
                     x.Student.User.Dni.Contains(search) ||
                     x.Student.User.UserName.Contains(search));
            }

            //si no hay userId es el superadmin o admin
            if (!string.IsNullOrEmpty(userId))
            {
                var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId).ToListAsync();
                query = query.Where(x => careers.Any(y => y.Id == x.Student.CareerId));
            }

            if (user != null)
            {
                var claimUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var academicrecordCareers = await _context.AcademicRecordDepartments.Where(x => x.UserId == claimUserId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                    query = query.Where(x => academicrecordCareers.Any(y => y == x.Student.CareerId));
                }
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Student.Career.Faculty.DeanId == claimUserId || x.Student.Career.Faculty.SecretaryId == claimUserId);
                }
            }

            if (academicProgramId.HasValue)
            {
                var academicPrograms = await _context.AcademicPrograms.FirstOrDefaultAsync(x => x.Id == academicProgramId.Value);
                query = query.Where(x => academicPrograms.Id == x.Student.AcademicProgramId);
            }

            if (careerId.HasValue)
                query = query.Where(x => x.Student.CareerId == careerId.Value);
            if (campusId.HasValue)
                query = query.Where(x => x.Student.CampusId == campusId.Value);

            query = query
                .Include(x => x.Student.Campus)
                .Include(x => x.Student.Career)
                .Include(x => x.Student.User)
                .OrderBy(x => x.MeritOrder);
            //.Include(x => x.AcademicSummaries)
            //.OrderByDescending(x => x.AcademicSummaries.FirstOrDefault(a => a.TermId == termId).WeightedAverageGrade);

            var data = await query.ToDataTables(sentParameters, null);

            var newData = data.Data.Select(x => new StudentRankingByTermTemplate
            {
                Position = x.MeritOrder.ToString() ?? "-",
                AcademicYear = x.StudentAcademicYear.ToString("D2") ?? "-",
                Code = x.Student.User.UserName,
                Name = x.Student.User.FullName,
                Career = x.Student.Career == null ? "" : x.Student.Career.Name,
                Campus = x.Student.Campus == null ? null : x.Student.Campus.Name.Substring(0, 1).Insert(1, ".") ?? "---",
                WeightedAverageGrade = x.WeightedAverageGrade.ToString("0.0000") ?? "---",
                Credits = x.TotalCredits.ToString("0.00") ?? "0",
                MeritType = ConstantHelpers.ACADEMIC_ORDER.VALUES.GetValueOrDefault(x.MeritType) ?? "---",
            });

            var result = new DataTablesStructs.ReturnedData<StudentRankingByTermTemplate>
            {
                Data = newData,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = data.RecordsFiltered,
                RecordsTotal = data.RecordsTotal
            };

            return result;
        }

        private Expression<Func<StudentRankingByTermTemplate, dynamic>> GetStudentRankingByTermDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                //case "1":
                //    return ((x) => x.Code);
                //case "2":
                //    return ((x) => x.Career);
                //case "3":
                //    return ((x) => x.AcademicProgram);
                //case "4":
                //    return ((x) => x.Intents);
                //case "5":
                //    return ((x) => x.Grade);
                //case "6":
                //    return ((x) => x.Approbed);
                default:
                    return ((x) => x.Name);
            }
        }
        private Func<StudentRankingByTermTemplate, string[]> GetStudentRankingByTermDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name  +"",
                x.Position+"",
                x.WeightedAverageGrade+"",
                x.MeritType+"",
                x.Credits+"",
                x.Code +"",
                x.Career+"",
                x.Campus+"",
                x.AcademicYear +"",
            };
        }
        #endregion

        #region STUDENT RANKING FOR CREDITS DATA TABLE
        public async Task<DataTablesStructs.ReturnedData<StudentRankingForCreditsTemplate>> GetStudentRankingForCreditsDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? campusId, string search)
        {
            return await GetStudentRankingForCreditsDataTable(parameters, user, termId, careerId, academicProgramId, campusId, search, null, GetStudentRankingForCreditsDatatableOrderByPredicate(parameters), GetStudentRankingForCreditsDatatableSearchValuePredicate());
        }
        private async Task<DataTablesStructs.ReturnedData<StudentRankingForCreditsTemplate>> GetStudentRankingForCreditsDataTable(
             DataTablesStructs.SentParameters sentParameters,
             ClaimsPrincipal user, Guid termId, Guid? careerId, Guid? academicProgramId, Guid? campusId, string search,
             Expression<Func<StudentRankingForCreditsTemplate, StudentRankingForCreditsTemplate>> selectPredicate = null,
             Expression<Func<StudentRankingForCreditsTemplate, dynamic>> orderByPredicate = null,
             Func<StudentRankingForCreditsTemplate, string[]> searchValuePredicate = null)
        {
            var query = _context.AcademicSummaries
                .Where(x => x.TermId == termId)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                    {
                        query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                    }
                    else
                    {
                        var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId).ToListAsync();
                        query = query.Where(x => careers.Any(y => y.Id == x.CareerId));
                    }
                }
            }

            if (academicProgramId.HasValue) query = query.Where(x => academicProgramId == x.Student.AcademicProgramId);
            if (careerId.HasValue) query = query.Where(x => x.CareerId == careerId.Value);
            if (campusId.HasValue) query = query.Where(x => x.Student.CampusId == campusId.Value);

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    //search = $"\"{search}*\"";
                    //query = query.Where(q => EF.Functions.Contains(q.User.FullName, search) ||
                    //EF.Functions.Contains(q.User.UserName, search) ||
                    //EF.Functions.Contains(q.User.Dni, search) ||
                    // (q.AcademicSummaries != null && q.AcademicSummaries.Count() != 0 && (
                    //                              q.AcademicSummaries.First().WeightedAverageGrade.ToString().Contains(search)
                    //                              || q.AcademicSummaries.First().TotalCredits.ToString().Contains(search)))
                    //                           || (q.EnrollmentTurns != null && q.EnrollmentTurns.Count() != 0 &&
                    //                               q.EnrollmentTurns.First().CreditsLimit.ToString().Contains(search)));
                }
                else
                    query = query.Where(q => q.Student.User.UserName.Contains(search)
                    || q.Student.User.Dni.Contains(search)
                    || q.Student.User.FullName.Contains(search));
            }

            var recordsTotal = await query.CountAsync();

            var dbData = await query
                .Select(x => new
                {
                    x.Student.User.UserName,
                    CareerName = x.Career.Name,
                    x.Student.User.FullName,
                    Campus = x.Student.CampusId.HasValue ? x.Student.Campus.Code : "",
                    x.WeightedAverageGrade,
                    x.MeritOrder,
                    x.MeritType,
                    x.TotalCredits,
                    x.ApprovedCredits,
                    x.StudentAcademicYear,
                    EnrollmentTurn = x.Student.EnrollmentTurns
                        .Where(y => y.TermId == x.TermId)
                        .Select(y => new
                        {
                            y.CreditsLimit
                        }).FirstOrDefault()
                }).ToListAsync();

            var data = dbData
                .OrderBy(x => x.TotalCredits)
                .ThenBy(x => x.ApprovedCredits)
                .Select((x, i) => new StudentRankingForCreditsTemplate
                {
                    Position = i + 1,
                    AcademicYear = x.StudentAcademicYear.ToString("D2"),
                    Code = x.UserName,
                    Name = x.FullName,
                    Career = x.CareerName,
                    Campus = x.Campus,
                    WeightedAverageGrade = x.WeightedAverageGrade.ToString("0.0000"),
                    Credits = x.TotalCredits.ToString("0.00"),
                    MaxCredits = x.EnrollmentTurn == null ? 0 : x.EnrollmentTurn.CreditsLimit
                }).ToList();

            var recordsFiltered = data.Count;

            return new DataTablesStructs.ReturnedData<StudentRankingForCreditsTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                //RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
            };
        }
        private Expression<Func<StudentRankingForCreditsTemplate, dynamic>> GetStudentRankingForCreditsDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                //case "1":
                //    return ((x) => x.Code);
                //case "2":
                //    return ((x) => x.Career);
                //case "3":
                //    return ((x) => x.AcademicProgram);
                //case "4":
                //    return ((x) => x.Intents);
                //case "5":
                //    return ((x) => x.Grade);
                //case "6":
                //    return ((x) => x.Approbed);
                default:
                    return ((x) => x.Name);
            }
        }
        private Func<StudentRankingForCreditsTemplate, string[]> GetStudentRankingForCreditsDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name  +"",
                //X.position+"",
                x.WeightedAverageGrade+"",
                //X.meritType+"",
                x.Credits+"",
                x.Code +"",
                x.Career+"",
                x.Campus+"",
                x.AcademicYear +"",
            };
        }
        #endregion

        #region GRADUATEDS DATA TABLE
        public async Task<DataTablesStructs.ReturnedData<GraduatedsTemplate>> GetGraduatedsDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null, Guid? academicProgramId = null, string search = null)
        {
            return await GetGraduatedsDataTable(parameters, user, admissionTermId, graduationTermId, careerId, academicProgramId, search, null, GetGraduatedsDataTableOrderByPredicate(parameters), GetGraduatedsDataTableSearchValuePredicate());
        }
        private async Task<DataTablesStructs.ReturnedData<GraduatedsTemplate>> GetGraduatedsDataTable(
             DataTablesStructs.SentParameters sentParameters,
             ClaimsPrincipal user, Guid? admissionTermId = null, Guid? graduationTermId = null, Guid? careerId = null, Guid? academicProgramId = null, string search = null,

             Expression<Func<GraduatedsTemplate, GraduatedsTemplate>> selectPredicate = null,
             Expression<Func<GraduatedsTemplate, dynamic>> orderByPredicate = null,
             Func<GraduatedsTemplate, string[]> searchValuePredicate = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate2 = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate2 = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate2 = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate2 = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate2 = (x) => x.AdmissionTerm.Name;
                    break;
                case "4":
                    orderByPredicate2 = (x) => x.GraduationTerm.Name;
                    break;
                default:
                    orderByPredicate2 = (x) => x.User.UserName;
                    break;
            }

            var query = _context.Students
                  .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED)
                  .OrderByCondition(sentParameters.OrderDirection, orderByPredicate2)
                  .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                    {
                        query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                    }

                    if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
                    {
                        var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId).Select(x => x.Id).ToListAsync();
                        query = query.Where(x => careers.Contains(x.CareerId));
                    }
                }
            }

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
            {
                var academicPrograms = await _context.AcademicPrograms.FirstOrDefaultAsync(x => x.Id == academicProgramId.Value);
                query = query.Where(x => academicPrograms.Id == x.AcademicProgramId);
            }

            if (admissionTermId.HasValue && admissionTermId != Guid.Empty)
                query = query.Where(x => x.AdmissionTermId == admissionTermId.Value);

            if (graduationTermId.HasValue && graduationTermId != Guid.Empty)
                query = query.Where(x => x.GraduationTermId == graduationTermId.Value);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId.Value);

            var result = query
                .Select(x => new GraduatedsTemplate
                {
                    Position = 0,
                    Code = x.User.UserName,
                    Dni = x.User.Dni,
                    Career = (x.Career == null ? "" : x.Career.Name),
                    Name = x.User.FullName,
                    AdmissionTerm = x.AdmissionTerm == null ? "" : x.AdmissionTerm.Name,
                    GraduationTerm = x.GraduationTerm == null ? "" : x.GraduationTerm.Name,
                    WeightedAverageGrade = x.AcademicSummaries.Any()
                    ? x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.WeightedAverageCumulative).FirstOrDefault().ToString("0.00")
                    : "---",
                    MeritOrder = x.AcademicSummaries.Any()
                    ? x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.MeritOrder).FirstOrDefault()
                    : -1,
                    MeritType = //x.AcademicSummaries.FirstOrDefault() == null ? null : ConstantHelpers.ACADEMIC_ORDER.VALUES.GetValueOrDefault(x.AcademicSummaries.FirstOrDefault().MeritType)
                    x.AcademicSummaries.Any()
                    ? ConstantHelpers.ACADEMIC_ORDER.VALUES.GetValueOrDefault(x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.MeritType).FirstOrDefault())
                    : "---"
                }, search);

            return await result.ToDataTables(sentParameters, selectPredicate);
        }
        private Expression<Func<GraduatedsTemplate, dynamic>> GetGraduatedsDataTableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                //case "1":
                //    return ((x) => x.Code);
                //case "2":
                //    return ((x) => x.Career);
                //case "3":
                //    return ((x) => x.AcademicProgram);
                //case "4":
                //    return ((x) => x.Intents);
                //case "5":
                //    return ((x) => x.Grade);
                //case "6":
                //    return ((x) => x.Approbed);
                default:
                    return ((x) => x.Name);
            }
        }
        private Func<GraduatedsTemplate, string[]> GetGraduatedsDataTableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Position +"",
                x.Code +"",
                x.Dni +"",
                x.Career  +"",
                x.Name  +"",
                x.AdmissionTerm  +"",
                x.GraduationTerm  +"",
                x.WeightedAverageGrade +"",
                x.MeritOrder  +"",
                x.MeritType +"",
            };
        }
        #endregion

        #region NEW STUDENTS DATA TABLE
        public async Task<DataTablesStructs.ReturnedData<NewStudentTemplate>> GetNewStudentsDataTable(DataTablesStructs.SentParameters parameters, ClaimsPrincipal user, Guid? admissionTermId = null, Guid? careerId = null, Guid? academicProgramId = null, int? status = null, string search = null)
        {
            return await GetNewStudentsDataTable(parameters, user, admissionTermId, careerId, academicProgramId, status, search, null, GetNewStudentsDataTableOrderByPredicate(parameters), GetNewStudentssDataTableSearchValuePredicate());
        }
        private async Task<DataTablesStructs.ReturnedData<NewStudentTemplate>> GetNewStudentsDataTable(
             DataTablesStructs.SentParameters sentParameters,
             ClaimsPrincipal user, Guid? admissionTermId = null, Guid? careerId = null, Guid? academicProgramId = null, int? status = null, string search = null,

             Expression<Func<NewStudentTemplate, NewStudentTemplate>> selectPredicate = null,
             Expression<Func<NewStudentTemplate, dynamic>> orderByPredicate = null,
             Func<NewStudentTemplate, string[]> searchValuePredicate = null)
        {

            var query = _context.Students.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                    {
                        query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                    }
                    else
                    {
                        var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId).ToListAsync();
                        query = query.Where(x => careers.Any(y => y.Id == x.CareerId));
                    }
                }
            }

            if (academicProgramId.HasValue) query = query.Where(x => academicProgramId == x.AcademicProgramId);
            if (careerId.HasValue) query = query.Where(x => x.CareerId == careerId);
            if (admissionTermId.HasValue) query = query.Where(x => x.AdmissionTermId == admissionTermId);
            if (status.HasValue) query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(search))
            {

            }

            var recordsTotal = await query.CountAsync();

            var dbData = await query
                .Select(x => new
                {
                    Code = x.User.UserName,
                    x.User.Dni,
                    CareerCode = x.Career.Code,
                    x.User.FullName,
                    CurriculumCode = x.Curriculum.Code,
                    Campus = x.CampusId.HasValue ? x.Campus.Name : "",
                    AdmissionTerm = x.AdmissionTerm.Name,
                    AcademicSummary = x.AcademicSummaries
                        .OrderByDescending(y => y.Term.Year)
                        .ThenByDescending(y => y.Term.Number)
                        .Select(y => new
                        {
                            TermName = y.Term.Name,
                            y.WeightedAverageGrade,
                            y.MeritOrder,
                            y.MeritType
                        }).FirstOrDefault(),

                    GraduationTerm = x.GraduationTermId.HasValue ? x.GraduationTerm.Name : "",
                    x.Status,
                    x.CurrentAcademicYear
                }).ToListAsync();

            var data = dbData
                .OrderBy(x => x.AcademicSummary?.MeritOrder)
                .ThenBy(x => x.FullName)
                .Select((x, i) => new NewStudentTemplate
                {
                    Position = i + 1,
                    Code = x.Code,
                    Dni = x.Dni,
                    CareerCode = x.CareerCode,
                    Name = x.FullName,
                    CurriculumCode = x.CurriculumCode,
                    FirstCampus = x.Campus,
                    CurrentCampus = x.Campus,
                    AdmissionTerm = x.AdmissionTerm,
                    LastTerm = x.AcademicSummary == null ? "" : x.AcademicSummary.TermName,
                    LastWeightedAverageGrade = x.AcademicSummary == null ? 0 : x.AcademicSummary.WeightedAverageGrade,
                    GraduationTerm = x.GraduationTerm,
                    GraduationWeightedAverageGrade = string.IsNullOrEmpty(x.GraduationTerm) ? 0 : x.AcademicSummary.WeightedAverageGrade,
                    Status = ConstantHelpers.Student.States.VALUES.GetValueOrDefault(x.Status),
                    CurrentAcademicYear = x.CurrentAcademicYear.ToString("D2"),
                    MeritType = x.AcademicSummary != null ? ConstantHelpers.ACADEMIC_ORDER.VALUES.GetValueOrDefault(x.AcademicSummary.MeritType) : string.Empty
                }).ToList();

            var recordsFiltered = data.Count;

            return new DataTablesStructs.ReturnedData<NewStudentTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                //RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
            };
        }
        private Expression<Func<NewStudentTemplate, dynamic>> GetNewStudentsDataTableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                //case "1":
                //    return ((x) => x.Code);
                //case "2":
                //    return ((x) => x.Career);
                //case "3":
                //    return ((x) => x.AcademicProgram);
                //case "4":
                //    return ((x) => x.Intents);
                //case "5":
                //    return ((x) => x.Grade);
                //case "6":
                //    return ((x) => x.Approbed);
                default:
                    return ((x) => x.Name);
            }
        }
        private Func<NewStudentTemplate, string[]> GetNewStudentssDataTableSearchValuePredicate()
        {
            return (x) => new[]
            {
              x.Name  +"",
              x.Position +"",
              x.Code  +"",
              x.Dni +"",
              x.CareerCode +"",
              x.Name  +"",
              x.CurriculumCode  +"",
              x.FirstCampus  +"",
              x.CurrentCampus +"",
              x.AdmissionTerm  +"",
              x.LastTerm  +"",
              x.LastWeightedAverageGrade +"",
              x.GraduationTerm  +"",
              x.GraduationWeightedAverageGrade +"",
              x.Status  +"",
              x.CurrentAcademicYear  +"",
              x.MeritType  +"",
            };
        }
        #endregion

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsByFacultyAndCareerAndAcademicProgramDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                default:
                    //orderByPredicate = (x) => x.User.UserName;
                    break;
            }

            var query = _context.Students.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                    query = query.Where(x => careers.Any(y => y == x.CareerId));
                }
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
            }

            if (facultyId != null && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

            if (academicProgramId != null && academicProgramId != Guid.Empty) query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (careerId != null && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue) || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue));
            }

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    academicProgram = x.AcademicProgram.Name,
                    faculty = x.Career.Faculty.Name,
                    //grade = x.AcademicSummaries.OrderByDescending(y => y.Term.Name).FirstOrDefault() != null ? x.AcademicSummaries.OrderByDescending(y => y.Term.Name).FirstOrDefault().WeightedFinalGrade.ToString() : "-"
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

        public async Task<object> SearchStudentByTerm(string term, Guid? careerId = null, ClaimsPrincipal user = null, bool onlyActiveStudents = false)
        {
            var query = _context.Students
               //.Where(x => x.User.FullName.ToUpper().Contains(term.ToUpper()) || x.User.UserName.ToUpper().Contains(term.ToUpper()))
               .AsNoTracking();

            if (onlyActiveStudents) query = query.FilterActiveStudents();

            if (!string.IsNullOrEmpty(term))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    term = $"\"{term}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, term) || EF.Functions.Contains(x.User.UserName, term));
                }
                else
                {
                    query = query
                        .Where(x => x.User.FullName.ToUpper().Contains(term.ToUpper()) || x.User.UserName.ToUpper().Contains(term.ToUpper()));
                }

            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers
                            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                            .AsNoTracking();

                        if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);
                        var careers = qryCareers.Select(x => x.Id).ToHashSet();

                        query = query.Where(x => careers.Contains(x.CareerId));
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                        query = query.Where(x => careers.Contains(x.CareerId));
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
                else
                {
                    if (careerId != null && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL))
                {
                    var maxAcademicYear = Convert.ToInt16(await GetConfigurationValue(ConstantHelpers.Configuration.TeacherManagement.CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR));
                    query = query.Where(x => x.CurrentAcademicYear <= maxAcademicYear);
                }
            }


            var students = await query
                .OrderBy(x => x.User.UserName)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.User.UserName} - {x.User.FullName}"
                })
                .Take(5).ToListAsync();

            return students;
        }
        //Logica especifica de grados con los estados
        public async Task<object> SearchStudentForDegreeByTerm(string term, int? degreeType = null, Guid? careerId = null, ClaimsPrincipal user = null, bool onlyActiveStudents = false)
        {
            var query = _context.Students
               .AsNoTracking();

            if (onlyActiveStudents) query = query.FilterActiveStudents();

            if (!string.IsNullOrEmpty(term))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    term = $"\"{term}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, term) || EF.Functions.Contains(x.User.UserName, term));
                }
                else
                {
                    query = query
                        .Where(x => x.User.FullName.ToUpper().Contains(term.ToUpper()) || x.User.UserName.ToUpper().Contains(term.ToUpper()));
                }

            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers
                            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                            .AsNoTracking();

                        if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);
                        var careers = qryCareers.Select(x => x.Id).ToHashSet();

                        query = query.Where(x => careers.Contains(x.CareerId));
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                        query = query.Where(x => careers.Contains(x.CareerId));
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
                else
                {
                    if (careerId != null && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
                }
            }

            //Esto es de grados /admin/solicitud-grado-por-requisitos

            if (degreeType != null)
            {
                //Para Bachiller debe ser egresados 
                //Para titulado debe ser egresado o bachiller
                if (degreeType == ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE)
                {
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED || x.Status == ConstantHelpers.Student.States.BACHELOR);
                }
                else if (degreeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
                {
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED);
                }
                else if (degreeType == ConstantHelpers.GRADE_INFORM.DegreeType.GRADUATED)
                {
                    query = query.Where(x => !(x.Status == ConstantHelpers.Student.States.QUALIFIED ||
                                            x.Status == ConstantHelpers.Student.States.BACHELOR ||
                                            x.Status == ConstantHelpers.Student.States.GRADUATED));
                }
            }


            var students = await query
                .OrderBy(x => x.User.UserName)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.User.UserName} - {x.User.FullName}"
                })
                .Take(5).ToListAsync();

            return students;
        }

        public async Task<StudentInformationTemplate> GetStudentinformationById(Guid id)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var anySection = false;

            if (term != null)
                anySection = await _context.StudentSections.AnyAsync(x => x.StudentId == id && x.Section.CourseTerm.TermId == term.Id);

            var lastTerm = await _context.AcademicSummaries
                .Where(x => x.StudentId == id)
                .OrderByDescending(x => x.Term.Year)
                .ThenByDescending(x => x.Term.Number)
                .Select(x => new
                {
                    x.WeightedAverageGrade,
                    Term = x.Term.Name,
                    x.WeightedAverageCumulative
                })
                .FirstOrDefaultAsync();

            var model = await _context.Students
                .Where(x => x.Id == id)
                .Select(x => new StudentInformationTemplate
                {
                    Id = x.Id,
                    Picture = x.User.Picture,
                    Code = x.User.UserName,
                    FullName = x.User.FullName,
                    Career = x.Career.Name,
                    CareerId = x.CareerId,
                    Campus = x.CampusId.HasValue ? x.Campus.Name : "-",
                    Dni = x.User.Dni,
                    MeritOrder = x.CurrentMeritOrder,
                    Modality = x.AdmissionType.Name,
                    FirstEnrollmentDate = x.FirstEnrollmentDate.HasValue ? x.FirstEnrollmentDate.Value.ToLocalDateFormat() : "-",
                    AdmissionTerm = x.AdmissionTerm.Name,
                    AdmissionTermDate = x.AdmissionDate.HasValue ? x.AdmissionDate.ToLocalDateFormat() : "-",
                    GraduationTerm = x.GraduationTermId.HasValue ? x.GraduationTerm.Name : "-",
                    GraduationTermDate = x.GraduationTermId.HasValue ? x.GraduationTerm.EndDate.ToLocalDateFormat() : "-",
                    IsBachelor = "true",
                    Curriculum = x.Curriculum.Code,
                    AcademicProgramId = x.AcademicProgramId.HasValue ? x.AcademicProgramId.Value : Guid.Empty,
                    CurriculumId = x.CurriculumId,
                })
                .FirstOrDefaultAsync();

            model.CurrentTerm = anySection ? term.Name : lastTerm != null ? lastTerm.Term : "--";
            model.AverageGrade = lastTerm != null ? lastTerm.WeightedAverageGrade : -1;
            model.CumulativeGrade = lastTerm != null ? lastTerm.WeightedAverageCumulative : -1;

            return model;
        }

        public async Task<StudentGeneralDataTemplate> GetStudentGeneralDataById(Guid id)
        {
            var model = await _context.Students
                .Where(x => x.Id == id)
                .Select(x => new StudentGeneralDataTemplate
                {
                    Id = x.Id,
                    Email = x.User.Email,
                    PersonalEmail = x.User.PersonalEmail,
                    PhoneNumber = x.User.PhoneNumber,
                    UserName = x.User.UserName,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    Address = x.User.Address,
                    DocumentType = ConstantHelpers.DOCUMENT_TYPES.VALUES.ContainsKey(x.User.DocumentType) ?
                                    ConstantHelpers.DOCUMENT_TYPES.VALUES[x.User.DocumentType] : "",
                    Dni = x.User.Document,
                    PictureUrl = x.User.Picture,
                    Sex = x.User.Sex,
                    BirthDate = $"{x.User.BirthDate:dd/MM/yyyy}",
                    FacultyId = x.Career.FacultyId,
                    SelectedCareer = x.CareerId,
                    AcademicProgramName = x.AcademicProgramId.HasValue ? x.AcademicProgram.Name : "-",
                    DepartmentId = x.User.DepartmentId,
                    ProvinceId = x.User.ProvinceId,
                    DistrictId = x.User.DistrictId,
                    UserWeb = x.User.UserWeb,
                    RacialIdentity = x.RacialIdentity,
                    //StudentScaleId = x.StudentScaleId
                    EnrollmentFeeId = x.EnrollmentFeeId
                })
                .FirstOrDefaultAsync();

            var birthdate = ConvertHelpers.DatepickerToDatetime(model.BirthDate);
            var today = DateTime.Today;
            var age = today.Year - birthdate.Year;
            if (birthdate.Date > today.AddYears(-age)) age--;
            model.Age = age;

            return model;
        }

        public async Task<bool> ValidateUserRegister(string username, Guid id)
        {
            return await _context.Students.AnyAsync(x => x.User.UserName.Equals(username) && x.Id != id);
        }

        public async Task<bool> ValidateUserEmail(string email, Guid id)
        {
            return await _context.Students.AnyAsync(x => x.User.Email.Equals(email) && x.Id != id);
        }

        public async Task<bool> ValidateUserPersonalEmail(string email, Guid id)
        {
            return await _context.Students.AnyAsync(x => x.User.PersonalEmail.ToUpper() == email.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Guid? id = null, string search = null)
        {

            Expression<Func<UserProcedure, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var student = await _context.Students.FindAsync(id);

            var query = _context.UserProcedures.Where(x => x.UserId == student.UserId).AsTracking();

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.Contains(search) || x.User.UserName.Contains(search));
            }

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var model = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.Procedure.Code,
                    description = x.Procedure.Name,
                    date = x.CreatedAt,
                    term = x.TermId.HasValue ? x.Term.Name : "-",
                    status = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.STATUS.VALUES[x.Status],
                    dependency = x.DependencyId.HasValue ? x.Dependency.Name : "-"
                }).ToListAsync();

            var data = model.Select(x => new
            {
                code = x.code,
                description = x.description,
                date = x.date,
                term = x.term,
                status = x.status,
                dependency = x.dependency
            }).ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, Guid termId)
        {
            var query = _context.StudentSections
                .Where(ss => ss.StudentId == id && ss.Section.CourseTerm.TermId == termId)
                .AsNoTracking();

            var activeTermId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    courseId = x.Section.CourseTerm.CourseId,
                    code = x.Section.CourseTerm.Course.Code,
                    course = x.Section.IsDirectedCourse ? $"{x.Section.CourseTerm.Course.Name} (DIRIGIDO)" : x.Section.CourseTerm.Course.Name,
                    section = x.Section.Code,
                    credits = x.Section.CourseTerm.Course.Credits.ToString("0.0"),
                    Try = x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN ? "RET" : x.Try.ToString(),
                    isActiveTerm = x.Section.CourseTerm.TermId == activeTermId,
                    academicYear = "",
                    teacherName = x.Section.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.FullName).FirstOrDefault(),
                    teacherEmail = x.Section.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.User.Email).FirstOrDefault(),
                    teacherDepartment = x.Section.TeacherSections.Where(y => y.IsPrincipal).Select(y => y.Teacher.AcademicDepartmentId.HasValue ? y.Teacher.AcademicDepartment.Name : "").FirstOrDefault(),
                    disapproved = x.Try == 2,
                    risk = x.Try > 2,
                }).ToListAsync();


            var curriculumId = (Guid?)null;
            if (termId == activeTermId)
                curriculumId = await _context.Students.Where(x => x.Id == id).Select(x => x.CurriculumId).FirstOrDefaultAsync();
            else curriculumId = await _context.AcademicSummaries.Where(x => x.Id == id && x.TermId == termId).Select(x => x.CurriculumId).FirstOrDefaultAsync();

            var academicYearCourses = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculumId)
                .Select(x => new
                {
                    x.IsElective,
                    x.CourseId,
                    x.AcademicYear
                })
                .ToListAsync();

            var electiveCourses = academicYearCourses
                .Where(x => x.IsElective)
                .Select(x => x.CourseId)
                .ToList();

            data = data
                .Select(x => new
                {
                    x.id,
                    x.courseId,
                    x.code,
                    course = electiveCourses.Contains(x.id) ? $"{x.course} - (E)" : x.course,
                    x.section,
                    x.credits,
                    x.Try,
                    x.isActiveTerm,
                    academicYear = academicYearCourses.Where(y => y.CourseId == x.courseId).Select(y => y.AcademicYear.ToString("D2")).FirstOrDefault(),
                    x.teacherName,
                    x.teacherEmail,
                    x.teacherDepartment,
                    x.disapproved,
                    x.risk
                }).ToList();

            data = data.OrderBy(x => x.academicYear).ThenBy(x => x.code).ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetAvailableSections(Guid id)
        {
            var studentSection = await _context.StudentSections.Where(x => x.Id == id).Include(x => x.Section).FirstOrDefaultAsync();

            var availableSections = await _context.Sections
                .Where(x => x.CourseTermId == studentSection.Section.CourseTermId
                && x.Id != studentSection.SectionId)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Code
                })
                  .OrderBy(x => x.text)
                .ToListAsync();

            return availableSections;
        }

        public async Task<object> StudentsReport1(bool isCoordinator, List<Guid> careers)
        {
            var total = new List<string>();
            var categories = new List<string>();
            var clientExp = await _context.StudentExperiences
                .Where(x => x.CurrentWork)
                .Select(x => new
                {
                    x.Student.Career.Name,
                    x.Student.CareerId,
                }).ToListAsync();
            var clientCareers = await _context.Careers.ToListAsync();
            if (!careers.Contains(Guid.Empty))
            {
                total = clientExp
                   .Where(x => careers.Contains(x.CareerId))
                   .Select(x => x.Name).ToList();
                categories = clientCareers
                        .Where(x => careers.Contains(x.Id))
                        .Select(x => x.Name).OrderBy(x => x).ToList();
            }
            else
            {
                total = clientExp
                   .Select(x => x.Name).ToList();
                categories = clientCareers
                         .Select(x => x.Name).OrderBy(x => x).ToList();
            }

            var data = new List<int>();

            for (int i = 0; i < categories.Count; i++)
                data.Add(0);
            for (int i = 0; i < total.Count; i++)
                data[categories.IndexOf(total[i])]++;

            return new { categories, data };
        }

        public async Task<List<StudentJobExchangeExcel>> ExcelStudentsReport1(bool isCoordinator, List<Guid> careers)
        {
            var studentExperiences = _context.StudentExperiences
                .Where(x => x.CurrentWork).AsQueryable();

            if (!careers.Contains(Guid.Empty))
            {
                studentExperiences = studentExperiences
                   .Where(x => careers.Contains(x.Student.CareerId));
            }

            var result = await studentExperiences.Select(x => new StudentJobExchangeExcel
            {
                FullName = x.Student.User.FullName,
                UserName = x.Student.User.UserName,
                Career = x.Student.Career.Name,
                Email1 = x.Student.User.Email,
                Email2 = x.Student.User.PersonalEmail,
                Phone = x.Student.User.PhoneNumber,
                Company = x.CompanyId.HasValue ? x.Company.Description : x.CompanyName

            }).ToListAsync();
            return result;
        }

        public async Task<List<StudentJobExchangeExcel>> ExcelStudentsReport8(List<Guid> careers, Guid? facultyId)
        {

            var studentExperiences = _context.StudentExperiences.Where(x => x.CurrentWork).AsQueryable();
            //var isCoordinator = verification.Item1;
            //if (isCoordinator)
            //{
            //    var careers = verification.Item2;
            //    studentExperiences.Where(x => careers.Contains(x.Student.CareerId));
            //}

            var result = await studentExperiences.Select(x => new StudentJobExchangeExcel
            {
                FullName = x.Student.User.FullName,
                UserName = x.Student.User.UserName,
                Career = x.Student.Career.Name,
                Email1 = x.Student.User.Email,
                Email2 = x.Student.User.PersonalEmail,
                Phone = x.Student.User.PhoneNumber,
                Company = x.CompanyId.HasValue ? x.Company.Description : x.CompanyName,
                //JournalTime = x.

            }).ToListAsync();
            return result;

            //var query = _context.JobOfferCareers.Include(x => x.JobOffer).AsQueryable();
            //if (careers != null || !careers.Contains(Guid.Empty))
            //{
            //    query = query.Where(x => careers.Contains(x.CareerId)).AsQueryable();
            //}
            //else
            //{
            //    if (facultyId != Guid.Empty)
            //    {
            //        query = query.Where(x => x.Career.FacultyId == facultyId).AsQueryable();
            //    }
            //}

            //var total = await query.Select(x => new
            //{
            //    career = x.JobOffer,
            //    isFullTime = (x.JobOffer.WorkType == ConstantHelpers.JobOffer.WorkType.FULL_TIME) ? true : false

            //}).ToListAsync();

            //var result = await query.Select(x => new StudentJobExchangeExcel
            //{
            //    FullName = x.JobOffer.JobOfferApplications.User.FullName,
            //    UserName = x.Student.User.UserName,
            //    Career = x.Student.Career.Name,
            //    Email1 = x.Student.User.Email,
            //    Email2 = x.Student.User.PersonalEmail,
            //    Phone = x.Student.User.PhoneNumber,
            //    Company = x.CompanyId.HasValue ? x.Company.Description : x.CompanyName,
            //    JournalTime = x.

            //}).ToListAsync();
            //return result;
        }

        public async Task<List<StudentJobExchangeExcel>> ExcelStudentsReport9(Tuple<bool, List<Guid>> verification)
        {

            var studentExperiences = _context.StudentExperiences.Where(x => x.CurrentWork).AsQueryable();
            var isCoordinator = verification.Item1;
            if (isCoordinator)
            {
                var careers = verification.Item2;
                studentExperiences.Where(x => careers.Contains(x.Student.CareerId));
            }

            var result = await studentExperiences.Select(x => new StudentJobExchangeExcel
            {
                FullName = x.Student.User.FullName,
                UserName = x.Student.User.UserName,
                Career = x.Student.Career.Name,
                Email1 = x.Student.User.Email,
                Email2 = x.Student.User.PersonalEmail,
                Phone = x.Student.User.PhoneNumber,
                Company = x.CompanyId.HasValue ? x.Company.Description : x.CompanyName,
                Sector = x.IsPrivate ? "Privado" : "Público"

            }).ToListAsync();
            return result;
        }


        public async Task<object> StudentsReport2(int startYear, int endYear, List<Guid> careers, Guid? facultyId)
        {
            var query = _context.StudentExperiences.AsQueryable();

            if (facultyId != Guid.Empty)
            {
                query = query.Where(x => x.Student.Career.FacultyId == facultyId).AsQueryable();
            }

            if (careers.Count > 0 && !careers.Contains(Guid.Empty))
            {
                query = query.Where(x => careers.Contains(x.Student.CareerId));
            }

            query = query.Where(x => x.EndDate.HasValue ? (x.StartDate.Year >= startYear && x.EndDate.Value.Year >= endYear) :
                        (x.StartDate.Year >= startYear && x.StartDate.Year <= endYear));

            var total = await query
                .Select(x => new
                {
                    working = x.CurrentWork,
                    faculty = x.Student.Career.Faculty.Name,
                    career = x.Student.Career.Name,
                    startYear = x.StartDate.Year,
                    careerId = x.Student.CareerId,
                    endYear = x.EndDate == null ? DateTime.Now.Year : x.EndDate.Value.Year
                }).ToListAsync();


            List<string> categories = new List<string>() { "Alumnos laborando", "Alumnos no laborando" };
            List<int> data = new List<int>() { 0, 0 };

            for (int i = 0; i < total.Count; i++)
            {
                if (total[i].working)
                    data[0] = data[0] + 1;
                else
                    data[1] = data[1] + 1;
            }
            return new { categories, data };
        }

        public async Task<object> StudentsReport3()
        {
            var total = await _context.Abilities.Include(x => x.StudentAbilities).Select(x => new
            {
                ability = x.Description,
                count = x.StudentAbilities.Count()
            }).OrderByDescending(x => x.count).Take(10).ToListAsync();

            List<string> categories = new List<string>();
            List<int> data = new List<int>();

            for (int i = 0; i < total.Count; i++)
            {
                categories.Add(total[i].ability);
                data.Add(total[i].count);
            }

            return new { categories, data };
        }

        public async Task<object> StudentsReport4(bool isCoordinator, List<Guid> careers)
        {
            var total = new List<string>();
            if (careers == null || careers.Contains(Guid.Empty))
            {
                total = await _context.Students.Include(x => x.RegistryPatterns)
                    //.Where(x => careers.Contains(x.CareerId))
                    .Select(x => x.RegistryPatterns.FirstOrDefault(y => y.GradDenomination == "TITULO") != null
                    ? "TITULO"
                    : x.RegistryPatterns.FirstOrDefault(y => y.GradDenomination == "BACHILLER") != null ? "BACHILLER" : "ESTUDIANTE")
                    .ToListAsync();
            }
            else
            {
                var client = await _context.Students.Include(x => x.RegistryPatterns)
                 .Where(x => careers.Contains(x.CareerId))
                 .Select(x => x.RegistryPatterns.FirstOrDefault(y => y.GradDenomination == "TITULO") != null
                 ? "TITULO"
                 : x.RegistryPatterns.FirstOrDefault(y => y.GradDenomination == "BACHILLER") != null ? "BACHILLER" : "ESTUDIANTE")
                 .ToListAsync();
            }
            //else
            //{
            //    total = await _context.Students
            //        .Include(x => x.RegistryPatterns)
            //        .Select(x => x.RegistryPatterns.FirstOrDefault(y => y.GradDenomination == "TITULO") != null
            //        ? "TITULO"
            //        : x.RegistryPatterns.FirstOrDefault(y => y.GradDenomination == "BACHILLER") != null ? "BACHILLER" : "ESTUDIANTE")
            //        .ToListAsync();
            //}

            List<decimal> count = new List<decimal>() { 0, 0, 0 };

            for (int i = 0; i < total.Count; i++)
                if (total[i] == "TITULO")
                    count[0]++;
                else if (total[i] == "BACHILLER")
                    count[1]++;
                else
                    count[2]++;

            var titulo = new { name = "Titulados", y = count[0] };
            var egreso = new { name = "Egresados", y = count[1] };
            var alumno = new { name = "Estudiantes", y = count[2] };

            var data = new[] { alumno, egreso, titulo }.ToList();

            return new { data };
        }

        public async Task<object> StudentsReport5(bool isCoordinator, List<Guid> careers)
        {
            int total = 0;
            if (isCoordinator)
            {
                total = await _context.Students.Include(x => x.RegistryPatterns)
                    .Where(x => x.GraduationTermId.HasValue && x.GraduationTerm.Year - x.RegisterDate.Year <= 5 && careers.Contains(x.CareerId))
                    .CountAsync();
            }
            else
            {
                total = await _context.Students.Include(x => x.RegistryPatterns)
                    .Where(x => x.GraduationTermId.HasValue && x.GraduationTerm.Year - x.RegisterDate.Year <= 5)
                    .CountAsync();
            }

            var data = new[] { new { name = "Egresados", y = total } }.ToList();
            return new { data };
        }

        public async Task<object> StudentsReport6(bool isCoordinator, List<Guid> careers)
        {
            var total = new List<string>();
            int degrees = 0;
            if (isCoordinator)
            {
                var client = await _context.StudentExperiences
                    .Include(x => x.Student.Career)
                    .Where(x => x.Student.GraduationTermId.HasValue && x.StartDate.Year > x.Student.GraduationTerm.Year)
                    .Select(x => new
                    {
                        x.Student.Career.Faculty.Name,
                        x.Student.CareerId,
                    })
                    .ToListAsync();
                total = client.Where(x => careers.Contains(x.CareerId)).Select(x => x.Name).ToList();

                degrees = await _context.Students.Where(x => x.GraduationTermId.HasValue && careers.Contains(x.CareerId)).CountAsync();
            }
            else
            {
                total = await _context.StudentExperiences
                    .Include(x => x.Student.Career)
                    .Where(x => x.Student.GraduationTermId.HasValue && x.StartDate.Year > x.Student.GraduationTerm.Year)
                    .Select(x => x.Student.Career.Faculty.Name)
                    .ToListAsync();

                degrees = await _context.Students.Where(x => x.GraduationTermId.HasValue).CountAsync();
            }

            List<string> categories = new List<string>();
            List<decimal> data = new List<decimal>();

            for (int i = 0; i < total.Count; i++)
            {
                if (categories.FirstOrDefault(x => x == total[i]) == null)
                {
                    categories.Add(total[i]);
                    data.Add(1);
                }
                else
                {
                    data[categories.IndexOf(total[i])]++;
                }
            }

            for (int i = 0; i < categories.Count; i++)
            {
                data[i] = decimal.Round(data[i] * 100 / degrees, 2);
            }

            return new { categories, data };
        }

        public async Task<object> StudentsReport7(bool isCoordinator, List<Guid> careers)
        {
            List<string> categories = new List<string>();
            var query = _context.Students
                .Include(x => x.Career)
                .Include(x => x.RegistryPatterns)
                .Include(x => x.StudentExperiences)
                .AsQueryable();

            categories = await _context.Careers
                .OrderBy(x => x.Name).Select(x => x.Name)
                .ToListAsync();

            if (isCoordinator)
            {
                query = query
                    .Where(x => careers.Contains(x.CareerId))
                    .AsQueryable();

                categories = await _context.Careers
                    .Where(x => careers.Contains(x.Id)).OrderBy(x => x.Name)
                    .Select(x => x.Name)
                    .ToListAsync();
            }

            var total = await query
                .Where(x => x.GraduationTermId != null)
                .Where(x => x.RegistryPatterns.Count > 0)
                .Where(x => x.StudentExperiences.Count > 0)
                .Select(x => new
                {
                    career = x.Career.Name,
                    titulo = x.RegistryPatterns.OrderBy(y => y.GradDenomination).FirstOrDefault(y => y.GradDenomination == "TITULO")
                }).ToListAsync();


            List<int> titulo = new List<int>();
            List<int> egreso = new List<int>();

            for (int i = 0; i < categories.Count; i++)
            {
                titulo.Add(0);
                egreso.Add(0);
            }

            for (int i = 0; i < total.Count; i++)
            {
                int index = categories.IndexOf(total[i].career);
                if (total[i].titulo != null)
                    titulo[index]++;
                else
                    egreso[index]++;
            }

            List<object> series = new List<object>
                {
                    new { name = "Egresados", data = egreso },
                    new { name = "Titulados", data = titulo }
                };

            return new { categories, series };
        }

        public async Task<object> StudentsReport8(List<Guid> careers, Guid? facultyId)
        {
            var query = _context.JobOfferCareers.Include(x => x.JobOffer).AsQueryable();
            if (careers.Count > 0 && !careers.Contains(Guid.Empty))
            {
                query = query.Where(x => careers.Contains(x.CareerId)).AsQueryable();
            }
            else
            {
                if (facultyId != Guid.Empty)
                {
                    query = query.Where(x => x.Career.FacultyId == facultyId).AsQueryable();
                }
            }

            var total = await query.Select(x => new
            {
                career = x.Career.Name,
                isFullTime = (x.JobOffer.WorkType == ConstantHelpers.JobOffer.WorkType.FULL_TIME) ? true : false

            }).ToListAsync();

            List<string> categories = new List<string>() { "Tiempo Completo", "Tiempo Parcial" };
            List<int> data = new List<int>() { 0, 0 };

            for (int i = 0; i < total.Count; i++)
            {
                if (total[i].isFullTime)
                    data[0] = data[0] + 1;
                else
                    data[1] = data[1] + 1;
            }

            return new { categories, data };
        }

        public async Task<object> StudentsReport9()
        {
            var query = _context.StudentExperiences.Where(x => x.CurrentWork).AsQueryable();

            List<string> categories = new List<string>() { "Sector Público", "Sector Privado" };
            List<int> data = new List<int>() { 0, 0 };

            data[0] = await query.Where(x => x.IsPrivate == false).CountAsync();
            data[1] = await query.Where(x => x.IsPrivate).CountAsync();

            return new { categories, data };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> SearchList(DataTablesStructs.SentParameters sentParameters, bool isCoordinator, List<Guid> careers, string DNI, string name, string code)
        {
            var query = _context.Students.AsQueryable();

            if (isCoordinator)
            {
                query = query.Where(x => careers.Contains(x.CareerId)).AsQueryable();
            }
            if (!string.IsNullOrEmpty(DNI))
                query = query.Where(x => x.User.Dni.ToLower().Contains(DNI.ToLower()));
            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.User.FullName.ToLower().Contains(name.ToLower()));
            if (!string.IsNullOrEmpty(code))
                query = query.Where(x => x.User.UserName.ToLower().Contains(code.ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.User.UserName,
                    name = x.User.FullName,
                    number = x.User.PhoneNumber,
                    email = x.User.Email ?? "-",
                    dni = x.User.Dni == "" ? "-" : x.User.Dni,
                    school = x.Career.Name,
                    degreeYear = x.GraduationTermId.HasValue ? x.GraduationTerm.Year.ToString() : "--"
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

        public async Task<Student> GetWithIncludes(Guid id)
        {
            var query = _context.Students
                .Include(x => x.User)
                .Include(x => x.Career.Faculty)
                .Include(x => x.Campus)
                .Include(x => x.Curriculum)
                .Include(x => x.StudentInformation.PlaceOriginDistrict.Province.Department)
                .Include(x => x.StudentInformation.OriginDistrict.Province.Department)
                .Include(x => x.User.District.Province.Department)
                .Include(x => x.GraduationTerm)
                .Include(x => x.AdmissionTerm)
                .Include(x => x.AcademicProgram)
                .Include(x => x.RecordHistories)
                .Include(x => x.MedicalRecord)
                .Include(x => x.AdmissionType)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<TemplateDataCV> GetStudentProfileDetail(Guid id)
        {
            var data = await _context.Students
                .Where(x => x.Id == id)
                .Select(x => new TemplateDataCV
                {
                    StudentId = x.Id,
                    FullName = x.User.FullName ?? "",
                    Birthday = x.User.BirthDate.ToLocalDateFormat(),
                    Career = x.Career.Name,
                    Phone = x.User.PhoneNumber ?? "",
                    Address = x.User.Address ?? "",
                    Email = x.User.Email ?? "",
                    DNI = x.User.Dni ?? "",
                    CivilStatus = ConstantHelpers.CIVIL_STATUS.VALUES.ContainsKey(x.User.CivilStatus) ?
                        ConstantHelpers.CIVIL_STATUS.VALUES[x.User.CivilStatus] : "",
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.User.Sex) ?
                        ConstantHelpers.SEX.VALUES[x.User.Sex] : "",
                    Description = x.CurriculumVitaes.Select(y => y.Description).FirstOrDefault() ?? "",
                    LstAcademicFormationDates = x.StudentAcademicEducations
                        .Select(y => new AcademicFormationDate
                        {
                            AcademicLevel = y.Type,
                            Description = y.Description,
                            IsStudying = y.IsStudying,
                            RangeDate = y.IsStudying ? $"{y.StartYear} - {y.EndYear}" : $"{y.StartYear} - Hasta la actualidad"
                        }).ToList(),
                    LstDegreeDates = x.RegistryPatterns
                        .Select(y => new DegreeDate
                        {
                            Description = y.AcademicDegreeDenomination,
                            Institution = GeneralHelpers.GetInstitutionName()
                        }).ToList(),
                    LstCertificateDates = x.StudentCertificates
                        .Select(y => new CertificateDate
                        {
                            Description = y.Description,
                            Institution = y.Institution
                        }).ToList(),
                    LstAbilityDates = new List<AbilityDate>(),
                    LstLanguageDates = new List<LanguageDate>(),
                    LstExperienceDates = x.StudentExperiences
                        .Select(y => new ExperienceDate
                        {
                            Description = y.CompanyId.HasValue ? y.Company.User.Name : y.CompanyName,
                            RangeDate = y.CurrentWork ? $"{y.StartDate.ToLocalDateFormat()} - Hasta la actualidad " : $"{y.StartDate.ToLocalDateFormat()} - {y.EndDate.ToLocalDateFormat()}",
                            Position = y.Position
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (data != null)
            {
                var studentAbilities = await _context.StudentAbilities
                    .Where(x => x.StudentId == data.StudentId)
                    .Select(x => new AbilityDate
                    {
                        Ability = x.Ability.Description,
                        Level = x.Level,
                        StringLevel = ConstantHelpers.LEVEL_EXPERIENCE.VALUES[Convert.ToInt32(x.Level)]
                    }).ToListAsync();

                data.LstAbilityDates = studentAbilities;

                var studentLanguages = await _context.StudentLanguages
                    .Where(x => x.StudentId == data.StudentId)
                    .Select(x => new LanguageDate
                    {
                        Language = x.Language.Name,
                        Level = x.Level,
                        StringLevel = ConstantHelpers.LEVEL_EXPERIENCE.VALUES[Convert.ToInt32(x.Level)]
                    })
                    .ToListAsync();

                data.LstLanguageDates = studentLanguages;
            }

            return data;
        }


        public async Task<StudentStudiesInfoTemplate> GetStudentStudiesDetail(Guid id)
        {
            var data = await _context.Students
                .Where(x => x.Id == id)
                .Select(x => new StudentStudiesInfoTemplate
                {
                    StudentId = x.Id,
                    UserName = x.User.UserName,
                    DNI = x.User.Dni,
                    FullName = x.User.FullName,
                    CareerName = x.Career.Name,
                    UserPicture = x.User.Picture,
                    Email = x.User.Email,
                    DegreeYear = x.GraduationTermId != null ? x.GraduationTerm.Year.ToString() : "--",
                    StudentState = ConstantHelpers.Student.States.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.Student.States.VALUES[x.Status] : "--",
                    WeightedAverageGrade = (x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number)
                                .Select(y => y.WeightedAverageGrade).FirstOrDefault()).ToString("0.00"),
                    BachelorDegree = new StudyTemplate(),
                    ProfesionalTitleDegree = new StudyTemplate()
                }).FirstOrDefaultAsync();

            if (data != null)
            {
                var bachelorDegree = await _context.RegistryPatterns
                    .Where(x => x.StudentId == data.StudentId && x.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
                    .Select(x => new StudyTemplate
                    {
                        Year = x.OriginDiplomatDate == null ? "" : x.OriginDiplomatDate.Value.Year.ToString(),
                        Resolution = x.ResolutionNumber,
                        ResolutionDate = x.ResolutionDateByUniversityCouncil == null ? "" : x.ResolutionDateByUniversityCouncil.Value.ToLocalDateFormat(),
                        Diploma = x.DiplomatNumber,
                        DiplomaDate = x.OriginDiplomatDate == null ? "" : x.OriginDiplomatDate.Value.ToLocalDateFormat()
                    })
                    .FirstOrDefaultAsync();

                if (bachelorDegree != null) data.BachelorDegree = bachelorDegree;

                var profesionalTitleDegree = await _context.RegistryPatterns
                    .Where(x => x.StudentId == data.StudentId && x.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE)
                    .Select(x => new StudyTemplate
                    {
                        Year = x.OriginDiplomatDate == null ? "" : x.OriginDiplomatDate.Value.Year.ToString(),
                        Resolution = x.ResolutionNumber,
                        ResolutionDate = x.ResolutionDateByUniversityCouncil == null ? "" : x.ResolutionDateByUniversityCouncil.Value.ToLocalDateFormat(),
                        Diploma = x.DiplomatNumber,
                        DiplomaDate = x.OriginDiplomatDate == null ? "" : x.OriginDiplomatDate.Value.ToLocalDateFormat()
                    })
                    .FirstOrDefaultAsync();

                if (profesionalTitleDegree != null) data.ProfesionalTitleDegree = profesionalTitleDegree;
            }

            return data;
        }
        public async Task<StudentComprobantInscriptionTemplate> GetStudentComprobantInscriptionTemplate(Guid studentId)
        {

            var postulant = await _context.Postulants.Where(x => x.StudentId == studentId).Include(x => x.ApplicationTerm).FirstOrDefaultAsync();

            var result = await _context.Students.Where(x => x.Id == studentId)
                .Select(x => new StudentComprobantInscriptionTemplate
                {
                    Name = x.User.Name,
                    LastNames = $"{(string.IsNullOrEmpty(x.User.PaternalSurname) ? "" : $"{x.User.PaternalSurname} ")}{(string.IsNullOrEmpty(x.User.MaternalSurname) ? "" : $"{x.User.MaternalSurname}")}",
                    UserName = x.User.UserName,
                    Faculty = x.Career.Faculty.Name,
                    Career = x.Career.Name,
                    Document = x.User.Document,
                    DocumentType = ConstantHelpers.DOCUMENT_TYPES.VALUES[x.User.DocumentType].ToUpper(),
                    AdmissionTermYear = x.AdmissionTerm.Year,
                    ModalityName = x.AdmissionType.Name,
                    Picture = x.User.Picture,
                    AdmissionTypeResolution = x.AdmissionType.Resolution,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                })
                .FirstOrDefaultAsync();

            result.OrderMeritBySchool = postulant?.OrderMeritBySchool;
            result.OrderMeritGeneral = postulant?.OrderMerit;
            result.Score = postulant?.FinalScore;
            result.ApplicationTerm = postulant?.ApplicationTerm.Name;
            result.PostulantCode = postulant?.Code;

            return result;
        }

        public async Task<List<StudentComprobantInscriptionTemplate>> GetStudentsComprobantInscriptionsTemplateByFilters(Guid termId, Guid? careerId, Guid? facultyId)
        {

            var queryPostulants = _context.Postulants
                .Where(x => x.Student.AdmissionTermId == termId)
                .AsNoTracking();

            var query = _context.Students
                .Where(x => x.AdmissionTermId == termId)
                .AsNoTracking();

            if (facultyId.HasValue && facultyId != Guid.Empty)
            {
                query = query.Where(x => x.Career.FacultyId == facultyId);
                queryPostulants = queryPostulants.Where(x => x.Student.Career.FacultyId == facultyId);
            }

            if (careerId.HasValue && careerId != Guid.Empty)
            {
                query = query.Where(x => x.CareerId == careerId);
                queryPostulants = queryPostulants.Where(x => x.Student.CareerId == careerId);
            }

            var postulants = await queryPostulants.ToListAsync();

            var result = await query
               .Select(x => new StudentComprobantInscriptionTemplate
               {
                   StudentId = x.Id,
                   Name = x.User.Name,
                   LastNames = $"{(string.IsNullOrEmpty(x.User.PaternalSurname) ? "" : $"{x.User.PaternalSurname} ")}{(string.IsNullOrEmpty(x.User.MaternalSurname) ? "" : $"{x.User.MaternalSurname}")}",
                   UserName = x.User.UserName,
                   Faculty = x.Career.Faculty.Name,
                   Picture = x.User.Picture,
                   Career = x.Career.Name,
                   Document = x.User.Document,
                   DocumentType = ConstantHelpers.DOCUMENT_TYPES.VALUES[x.User.DocumentType].ToUpper(),
                   AdmissionTermYear = x.AdmissionTerm.Year,
                   ModalityName = x.AdmissionType.Name,
                   AdmissionTypeResolution = x.AdmissionType.Resolution
               })
               .ToListAsync();

            foreach (var item in result)
            {
                var postulant = postulants.Where(x => x.StudentId == item.StudentId).FirstOrDefault();
                item.OrderMeritBySchool = postulant?.OrderMeritBySchool;
                item.OrderMeritGeneral = postulant?.OrderMerit;
                item.Score = postulant?.FinalScore;
            }

            return result;
        }

        public async Task<GraduatedFiltersTemplate> ReportGraduates(bool isCoordinator, List<Guid> careers)
        {
            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED ||
                            x.Status == ConstantHelpers.Student.States.BACHELOR ||
                            x.Status == ConstantHelpers.Student.States.QUALIFIED)
                .AsNoTracking();

            query = query.Where(x => x.GraduationTermId != null);

            var yearsBachiller = await query.Select(x => new { Year = x.GraduationTerm.Year }).Distinct().OrderBy(x => x.Year).ToListAsync();
            var grades = new List<GradeSelect2>();

            var careersSel = new List<CareerSelect2>();
            var años = new List<string>();
            if (isCoordinator)
            {
                careersSel = await _context.Careers.Where(x => careers.Contains(x.Id)).Select(x => new CareerSelect2
                {
                    Id = x.Id,
                    Description = x.Name
                }).ToListAsync();
            }
            else
            {
                careersSel = await _context.Careers.Select(x => new CareerSelect2
                {
                    Id = x.Id,
                    Description = x.Name
                }).ToListAsync();
            }


            //int firstYear = 0;
            //if (await query.Where(x => x.GraduationTermId != null).CountAsync() > 0)
            //{
            //    firstYear = query.Select(x => x.GraduationTerm.Year).OrderBy(x => x).FirstOrDefault();
            //}
            //else
            //{
            //    firstYear = 2013;
            //}
            var result = new GraduatedFiltersTemplate
            {
                //Grados = grades,
                Carreras = careersSel,
                Años = años
            };

            foreach (var item in yearsBachiller)
            {
                result.Años.Add(item.Year.ToString());
            }

            return result;
        }

        public async Task<int> CountAllGraduated()
        {
            var query = _context.Students.Where(x => x.GraduationTermId != null);
            return await query.CountAsync();
        }

        public async Task<CuantityReportTemplate.Table> ReportNumberGraduates(bool isCoordinator, List<Guid> careers, string startDate, string endDate, Guid careerId)
        {
            int startDate_n;
            int endDate_n;

            bool startDateVerification = int.TryParse(startDate, out startDate_n);
            bool endDateVerification = int.TryParse(endDate, out endDate_n);

            CuantityReportTemplate.Table model = new CuantityReportTemplate.Table();

            var todayYear = DateTime.Now.Year;

            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED ||
                            x.Status == ConstantHelpers.Student.States.BACHELOR ||
                            x.Status == ConstantHelpers.Student.States.QUALIFIED)
                .AsNoTracking();

            query = query.Where(x => x.GraduationTermId != null);

            if (isCoordinator)
            {
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            if (careerId != null && careerId != Guid.Empty)
            {
                query = query.Where(x => x.CareerId == careerId);
            }

            var yearsBachiller = new List<int>();
            var dataBachiller = await query.Select(x => new
            {
                Carrera = x.Career.Name,
                x.GraduationTerm.Year
            }).OrderBy(x => x.Year).ToListAsync();

            if (!startDateVerification && !endDateVerification)
            {
                yearsBachiller = dataBachiller.Where(x => todayYear - 5 <= x.Year && x.Year <= todayYear).OrderBy(x => x.Year).Select(x => x.Year).Distinct().ToList();
            }
            if (startDateVerification)
            {
                yearsBachiller = dataBachiller.Where(x => x.Year >= startDate_n && x.Year <= todayYear).OrderBy(x => x.Year).Select(x => x.Year).Distinct().ToList();
            }
            if (endDateVerification)
            {
                if (startDateVerification)
                {
                    yearsBachiller = yearsBachiller.Where(x => x <= endDate_n).OrderBy(x => x).Select(x => x).Distinct().ToList();
                }
                else
                {
                    yearsBachiller = dataBachiller.Where(x => endDate_n - 5 <= x.Year && x.Year <= endDate_n).OrderBy(x => x.Year).Select(x => x.Year).Distinct().ToList();
                }

            }
            var carrerasBachiller = dataBachiller.Select(x => new { x.Carrera }).Distinct().OrderBy(x => x.Carrera).ToList();

            model = new CuantityReportTemplate.Table { Header = new List<string>() { "Escuela Profesional" } };
            for (int i = 0; i < yearsBachiller.Count; i++)
                model.Header.Add(yearsBachiller[i].ToString());
            model.Header.Add("Sub Total");

            model.Content = new List<List<string>>();
            for (int i = 0; i < carrerasBachiller.Count; i++)
            {
                List<string> data = new List<string> { carrerasBachiller[i].Carrera };
                int sub = 0;
                for (int j = 0; j < yearsBachiller.Count(); j++)
                {
                    int cant = dataBachiller.Where(x => x.Carrera == carrerasBachiller[i].Carrera && x.Year == yearsBachiller[j]).Count();
                    data.Add(cant.ToString());
                    sub += cant;
                }
                data.Add(sub.ToString());
                model.Content.Add(data);
            }

            model.Footer = new List<string>() { "Sub Total" };
            var subTotalMx = 0;
            for (int i = 0; i < yearsBachiller.Count; i++)
            {
                model.Footer.Add(dataBachiller.Where(x => x.Year == yearsBachiller[i]).Count().ToString());
                subTotalMx += dataBachiller.Where(x => x.Year == yearsBachiller[i]).Count();
            }
            model.Footer.Add(subTotalMx.ToString());
            return model;
        }

        public async Task<object> GlobalList(bool isCoordinator, Guid? careerId, string dni, string nombre, string codigo, int draw, int start, int length)
        {
            int total = await _context.Students.CountAsync();

            var first_query = _context.Students
                .Include(x => x.User)
                .Include(x => x.Career)
                .Include(x => x.GraduationTerm)
                .Include(x => x.AdmissionTerm)
                .Include(x => x.RegistryPatterns)
                .Include(x => x.AcademicSummaries)
                .AsQueryable();
            if (isCoordinator)
            {
                total = await _context.Students.Include(x => x.Career).Where(x => x.CareerId == careerId).CountAsync();
                first_query = first_query.Where(x => x.CareerId == careerId).AsQueryable();
            }

            var _query =
                first_query
                .Select(x => new
                {
                    nro = start,
                    code = x.User.UserName,
                    dni = !String.IsNullOrEmpty(x.User.Dni) ? x.User.Dni : "-",
                    paternalsurname = x.User.PaternalSurname,
                    maternalsurname = x.User.MaternalSurname,
                    names = x.User.Name,
                    career = x.Career.Name ?? "---",
                    sede = "-",
                    sex = ConstantHelpers.SEX.ABREV[x.User.Sex],
                    email = x.User.Email ?? "-",
                    address = x.User.Address == "" ? "-" : x.User.Address,
                    registerTerm = x.AdmissionTerm == null ? "-" : x.AdmissionTerm.Name,
                    degreeTerm = x.GraduationTerm == null ? "-" : x.GraduationTerm.Name,
                    academicGrade = ConstantHelpers.ACADEMIC_ORDER.VALUES[x.CurrentMeritType],
                    score = x.AcademicSummaries.Select(y => new { y.Term.EndDate, y.WeightedAverageGrade }).FirstOrDefault(),
                    grade = x.RegistryPatterns.FirstOrDefault(y => y.GradDenomination == "BACHILLER"),
                    title = x.RegistryPatterns.FirstOrDefault(y => y.GradDenomination == "TITULO")
                });


            var query = _query.Select(x => new
            {
                x.nro,
                x.code,
                x.dni,
                x.paternalsurname,
                x.maternalsurname,
                x.names,
                x.career,
                x.sede,
                x.sex,
                x.email,
                x.address,
                x.registerTerm,
                x.degreeTerm,
                x.academicGrade,
                average = x.score == null ? "0.00" : x.score.WeightedAverageGrade.ToString("0.00"),

                resolutionDate = x.grade == null || x.grade.OriginDiplomatDate == null ? "-" : x.grade.OriginDiplomatDate.Value.ToString("dd/MM/yyyy"),
                resolution = x.grade == null ? "-" : x.grade.ResolutionNumber,
                degreeYear = x.grade == null || x.grade.OriginDiplomatDate == null ? "-" : x.grade.OriginDiplomatDate.Value.Year.ToString(),
                GraduationYear = x.grade == null || x.grade.OriginDiplomatDate == null ? "-" : x.grade.OriginDiplomatDate.Value.ToString("dd/MM/yyyy"),

                resolutionDate2 = x.title == null || x.title.OriginDiplomatDate == null ? "-" : x.title.OriginDiplomatDate.Value.ToString("dd/MM/yyyy"),
                resolution2 = x.title == null ? "-" : x.title.ResolutionNumber,
                titleYear = x.title == null || x.title.OriginDiplomatDate == null ? "-" : x.title.OriginDiplomatDate.Value.Year.ToString(),
                titleDate = x.title == null || x.title.OriginDiplomatDate == null ? "-" : x.title.OriginDiplomatDate.Value.ToString("dd/MM/yyyy"),
            });

            if (!string.IsNullOrEmpty(dni))
                query = query.Where(x => x.dni.ToLower().Contains(dni));
            if (!string.IsNullOrEmpty(codigo))
                query = query.Where(x => x.code.ToString().Contains(codigo));
            if (!string.IsNullOrEmpty(nombre))
                foreach (string filtro in nombre.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Where(x => x.names.ToLower().Contains(filtro.ToLower()) || x.paternalsurname.ToLower().Contains(filtro.ToLower()) || x.maternalsurname.ToLower().Contains(filtro.ToLower()));

            int filteredcount = await query.CountAsync();

            query = query.OrderBy(x => x.code);

            if (length != -1) query = query.Skip(start).Take(length);

            var result = query.ToList();

            return new { draw, data = result, recordsTotal = total, recordsFiltered = filteredcount };

        }

        public async Task<CuantityReportTemplate> ReportBachelorTitle(bool isCoordinator, List<Guid> careers)
        {
            CuantityReportTemplate model = new CuantityReportTemplate();
            var query = _context.RegistryPatterns.Include(x => x.Student.Career).AsQueryable();
            if (isCoordinator)
            {
                query = query.Where(x => careers.Contains(x.Student.CareerId));
            }
            var registryPatterns = await query
                    .Select(x => new
                    {
                        career = x.Student.Career.Name,
                        gradeType = x.GradeType,
                        year = x.OriginDiplomatDate.HasValue ? x.OriginDiplomatDate.Value.Year : 0
                    }).ToListAsync();
            var todayYear = DateTime.Now.Year;
            //bachiller
            {
                var dataBachiller = registryPatterns.Where(x => x.gradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR).Select(x => new
                {
                    Carrera = x.career,
                    Year = x.year
                }).OrderBy(x => x.Year).ToList();

                var carrerasBachiller = dataBachiller.Select(x => new { x.Carrera }).Distinct().OrderBy(x => x.Carrera).ToList();
                var yearsBachiller = dataBachiller.Where(x => todayYear - 5 <= x.Year && x.Year <= todayYear).Select(x => new { x.Year }).Distinct().OrderBy(x => x.Year).ToList();

                model.Bachiller = new CuantityReportTemplate.Table { Header = new List<string>() { "Escuela Profesional" } };
                for (int i = 0; i < yearsBachiller.Count; i++)
                    model.Bachiller.Header.Add(yearsBachiller[i].Year.ToString());
                model.Bachiller.Header.Add("Sub Total");

                model.Bachiller.Content = new List<List<string>>();
                for (int i = 0; i < carrerasBachiller.Count; i++)
                {
                    List<string> data = new List<string> { carrerasBachiller[i].Carrera };
                    int sub = 0;
                    for (int j = 0; j < yearsBachiller.Count(); j++)
                    {
                        int cant = dataBachiller.Where(x => x.Carrera == carrerasBachiller[i].Carrera && x.Year == yearsBachiller[j].Year).Count();
                        data.Add(cant.ToString());
                        sub += cant;
                    }
                    data.Add(sub.ToString());
                    model.Bachiller.Content.Add(data);
                }

                model.Bachiller.Footer = new List<string>() { "Sub Total" };
                var finalSubTotal = 0;
                for (int i = 0; i < yearsBachiller.Count; i++)
                {
                    model.Bachiller.Footer.Add(dataBachiller.Where(x => x.Year == yearsBachiller[i].Year).Count().ToString());
                    finalSubTotal += dataBachiller.Where(x => x.Year == yearsBachiller[i].Year).Count();
                }

                model.Bachiller.Footer.Add(finalSubTotal.ToString());
            }

            //titulo
            {
                var dataTitulo = registryPatterns.Where(x => x.gradeType == ConstantHelpers.GRADE_INFORM.DegreeType.PROFESIONAL_TITLE).Select(x => new
                {
                    Carrera = x.career,
                    Year = x.year
                }).OrderBy(x => x.Year).ToList();

                var carrerasTitulo = dataTitulo.Select(x => new { x.Carrera }).Distinct().OrderBy(x => x.Carrera).ToList();
                var yearsTitulo = dataTitulo.Where(x => todayYear - 5 < x.Year && x.Year < todayYear).Select(x => new { x.Year }).Distinct().OrderBy(x => x.Year).ToList();

                model.Titulo = new CuantityReportTemplate.Table { Header = new List<string>() { "Carrera Profesional" } };
                for (int i = 0; i < yearsTitulo.Count; i++)
                    model.Titulo.Header.Add(yearsTitulo[i].Year.ToString());
                model.Titulo.Header.Add("Sub Total");

                model.Titulo.Content = new List<List<string>>();
                for (int i = 0; i < carrerasTitulo.Count; i++)
                {
                    List<string> data = new List<string> { carrerasTitulo[i].Carrera };
                    int sub = 0;
                    for (int j = 0; j < yearsTitulo.Count(); j++)
                    {
                        int cant = dataTitulo.Where(x => x.Carrera == carrerasTitulo[i].Carrera && x.Year == yearsTitulo[j].Year).Count();
                        data.Add(cant.ToString());
                        sub += cant;
                    }
                    data.Add(sub.ToString());
                    model.Titulo.Content.Add(data);
                }

                model.Titulo.Footer = new List<string>() { "Sub Total" };
                var finalSubTotal2 = 0;
                for (int i = 0; i < yearsTitulo.Count; i++)
                {
                    model.Titulo.Footer.Add(dataTitulo.Where(x => x.Year == yearsTitulo[i].Year).Count().ToString());
                    finalSubTotal2 += dataTitulo.Where(x => x.Year == yearsTitulo[i].Year).Count();
                }
                model.Titulo.Footer.Add(finalSubTotal2.ToString());
            }
            return model;
        }

        public async Task<CuantityReportTemplate> ReportBachelorTitleSunedu(bool isCoordinator, List<Guid> careers)
        {
            CuantityReportTemplate model = new CuantityReportTemplate();
            var query = _context.RegistryPatterns.Include(x => x.Student.Career).Where(x => x.OriginDiplomatDate != null).AsQueryable();
            if (isCoordinator)
            {
                query = query.Where(x => careers.Contains(x.Student.CareerId));
            }
            var registryPatterns = await query
                    .Select(x => new
                    {
                        career = x.Student.Career.Name,
                        degree = x.GradDenomination,
                        year = x.OriginDiplomatDate.Value.Year
                    }).ToListAsync();

            //bachiller
            {
                var dataBachiller = registryPatterns.Where(x => x.degree == "BACHILLER").Select(x => new
                {
                    Carrera = x.career,
                    Year = x.year
                }).OrderBy(x => x.Year).ToList();

                var carrerasBachiller = dataBachiller.Select(x => new { x.Carrera }).Distinct().OrderBy(x => x.Carrera).ToList();
                var yearsBachiller = dataBachiller.Select(x => new { x.Year }).Distinct().OrderBy(x => x.Year).ToList();

                model.Bachiller = new CuantityReportTemplate.Table { Header = new List<string>() { "Carrera Profesional" } };
                for (int i = 0; i < yearsBachiller.Count; i++)
                    model.Bachiller.Header.Add(yearsBachiller[i].Year.ToString());
                model.Bachiller.Header.Add("Sub Total");

                model.Bachiller.Content = new List<List<string>>();
                for (int i = 0; i < carrerasBachiller.Count; i++)
                {
                    List<string> data = new List<string> { carrerasBachiller[i].Carrera };
                    int sub = 0;
                    for (int j = 0; j < yearsBachiller.Count(); j++)
                    {
                        int cant = dataBachiller.Where(x => x.Carrera == carrerasBachiller[i].Carrera && x.Year == yearsBachiller[j].Year).Count();
                        data.Add(cant.ToString());
                        sub += cant;
                    }
                    data.Add(sub.ToString());
                    model.Bachiller.Content.Add(data);
                }

                model.Bachiller.Footer = new List<string>() { "Sub Total" };
                for (int i = 0; i < yearsBachiller.Count; i++)
                    model.Bachiller.Footer.Add(dataBachiller.Where(x => x.Year == yearsBachiller[i].Year).Count().ToString());
                model.Bachiller.Footer.Add(dataBachiller.Count().ToString());
            }

            //titulo
            {
                var dataTitulo = registryPatterns.Where(x => x.degree == "TITULO").Select(x => new
                {
                    Carrera = x.career,
                    Year = x.year
                }).OrderBy(x => x.Year).ToList();

                var carrerasTitulo = dataTitulo.Select(x => new { x.Carrera }).Distinct().OrderBy(x => x.Carrera).ToList();
                var yearsTitulo = dataTitulo.Select(x => new { x.Year }).Distinct().OrderBy(x => x.Year).ToList();

                model.Titulo = new CuantityReportTemplate.Table { Header = new List<string>() { "Carrera Profesional" } };
                for (int i = 0; i < yearsTitulo.Count; i++)
                    model.Titulo.Header.Add(yearsTitulo[i].Year.ToString());
                model.Titulo.Header.Add("Sub Total");

                model.Titulo.Content = new List<List<string>>();
                for (int i = 0; i < carrerasTitulo.Count; i++)
                {
                    List<string> data = new List<string> { carrerasTitulo[i].Carrera };
                    int sub = 0;
                    for (int j = 0; j < yearsTitulo.Count(); j++)
                    {
                        int cant = dataTitulo.Where(x => x.Carrera == carrerasTitulo[i].Carrera && x.Year == yearsTitulo[j].Year).Count();
                        data.Add(cant.ToString());
                        sub += cant;
                    }
                    data.Add(sub.ToString());
                    model.Titulo.Content.Add(data);
                }

                model.Titulo.Footer = new List<string>() { "Sub Total" };
                for (int i = 0; i < yearsTitulo.Count; i++)
                    model.Titulo.Footer.Add(dataTitulo.Where(x => x.Year == yearsTitulo[i].Year).Count().ToString());
                model.Titulo.Footer.Add(dataTitulo.Count().ToString());
            }
            return model;
        }



        public async Task<object> GetBachelors(List<Guid> Faculties, List<Guid> Careers, List<Guid> AcademicPrograms, int GradeType)
        {
            var query = _context.Students.Include(x => x.Career.Faculty).Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR || x.Status == ConstantHelpers.Student.States.QUALIFIED)
                .Include(x => x.AcademicProgram).AsQueryable();
            if (GradeType > 0)
            {
                if (GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR)
                {
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR);
                }
                else
                {
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED);
                }
            }
            if (Faculties != null)
            {
                if (Faculties.Count > 0)
                {
                    query = query.Where(x => Faculties.Contains(x.Career.FacultyId));
                }
            }
            if (Careers != null)
            {
                if (Careers.Count > 0)
                {
                    query = query.Where(x => Careers.Contains(x.CareerId));
                }
            }
            if (AcademicPrograms != null)
            {
                if (AcademicPrograms.Count > 0)
                {
                    query = query.Where(x => AcademicPrograms.Contains(x.AcademicProgramId.Value));
                }
            }

            var count = await query.CountAsync();

            var result = await query
                .GroupBy(x => x.Career.Name)
                .Select(x => new
                {
                    Program = x.Key,
                    BachelorCount = x.Count()
                })
                .ToListAsync();

            return result;
        }

        public async Task<object> GetBachelorsWithOutConfiguration(List<Guid> Faculties, List<Guid> Careers = null, List<Guid> AcademicPrograms = null)
        {
            int hasData = 0;
            int notHasData = 0;
            var query = _context.Students.Include(x => x.Career.Faculty).Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED).AsQueryable();

            if (Faculties != null)
            {
                if (Faculties.Count > 0)
                {
                    query = query.Where(x => Faculties.Contains(x.Career.FacultyId));
                }
            }
            if (Careers != null)
            {
                if (Careers.Count > 0)
                {
                    query = query.Where(x => Careers.Contains(x.CareerId));
                }
            }
            if (AcademicPrograms != null)
            {
                if (AcademicPrograms.Count > 0)
                {
                    query = query.Where(x => AcademicPrograms.Contains(x.AcademicProgramId.Value));
                }
            }
            notHasData = await query.Where(x => x.RegistryPatterns.Count == 0).CountAsync();
            hasData = await query.Where(x => x.RegistryPatterns.Count > 0).CountAsync();

            var obj = new { Has = hasData, NotHave = notHasData };
            return obj;
        }

        #region STUDENT INFORMATION DATA TABLE
        public async Task<DataTablesStructs.ReturnedData<StudentInformationDataTableTemplate>> GetStudentInformationDataTable(DataTablesStructs.SentParameters parameters, Guid careerId)
        {
            return await GetStudentInformationDataTable(parameters, careerId, null, GetStudentInformationDataTableOrderByPredicate(parameters), GetStudentInformationDataTableSearchValuePredicate());
        }
        private async Task<DataTablesStructs.ReturnedData<StudentInformationDataTableTemplate>> GetStudentInformationDataTable(
             DataTablesStructs.SentParameters sentParameters,
             Guid careerId,

             Expression<Func<StudentInformationDataTableTemplate, StudentInformationDataTableTemplate>> selectPredicate = null,
             Expression<Func<StudentInformationDataTableTemplate, dynamic>> orderByPredicate = null,
             Func<StudentInformationDataTableTemplate, string[]> searchValuePredicate = null)
        {
            var query = _context.Students.Where(x => x.CareerId == careerId).AsQueryable();

            var result = query
                .Select(x => new StudentInformationDataTableTemplate
                {
                    Username = x.User.UserName,
                    Name = x.User.Name,
                    Paternalsurname = x.User.PaternalSurname,
                    Maternalsurname = x.User.MaternalSurname,
                    Email = x.User.Email,
                    Existfile = x.StudentInformationId.HasValue == false ? false : true
                });
            return await result.ToDataTables(sentParameters, selectPredicate);
        }
        private Expression<Func<StudentInformationDataTableTemplate, dynamic>> GetStudentInformationDataTableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                //case "1":
                //    return ((x) => x.Code);
                //case "2":
                //    return ((x) => x.Career);
                //case "3":
                //    return ((x) => x.AcademicProgram);
                //case "4":
                //    return ((x) => x.Intents);
                //case "5":
                //    return ((x) => x.Grade);
                //case "6":
                //    return ((x) => x.Approbed);
                default:
                    return ((x) => x.Name);
            }
        }
        private Func<StudentInformationDataTableTemplate, string[]> GetStudentInformationDataTableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name  +"",
            };
        }
        #endregion

        #region STUDENT PERSONAL INFORMATION DATA TABLE
        public async Task<DataTablesStructs.ReturnedData<object>> GetReportPersonalInformationDataTable(DataTablesStructs.SentParameters parameters, Guid? careerId = null, Guid? departmentId = null, Guid? provinceId = null, Guid? districtId = null, int? sex = null, int? schoolType = null, int? universityPreparation = null, Guid? admissionTypeId = null, int? startAge = null, int? endAge = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.PaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.MaternalSurname;
                    break;
                case "4":
                    orderByPredicate = (x) => x.User.Email;
                    break;
                case "5":
                    orderByPredicate = (x) => x.StudentInformation.Term.Name;
                    break;
            }

            var query = _context.Students.Where(x => x.StudentInformationId != null).AsNoTracking();

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);


            if (departmentId != null)
                query = query.Where(x => x.User.DepartmentId == departmentId);

            if (provinceId != null)
                query = query.Where(x => x.User.ProvinceId == provinceId);

            if (districtId != null)
                query = query.Where(x => x.User.DistrictId == districtId);

            if (sex != null)
                query = query.Where(x => x.User.Sex == sex);

            if (schoolType != null)
                query = query.Where(x => x.StudentInformation.SchoolType == schoolType);

            if (universityPreparation != null)
                query = query.Where(x => x.StudentInformation.UniversityPreparationId == universityPreparation);

            if (admissionTypeId != null)
                query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (startAge != null)
            {
                var ageBefore = DateTime.UtcNow.Year - startAge;
                query = query.Where(x => x.User.BirthDate.Year <= ageBefore);
            }

            if (endAge != null)
            {
                var ageAfter = DateTime.UtcNow.Year - endAge;
                query = query.Where(x => x.User.BirthDate.Year >= ageAfter);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    studentId = x.Id,
                    termId = x.StudentInformation.TermId,
                    x.User.UserName,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    x.User.Name,
                    x.User.Email,
                    termName = x.StudentInformation.Term.Name
                })
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
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

        #endregion

        public async Task<StudyRecordTemplate> GetStudyRecord(Guid studentId)
        {
            var model = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new StudyRecordTemplate
                {
                    Code = x.User.UserName,
                    Student = x.User.FullName,
                    Faculty = x.Career.Faculty.Name,
                    AcademicYear = x.CurrentAcademicYear.ToString("D2"),
                    Sex = x.User.Sex,
                }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<Student> GetStudentProofOfInCome(Guid studentId)
        {
            var student = await _context.Students
                            .Include(x => x.User)
                            .Include(x => x.AdmissionType)
                            .Include(x => x.AdmissionTerm)
                            .Include(x => x.Career)
                            .ThenInclude(x => x.Faculty)
                            .Where(x => x.Id == studentId).FirstOrDefaultAsync();

            return student;
        }

        public async Task<Student> GetStudentRecordEnrollment(Guid studentId)
        {

            var student = await _context.Students
                                        .Include(x => x.User)
                                        .Include(x => x.AdmissionTerm)
                                        .Include(x => x.Career)
                                        .ThenInclude(x => x.Faculty)
                                        .Where(x => x.Id == studentId).FirstOrDefaultAsync();

            return student;
        }

        public async Task<Student> GetStudentWithCareerAcademicUser(Guid studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId)
                .Include(x => x.Career.Faculty)
                .Include(x => x.AcademicProgram)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            return student;
        }

        public async Task<Student> GetStudentWithCareerAdmissionAcademicUser(Guid studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId)
                .Include(x => x.Career)
                .Include(x => x.Career.Faculty)
                .Include(X => X.AdmissionTerm)
                .Include(x => x.AcademicProgram)
                .Include(x => x.User).FirstOrDefaultAsync();

            return student;
        }

        public async Task<IEnumerable<Student>> GetGraduateds()
        {
            return await _context.Students
                           .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED)
                           .ToListAsync();
        }

        public async Task<HeadBoardCertificateTemplate> GetStudntCertificate(Guid studentId, string university)
        {
            var student = await _context.Students.Where(x => x.Id == studentId).Select(x => new HeadBoardCertificateTemplate
            {
                IdStudent = x.Id,
                CareerName = x.Career.Name,
                FacultyName = x.Career.Faculty.Name,
                FullName = x.User.FullName,
                UserName = x.User.UserName,
                AdmissionYear = x.AdmissionTerm.Name == null ? "--" : x.AdmissionTerm.Name.Substring(0, 4),
                UniversityName = university,
                GraduationYear = x.GraduationTerm.Name.Substring(0, 4),
                CurriculumId = x.CurriculumId,
                Dni = x.User.Dni,
                AcademicProgram = x.AcademicProgramId.HasValue ? x.AcademicProgram.Name : null,
                StudentSex = x.User.Sex,
                BachelorName = x.Curriculum.AcademicDegreeBachelor
            }).FirstOrDefaultAsync();

            return student;
        }

        public async Task<IEnumerable<Student>> GetStudentsBySeciontId(Guid sectionId)
        {
            return await _context.Students.Include(x => x.User).Where(x => x.StudentSections.Any(y => y.SectionId == sectionId)).ToArrayAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituation(DataTablesStructs.SentParameters sentParameters, Guid tid, Guid fid, Guid cid, string searchValue = null, int? academicOrder = null, ClaimsPrincipal user = null, Guid? curriculumId = null, int? year = null)
        {
            Expression<Func<AcademicSummary, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.MeritOrder;
                    break;
                case "4":
                    orderByPredicate = (x) => x.WeightedAverageGrade;
                    break;
                case "5":
                    orderByPredicate = (x) => x.MeritType;
                    break;
                case "6":
                    orderByPredicate = (x) => x.ApprovedCredits;
                    break;
                default:
                    break;

            }

            var query = _context.AcademicSummaries.Where(x => x.TermId == tid).AsNoTracking();

            if (fid != Guid.Empty) query = query.Where(x => x.Career.FacultyId == fid);

            if (cid != Guid.Empty) query = query.Where(x => x.CareerId == cid);

            if (curriculumId.HasValue && curriculumId != Guid.Empty) query = query.Where(x => x.CurriculumId == curriculumId);

            if (year.HasValue) query = query.Where(x => x.StudentAcademicYear == year);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (academicOrder.HasValue)
            {
                switch (academicOrder.Value)
                {
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH:
                        query = query.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH);
                        break;
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH:
                        query = query.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH);
                        break;
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD:
                        query = query.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD);
                        break;
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF:
                        query = query.Where(x => x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD || x.MeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF);
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchValue)) query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            var recorsTotal = await query.CountAsync();
            var recordsFiltered = recorsTotal;

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Student.User.FullName,
                    code = x.Student.User.UserName,
                    career = x.Career.Name,
                    careerId = x.CareerId,
                    facultyId = x.Career.FacultyId,
                    order = x.MeritOrder,
                    approvedCredits = x.ApprovedCredits.ToString(),
                    grade = x.WeightedAverageGrade.ToString("0.00"),
                    meritType = x.MeritType
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recorsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituationCumulative(DataTablesStructs.SentParameters sentParameters, Guid tid, Guid fid, Guid cid, string searchValue = null, int? academicOrder = null, ClaimsPrincipal user = null)
        {
            Expression<Func<AcademicSummary, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.TotalOrder;
                    break;
                case "4":
                    orderByPredicate = (x) => x.WeightedAverageCumulative;
                    break;
                case "5":
                    orderByPredicate = (x) => x.TotalMeritType;
                    break;
                case "6":
                    orderByPredicate = (x) => x.ApprovedCredits;
                    break;
                default:
                    orderByPredicate = (x) => x.TotalOrder;
                    break;

            }

            var query = _context.AcademicSummaries.Where(x => x.TermId == tid).AsNoTracking();

            if (fid != Guid.Empty) query = query.Where(x => x.Career.FacultyId == fid);
            if (cid != Guid.Empty) query = query.Where(x => x.CareerId == cid);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (academicOrder.HasValue)
            {
                switch (academicOrder.Value)
                {
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH:
                        query = query.Where(x => x.TotalMeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH);
                        break;
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH:
                        query = query.Where(x => x.TotalMeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH || x.TotalMeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH);
                        break;
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD:
                        query = query.Where(x => x.TotalMeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH || x.TotalMeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH || x.TotalMeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD);
                        break;
                    case ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF:
                        query = query.Where(x => x.TotalMeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH || x.TotalMeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH || x.TotalMeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD || x.TotalMeritType == ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF);
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchValue)) query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper()) || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            var recorsTotal = await query.CountAsync();
            var recordsFiltered = recorsTotal;

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Student.User.FullName,
                    code = x.Student.User.UserName,
                    career = x.Career.Name,
                    careerId = x.CareerId,
                    facultyId = x.Career.FacultyId,
                    order = x.TotalOrder,
                    approvedCredits = x.ApprovedCredits.ToString(),
                    grade = x.WeightedAverageCumulative.ToString("0.00"),
                    meritType = x.TotalMeritType
                })
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recorsTotal
            };
        }

        public async Task<IEnumerable<AcademicSituationExcelTemplate>> GetStudentAcademicSituationExcelTemplate(Guid? termId, Guid? facultyId, Guid? careerId, int? academicOrder, ClaimsPrincipal user = null)
        {
            var query = _context.Students.AsNoTracking();

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (academicOrder.HasValue)
            {
                if (academicOrder.Value == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD ||
                    academicOrder.Value == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH ||
                    academicOrder.Value == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH)
                {
                    query = query.Where(x => x.AcademicSummaries.Where(a => a.TermId == termId).Select(a => a.MeritType).FirstOrDefault() != ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF && x.AcademicSummaries.Where(a => a.TermId == termId).Select(a => a.MeritType).FirstOrDefault() >= academicOrder.Value);
                }
                else if (academicOrder.Value == ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF)
                {
                    query = query.Where(x => x.AcademicSummaries.Where(a => a.TermId == termId).Select(a => a.MeritType).FirstOrDefault() >= ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD);
                }
            }

            var data = await query
                .Select(x => new AcademicSituationExcelTemplate
                {
                    Name = x.User.FullName,
                    Dni = x.User.Dni,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    LastOrder = x.AcademicSummaries.Where(a => a.TermId == termId).Select(a => a.MeritOrder.ToString()).FirstOrDefault() ?? "---", //x.AcademicSummaries.Any() ? x.AcademicSummaries.OrderByDescending(asm => asm.Term.StartDate).FirstOrDefault().Order.ToString() : "-",
                    AprrovedCredits = x.AcademicSummaries.Where(a => a.TermId == termId).Select(a => a.ApprovedCredits.ToString()).FirstOrDefault() ?? "---", //x.AcademicSummaries.Any() ? x.AcademicSummaries.Sum(asm => asm.ApprovedCredits).ToString() : "---",
                    LastGrade = x.AcademicSummaries.Where(a => a.TermId == termId).Select(a => a.WeightedAverageGrade.ToString("0.00")).FirstOrDefault() ?? "---", //x.AcademicSummaries.Any() ? x.AcademicSummaries.OrderByDescending(asm => asm.Term.StartDate).FirstOrDefault().WeightedFinalGrade.ToString() : "-",
                    LastMeritOrder = x.AcademicSummaries.Where(a => a.TermId == termId).Select(a => a.MeritType).FirstOrDefault() //x.AcademicSummaries.Any() ? x.AcademicSummaries.OrderByDescending(asm => asm.Term.StartDate).FirstOrDefault().MeritOrder : null}).AsNoTracking();
                })
                .ToArrayAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituationWithOutComent(DataTablesStructs.SentParameters sentParameters, Guid fid, Guid cid, int? academicOrder = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CareerId;
                    break;
                default:
                    orderByPredicate = (x) => x.User.FullName;
                    break;

            }

            var query = _context.Students.Select(x => new
            {
                id = x.Id,
                name = x.User.FullName,
                career = x.Career.Name,
                careerId = x.CareerId,
                facultyId = x.Career.FacultyId,
                lastOrder = x.AcademicSummaries.Any() ? x.AcademicSummaries.OrderByDescending(asm => asm.Term.StartDate).FirstOrDefault().MeritOrder.ToString() : "-",
                approvedCredits = x.AcademicSummaries.Any() ? x.AcademicSummaries.Sum(asm => asm.ApprovedCredits).ToString() : "-",
                lastGrade = x.AcademicSummaries.Any() ? x.AcademicSummaries.OrderByDescending(asm => asm.Term.StartDate).FirstOrDefault().WeightedAverageGrade.ToString() : "-",
                lastMeritOrder = x.AcademicSummaries.Any() ? x.AcademicSummaries.OrderByDescending(asm => asm.Term.StartDate).FirstOrDefault().MeritOrder : CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.NONE
            }).AsNoTracking();
            if (fid != Guid.Empty)
                query = query.Where(x => x.facultyId == fid);
            if (cid != Guid.Empty)
                query = query.Where(x => x.careerId == cid);
            if (academicOrder.HasValue)
            {
                if (academicOrder.Value == ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD ||
                    academicOrder.Value == ConstantHelpers.ACADEMIC_ORDER.UPPER_FIFTH ||
                    academicOrder.Value == ConstantHelpers.ACADEMIC_ORDER.UPPER_TENTH)
                {
                    query = query.Where(x => x.lastMeritOrder != ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF && x.lastMeritOrder >= academicOrder.Value);
                }
                else if (academicOrder.Value == ConstantHelpers.ACADEMIC_ORDER.UPPER_HALF)
                {
                    query = query.Where(x => x.lastMeritOrder >= ConstantHelpers.ACADEMIC_ORDER.UPPER_THIRD);
                }
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.id,
                    name = x.name,
                    career = x.career,
                    careerId = x.careerId,
                    facultyId = x.facultyId,
                    lastOrder = x.lastOrder,
                    approvedCredits = x.approvedCredits,
                    lastGrade = x.lastGrade,
                    lastMeritOrder = x.lastMeritOrder
                }).ToListAsync();

            var recorsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recorsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableAcademicSituationDean(DataTablesStructs.SentParameters sentParameters, Guid fid, Guid cid, int? academicOrder = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CareerId;
                    break;
                default:
                    orderByPredicate = (x) => x.User.FullName;
                    break;

            }

            var query = _context.Students.Where(x => x.Career.FacultyId == fid)
                            .Select(x => new
                            {
                                id = x.Id,
                                name = x.User.FullName,
                                career = x.Career.Name,
                                careerId = x.CareerId,
                                facultyId = x.Career.FacultyId,
                                lastOrder = x.AcademicSummaries.Any()
                                    ? x.AcademicSummaries.OrderByDescending(asm => asm.Term.StartDate).FirstOrDefault().MeritOrder
                                        .ToString()
                                    : "-",
                                approvedCredits = x.AcademicSummaries.Any()
                                    ? x.AcademicSummaries.Sum(asm => asm.ApprovedCredits).ToString()
                                    : "-",
                                lastGrade = x.AcademicSummaries.Any()
                                    ? x.AcademicSummaries.OrderByDescending(asm => asm.Term.StartDate).FirstOrDefault()
                                        .WeightedAverageGrade.ToString()
                                    : "-",
                                lastMeritOrder = x.AcademicSummaries.Any()
                                    ? x.AcademicSummaries.OrderByDescending(asm => asm.Term.StartDate).FirstOrDefault().MeritType
                                    : CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.NONE
                            }).AsNoTracking();

            if (cid != Guid.Empty)
                query = query.Where(x => x.careerId == cid);
            if (academicOrder.HasValue)
                query = query.Where(x => x.lastMeritOrder >= academicOrder.Value);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.id,
                    name = x.name,
                    career = x.career,
                    careerId = x.careerId,
                    facultyId = x.facultyId,
                    lastOrder = x.lastOrder,
                    approvedCredits = x.approvedCredits,
                    lastGrade = x.lastGrade,
                    lastMeritOrder = x.lastMeritOrder
                }).ToListAsync();

            var recorsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recorsTotal
            };
        }
        public async Task<Student> GetStudentHome(string userId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.UserId.Equals(userId));

            return student;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithCreditsDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? type = null, string searchValue = null, bool? onlyWithCredits = null, ClaimsPrincipal user = null)
        {
            var watch = new Stopwatch();
            watch.Restart();

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                default:
                    break;
            }

            var query = _context.Students
                .FilterActiveStudents()
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

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty)
                    query = query.Where(x => x.Career.FacultyId == facultyId);


                if (careerId.HasValue && careerId != Guid.Empty)
                    query = query.Where(x => x.CareerId == careerId);
            }

            if (type.HasValue && type != -1) query = query.Where(x => x.Status == type);

            if (onlyWithCredits.HasValue && onlyWithCredits.Value)
                query = query.Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId));


            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue) || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                    || x.User.UserName.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    academicYear = x.CurrentAcademicYear,
                    type = ConstantHelpers.Student.States.VALUES.ContainsKey(x.Status) ? ConstantHelpers.Student.States.VALUES[x.Status] : "---",
                    credits = x.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    id = x.Id,
                    academicProgram = x.AcademicProgram.Name
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var time = watch.Elapsed;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<Student> GetStudentWithTmpEnrollmentData(Guid studentId, Guid termId)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync();

            return student;
        }

        public async Task<IEnumerable<Student>> GetAllByFacultyOrCareer(Guid facultyId, Guid careerId)
        {
            var query = _context.Students.AsQueryable();

            if (facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);
            if (careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsAcademicHistoryDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid? faculty = null, Guid? career = null, string search = null)
        {

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
            }

            var query = _context.Students
                .Where(x => x.GraduationTermId != null).AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

                if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);
            }

            var recordsFiltered = await query.CountAsync();


            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }
            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    grade = x.AcademicSummaries.OrderByDescending(y => y.Term.Name).Select(y => y.WeightedAverageCumulative).FirstOrDefault(),
                    graduationTerm = x.GraduationTermId != null ? x.GraduationTerm.Name : "",
                    firstEnrollmentDate = x.FirstEnrollmentDate != null ? x.FirstEnrollmentDate.ToLocalDateFormat() : ""
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

        public async Task<List<AlumniExcelTemplate>> GetStudentToExcelAlumni(Guid? faculty = null, Guid? career = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                  .Where(x => x.GraduationTermId != null).AsQueryable();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

                if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);
            }

            var students = await query.Select(x => new AlumniExcelTemplate
            {
                Code = x.User.UserName,
                Name = x.User.FullName,
                Career = x.Career.Name,
                Faculty = x.Career.Faculty.Name,
                Grade = x.AcademicSummaries.OrderByDescending(y => y.Term.Name).Select(y => y.WeightedAverageCumulative).FirstOrDefault()
            }).ToListAsync();

            return students;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetSanctionedStudentDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string userId, Guid? faculty = null, Guid? career = null, string search = null, Guid? term = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                default:
                    //orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
            }

            var query = _context.Students
            .Where(x => x.Status == CORE.Helpers.ConstantHelpers.Student.States.SANCTIONED).AsNoTracking();

            if (user != null)
            {
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    var careers = await _context.Careers
                          .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                          .Select(x => x.Id).ToListAsync();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

            if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                //.Skip(sentParameters.PagingFirstRecord)
                //.Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    lastTermId = x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.TermId).FirstOrDefault(),
                    lastTerm = x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.Term.Name).FirstOrDefault(),
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    year = x.CurrentAcademicYear,
                    end = x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.Term.EndDate).FirstOrDefault(),
                    status = 0
                }).ToListAsync();

            if (term.HasValue && term != Guid.Empty)
                data = data.Where(x => x.lastTermId == term.Value).ToList();

            var recordsFiltered = data.Count;
            var recordsTotal = data.Count;

            var terms = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2"))
                .Select(x => new
                {
                    x.Id,
                    x.StartDate
                }).ToListAsync();

            var minTermsRequired = 2;

            var pagedList = data
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.id,
                    x.lastTerm,
                    x.code,
                    x.name,
                    x.career,
                    x.faculty,
                    x.year,
                    status = terms.Where(y => x.end < y.StartDate && y.StartDate <= DateTime.UtcNow).Count() >= minTermsRequired ? 1
                : terms.Where(y => x.end < y.StartDate && y.StartDate <= DateTime.UtcNow).Count() >= minTermsRequired / 2.0 ? 2
                : 3
                }).ToList();


            return new DataTablesStructs.ReturnedData<object>
            {
                Data = pagedList,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDesertionStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                default:
                    //orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
            }

            var query = _context.Students
            .Where(x => x.Status == ConstantHelpers.Student.States.DESERTION).AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

                if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);
            }

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var pagedList = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    lastTerm = x.AcademicSummaries.OrderByDescending(y => y.Term.Name).Select(y => y.Term.Name).FirstOrDefault(),
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    year = x.CurrentAcademicYear,
                    end = x.AcademicSummaries.OrderByDescending(y => y.Term.Name).Select(y => y.Term.EndDate).FirstOrDefault(),
                    //status = 0
                }).ToListAsync();

            var recordsTotal = pagedList.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = pagedList,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<SacntionedExcelTemplate>> GetStudentToExcelSantioned(Guid? term = null, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                .Where(x => x.Status == CORE.Helpers.ConstantHelpers.Student.States.SANCTIONED)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    var careers = await _context.Careers
                          .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                          .Select(x => x.Id).ToListAsync();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

            if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var data = await query
                .Select(x => new
                {
                    LastTermId = x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.TermId).FirstOrDefault(),
                    LastTerm = x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.Term.Name).FirstOrDefault(),
                    Code = x.User.UserName,
                    Name = x.User.FullName,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    Year = x.CurrentAcademicYear
                }).ToListAsync();

            if (term.HasValue && term != Guid.Empty)
                data = data.Where(x => x.LastTermId == term.Value).ToList();

            var students = data
                .Select(x => new SacntionedExcelTemplate
                {
                    LastTerm = x.LastTerm,
                    Code = x.Code,
                    Name = x.Name,
                    Career = x.Career,
                    Faculty = x.Faculty,
                    Year = x.Year
                }).ToList();

            return students;
        }

        public async Task<List<SacntionedExcelTemplate>> GetStudentToExcelDesertion(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.DESERTION)
                .AsQueryable();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

                if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);
            }

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var students = await query
                .Select(x => new SacntionedExcelTemplate
                {
                    LastTerm = x.AcademicSummaries.OrderByDescending(y => y.Term.Name).Select(y => y.Term.Name).FirstOrDefault(),
                    Code = x.User.UserName,
                    Name = x.User.FullName,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    Year = x.CurrentAcademicYear
                }).ToListAsync();

            return students;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsUnbeatenDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string userId, Guid? faculty = null, Guid? career = null, string search = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
            }


            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.UNBEATEN)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
            {
                var qryCareers = _context.Careers
                      .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                      .AsNoTracking();

                if (faculty.HasValue && faculty != Guid.Empty)
                    qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                if (career.HasValue && career != Guid.Empty)
                    qryCareers = qryCareers.Where(x => x.Id == career);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

                if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);
            }

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    academicYear = x.CurrentAcademicYear
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsAcademicSummaryDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid termId, Guid? careerId, bool? unbeaten, string search = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                default:
                    orderByPredicate = (x) => x.User.FullName;
                    break;
            }


            var query = _context.Students.Where(x => x.AcademicSummaries.Any(y => y.TermId == termId))
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    var careers = await _context.Careers
                          .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                          .Select(x => x.Id).ToListAsync();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (unbeaten.HasValue)
            {
                if (unbeaten.Value)
                {
                    query = query.Where(x => x.AcademicSummaries.Any(y => y.TermId == termId && y.ApprovedCredits == y.TotalCredits));
                }
                else
                {
                    query = query.Where(x => x.AcademicSummaries.Any(y => y.TermId == termId && y.ApprovedCredits != y.TotalCredits));
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    academicYear = x.CurrentAcademicYear,
                    totalCredits = x.AcademicSummaries.Where(y => y.TermId == termId).Select(y => y.TotalCredits).FirstOrDefault(),
                    approvedCredits = x.AcademicSummaries.Where(y => y.TermId == termId).Select(y => y.ApprovedCredits).FirstOrDefault()
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<StudentAcademicSummaryTemplate>> GetStudentsAcademicSummaryData(ClaimsPrincipal user, Guid termId, Guid? careerId, bool? unbeaten, string search = null)
        {
            var query = _context.Students
                .Where(x => x.AcademicSummaries.Any(y => y.TermId == termId))
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    var careers = await _context.Careers
                          .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                          .Select(x => x.Id).ToListAsync();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (unbeaten.HasValue)
            {
                if (unbeaten.Value)
                    query = query.Where(x => x.AcademicSummaries.Any(y => y.TermId == termId && y.ApprovedCredits == y.TotalCredits));
                else
                    query = query.Where(x => x.AcademicSummaries.Any(y => y.TermId == termId && y.ApprovedCredits != y.TotalCredits));
            }

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var data = await query
                .Select(x => new StudentAcademicSummaryTemplate
                {
                    Id = x.Id,
                    Code = x.User.UserName,
                    FullName = x.User.FullName,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    AcademicYear = x.CurrentAcademicYear,
                    TotalCredits = x.AcademicSummaries.Where(y => y.TermId == termId).Select(y => y.TotalCredits).FirstOrDefault(),
                    ApprovedCredits = x.AcademicSummaries.Where(y => y.TermId == termId).Select(y => y.ApprovedCredits).FirstOrDefault()
                }).ToListAsync();

            return data;
        }


        public async Task<List<UnbeatenExcelTemplate>> GetStudentToExcelUnbeaten(Guid? faculty = null, Guid? career = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
             .Where(x => x.Status == ConstantHelpers.Student.States.UNBEATEN)
             .AsQueryable();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

                if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);
            }

            var students = await query.Select(x => new UnbeatenExcelTemplate
            {
                Code = x.User.UserName,
                Name = x.User.FullName,
                Career = x.Career.Name,
                Faculty = x.Career.Faculty.Name,
                AcademicYear = x.CurrentAcademicYear
            }).ToListAsync();

            return students;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GraduatedListReport(DataTablesStructs.SentParameters sentParameters, bool isCoordinator, List<Guid> careers, int gradeType, Guid careerParameterId, int year = 0, int admissionYear = 0)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.PaternalSurname); break;
                case "2":
                    orderByPredicate = ((x) => x.User.MaternalSurname); break;
                case "3":
                    orderByPredicate = ((x) => x.User.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.Career.Faculty.Name); break;
                case "5":
                    orderByPredicate = ((x) => x.Career.Name); break;
                case "6":
                    orderByPredicate = ((x) => x.AcademicProgram.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.User.Sex); break;
                case "8":
                    orderByPredicate = ((x) => x.StudentExperiences.Count); break;
                case "9":
                    orderByPredicate = ((x) => x.User.PhoneNumber); break;
                case "10":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "11":
                    orderByPredicate = ((x) => x.User.Dni); break;
                case "12":
                    orderByPredicate = ((x) => x.User.Address); break;
            }


            var query = _context.Students.AsQueryable();

            if (isCoordinator)
            {
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            if (careerParameterId != Guid.Empty)
            {
                query = query.Where(x => x.CareerId == careerParameterId);
            }
            if (year > 0)
            {
                query = query.Where(x => x.GraduationTermId != null && x.GraduationTerm.Year == year);
            }
            if (admissionYear > 0)
            {
                query = query.Where(x => x.AdmissionTerm.Year == admissionYear);
            }

            switch (gradeType)
            {
                case ConstantHelpers.Student.States.BACHELOR:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR);
                    break;
                case ConstantHelpers.Student.States.QUALIFIED:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED);
                    break;
                case ConstantHelpers.Student.States.GRADUATED:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED);
                    break;
                default:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED || x.Status == ConstantHelpers.Student.States.BACHELOR || x.Status == ConstantHelpers.Student.States.QUALIFIED);
                    break;

            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
               .Select(x => new
               {
                   code = x.User.UserName,
                   paternalSurname = x.User.PaternalSurname,
                   maternalSurname = x.User.MaternalSurname,
                   name = x.User.Name,
                   fullName = x.User.FullName,
                   sex = ConstantHelpers.SEX.ABREV.ContainsKey(x.User.Sex) ? ConstantHelpers.SEX.ABREV[x.User.Sex] : "-",
                   practices = x.StudentExperiences.Count == 0 ? "NO" : "SI",
                   number = x.User.PhoneNumber,
                   faculty = x.Career.Faculty.Name,
                   career = x.Career.Name,
                   AcademicProgram = x.AcademicProgram.Name,
                   email = x.User.Email ?? "-",
                   dni = String.IsNullOrEmpty(x.User.Dni) ? "-" : x.User.Dni,
                   address = String.IsNullOrEmpty(x.User.Address) ? "-" : x.User.Address,
                   grade = "--",
                   year = "--"
               }).ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }



        public async Task<DataTablesStructs.ReturnedData<object>> GetNonEnrolledStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null, int? year = null, bool excludeLastYear = false)
        {
            var watch = new Stopwatch();
            watch.Restart();

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            var prevRegularTerm = await _context.Terms
            .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2"))
            .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number)
            .FirstOrDefaultAsync();

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Curriculum.Code;
                    break;
                case "5":
                    orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                case "6":
                    if (prevRegularTerm != null)
                        orderByPredicate = (x) => x.StudentSections.Where(y => y.Section.CourseTerm.TermId == prevRegularTerm.Id).Max(y => y.Try);

                    break;
                default:
                    //orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
            }

            var activeTerm = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            if (activeTerm == null)
                activeTerm = new Term();

            var query = _context.Students
                .FilterActiveStudents()
                //.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == prevRegularTerm.Id) 
                //&& !x.StudentSections.Any(y => y.Section.CourseTerm.TermId == activeTerm.Id))
                .AsNoTracking();

            //var recordsTotal = await query.CountAsync();

            if (prevRegularTerm != null)
                //query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == prevRegularTerm.Id));
                query = query.Where(x => x.AcademicSummaries.Any(y => y.TermId == prevRegularTerm.Id));

            query = query.Where(x => !x.StudentSections.Any(y => y.Section.CourseTerm.TermId == activeTerm.Id));

            //var query2 = await query.Select(x =>new {
            //    studentsections  = x.StudentSections.Select(y=> y.Section.CourseTerm.TermId).ToList(),
            //    stuentId = x.Id
            //}).ToListAsync();

            //var users = query2.Where(x => x.studentsections.Contains(prevRegularTerm.Id)
            //    && !x.studentsections.Contains(activeTerm.Id))
            //    .Select(x => x.stuentId).ToList();

            //query = query.Where(x => users.Contains(x.Id));

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers
                            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                            .AsNoTracking();

                        if (faculty.HasValue && faculty != Guid.Empty)
                            qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                        if (career.HasValue && career != Guid.Empty)
                            qryCareers = qryCareers.Where(x => x.Id == career);

                        var careers = qryCareers.Select(x => x.Id).ToHashSet();

                        query = query.Where(x => careers.Contains(x.CareerId));
                    }
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId || x.Career.Faculty.AdministrativeAssistantId == userId);
                }
            }

            if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);
            if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            if (year.HasValue) query = query.Where(x => x.CurrentAcademicYear == year);

            if (excludeLastYear)
                query = query.Where(x => x.CurrentAcademicYear != x.Curriculum.AcademicYearCourses.Max(y => y.AcademicYear));

            var recordsFiltered = await query.CountAsync();

            var dbData = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    year = x.CurrentAcademicYear,
                    curriculum = x.Curriculum.Code,
                    @try = prevRegularTerm == null ? 0 : x.StudentSections.Where(y => y.Section.CourseTerm.TermId == prevRegularTerm.Id).Max(y => y.Try),
                    isDirected = prevRegularTerm == null ? false : x.StudentSections.Where(y => y.Section.CourseTerm.TermId == prevRegularTerm.Id).Any(y => y.Section.IsDirectedCourse),
                }).ToListAsync();

            var pagedList = dbData
                .Select(x => new
                {
                    x.id,
                    x.code,
                    x.name,
                    x.career,
                    x.faculty,
                    x.year,
                    x.curriculum,
                    condition = x.isDirected ? "DIRIGIDO"
                    : ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES.ContainsKey(x.@try) ? ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[x.@try] : "REGULAR"
                }).ToList();

            var time = watch.Elapsed;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = pagedList,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<NonEnrolledStudentTemplate>> GetNonEnrolledStudentData(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null, int? year = null, bool excludeLastYear = false)
        {
            var prevRegularTerm = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2"))
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number)
                .FirstOrDefaultAsync();

            var activeTerm = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (activeTerm == null) activeTerm = new Term();

            var query = _context.Students
                .FilterActiveStudents()
                .AsNoTracking();

            if (prevRegularTerm != null)
                query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == prevRegularTerm.Id));

            query = query.Where(x => !x.StudentSections.Any(y => y.Section.CourseTerm.TermId == activeTerm.Id));

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers
                            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                            .AsNoTracking();

                        if (faculty.HasValue && faculty != Guid.Empty)
                            qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                        if (career.HasValue && career != Guid.Empty)
                            qryCareers = qryCareers.Where(x => x.Id == career);

                        var careers = qryCareers.Select(x => x.Id).ToHashSet();

                        query = query.Where(x => careers.Contains(x.CareerId));
                    }
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId || x.Career.Faculty.AdministrativeAssistantId == userId);
                }
            }

            if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);
            if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            if (year.HasValue) query = query.Where(x => x.CurrentAcademicYear == year);

            if (excludeLastYear)
                query = query.Where(x => x.CurrentAcademicYear != x.Curriculum.AcademicYearCourses.Max(y => y.AcademicYear));

            var data = await query
                .Select(x => new NonEnrolledStudentTemplate
                {
                    Id = x.Id,
                    UserName = x.User.UserName,
                    FullName = x.User.FullName,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    AcademicYear = x.CurrentAcademicYear,
                    Curriculum = x.Curriculum.Code,
                    Try = prevRegularTerm == null ? 0 : x.StudentSections.Where(y => y.Section.CourseTerm.TermId == prevRegularTerm.Id).Max(y => y.Try),
                    HasDirectedCourse = prevRegularTerm == null ? false : x.StudentSections.Where(y => y.Section.CourseTerm.TermId == prevRegularTerm.Id).Any(y => y.Section.IsDirectedCourse),
                }).ToListAsync();

            //var data = dbData
            //    .Select(x => new NonEnrolledStudentTemplate
            //    {
            //        x.id,
            //        x.code,
            //        x.name,
            //        x.career,
            //        x.faculty,
            //        x.year,
            //        x.curriculum,
            //        condition = x.isDirected ? "DIRIGIDO"
            //        : ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES.ContainsKey(x.@try) ? ConstantHelpers.Student.COURSE_ATTEMPTS.NAMES[x.@try] : "REGULAR"
            //    }).ToList();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExpelledStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                default:
                    //orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
            }

            var query = _context.Students
            .Where(x => x.Status == ConstantHelpers.Student.States.EXPELLED).AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

                if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);
            }

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    lastTerm = x.AcademicSummaries.OrderByDescending(y => y.Term.Name).Select(y => y.Term.Name).FirstOrDefault(),
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    year = x.CurrentAcademicYear
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

        public async Task<List<SacntionedExcelTemplate>> GetStudentToExcelExpelled(Guid? faculty = null, Guid? career = null, string search = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                        .Where(x => x.Status == ConstantHelpers.Student.States.EXPELLED).AsQueryable();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (faculty.HasValue && faculty != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.FacultyId == faculty);

                    if (career.HasValue && career != Guid.Empty)
                        qryCareers = qryCareers.Where(x => x.Id == career);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

                if (career != Guid.Empty) query = query.Where(x => x.CareerId == career);
            }

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var students = await query
                .Select(x => new SacntionedExcelTemplate
                {
                    LastTerm = x.AcademicSummaries.OrderByDescending(y => y.Term.Name).Select(y => y.Term.Name).FirstOrDefault(),
                    Code = x.User.UserName,
                    Name = x.User.FullName,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    Year = x.CurrentAcademicYear
                }).ToListAsync();

            return students;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaStudentsDatatable(DataTablesStructs.SentParameters sentParameters, bool isChekedAll, string searchValue = null, Guid? careerId = null, Guid? facultyId = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserId;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.UserId;
                    break;
            }

            var query = _context.Students
                .Where(x => x.Status != ConstantHelpers.Student.States.BACHELOR
                || x.Status != ConstantHelpers.Student.States.GRADUATED
                || x.Status != ConstantHelpers.Student.States.QUALIFIED
                || x.Status != ConstantHelpers.Student.States.EXPELLED
                || x.Status != ConstantHelpers.Student.States.RETIRED
                || x.Status != ConstantHelpers.Student.States.NOENROLLMENT
                || x.Status != ConstantHelpers.Student.States.DESERTION
                ).AsNoTracking();

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue) || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper()) || x.User.UserName.ToUpper().Contains(searchValue.ToUpper()));
            }
            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            //Usuarios que esten en la tabla de UserCafeteriaServiceTerms
            var cafeteriaServiceTermId = await _context.CafeteriaServiceTerms.Where(x => (x.DateBegin <= DateTime.UtcNow && x.DateEnd > DateTime.UtcNow)).Select(x => x.Id).FirstOrDefaultAsync();
            var userCafeteriaServiceTerm = await _context.UserCafeteriaServiceTerms.Where(x => x.CafeteriaServiceTermId == cafeteriaServiceTermId).Select(x => x.StudentId).ToArrayAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    x.User.Email,
                    Career = x.Career.Name,
                    hasService = (isChekedAll) ? true : userCafeteriaServiceTerm.Contains(x.Id)
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

        public async Task<List<Student>> GetAllStudentsWithFacultyAndTerm(Guid facultyId, Guid careerId, Guid? termId = null)
        {
            var admittedList = await _context.Students
            .Include(x => x.User)
            .Include(x => x.AdmissionType)
            .Where(x => x.CareerId == careerId && x.Career.Faculty.Id == facultyId && x.AdmissionTermId == termId).ToListAsync();

            return admittedList;
        }
        public IQueryable<Student> GetStudentOrdered(int status, Guid careerId)
        {
            var qry = _context.Students
            .Include(s => s.User)
            .Where(s => s.CurrentAcademicYear == 1 && s.Status == status && s.CareerId == careerId);

            return qry;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetQualifiedStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            var recordsFiltered = await query
                .Select(x => x.CareerId)
                .Distinct()
                .CountAsync();

            var data = await query
                .GroupBy(x => new { Career = x.Career.Name, x.CareerId })
                .Select(x => new
                {
                    x.Key.Career,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Career)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<object> GetQualifiedStudentsChart(Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var students = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED)
                .AsQueryable();

            var careersQuery = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    students = students.Where(x => x.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
                students = students.Where(x => x.CareerId == careerId);

            var data = await careersQuery
                .Select(x => new
                {
                    Career = x.Name,
                    Count = students.Where(y => y.CareerId == x.Id).Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Career)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Career).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetBachelorStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            var recordsFiltered = await query
                .Select(x => x.CareerId)
                .Distinct()
                .CountAsync();

            var data = await query
                .GroupBy(x => new { Career = x.Career.Name, x.CareerId })
                .Select(x => new
                {
                    x.Key.Career,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Career)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<object> GetBachelorStudentsChart(Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var students = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR)
                .AsQueryable();

            var careersQuery = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    students = students.Where(x => x.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
                students = students.Where(x => x.CareerId == careerId);

            var data = await careersQuery
                .Select(x => new
                {
                    Career = x.Name,
                    Count = students.Where(y => y.CareerId == x.Id).Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Career)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Career).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrolledStudent(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, int year, int? type = null, Guid? academicProgramId = null, int? cycle = null, Guid? campusId = null, string search = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "4":
                    if (term.Status == ConstantHelpers.TERM_STATES.FINISHED) orderByPredicate = (x) => !x.AcademicSummaries.Where(y => y.TermId == termId).Any() ? x.CurrentAcademicYear : x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear);
                    else orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                case "5":
                    orderByPredicate = (x) => x.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits);
                    break;
                default:
                    break;
            }

            var query = _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId
                && ss.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN))
                .AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.SECRETARY_OF_PROFESSIONAL_SCHOOLS_ADMINISTRATIONS))
            {
                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                     .AsNoTracking();

                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || User.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId || x.Faculty.AdministrativeAssistantId == userId).AsNoTracking();


                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

                if (careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (type.HasValue && type != -1) query = query.Where(x => x.Status == type);

            if (academicProgramId.HasValue) query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (cycle.HasValue)
            {
                if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
                    query = query.Where(x => x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) == cycle || (!x.AcademicSummaries.Where(y => y.TermId == termId).Any() && x.CurrentAcademicYear == cycle));
                else
                    query = query.Where(x => x.CurrentAcademicYear == cycle);
            }

            if (campusId.HasValue && campusId != Guid.Empty) query = query.Where(x => x.CampusId == campusId);

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper()) || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    faculty = s.Career.Faculty.Name,
                    academicYear = term.Status == ConstantHelpers.TERM_STATES.FINISHED ?
                        s.AcademicSummaries.Where(y => y.TermId == termId).Any() ? s.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) : s.CurrentAcademicYear
                        : s.CurrentAcademicYear,
                    credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    id = s.Id,
                    curriculum = s.Curriculum.Code
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,// recordsTotal,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPostPaymentEnrolledStudentDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? year = null, int status = 1, string search = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Curriculum.Code;
                    break;
                case "4":
                    if (term.Status == ConstantHelpers.TERM_STATES.FINISHED) orderByPredicate = (x) => !x.AcademicSummaries.Where(y => y.TermId == termId).Any() ? x.CurrentAcademicYear : x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear);
                    else orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                case "5":
                    orderByPredicate = (x) => x.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits);
                    break;
                case "6":
                    orderByPredicate = (x) => x.User.Payments.Where(y => y.TermId == term.Id
                    && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                    || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue);
                    break;
                default:
                    break;
            }

            //var payments = _context.Payments
            //    .Where(x => x.TermId == term.Id && (x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT))
            //    .Select(x => new
            //    {
            //        x.UserId,
            //        x.Status,
            //        x.PaymentDate
            //    }).ToListAsync();

            var query = _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.SECRETARY_OF_PROFESSIONAL_SCHOOLS_ADMINISTRATIONS))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                     .AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || User.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId || x.Faculty.AdministrativeAssistantId == userId).AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (year.HasValue)
            {
                if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
                    query = query.Where(x => x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) == year || (!x.AcademicSummaries.Where(y => y.TermId == termId).Any() && x.CurrentAcademicYear == year));
                else
                    query = query.Where(x => x.CurrentAcademicYear == year);
            }

            switch (status)
            {
                case 2:
                    query = query.Where(x => x.User.Payments.Where(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                    break;
                case 3:
                    query = query.Where(x => !x.User.Payments.Where(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper()) || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    faculty = s.Career.Faculty.Name,
                    academicYear = term.Status == ConstantHelpers.TERM_STATES.FINISHED ?
                        s.AcademicSummaries.Where(y => y.TermId == termId).Any() ? s.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) : s.CurrentAcademicYear
                        : s.CurrentAcademicYear,
                    credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    id = s.Id,
                    curriculum = s.Curriculum.Code,
                    paid = s.User.Payments.Where(y => y.TermId == term.Id
                    && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                    || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue)
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,// recordsTotal,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrollentTutoring(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, Guid termId, Guid? careerId = null, int? cycle = null, string search = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "4":
                    if (term.Status == ConstantHelpers.TERM_STATES.FINISHED) orderByPredicate = (x) => !x.AcademicSummaries.Where(y => y.TermId == termId).Any() ? x.CurrentAcademicYear : x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear);
                    else orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                case "5":
                    orderByPredicate = (x) => x.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits);
                    break;
                default:
                    break;
            }

            var query = _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.TUTORING_COORDINATOR))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    //var teacher = _context.Teachers.Where(x => x.UserId == userid).FirstOrDefault();

                    //query = query = query.Where(x => x.CareerId == teacher.CareerId);

                    var coordinatorCareerId = await _context.TutoringCoordinators
                        .Where(x => x.UserId == userId)
                        .Select(x => x.CareerId)
                        .ToListAsync();

                    query = query = query.Where(x => coordinatorCareerId.Contains(x.CareerId));

                }
            }


            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            if (cycle.HasValue && cycle != 0)
            {
                if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
                    query = query.Where(x => x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) == cycle || (!x.AcademicSummaries.Where(y => y.TermId == termId).Any() && x.CurrentAcademicYear == cycle));
                else
                    query = query.Where(x => x.CurrentAcademicYear == cycle);
            }

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                    || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    faculty = s.Career.Faculty.Name,
                    academicYear = term.Status == ConstantHelpers.TERM_STATES.FINISHED ?
                        s.AcademicSummaries.Where(y => y.TermId == termId).Any() ? s.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) : s.CurrentAcademicYear
                        : s.CurrentAcademicYear,
                    credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    id = s.Id
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,// recordsTotal,
                RecordsTotal = recordsFiltered
            };
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrolledExtraCreditStudent(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, int year, int? type = null, Guid? academicProgramId = null, int? cycle = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                case "5":
                    orderByPredicate = (x) => x.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits);
                    break;
                default:
                    break;
            }

            var query = _context.Students
                .Where(x => x.EnrollmentTurns.Any(y => y.SpecialEnrollment && y.TermId == termId))
                .AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                     .AsNoTracking();

                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

                if (careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (type.HasValue && type != -1) query = query.Where(x => x.Status == type);

            if (academicProgramId.HasValue) query = query.Where(x => x.AcademicProgramId == academicProgramId);

            query = query.Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId));

            if (cycle.HasValue) query = query.Where(x => x.CurrentAcademicYear == cycle);

            var recordsFiltered = await query.CountAsync();

            //creditos regulares
            var regularCredits = decimal.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REGULAR_MAXIMUM_CREDITS));
            var queryclient = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    faculty = s.Career.Faculty.Name,
                    academicYear = s.CurrentAcademicYear,
                    id = s.Id,
                    //creditos inscritos
                    credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    limit = s.EnrollmentTurns.FirstOrDefault().CreditsLimit,
                    //creditos extras 
                    //extra = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits) - regularCredits > 0 ?
                    //        s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits) - regularCredits : 0,

                    turns = s.EnrollmentTurns.Select(x => new
                    {
                        firsthistory = x.EnrollmentTurnHistories != null ? x.EnrollmentTurnHistories
            .Select(h => new
            {
                h.CreatedAt,
                h.CreditsLimit
            })
            .OrderBy(h => h.CreatedAt)
            .FirstOrDefault()
            : null,
                    }).FirstOrDefault()
                }).ToListAsync();
            var data = queryclient.Select(s => new
            {
                s.code,
                s.name,
                s.career,
                s.faculty,
                s.academicYear,
                s.id,
                //creditos inscritos
                s.credits,
                s.limit,
                //creditos extras 
                extra = s.turns.firsthistory?.CreditsLimit ?? 0,
            }).ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,// recordsTotal,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<IEnumerable<EnrolledStudentExcelTemplate>> GetEnrolledStudentsExcelReport(ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, int? year = null, int? type = null, Guid? academicProgramId = null, Guid? campusId = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            var query = _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId
                && ss.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN))
                .AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                     .AsNoTracking();

                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId).AsNoTracking();


                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);
                if (careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (type.HasValue && type != -1) query = query.Where(x => x.Status == type);

            if (academicProgramId.HasValue) query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (year.HasValue)
            {
                if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
                    query = query.Where(x => x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) == year || (!x.AcademicSummaries.Where(y => y.TermId == termId).Any() && x.CurrentAcademicYear == year));
                else
                    query = query.Where(x => x.CurrentAcademicYear == year);
            }

            if (campusId.HasValue && campusId != Guid.Empty) query = query.Where(x => x.CampusId == campusId);

            var data = await query
                .Select(s => new EnrolledStudentExcelTemplate
                {
                    Code = s.User.UserName,
                    Name = s.User.FullName,
                    Career = s.Career.Name,
                    Faculty = s.Career.Faculty.Name,
                    AcademicYear = term.Status == ConstantHelpers.TERM_STATES.FINISHED ?
                        s.AcademicSummaries.Where(y => y.TermId == termId).Any() ? s.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) : s.CurrentAcademicYear
                        : s.CurrentAcademicYear,
                    Document = s.User.Document,
                    AcademicProgram = s.AcademicProgram.Name,
                    Id = s.Id,
                    Credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    Sex = s.User.Sex == ConstantHelpers.SEX.MALE ? "M" : "F",
                    //Age = s.User.BirthDate
                    BirthDate = s.User.BirthDate,
                    Department = s.User.DepartmentId.HasValue ? s.User.Department.Name : "",
                    Province = s.User.ProvinceId.HasValue ? s.User.Province.Name : "",
                    District = s.User.DistrictId.HasValue ? s.User.District.Name : "",
                    Curriculum = s.Curriculum.Code
                }).ToListAsync();

            data.ForEach(x =>
            {
                var age = DateTime.UtcNow.Year - x.BirthDate.Year;
                if (x.BirthDate.Date > DateTime.UtcNow.AddYears(-age)) age--;
                x.Age = age;
            });

            return data;
        }
        public async Task<IEnumerable<EnrolledStudentExcelTemplate>> GetEnrolledTutoringStudentsExcelReport(ClaimsPrincipal User, Guid termId, Guid? careerId = null, int? year = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            var query = _context.Students.AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.TUTORING_COORDINATOR))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    //var teacher = _context.Teachers.Where(x => x.UserId == userid).FirstOrDefault();

                    //query = query = query.Where(x => x.CareerId == teacher.CareerId);

                    var coordinatorCareerId = await _context.TutoringCoordinators
                        .Where(x => x.UserId == userId)
                        .Select(x => x.CareerId)
                        .ToListAsync();

                    query = query = query.Where(x => coordinatorCareerId.Contains(x.CareerId));
                }
            }


            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            if (year.HasValue && year != 0)
            {
                if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
                    query = query.Where(x => x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) == year || (!x.AcademicSummaries.Where(y => y.TermId == termId).Any() && x.CurrentAcademicYear == year));
                else
                    query = query.Where(x => x.CurrentAcademicYear == year);
            }

            query = query.Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId));

            var data = await query
                .Select(s => new EnrolledStudentExcelTemplate
                {
                    Code = s.User.UserName,
                    Name = s.User.FullName,
                    Career = s.Career.Name,
                    Faculty = s.Career.Faculty.Name,
                    AcademicYear = s.CurrentAcademicYear,
                    Document = s.User.Document,
                    AcademicProgram = s.AcademicProgram.Name,
                    Id = s.Id,
                    Credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    Sex = ConstantHelpers.SEX.ABREV.ContainsKey(s.User.Sex) ? ConstantHelpers.SEX.ABREV[s.User.Sex] : "-",
                    //Age = s.User.BirthDate
                    BirthDate = s.User.BirthDate,
                    Department = s.User.DepartmentId.HasValue ? s.User.Department.Name : "",
                    Province = s.User.ProvinceId.HasValue ? s.User.Province.Name : "",
                    District = s.User.DistrictId.HasValue ? s.User.District.Name : "",
                }).ToListAsync();

            data.ForEach(x =>
            {
                var age = DateTime.UtcNow.Year - x.BirthDate.Year;
                if (x.BirthDate.Date > DateTime.UtcNow.AddYears(-age)) age--;
                x.Age = age;
            });

            return data;
        }
        public async Task<IEnumerable<StudentSiriesTemplate>> GetStudentsExcelSiriesReport(ClaimsPrincipal user, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? campusId = null, int? year = null)
        {
            var term = new Term();
            if (termId.HasValue && termId != Guid.Empty) term = await _context.Terms.FindAsync(termId);
            else term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            //var previousTerm = await _context.Terms.Where(x => x.Name == "2021-2").FirstOrDefaultAsync();

            var query = _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == term.Id
                && ss.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN))
                //.Where(x => x.GraduationTermId == term.Id)
                .AsNoTracking();

            //var summaryQuery = _context.AcademicSummaries.AsNoTracking();
            var academicHistoryQuery = _context.AcademicHistories
                .Where(x => x.Term.StartDate <= term.StartDate && x.Student.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == term.Id) && x.Approved)
                //.Where(x => x.Term.StartDate <= term.StartDate && x.Student.GraduationTermId == term.Id && x.Approved)
                .AsNoTracking();

            var academicYearCourseQuery = _context.AcademicYearCourses.AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var qryCareers = _context.Careers
                    .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                    .AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);
                var careers = qryCareers.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
                //summaryQuery = summaryQuery.Where(x => careers.Contains(x.CareerId));
                academicHistoryQuery = academicHistoryQuery.Where(x => careers.Contains(x.Student.CareerId));
                academicYearCourseQuery = academicYearCourseQuery.Where(x => careers.Contains(x.Curriculum.CareerId));
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty)
                {
                    query = query.Where(x => x.Career.FacultyId == facultyId);
                    //summaryQuery = summaryQuery.Where(x => x.Career.FacultyId == facultyId);
                    academicHistoryQuery = academicHistoryQuery.Where(x => x.Student.Career.FacultyId == facultyId);
                    academicYearCourseQuery = academicYearCourseQuery.Where(x => x.Curriculum.Career.FacultyId == facultyId);
                }

                if (careerId.HasValue && careerId != Guid.Empty)
                {
                    query = query.Where(x => x.CareerId == careerId);
                    //summaryQuery = summaryQuery.Where(x => x.CareerId == careerId);
                    academicHistoryQuery = academicHistoryQuery.Where(x => x.Student.CareerId == careerId);
                    academicYearCourseQuery = academicYearCourseQuery.Where(x => x.Curriculum.CareerId == careerId);
                }
            }

            if (year.HasValue)
            {
                if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
                    query = query.Where(x => x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) == year || (!x.AcademicSummaries.Where(y => y.TermId == termId).Any() && x.CurrentAcademicYear == year));
                else
                    query = query.Where(x => x.CurrentAcademicYear == year);
            }

            if (campusId.HasValue && campusId != Guid.Empty) query = query.Where(x => x.CampusId == campusId);

            var academicYearCourses = await academicYearCourseQuery
                .Select(x => new
                {
                    x.CurriculumId,
                    x.CourseId
                }).ToListAsync();

            //var academicSummaries = await summaryQuery
            //    .Select(x => new
            //    {
            //        x.StudentId,
            //        x.TermId,
            //        x.Term.Year,
            //        x.Term.Number,
            //        x.WeightedAverageGrade,
            //        x.Term.Name
            //    }).ToListAsync();

            var dbData = await query
                .Select(x => new
                {
                    district = x.User.District.Name,
                    province = x.User.Province.Name,
                    department = x.User.Department.Name,
                    address = x.User.Address,
                    x.User.UserName,
                    x.Id,
                    x.CurriculumId,
                    x.User.DocumentType,
                    x.User.Document,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    x.User.Name,
                    x.User.Sex,
                    x.User.BirthDate,
                    Campus = x.CampusId.HasValue ? x.Campus.Name : "",
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    AdmissionTerm = x.AdmissionTerm.Name,
                    x.User.Email,
                    x.User.PersonalEmail,
                    x.User.PhoneNumber,
                    FirstEnrollmentTerm = x.FirstEnrollmentTerm.Name,
                    AverageGrade = x.AcademicSummaries.Any(y => y.TermId == term.Id) ?
                    x.AcademicSummaries.Where(y => y.TermId == term.Id).Select(y => y.WeightedAverageGrade).FirstOrDefault()
                    : x.AcademicSummaries.OrderByDescending(y => y.Term.StartDate).Select(y => y.WeightedAverageGrade).FirstOrDefault(),

                    AverageGradeCumulative = x.AcademicSummaries.Any(y => y.TermId == term.Id) ?
                    x.AcademicSummaries.Where(y => y.TermId == term.Id).Select(y => y.WeightedAverageCumulative).FirstOrDefault()
                    : x.AcademicSummaries.OrderByDescending(y => y.Term.StartDate).Select(y => y.WeightedAverageCumulative).FirstOrDefault(),

                    //PreviousGrade = x.AcademicSummaries.Where(x=>x.TermId == previousTerm.Id).Select(y => y.WeightedAverageGrade).FirstOrDefault(),

                    //AverageGradeCumulative = x.AcademicSummaries.Where(y => y.CurriculumId == x.CurriculumId && y.Term.StartDate <= term.StartDate).OrderByDescending(y => y.Term.StartDate).Select(y => y.WeightedAverageCumulative).FirstOrDefault(),
                    AcademicProgram = x.AcademicProgramId.HasValue ? x.AcademicProgram.Name : "",
                    HasDisability = x.AdmissionType.HasDisability,
                    AdmissionType = x.AdmissionType.Name,
                    AcademicYear = x.AcademicSummaries.Any(y => y.TermId == term.Id) ? x.AcademicSummaries.Where(y => y.TermId == term.Id).Select(y => y.StudentAcademicYear).FirstOrDefault() : x.CurrentAcademicYear,
                    RacialIdentity = x.RacialIdentity == 0 ? ConstantHelpers.Student.RacialIdentity.OTHER : x.RacialIdentity
                }).ToListAsync();

            var enrollmentTurns = await _context.EnrollmentTurns
                .Where(x => x.TermId == term.Id && x.ConfirmationDate.HasValue)
                .Select(x => new
                {
                    x.StudentId,
                    x.ConfirmationDate
                }).ToListAsync();

            var adminEnrollments = await _context.AdminEnrollments
                .Where(x => x.TermId == term.Id && x.CreatedAt.HasValue)
                .Select(x => new
                {
                    x.StudentId,
                    x.CreatedAt
                }).ToListAsync();

            var academicHistories = await academicHistoryQuery
                .Select(x => new
                {
                    x.CourseId,
                    x.StudentId,
                    x.Course.Credits
                }).ToListAsync();

            var result = new List<StudentSiriesTemplate>();

            result = dbData
                .Select(item => new StudentSiriesTemplate
                {
                    UserName = item.UserName,
                    DocumentType = ConstantHelpers.DOCUMENT_TYPES.VALUES[item.DocumentType],
                    Document = item.Document,
                    PaternalSurname = item.PaternalSurname,
                    MaternalSurname = item.MaternalSurname,
                    Name = item.Name,
                    Sex = item.Sex == ConstantHelpers.SEX.MALE ? "MASCULINO" : "FEMENINO",
                    //BirthDate = item.BirthDate,
                    BirthDate = $"{item.BirthDate.ToDefaultTimeZone():dd/MM/yyyy}",
                    Department = item.department,
                    District = item.district,
                    Province = item.province,
                    Address = item.address,
                    EnrolledTerm = term.Name,
                    Campus = item.Campus,
                    Career = item.Career,
                    AverageGrade = /*term.Status == ConstantHelpers.TERM_STATES.ACTIVE ? 0.00M : */
                    item.AverageGrade == 0 ? 0.00M : item.AverageGrade,
                    AdmissionTerm = item.AdmissionTerm,
                    FirstTerm = item.FirstEnrollmentTerm,
                    InstitutionalEmail = string.IsNullOrEmpty(item.Email) ? "" : item.Email,
                    PersonalEmail = string.IsNullOrEmpty(item.PersonalEmail) ? "" : item.PersonalEmail,
                    Phone = item.PhoneNumber,
                    Cellphone = item.PhoneNumber,
                    TotalCredits = academicHistories
                    .Where(x => x.StudentId == item.Id && academicYearCourses.Where(y => y.CurriculumId == item.CurriculumId).Any(y => y.CourseId == x.CourseId))
                    .Sum(x => x.Credits),
                    HasDisability = item.HasDisability ? "SI" : "NO",
                    Disability = item.HasDisability ? item.AdmissionType : "",
                    AcademicProgram = item.AcademicProgram,
                    AverageGradeCumulative = item.AverageGradeCumulative == 0 ? 0.00M : item.AverageGradeCumulative,
                    EnrollmentDate = enrollmentTurns.Any(x => x.StudentId == item.Id) ? enrollmentTurns.FirstOrDefault(x => x.StudentId == item.Id).ConfirmationDate.ToLocalDateFormat()
                    : adminEnrollments.Any(x => x.StudentId == item.Id) ? adminEnrollments.OrderBy(x => x.CreatedAt).FirstOrDefault(x => x.StudentId == item.Id).CreatedAt.ToLocalDateFormat()
                    : "",
                    AcademicYear = item.AcademicYear,
                    Faculty = item.Faculty,
                    RacialIdentity = ConstantHelpers.Student.RacialIdentity.VALUES[item.RacialIdentity],
                })
                .ToList();

            //foreach (var item in dbData)
            //{
            //    //var studentSummaries = academicSummaries
            //    //    .Where(x => x.StudentId == item.Id)
            //    //    .ToList();

            //    //var averageGrade = studentSummaries
            //    //    .Where(y => y.TermId == term.Id)
            //    //    .Select(y => y.WeightedAverageGrade)
            //    //    .FirstOrDefault();

            //    //var firstTerm = studentSummaries
            //    //    .OrderBy(y => y.Year).ThenBy(y => y.Number)
            //    //    .Select(y => y.Name).FirstOrDefault();

            //    var curriculumCourses = academicYearCourses
            //        .Where(x => x.CurriculumId == item.CurriculumId)
            //        .Select(x => x.CourseId)
            //        .ToHashSet();

            //    var totalCredits = academicHistories
            //        .Where(x => x.StudentId == item.Id && curriculumCourses.Contains(x.CourseId))
            //        .Sum(x => x.Credits);

            //    var enrollmentDate = "";
            //    if (enrollmentTurns.Any(x => x.StudentId == item.Id))
            //    {
            //        var date = enrollmentTurns.FirstOrDefault(x => x.StudentId == item.Id);
            //        enrollmentDate = $"{date.ConfirmationDate.ToDefaultTimeZone():dd/MM/yyyy}";
            //    }
            //    else if (adminEnrollments.Any(x => x.StudentId == item.Id))
            //    {
            //        var date = adminEnrollments.OrderBy(x => x.CreatedAt).FirstOrDefault(x => x.StudentId == item.Id);
            //        enrollmentDate = $"{date.CreatedAt.ToDefaultTimeZone():dd/MM/yyyy}";
            //    }

            //    result.Add(new StudentSiriesTemplate
            //    {
            //        UserName = item.UserName,
            //        DocumentType = ConstantHelpers.DOCUMENT_TYPES.VALUES[item.DocumentType],
            //        Document = item.Document,
            //        PaternalSurname = item.PaternalSurname,
            //        MaternalSurname = item.MaternalSurname,
            //        Name = item.Name,
            //        Sex = item.Sex == ConstantHelpers.SEX.MALE ? "MASCULINO" : "FEMENINO",
            //        //BirthDate = item.BirthDate,
            //        BirthDate = $"{item.BirthDate.ToDefaultTimeZone():dd/MM/yyyy}",
            //        Department = item.department,
            //        District = item.district,
            //        Province = item.province,
            //        Address = item.address,
            //        EnrolledTerm = term.Name,
            //        Campus = item.Campus,
            //        Career = item.Career,
            //        AverageGrade = /*term.Status == ConstantHelpers.TERM_STATES.ACTIVE ? 0.00M : */
            //        item.AverageGrade == 0 ? 0.00M : item.AverageGrade,
            //        AdmissionTerm = item.AdmissionTerm,
            //        FirstTerm = item.FirstEnrollmentTerm,
            //        InstitutionalEmail = string.IsNullOrEmpty(item.Email) ? "" : item.Email,
            //        PersonalEmail = string.IsNullOrEmpty(item.PersonalEmail) ? "" : item.PersonalEmail,
            //        Phone = item.PhoneNumber,
            //        Cellphone = item.PhoneNumber,
            //        TotalCredits = totalCredits,
            //        HasDisability = item.HasDisability ? "SI" : "NO",
            //        Disability = item.HasDisability ? item.AdmissionType : "",
            //        AcademicProgram = item.AcademicProgram,
            //        AverageGradeCumulative = item.AverageGradeCumulative == 0 ? 0.00M : item.AverageGradeCumulative,
            //        EnrollmentDate = enrollmentDate,
            //        AcademicYear = item.AcademicYear,
            //        Faculty = item.Faculty,
            //        RacialIdentity = ConstantHelpers.Student.RacialIdentity.VALUES[item.RacialIdentity],

            //        //PreviousAverageGrade = item.PreviousGrade
            //    });
            //}

            return result.OrderBy(x => x.PaternalSurname).ThenBy(x => x.MaternalSurname).ThenBy(x => x.Name).ToList();
        }

        public async Task<IEnumerable<StudentSuneduTemplate>> GetStudentsSuneduReport(ClaimsPrincipal user, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? campusId = null, int? year = null)
        {
            var term = new Term();
            if (termId.HasValue && termId != Guid.Empty) term = await _context.Terms.FindAsync(termId);
            else term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var query = _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == term.Id
                && ss.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN))
                .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var qryCareers = _context.Careers
                    .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                    .AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);
                var careers = qryCareers.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty)
                {
                    query = query.Where(x => x.Career.FacultyId == facultyId);
                }

                if (careerId.HasValue && careerId != Guid.Empty)
                {
                    query = query.Where(x => x.CareerId == careerId);
                }
            }

            if (year.HasValue)
            {
                if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
                    query = query.Where(x => x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) == year || (!x.AcademicSummaries.Where(y => y.TermId == termId).Any() && x.CurrentAcademicYear == year));
                else
                    query = query.Where(x => x.CurrentAcademicYear == year);
            }

            if (campusId.HasValue && campusId != Guid.Empty) query = query.Where(x => x.CampusId == campusId);

            var dbData = await query
                .Select(x => new
                {
                    x.Id,
                    CampusCode = x.CampusId.HasValue ? x.Campus.Code : "",
                    FacultyCode = x.Career.Faculty.SuneduCode,
                    AcademicProgramCode = x.AcademicProgramId.HasValue ? x.AcademicProgram.SuneduCode : "",
                    x.User.DocumentType,
                    x.User.Document,
                    x.User.UserName,
                    AcademicYear = x.AcademicSummaries.Any(y => y.TermId == term.Id) ? x.AcademicSummaries.Where(y => y.TermId == term.Id).Select(y => y.StudentAcademicYear).FirstOrDefault() : x.CurrentAcademicYear,
                }).ToListAsync();

            var result = new List<StudentSuneduTemplate>();

            var enrollmentTurns = await _context.EnrollmentTurns
                .Where(x => x.TermId == term.Id && x.ConfirmationDate.HasValue)
                .Select(x => new
                {
                    x.StudentId,
                    x.ConfirmationDate
                }).ToListAsync();

            var adminEnrollments = await _context.AdminEnrollments
                .Where(x => x.TermId == term.Id && x.CreatedAt.HasValue)
                .Select(x => new
                {
                    x.StudentId,
                    x.CreatedAt
                }).ToListAsync();

            foreach (var item in dbData)
            {
                var enrollmentDate = "";
                var date = (DateTime?)null;

                if (enrollmentTurns.Any(x => x.StudentId == item.Id))
                {
                    var turn = enrollmentTurns.FirstOrDefault(x => x.StudentId == item.Id);
                    date = turn.ConfirmationDate.Value;
                }

                if (adminEnrollments.Any(x => x.StudentId == item.Id))
                {
                    var adminEnrollment = adminEnrollments.OrderBy(x => x.CreatedAt).FirstOrDefault(x => x.StudentId == item.Id);
                    if (adminEnrollment.CreatedAt.HasValue && (date == null || date > adminEnrollment.CreatedAt))
                        date = adminEnrollment.CreatedAt.Value;
                }

                if (date.HasValue) enrollmentDate = $"{date.Value:dd/MM/yyyy}";

                var type = 1;
                switch (item.DocumentType)
                {
                    case ConstantHelpers.DOCUMENT_TYPES.PASSPORT: type = 2; break;
                    case ConstantHelpers.DOCUMENT_TYPES.FOREIGN_RESIDENT_IDENTIFICATION_CARD: type = 3; break;
                    case ConstantHelpers.DOCUMENT_TYPES.PROVISIONAL_DOCUMENT: type = 6; break;
                    case ConstantHelpers.DOCUMENT_TYPES.IDENTITY_CARD: type = 7; break;
                    default:
                        break;
                }

                result.Add(new StudentSuneduTemplate
                {
                    UserName = item.UserName,
                    DocumentType = type,
                    Document = item.Document,
                    EnrollmentDate = enrollmentDate,
                    AcademicYear = item.AcademicYear,
                    FacultyCode = item.FacultyCode,
                    AcademicProgramCode = item.AcademicProgramCode,
                    CampusCode = item.CampusCode,
                    Term = term.Name
                });
            }

            return result.OrderBy(x => x.FacultyCode).ThenBy(x => x.AcademicProgramCode).ThenBy(x => x.UserName).ToList();
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDatatableEnrolledBySectionsStudent(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid academicProgramId, Guid curriculumId, Guid courseId, Guid sectionId)
        {
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Section.CourseTerm.Course.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Section.Code;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Student.CurrentAcademicYear;
                    break;
            }

            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == termId).AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                query = query.Where(x => x.Student.Career.AcademicCoordinatorId == userId || x.Student.Career.CareerDirectorId == userId || x.Student.Career.AcademicSecretaryId == userId);
            }

            if (facultyId != Guid.Empty)
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            if (careerId != Guid.Empty)
                query = query.Where(x => x.Student.CareerId == careerId);

            if (academicProgramId != Guid.Empty)
                query = query.Where(x => x.Student.AcademicProgramId == academicProgramId);

            if (sectionId != Guid.Empty)
                query = query.Where(x => x.SectionId == sectionId);

            if (courseId != Guid.Empty)
                query = query.Where(x => x.Section.CourseTerm.CourseId == courseId);

            if (curriculumId != Guid.Empty)
                query = query.Where(x => x.Student.AcademicProgram.AcademicProgramCurriculums.Any(y => y.CurriculumId == curriculumId));

            int recordsFiltered = await query.CountAsync();

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
                    course = $"{x.Section.CourseTerm.Course.Code} - {x.Section.CourseTerm.Course.Name}",
                    section = x.Section.Code
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

        public IQueryable<Student> GetStudentIncludeInformationGeo(string userId)
        {
            var query = _context.Students.Include(x => x.StudentInformation.OriginDistrict.Province.Department).Include(x => x.User.District.Province.Department).Include(x => x.Career).Where(x => x.UserId == userId).AsQueryable();

            return query;
        }

        public async Task<Student> GetStudentIncludeInformationUser(string studentId)
        {
            var student = await _context.Students.Where(x => x.UserId == studentId).Include(x => x.StudentInformation).Include(x => x.User).FirstOrDefaultAsync();

            return student;
        }

        public IQueryable<Student> GetStudentIncludeInformationGeoCareer(string userId)
        {
            var query = _context.Students.Include(x => x.StudentInformation.OriginDistrict.Province.Department)
                .Include(x => x.User.District.Province.Department)
                .Include(x => x.Career)
                .Where(x => x.UserId == userId).AsQueryable();

            return query;
        }

        public async Task<Student> GetStudentIncludeInformation(string studentId)
        {
            var student = await _context.Students.Where(x => x.UserId == studentId).Include(x => x.StudentInformation).FirstOrDefaultAsync();
            return student;
        }

        public async Task<Student> GetStudentIncludeInformationGeoCareer2(Guid StudentId)
        {
            var student = await _context.Students
                .Include(x => x.StudentInformation.OriginDistrict.Province.Department)
                .Include(x => x.StudentInformation.PlaceOriginDistrict.Province.Department)
                .Include(x => x.User.District.Province.Department).Include(x => x.Career.Faculty)
                .Include(x => x.AdmissionType)
                .Where(x => x.Id == StudentId).FirstOrDefaultAsync();
            return student;
        }

        public async Task<object> GetAllStudentWithData(Guid termId)
        {
            var result = await _context.Students
                .Where(s => s.Status == CORE.Helpers.ConstantHelpers.Student.States.ENTRANT)
                //.Where(s => s.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == term.Id))
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    faculty = s.Career.Faculty.Name,
                    academicYear = s.CurrentAcademicYear,
                    credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    id = s.Id
                }).ToListAsync();

            return result;
        }

        public async Task<object> GetEntrantStudentsEnrolledDatatable(Guid careerId, Guid termId, string search)
        {
            var query = _context.Students
                .Where(s => s.CurrentAcademicYear == 1
                && s.Status == CORE.Helpers.ConstantHelpers.Student.States.ENTRANT
                && s.CareerId == careerId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper()) || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            var result = await query
                .OrderBy(s => s.User.FullName)
                .Select(s => new
                {
                    id = s.Id,
                    code = s.User.UserName,
                    name = s.User.FullName,
                    curriculum = s.Curriculum.Code,
                    progress = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Count() * 1.0M / s.Curriculum.AcademicYearCourses.Where(ayc => ayc.AcademicYear == 1).Count()
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Student>> GetAllStudentWithDataIn(Guid id)
        {
            var result = await _context.Students
                .Where(s => s.Id == id)
                .Include(x => x.Career).ThenInclude(x => x.Faculty)
                .Include(x => x.AcademicSummaries)
                .Include(x => x.StudentInformation)
                .Include(x => x.User).ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentByStatusListDatatable(DataTablesStructs.SentParameters sentParameters, IEnumerable<int> status, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                .Where(x => status.Any(y => y == x.Status))
                .AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId.HasValue)
                query = query.Where(x => x.GraduationTermId == termId);


            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new StudentFilterTemplate
                {
                    Id = x.Id,
                    UserName = x.User.UserName,
                    FullName = x.User.FullName,
                    Dni = x.User.Dni,
                    Email = x.User.Email,
                    Career = x.Career.Name,
                    GraduationTerm = x.GraduationTermId == null ? "" : x.GraduationTerm.Name,
                    Status = x.Status
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            if (status.Any(y => y == ConstantHelpers.Student.States.RESIGNATION))
            {
                var studentsId = data.Select(x => x.Id).ToHashSet();
                var studentObservations = await _context.StudentObservations.Where(x => studentsId.Contains(x.StudentId) && x.Type == ConstantHelpers.OBSERVATION_TYPES.RESIGNATION).ToListAsync();

                foreach (var item in data)
                {
                    item.ResignationDateTime = studentObservations.Where(y => y.StudentId == item.Id).Select(y => y.CreatedAt.ToLocalDateTimeFormat()).FirstOrDefault();
                }
            }

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetGraduatedAndBachelorStudentPieChart(Guid? termId = null, ClaimsPrincipal user = null)
        {
            var students = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED ||
                x.Status == ConstantHelpers.Student.States.BACHELOR)
                .AsQueryable();


            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    students = students.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId.HasValue)
                students = students.Where(x => x.GraduationTermId == termId);

            var graduateds = await students.CountAsync(x => x.Status == ConstantHelpers.Student.States.GRADUATED);
            var bachelors = await students.CountAsync(x => x.Status == ConstantHelpers.Student.States.BACHELOR);

            var data = new List<dynamic>
            {
                new { name = "Egresado"  , y = graduateds},
                new { name = "Bachiller"  , y = bachelors},
            };

            return new { data };
        }

        public async Task<object> GetBachelorAndQualifiedStudentPieChart(Guid? termId = null, ClaimsPrincipal user = null)
        {
            var students = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR ||
                x.Status == ConstantHelpers.Student.States.QUALIFIED)
                .AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    students = students.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId.HasValue)
                students = students.Where(x => x.GraduationTermId == termId);

            var qualifieds = await students.CountAsync(x => x.Status == ConstantHelpers.Student.States.QUALIFIED);
            var bachelors = await students.CountAsync(x => x.Status == ConstantHelpers.Student.States.BACHELOR);

            var data = new List<dynamic>
            {
                new { name = "Titulado"  , y = qualifieds},
                new { name = "Bachiller"  , y = bachelors},
            };

            return new { data };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetBachillerInTimeDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
        {
            var query = _context.Students.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            //Total Egresados + Bachiller
            var totalStudents = await query.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR || x.Status == ConstantHelpers.Student.States.GRADUATED).CountAsync();

            //Total de Egresados
            var totalGraduated = await query.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED).CountAsync();
            //Total de Bachillers
            var totalBachellors = await query.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR).CountAsync();

            //Total de Bachiller en el tiempo
            var TotalBachellorsInTime = await query
                .Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR && x.GraduationTermId != null && x.GraduationTerm.EndDate.AddYears(1).Year <= x.GraduationTerm.Year)
                .CountAsync();

            //Total de Bachiller fuera de tiempo
            var TotalBachellorNotInTime = totalBachellors - TotalBachellorsInTime;

            //Report
            var report = new List<StudentReportTemplate>
            {
                new StudentReportTemplate { DataReport = "Total de Alumnos que Terminaron Estudios (Egresados y Bachilleres)", ValueData = totalStudents },
                new StudentReportTemplate { DataReport = "Total de Alumnos Egresados", ValueData = totalGraduated },
                new StudentReportTemplate { DataReport = "Total de Alumnos con Bachiller", ValueData = totalBachellors},
                new StudentReportTemplate { DataReport = "Con Bachiller en Tiempo (1 año despues de graduarse)", ValueData = TotalBachellorsInTime },
                new StudentReportTemplate { DataReport = "Con Bachiller fuera de Tiempo", ValueData = TotalBachellorNotInTime }
            };

            var recordsFiltered = report.Count();

            var data = report
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }
        //HUMO
        public async Task<object> GetBachillerInTimeChart(ClaimsPrincipal user = null)
        {
            var query = _context.Students.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            //Total de Bachillers
            var totalBachellors = await query.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR).CountAsync();

            //Total de Bachiller en el tiempo
            var TotalBachellorsInTime = await query
                .Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR && x.GraduationTermId != null && x.GraduationTerm.EndDate.AddYears(1).Year <= x.GraduationTerm.Year)
                .CountAsync();

            //Total de Bachiller fuera de tiempo
            var TotalBachellorNotInTime = totalBachellors - TotalBachellorsInTime;

            double CalculatedBachellorInTime = 0.00;
            double CalculatedBachellorNotInTime = 0.00;
            if (totalBachellors > 0)
            {
                CalculatedBachellorInTime = (TotalBachellorsInTime * 100) / (totalBachellors * 1.0);
                CalculatedBachellorNotInTime = 100 - CalculatedBachellorInTime;
            }

            //Report
            var data = new List<StudentReportDoubleTemplate>
            {
                new StudentReportDoubleTemplate { DataReport = "Con Bachiller en Tiempo", ValueData = Math.Round(CalculatedBachellorInTime,2,MidpointRounding.AwayFromZero) },
                new StudentReportDoubleTemplate { DataReport = "Con Bachiller fuera de Tiempo", ValueData = Math.Round(CalculatedBachellorNotInTime,2,MidpointRounding.AwayFromZero) }
            };

            var result = new
            {
                categories = data.Select(x => x.DataReport).ToList(),
                data = data.Select(x => x.ValueData).ToList()
            };

            return result;

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGraduatedStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null)
        {
            //Todos los estudiantes graduados
            var students = _context.Students
                .Where(x => x.GraduationTermId != null)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    students = students.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }


            if (graduationTermId != null)
            {
                students = students.Where(x => x.GraduationTermId == graduationTermId);
            }

            //Usaremos Carreras temporalmente
            if (careerId != null)
            {
                students = students.Where(x => x.CareerId == careerId);
            }

            var recordsFiltered = await students
                .Select(x => new { x.CareerId, x.GraduationTermId })
                .Distinct()
                .CountAsync();

            var data = await students
                    .GroupBy(x => new { x.CareerId, Career = x.Career.Name, x.GraduationTermId, GraduationTerm = x.GraduationTerm.Name })
                    .Select(x => new
                    {
                        x.Key.Career,
                        x.Key.GraduationTerm,
                        Accepted = x.Count()
                    })
                    .OrderByDescending(x => x.Accepted)
                    .ThenBy(x => x.Career)
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
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

        public async Task<object> GetGraduatedStudentChart(Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null)
        {
            //Todos los estudiantes graduados
            var students = _context.Students
                .Where(x => x.GraduationTermId != null)
                .AsQueryable();

            var careerQuerty = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    students = students.Where(x => x.Career.QualityCoordinatorId == userId);
                    careerQuerty = careerQuerty.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            if (graduationTermId != null)
            {
                students = students.Where(x => x.GraduationTermId == graduationTermId);
            }

            //Usaremos Carreras temporalmente
            if (careerId != null)
            {
                students = students.Where(x => x.CareerId == careerId);
            }

            var careers = await careerQuerty
                .Select(x => new
                {
                    Career = x.Name,
                    Accepted = students.Where(y => y.CareerId == x.Id).Count()
                })
                .OrderByDescending(x => x.Accepted)
                .ThenBy(x => x.Career)
                .ToListAsync();

            var result = new
            {
                categories = careers.Select(x => x.Career).ToList(),
                data = careers.Select(x => x.Accepted).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGraduatedStudentInTimeDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null)
        {
            var students = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED && x.GraduationTermId != null)
                .AsNoTracking();

            if (graduationTermId != null)
            {
                students = students.Where(x => x.GraduationTermId == graduationTermId);
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    students = students.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
            {
                students = students.Where(x => x.CareerId == careerId);
            }

            students = students.Where(x => x.AcademicSummaries.Count() == _context.AcademicYearCourses.Where(y => y.CurriculumId == x.CurriculumId).DefaultIfEmpty().Max(y => (int)y.AcademicYear));

            var recordsFiltered = await students
                    .Select(x => new { x.CareerId, x.GraduationTermId })
                    .Distinct().CountAsync();

            var data = await students
                .GroupBy(x => new { x.CareerId, Career = x.Career.Name, x.GraduationTermId, GraduationTerm = x.GraduationTerm.Name })
                .Select(x => new
                {
                    x.Key.Career,
                    x.Key.GraduationTerm,
                    Accepted = x.Count()
                })
                .OrderByDescending(x => x.Accepted)
                .ThenBy(x => x.Career)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<object> GetGraduatedStudentInTimeChart(Guid? careerId = null, Guid? graduationTermId = null, ClaimsPrincipal user = null)
        {

            var students = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED && x.GraduationTermId != null)
                .AsNoTracking();

            var careerQuery = _context.Careers.AsNoTracking();

            if (graduationTermId != null)
            {
                students = students.Where(x => x.GraduationTermId == graduationTermId);
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    students = students.Where(x => x.Career.QualityCoordinatorId == userId);
                    careerQuery = careerQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            if (careerId != null)
            {
                students = students.Where(x => x.CareerId == careerId);
            }

            students = students.Where(x => x.AcademicSummaries.Count() == _context.AcademicYearCourses.Where(y => y.CurriculumId == x.CurriculumId).DefaultIfEmpty().Max(y => (int)y.AcademicYear));

            var data = await careerQuery
                .Select(x => new
                {
                    Career = x.Name,
                    Count = students.Where(y => y.CareerId == x.Id).Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Career)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Career).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<object> GetStudentJson(Guid? sid = null)
        {
            var query = _context.Students
                .Include(x => x.User)
                .Include(x => x.StudentSections)
                .AsQueryable();

            if (sid.HasValue && !sid.Equals(Guid.Empty))
                query = query.Where(s => s.StudentSections.Any(ss => ss.SectionId.Equals(sid)));

            var result = await query
                .Select(s => new
                {
                    id = s.Id,
                    text = s.User.FullName
                }).ToListAsync();

            return result;
        }

        public async Task<EnrollmentInformationTemplate> GetStudentEnrollmentInformation(Guid studentId)
        {
            var model = await _context.Students
                .Where(s => s.Id == studentId)
                .Select(x => new EnrollmentInformationTemplate
                {
                    AdmissionTerm = x.AcademicHistories.OrderBy(y => y.Term.Year).ThenBy(y => y.Term.Number).Select(y => y.Term.Name).FirstOrDefault(),
                    Career = x.Career.Name,
                    CurrentAcademicYear = x.CurrentAcademicYear,
                    CurriculumId = x.CurriculumId,
                    Curriculum = x.Curriculum.Code,
                    Faculty = x.Career.Faculty.Name,
                    FullName = x.User.FullName,
                    UserName = x.User.UserName,
                    Document = x.User.Document,
                    BirthDay = x.User.BirthDate,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    Status = x.Status,
                    CareerId = x.CareerId
                }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<Student> GetStudentWithDataByUserId(string userId)
        {
            var model = await _context.Students
                .Where(s => s.UserId == userId)
                .Include(x => x.User)
                .Include(x => x.Career)
                .FirstOrDefaultAsync();

            return model;
        }
        public async Task<Student> GetStudentByUserName(string username)
        {
            var result = await _context.Students.Where(X => X.User.UserName == username).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Student> GetStudentIdString(string studentId)
        {
            var result = await _context.Students.Where(x => x.UserId == studentId)
                .Include(x => x.User)
                .Include(x => x.StudentInformation)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetNextEnrollmentStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, int? academicYear = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                default:
                    break;
            }

            var query = _context.Students
                .FilterActiveStudents()
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

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (academicYear.HasValue && academicYear != -1) query = query.Where(x => x.CurrentAcademicYear == academicYear);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    faculty = s.Career.Faculty.Name,
                    academicYear = s.CurrentAcademicYear,
                    id = s.Id
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<object> GetStudentAcademicYearsSelectClientSide(Guid? faculty = null, Guid? career = null)
        {
            var query = _context.Students.AsNoTracking();

            if (faculty.HasValue && faculty != Guid.Empty) query = query.Where(x => x.Career.FacultyId == faculty);

            if (career.HasValue && career != Guid.Empty) query = query.Where(x => x.CareerId == career);

            var academicYears = await query
                .GroupBy(x => x.CurrentAcademicYear)
                .Select(x => x.Key).ToListAsync();

            var result = academicYears.Select(x => new
            {
                id = x,
                text = x.ToString()
            })
            .OrderBy(x => x.id)
            .ToList();

            result.Insert(0, new { id = -1, text = "Todos" });

            return result;
        }

        public async Task<int> GetStudentsEnrolledCountByTerm(Guid termId, ClaimsPrincipal user = null)
        {
            var query = _context.Students.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            return await query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId)).CountAsync();
        }

        public async Task<Student> GetStudentByStudentInformationId(Guid studentInformId)
        {
            var result = await _context.Students.Where(X => X.StudentInformationId == studentInformId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDeserterStudentReportDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null)
        {
            //La comparacion se lleva acabo con la matricula actual y la matricula anterior
            var prevRegularTerm = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2"))
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number)
                .Select(x => new { x.Id, x.Name })
                .FirstOrDefaultAsync();

            var activeTerm = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                .Select(x => new { x.Id, x.Name })
                .FirstOrDefaultAsync();

            var percentTotalDesertor = 0.00;
            var totalDesertors = 0;
            var desertorAfterFourTerm = 0;
            var desertorBeforeFourTerm = 0;
            var percentDesertorAfterFourTerm = 0.00;
            var percentDesertorBeforeFourTerm = 0.00;

            if (prevRegularTerm != null && activeTerm != null)
            {
                var query = _context.StudentSections.AsNoTracking();

                if (user != null)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                    {
                        query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                    }
                }

                //estudiantes matriculados en el cicloActual
                var studentsActiveTerm = await query
                            .Where(x => x.Section.CourseTerm.TermId == activeTerm.Id).Select(x => new { x.StudentId, x.Student.CurrentAcademicYear }).Distinct().ToListAsync();

                //estudiantes matriculados en el cicloRegularAnterior
                var studentsPrevRegularTerm = await query
                            .Where(x => x.Section.CourseTerm.TermId == prevRegularTerm.Id).Select(x => new { x.StudentId, x.Student.CurrentAcademicYear }).Distinct().ToListAsync();

                var desertorStudents = studentsPrevRegularTerm.Where(x => !studentsActiveTerm.Any(y => y.StudentId == x.StudentId)).ToList();

                totalDesertors = desertorStudents.Count();
                //Aquellos que desertaron y solo hicieron dos primeros años academicos
                desertorAfterFourTerm = desertorStudents.Where(x => x.CurrentAcademicYear > 4).Count();
                desertorBeforeFourTerm = totalDesertors - desertorAfterFourTerm;

                if (studentsPrevRegularTerm.Count > 0)
                {
                    percentTotalDesertor = (totalDesertors * 100) / (studentsPrevRegularTerm.Count * 1.0);
                    percentDesertorBeforeFourTerm = (desertorBeforeFourTerm * 100) / (studentsPrevRegularTerm.Count * 1.0);
                    percentDesertorAfterFourTerm = (desertorAfterFourTerm * 100) / (studentsPrevRegularTerm.Count * 1.0);
                }
            }

            var studentDesertorReport = new List<StudentDesertorReportTemplate>
            {
                new StudentDesertorReportTemplate { DataReport = "Total Desertores", PercentValueData =  $"{Math.Round(percentTotalDesertor,2,MidpointRounding.AwayFromZero)} %"  , ValueData = totalDesertors },
                new StudentDesertorReportTemplate { DataReport = "Durante los 2 años Academicos", PercentValueData = $"{Math.Round(percentDesertorBeforeFourTerm,2,MidpointRounding.AwayFromZero)} %" , ValueData = desertorBeforeFourTerm },
                new StudentDesertorReportTemplate { DataReport = "Despues de 2 Años Academicos", PercentValueData = $"{Math.Round(percentDesertorAfterFourTerm,2,MidpointRounding.AwayFromZero)} %" , ValueData = desertorAfterFourTerm }
            };

            var recordsFiltered = studentDesertorReport.Count();


            var data = studentDesertorReport
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetDeserterStudentReportChart(ClaimsPrincipal user = null)
        {
            //La comparacion se lleva acabo con la matricula actual y la matricula anterior
            var prevRegularTerm = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2"))
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number)
                .Select(x => new { x.Id, x.Name })
                .FirstOrDefaultAsync();

            var activeTerm = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                .Select(x => new { x.Id, x.Name })
                .FirstOrDefaultAsync();

            var totalDesertors = 0;
            var desertorAfterFourTerm = 0;
            var desertorBeforeFourTerm = 0;
            var percentDesertorAfterFourTerm = 0.00;
            var percentDesertorBeforeFourTerm = 0.00;

            if (prevRegularTerm != null && activeTerm != null)
            {

                var query = _context.StudentSections.AsNoTracking();

                if (user != null)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                    {
                        query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                    }
                }

                //estudiantes matriculados en el cicloActual
                var studentsActiveTerm = await query
                            .Where(x => x.Section.CourseTerm.TermId == activeTerm.Id).Select(x => new { x.StudentId, x.Student.CurrentAcademicYear }).Distinct().ToListAsync();

                //estudiantes matriculados en el cicloRegularAnterior
                var studentsPrevRegularTerm = await query
                            .Where(x => x.Section.CourseTerm.TermId == prevRegularTerm.Id).Select(x => new { x.StudentId, x.Student.CurrentAcademicYear }).Distinct().ToListAsync();

                var desertorStudents = studentsPrevRegularTerm.Where(x => !studentsActiveTerm.Any(y => y.StudentId == x.StudentId)).ToList();

                totalDesertors = desertorStudents.Count();
                //Aquellos que desertaron y solo hicieron dos primeros años academicos
                desertorAfterFourTerm = desertorStudents.Where(x => x.CurrentAcademicYear > 4).Count();
                desertorBeforeFourTerm = totalDesertors - desertorAfterFourTerm;

                if (studentsPrevRegularTerm.Count > 0)
                {
                    percentDesertorBeforeFourTerm = (desertorBeforeFourTerm * 100) / (studentsPrevRegularTerm.Count * 1.0);
                    percentDesertorAfterFourTerm = (desertorAfterFourTerm * 100) / (studentsPrevRegularTerm.Count * 1.0);
                }
            }

            var studentDesertorReport = new List<StudentDesertorReportTemplate>
            {
                new StudentDesertorReportTemplate { DataReport = "Durante los 2 años Academicos", PercentValueData = $"{Math.Round(percentDesertorBeforeFourTerm,2,MidpointRounding.AwayFromZero)} %" , ValueData = desertorBeforeFourTerm },
                new StudentDesertorReportTemplate { DataReport = "Despues de 2 Años Academicos", PercentValueData = $"{Math.Round(percentDesertorAfterFourTerm,2,MidpointRounding.AwayFromZero)} %" , ValueData = desertorAfterFourTerm }
            };


            var result = new
            {
                categories = studentDesertorReport.Select(x => x.DataReport).ToList(),
                data = studentDesertorReport.Select(x => x.ValueData).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTransferStudentDataTable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid? faculty = null, Guid? career = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.User.UserName;
                    break;
            }


            var query = _context.Students
                .Where(x => x.Status == CORE.Helpers.ConstantHelpers.Student.States.TRANSFER).AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (User.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR))
            {
                var coordinatorCareer = await _context.Careers.FirstOrDefaultAsync(x => x.AcademicCoordinatorId == userId);
                career = coordinatorCareer != null ? coordinatorCareer.Id : Guid.Empty;

                query = query.Where(x => x.CareerId == career);
            }
            else
            {
                if (faculty != Guid.Empty)
                {
                    query = query.Where(x => x.Career.FacultyId == faculty);
                }

                if (career != Guid.Empty)
                {
                    query = query.Where(x => x.CareerId == career);
                }
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.User.UserName,
                    name = x.User.FullName,
                    faculty = x.Career.Faculty.Name,
                    career = x.Career.Name,
                    admissionType = x.AdmissionType.Name
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

        public async Task<object> GetEnrolledByFacultyAndTermChart(Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections
                           .Where(x => !x.Section.IsDirectedCourse)
                           .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }

            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (facultyId != null) query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            var data = await _context.Faculties
                .Select(x => new
                {
                    Career = x.Name,
                    Count = query.Where(y => y.Student.Career.FacultyId == x.Id).Select(y => new { y.StudentId, y.Section.CourseTerm.TermId }).Distinct().Count()
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Career)
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Career).ToList(),
                data = data.Select(x => x.Count).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledByFacultyAndTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? facultyId = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections
                .Where(x => !x.Section.IsDirectedCourse)
                .AsNoTracking();


            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                }
            }


            if (termId != null) query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (facultyId != null) query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            var recordsFiltered = await query
                .Select(x => new { x.Section.CourseTerm.TermId, x.Student.CareerId, x.Student.Career.FacultyId })
                .Distinct()
                .CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.StudentId,
                    x.Section.CourseTerm.TermId,
                    TermYear = x.Section.CourseTerm.Term.Year,
                    TermNumber = x.Section.CourseTerm.Term.Number,
                    Term = x.Section.CourseTerm.Term.Name,
                    x.Student.Career.FacultyId,
                    x.Student.CareerId,
                    Faculty = x.Student.Career.Faculty.Name,
                    Career = x.Student.Career.Name,
                })
                .Distinct()
                .GroupBy(x => new { x.TermId, x.Term, x.FacultyId, x.Faculty, x.CareerId, x.Career, x.TermYear, x.TermNumber })
                .OrderByDescending(x => x.Key.TermYear)
                .ThenByDescending(x => x.Key.TermNumber)
                .ThenBy(x => x.Key.Faculty)
                .ThenBy(x => x.Key.Career)
                .Select(x => new
                {
                    x.Key.Term,
                    x.Key.Faculty,
                    x.Key.Career,
                    Count = x.Count()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetOthersEnrolledCoursesDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId, Guid termId)
        {
            Expression<Func<AcademicHistory, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.StudentId;
                    break;
            }

            var query = _context.AcademicHistories
                .Where(x => x.Type != ConstantHelpers.AcademicHistory.Types.REGULAR && x.Type != ConstantHelpers.AcademicHistory.Types.DIRECTED)
                .Where(x => x.StudentId == studentId && x.TermId == termId)
                //.Where(x => x.Section.StudentSections.Any(y => y.StudentId == studentId) && x.Section.CourseTerm.TermId == termId)
                .AsQueryable();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    type = ConstantHelpers.AcademicHistory.Types.VALUES.ContainsKey(x.Type) ? ConstantHelpers.AcademicHistory.Types.VALUES[x.Type] : "---",
                    course = x.Course.Name,
                    code = x.Section.CourseTerm.Course.Code,
                    note = x.Validated && x.Grade <= 0 ? "CON" : $"{x.Grade}",
                    credits = x.Section.CourseTerm.Course.Credits.ToString("0.0")
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

        public async Task<string> GetFacultyByStudentIncludeCareerUser(string userId)
        {
            var faculty = await _context.Students.Include(y => y.Career.Faculty).Include(y => y.User).Where(y => y.UserId == userId).FirstOrDefaultAsync();

            return faculty.Career.Faculty.Name;
        }

        public async Task<string> GetStudentFullNameIncludeUserByUserId(string userId)
        {
            var result = await _context.Students.Include(y => y.User).Where(y => y.UserId == userId).FirstOrDefaultAsync();
            return result.User.FullName;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrollmentDatatableCon(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.User.UserName;
                    break;
            }


            var query = _context.Students
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = query.Where(x => x.Status == ConstantHelpers.Student.States.IRREGULAR
                || x.Status == ConstantHelpers.Student.States.OBSERVED
                || x.Status == ConstantHelpers.Student.States.QUALIFIED
                || x.Status == ConstantHelpers.Student.States.REGULAR
                || x.Status == ConstantHelpers.Student.States.REPEATER
                || x.Status == ConstantHelpers.Student.States.SANCTIONED
                || x.Status == ConstantHelpers.Student.States.TRANSFER
                || x.Status == ConstantHelpers.Student.States.UNBEATEN).AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    status = x.Status
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

        public async Task<Student> GetByIdWithData(Guid id)
        {
            return await _context.Students
                        .Include(x => x.User)
                        .Include(x => x.Career)
                        .Where(x => x.Id == id)
                        .FirstOrDefaultAsync();
        }

        public async Task<List<Student>> GetStudentWitData()
        {
            var result = await _context.Students.Include(x => x.User).ToListAsync();
            //var result2 = _context.Students.Include(x => x.User);

            return result;
        }

        public async Task<bool> GetStudentToUpdateByCode(string code)
        {
            var result = await _context.Students
                .Where(x => x.User.UserName.ToUpper() == code.ToUpper())
                .Select(x => x.StudentInformation != null)
                .FirstOrDefaultAsync();

            return result;
        }
        public async Task<Student> GetWithData(Guid id)
            => await _context.Students
            .Include(x => x.User)
            .Include(x => x.StudentInformation)
            .Include(x => x.Career)
            .Include(x => x.AdmissionTerm)
            .Include(x => x.GraduationTerm)
            .Include(x => x.Campus)
            .Include(x => x.AcademicProgram)
            .Include(x => x.User.District.Province.Department).Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GraduatedListReportToAcademicRecord(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, int gradeType, Guid careerId, int year)
        {
            var query = _context.Students
                              .Include(x => x.User)
                              .Include(x => x.Career)
                              .Include(x => x.RegistryPatterns)
                              .AsQueryable();

            if (!user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD) && !user.IsInRole(ConstantHelpers.ROLES.REPORT_QUERIES))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(y => y.AcademicDepartment.CareerId).ToArrayAsync();
                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

            if (careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (year > 0)
                query = query.Where(x => x.GraduationTerm.Year == year).AsQueryable();

            switch (gradeType)
            {
                case ConstantHelpers.Student.States.BACHELOR:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR);
                    break;
                case ConstantHelpers.Student.States.GRADUATED:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED);
                    break;
                case ConstantHelpers.Student.States.QUALIFIED:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED);
                    break;
                default:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR ||
                    x.Status == ConstantHelpers.Student.States.GRADUATED || x.Status == ConstantHelpers.Student.States.QUALIFIED);
                    break;
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    sex = x.User.Sex == ConstantHelpers.SEX.MALE ? "M" : "F",
                    practices = x.StudentExperiences.Count == 0 ? "NO" : "SI",
                    number = x.User.PhoneNumber,
                    career = x.Career.Name,
                    email = x.User.Email ?? "-",
                    dni = String.IsNullOrEmpty(x.User.Dni) ? "-" : x.User.Dni,
                    address = String.IsNullOrEmpty(x.User.Address) ? "-" : x.User.Address,
                    grade = (x.Status == ConstantHelpers.Student.States.BACHELOR) ? "Bachiller" : (x.Status == ConstantHelpers.Student.States.QUALIFIED) ? "Titulado" : "Egresado",
                    year = (x.GraduationTermId.HasValue) ? x.GraduationTerm.Year.ToString() : "--"
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
        public async Task<List<SubstituteExamTemplate>> GetStudentsForSubstituteExamDataAsync(Guid termid, Guid sectionId, string search = null)
        {
            var term = await _context.Terms.Where(x => x.Id == termid).FirstOrDefaultAsync();

            var configs = await _context.Configurations.ToDictionaryAsync(x => x.Key, x => x.Value);

            var min_substitute_examen = decimal.Parse(GetConfigurationValue(configs, CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.MIN_SUBSTITUTE_EXAMEN));

            var query = _context.StudentSections.Include(x => x.Section).Include(x => x.Student)
                                    .Where(f => f.SectionId == sectionId &&
                                        f.FinalGrade >= min_substitute_examen &&
                                        f.FinalGrade < f.Section.CourseTerm.Term.MinGrade &&
                                        //f.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS
                                        //&&
                                        f.Section.Code != "EVALUACIÓN EXTRAORDINARIA"
                                        );

            if (term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
            {
                query = query.Where(x => x.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS);
            }
            else
            {
                query = query.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN);
            }

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(search) || x.Student.User.Dni.Contains(search) || x.Student.User.UserName.Contains(search));
            }

            return await query
                .OrderByCondition(ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, (x) => x.Student.User.FullName)
                .Select(x => new SubstituteExamTemplate
                {
                    id = x.StudentId,
                    //code = x.Student.User.UserName,
                    //name = x.Student.User.FullName,
                    StudentId = x.StudentId,
                    //isCheched = substitueexamns.Contains(x.StudentId),
                    score = x.FinalGrade.ToString()
                }).ToListAsync();
        }
        private string GetConfigurationValue(Dictionary<string, string> list, string key)
        {
            return list.ContainsKey(key) ? list[key] :

                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[key] : "";
        }

        public async Task<DataTablesStructs.ReturnedData<SubstituteExamTemplate>> GetStudentsForSubstiteExam(DataTablesStructs.SentParameters sentParameters, Guid termid, Guid sectionId, string search)
        {
            Expression<Func<StudentSection, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = (x) => x.Student.User.PaternalSurname;
                    break;
            }

            var gradeCorrections = await _context.GradeCorrections.Where(x => x.Grade.StudentSection.SectionId == sectionId && x.State == ConstantHelpers.GRADECORRECTION_STATES.PENDING)
                .Select(x => new
                {
                    x.Grade.StudentSection.StudentId,
                    x.GradeId
                })
                .ToListAsync();
            var recoveryExams = await _context.GradeRecoveries.Where(x => x.GradeRecoveryExam.SectionId == sectionId && (x.GradeRecoveryExam.Status == ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.CONFIRMED || x.GradeRecoveryExam.Status == ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.PENDING)).ToListAsync();

            var query = _context.SubstituteExams.Where(x => x.SectionId == sectionId).AsQueryable();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new SubstituteExamTemplate
                {
                    id = x.Id,
                    code = x.Student.User.UserName,
                    StudentId = x.StudentId,
                    name = x.Student.User.FullName,
                    termIsActive = x.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE,
                    isChecked = x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED,
                    score = x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED ? x.TeacherExamScore.HasValue ? x.TeacherExamScore.Value.ToString() : x.ExamScore.Value.ToString() : "Sin calificar",
                    Evaluated = x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED,
                })
                .OrderBy(x => x.name)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            foreach (var item in data)
            {
                item.HasGradeCorrection = gradeCorrections.Any(y => y.StudentId == item.StudentId);
                item.HasGradeRecoveryExam = recoveryExams.Any(y => y.StudentId == item.StudentId);
            }

            var recordsTotal = data.Count;
            return new DataTablesStructs.ReturnedData<SubstituteExamTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGlobalListDatatable(DataTablesStructs.SentParameters sentParameters, string dni = null, string name = null, string userName = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null)
        {

            Expression<Func<Student, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.Dni); break;
                case "1":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.PaternalSurname); break;
                case "3":
                    orderByPredicate = ((x) => x.User.MaternalSurname); break;
                case "4":
                    orderByPredicate = ((x) => x.User.Name); break;
                case "5":
                    orderByPredicate = ((x) => x.Career.Faculty.Name); break;
                case "6":
                    orderByPredicate = ((x) => x.Career.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.AcademicProgram.Name); break;
                case "8":
                    orderByPredicate = ((x) => x.Curriculum.Code); break;
                case "9":
                    orderByPredicate = ((x) => x.Status); break;
                case "10":
                    orderByPredicate = ((x) => x.Status); break;
                case "11":
                    orderByPredicate = ((x) => x.User.Sex); break;
                case "12":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "13":
                    orderByPredicate = ((x) => x.User.PhoneNumber); break;
                case "14":
                    orderByPredicate = ((x) => x.User.Address); break;
                case "15":
                    orderByPredicate = ((x) => x.User.Department.Name); break;
                case "16":
                    orderByPredicate = ((x) => x.User.Province.Name); break;
                case "17":
                    orderByPredicate = ((x) => x.User.District.Name); break;
            }

            var query = _context.Students.AsQueryable();

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (academicProgramId != null)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (!string.IsNullOrEmpty(dni))
                query = query.Where(x => x.User.Dni.ToUpper().Contains(dni.ToUpper()));

            if (!string.IsNullOrEmpty(userName))
                query = query.Where(x => x.User.UserName.ToUpper().Contains(userName.ToUpper()));

            if (termId.HasValue)
            {
                query = query.Where(x => x.GraduationTermId == termId);
            }

            if (!string.IsNullOrEmpty(name))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    name = $"\"{name}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, name));
                }
                else
                    query = query.Where(x => x.User.Name.ToUpper().Contains(name.ToUpper())
                                    || x.User.PaternalSurname.ToUpper().Contains(name.ToUpper())
                                    || x.User.MaternalSurname.ToUpper().Contains(name.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            //var reservations = _context.EnrollmentReservations.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    dni = x.User.Dni ?? "-",
                    code = x.User.UserName ?? "-",
                    paternalName = x.User.PaternalSurname ?? "-",
                    maternalName = x.User.MaternalSurname ?? "-",
                    name = x.User.Name ?? "-",
                    sex = ConstantHelpers.SEX.ABREV.ContainsKey(x.User.Sex)
                            ? ConstantHelpers.SEX.ABREV[x.User.Sex] : "-",
                    email = x.User.Email ?? "-",
                    phoneNumber = x.User.PhoneNumber ?? "-",
                    address = x.User.Address ?? "-",
                    department = x.User.Department.Name,
                    province = x.User.Province.Name,
                    district = x.User.District.Name,
                    campus = x.Campus.Name ?? "",
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    academicProgram = x.AcademicProgram.Name,
                    curriculum = x.Curriculum.Code,
                    x.Status,
                    admisionTerm = x.AdmissionTerm.Name,
                    admisionmodality = x.AdmissionType.Name,
                    //firstPeriodic = "",
                    firstPeriodic = x.FirstEnrollmentTermId.HasValue ? x.FirstEnrollmentTerm.Name : "-",
                    graduationTerm = x.GraduationTermId.HasValue ? x.GraduationTerm.Name : "-",
                    average = (x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number)
                                .Select(y => y.WeightedAverageCumulative).FirstOrDefault()).ToString("0.00"),

                    //_context.EnrollmentReservations.Where(y => y.StudentId == x.Id && x.AcademicSummaries.Any(z => y.TermId != z.TermId)).FirstOrDefault().Term.Name,

                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeReportGraduatedDatatable(DataTablesStructs.SentParameters sentParameters, string userName = null, string dni = null, string fullName = null, int? studentState = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null, Guid? graduationTermId = null, int? graduationYear = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.PaternalSurname); break;
                case "2":
                    orderByPredicate = ((x) => x.User.MaternalSurname); break;
                case "3":
                    orderByPredicate = ((x) => x.User.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.User.Dni); break;
                case "5":
                    orderByPredicate = ((x) => x.Career.Faculty.Name); break;
                case "6":
                    orderByPredicate = ((x) => x.Career.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.AcademicProgram.Name); break;
                case "8":
                    orderByPredicate = ((x) => x.Curriculum.Code); break;
                case "9":
                    orderByPredicate = ((x) => x.Status); break;
                case "10":
                    orderByPredicate = ((x) => x.Campus.Name); break;
                case "11":
                    orderByPredicate = ((x) => x.AdmissionTerm.Name); break;
                case "12":
                    orderByPredicate = ((x) => x.AdmissionType.Name); break;
                case "13":
                    orderByPredicate = ((x) => x.FirstEnrollmentTerm.Name); break;
                case "14":
                    orderByPredicate = ((x) => x.GraduationTerm.Name); break;
                case "15":
                    orderByPredicate = ((x) => x.GraduationTerm.Name); break;//Nota final de academicsummaries WeightedAverageCumulative
                case "16":
                    orderByPredicate = ((x) => x.User.Sex); break;
                case "17":
                    orderByPredicate = ((x) => x.User.PhoneNumber); break;
                case "18":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "19":
                    orderByPredicate = ((x) => x.User.Address); break;
                case "20":
                    orderByPredicate = ((x) => x.User.Department.Name); break;
                case "21":
                    orderByPredicate = ((x) => x.User.Province.Name); break;
                case "22":
                    orderByPredicate = ((x) => x.User.District.Name); break;
            }

            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED ||
                            x.Status == ConstantHelpers.Student.States.BACHELOR ||
                            x.Status == ConstantHelpers.Student.States.QUALIFIED)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(userName))
            {
                var userNameTrimmed = userName.Trim();
                query = query.Where(x => x.User.UserName.ToUpper().Contains(userNameTrimmed.ToUpper()));
            }

            if (!string.IsNullOrEmpty(dni))
            {
                var dniTrimmed = dni.Trim();
                query = query.Where(x => x.User.Dni.ToUpper().Contains(dniTrimmed.ToUpper()));
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                var fullNameTrimmed = fullName.Trim();
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    fullNameTrimmed = $"\"{fullNameTrimmed}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, fullNameTrimmed));
                }
                else
                    query = query.Where(x => x.User.Name.ToUpper().Contains(fullNameTrimmed.ToUpper())
                                    || x.User.PaternalSurname.ToUpper().Contains(fullNameTrimmed.ToUpper())
                                    || x.User.MaternalSurname.ToUpper().Contains(fullNameTrimmed.ToUpper()));
            }


            if (studentState != null)
                query = query.Where(x => x.Status == studentState.Value);

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (academicProgramId != null)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (admissionTermId != null)
                query = query.Where(x => x.AdmissionTermId == admissionTermId);

            if (graduationTermId != null)
                query = query.Where(x => x.GraduationTermId == graduationTermId);

            if (graduationYear != null)
                query = query.Where(x => x.GraduationTerm.Year == graduationYear.Value);


            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    userName = x.User.UserName,
                    paternalSurname = x.User.PaternalSurname ?? "-",
                    MaternalSurname = x.User.MaternalSurname ?? "-",
                    name = x.User.Name ?? "-",
                    dni = x.User.Dni ?? "-",
                    facultyName = x.Career.Faculty.Name,
                    careerName = x.Career.Name,
                    academicProgramName = x.AcademicProgram.Name ?? "-",
                    curriculumCode = x.Curriculum.Code,
                    studentState = ConstantHelpers.Student.States.VALUES.ContainsKey(x.Status) ?
                            ConstantHelpers.Student.States.VALUES[x.Status] : "-",
                    campusName = x.Campus.Name ?? "",
                    admissionTermName = x.AdmissionTerm.Name,
                    admissionTypeName = x.AdmissionType.Name,
                    firstEnrollmentTermName = x.FirstEnrollmentTermId != null ? x.FirstEnrollmentTerm.Name : "-",
                    graduationTermName = x.GraduationTermId.HasValue ? x.GraduationTerm.Name : "-",
                    weightedAverageCumulative = (x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number)
                                .Select(y => y.WeightedAverageCumulative).FirstOrDefault()).ToString("0.00"),
                    sex = ConstantHelpers.SEX.ABREV.ContainsKey(x.User.Sex)
                            ? ConstantHelpers.SEX.ABREV[x.User.Sex] : "-",
                    phoneNumber = x.User.PhoneNumber ?? "-",
                    email = x.User.Email ?? "-",
                    address = x.User.Address ?? "-",
                    department = x.User.Department.Name ?? "-",
                    province = x.User.Province.Name ?? "-",
                    district = x.User.District.Name ?? "-",
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeReportStudentDatatable(DataTablesStructs.SentParameters sentParameters, string userName = null, string dni = null, string fullName = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.PaternalSurname); break;
                case "2":
                    orderByPredicate = ((x) => x.User.MaternalSurname); break;
                case "3":
                    orderByPredicate = ((x) => x.User.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.User.Dni); break;
                case "5":
                    orderByPredicate = ((x) => x.Career.Faculty.Name); break;
                case "6":
                    orderByPredicate = ((x) => x.Career.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.AcademicProgram.Name); break;
                case "8":
                    orderByPredicate = ((x) => x.Curriculum.Code); break;
                case "9":
                    orderByPredicate = ((x) => x.Status); break; //Estado de Practicas ?
                case "10":
                    orderByPredicate = ((x) => x.Campus.Name); break;
                case "11":
                    orderByPredicate = ((x) => x.AdmissionTerm.Name); break;
                case "12":
                    orderByPredicate = ((x) => x.AdmissionType.Name); break;
                case "13":
                    orderByPredicate = ((x) => x.FirstEnrollmentTerm.Name); break;
                case "14":
                    orderByPredicate = ((x) => x.User.Sex); break;
                case "15":
                    orderByPredicate = ((x) => x.User.PhoneNumber); break;
                case "16":
                    orderByPredicate = ((x) => x.User.Email); break;
                case "17":
                    orderByPredicate = ((x) => x.User.Address); break;
                case "18":
                    orderByPredicate = ((x) => x.User.Department.Name); break;
                case "19":
                    orderByPredicate = ((x) => x.User.Province.Name); break;
                case "20":
                    orderByPredicate = ((x) => x.User.District.Name); break;
            }

            var query = _context.Students
                .AsNoTracking();

            if (!string.IsNullOrEmpty(userName))
            {
                var userNameTrimmed = userName.Trim();
                query = query.Where(x => x.User.UserName.ToUpper().Contains(userNameTrimmed.ToUpper()));
            }

            if (!string.IsNullOrEmpty(dni))
            {
                var dniTrimmed = dni.Trim();
                query = query.Where(x => x.User.Dni.ToUpper().Contains(dniTrimmed.ToUpper()));
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                var fullNameTrimmed = fullName.Trim();
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    fullNameTrimmed = $"\"{fullNameTrimmed}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, fullNameTrimmed));
                }
                else
                    query = query.Where(x => x.User.Name.ToUpper().Contains(fullNameTrimmed.ToUpper())
                                    || x.User.PaternalSurname.ToUpper().Contains(fullNameTrimmed.ToUpper())
                                    || x.User.MaternalSurname.ToUpper().Contains(fullNameTrimmed.ToUpper()));
            }

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (academicProgramId != null)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (admissionTermId != null)
                query = query.Where(x => x.AdmissionTermId == admissionTermId);


            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    userName = x.User.UserName,
                    paternalSurname = x.User.PaternalSurname ?? "-",
                    MaternalSurname = x.User.MaternalSurname ?? "-",
                    name = x.User.Name ?? "-",
                    dni = x.User.Dni ?? "-",
                    facultyName = x.Career.Faculty.Name,
                    careerName = x.Career.Name,
                    academicProgramName = x.AcademicProgram.Name ?? "-",
                    curriculumCode = x.Curriculum.Code,
                    //Estado de practicas ???
                    hasExperience = x.StudentExperiences.Count == 0 ? "NO" : "SI",
                    campusName = x.Campus.Name ?? "",
                    admissionTermName = x.AdmissionTerm.Name,
                    admissionTypeName = x.AdmissionType.Name,
                    firstEnrollmentTermName = x.FirstEnrollmentTermId != null ? x.FirstEnrollmentTerm.Name : "-",
                    sex = ConstantHelpers.SEX.ABREV.ContainsKey(x.User.Sex)
                            ? ConstantHelpers.SEX.ABREV[x.User.Sex] : "-",
                    phoneNumber = x.User.PhoneNumber ?? "-",
                    email = x.User.Email ?? "-",
                    address = x.User.Address ?? "-",
                    department = x.User.Department.Name ?? "-",
                    province = x.User.Province.Name ?? "-",
                    district = x.User.District.Name ?? "-",
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task DownloadGlobalListExcel(IXLWorksheet worksheet, string dni = null, string name = null, string userName = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null)
        {
            var facultyName = "Todas";
            var careerName = "Todas";
            var termName = "TODOS";
            if (termId.HasValue)
            {
                var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
                termName = term.Name;
            }
            if (facultyId.HasValue)
            {
                var faculty = await _context.Faculties.Where(x => x.Id == facultyId).FirstOrDefaultAsync();
                facultyName = faculty.Name;
            }
            if (careerId.HasValue)
            {
                var career = await _context.Careers.Where(x => x.Id == careerId).FirstOrDefaultAsync();
                careerName = career.Name;
            }
            CreateHeaderRow(worksheet, termName, facultyName, careerName);
            await LoadGlobalListInformation(worksheet, dni, name, userName, termId, facultyId, careerId, academicProgramId);
        }

        #region STUDENT EXCEL LISTA GLOBAL
        protected static void CreateHeaderRow(IXLWorksheet worksheet, string termName, string facultyName, string careerName)
        {
            const int position = 4;
            var column = 0;


            worksheet.Cell("A1").Value = "RELACION GLOBAL " + termName;
            var range1 = worksheet.Range("A1:Q1");
            range1.Merge().Style.Font.SetBold().Font.FontSize = 16;
            range1.Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //worksheet.Range("A1:Q17").Row(1).Merge();


            worksheet.Cell("A2").Value = "ESCUELA PROFESIONAL: " + careerName;
            var range2 = worksheet.Range("A2:D2");
            range2.Merge();

            worksheet.Cell("A3").Value = "FACULTAD: " + facultyName;
            var range3 = worksheet.Range("A3:D3");
            range3.Merge();


            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "N° DNI", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "CÓDIGO MATRICULA", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "APELLIDO PATERNO", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "APELLIDO MATERNO", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "NOMBRES", column);
            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "FACULTAD", column);
            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "ESCUELA PROFESIONAL", column);
            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "PROGRAMA ACADÉMICO/ESPECIALIDAD", column);
            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "PLAN CURRICULAR", column);
            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "BACHILLER", column);
            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "TITULADO", column);
            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "SEXO", column);
            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "CORREO ELECTRONICO", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "CELULAR", column);
            worksheet.Column(++column).Width = 80;
            SetHeaderRowStyle(worksheet, "DIRECCION", column);
            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "DEPARTAMENTO PROCEDENCIA", column);
            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "PROVINCIA PROCEDENCIA", column);
            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "DISTRITO PROCEDENCIA", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "PERIODO DE INGRESO", column);
            worksheet.Column(++column).Width = 45;
            SetHeaderRowStyle(worksheet, "MODALIDAD DE INGRESO", column);
            worksheet.Column(++column).Width = 40;
            SetHeaderRowStyle(worksheet, "PERIODO LECTIVO 1ra MATRICULA", column);
            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "PERIODO LECTIVO EGRESO", column);
            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "NOTA PROMEDIO DE EGRESO", column);


            worksheet.SheetView.FreezeRows(position);
            worksheet.Row(position).SetAutoFilter();
        }
        protected static void SetHeaderRowStyle(IXLWorksheet worksheet, string headerName, int column)
        {
            const int position = 4;
            var fillColor = XLColor.FromArgb(0x0c618c);
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);
            var fontColor = XLColor.White;
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            const XLAlignmentHorizontalValues alignmentHorizontal = XLAlignmentHorizontalValues.Left;

            worksheet.Column(column).Style.Alignment.Horizontal = alignmentHorizontal;
            worksheet.Cell(position, column).Value = headerName;
            worksheet.Cell(position, column).Style.Font.Bold = true;
            worksheet.Cell(position, column).Style.Font.FontColor = fontColor;
            worksheet.Cell(position, column).Style.Fill.BackgroundColor = fillColor;
            worksheet.Cell(position, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(position, column).Style.Border.OutsideBorderColor = outsideBorderColor;
        }

        protected static void SetHeaderRowStyle2(IXLWorksheet worksheet, string headerName, int column)
        {
            const int position = 1;
            var fillColor = XLColor.FromArgb(0x0c618c);
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);
            var fontColor = XLColor.White;
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            const XLAlignmentHorizontalValues alignmentHorizontal = XLAlignmentHorizontalValues.Left;

            worksheet.Column(column).Style.Alignment.Horizontal = alignmentHorizontal;
            worksheet.Cell(position, column).Value = headerName;
            worksheet.Cell(position, column).Style.Font.Bold = true;
            worksheet.Cell(position, column).Style.Font.FontColor = fontColor;
            worksheet.Cell(position, column).Style.Fill.BackgroundColor = fillColor;
            worksheet.Cell(position, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(position, column).Style.Border.OutsideBorderColor = outsideBorderColor;
        }

        protected static void SetInformationStyle(IXLWorksheet worksheet, int row, int column, string data, bool requireDateFormat = false, bool isPureText = false)
        {
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);

            if (requireDateFormat)
            {
                worksheet.Cell(row, column).Style.DateFormat.Format = "dd/MM/yyyy";
            }

            //https://github.com/ClosedXML/ClosedXML/issues/1142
            //Usado para que no se conviertan algunos textos en fechas
            if (isPureText)
            {
                worksheet.Cell(row, column).DataType = XLDataType.Text;
                worksheet.Cell(row, column).Value = $"'{data}";
            }
            else
            {
                worksheet.Cell(row, column).Value = data;
            }

            worksheet.Cell(row, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(row, column).Style.Border.OutsideBorderColor = outsideBorderColor;
        }
        private async Task LoadGlobalListInformation(IXLWorksheet worksheet, string dni = null, string name = null, string userName = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null)
        {

            var row = 5;
            const int DNI = 1;    //CODUNIV
            const int USERNAME = 2;    //RAZ_SOC 
            const int PATERNALSURNAME = 3;  //FAC_NOM
            const int MATERNALSURNAME = 4;    //CARR_PROG
            const int NAME = 5;  //
            const int FACULTY = 6;
            const int CAREER = 7;
            const int ACADEMICPROGRAM = 8;
            const int CURRICULUM = 9;
            const int BACHELOR = 10;
            const int QUALIFIED = 11;
            const int SEX = 12;    //ESC_POST
            const int EMAIL = 13;  //RELACION --
            const int PHONE = 14;     //
            const int ADDRESS = 15;     // 
            const int DEPARTMENT = 16;    //
            const int PROVINCE = 17;      //
            const int DISTRICT = 18;   //
            const int ADMISIONTERM = 19;  //  
            const int MODALITY_TYPE = 20; //MATRI_FEC 
            const int FIRST_TERM = 21; //EGRES_FEC
            const int GRADUATIONTERM = 22;  //PROC_BACH
            const int AVERAGE = 23;   //GRAD_TITU


            var query = _context.Students.AsQueryable();

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (academicProgramId != null)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (!string.IsNullOrEmpty(dni))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    dni = $"\"{dni}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.Dni, dni));
                }
                else
                    query = query.Where(x => x.User.Dni.ToUpper().Contains(dni.ToUpper()));
            }

            if (!string.IsNullOrEmpty(userName))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    userName = $"\"{userName}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.UserName, userName));
                }
                else
                    query = query.Where(x => x.User.UserName.ToUpper().Contains(userName.ToUpper()));
            }

            if (termId.HasValue)
            {
                query = query.Where(x => x.GraduationTermId == termId);
            }

            if (!string.IsNullOrEmpty(name))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    name = $"\"{name}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, name));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(name.ToUpper()));
            }


            var queryList = await query
                .Select(x => new
                {
                    x.User.Dni,
                    x.User.UserName,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    x.User.Name,
                    Sex = ConstantHelpers.SEX.ABREV.ContainsKey(x.User.Sex) ? ConstantHelpers.SEX.ABREV[x.User.Sex] : "-",
                    x.User.Email,
                    x.User.PhoneNumber,
                    x.User.Address,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    AcademicProgram = x.AcademicProgram.Name,
                    Curriculum = x.Curriculum.Code,
                    x.Status,
                    Department = x.User.Department != null ? x.User.Department.Name : "--",
                    Province = x.User.Province != null ? x.User.Province.Name : "--",
                    District = x.User.District != null ? x.User.District.Name : "--",
                    AdmissionTerm = x.AdmissionTerm.Name,
                    ModalityType = x.AdmissionType.Name,
                    FirstTerm = x.FirstEnrollmentTerm != null ? x.FirstEnrollmentTerm.Name : "--",
                    GraduationTerm = x.GraduationTerm != null ? x.GraduationTerm.Name : "--",
                    Average = (x.AcademicSummaries.Where(y => y.StudentId == x.Id).OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number)
                            .Select(y => y.WeightedAverageCumulative).FirstOrDefault()).ToString("0.00")
                })
                .ToListAsync();

            foreach (var item in queryList)
            {
                var bachelorText = "NO";
                if (item.Status == ConstantHelpers.Student.States.BACHELOR || item.Status == ConstantHelpers.Student.States.QUALIFIED)
                    bachelorText = "SI";

                var qualifiedText = "NO";
                if (item.Status == ConstantHelpers.Student.States.QUALIFIED)
                    qualifiedText = "SI";

                SetInformationStyle(worksheet, row, DNI, item.Dni);
                SetInformationStyle(worksheet, row, USERNAME, item.UserName);
                SetInformationStyle(worksheet, row, PATERNALSURNAME, item.PaternalSurname);
                SetInformationStyle(worksheet, row, MATERNALSURNAME, item.MaternalSurname);
                SetInformationStyle(worksheet, row, NAME, item.Name);
                SetInformationStyle(worksheet, row, FACULTY, item.Faculty);
                SetInformationStyle(worksheet, row, CAREER, item.Career);
                SetInformationStyle(worksheet, row, ACADEMICPROGRAM, item.AcademicProgram);
                SetInformationStyle(worksheet, row, CURRICULUM, item.Curriculum);
                SetInformationStyle(worksheet, row, BACHELOR, bachelorText);
                SetInformationStyle(worksheet, row, QUALIFIED, qualifiedText);
                SetInformationStyle(worksheet, row, SEX, item.Sex);
                SetInformationStyle(worksheet, row, EMAIL, item.Email);
                SetInformationStyle(worksheet, row, PHONE, item.PhoneNumber);
                SetInformationStyle(worksheet, row, ADDRESS, item.Address);
                SetInformationStyle(worksheet, row, DEPARTMENT, item.Department);
                SetInformationStyle(worksheet, row, PROVINCE, item.Province);
                SetInformationStyle(worksheet, row, DISTRICT, item.District);
                SetInformationStyle(worksheet, row, ADMISIONTERM, item.AdmissionTerm, false, true);
                SetInformationStyle(worksheet, row, MODALITY_TYPE, item.ModalityType);
                SetInformationStyle(worksheet, row, FIRST_TERM, item.FirstTerm, false, true);
                SetInformationStyle(worksheet, row, GRADUATIONTERM, item.GraduationTerm, false, true);
                SetInformationStyle(worksheet, row, AVERAGE, item.Average);
                row++;
            }
        }
        #endregion

        #region STUDENT EXCEL REPORTE EGRESADOS
        protected static void CreateHeaderRowGraduated(IXLWorksheet worksheet, string careerName)
        {
            const int position = 4;
            var column = 0;


            worksheet.Cell("A1").Value = "REPORTE DE EGRESADOS";
            var range1 = worksheet.Range("A1:J1");
            range1.Merge().Style.Font.SetBold().Font.FontSize = 16;
            range1.Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //worksheet.Range("A1:Q17").Row(1).Merge();


            worksheet.Cell("A2").Value = "ESCUELA PROFESIONAL: " + careerName;
            var range2 = worksheet.Range("A2:D2");
            range2.Merge();


            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Código", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Apellido Paterno", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Apellido Materno", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Nombres", column);
            worksheet.Column(++column).Width = 35;
            SetHeaderRowStyle(worksheet, "Facultad", column);
            worksheet.Column(++column).Width = 35;
            SetHeaderRowStyle(worksheet, "Escuela Profesional", column);
            worksheet.Column(++column).Width = 35;
            SetHeaderRowStyle(worksheet, "Programa Académico", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Sexo", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Prácticas", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Teléfono", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Correo", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "DNI", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Dirección", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Grado", column);
            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle(worksheet, "Año de egreso", column);
            worksheet.Column(++column).Width = 25;

            worksheet.SheetView.FreezeRows(position);
        }
        protected static void SetHeaderRowStyleGraduated(IXLWorksheet worksheet, string headerName, int column)
        {
            const int position = 4;
            var fillColor = XLColor.FromArgb(0x0c618c);
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);
            var fontColor = XLColor.White;
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            const XLAlignmentHorizontalValues alignmentHorizontal = XLAlignmentHorizontalValues.Left;

            worksheet.Column(column).Style.Alignment.Horizontal = alignmentHorizontal;
            worksheet.Cell(position, column).Value = headerName;
            worksheet.Cell(position, column).Style.Font.Bold = true;
            worksheet.Cell(position, column).Style.Font.FontColor = fontColor;
            worksheet.Cell(position, column).Style.Fill.BackgroundColor = fillColor;
            worksheet.Cell(position, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(position, column).Style.Border.OutsideBorderColor = outsideBorderColor;
        }

        protected static void SetInformationStyleGraduated(IXLWorksheet worksheet, int row, int column, string data, bool requireDateFormat = false)
        {
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);

            if (requireDateFormat)
            {
                worksheet.Cell(row, column).Style.DateFormat.Format = "dd/MM/yyyy";
            }

            worksheet.Cell(row, column).Value = data;
            worksheet.Cell(row, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(row, column).Style.Border.OutsideBorderColor = outsideBorderColor;
        }
        private async Task LoadGlobalListInformationGraduated(IXLWorksheet worksheet, bool isCoordinator, int gradeType, Guid careerParameterId, int year = 0, int admissionYear = 0, List<Guid> careers = null)
        {

            var row = 5;
            const int P1 = 1;    //CODUNIV
            const int P2 = 2;    //RAZ_SOC 
            const int P3 = 3;  //FAC_NOM
            const int P4 = 4;    //CARR_PROG
            const int P5 = 5;  //PROGRAMA_ACADEMICO --
            const int P6 = 6;    //ESC_POST
            const int P7 = 7;  //RELACION --
            const int P8 = 8;     //
            const int P9 = 9;     // 
            const int P10 = 10;     // 
            const int P11 = 11;     // 
            const int P12 = 12;     // 
            const int P13 = 13;     // 
            const int P14 = 14;     // 
            const int P15 = 15;     // 


            var query = _context.Students
                              .AsQueryable();

            if (year > 0)
            {
                query = query.Where(x => x.GraduationTermId != null && x.GraduationTerm.Year == year);
            }

            if (admissionYear > 0)
            {
                query = query.Where(x => x.AdmissionTerm.Year == year);
            }

            if (isCoordinator)
            {
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            if (careerParameterId != Guid.Empty)
            {
                query = query.Where(x => x.CareerId == careerParameterId);
            }

            switch (gradeType)
            {
                case ConstantHelpers.Student.States.GRADUATED:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED);
                    break;
                case ConstantHelpers.Student.States.BACHELOR:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR);
                    break;
                case ConstantHelpers.Student.States.QUALIFIED:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED);
                    break;
                default:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED || x.Status == ConstantHelpers.Student.States.BACHELOR || x.Status == ConstantHelpers.Student.States.GRADUATED);
                    break;
            }


            var queryList = await query
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    x.User.Name,
                    x.User.FullName,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    AcademicProgram = x.AcademicProgram.Name,
                    Sex = ConstantHelpers.SEX.ABREV.ContainsKey(x.User.Sex) ? ConstantHelpers.SEX.ABREV[x.User.Sex] : "--",
                    Dni = x.User.Dni ?? "--",
                    x.User.Email,
                    HasExperiences = x.StudentExperiences.Count == 0 ? "NO" : "SI",
                    x.User.PhoneNumber,
                    Address = x.User.Address ?? "--",
                    Status = ConstantHelpers.Student.States.VALUES.ContainsKey(x.Status) ? ConstantHelpers.Student.States.VALUES[x.Status] : "--",
                    GraduationYear = x.GraduationTerm == null ? "--" : x.GraduationTerm.Year.ToString()
                })
                .ToListAsync();

            foreach (var item in queryList)
            {
                SetInformationStyle(worksheet, row, P1, item.UserName);
                SetInformationStyle(worksheet, row, P2, item.PaternalSurname);
                SetInformationStyle(worksheet, row, P3, item.MaternalSurname);
                SetInformationStyle(worksheet, row, P4, item.Name);
                SetInformationStyle(worksheet, row, P5, item.Faculty);
                SetInformationStyle(worksheet, row, P6, item.Career);
                SetInformationStyle(worksheet, row, P7, item.AcademicProgram);
                SetInformationStyle(worksheet, row, P8, item.Sex);
                SetInformationStyle(worksheet, row, P9, item.HasExperiences);
                SetInformationStyle(worksheet, row, P10, item.PhoneNumber);
                SetInformationStyle(worksheet, row, P11, item.Email);
                SetInformationStyle(worksheet, row, P12, item.Dni);
                SetInformationStyle(worksheet, row, P13, item.Address);
                SetInformationStyle(worksheet, row, P14, item.Status);
                SetInformationStyle(worksheet, row, P15, item.GraduationYear);
                row++;
            }


        }
        #endregion

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsByAdmissionTermReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null, bool? showEnrolled = null, Guid? admissionTypeId = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.Dni;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.AdmissionType.Name;
                    break;
                case "6":
                    //orderByPredicate = (x) => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId);
                    break;
                default:
                    break;
            }

            var query = _context.Students
                .Where(x => x.AdmissionTermId == termId)
                .AsQueryable();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var careerQry = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.AcademicDepartmentDirectorId == userId || x.AcademicSecretaryId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty) careerQry = careerQry.Where(x => x.FacultyId == facultyId);
                    if (careerId.HasValue && careerId != Guid.Empty) careerQry = careerQry.Where(x => x.Id == careerId);

                    var careers = careerQry
                        .Select(x => x.Id)
                        .ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var careerQry = _context.Careers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId || x.Faculty.AdministrativeAssistantId == userId);
                    if (facultyId.HasValue && facultyId != Guid.Empty) careerQry = careerQry.Where(x => x.FacultyId == facultyId);
                    if (careerId.HasValue && careerId != Guid.Empty) careerQry = careerQry.Where(x => x.Id == careerId);
                    var careers = careerQry
                        .Select(x => x.Id)
                        .ToHashSet();
                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty) query = query.Where(x => x.AcademicProgramId == academicProgramId);
            if (admissionTypeId.HasValue && admissionTypeId != Guid.Empty) query = query.Where(x => x.AdmissionTypeId == admissionTypeId);
            if (showEnrolled.HasValue) query = query.Where(x => showEnrolled.Value == x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId));
            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.User.UserName,
                    document = x.User.Dni,
                    name = x.User.FullName,
                    admissionType = x.AdmissionType.Name,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    date = x.AdmissionTerm.StartDate.ToLocalDateFormat(),
                    enrolled = x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId)
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsByAdmissionTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, Guid? facultyId = null, string search = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.Document;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.AdmissionType.Name;
                    break;
                case "6":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                default:
                    break;
            }

            var query = _context.Students
                .Where(x => x.AdmissionTermId == termId)
                .AsQueryable();

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper()) || x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.Document.Contains(search));

            var recordsTotal = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    document = x.User.Document,
                    name = x.User.FullName,
                    faculty = x.Career.Faculty.Name,
                    career = x.Career.Name,
                    admissionType = x.AdmissionType.Name,
                    code = x.User.UserName
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetDebtorStudentsReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.Dni;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "6":
                    orderByPredicate = (x) => x.AdmissionType.Name;
                    break;
                default:
                    break;
            }

            var query = GetDebtorStudentsData(termId, facultyId, careerId, academicProgramId, user);

            var payments = await _context.Payments
                .Include(x => x.Concept)
                .Where(x => x.TermId == termId &&
                                            x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING &&
                                            x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT)
                .ToListAsync();

            var recordsFiltered = await query.CountAsync();
            var client = await query
                 .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.User.UserName,
                    document = x.User.Dni,
                    name = x.User.FullName,
                    admissionType = x.AdmissionType.Name,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    date = x.AdmissionTerm.StartDate.ToLocalDateFormat(),
                    userid = x.UserId
                })
                .ToListAsync();
            var data = client
                .Select(x => new DebtStudentTemplate
                {
                    Code = x.code,
                    Document = x.document,
                    Name = x.name,
                    AdmissionType = x.admissionType,
                    Career = x.career,
                    Faculty = x.faculty,
                    Date = x.date,
                    Debt = payments.Where(y => y.UserId == x.userid).Sum(y => y.Concept.Amount),
                    UserId = x.userid,
                }).ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<List<DebtStudentTemplate>> GetDebtorStudentsDataPdf(Guid termId, Guid? facultyId, Guid? careerId, Guid? academicProgramId, ClaimsPrincipal user)
        {
            var debts = GetDebtorStudentsData(termId, facultyId, careerId, academicProgramId, user);

            var payments = await _context.Payments
              .Include(x => x.Concept)
              .Where(x => x.TermId == termId &&
                                          x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING &&
                                          x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT)
              .ToListAsync();
            var client = await debts
               .Select(x => new
               {
                   code = x.User.UserName,
                   document = x.User.Dni,
                   name = x.User.FullName,
                   admissionType = x.AdmissionType.Name,
                   career = x.Career.Name,
                   faculty = x.Career.Faculty.Name,
                   date = x.AdmissionTerm.StartDate.ToLocalDateFormat(),
                   userid = x.UserId
               })
               .ToListAsync();
            var data = client
                .Select(x => new DebtStudentTemplate
                {
                    Code = x.code,
                    Document = x.document,
                    Name = x.name,
                    AdmissionType = x.admissionType,
                    Career = x.career,
                    Faculty = x.faculty,
                    Date = x.date,
                    UserId = x.userid,
                    Debt = payments.Where(y => y.UserId == x.userid).Sum(y => y.Concept.Amount),
                }).ToList();
            return data;
        }
        private IQueryable<Student> GetDebtorStudentsData(Guid termId, Guid? facultyId, Guid? careerId, Guid? academicProgramId, ClaimsPrincipal user)
        {
            var query = _context.Students
                .Include(x => x.User.Payments)
                                .Where(x => x.User.Payments.Any(y =>
                                            y.TermId == termId &&
                                            y.Status == ConstantHelpers.PAYMENT.STATUS.PENDING &&
                                            y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT))
                .AsQueryable();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var careerQry = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.AcademicDepartmentDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty) careerQry = careerQry.Where(x => x.FacultyId == facultyId);
                    if (careerId.HasValue && careerId != Guid.Empty) careerQry = careerQry.Where(x => x.Id == careerId);

                    var careers = careerQry
                        .Select(x => x.Id)
                        .ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty) query = query.Where(x => x.AcademicProgramId == academicProgramId);
            return query;
        }

        public async Task<IEnumerable<Student>> GetStudentsByAdmissionTermPdfData(Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, ClaimsPrincipal user = null, Guid? admissionTypeId = null)
        {
            var query = _context.Students
                .Where(x => x.AdmissionTermId == termId)
                .AsQueryable();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    var careerQry = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.AcademicDepartmentDirectorId == userId || x.AcademicSecretaryId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty) careerQry = careerQry.Where(x => x.FacultyId == facultyId);
                    if (careerId.HasValue && careerId != Guid.Empty) careerQry = careerQry.Where(x => x.Id == careerId);

                    var careers = careerQry
                        .Select(x => x.Id)
                        .ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.AdministrativeAssistantId == userId);
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty) query = query.Where(x => x.AcademicProgramId == academicProgramId);
            if (admissionTypeId.HasValue && admissionTypeId != Guid.Empty) query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            var data = await query
                .Select(x => new Student
                {
                    CreatedAt = x.CreatedAt,
                    Id = x.Id,
                    AdmissionDate = x.AdmissionDate,
                    User = new ApplicationUser
                    {
                        UserName = x.User.UserName,
                        FullName = x.User.FullName,
                        Dni = x.User.Dni,
                        Document = x.User.Document,
                        District = new District
                        {
                            Code = x.User.District.Code,
                            Name = x.User.District.Name,
                        },
                        Province = new Province
                        {
                            Code = x.User.Province.Code,
                            Name = x.User.Province.Name,
                        },
                        Department = new Department
                        {
                            Code = x.User.Department.Code,
                            Name = x.User.Department.Name,
                        }
                    },
                    AdmissionType = new ENTITIES.Models.Admission.AdmissionType
                    {
                        Name = x.AdmissionType.Name,

                    },
                    StudentSections = x.StudentSections
                        .Where(y => y.Section.CourseTerm.TermId == termId)
                        .Select(y => new StudentSection
                        {
                            Id = y.Id
                        }).ToList(),
                    Career = new Career
                    {
                        Code = x.Career.Code,
                        Name = x.Career.Name
                    },
                    AcademicProgram = new AcademicProgram
                    {
                        Code = x.AcademicProgram.Code,
                        Name = x.AcademicProgram.Name
                    },
                    StudentInformation = new StudentInformation
                    {
                        OriginDistrict = new District
                        {
                            Code = x.StudentInformation.OriginDistrict.Code,
                            Name = x.StudentInformation.OriginDistrict.Name,
                        },
                        PlaceOriginDistrict = new District
                        {
                            Code = x.StudentInformation.PlaceOriginDistrict.Code,
                            Name = x.StudentInformation.PlaceOriginDistrict.Name,
                        },
                    }
                })
                .ToListAsync();

            return data;

            //return new List<Student>();
        }

        public async Task<IEnumerable<EnrolledStudentExcelTemplate>> GetStudentsByEquivalenceData(Guid termId, Guid? facultyId = null, Guid? careerId = null)
        {

            var academicHistories = _context.AcademicHistories.Where(x => x.Type == ConstantHelpers.AcademicHistory.Types.REGULAR && x.Validated == true);

            var term = await _context.Terms.FindAsync(termId);

            var query = _context.Students
                .Where(x => x.Observations.Any(i => i.Type == ConstantHelpers.OBSERVATION_TYPES.EQUIVALENCE && i.CreatedAt.HasValue
                && term.StartDate.Date <= i.CreatedAt.Value.Date && i.CreatedAt.Value <= term.EndDate.Date))

                .AsNoTracking();

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);


            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);


            var data = await query.Select(x => new EnrolledStudentExcelTemplate
            {
                Code = x.User.UserName,
                Name = x.User.FullName,
                Career = x.Career.Name,
                Faculty = x.Career.Faculty.Name,
                AcademicYear = x.CurrentAcademicYear
            }).ToListAsync();

            return data;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithExtemporaneousEnrollmentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var watch = new Stopwatch();
            watch.Restart();

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                default:
                    break;
            }

            var query = _context.Students
              .FilterActiveStudents()
              .AsNoTracking();

            var isIntegrated = bool.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM));

            if (isIntegrated)
            {
                var prod = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_PROCEDURE);
                var procedureId = string.IsNullOrEmpty(prod) ? Guid.Empty : Guid.Parse(prod);

                var procedures = _context.UserProcedures
                     .Where(x => x.ProcedureId == procedureId
                     && x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.REQUESTED || x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.IN_PROCESS)
                     .Select(x => x.UserId)
                     .ToHashSet();

                query = query.Where(x => procedures.Contains(x.UserId));
            }
            else
            {
                var conceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXTEMPORANEOUS_ENROLLMENT_SURCHARGE_PROCEDURE));

                var payments = _context.Payments
                    .Where(x => x.ConceptId == conceptId && x.Status == CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PAID && x.PaymentDate.HasValue && x.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT_PROCESSED)
                    .Select(x => x.UserId)
                    .ToHashSet();

                query = query.Where(x => payments.Contains(x.UserId));
            }


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

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty)
                    query = query.Where(x => x.Career.FacultyId == facultyId);


                if (careerId.HasValue && careerId != Guid.Empty)
                    query = query.Where(x => x.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue) || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                  || x.User.UserName.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    academicYear = x.CurrentAcademicYear,
                    type = ConstantHelpers.Student.States.VALUES.ContainsKey(x.Status) ? ConstantHelpers.Student.States.VALUES[x.Status] : "---",
                    credits = x.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    id = x.Id,
                    academicProgram = x.AcademicProgram.Name
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            var time = watch.Elapsed;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithCreditsIncomingDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? type = null, string searchValue = null, bool? onlyWithCredits = null, ClaimsPrincipal user = null)
        {
            var watch = new Stopwatch();
            watch.Restart();

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                default:
                    break;
            }

            var term = await _context.Terms
                    .OrderByDescending(x => x.Year)
                    .ThenByDescending(x => x.Number)
                    .FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.ENTRANT)
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

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty)
                    query = query.Where(x => x.Career.FacultyId == facultyId);


                if (careerId.HasValue && careerId != Guid.Empty)
                    query = query.Where(x => x.CareerId == careerId);
            }

            if (type.HasValue && type != -1) query = query.Where(x => x.Status == type);

            if (onlyWithCredits.HasValue && onlyWithCredits.Value)
                query = query.Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId));

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue) || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                  || x.User.UserName.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    academicYear = x.CurrentAcademicYear,
                    type = ConstantHelpers.Student.States.VALUES.ContainsKey(x.Status) ? ConstantHelpers.Student.States.VALUES[x.Status] : "---",
                    credits = x.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    id = x.Id,
                    academicProgram = x.AcademicProgram.Name
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var time = watch.Elapsed;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsByRecognitionDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                default:
                    break;
            }

            var term = await _context.Terms.FindAsync(termId);

            var query = _context.Students
                .Where(x => x.Observations.Any(i => i.Type == ConstantHelpers.OBSERVATION_TYPES.EQUIVALENCE
                //&& i.CreatedAt.HasValue && term.StartDate.Date <= i.CreatedAt.Value.Date && i.CreatedAt.Value <= term.EndDate.Date))
                && i.TermId == term.Id))
                .AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId || x.Career.Faculty.AdministrativeAssistantId == userId);
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, search) || EF.Functions.Contains(x.User.UserName, search));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(search.ToUpper()) || x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    academicYear = x.CurrentAcademicYear,
                    id = x.Id,
                    academicProgram = x.AcademicProgram.Name
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
        public async Task<object> SearchIncomingStudentByTerm(string term, Guid? careerId = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.ENTRANT)
               .AsNoTracking();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(x => x.User.FullName.ToUpper().Contains(term.ToUpper()) || x.User.UserName.ToUpper().Contains(term.ToUpper()));
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers
                            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                            .AsNoTracking();

                        if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);
                        var careers = qryCareers.Select(x => x.Id).ToHashSet();

                        query = query.Where(x => careers.Contains(x.CareerId));
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                        query = query.Where(x => careers.Any(y => y == x.CareerId));
                    }
                }
                else
                {
                    if (careerId != null && careerId != Guid.Empty)
                        query = query.Where(x => x.CareerId == careerId);
                }
            }

            var students = await query
                .OrderBy(x => x.User.UserName)
                .Take(5)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.User.UserName} - {x.User.FullName}"
                })
                .ToListAsync();

            return students;
        }

        public async Task<List<Student>> GetByDni(string document)
        {
            return await _context.Students
                   .Where(x => x.User.Dni == document || x.User.Document == document)
                   .Include(x => x.User)
                   .Include(x => x.Career)
                   .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsByDniAndTerm(string document, Guid termId)
        {
            return await _context.Students
                   .Where(x => (x.User.Dni == document || x.User.Document == document) && x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                   .Include(x => x.User)
                   .Include(x => x.Career)
                   .ToListAsync();
        }

        public async Task<ApplicationUser> BankBatchUserByDocument(string document, int length)
        {
            var paddedDocument = document.PadLeft(length, '0');
            var sqlPad = new string('0', length);

            var user = await _context.Students
                   .Where(x => (sqlPad + x.User.Dni).EndsWith(paddedDocument)
                           || x.User.Dni == document
                           || (sqlPad + x.User.Document).EndsWith(paddedDocument)
                           || x.User.Document == document
                   )
                   .Select(x => x.User)
                   .FirstOrDefaultAsync();

            return user;
        }

        public IQueryable<Student> GetActiveStudents()
        {
            return _context.Students
                    .FilterActiveStudents()
                    .AsNoTracking();
        }

        public IQueryable<Student> GetStudentWithStudentSectionsByTermId(Guid termId)
        {
            return _context.Students
                     .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId))
                     .AsNoTracking();
        }

        public async Task<object> GetTopFacultiesEnrollled(Guid id)
        {
            return await _context.Students
              .Where(s => s.StudentSections.Any(ss => ss.Section.CourseTerm.TermId.Equals(id)))
              .GroupBy(s => s.Career.Faculty.Name)
              .Select(s => new
              {
                  faculty = s.Key,
                  careers = _context.Careers.Where(f => f.Faculty.Name.Equals(s.Key)).Count(),
                  //careers = _context.Faculties.First(f => f.Name.Equals(s.Key)).Careers.Count(),
                  enrolledCount = s.Count()
              }).ToListAsync();
        }

        public async Task UpdateStudentsCurrentAcademicYearJob(string connectionString)
        {
            var academicHistories = await _context.AcademicHistories
                .Where(x => x.Approved)
                .Select(x => new
                {
                    x.StudentId,
                    x.CourseId
                }).ToArrayAsync();

            var students = await _context.Students
                .Select(x => new
                {
                    x.Id,
                    x.CurriculumId
                }).ToArrayAsync();

            var curriculumsAcademicYearCourses = await _context.AcademicYearCourses
                //.Include(x => x.AcademicYearCourses)
                .ToListAsync();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    using (var sqlCommand = sqlConnection.CreateCommand())
                    {
                        sqlCommand.CommandText = $"UPDATE {CORE.Helpers.ConstantHelpers.ENTITY_MODELS.GENERALS.STUDENTS} SET CurrentAcademicYear = @CurrentAcademicYear WHERE Id = @Id";
                        sqlCommand.Transaction = sqlTransaction;

                        sqlCommand.Parameters.Add("@CurrentAcademicYear", SqlDbType.Int);
                        sqlCommand.Parameters.Add("@Id", SqlDbType.UniqueIdentifier);
                        sqlCommand.Prepare();

                        for (var i = 0; i < students.Count(); i++)
                        {
                            var student = students[i];

                            var list = academicHistories
                                .Where(x => x.StudentId == student.Id)
                                .Select(x => x.CourseId)
                                .ToArray();

                            var academicHistoriesCourseId = new HashSet<Guid>(list);

                            var academicYears = curriculumsAcademicYearCourses
                                .Where(x => x.CurriculumId == student.CurriculumId)
                                .GroupBy(x => x.AcademicYear)
                                .Select(x => new
                                {
                                    ApprovedCourses = x
                                        .Where(y => !y.IsElective && academicHistoriesCourseId.Contains(y.CourseId))
                                        .Select(y => y.Id)
                                        .Count(),
                                    TotalCourses = x
                                        .Where(y => !y.IsElective)
                                        .Select(y => y.Id)
                                        .Count(),
                                    Year = x.Key
                                })
                                .ToList();

                            var academicYear = academicYears
                                .Where(x => x.TotalCourses != 0 && x.ApprovedCourses == x.TotalCourses
                                || (x.ApprovedCourses < x.TotalCourses && (1.0 * x.ApprovedCourses / (x.TotalCourses * 1.0)) > 0.5))
                                .OrderByDescending(x => x.Year)
                                .FirstOrDefault();

                            if (academicYear != null)
                            {
                                sqlCommand.Parameters["@CurrentAcademicYear"].Value = academicYear.Year;
                                sqlCommand.Parameters["@Id"].Value = student.Id;

                                await sqlCommand.ExecuteNonQueryAsync();
                            }
                            else
                            {
                                academicYear = academicYears
                                    .Where(x => x.ApprovedCourses < x.TotalCourses)
                                    .OrderBy(x => x.Year)
                                    .FirstOrDefault();

                                if (academicYear != null)
                                {
                                    sqlCommand.Parameters["@CurrentAcademicYear"].Value = academicYear.Year;
                                    sqlCommand.Parameters["@Id"].Value = student.Id;

                                    await sqlCommand.ExecuteNonQueryAsync();
                                }
                            }
                        }

                        sqlTransaction.Commit();
                    }
                }
            }
        }

        public async Task UpdateStudentOrderJob()
        {
            var careers = await _context.Careers.ToListAsync();

            foreach (var career in careers)
            {
                var students = await _context.Students.Where(x => x.CareerId == career.Id).ToListAsync();

                foreach (var student in students)
                {
                    var summary = await _context.AcademicSummaries.Where(x => x.StudentId == student.Id).OrderByDescending(x => x.Term.Name).FirstOrDefaultAsync();

                    if (summary != null)
                    {
                        student.CurrentMeritOrder = summary.MeritOrder;
                        student.CurrentMeritType = summary.MeritType;
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task FixCurriculumStudentsJob()
        {
            var careersWithouteCurriculum = await _context.Careers.Where(x => !x.Curriculums.Any(y => y.IsActive)).ToListAsync();
            var courses = await _context.Courses.ToListAsync();
            foreach (var career in careersWithouteCurriculum)
            {
                var curriculum = new Curriculum
                {
                    IsActive = true,
                    CareerId = career.Id,
                    Code = $"{career.Name.Substring(0, 3)}2019",
                    IsNew = false,
                    CreationResolutionNumber = "RES. N°00001-2019",
                    CreationResolutionDate = DateTime.Today,
                    CreationResolutionFile = "",
                    StudyRegime = ConstantHelpers.CURRICULUM.STUDY_REGIME.BIANNUAL,
                    ValidityStart = DateTime.Today,
                    ValidityEnd = DateTime.Today.AddYears(2),
                    Year = 2018,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Curriculums.AddAsync(curriculum);

                await _context.SaveChangesAsync();
            }

            var students = await _context.Students.Where(x => x.CurriculumId == null).ToListAsync();

            var curriculums = await _context.Curriculums.Where(x => x.IsActive).ToListAsync();

            foreach (var student in students)
            {
                var curriculum = curriculums.Where(x => x.CareerId == student.CareerId).FirstOrDefault();

                student.CurriculumId = curriculum.Id;
            }

            await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateStudentAcademicYearJob()
        {
            var students = await _context.Students.ToListAsync();
            var count = 0;
            var total = 0;

            foreach (var student in students)
            {
                var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == student.Id && x.Approved).ToListAsync();

                var advance = await _context.AcademicYearCourses
                    .Where(x => x.CurriculumId == student.CurriculumId)
                    .GroupBy(x => x.AcademicYear)
                    .Select(x => new
                    {
                        Year = x.Key,
                        ApprovedCourses = x.Where(y => !y.IsElective && academicHistories.Any(z => z.CourseId == y.CourseId)).Count(),
                        TotalCourses = x.Where(y => !y.IsElective).Count()
                    }).ToListAsync();

                var year = advance
                    .Where(x => x.ApprovedCourses == x.TotalCourses || (x.ApprovedCourses < x.TotalCourses && (1.0 * x.ApprovedCourses / (x.TotalCourses * 1.0)) > 0.5))
                    .OrderByDescending(x => x.Year).FirstOrDefault();


                if (year != null)
                {
                    student.CurrentAcademicYear = year.Year == 10 ? year.Year : year.Year + 1;
                }
                else
                {
                    year = advance.Where(x => x.ApprovedCourses < x.TotalCourses).OrderBy(x => x.Year).FirstOrDefault();

                    if (year != null)
                    {
                        student.CurrentAcademicYear = year.Year;
                    }
                }

                count++;
                total++;

                if (count >= 250)
                {
                    await _context.SaveChangesAsync();
                    count = 0;
                }
            }

            await _context.SaveChangesAsync();
            return count;
        }

        public async Task UpdateStudentStatusJob()
        {
            var careers = await _context.Careers.ToListAsync();
            var count = 0;
            var disapprovedCoursesLimit = 2;

            for (var i = 0; i < careers.Count; i++)
            {
                var career = careers[i];
                var students = await _context.Students
                    .Where(x => x.CareerId == career.Id &&
                        (x.Status == CORE.Helpers.ConstantHelpers.Student.States.REGULAR ||
                        x.Status == CORE.Helpers.ConstantHelpers.Student.States.IRREGULAR ||
                        x.Status == CORE.Helpers.ConstantHelpers.Student.States.REPEATER)
                    )
                    .ToListAsync();

                for (var j = 0; j < students.Count; j++)
                {
                    var student = students[j];
                    var lastAcademicSummary = await _context.AcademicSummaries
                        .Where(x => x.StudentId == student.Id)
                        .OrderByDescending(x => x.Term.Name)
                        .FirstOrDefaultAsync();

                    if (lastAcademicSummary != null)
                    {
                        var academicHistories = _context.AcademicHistories
                         .Where(x => x.StudentId == student.Id && x.TermId == lastAcademicSummary.TermId)
                         .AsQueryable();

                        if (await academicHistories.AnyAsync(x => x.Try >= 3 && !x.Approved))
                        {
                            student.Status = CORE.Helpers.ConstantHelpers.Student.States.SANCTIONED;
                        }
                        else
                        {
                            if (await academicHistories.AllAsync(x => x.Grade >= 13))
                            {
                                student.Status = CORE.Helpers.ConstantHelpers.Student.States.UNBEATEN;
                            }
                            else
                            {
                                if (await academicHistories.AllAsync(x => x.Approved))
                                {
                                    student.Status = CORE.Helpers.ConstantHelpers.Student.States.REGULAR;
                                }
                                else
                                {
                                    if (await academicHistories.CountAsync(x => !x.Approved) <= disapprovedCoursesLimit)
                                    {
                                        student.Status = CORE.Helpers.ConstantHelpers.Student.States.IRREGULAR;
                                    }
                                    else
                                    {
                                        student.Status = CORE.Helpers.ConstantHelpers.Student.States.REPEATER;
                                    }
                                }
                            }
                        }
                    }

                    count++;
                }

                await _context.SaveChangesAsync();
            }
        }
        public async Task UnbeaterStudentsJob()
        {
            var academicHistories = await _context.AcademicHistories
                            .Where(x => x.Student.Status == CORE.Helpers.ConstantHelpers.Student.States.REGULAR)
                            .Select(x => new
                            {
                                x.StudentId,
                                x.Approved
                            }).ToArrayAsync();

            var students = await _context.Students
                .Where(x => x.Status == CORE.Helpers.ConstantHelpers.Student.States.REGULAR)
                .ToListAsync();

            foreach (var student in students)
            {
                var isUnbeaten = academicHistories.Where(x => x.StudentId == student.Id).All(x => x.Approved);

                if (isUnbeaten) student.Status = CORE.Helpers.ConstantHelpers.Student.States.UNBEATEN;
            }

            await _context.SaveChangesAsync();
        }
        public async Task CreateStudentsJob(UserManager<ApplicationUser> userManager, int count, Term term, bool isIntegrated, Guid procedureId_conceptId)
        {
            const string password = "Enchufate.2018";
            var admissionTypes = await _context.AdmissionTypes.ToListAsync();

            var oldTerms = await _context.Terms.Where(x => x.Status == CORE.Helpers.ConstantHelpers.TERM_STATES.FINISHED).OrderByDescending(x => x.Name).ToListAsync();
            var academicYearLimit = 9;// oldTerms.Count + 1;

            var careers = await _context.Careers.ToListAsync();

            var careersWithouteCurriculum = await _context.Careers.Where(x => !x.Curriculums.Any(y => y.IsActive)).ToListAsync();
            var courses = await _context.Courses.ToListAsync();
            foreach (var career in careersWithouteCurriculum)
            {
                var curriculum = new Curriculum
                {
                    IsActive = true,
                    CareerId = career.Id,
                    Code = $"TEST{career.Name.Substring(0, 3)}",
                    IsNew = false,
                    CreationResolutionNumber = "RESTEST #123",
                    CreationResolutionDate = DateTime.Today,
                    CreationResolutionFile = "",
                    StudyRegime = ConstantHelpers.CURRICULUM.STUDY_REGIME.BIANNUAL,
                    ValidityStart = DateTime.Today,
                    ValidityEnd = DateTime.Today.AddYears(2),
                    Year = 2018,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Curriculums.AddAsync(curriculum);

                var academicYearCourses = new AcademicYearCourse[]
                {
                    new AcademicYearCourse { AcademicYear =  1, CourseId = courses.FirstOrDefault(x => x.Name == "Lenguaje I").Id },
                    new AcademicYearCourse { AcademicYear =  1, CourseId = courses.FirstOrDefault(x => x.Name == "Matemática I").Id },
                    new AcademicYearCourse { AcademicYear =  1, CourseId = courses.FirstOrDefault(x => x.Name == "Programación I").Id },
                    new AcademicYearCourse { AcademicYear =  1, CourseId = courses.FirstOrDefault(x => x.Name == "Ética y Ciudadanía").Id },
                    new AcademicYearCourse { AcademicYear =  2, CourseId = courses.FirstOrDefault(x => x.Name == "Matemática II").Id },
                    new AcademicYearCourse { AcademicYear =  2, CourseId = courses.FirstOrDefault(x => x.Name == "Lenguaje II").Id },
                    new AcademicYearCourse { AcademicYear =  2, CourseId = courses.FirstOrDefault(x => x.Name == "Seminario I").Id },
                    new AcademicYearCourse { AcademicYear =  3, CourseId = courses.FirstOrDefault(x => x.Name == "Estructuras de Datos").Id },
                    new AcademicYearCourse { AcademicYear =  5, CourseId = courses.FirstOrDefault(x => x.Name == "Desarrollo de Aplicaciones").Id },
                    new AcademicYearCourse { AcademicYear =  6, CourseId = courses.FirstOrDefault(x => x.Name == "Design Thinking").Id },
                    new AcademicYearCourse { AcademicYear =  8, CourseId = courses.FirstOrDefault(x => x.Name == "Seminario II").Id },
                    new AcademicYearCourse { AcademicYear =  9, CourseId = courses.FirstOrDefault(x => x.Name == "Taller de Proyecto I").Id },
                    new AcademicYearCourse { AcademicYear = 10, CourseId = courses.FirstOrDefault(x => x.Name == "Taller de Proyecto II").Id },
                };

                await _context.AcademicYearCourses.AddRangeAsync(academicYearCourses);

                await _context.SaveChangesAsync();
            }


            var userCodePrefix = $"T{term.Year}{term.Number}";
            var usersWithCodeExist = await _context.Users.Where(u => u.NormalizedUserName.StartsWith(userCodePrefix)).Select(u => u.UserName).OrderByDescending(u => u).FirstOrDefaultAsync();
            var userCodeSufix = usersWithCodeExist == null ? 1 : int.Parse(usersWithCodeExist.Substring(usersWithCodeExist.Length - 5)) + 1;

            var birthDate = ConvertHelpers.DatepickerToDatetime("01/01/1990");

            var emailSuffix = $"@{GeneralHelpers.GetInstitutionAbbreviation().ToLower()}.pe";

            var rnd = new Random();

            var academicHistories = new List<AcademicHistory>();
            var academicSummaries = new List<AcademicSummary>();
            var enrollmentTurns = new List<EnrollmentTurn>();

            ///////////////////////////////////////////////////////////////////////////////////////


            var enrollmentFeeCost = 0.00M;
            var enrollmentFeeSubtotal = 0.00M;
            var enrollmentFeeIgv = 0.00M;

            var enrollmentFeeId = Guid.Empty;
            var enrollmentFeeDescription = "";

            var procedureDependency = (Guid?)null;

            if (isIntegrated)
            {
                var procedure = await _context.Procedures.FindAsync(procedureId_conceptId);

                enrollmentFeeId = procedureId_conceptId;
                enrollmentFeeDescription = procedure.Name;
                enrollmentFeeCost = (decimal)await _context.Procedures
                                .Where(x => x.Id == procedureId_conceptId)
                                .SumAsync(x => x.ProcedureRequirements.Sum(p => p.Cost));

                enrollmentFeeSubtotal = enrollmentFeeCost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                enrollmentFeeIgv = enrollmentFeeCost - enrollmentFeeSubtotal;

                procedureDependency = await _context.ProcedureDependencies.Where(x => x.ProcedureId == procedureId_conceptId).OrderBy(x => x.CreatedAt).Select(x => x.DependencyId).FirstOrDefaultAsync();
            }
            else
            {
                var concept = await _context.Concepts.FindAsync(procedureId_conceptId);

                enrollmentFeeId = procedureId_conceptId;
                enrollmentFeeDescription = concept.Description;
                enrollmentFeeCost = concept.Amount;

                if (concept.IsTaxed)
                {
                    enrollmentFeeSubtotal = enrollmentFeeCost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                    enrollmentFeeIgv = enrollmentFeeCost - enrollmentFeeSubtotal;
                }
                else
                {
                    enrollmentFeeSubtotal = enrollmentFeeCost;
                }
            }

            for (var i = 0; i < count; i++)
            {
                var dni = rnd.Next(10000000, 99999999);
                var admissionType = admissionTypes[rnd.Next(admissionTypes.Count)];
                var career = careers[rnd.Next(careers.Count)];

                var studentCode = userCodePrefix + userCodeSufix.ToString().PadLeft(5, '0');

                var validateCodeExist = await _context.Users.AnyAsync(x => x.UserName == studentCode);
                if (validateCodeExist)
                {
                    while (validateCodeExist)
                    {
                        userCodeSufix++;
                        studentCode = userCodePrefix + userCodeSufix.ToString().PadLeft(5, '0');
                        validateCodeExist = await _context.Users.AnyAsync(x => x.UserName == studentCode);
                    }
                }

                var user = new ApplicationUser
                {
                    UserName = studentCode,
                    Email = $"{studentCode}{emailSuffix}",
                    Dni = dni.ToString(),
                    Name = GetRandomName(rnd),
                    PaternalSurname = GetRandomLastName(rnd),
                    MaternalSurname = GetRandomLastName(rnd),
                    BirthDate = birthDate
                };
                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded && result.Errors.First().Code == "DuplicateUserName")
                {
                    while (!result.Succeeded && result.Errors.First().Code == "DuplicateUserName")
                    {
                        userCodeSufix++;
                        studentCode = userCodePrefix + userCodeSufix.ToString().PadLeft(5, '0');

                        user.UserName = studentCode;
                        result = await userManager.CreateAsync(user, password);
                    }
                }

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, CORE.Helpers.ConstantHelpers.ROLES.STUDENTS);
                    var curriculum = await _context.Curriculums.Where(x => x.CareerId == career.Id && x.IsActive).FirstOrDefaultAsync();

                    var academicYear = rnd.Next(1, academicYearLimit);
                    var student = new Student
                    {
                        CareerId = career.Id,
                        CurrentAcademicYear = academicYear,
                        RegisterDate = DateTime.UtcNow,
                        UserId = user.Id,
                        AdmissionTypeId = admissionType.Id,
                        AdmissionTermId = term.Id,
                        Status = academicYear == 1 ? CORE.Helpers.ConstantHelpers.Student.States.ENTRANT : CORE.Helpers.ConstantHelpers.Student.States.REGULAR,
                        CurriculumId = curriculum.Id
                    };

                    await _context.Students.AddAsync(student);


                    if (curriculum != null)
                    {
                        var academicYears = await _context.AcademicYearCourses
                            .Where(x => x.CurriculumId == curriculum.Id && x.AcademicYear < academicYear)
                            //.Include(x => x.AcademicYearCourses)
                            .OrderByDescending(x => x.AcademicYear)
                            .GroupBy(x => x.AcademicYear)
                            .ToListAsync();

                        var index = 0;

                        foreach (var year in academicYears)
                        {
                            foreach (var course in year)
                            {
                                var academicHistory = new AcademicHistory
                                {
                                    CourseId = course.CourseId,
                                    Approved = true,
                                    Grade = 18,
                                    SectionId = null,
                                    StudentId = student.Id,
                                    Observations = "Convalidado por seed",
                                    TermId = oldTerms[index].Id,
                                    Validated = true,
                                    Try = 1,
                                    Withdraw = false,
                                    Type = CORE.Helpers.ConstantHelpers.AcademicHistory.Types.REGULAR,
                                    CurriculumId = student.CurriculumId
                                };

                                academicHistories.Add(academicHistory);
                            }

                            var academicSummary = new AcademicSummary
                            {
                                CareerId = student.CareerId,
                                StudentId = student.Id,
                                TermId = term.Id,
                                WeightedAverageGrade = 18,
                                WeightedAverageCumulative = 18,
                                MeritOrder = 1,
                                MeritType = CORE.Helpers.ConstantHelpers.ACADEMIC_ORDER.NONE,
                                TotalCredits = 0,
                                ApprovedCredits = 0,
                                StudentAcademicYear = student.CurrentAcademicYear - index,
                                TermHasFinished = true
                            };

                            academicSummaries.Add(academicSummary);

                            index++;
                        }
                    }

                    var enrollmentTurn = new EnrollmentTurn
                    {
                        StudentId = student.Id,
                        TermId = term.Id,
                        Time = DateTime.Today
                    };
                    enrollmentTurns.Add(enrollmentTurn);

                    if (isIntegrated)
                    {
                        var userProcedure = new UserProcedure
                        {
                            UserId = user.Id,
                            ProcedureId = enrollmentFeeId,
                            TermId = term.Id,
                            Status = CORE.Helpers.ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED,
                            DependencyId = procedureDependency
                        };

                        await _context.UserProcedures.AddAsync(userProcedure);

                        var payment = new Payment
                        {
                            Description = enrollmentFeeDescription,
                            SubTotal = enrollmentFeeSubtotal,
                            IgvAmount = enrollmentFeeIgv,
                            Discount = 0.00M,
                            Total = enrollmentFeeCost,
                            EntityId = userProcedure.Id,
                            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId
                        };

                        await _context.Payments.AddAsync(payment);

                        userProcedure.PaymentId = payment.Id;
                    }
                    else
                    {
                        var payment = new Payment
                        {
                            Description = enrollmentFeeDescription,
                            SubTotal = enrollmentFeeSubtotal,
                            IgvAmount = enrollmentFeeIgv,
                            Discount = 0.00M,
                            Total = enrollmentFeeCost,
                            EntityId = enrollmentFeeId,
                            Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId,
                            ConceptId = enrollmentFeeId
                        };

                        await _context.Payments.AddAsync(payment);
                    }
                }

                userCodeSufix++;
            }

            await _context.EnrollmentTurns.AddRangeAsync(enrollmentTurns);
            await _context.AcademicHistories.AddRangeAsync(academicHistories);
            await _context.AcademicSummaries.AddRangeAsync(academicSummaries);

            await _context.SaveChangesAsync();
        }
        private string GetRandomName(Random rnd)
        {
            var names = new[]
            {
                "Rashad",
                "London",
                "Adelyn",
                "Ayana",
                "Destiny",
                "Gideon",
                "Leia",
                "Kendrick",
                "Amiah",
                "Julius",
                "Micah",
                "Issac",
                "Jase",
                "Sophia",
                "Isis",
                "Liliana",
                "Jordan",
                "Jay",
                "Aubrie",
                "Adison",
                "Allen",
                "Todd",
                "Micaela",
                "Julissa",
                "Annika",
                "Peyton",
                "Tiana",
                "Beau",
                "Max",
                "Johnathan",
                "Fiona",
                "Rayan",
                "Amelia",
                "Frank",
                "Gunnar",
                "Adrienne",
                "Miah",
                "Lincoln",
                "Jeremy",
                "Heath",
                "Camila",
                "Pamela",
                "Braxton",
                "Warren",
                "Gavyn",
                "Jazmine",
                "Nikhil",
                "Mekhi",
                "Jamie",
                "Eli"
            };
            return names[rnd.Next(names.Length)];
        }
        private string GetRandomLastName(Random rnd)
        {
            var lastnames = new[]
            {
                "Arellano",
                "Dunn",
                "Friedman",
                "Mclaughlin",
                "Norris",
                "Olson",
                "Santana",
                "Bass",
                "Evans",
                "Ho",
                "Davenport",
                "Marks",
                "Chandler",
                "Osborne",
                "Fuller",
                "Combs",
                "Bradford",
                "Curtis",
                "Green",
                "Vega",
                "Peck",
                "Love",
                "Moreno",
                "Mcdonald",
                "Ramirez",
                "Carter",
                "Spence",
                "Acevedo",
                "Solomon",
                "Schmitt",
                "Hicks",
                "Key",
                "Hampton",
                "Whitehead",
                "Schwartz",
                "Frye",
                "Webster",
                "Wagner",
                "Riddle",
                "Montes",
                "Gamble",
                "Allen",
                "Sparks",
                "Hays",
                "Walsh",
                "Reilly",
                "Ramos",
                "Tucker",
                "Morales",
                "Miles"
            };
            return lastnames[rnd.Next(lastnames.Length)];
        }


        public async Task<object> SearchEnabledStudentForCourseByTerm(string term, Guid? courseId = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                .FilterActiveStudents()
                .AsNoTracking();

            if (!string.IsNullOrEmpty(term))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    term = $"\"{term}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, term) || EF.Functions.Contains(x.User.UserName, term));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(term.ToUpper()) || x.User.UserName.ToUpper().Contains(term.ToUpper()));
            }

            if (courseId.HasValue && courseId != Guid.Empty)
            {
                var courseCurriculums = _context.AcademicYearCourses.Where(x => x.CourseId == courseId).Select(x => x.CurriculumId).ToHashSet();
                query = query.Where(x => courseCurriculums.Contains(x.CurriculumId) && !x.AcademicHistories.Any(y => y.CourseId == courseId && y.Approved));
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var qryCareers = _context.Careers
                            .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                            .AsNoTracking();

                        var careers = qryCareers.Select(x => x.Id).ToHashSet();

                        query = query.Where(x => careers.Contains(x.CareerId));
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                        query = query.Where(x => careers.Any(y => y == x.CareerId));
                    }
                }
            }

            var students = await query
                .OrderBy(x => x.User.UserName)
                .ThenBy(x => x.User.FullName)
                .Take(5)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.User.UserName} - {x.User.FullName}"
                })
                .ToListAsync();

            return students;
        }

        public async Task<IEnumerable<Student>> GetFacultyStudentsEnrolled(Guid termId)
        {
            var query = _context.Students
                .Where(x => x.StudentSections.Any(z => z.Section.CourseTerm.TermId == termId
                && z.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN))
                .AsQueryable();

            var result = await query
                .Select(x => new Student
                {
                    Id = x.Id,
                    Career = new Career
                    {
                        Id = x.Career.Id,
                        Faculty = new Faculty
                        {
                            Id = x.Career.FacultyId
                        }
                    },
                    CampusId = x.CampusId
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentLastTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string searchValue = null)
        {
            var activeTermId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();
            var lastTermId = await _context.Terms.OrderByDescending(x => x.Year).ThenByDescending(x => x.Number).Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED).Select(x => x.Id).FirstOrDefaultAsync();

            var studentLastTerm = _context.Students.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == lastTermId)).Select(x => x.Id);

            var studentActiveTerm = _context.Students.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == activeTermId)).Select(x => x.Id);

            var query = _context.Students.Where(x => studentLastTerm.Contains(x.Id) && !studentActiveTerm.Contains(x.Id)).AsQueryable();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue) || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                  || x.User.UserName.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    career = x.Career.Name,
                    faculty = x.Career.Faculty.Name,
                    academicYear = x.CurrentAcademicYear,
                    type = ConstantHelpers.Student.States.VALUES.ContainsKey(x.Status) ? ConstantHelpers.Student.States.VALUES[x.Status] : "---",
                    id = x.Id,
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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
        public async Task<List<StudentsByInstitutionalRecordTemplate>> GetStudentsByInstitutionalRecordGeneralData(Guid id, Guid termId, byte sisfohClasification)
        {
            var query = _context.InstitutionalRecordCategorizationByStudents.Include(x => x.Student.User).Where(x => x.InstitutionalWelfareRecordId == id && x.TermId == termId).AsQueryable();
            if (sisfohClasification > 0)
            {
                query = query.Where(x => x.SisfohClasification == sisfohClasification);
            }

            var data = await query
                .Select(x => new StudentsByInstitutionalRecordTemplate
                {
                    User = x.Student.User.FullName,
                    Email = x.Student.User.Email,
                    UserName = x.Student.User.UserName,
                    CategorizationName = x.CategorizationLevel.Name,
                    FinalScore = x.StudentScore,
                    CareerName = x.Student.Career.Name,
                    SisfohClasification = ConstantHelpers.INSTITUTIONAL_WELFARE_SURVEY.SISFOH.VALUES.ContainsKey(x.SisfohClasification) ? ConstantHelpers.INSTITUTIONAL_WELFARE_SURVEY.SISFOH.VALUES[x.SisfohClasification] : "--",
                    SisfohConstancy = x.SisfohConstancy
                }).ToListAsync();

            return data;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsByInstitutionalRecordGeneralDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, Guid termId,  byte? sisfohClasification = null, Guid? categorizationLevelId = null, Guid? careerId = null)
        {
            var query = _context.InstitutionalRecordCategorizationByStudents
                .Where(x => x.InstitutionalWelfareRecordId == id && x.TermId == termId).AsNoTracking();

            if (sisfohClasification != null)
            {
                query = query.Where(x => x.SisfohClasification == sisfohClasification);
            }

            if (categorizationLevelId != null)
                query = query.Where(x => x.CategorizationLevelId == categorizationLevelId);

            if (careerId != null)
                query = query.Where(x => x.Student.CareerId == careerId);

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    Id = x.Id,
                    x.StudentId,
                    User = x.Student.User.FullName,
                    Email = x.Student.User.Email,
                    UserName = x.Student.User.UserName,
                    CategorizationName = x.CategorizationLevel.Name,
                    FinalScore = x.StudentScore,
                    CareerName = x.Student.Career.Name,
                    SisfohClasification = ConstantHelpers.INSTITUTIONAL_WELFARE_SURVEY.SISFOH.VALUES.ContainsKey(x.SisfohClasification) ? ConstantHelpers.INSTITUTIONAL_WELFARE_SURVEY.SISFOH.VALUES[x.SisfohClasification] : "--",
                    SisfohClasificationExists = String.IsNullOrEmpty(x.SisfohConstancy),
                    SisfohConstancy = x.SisfohConstancy
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

        public async Task DownloadExcelGraduates(IXLWorksheet worksheet, bool isCoordinator, List<Guid> careers, int gradeType, Guid careerParameterId, int year = 0, int admissionYear = 0)
        {
            var careerName = "Todas";
            if (careerParameterId != Guid.Empty)
            {
                var career = await _context.Careers.Where(x => x.Id == careerParameterId).FirstOrDefaultAsync();
                careerName = career.Name;
            }
            CreateHeaderRowGraduated(worksheet, careerName);
            await LoadGlobalListInformationGraduated(worksheet, isCoordinator, gradeType, careerParameterId, year, admissionYear, careers);
        }

        public async Task<decimal> GetApprovedCreditsByStudentId(Guid studentId)
        {
            var total = 0.0M;

            var student = await _context.Students.FindAsync(studentId);

            var academicYearCoursesQry = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId
                && (x.IsElective || !x.Course.AcademicProgramId.HasValue || x.Course.AcademicProgramId == student.AcademicProgramId || x.Course.AcademicProgram.Code == "00"))
                .AsNoTracking();

            var academicYearCourses = await academicYearCoursesQry
                .Select(x => x.CourseId)
                .ToListAsync();

            var approvedCourses = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId && x.Approved)
                .Select(x => new
                {
                    x.CourseId,
                    x.Course.Credits
                })
                .Distinct()
                .ToListAsync();

            total = approvedCourses.Where(x => academicYearCourses.Contains(x.CourseId)).Sum(x => x.Credits);
            return total;
        }

        public async Task<decimal> GetEnrolledCreditsByStudentId(Guid studentId, byte? status = null)
        {
            var query = _context.StudentSections
                .Where(x => x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE && x.StudentId == studentId)
                .AsNoTracking();

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status);
            }

            var credits = await query.SumAsync(y => y.Section.CourseTerm.Course.Credits);
            return credits;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetOriginStudent(List<Guid> careerId, DataTablesStructs.SentParameters sentParameters, Guid? termId = null, Guid? deparment = null, Guid? province = null, Guid? district = null)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.Email;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.PhoneNumber;
                    break;
                case "4":
                    orderByPredicate = (x) => x.User.Department.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.User.Province.Name;
                    break;
                case "6":
                    orderByPredicate = (x) => x.User.District.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.User.FullName;
                    break;
            }

            var query = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                .AsNoTracking();


            if (deparment.HasValue && deparment != Guid.Empty)
                query = query.Where(x => x.User.DepartmentId == deparment);
            if (province.HasValue && province != Guid.Empty)
                query = query.Where(x => x.User.ProvinceId == province);
            if (district.HasValue && district != Guid.Empty)
                query = query.Where(x => x.User.DistrictId == district);
            //if (careerId.HasValue && careerId != Guid.Empty)
            //    query = query.Where(x => x.CareerId == careerId);
            if (careerId.Count > 0)
            {
                query = query.Where(x => careerId.Contains(x.CareerId));
            }
            //if (careerId.Count > 0)
            //{
            //    foreach(var item in careerId)
            //    {
            //        query = query.Where(x => x.CareerId == item);
            //    }
            //}

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    email = x.User.Email,
                    phoneNumber = x.User.PhoneNumber,
                    department = x.User.Department.Name,
                    province = x.User.Province.Name,
                    district = x.User.District.Name,
                    career = x.Career.Name
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
        public async Task<List<ReportOriginTemplate>> GetOriginStudentDatatable(List<Guid> careerId, Guid? termId = null, Guid? deparment = null, Guid? province = null, Guid? district = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                .AsNoTracking();



            //AREQUIPA
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "01");

            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "02");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "03");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "04");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "06");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "07");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "08");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "10");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "20");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "21");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "27");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "28");
            query = query.Where(x => x.User.Department.Code != "04" || x.User.Province.Code != "01" || x.User.District.Code != "29");

            ////CALLAO
            query = query.Where(x => x.User.Department.Code != "07" || x.User.Province.Code != "01" || x.User.District.Code != "01");

            ////CUSCO
            query = query.Where(x => x.User.Department.Code != "08" || x.User.Province.Code != "05" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "08" || x.User.Province.Code != "07" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "08" || x.User.Province.Code != "07" || x.User.District.Code != "05");

            ////LIMA                                                           
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "03");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "06");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "08");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "09");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "10");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "11");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "12");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "13");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "14");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "15");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "17");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "18");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "20");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "21");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "22");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "31");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "32");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "34");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "38");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "40");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "41");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "42");
            query = query.Where(x => x.User.Department.Code != "15" || x.User.Province.Code != "01" || x.User.District.Code != "43");

            ////MOQUEGUA                              
            query = query.Where(x => x.User.Department.Code != "18" || x.User.Province.Code != "03" || x.User.District.Code != "01");

            ////PUNO                                       
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "01" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "01" || x.User.District.Code != "07");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "01" || x.User.District.Code != "09");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "01" || x.User.District.Code != "10");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "01" || x.User.District.Code != "11");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "01" || x.User.District.Code != "12");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "02" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "03" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "04" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "04" || x.User.District.Code != "02");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "04" || x.User.District.Code != "06");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "05" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "06" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "07" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "08" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "09" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "10" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "11" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "12" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "21" || x.User.Province.Code != "13" || x.User.District.Code != "01");

            ////TACNA                                     
            query = query.Where(x => x.User.Department.Code != "23" || x.User.Province.Code != "03" || x.User.District.Code != "01");
            query = query.Where(x => x.User.Department.Code != "23" || x.User.Province.Code != "03" || x.User.District.Code != "02");
            query = query.Where(x => x.User.Department.Code != "23" || x.User.Province.Code != "03" || x.User.District.Code != "09");

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId || x.Career.Faculty.AdministrativeAssistantId == userId);
                    }
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        query = query.Where(x => x.Career.CareerDirectorId == userId || x.Career.AcademicCoordinatorId == userId || x.Career.AcademicSecretaryId == userId);
                    }
                }
            }



            if (deparment.HasValue && deparment != Guid.Empty)
                query = query.Where(x => x.User.DepartmentId == deparment);
            if (province.HasValue && province != Guid.Empty)
                query = query.Where(x => x.User.ProvinceId == province);
            if (district.HasValue && district != Guid.Empty)
                query = query.Where(x => x.User.DistrictId == district);
            if (careerId.Count > 0)
                query = query.Where(x => careerId.Contains(x.CareerId));


            var studentReportOrigin = await query
                .Select(x => new
                {
                    code = x.User.UserName,
                    name = x.User.FullName,
                    email = x.User.Email,
                    phoneNumber = x.User.PhoneNumber,
                    departmentId = x.User.DepartmentId,
                    department = x.User.Department.Name,
                    provinceId = x.User.ProvinceId,
                    province = x.User.Province.Name,
                    districtId = x.User.DistrictId,
                    district = x.User.District.Name,
                    career = x.Career.Name
                })
                .ToListAsync();

            var result = studentReportOrigin
                .GroupBy(y => new { y.departmentId, y.department })
                .Where(x => !string.IsNullOrEmpty(x.Key.department))
                .Select(x => new ReportOriginTemplate
                {
                    DepartmentName = !string.IsNullOrEmpty(x.Key.department) ? x.Key.department : "--NO REGISTRADO--",
                    Provinces = x.GroupBy(y => new { y.provinceId, y.province })
                    .Select(y => new ProvinceOriginTemplate
                    {
                        ProvinceName = !string.IsNullOrEmpty(y.Key.province) ? y.Key.province : "--NO REGISTRADO--",
                        Districts = y.GroupBy(z => new { z.districtId, z.district })
                        .Select(q => new DistrictOriginTemplate
                        {
                            DistrictName = !string.IsNullOrEmpty(q.Key.district) ? q.Key.district : "--NO REGISTRADO--",
                            Total = q.Count(),
                            Students = q.Select(w => new StudentReportOriginTemplate
                            {
                                Code = w.code,
                                Name = w.name,
                                Email = w.email,
                                PhoneNumber = w.phoneNumber,
                                Province = w.province,
                                Department = w.department,
                                Career = w.career,
                                District = w.district
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();

            return result;

        }
        public async Task<List<StudentOriginTemplate>> GetOriginStudentReportUniqe(List<Guid> careerId, Guid? termId = null, Guid? deparment = null, Guid? province = null, Guid? district = null, ClaimsPrincipal user = null)
        {
            var query = _context.Students
               .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
               .AsNoTracking();


            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }

            }
            if (deparment.HasValue && deparment != Guid.Empty)
                query = query.Where(x => x.User.DepartmentId == deparment);
            if (province.HasValue && province != Guid.Empty)
                query = query.Where(x => x.User.ProvinceId == province);
            if (district.HasValue && district != Guid.Empty)
                query = query.Where(x => x.User.DistrictId == district);
            if (careerId.Count > 0)
                query = query.Where(x => careerId.Contains(x.CareerId));

            var data = await query
                .OrderBy(x => x.User.FullName)
                .Select(x => new StudentOriginTemplate
                {
                    Code = x.User.UserName,
                    Name = x.User.FullName,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    Department = x.User.Department.Name,
                    Province = x.User.Province.Name,
                    District = x.User.District.Name,
                })
                .ToListAsync();

            return data;
        }
        public async Task<List<OriginReportTemplate>> GetOriginStudentReport(Guid termId, ClaimsPrincipal user = null)
        {
            //var termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var query = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId)).AsNoTracking();

            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
            }

            var data = await query
                .Select(x => new
                {
                    province = x.User.Province.Name,
                    deparment = x.User.Department.Name,
                    disctrict = x.User.District.Name,
                    career = x.Career.Name,
                    x.User.Sex
                })
                .ToListAsync();

            var result = data
                .GroupBy(x => new
                {
                    x.province,
                    x.deparment,
                    x.disctrict,
                    x.career
                })
                .Select(x => new OriginReportTemplate
                {
                    TermName = term.Name,
                    Career = x.Key.career,
                    Department = x.Key.deparment,
                    District = x.Key.disctrict,
                    Province = x.Key.province,
                    Fermale = x.Where(x => x.Sex == ConstantHelpers.SEX.FEMALE).Count(),
                    Male = x.Where(x => x.Sex == ConstantHelpers.SEX.MALE).Count(),
                    None = x.Where(x => x.Sex != ConstantHelpers.SEX.MALE && x.Sex != ConstantHelpers.SEX.FEMALE).Count(),
                    Total = x.Count()
                })
                .OrderBy(x => x.Department)
                .ThenBy(x => x.Province)
                .ThenBy(x => x.District)
                .ToList();

            return result;
        }

        public async Task<StudentSexReportTemplate> GetStudentSexReport(Guid termId, ClaimsPrincipal user = null)
        {
            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            var query = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == term.Id)).AsQueryable();

            if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                }
            }

            var data = await query
                .Select(x => new
                {
                    career = x.Career.Name,
                    careerCode = x.Career.Code,
                    x.User.Sex
                })
                .ToListAsync();

            var details = data
                .GroupBy(x => x.career)
                .Select(x => new StudentSexReportDetailTemplate
                {
                    Career = x.Key,
                    CareerCode = x.Select(x => x.careerCode).FirstOrDefault(),
                    Fermale = x.Where(x => x.Sex == ConstantHelpers.SEX.FEMALE).Count(),
                    Male = x.Where(x => x.Sex == ConstantHelpers.SEX.MALE).Count(),
                    None = x.Where(x => x.Sex != ConstantHelpers.SEX.MALE && x.Sex != ConstantHelpers.SEX.FEMALE).Count()
                })
                .ToList();

            var result = new StudentSexReportTemplate
            {
                Term = term.Name,
                Details = details
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentSecondCareerDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid? academicProgramId = null, int? academicYear = null)
        {
            var query = _context.Students
                .Where(x => x.CareerNumber == 2 && x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                     .AsNoTracking();

                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || User.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var deanId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == deanId || x.Faculty.SecretaryId == deanId || x.Faculty.AdministrativeAssistantId == deanId).AsNoTracking();


                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();

                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);
                if (careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (academicProgramId.HasValue) query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (academicYear.HasValue) query = query.Where(x => x.CurrentAcademicYear == academicYear);

            var students = await query
                .Select(x => new
                {
                    x.Id,
                    x.CareerNumber,
                    x.User.UserName,
                    x.User.Document,
                    x.User.FullName,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    x.CurrentAcademicYear
                }).ToListAsync();

            var studentHash = students.Select(x => x.Document).ToHashSet();

            var repeatedStudents = await _context.Students
                .Where(x => x.CareerNumber == 1 && studentHash.Contains(x.User.Document))
                .Select(x => new
                {
                    x.User.Document,
                    FirstCareer = x.Career.Name
                }).ToListAsync();
            //var repeatedStudents = students
            //    .GroupBy(x => x.Document)
            //    .Select(x => new
            //    {
            //        Dni = x.Key,
            //        FirstStudentId = x.OrderBy(y => y.CareerNumber).FirstOrDefault().Id,
            //        FirstCareerNumber = x.OrderBy(y => y.CareerNumber).FirstOrDefault().CareerNumber,
            //        FirstCareerName = x.OrderBy(y => y.CareerNumber).FirstOrDefault().Career,
            //        //FirstCareerNumber = x.OrderBy(y => y.CareerNumber).FirstOrDefault().CareerNumber,
            //        Careers = x.ToList(),
            //        Count = x.Count()
            //    })
            //    .Where(x => x.Count > 1)
            //    .ToList();

            //students = students
            //    .Where(x => repeatedStudents.Any(y => y.Dni == x.Document && y.FirstStudentId != x.Id))
            //    .ToList();

            var data = students
                .Select(x => new
                {
                    id = x.Id,
                    code = x.UserName,
                    name = x.FullName,
                    career = x.Career,
                    faculty = x.Faculty,
                    academicYear = x.CurrentAcademicYear,
                    firstCareer = repeatedStudents.Any(y => y.Document == x.Document) ? repeatedStudents.FirstOrDefault(y => y.Document == x.Document).FirstCareer : "---"
                }).ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsEnrolledMultipleCareersDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid? academicProgramId = null, int? academicYear = null, string search = null)
        {
            //var test = await _context.Users
            //    .Where(x => x.Students.Any(y => y.StudentSections.Any(z => z.)))
            //    .GroupBy(x => x.Document)
            //    .Where(x => x.Count() > 1)
            //    .Select(x => new {
            //        document = x.Key,
            //        name = x.Select(y => y.FullName).FirstOrDefault()
            //    }).ToListAsync();

            var students = await _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId))
                .Select(x => new
                {
                    x.User.Document,
                    x.User.FullName,
                    x.CareerId,
                    x.Career.Code,
                    x.Career.FacultyId,
                    x.AcademicProgramId,
                    x.CurrentAcademicYear
                }).ToListAsync();

            var dbData = students
                .GroupBy(x => new { x.Document, x.FullName })
                .Where(x => x.Count() > 1)
                .Select(x => new
                {
                    x.Key.Document,
                    x.Key.FullName,
                    Careers = x.ToList()
                }).ToList();

            var query = dbData.AsQueryable();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                     .AsNoTracking();

                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => x.Careers.Any(y => careers.Contains(y.CareerId)));
            }
            else if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || User.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var deanId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == deanId || x.Faculty.SecretaryId == deanId || x.Faculty.AdministrativeAssistantId == deanId).AsNoTracking();

                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => x.Careers.Any(y => careers.Contains(y.CareerId)));
            }
            else
            {
                if (facultyId != Guid.Empty) query = query.Where(x => x.Careers.Any(y => y.FacultyId == facultyId));
                if (careerId != Guid.Empty) query = query.Where(x => x.Careers.Any(y => y.CareerId == careerId));
            }

            if (academicProgramId.HasValue) query = query.Where(x => x.Careers.Any(y => y.AcademicProgramId == academicProgramId));

            if (academicYear.HasValue) query = query.Where(x => x.Careers.Any(y => y.CurrentAcademicYear == academicYear));

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Document.ToUpper().Contains(search.ToUpper()) || x.FullName.ToUpper().Contains(search.ToUpper()));

            var data = query
                .Select(x => new
                {
                    code = x.Document,
                    name = x.FullName,
                    careers = x.Careers.Count()
                }).ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<StudentMultipleCareersTemplate>> GetStudentsEnrolledMultipleCareersData(ClaimsPrincipal User, string userId, Guid termId, Guid facultyId, Guid careerId, Guid? academicProgramId = null, int? academicYear = null)
        {
            var students = await _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId))
                .Select(x => new
                {
                    x.User.Document,
                    x.User.FullName,
                    x.User.UserName,
                    x.CareerId,
                    CareerName = x.Career.Name,
                    x.Career.FacultyId,
                    x.AcademicProgramId,
                    x.CurrentAcademicYear,
                    x.CareerNumber,
                    Credits = x.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(y => y.Section.CourseTerm.Course.Credits)
                }).ToListAsync();

            var dbData = students
                .GroupBy(x => new { x.Document, x.FullName })
                .Where(x => x.Count() > 1)
                .Select(x => new
                {
                    x.Key.Document,
                    x.Key.FullName,
                    Careers = x.ToList()
                }).ToList();

            var query = dbData.AsQueryable();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                     .AsNoTracking();

                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => x.Careers.Any(y => careers.Contains(y.CareerId)));
            }
            else if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || User.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var deanId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == deanId || x.Faculty.SecretaryId == deanId || x.Faculty.AdministrativeAssistantId == deanId).AsNoTracking();

                if (facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => x.Careers.Any(y => careers.Contains(y.CareerId)));
            }
            else
            {
                if (facultyId != Guid.Empty) query = query.Where(x => x.Careers.Any(y => y.FacultyId == facultyId));
                if (careerId != Guid.Empty) query = query.Where(x => x.Careers.Any(y => y.CareerId == careerId));
            }

            if (academicProgramId.HasValue) query = query.Where(x => x.Careers.Any(y => y.AcademicProgramId == academicProgramId));

            if (academicYear.HasValue) query = query.Where(x => x.Careers.Any(y => y.CurrentAcademicYear == academicYear));

            var data = query
                .Select(x => new StudentMultipleCareersTemplate
                {
                    Document = x.Document,
                    FullName = x.FullName,
                    Quantity = x.Careers.Count,

                    Career1UserName = x.Careers.OrderBy(y => y.CareerNumber).First().UserName,
                    Career1Name = x.Careers.OrderBy(y => y.CareerNumber).First().CareerName,
                    Career1Credits = x.Careers.OrderBy(y => y.CareerNumber).First().Credits,
                    Career1AcademicYear = x.Careers.OrderBy(y => y.CareerNumber).First().CurrentAcademicYear,

                    Career2UserName = x.Careers.OrderBy(y => y.CareerNumber).Last().UserName,
                    Career2Name = x.Careers.OrderBy(y => y.CareerNumber).Last().CareerName,
                    Career2Credits = x.Careers.OrderBy(y => y.CareerNumber).Last().Credits,
                    Career2AcademicYear = x.Careers.OrderBy(y => y.CareerNumber).Last().CurrentAcademicYear
                }).ToList();

            return data;
        }


        public async Task<Select2Structs.ResponseParameters> GetStudentsByCareer(Select2Structs.RequestParameters requestParameters, int currentAcademicYear, Guid id, Expression<Func<Student, Select2Structs.Result>> selectPredicate = null, Func<Student, string[]> searchValuePredicate = null, string searchValue = null, int? status = null)
        {
            var test = await _context.StudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == id).Select(x => x.Student.UserId).ToListAsync();

            var query = _context.Students.Where(x => test.Any(y => y == x.UserId) && x.CareerId == id && x.CurrentAcademicYear > currentAcademicYear)
                .Include(x => x.Career)
                            .WhereSearchValue(searchValuePredicate, searchValue)
                            .AsNoTracking();


            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                }
                else
                    query = query.WhereSearchValue((x) => new[] { x.User.FullName }, searchValue);
            }
            return await query.ToSelect2(requestParameters, selectPredicate);
        }
        public async Task<Select2Structs.ResponseParameters> GetStudentsByCareerSelect2(Select2Structs.RequestParameters requestParameters, int? currentAcademicYear = null, Guid? id = null, string searchValue = null, int? status = null)
        {
            //var test = await _context.StudentSections.Where(x => x.Section.CourseTerm.Course.CareerId == id).Select(x => x.Student.UserId).ToListAsync();
            //

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var query = _context.Students
                .AsNoTracking();

            if (currentAcademicYear != null)
                query = query.Where(x => x.CurrentAcademicYear >= currentAcademicYear);

            if (id != null)
            {
                query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.Course.CareerId == id && y.Section.CourseTerm.TermId == term.Id));
            }
            else
            {
                query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == term.Id));
            }

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue));
            }

            Expression<Func<Student, Select2Structs.Result>> selectPredicate = (x) => new Select2Structs.Result
            {
                Id = x.UserId,
                Text = x.User.FullName
            };

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<Select2Structs.ResponseParameters> GetStudentsWithPendingCourse(Select2Structs.RequestParameters requestParameters, Guid courseId, string searchValue)
        {
            var query = _context.Students.Where(x => x.Curriculum.AcademicYearCourses.Any(y => y.CourseId == courseId) && !x.AcademicHistories.Any(y => y.CourseId == courseId && y.Approved)).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.User.FullName.Contains(searchValue) || x.User.UserName.Contains(searchValue));

            Expression<Func<Student, Select2Structs.Result>> selectPredicate = (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = $"{x.User.UserName} - {x.User.FullName}"
            };

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsToGraduateDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? facultyId, Guid? careerId, Guid? curriculumId, string searchValue, ClaimsPrincipal user)
        {
            if (!termId.HasValue || termId == Guid.Empty)
            {
                termId = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).Select(x => x.Id).FirstOrDefaultAsync();
            }

            var filterTerm = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            var termsByyear = await _context.Terms.OrderByDescending(x => x.Year).ThenByDescending(x => x.Number).Where(x => x.Year >= filterTerm.Year).ToArrayAsync();
            var posFilterTerm = termsByyear.IndexOf(filterTerm);
            var nextTerms = termsByyear.Take(posFilterTerm).Select(y => y.Id).ToList();

            var query =
                _context.Students
                .Where(x =>
                x.Status == ConstantHelpers.Student.States.REGULAR ||
                x.Status == ConstantHelpers.Student.States.TRANSFER ||
                x.Status == ConstantHelpers.Student.States.IRREGULAR ||
                x.Status == ConstantHelpers.Student.States.REPEATER ||
                x.Status == ConstantHelpers.Student.States.UNBEATEN ||
                x.Status == ConstantHelpers.Student.States.DESERTION ||
                x.Status == ConstantHelpers.Student.States.OBSERVED
                )
                .AsNoTracking();


            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId).Select(x => x.Id).ToListAsync();
                query = query.Where(x => careers.Contains(x.CareerId));
            }

            //Creditos Requeridos
            query = query.Where(x => x.Curriculum.RequiredCredits <= x.Curriculum.AcademicYearCourses.Where(y => !y.IsElective && x.AcademicHistories.Any(z => z.Approved && y.CourseId == z.CourseId)).Sum(y => y.Course.Credits));

            //Creditos Electivos
            query = query.Where(x => x.Curriculum.ElectiveCredits == 0 || x.Curriculum.ElectiveCredits <= x.Curriculum.AcademicYearCourses.Where(y => y.IsElective && x.AcademicHistories.Any(z => z.Approved && y.CourseId == z.CourseId)).Sum(y => y.Course.Credits));

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CurriculumId == curriculumId);

            query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId) && !x.StudentSections.Any(y => nextTerms.Contains(y.Section.CourseTerm.TermId)));

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.User.UserName.ToLower().Trim().Contains(searchValue) ||
                x.User.FullName.ToLower().Trim().Contains(searchValue));
            }

            var recordsTotal = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    career = x.Career.Name,
                })
                .ToListAsync();

            var recordsFiltered = data.Count;


            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };


        }

        public async Task DownloadExcelStudentInformationData(IXLWorksheet worksheet, Guid? careerId = null, Guid? departmentId = null, Guid? provinceId = null, Guid? districtId = null, int? sex = null, int? schoolType = null, int? universityPreparation = null, Guid? admissionTypeId = null, int? startAge = null, int? endAge = null)
        {
            const int position = 1;
            var column = 0;

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "CÓDIGO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "NOMBRE", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "APELLIDO PATERNO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "APELLIDO MATERNO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "EMAIL", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "DISTRITO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "PROVINCIA", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "DEPARTAMENTO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "DIRECCION", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "COLEGIO DE PROCEDENCIA", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "LUGAR", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIPO DE COLEGIO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "PREPARACION UNIVERSITARIA", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "PADECE ENFERMEDAD", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIPO DE ENFERMEDAD", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIENE SEGURO DE SALUD", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "SEGURO DE VIDA DESCRIPCION", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "SOSTIENE EL HOGAR", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "VIVIENDA DEL ESTUDIANTE", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "DEDICACIÓN DEL TRABAJO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TRABAJO DESCRIPCION", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "CONDICION LABORAL", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TENENCIA DE VIVIENDA", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIPO DE VIVIENDA", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIPO DE CONSTRUCCION", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "N° DE PISOS", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "N° DE DORMITORIOS", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "N° DE BAÑOS", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIENE SERVICIO DE AGUA", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIENE SERVICIO DE DESAGUE", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIENE SERVICIO DE TELÉFONO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIENE SERVICIO DE LUZ", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIENE SERVICIO DE INTERNET", column);

            worksheet.SheetView.FreezeRows(position);


            var row = 2;
            const int USERNAME = 1;
            const int NAME = 2;
            const int PATERNAL_SURNAME = 3;
            const int MATERNAL_SURNAME = 4;
            const int EMAIL = 5;


            var query = _context.Students.Include(x => x.User.District.Province.Department).Include(x => x.StudentInformation).Where(x => x.StudentInformationId != null).AsQueryable();

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (departmentId != null)
                query = query.Where(x => x.User.DepartmentId == departmentId);

            if (provinceId != null)
                query = query.Where(x => x.User.ProvinceId == provinceId);

            if (districtId != null)
                query = query.Where(x => x.User.DistrictId == districtId);

            if (sex != null)
                query = query.Where(x => x.User.Sex == sex);

            if (schoolType != null)
                query = query.Where(x => x.StudentInformation.SchoolType == schoolType);

            if (universityPreparation != null)
                query = query.Where(x => x.StudentInformation.UniversityPreparationId == universityPreparation);

            if (admissionTypeId != null)
                query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (startAge != null)
            {
                var ageBefore = DateTime.UtcNow.Year - startAge;
                query = query.Where(x => x.User.BirthDate.Year <= ageBefore);
            }

            if (endAge != null)
            {
                var ageAfter = DateTime.UtcNow.Year - endAge;
                query = query.Where(x => x.User.BirthDate.Year >= ageAfter);
            }

            var queryList = await query.ToListAsync();

            foreach (var item in queryList)
            {
                SetInformationStyle(worksheet, row, USERNAME, item.User.UserName ?? "--");
                SetInformationStyle(worksheet, row, NAME, item.User.Name ?? "--");
                SetInformationStyle(worksheet, row, PATERNAL_SURNAME, item.User.PaternalSurname ?? "--");
                SetInformationStyle(worksheet, row, MATERNAL_SURNAME, item.User.MaternalSurname ?? "--");
                SetInformationStyle(worksheet, row, EMAIL, item.User.Email ?? "--");
                SetInformationStyle(worksheet, row, 6, item.User.DistrictId.HasValue ? item.User.District.Name : "--");
                SetInformationStyle(worksheet, row, 7, item.User.ProvinceId.HasValue ? item.User.Province.Name : "--");
                SetInformationStyle(worksheet, row, 8, item.User.DepartmentId.HasValue ? item.User.Department.Name : "--");
                SetInformationStyle(worksheet, row, 9, item.StudentInformation.OriginAddress ?? "--");
                SetInformationStyle(worksheet, row, 10, item.StudentInformation.OriginSchool ?? "--");
                SetInformationStyle(worksheet, row, 11, item.StudentInformation.OriginSchoolPlace ?? "--");
                SetInformationStyle(worksheet, row, 12, ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_TYPE_SCHOOL.TYPE_SCHOOL.ContainsKey(item.StudentInformation.SchoolType) ? ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_TYPE_SCHOOL.TYPE_SCHOOL[item.StudentInformation.SchoolType] : "--");
                SetInformationStyle(worksheet, row, 13, ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_UNIVERSITY_PREPARATION.UNIVERSITY_PREPARATION.ContainsKey(item.StudentInformation.UniversityPreparationId) ? ConstantHelpers.STUDENT_INFORMATION.PERSONAL_INFORMATION.PERSONAL_INFORMATION_UNIVERSITY_PREPARATION.UNIVERSITY_PREPARATION[item.StudentInformation.UniversityPreparationId] : "--");
                SetInformationStyle(worksheet, row, 14, ConstantHelpers.STUDENT_INFORMATION.HEALTH.HEALTH_PARENT_SICK.PARENT_SICK.ContainsKey(item.StudentInformation.IsSick) ? ConstantHelpers.STUDENT_INFORMATION.HEALTH.HEALTH_PARENT_SICK.PARENT_SICK[item.StudentInformation.IsSick] : "--");
                SetInformationStyle(worksheet, row, 15, item.StudentInformation.TypeParentIllness ?? "--");
                SetInformationStyle(worksheet, row, 16, ConstantHelpers.STUDENT_INFORMATION.HEALTH.TYPE_INSURANCE.VALUES.ContainsKey(item.StudentInformation.HaveInsurance) ? ConstantHelpers.STUDENT_INFORMATION.HEALTH.TYPE_INSURANCE.VALUES[item.StudentInformation.HaveInsurance] : "--");
                SetInformationStyle(worksheet, row, 17, ConstantHelpers.STUDENT_INFORMATION.HEALTH.TYPE_INSURANCE.VALUES.ContainsKey(item.StudentInformation.InsuranceDescription) ? ConstantHelpers.STUDENT_INFORMATION.HEALTH.TYPE_INSURANCE.VALUES[item.StudentInformation.InsuranceDescription] : "--");
                SetInformationStyle(worksheet, row, 18, ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_PRINCIPAL_PERSON.PRINCIPAL_PERSON.ContainsKey(item.StudentInformation.PrincipalPerson) ? ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_PRINCIPAL_PERSON.PRINCIPAL_PERSON[item.StudentInformation.PrincipalPerson] : "--");
                SetInformationStyle(worksheet, row, 19, ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_COEXISTENCE.STUDENT_COEXISTENCE.ContainsKey(item.StudentInformation.StudentCoexistence) ? ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_STUDENT_COEXISTENCE.STUDENT_COEXISTENCE[item.StudentInformation.StudentCoexistence] : "--");
                SetInformationStyle(worksheet, row, 20, ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_WORK_DEDICATION.VALUES.ContainsKey(item.StudentInformation.StudentWorkDedication) ? ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_WORK_DEDICATION.VALUES[item.StudentInformation.StudentWorkDedication] : "--");
                SetInformationStyle(worksheet, row, 21, item.StudentInformation.StudentWorkDescription ?? "--");
                SetInformationStyle(worksheet, row, 22, ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_WORK_CONDITION.VALUES.ContainsKey(item.StudentInformation.StudentWorkCondition) ? ConstantHelpers.STUDENT_INFORMATION.ECONOMY.ECONOMY_WORK_CONDITION.VALUES[item.StudentInformation.StudentWorkCondition] : "--");
                SetInformationStyle(worksheet, row, 23, ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVING_PLACE_TENURE.TENURE.ContainsKey(item.StudentInformation.Tenure) ? ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVING_PLACE_TENURE.TENURE[item.StudentInformation.Tenure] : "--");
                SetInformationStyle(worksheet, row, 24, ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVING_PLACE_TYPE.TYPE.ContainsKey(item.StudentInformation.ZoneType) ? ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVING_PLACE_TYPE.TYPE[item.StudentInformation.ZoneType] : "--");
                SetInformationStyle(worksheet, row, 25, ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVINGPLACE_CONSTRUCTION_TYPE.CONSTRUCTION_TYPE.ContainsKey(item.StudentInformation.ContructionType) ? ConstantHelpers.STUDENT_INFORMATION.LIVING_PLACE.LIVINGPLACE_CONSTRUCTION_TYPE.CONSTRUCTION_TYPE[item.StudentInformation.ContructionType] : "--");
                SetInformationStyle(worksheet, row, 26, item.StudentInformation.NumberFloors.ToString());
                SetInformationStyle(worksheet, row, 27, item.StudentInformation.NumberRooms.ToString());
                SetInformationStyle(worksheet, row, 28, item.StudentInformation.NumberBathroom.ToString());
                SetInformationStyle(worksheet, row, 29, item.StudentInformation.Water ? "Si" : "No");
                SetInformationStyle(worksheet, row, 30, item.StudentInformation.Drain ? "Si" : "No");
                SetInformationStyle(worksheet, row, 31, item.StudentInformation.LivingPlacePhone ? "Si" : "No");
                SetInformationStyle(worksheet, row, 32, item.StudentInformation.Light ? "Si" : "No");
                SetInformationStyle(worksheet, row, 33, item.StudentInformation.Internet ? "Si" : "No");

                row++;
            }
        }



        public async Task DownloadExcelStudentFamilyInformationData(IXLWorksheet worksheet, Guid? careerId = null, Guid? departmentId = null, Guid? provinceId = null, Guid? districtId = null, int? sex = null, int? schoolType = null, int? universityPreparation = null, Guid? admissionTypeId = null, int? startAge = null, int? endAge = null)
        {
            const int position = 1;
            var column = 0;

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "CÓDIGO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "NOMBRE", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "PARENTESCO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "NOMBRE DEL FAMILIAR", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "APELLIDO PATERNO DEL FAMILIAR", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "APELLIDO MATERNO DEL FAMILIAR", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "F. NACIMIENTO", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "ESTADO CIVIL", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "GRADO DE INSTRUCCIÓN", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TITULADO / MAESTRIA", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "OCUPACIÓN", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "CENTRO LABORAL Y/O ESTUDIOS", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "LOCALIDAD", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "¿ESTA ENFERMO?", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "TIPO DE ENFERMEDAD", column);

            worksheet.Column(++column).Width = 25;
            SetHeaderRowStyle2(worksheet, "¿TIENE INTERVENCIONES QUIRÚRGICAS?", column);

            worksheet.SheetView.FreezeRows(position);


            var row = 2;

            var query = _context.Students.Include(x => x.StudentFamilies).Include(x => x.User.District.Province.Department).Include(x => x.StudentInformation).Where(x => x.StudentInformationId != null).AsQueryable();

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (departmentId != null)
                query = query.Where(x => x.User.DepartmentId == departmentId);

            if (provinceId != null)
                query = query.Where(x => x.User.ProvinceId == provinceId);

            if (districtId != null)
                query = query.Where(x => x.User.DistrictId == districtId);

            if (sex != null)
                query = query.Where(x => x.User.Sex == sex);

            if (schoolType != null)
                query = query.Where(x => x.StudentInformation.SchoolType == schoolType);

            if (universityPreparation != null)
                query = query.Where(x => x.StudentInformation.UniversityPreparationId == universityPreparation);

            if (admissionTypeId != null)
                query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (startAge != null)
            {
                var ageBefore = DateTime.UtcNow.Year - startAge;
                query = query.Where(x => x.User.BirthDate.Year <= ageBefore);
            }

            if (endAge != null)
            {
                var ageAfter = DateTime.UtcNow.Year - endAge;
                query = query.Where(x => x.User.BirthDate.Year >= ageAfter);
            }

            var queryList = await query.ToListAsync();

            foreach (var item in queryList)
            {
                if (item.StudentFamilies != null && item.StudentFamilies.Count > 0)
                {
                    foreach (var sf in item.StudentFamilies)
                    {
                        SetInformationStyle(worksheet, row, 1, item.User.UserName ?? "--");
                        SetInformationStyle(worksheet, row, 2, item.User.FullName ?? "--");
                        SetInformationStyle(worksheet, row, 3, ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.TYPE.ContainsKey(sf.RelationshipInt) ? ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.TYPE[sf.RelationshipInt] : "--");
                        SetInformationStyle(worksheet, row, 4, sf.Name ?? "--");
                        SetInformationStyle(worksheet, row, 5, sf.PaternalName ?? "--");
                        SetInformationStyle(worksheet, row, 6, sf.MaternalName ?? "--");
                        SetInformationStyle(worksheet, row, 7, sf.Birthday != null ? sf.Birthday.ToLocalDateFormat() : "--");
                        SetInformationStyle(worksheet, row, 8, ConstantHelpers.CIVIL_STATUS.VALUES.ContainsKey(sf.CivilStatusInt) ? ConstantHelpers.CIVIL_STATUS.VALUES[sf.CivilStatusInt] : "--");
                        SetInformationStyle(worksheet, row, 9, ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.TYPE.ContainsKey(sf.DegreeInstructionInt) ? ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.TYPE[sf.DegreeInstructionInt] : "--");
                        SetInformationStyle(worksheet, row, 10, sf.Certificated ?? "--");
                        SetInformationStyle(worksheet, row, 11, sf.Occupation ?? "--");
                        SetInformationStyle(worksheet, row, 12, sf.WorkCenter ?? "--");
                        SetInformationStyle(worksheet, row, 13, sf.Location ?? "--");
                        SetInformationStyle(worksheet, row, 14, sf.IsSick ? "Si" : "No");
                        SetInformationStyle(worksheet, row, 15, sf.DiseaseType ?? "--");
                        SetInformationStyle(worksheet, row, 16, sf.SurgicalIntervention ? "Si" : "No");
                        row++;
                    }

                }


            }
        }

        public async Task<decimal> GetRequiredApprovedCredits(Guid studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();
            var query = _context.AcademicYearCourses.AsNoTracking();

            var result = await query.Where(x => x.CurriculumId == student.CurriculumId && !x.IsElective && x.Course.AcademicHistories.Any(y => y.StudentId == studentId && y.Approved)).SumAsync(y => y.Course.Credits);
            return result;
        }

        public async Task<decimal> GetElectiveApprovedCredits(Guid studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();
            var query = _context.AcademicYearCourses.AsNoTracking();

            var courses = await query.Where(x => x.IsElective && x.CurriculumId == student.CurriculumId)
                .Select(x => x.CourseId)
                .ToListAsync();

            var result = await query.Where(x => x.CurriculumId == student.CurriculumId && x.IsElective && x.Course.AcademicHistories.Any(y => y.StudentId == studentId && y.Approved)).SumAsync(y => y.Course.Credits);

            return result;
        }

        public async Task<int> CalculateEnrollmentAcademicYear(Guid studentId, Guid termId)
        {
            var student = await _context.Students.FindAsync(studentId);
            var term = await _context.Terms.FindAsync(termId);

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == studentId && x.Term.StartDate < term.StartDate)
                .Select(x => new
                {
                    x.CourseId,
                    x.Course.Credits,
                    TermNumber = x.Term.Number,
                    TermYear = x.Term.Year,
                    x.Approved
                }).ToListAsync();

            var academicYearCourses = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId)
                .Select(x => x.CourseId)
                .ToListAsync();

            var totalApprovedCredits = academicHistories
                         .Where(x => academicYearCourses.Contains(x.CourseId) && x.Approved)
                         .Distinct()
                         .Sum(x => x.Credits);

            var aditionalCredits = 0.0M;

            if (await _context.StudentSections.AnyAsync(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId))
            {
                aditionalCredits = await _context.StudentSections
                    .Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == termId)
                    .SumAsync(x => x.Section.CourseTerm.Course.Credits);
            }
            else
            {
                var summaries = academicHistories
                         .Where(x => x.TermNumber == "1" || x.TermNumber == "2")
                         .GroupBy(x => new { x.TermYear, x.TermNumber })
                         .Select(x => new
                         {
                             Year = x.Key.TermYear,
                             Number = x.Key.TermNumber,
                             Courses = x.ToList()
                         }).ToList();
                var lastSummary = summaries.OrderBy(x => x.Year).ThenBy(x => x.Number).LastOrDefault();
                var disapprovedCredits = lastSummary == null ? 0
                    : lastSummary.Courses.Where(x => !academicHistories.Any(y => y.CourseId == x.CourseId && y.Approved)).Sum(x => x.Credits);

                var curriculumCredit = await _context.CurriculumCredits.FirstOrDefaultAsync(x => x.CurriculumId == student.CurriculumId);
                if (curriculumCredit == null) return -1;

                aditionalCredits = curriculumCredit.CreditsDisapproved <= disapprovedCredits ? curriculumCredit.CreditsObservation : curriculumCredit.MaxCredits;
            }

            var totalCredits = totalApprovedCredits + aditionalCredits;

            var academicYearCredits = await _context.AcademicYearCredits
                .Where(x => x.CurriculumId == student.CurriculumId)
                .ToListAsync();

            var academicYearCredit = academicYearCredits
                .FirstOrDefault(x => x.CurriculumId == student.CurriculumId
                && x.StartCredits <= totalCredits && totalCredits <= x.EndCredits);

            if (academicYearCredit == null)
            {
                if (academicYearCredits.Any())
                    academicYearCredit = academicYearCredits.Where(x => x.CurriculumId == student.CurriculumId).OrderBy(x => x.AcademicYear).LastOrDefault();
                else
                    return -1;
            }

            return academicYearCredit.AcademicYear;
        }

        public async Task<List<DateTime>> GetStudentsBirthData(Guid termId, Guid? careerId, ClaimsPrincipal user = null)
        {
            if (termId == Guid.Empty)
                termId = (await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE)).Id;

            var qryStudents = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        qryStudents = qryStudents.Where(x => x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId);
                    }
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        qryStudents = qryStudents.Where(x => x.Career.CareerDirectorId == userId || x.Career.AcademicCoordinatorId == userId);
                    }
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                qryStudents = qryStudents.Where(x => x.CareerId == careerId);

            var data = await qryStudents
                .Select(x => x.User.BirthDate)
                .ToListAsync();

            return data;
        }

        public async Task<object> GetTotalNumberOfStudentsEnrolledByCareerAndTermIdChart(Guid? termId = null, ClaimsPrincipal user = null)
        {
            var query = _context.StudentSections.AsNoTracking();

            var careersQuery = _context.Careers.AsNoTracking();

            if (termId != null)
                query = query.Where(x => x.Section.CourseTerm.TermId == termId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.Student.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            var result = await query
                 .Select(x => new
                 {
                     x.StudentId,
                     CareerName = x.Student.Career.Name,
                     x.Student.CareerId,
                     x.Section.CourseTerm.TermId,
                     TermName = x.Section.CourseTerm.Term.Name
                 })
                 .Distinct()
                 .GroupBy(x => new { x.StudentId, x.CareerName, x.CareerId })
                 .Select(x => new
                 {
                     Name = x.Key.CareerName,
                     y = x.Count()
                 }).ToListAsync();


            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReEnrolledStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId, Guid? careerId, ClaimsPrincipal user, string search = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "4":
                    if (term.Status == ConstantHelpers.TERM_STATES.FINISHED) orderByPredicate = (x) => !x.AcademicSummaries.Where(y => y.TermId == termId).Any() ? x.CurrentAcademicYear : x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear);
                    else orderByPredicate = (x) => x.CurrentAcademicYear;
                    break;
                case "5":
                    orderByPredicate = (x) => x.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits);
                    break;
                default:
                    break;
            }

            var prevRegularTerm = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED
                && (x.Number == "1" || x.Number == "2") && x.StartDate < term.StartDate)
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number)
                .FirstOrDefaultAsync();

            var query = _context.Students
                .Where(x => prevRegularTerm != null && x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == term.Id)
                && !x.StudentSections.Any(y => y.Section.CourseTerm.TermId == prevRegularTerm.Id)
                && x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId != term.Id)
                )
                .AsNoTracking();

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                     .AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);
                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId || x.Faculty.AdministrativeAssistantId == userId).AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);
                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty)
                    query = query.Where(x => x.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty)
                    query = query.Where(x => x.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                    || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    faculty = s.Career.Faculty.Name,
                    academicYear = term.Status == ConstantHelpers.TERM_STATES.FINISHED ?
                        s.AcademicSummaries.Where(y => y.TermId == termId).Any() ? s.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) : s.CurrentAcademicYear
                        : s.CurrentAcademicYear,
                    credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    id = s.Id,
                    curriculum = s.Curriculum.Code
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGraduatedPostulantDatatable(DataTablesStructs.SentParameters sentParameters, int state, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var postulants = _context.Postulants
                .Where(x => x.AdmissionState == ConstantHelpers.POSTULANTS.ApplicationStatus.ADMITTED)
                .AsNoTracking();

            var students = _context.Students
                .Where(x => x.Status == state && x.GraduationTermId != null)
                .AsNoTracking();

            var query = _context.Terms.AsNoTracking();

            if (termId != null)
                query = query.Where(x => x.Id == termId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    postulants = postulants.Where(x => x.Career.QualityCoordinatorId == userId);
                    students = students.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            var recordsTotal = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Number)
                .Select(x => new
                {
                    Term = x.Name,
                    GraduatedQualified = students.Where(y => y.GraduationTermId == x.Id).Count(),
                    Entrant = postulants.Where(y => y.ApplicationTerm.TermId == x.Id).Count()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsFiltered = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetGraduatedPostulantChart(int state, Guid? termId = null, ClaimsPrincipal user = null)
        {
            var postulants = _context.Postulants
                            .Where(x => x.AdmissionState == ConstantHelpers.POSTULANTS.ApplicationStatus.ADMITTED)
                            .AsNoTracking();

            var students = _context.Students
                .Where(x => x.Status == state && x.GraduationTermId != null)
                .AsNoTracking();

            var query = _context.Terms.AsNoTracking();

            if (termId != null)
                query = query.Where(x => x.Id == termId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    postulants = postulants.Where(x => x.Career.QualityCoordinatorId == userId);
                    students = students.Where(x => x.Career.QualityCoordinatorId == userId);
                }
            }

            var data = await query
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Number)
                .Select(x => new
                {
                    Term = x.Name,
                    GraduatedQualified = students.Where(y => y.GraduationTermId == x.Id).Count(),
                    Entrant = postulants.Where(y => y.ApplicationTerm.TermId == x.Id).Count()
                })
                .ToListAsync();

            var result = new
            {
                categories = data.Select(x => x.Term).ToList(),
                data = data.Select(x => x.Entrant == 0 ? 0.0 :
                    Math.Round((x.GraduatedQualified * 100.0) / (x.Entrant * 1.0), 2, MidpointRounding.AwayFromZero)).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUserLoginStudentsDatatable(DataTablesStructs.SentParameters sentParameters, byte system, byte roleType, string startDate = null, string endDate = null, string search = null)
        {
            var query = _context.Students.AsNoTracking();

            if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
            {
                var dateStartDateTime = ConvertHelpers.DatepickerToUtcDateTime(startDate);
                var dateEndDateTime = ConvertHelpers.DatepickerToUtcDateTime(endDate);

                query = query.Where(x => x.User.UserLogins.Any(s => s.System == system && dateStartDateTime.Date <= s.LastLogin.Date && s.LastLogin.Date <= dateEndDateTime.Date));
            }
            else
            {
                query = query.Where(x => x.User.UserLogins.Any(s => s.System == system));
            }

            switch (roleType)
            {
                case 1:
                    query = query.Where(x => x.Status != ConstantHelpers.Student.States.GRADUATED && x.Status != ConstantHelpers.Student.States.BACHELOR && x.Status != ConstantHelpers.Student.States.QUALIFIED);
                    break;
                case 2:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED || x.Status == ConstantHelpers.Student.States.BACHELOR || x.Status == ConstantHelpers.Student.States.QUALIFIED);
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper())
                    || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();


            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    ip = s.User.UserLogins.FirstOrDefault().Ip,
                    firstLoginStr = s.User.UserLogins.FirstOrDefault().FirstLogin.ToLocalDateFormat(),
                    lastLogin = s.User.UserLogins.FirstOrDefault().FirstLogin,
                    lastLoginStr = s.User.UserLogins.FirstOrDefault().LastLogin.ToLocalDateFormat(),
                }).OrderByDescending(s => s.lastLogin).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetHomeLoginsDatatable(DataTablesStructs.SentParameters sentParameters, byte? system = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.UserLogins.FirstOrDefault().LastLogin);
                    break;
            }

            var query = _context.Students.AsNoTracking();

            if (system != null)
            {
                query = query.Where(x => x.User.UserLogins.Any(s => s.System == system));
            }

            var recordsFiltered = await query.CountAsync();


            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    lastLogin = s.User.UserLogins.FirstOrDefault().LastLogin,
                    lastLoginStr = s.User.UserLogins.FirstOrDefault().LastLogin.ToLocalDateFormat(),
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
        public async Task<List<UserLoginStudentTemplate>> GetUserLoginStudentsTemplate(byte system, byte roleType, string startDate = null, string endDate = null)
        {
            var query = _context.Students.AsNoTracking();

            if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
            {
                var dateStartDateTime = ConvertHelpers.DatepickerToUtcDateTime(startDate);
                var dateEndDateTime = ConvertHelpers.DatepickerToUtcDateTime(endDate);

                query = query.Where(x => x.User.UserLogins.Any(s => s.System == system && dateStartDateTime.Date <= s.LastLogin.Date && s.LastLogin.Date <= dateEndDateTime.Date));
            }
            else
            {
                query = query.Where(x => x.User.UserLogins.Any(s => s.System == system));
            }

            switch (roleType)
            {
                case 1:
                    query = query.Where(x => x.Status != ConstantHelpers.Student.States.GRADUATED && x.Status != ConstantHelpers.Student.States.BACHELOR && x.Status != ConstantHelpers.Student.States.QUALIFIED);
                    break;
                case 2:
                    query = query.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED || x.Status == ConstantHelpers.Student.States.BACHELOR || x.Status == ConstantHelpers.Student.States.QUALIFIED);
                    break;
                default:
                    break;
            }

            var data = await query
                .Select(s => new UserLoginStudentTemplate
                {
                    UserName = s.User.UserName,
                    FullName = s.User.FullName,
                    Career = s.Career.Name,
                    FirstLoginStr = s.User.UserLogins.FirstOrDefault().FirstLogin.ToLocalDateFormat(),
                    FirstLogin = s.User.UserLogins.FirstOrDefault().FirstLogin,
                    LastLogin = s.User.UserLogins.FirstOrDefault().LastLogin,
                    LastLoginStr = s.User.UserLogins.FirstOrDefault().LastLogin.ToLocalDateFormat(),
                }).OrderByDescending(s => s.LastLogin).ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalWelfareStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? facultyId = null, Guid? careerId = null, Guid? admissionTypeId = null, string search = null)
        {
            var query = _context.Students
                .Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (admissionTypeId != null)
                query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (!string.IsNullOrEmpty(search))
            {
                var trimSearch = search.Trim();
                query = query.Where(x => x.User.UserName.ToUpper().Contains(trimSearch.ToUpper())
                        || x.User.FullName.ToUpper().Contains(trimSearch.ToUpper())
                        || x.User.PaternalSurname.ToUpper().Contains(trimSearch.ToUpper())
                        || x.User.MaternalSurname.ToUpper().Contains(trimSearch.ToUpper())
                        || x.User.Name.ToUpper().Contains(trimSearch.ToUpper()));
            }


            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    names = x.User.Name,
                    paternalSurname = x.User.PaternalSurname,
                    maternalSurname = x.User.MaternalSurname,
                    userName = x.User.UserName,
                    fullname = x.User.FullName,
                    email = x.User.Email,
                    faculty = x.Career.Faculty.Name,
                    career = x.Career.Name,
                    phoneNumber = x.User.PhoneNumber,
                    hasStudentInformationTerm = x.StudentInformations.Any(y => y.TermId == termId && y.StudentId == x.Id)
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<bool> IsSanctionedStudentValidToStudy(Guid studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null || student.Status != ConstantHelpers.Student.States.SANCTIONED)
                return false;

            var lastTerm = await _context.AcademicSummaries
                .Where(x => x.StudentId == studentId)
                .OrderByDescending(x => x.Term.StartDate)
                .Select(x => new
                {
                    x.TermId,
                    x.Term.StartDate,
                    x.Term.EndDate
                }).FirstOrDefaultAsync();

            var termsBetween = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED
                && !x.IsSummer && (x.Number == "1" || x.Number == "2")
                && lastTerm.StartDate < x.StartDate
                && x.StartDate <= DateTime.UtcNow)
                .CountAsync();

            return termsBetween >= 2;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsWithAverageFinalGrades(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid facultyId, int? academicYear = null, Guid? careerId = null, string search = null, ClaimsPrincipal user = null)
        {
            //Por AcademicSummaries
            Expression<Func<StudentSummariesTemplate, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Career;
                    break;
                case "5":
                    orderByPredicate = (x) => x.WeightedAverageGrade;
                    break;
                case "6":
                    orderByPredicate = (x) => x.ArithmeticAverageGrade;
                    break;
            }

            var query = _context.AcademicSummaries
                .Where(x => x.TermId == termId && x.Career.FacultyId == facultyId)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {

                    query = query.Where(x => x.Career.CareerDirectorId == userId || x.Career.AcademicSecretaryId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR_GENERAL))
                {
                    var maxAcademicYear = Convert.ToInt16(await GetConfigurationValue(ConstantHelpers.Configuration.TeacherManagement.CAREER_DIRECTOR_GENERAL_ACADEMIC_YEAR));
                    query = query.Where(x => x.Student.CurrentAcademicYear <= maxAcademicYear);
                }
            }

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (academicYear != null)
                query = query.Where(x => x.StudentAcademicYear == academicYear);

            if (!string.IsNullOrEmpty(search))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    search = $"\"{search}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.Student.User.FullName, search) || EF.Functions.Contains(x.Student.User.UserName, search));
                }
                else
                    query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(search.ToUpper()) ||
                                        x.Student.User.UserName.ToUpper().Contains(search.ToUpper()) ||
                                        x.Student.User.PaternalSurname.ToUpper().Contains(search.ToUpper()) ||
                                        x.Student.User.MaternalSurname.ToUpper().Contains(search.ToUpper()) ||
                                        x.Student.User.Name.ToUpper().Contains(search.ToUpper()));
            }


            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new StudentSummariesTemplate
                {
                    UserName = x.Student.User.UserName,
                    PaternalSurname = x.Student.User.PaternalSurname,
                    MaternalSurname = x.Student.User.MaternalSurname,
                    Name = x.Student.User.Name,
                    Career = x.Career.Name,
                    //Promedio ponderado, cuenta pesos de creditos
                    WeightedAverageGrade = x.WeightedAverageGrade,
                    //Promedio aritmetico o promedio simple, no cuenta los pesos de los creditos
                    ArithmeticAverageGrade = Math.Round((x.Student.AcademicHistories.Where(y => y.TermId == termId && y.Withdraw == false).Sum(y => y.Grade) * 1.0M / x.Student.AcademicHistories.Where(y => y.TermId == termId && y.Withdraw == false).Count() * 1.0M), 2, MidpointRounding.AwayFromZero)
                })
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<object> GetAllSelectByStatus(int? status = null)
        {
            var query = _context.Students.AsNoTracking();

            if (status != null)
            {
                query = query.Where(x => x.Status == status);
            }

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.User.FullName
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAverageTimeToFinishByCareerAndTermDatatable(DataTablesStructs.SentParameters sentParameters, Guid graduationTermId, Guid? careerId = null)
        {
            //Expression<Func<Student, dynamic>> orderByPredicate = null;

            //switch (sentParameters.OrderColumn)
            //{
            //    case "0":
            //        orderByPredicate = (x) => x.User.UserName;
            //        break;
            //    case "1":
            //        orderByPredicate = (x) => x.User.PaternalSurname;
            //        break;
            //    case "2":
            //        orderByPredicate = (x) => x.User.MaternalSurname;
            //        break;
            //    case "3":
            //        orderByPredicate = (x) => x.User.Name;
            //        break;
            //    case "4":
            //        orderByPredicate = (x) => x.Career.Name;
            //        break;
            //}


            var query = _context.Students
                .Where(x => x.GraduationTermId == graduationTermId)
                .AsNoTracking();

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            var preData = await query
                .Select(x => new
                {
                    studentTerms = x.AcademicSummaries.Where(y => y.CareerId == x.CareerId && y.CurriculumId == x.CurriculumId).Count(),
                    curriculumId = x.CurriculumId,
                    curriculumAcademicYears = x.Curriculum.AcademicYearCourses.Where(y => y.CurriculumId == x.CurriculumId).Select(x => x.AcademicYear).Distinct().Count(),
                    curriculumCode = x.Curriculum.Code,
                    careerId = x.CareerId,
                    careerName = x.Career.Name,
                    careerCode = x.Career.Code,
                    graduationTermId = x.GraduationTermId,
                    graduationTermName = x.GraduationTerm.Name
                })
                .GroupBy(x => new { x.careerId, x.careerName, x.careerCode, x.graduationTermId, x.graduationTermName, x.curriculumId, x.curriculumCode, x.curriculumAcademicYears })
                .Select(x => new
                {
                    studentTerms = x.Average(y => y.studentTerms),
                    curriculumId = x.Key.curriculumId,
                    curriculumCode = x.Key.curriculumCode,
                    curriculumAcademicYears = x.Key.curriculumAcademicYears,
                    careerId = x.Key.careerId,
                    careerName = x.Key.careerName,
                    careerCode = x.Key.careerCode,
                    graduationTermId = x.Key.graduationTermId,
                    graduationTermName = x.Key.graduationTermName
                })
                .ToListAsync();

            var recordsTotal = preData.Count();

            var data = preData
                .OrderByDescending(x => x.careerName)
                .ThenByDescending(x => x.curriculumCode)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsFiltered = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetFutureGraduatedStudentDatatable(DataTablesStructs.SentParameters parameters, Guid? careerId, Guid? curriculumId, string searchValue, ClaimsPrincipal user)
        {
            var query = _context.Students
                .Where(x =>
                x.Status == ConstantHelpers.Student.States.REGULAR ||
                x.Status == ConstantHelpers.Student.States.TRANSFER ||
                x.Status == ConstantHelpers.Student.States.IRREGULAR ||
                x.Status == ConstantHelpers.Student.States.REPEATER ||
                x.Status == ConstantHelpers.Student.States.UNBEATEN ||
                x.Status == ConstantHelpers.Student.States.DESERTION ||
                x.Status == ConstantHelpers.Student.States.OBSERVED
                )
                .AsNoTracking();

            query = query.Where(x => x.Curriculum.RequiredCredits <= (x.AcademicHistories.Where(y => y.Approved && y.Course.AcademicYearCourses.Any(z => z.CurriculumId == x.CurriculumId)
            ).Sum(y => y.Course.Credits) + x.StudentSections.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS).Sum(y => y.Section.CourseTerm.Course.Credits)));

            query = query.Where(x => !x.DisapprovedCourses.Any());

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CurriculumId == curriculumId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.Trim().ToLower();
                query = query.Where(x => x.User.UserName.ToLower().Trim().Contains(searchValue) ||
                x.User.FullName.ToLower().Trim().Contains(searchValue));
            }

            var recordsTotal = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    career = x.Career.Name,
                })
                .ToListAsync();

            var recordsFiltered = data.Count;


            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };


        }

        public async Task<List<FutureGradutedStudentTemplate>> GetFutureGraduatedStudentsTemplate(Guid? careerId, Guid? curriculumId, ClaimsPrincipal user)
        {
            var query = _context.Students
               .Where(x =>
               x.Status == ConstantHelpers.Student.States.REGULAR ||
               x.Status == ConstantHelpers.Student.States.TRANSFER ||
               x.Status == ConstantHelpers.Student.States.IRREGULAR ||
               x.Status == ConstantHelpers.Student.States.REPEATER ||
               x.Status == ConstantHelpers.Student.States.UNBEATEN ||
               x.Status == ConstantHelpers.Student.States.DESERTION ||
               x.Status == ConstantHelpers.Student.States.OBSERVED
               )
               .AsNoTracking();

            query = query.Where(x => x.Curriculum.RequiredCredits <= (x.AcademicHistories.Where(y => y.Approved && y.Course.AcademicYearCourses.Any(z => z.CurriculumId == x.CurriculumId)
            ).Sum(y => y.Course.Credits) + x.StudentSections.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS).Sum(y => y.Section.CourseTerm.Course.Credits)));

            query = query.Where(x => !x.DisapprovedCourses.Any());

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (curriculumId.HasValue && curriculumId != Guid.Empty)
                query = query.Where(x => x.CurriculumId == curriculumId);

            var recordsTotal = await query.CountAsync();

            var data = await query
                .Select(x => new FutureGradutedStudentTemplate
                {
                    Id = x.Id,
                    UserName = x.User.UserName,
                    FullName = x.User.FullName,
                    Career = x.Career.Name,
                    ApprovedCredits = x.AcademicHistories.Where(y => y.Approved && y.Course.AcademicYearCourses.Any(z => z.CurriculumId == x.CurriculumId)).Sum(y => y.Course.Credits),
                    EnrolledCredits = x.StudentSections.Where(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS).Sum(y => y.Section.CourseTerm.Course.Credits)
                })
                .ToListAsync();

            return data;
        }

        public async Task<Term> GetLastTermEnrolled(Guid studentId)
            => await _context.AcademicSummaries.Where(y => y.StudentId == studentId).OrderByDescending(x => x.Term.Year).ThenByDescending(x => x.Term.Number).Select(y => y.Term).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsManagementDatatable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, string searchValue = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.Document);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.User.PhoneNumber);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.User.UserLogins.Select(y => y.LastLogin));
                    break;
                default:
                    break;
            }

            var query = _context.Students
               .AsNoTracking();

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(q => q.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(q => q.CareerId == careerId);

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                query = query.Where(q => q.AcademicProgramId == academicProgramId);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue)
                    || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.Contains(searchValue)
                    || x.User.UserName.Contains(searchValue) || x.User.Document.Contains(searchValue));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    picture = x.User.Picture,
                    username = x.User.UserName,
                    fullname = x.User.FullName,
                    document = x.User.Document,
                    career = x.Career.Name,
                    lastLogin = x.User.UserLogins.Any() ? x.User.UserLogins.OrderByDescending(y => y.LastLogin).Select(y => y.LastLogin.ToLocalDateFormat()).FirstOrDefault()
                    : "-"
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<object> GetWithdrawnStudentsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? termId, string search)
        {
            Expression<Func<StudentObservation, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.Student.User.UserName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Student.User.FullName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Student.Career.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                default:
                    orderByPredicate = ((x) => x.Student.User.UserName);
                    break;
            }

            var query = _context.StudentObservations
                .Where(x => x.Type == ConstantHelpers.OBSERVATION_TYPES.WITHDRAWN)
               .AsNoTracking();

            if (termId.HasValue && termId != Guid.Empty)
                query = query.Where(x => x.TermId == termId);


            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                query = query.Where(x => x.Student.User.FullName.ToLower().Contains(search) || x.Student.User.UserName.ToLower().Contains(search));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.StudentId,
                    createdAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    career = x.Student.Career.Name
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<WithdrawnStudentTemplate>> GetWithdrawnStudentsTemplate(Guid? termId)
        {

            var query = _context.StudentObservations
                .Where(x => x.Type == ConstantHelpers.OBSERVATION_TYPES.WITHDRAWN)
               .AsNoTracking();

            if (termId.HasValue && termId != Guid.Empty)
                query = query.Where(x => x.TermId == termId);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new WithdrawnStudentTemplate
                {
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    UserName = x.Student.User.UserName,
                    FullName = x.Student.User.FullName,
                    Career = x.Student.Career.Name
                }).ToListAsync();

            return data;
        }

        public async Task<StudentGraduatedSurveyInformation> GetStudentGraduatedSurveyInformation(string userId)
        {
            var result = await _context.Students
                .Where(x => x.UserId == userId)
                .Select(x => new StudentGraduatedSurveyInformation
                {
                    FullName = x.User.FullName,
                    Faculty = x.Career.Faculty.Name,
                    Career = x.Career.Name,
                    StudentPhoneNumber = x.User.PhoneNumber,
                    Dni = x.User.Dni,
                    GraduatedTerm = x.GraduationTermId == null ? "" : x.GraduationTerm.Name,
                    StudentAddressUbigeo = "",
                    StudentCurrentAddress = x.User.Address,
                    StudentEmail = x.User.Email,
                    HasGraduatedSurveyCompleted = x.HasGraduatedSurveyCompleted
                    //Datos del API?
                })
                .FirstOrDefaultAsync();

            return result;
        }

        //Para bolsita no editar loquitos
        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentByStatusDatatable(DataTablesStructs.SentParameters sentParameters, List<int> status, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, List<Guid> careers = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.Dni);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.PaternalSurname);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.MaternalSurname);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.AdmissionTerm.Name);
                    break;
                case "6":
                    orderByPredicate = ((x) => x.GraduationTerm.Name);
                    break;
                case "7":
                    orderByPredicate = ((x) => x.User.PhoneNumber);
                    break;
            }

            var query = _context.Students
                .Where(x => status.Any(y => y == x.Status))
                .AsNoTracking();

            if (facultyId != null)
            {
                query = query.Where(x => x.Career.FacultyId == facultyId);
            }

            if (careerId != null)
            {
                query = query.Where(x => x.CareerId == careerId);
            }

            if (careers != null)
            {
                query = query.Where(x => careers.Contains(x.CareerId));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchTrim = searchValue.Trim();
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchTrim}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue)
                    || EF.Functions.Contains(x.User.UserName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.Contains(searchTrim)
                        || x.User.UserName.Contains(searchTrim)
                        || x.User.Document.Contains(searchTrim));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.User.UserName,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    x.User.Name,
                    x.User.Dni,
                    AdmissionTerm = x.AdmissionTerm.Name,
                    GraduationTerm = x.GraduationTerm.Name,
                    x.User.PhoneNumber
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDegreeReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null)
        {
            var query = _context.Students
                .Where(x => x.GraduationTermId != null)
                .AsNoTracking();

            var careersQuery = _context.Careers.AsNoTracking();

            if (careers.Count > 0 && !careers.Contains(Guid.Empty))
            {
                query = query.Where(x => careers.Contains(x.CareerId));
                careersQuery = careersQuery.Where(x => careers.Contains(x.Id));
            }

            if (graduatedYear != null)
            {
                query = query.Where(x => x.GraduationTerm.Year == graduatedYear);
            }

            if (graduatedTermId != null)
            {
                query = query.Where(x => x.GraduationTermId == graduatedTermId);
            }

            //Total de estudiantes por escuela
            var studentData = await _context.Students
                .GroupBy(x => new { x.CareerId })
                .Select(x => new
                {
                    x.Key.CareerId,
                    Total = x.Count()
                })
                .ToListAsync();

            var queryData = await query
                .Select(x => new
                {
                    x.CareerId,
                    CareerName = x.Career.Name,
                    x.Status
                })
                .OrderBy(x => x.CareerName)
                .ToListAsync();

            var careersDb = await careersQuery
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            var data = careersDb
                .Select(x => new
                {
                    CareerId = x.Id,
                    CareerName = x.Name,
                    StudentTotal = studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault(),
                    Graduated = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.GRADUATED).Count(),
                    Bachelor = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.BACHELOR).Count(),
                    Qualified = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.QUALIFIED).Count()
                })
                .ToList();


            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                //DrawCounter = sentParameters.DrawCounter,
                //RecordsFiltered = recordsFiltered,
                //RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<object> GetStudentDegreeReportChart(List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null)
        {
            var query = _context.Students
                .Where(x => x.GraduationTermId != null)
                .AsNoTracking();

            var careersQuery = _context.Careers.AsNoTracking();

            if (careers.Count > 0 && !careers.Contains(Guid.Empty))
            {
                query = query.Where(x => careers.Contains(x.CareerId));
                careersQuery = careersQuery.Where(x => careers.Contains(x.Id));
            }

            if (graduatedYear != null)
            {
                query = query.Where(x => x.GraduationTerm.Year == graduatedYear);
            }

            if (graduatedTermId != null)
            {
                query = query.Where(x => x.GraduationTermId == graduatedTermId);
            }

            //Total de estudiantes por escuela
            var studentData = await _context.Students
                .GroupBy(x => new { x.CareerId })
                .Select(x => new
                {
                    x.Key.CareerId,
                    Total = x.Count()
                })
                .ToListAsync();

            var queryData = await query
                .Select(x => new
                {
                    x.CareerId,
                    CareerName = x.Career.Name,
                    x.Status
                })
                .OrderBy(x => x.CareerName)
                .ToListAsync();

            var careersDb = await careersQuery
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            var data = careersDb
                .Select(x => new
                {
                    CareerId = x.Id,
                    CareerName = x.Name,
                    StudentTotal = studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault(),
                    Graduated = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.GRADUATED).Count(),
                    Bachelor = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.BACHELOR).Count(),
                    Qualified = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.QUALIFIED).Count(),
                    GraduatedPercentage = studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault() == 0 ? 0.0 : (queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.GRADUATED).Count() * 100.0) / studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault(),
                    BachelorPercentage = studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault() == 0 ? 0.0 : (queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.BACHELOR).Count() * 100.0) / studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault(),
                    QualifiedPercentage = studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault() == 0 ? 0.0 : (queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.QUALIFIED).Count() * 100.0) / studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault()
                })
                .ToList();

            var result = new
            {
                categories = data.Select(x => x.CareerName).ToList(),
                dataCategories = new List<dynamic>
                {
                    //new { name = "Total", data = data.Select(x => x.StudentTotal).ToList() },
                    new { name = "%Egresado", data = data.Select(x => x.GraduatedPercentage).ToList() },
                    new { name = "%Bachiller", data = data.Select(x => x.BachelorPercentage).ToList() },
                    new { name = "%Titulado", data = data.Select(x => x.QualifiedPercentage).ToList() },
                }
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentDegreeFirstJobReportDatatable(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null)
        {
            var query = _context.Students
                .Where(x => x.GraduationTermId != null)
                .AsNoTracking();

            var careersQuery = _context.Careers.AsNoTracking();

            if (careers.Count > 0 && !careers.Contains(Guid.Empty))
            {
                query = query.Where(x => careers.Contains(x.CareerId));
                careersQuery = careersQuery.Where(x => careers.Contains(x.Id));
            }

            if (graduatedYear != null)
            {
                query = query.Where(x => x.GraduationTerm.Year == graduatedYear);
            }

            if (graduatedTermId != null)
            {
                query = query.Where(x => x.GraduationTermId == graduatedTermId);
            }

            //Total de estudiantes por escuela
            var studentData = await _context.Students
                .GroupBy(x => new { x.CareerId })
                .Select(x => new
                {
                    x.Key.CareerId,
                    Total = x.Count()
                })
                .ToListAsync();

            var queryData = await query
                .Select(x => new
                {
                    x.CareerId,
                    CareerName = x.Career.Name,
                    x.Status,
                    JobInLess3Month = x.StudentExperiences.Any(y => y.StartDate.Date <= x.GraduationTerm.StartDate.Date.AddMonths(3))
                })
                .OrderBy(x => x.CareerName)
                .ToListAsync();

            var careersDb = await careersQuery
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            var data = careersDb
                .Select(x => new
                {
                    CareerId = x.Id,
                    CareerName = x.Name,
                    StudentTotal = studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault(),
                    Graduated = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.GRADUATED && y.JobInLess3Month).Count(),
                    Bachelor = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.BACHELOR && y.JobInLess3Month).Count(),
                    Qualified = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.QUALIFIED && y.JobInLess3Month).Count()
                })
                .ToList();


            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                //DrawCounter = sentParameters.DrawCounter,
                //RecordsFiltered = recordsFiltered,
                //RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<object> GetStudentDegreeFirstJobReportChart(List<Guid> careers, int? graduatedYear = null, Guid? graduatedTermId = null)
        {
            var query = _context.Students
                .Where(x => x.GraduationTermId != null)
                .AsNoTracking();

            var careersQuery = _context.Careers.AsNoTracking();

            if (careers.Count > 0 && !careers.Contains(Guid.Empty))
            {
                query = query.Where(x => careers.Contains(x.CareerId));
                careersQuery = careersQuery.Where(x => careers.Contains(x.Id));
            }

            if (graduatedYear != null)
            {
                query = query.Where(x => x.GraduationTerm.Year == graduatedYear);
            }

            if (graduatedTermId != null)
            {
                query = query.Where(x => x.GraduationTermId == graduatedTermId);
            }

            //Total de estudiantes por escuela
            var studentData = await _context.Students
                .GroupBy(x => new { x.CareerId })
                .Select(x => new
                {
                    x.Key.CareerId,
                    Total = x.Count()
                })
                .ToListAsync();

            var queryData = await query
                .Select(x => new
                {
                    x.CareerId,
                    CareerName = x.Career.Name,
                    x.Status,
                    JobInLess3Month = x.StudentExperiences.Any(y => y.StartDate.Date <= x.GraduationTerm.StartDate.Date.AddMonths(3))
                })
                .OrderBy(x => x.CareerName)
                .ToListAsync();

            var careersDb = await careersQuery
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            var data = careersDb
                .Select(x => new
                {
                    CareerId = x.Id,
                    CareerName = x.Name,
                    StudentTotal = studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault(),
                    Graduated = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.GRADUATED && y.JobInLess3Month).Count(),
                    Bachelor = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.BACHELOR && y.JobInLess3Month).Count(),
                    Qualified = queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.QUALIFIED && y.JobInLess3Month).Count(),
                    GraduatedPercentage = studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault() == 0 ? 0.0 : (queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.GRADUATED && y.JobInLess3Month).Count() * 100.0) / studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault(),
                    BachelorPercentage = studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault() == 0 ? 0.0 : (queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.BACHELOR && y.JobInLess3Month).Count() * 100.0) / studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault(),
                    QualifiedPercentage = studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault() == 0 ? 0.0 : (queryData.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.QUALIFIED && y.JobInLess3Month).Count() * 100.0) / studentData.Where(y => y.CareerId == x.Id).Select(y => y.Total).FirstOrDefault()
                })
                .ToList();

            var result = new
            {
                categories = data.Select(x => x.CareerName).ToList(),
                dataCategories = new List<dynamic>
                {
                    //new { name = "Total", data = data.Select(x => x.StudentTotal).ToList() },
                    new { name = "%Egresado", data = data.Select(x => x.GraduatedPercentage).ToList() },
                    new { name = "%Bachiller", data = data.Select(x => x.BachelorPercentage).ToList() },
                    new { name = "%Titulado", data = data.Select(x => x.QualifiedPercentage).ToList() },
                }
            };

            return result;
        }


        public async Task<List<StudentFilterTemplate>> GetStudentFilterTemplatesByStatus(List<int> status)
        {
            var query = _context.Students
               .Where(x => status.Any(y => y == x.Status))
               .AsQueryable();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new StudentFilterTemplate
                {
                    Id = x.Id,
                    UserName = x.User.UserName,
                    FullName = x.User.FullName,
                    Dni = x.User.Dni,
                    Email = x.User.Email,
                    Career = x.Career.Name,
                    GraduationTerm = x.GraduationTermId == null ? "" : x.GraduationTerm.Name,
                    Status = x.Status
                })
                .ToListAsync();

            if (status.Any(y => y == ConstantHelpers.Student.States.RESIGNATION))
            {
                var studentsId = data.Select(x => x.Id).ToHashSet();
                var studentObservations = await _context.StudentObservations.Where(x => studentsId.Contains(x.StudentId) && x.Type == ConstantHelpers.OBSERVATION_TYPES.RESIGNATION).ToListAsync();

                foreach (var item in data)
                {
                    item.ResignationDateTime = studentObservations.Where(y => y.StudentId == item.Id).Select(y => y.CreatedAt.ToLocalDateTimeFormat()).FirstOrDefault();
                }
            }

            return data;
        }

        public async Task<StudentDataTemplate> GetStudentDataByUserId(string userId)
        {
            var studentData = await _context.Students
                .Where(x => x.UserId == userId)
                .Select(x => new StudentDataTemplate
                {
                    StudentId = x.Id,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    BirthDate = x.User.BirthDate,
                    Status = x.Status,
                    Career = x.Career.Name,
                    Faculty = x.Career.Faculty.Name,
                    CivilStatus = x.User.CivilStatus,
                    CurrentAcademicYear = x.CurrentAcademicYear,
                    CurrentAddress = x.User.Address,
                    CurrentPhoneNumber = x.User.PhoneNumber,
                    CurrentDepartmentId = x.User.DepartmentId,
                    CurrentDepartmentDescription = x.User.Department.Name,
                    CurrentDistrictId = x.User.DistrictId,
                    CurrentDistrictDescription = x.User.District.Name,
                    CurrentProvinceId = x.User.ProvinceId,
                    CurrentProvinceDescription = x.User.Province.Name,
                    Sex = x.User.Sex,
                    DNI = x.User.Dni,
                    Email = x.User.Email,
                    PersonalEmail = x.User.PersonalEmail,
                    UserName = x.User.UserName,
                    StudentInformationData = x.StudentInformationId == null ? null : new StudentInformationDataTemplate
                    {
                        StudentInformationId = x.StudentInformation.Id,
                        //StudentId = x.StudentInformation.StudentId,
                        TermId = x.StudentInformation.TermId,
                        Term = x.StudentInformation.Term.Name,
                        //General
                        OriginDepartmentId = x.StudentInformation.OriginDistrict.Province.DepartmentId,
                        OriginProvinceId = x.StudentInformation.OriginDistrict.ProvinceId,
                        OriginDistrictId = x.StudentInformation.OriginDistrictId,
                        PlaceOriginDepartmentId = x.StudentInformation.PlaceOriginDistrict.Province.DepartmentId,
                        PlaceOriginProvinceId = x.StudentInformation.PlaceOriginDistrict.ProvinceId,
                        PlaceOriginDistrictId = x.StudentInformation.PlaceOriginDistrictId,
                        PlaceOriginDepartmentDescription = x.StudentInformation.PlaceOriginDistrict.Province.Department.Name,
                        PlaceOriginProvinceDescription = x.StudentInformation.PlaceOriginDistrict.Province.Name,
                        PlaceOriginDistrictDescription = x.StudentInformation.PlaceOriginDistrict.Name,
                        //Personal Information 
                        OriginAddress = x.StudentInformation.OriginAddress,
                        OriginPhoneNumber = x.StudentInformation.OriginPhoneNumber,
                        FullNameExternalPerson = x.StudentInformation.FullNameExternalPerson,
                        AddressExternalPerson = x.StudentInformation.AddressExternalPerson,
                        EmailExternalPerson = x.StudentInformation.EmailExternalPerson,
                        PhoneExternalPerson = x.StudentInformation.PhoneExternalPerson,
                        Age = x.StudentInformation.Age.ToString(),
                        //Academic Background
                        OriginSchool = x.StudentInformation.OriginSchool,
                        OriginSchoolPlace = x.StudentInformation.OriginSchoolPlace,
                        SchoolType = x.StudentInformation.SchoolType,
                        UniversityPreparationId = x.StudentInformation.UniversityPreparationId,
                        //Health
                        IsSick = x.StudentInformation.IsSick,
                        TypeParentIllness = x.StudentInformation.TypeParentIllness,
                        HaveInsurance = x.StudentInformation.HaveInsurance,
                        InsuranceDescription = x.StudentInformation.InsuranceDescription,
                        //Feeding
                        BreakfastHome = x.StudentInformation.BreakfastHome,
                        BreakfastPension = x.StudentInformation.BreakfastPension,
                        BreakfastRelativeHome = x.StudentInformation.BreakfastRelativeHome,
                        BreakfastOther = x.StudentInformation.BreakfastOther,
                        LunchHome = x.StudentInformation.LunchHome,
                        LunchPension = x.StudentInformation.LunchPension,
                        LunchRelativeHome = x.StudentInformation.LunchRelativeHome,
                        LunchOther = x.StudentInformation.LunchOther,
                        DinnerHome = x.StudentInformation.DinnerHome,
                        DinnerPension = x.StudentInformation.DinnerPension,
                        DinnerRelativeHome = x.StudentInformation.DinnerRelativeHome,
                        DinnerOther = x.StudentInformation.DinnerOther,
                        //Economy
                        PrincipalPerson = x.StudentInformation.PrincipalPerson,
                        EconomicMethodFatherTutor = x.StudentInformation.EconomicMethodFatherTutor,
                        DSectorFatherTutor = x.StudentInformation.DSectorFatherTutor,
                        DWorkConditionFatherTutor = x.StudentInformation.DWorkConditionFatherTutor,
                        DEspecificActivityFatherTutor = x.StudentInformation.DEspecificActivityFatherTutor,
                        DBusyFatherTutor = x.StudentInformation.DBusyFatherTutor,
                        ISectorFatherTutor = x.StudentInformation.ISectorFatherTutor,
                        IWorkConditionFatherTutor = x.StudentInformation.IWorkConditionFatherTutor,
                        IEspecificActivityFatherTutor = x.StudentInformation.IEspecificActivityFatherTutor,
                        EconomicMethodMother = x.StudentInformation.EconomicMethodMother,
                        DSectorMother = x.StudentInformation.DSectorMother,
                        DWorkConditionMother = x.StudentInformation.DWorkConditionMother,
                        DEspecificActivityMother = x.StudentInformation.DEspecificActivityMother,
                        DBusyMother = x.StudentInformation.DBusyMother,
                        ISectorMother = x.StudentInformation.ISectorMother,
                        IWorkConditionMother = x.StudentInformation.IWorkConditionMother,
                        IEspecificActivityMother = x.StudentInformation.IEspecificActivityMother,
                        EconomicExpensesFeeding = x.StudentInformation.EconomicExpensesFeeding,
                        EconomicExpensesBasicServices = x.StudentInformation.EconomicExpensesBasicServices,
                        EconomicExpensesEducation = x.StudentInformation.EconomicExpensesEducation,
                        EconomicExpensesOthers = x.StudentInformation.EconomicExpensesOthers,
                        FatherRemuneration = x.StudentInformation.FatherRemuneration,
                        MotherRemuneration = x.StudentInformation.MotherRemuneration,
                        StudentRemuneration = x.StudentInformation.StudentRemuneration,
                        OtherRemuneration = x.StudentInformation.OtherRemuneration,
                        TotalRemuneration = x.StudentInformation.TotalRemuneration,
                        StudentDependency = x.StudentInformation.StudentDependency,
                        StudentCoexistence = x.StudentInformation.StudentCoexistence,
                        FamilyRisk = x.StudentInformation.FamilyRisk,
                        StudentWorkDedication = x.StudentInformation.StudentWorkDedication,
                        StudentWorkDescription = x.StudentInformation.StudentWorkDescription,
                        StudentWorkCondition = x.StudentInformation.StudentWorkCondition,
                        AuthorizeCheck = x.StudentInformation.AuthorizeCheck,
                        AuthorizedPersonFullName = x.StudentInformation.AuthorizedPersonFullName,
                        AuthorizedPersonAddress = x.StudentInformation.AuthorizedPersonAddress,
                        AuthorizedPersonPhone = x.StudentInformation.AuthorizedPersonPhone,
                        //Living Place
                        Tenure = x.StudentInformation.Tenure,
                        ContructionType = x.StudentInformation.ContructionType,
                        ZoneType = x.StudentInformation.ZoneType,
                        BuildType = x.StudentInformation.BuildType,
                        OtherTypeLivingPlace = x.StudentInformation.OtherTypeLivingPlace,
                        NumberFloors = x.StudentInformation.NumberFloors,
                        NumberRooms = x.StudentInformation.NumberRooms,
                        NumberKitchen = x.StudentInformation.NumberKitchen,
                        NumberBathroom = x.StudentInformation.NumberBathroom,
                        NumberLivingRoom = x.StudentInformation.NumberLivingRoom,
                        NumberDinningRoom = x.StudentInformation.NumberDinningRoom,
                        Water = x.StudentInformation.Water,
                        Drain = x.StudentInformation.Drain,
                        LivingPlacePhone = x.StudentInformation.LivingPlacePhone,
                        Light = x.StudentInformation.Light,
                        Internet = x.StudentInformation.Internet,
                        TV = x.StudentInformation.TV,
                        HasPhone = x.StudentInformation.HasPhone,
                        Radio = x.StudentInformation.Radio,
                        Stereo = x.StudentInformation.Stereo,
                        Iron = x.StudentInformation.Iron,
                        EquipPhone = x.StudentInformation.EquipPhone,
                        Laptop = x.StudentInformation.Laptop,
                        Closet = x.StudentInformation.Closet,
                        Fridge = x.StudentInformation.Fridge,
                        PersonalLibrary = x.StudentInformation.PersonalLibrary,
                        EquipComputer = x.StudentInformation.EquipComputer,
                    }
                })
                .FirstOrDefaultAsync();

            return studentData;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCurrentEnrolledStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Career.Faculty.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "6":
                    orderByPredicate = (x) => x.AcademicSummaries.DefaultIfEmpty().Count(y => y.StudentId == x.Id && y.CareerId == x.CareerId && y.CurriculumId == x.CurriculumId && !y.Term.IsSummer);
                    break;
            }

            var query = _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.User.UserName,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    x.User.Name,
                    Faculty = x.Career.Faculty.Name,
                    Career = x.Career.Name,
                    EnrolledSemester = (x.AcademicSummaries.DefaultIfEmpty().Count(y => y.StudentId == x.Id && y.CareerId == x.CareerId && y.CurriculumId == x.CurriculumId && !y.Term.IsSummer) + 1)
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

        #region PROCEDURES

        public async Task<StudentProcedureResult> StudentCareerTransferRequest(ClaimsPrincipal user, Guid studentId, Guid newCareerId, Guid newCurriculumId)
        {
            var result = new StudentProcedureResult();

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();

            var oldCareer = await _context.Careers.Where(x => x.Id == student.CareerId).FirstOrDefaultAsync();
            var newCareer = await _context.Careers.Where(x => x.Id == newCareerId).FirstOrDefaultAsync();

            var oldCurriculum = await _context.Curriculums.Where(x => x.Id == student.CurriculumId).FirstOrDefaultAsync();
            var newCurriculum = await _context.Curriculums.Where(x => x.Id == newCurriculumId).FirstOrDefaultAsync();

            student.CareerId = newCareer.Id;
            student.CurriculumId = newCurriculum.Id;
            student.Status = ConstantHelpers.Student.States.TRANSFER;

            string academicProgramTextLog = "";

            academicProgramTextLog += "del programa académico ";

            if (student.AcademicProgramId.HasValue)
            {
                var oldAcademicProgram = await _context.AcademicPrograms.Where(x => x.Id == student.AcademicProgramId.Value).FirstOrDefaultAsync();
                academicProgramTextLog += $"{oldAcademicProgram.Code}-{oldAcademicProgram.Name}";
            }
            else
            {
                academicProgramTextLog += "--";
            }

            academicProgramTextLog += " al programa académico ";

            if (newCurriculum.AcademicProgramId.HasValue)
            {
                var newAcademicProgram = await _context.AcademicPrograms.Where(x => x.Id == newCurriculum.AcademicProgramId.Value).FirstOrDefaultAsync();
                academicProgramTextLog += $"{newAcademicProgram.Code}-{newAcademicProgram.Name}";
                student.AcademicProgramId = newAcademicProgram.Id;
            }
            else
            {
                academicProgramTextLog += "--";
                student.AcademicProgramId = null;
            }

            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();

            var observation = new StudentObservation
            {
                Observation = $"Traslado de la escuela profesional {oldCareer.Code}-{oldCareer.Name} a la escuela {newCareer.Code}-{newCareer.Name}," +
                    $" del plan curricular {oldCurriculum.Year}-{oldCurriculum.Code} al plan curricular {newCurriculum.Year}-{newCurriculum.Code}," +
                    $" {academicProgramTextLog}",
                StudentId = student.Id,
                Type = ConstantHelpers.OBSERVATION_TYPES.OBSERVATION,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null
            };

            await _context.StudentObservations.AddAsync(observation);
            await _context.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<StudentProcedureResult> StudentAcademicYearWithdrawalRequest(ClaimsPrincipal user, Guid studentId)
        {
            var result = new StudentProcedureResult();
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();

            if (term is null)
            {
                result.Message = "No existe periodo activo";
                return result;
            }

            var studentSections = await _context.StudentSections.Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == term.Id).ToListAsync();

            if (!studentSections.Any())
            {

                result.Message = "No se encuentra matriculado en el periodo activo";
                return result;
            }

            if (studentSections.All(x => x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN))
            {
                result.Message = "Ya se ha retirado en el periodo activo";
                return result;
            }

            foreach (var item in studentSections)
            {
                item.Status = ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN;
            }

            await _context.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<StudentProcedureResult> StudentCourseWithdrawalRequest(ClaimsPrincipal user, Guid studentSectionId)
        {
            var result = new StudentProcedureResult();

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var data = await _context.StudentSections.Where(x => x.Id == studentSectionId)
                .Select(x => new
                {
                    x.Id,
                    x.Section.CourseTerm.CourseId,
                    x.SectionId,
                    x.StudentId
                })
                .FirstOrDefaultAsync();

            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();

            var studentSection = await _context.StudentSections.Where(x => x.Id == studentSectionId).FirstOrDefaultAsync();

            if (studentSection.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN)
            {
                result.Message = "Ya no se encuentra matriculado en el curso.";
                return result;
            }

            studentSection.Status = ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN;

            var observation = new StudentObservation()
            {
                Observation = "Retiro de asignatura",
                StudentId = studentSection.StudentId,
                Type = ConstantHelpers.OBSERVATION_TYPES.OBSERVATION,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null
            };

            await _context.StudentObservations.AddAsync(observation);
            await _context.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<StudentProcedureResult> StudentCourseWithdrawalMassiveRequest(ClaimsPrincipal user, List<Guid> studentSectionIds)
        {
            var result = new StudentProcedureResult();

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var data = await _context.StudentSections.Where(x => studentSectionIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Section.CourseTerm.CourseId,
                    x.SectionId,
                    x.StudentId,
                    x.Status,
                    x.Section.CourseTerm.Course.Name
                })
                .ToListAsync();

            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();

            var studentSections = await _context.StudentSections.Where(x => studentSectionIds.Contains(x.Id)).ToListAsync();

            if (studentSections.Any(y => y.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN))
            {
                result.Message = $"Ya no se encuentra matriculado en el(los) curso(s) {string.Join("; ", data.Where(x => x.Status == ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN).Select(x => x.Name).ToList())}.";
                return result;
            }

            studentSections.ForEach(x => x.Status = ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN);

            var observation = new StudentObservation()
            {
                Observation = "Retiro de asignatura en bloque",
                StudentId = data.Select(x => x.StudentId).FirstOrDefault(),
                Type = ConstantHelpers.OBSERVATION_TYPES.OBSERVATION,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null
            };

            await _context.StudentObservations.AddAsync(observation);
            await _context.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<StudentProcedureResult> ResignStudentRequest(ClaimsPrincipal user, Guid studentId, string reason, string fileUrl)
        {
            var result = new StudentProcedureResult();

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();

            if (term != null)
            {
                var studentSections = await _context.StudentSections.Where(x => x.StudentId == studentId && x.Section.CourseTerm.TermId == term.Id).ToListAsync();
                foreach (var item in studentSections)
                {
                    item.Status = ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN;
                }
            }

            student.Status = ConstantHelpers.Student.States.RESIGNATION;

            var observation = new StudentObservation
            {
                Observation = reason,
                StudentId = student.Id,
                Type = ConstantHelpers.OBSERVATION_TYPES.RESIGNATION,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null,
                File = string.IsNullOrEmpty(fileUrl) ? null : fileUrl
            };

            await _context.StudentObservations.AddAsync(observation);
            await _context.SaveChangesAsync();

            result.Succeeded = true;

            return result;
        }

        public async Task<StudentProcedureResult> ReentryStudentRequest(ClaimsPrincipal user, Guid studentId, string fileUrl)
        {
            var result = new StudentProcedureResult();

            var status = ConstantHelpers.Student.States.ENTRANT;
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var academicSummaries = await _context.AcademicSummaries
                .Include(x => x.Term)
                .Where(x => x.StudentId == studentId && !x.WasWithdrawn)
                .ToListAsync();

            if (academicSummaries.Any())
            {
                var summary = academicSummaries
                    .OrderByDescending(x => x.Term.Year)
                    .ThenByDescending(x => x.Term.Number)
                    .FirstOrDefault();

                status = summary.StudentStatus != ConstantHelpers.Student.States.SANCTIONED && summary.StudentStatus != ConstantHelpers.Student.States.DESERTION
                        ? summary.StudentStatus : ConstantHelpers.Student.States.IRREGULAR;
            }

            student.Status = status;

            var observation = new StudentObservation
            {
                Observation = "Reincorporación del estudiante",
                StudentId = student.Id,
                Type = ConstantHelpers.OBSERVATION_TYPES.OBSERVATION,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null,
                File = string.IsNullOrEmpty(fileUrl) ? null : fileUrl
            };

            await _context.StudentObservations.AddAsync(observation);
            await _context.SaveChangesAsync();

            result.Succeeded = true;

            return result;
        }

        public async Task<StudentProcedureResult> StudentReservationRequest(ClaimsPrincipal user, Guid studentId, string receipt, string fileUrl, string observation)
        {
            var result = new StudentProcedureResult();
            if (string.IsNullOrEmpty(receipt))
            {
                result.Message = "Debe completar todos los campos.";
                return result;
            }

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null)
            {
                result.Message = "No puede realizar una reserva fuera del periodo académico.";
                return result;
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();

            var years = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_TIME_LIMIT));
            var isRenewable = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_RENEWABLE_PER_SEMESTER));
            var enrollmentRequired = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REQUIRE_ENROLLMENT_FOR_RESERVATION));

            var studentSections = await _context.StudentSections.Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == term.Id).ToListAsync();

            if (isRenewable)
            {
                var prevReservations = await _context.EnrollmentReservations
                    .Where(x => x.StudentId == student.Id)
                    .ToListAsync();

                if (prevReservations.Count() * 0.5 >= years)
                {
                    result.Message = $"El estudiante no puede registrar más reservas de matrícula. Llegó al límite de {years} años.";
                    return result;
                }
            }

            if (enrollmentRequired && !studentSections.Any())
            {
                result.Message = "No se encuentra matriculado en el periodo actual.";
                return result;
            }

            var turn = await _context.EnrollmentTurns.FirstOrDefaultAsync(x => x.StudentId == student.Id && x.TermId == term.Id);
            if (turn != null) turn.IsOnline = false;

            if (studentSections.Any())
            {
                try
                {
                    var studentSectionIds = studentSections.Select(x => x.Id).ToHashSet();

                    var otherQualifications = await _context.OtherQualificationStudents
                        .Where(x => studentSectionIds.Contains(x.StudentSectionId))
                        .ToListAsync();
                    _context.OtherQualificationStudents.RemoveRange(otherQualifications);

                    var temporalGrades = await _context.TemporalGrades
                    .Where(x => studentSectionIds.Contains(x.StudentSectionId))
                    .ToListAsync();
                    _context.TemporalGrades.RemoveRange(temporalGrades);

                    var grades = await _context.Grades
                        .Where(x => studentSectionIds.Contains(x.StudentSectionId))
                        .ToListAsync();
                    _context.Grades.RemoveRange(grades);


                    var gradeCorrections = await _context.GradeCorrections.Where(x => studentSectionIds.Contains(x.Grade.StudentSectionId)).ToListAsync();
                    _context.GradeCorrections.RemoveRange(gradeCorrections);

                    _context.StudentSections.RemoveRange(studentSections);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    result.Message = "El estudiante ya posee información relacionada de sus clases";
                    return result;
                }
            }

            var expirationDate = term.EndDate;

            var reservation = new EnrollmentReservation
            {
                TermId = term.Id,
                ExpirationDate = expirationDate,
                StudentId = student.Id,
                Receipt = receipt,
                Observation = observation,
                FileURL = string.IsNullOrEmpty(fileUrl) ? null : fileUrl
            };

            student.Status = ConstantHelpers.Student.States.RESERVED;

            await _context.EnrollmentReservations.AddAsync(reservation);
            await _context.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<StudentProcedureResult> StudentChangeAcademicProgramRequest(ClaimsPrincipal user, Guid studentId, Guid newAcademicProgramId)
        {
            var result = new StudentProcedureResult();

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();

            student.AcademicProgramId = newAcademicProgramId;

            var observation = new StudentObservation
            {
                Observation = "Cambio de especialidad",
                StudentId = student.Id,
                Type = ConstantHelpers.OBSERVATION_TYPES.RESIGNATION,
                UserId = userId,
                TermId = term != null ? term.Id : (Guid?)null,
            };

            await _context.StudentObservations.AddAsync(observation);
            await _context.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<StudentProcedureResult> StudentExoneratedCourseRequest(ClaimsPrincipal user, Guid studentId, Guid courseId)
        {
            var result = new StudentProcedureResult();

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();

            if (term == null)
            {
                result.Message = "No se encuentra dentro del periodo de matrícula.";
                return result;
            }

            var evaluation = await _context.ExtraordinaryEvaluations
                .FirstOrDefaultAsync(x => x.TermId == term.Id && x.CourseId == courseId
                && x.Type == ConstantHelpers.Intranet.ExtraordinaryEvaluation.EXONERATED);

            if (evaluation == null)
            {
                evaluation = new ExtraordinaryEvaluation
                {
                    Type = ConstantHelpers.Intranet.ExtraordinaryEvaluation.EXONERATED,
                    CourseId = courseId,
                    Resolution = null,
                    TermId = term.Id,
                    TeacherId = null
                };

                await _context.ExtraordinaryEvaluations.AddAsync(evaluation);
            }

            var entity = await _context.ExtraordinaryEvaluationStudents.Where(x => x.ExtraordinaryEvaluationId == evaluation.Id && x.StudentId == student.Id).FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new ExtraordinaryEvaluationStudent
                {
                    ExtraordinaryEvaluationId = evaluation.Id,
                    Status = ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.PENDING,
                    StudentId = student.Id
                };

                await _context.ExtraordinaryEvaluationStudents.AddAsync(entity);
            }

            await _context.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<StudentProcedureResult> StudentExtraordinaryEvaluationRequest(ClaimsPrincipal user, Guid studentId, Guid courseId)
        {
            var result = new StudentProcedureResult();

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();

            if (term == null)
            {
                result.Message = "No se encuentra dentro del periodo de matrícula.";
                return result;
            }

            var evaluation = await _context.ExtraordinaryEvaluations
                .FirstOrDefaultAsync(x => x.TermId == term.Id && x.CourseId == courseId
                && x.Type == ConstantHelpers.Intranet.ExtraordinaryEvaluation.EXTRAORDINARY);

            if (evaluation == null)
            {
                evaluation = new ExtraordinaryEvaluation
                {
                    Type = ConstantHelpers.Intranet.ExtraordinaryEvaluation.EXTRAORDINARY,
                    CourseId = courseId,
                    Resolution = null,
                    TermId = term.Id,
                    TeacherId = null
                };

                await _context.ExtraordinaryEvaluations.AddAsync(evaluation);
            }

            var entity = await _context.ExtraordinaryEvaluationStudents.Where(x => x.ExtraordinaryEvaluationId == evaluation.Id && x.StudentId == student.Id).FirstOrDefaultAsync();

            if (entity == null)
            {
                entity = new ExtraordinaryEvaluationStudent
                {
                    ExtraordinaryEvaluationId = evaluation.Id,
                    Status = ConstantHelpers.Intranet.ExtraordinaryEvaluationStudent.PENDING,
                    StudentId = student.Id
                };

                await _context.ExtraordinaryEvaluationStudents.AddAsync(entity);
            }

            await _context.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<StudentProcedureResult> StudentSubstituteExamRequest(ClaimsPrincipal user, Guid studentSectionId, string paymentReceipt)
        {
            var result = new StudentProcedureResult();
            var studentSection = await _context.StudentSections.Where(x => x.Id == studentSectionId).FirstOrDefaultAsync();
            var section = await _context.Sections.Where(x => x.Id == studentSection.SectionId).FirstOrDefaultAsync();
            var courseTerm = await _context.CourseTerms.Where(x => x.Id == section.CourseTermId).FirstOrDefaultAsync();
            var course = await _context.Courses.Where(x => x.Id == courseTerm.CourseId).FirstOrDefaultAsync();
            var term = await _context.Terms.Where(x => x.Id == courseTerm.TermId).FirstOrDefaultAsync();
            var evaluations = await _context.Evaluations.Where(x => x.CourseTermId == section.CourseTermId).ToListAsync();
            var grades = await _context.Grades.Where(x => x.StudentSection.SectionId == section.Id).ToListAsync();
            var studentsEnabledForSubstituteExam = await GetStudentsForSubstituteExamDataAsync(term.Id, section.Id);

            foreach (var item in evaluations)
            {
                if (grades.Any(x => x.EvaluationId == item.Id))
                {
                    result.Message = $"La evaluación '{item.Name}' no tiene notas registradas.";
                    return result;
                }
            }

            if (section == null)
            {
                result.Message = "No se encontró la sección seleccionada.";
                return result;
            }

            if (!studentsEnabledForSubstituteExam.Any(y => y.StudentId == studentSection.StudentId))
            {
                result.Message = $"No cuentas con los requisitos para solicitar examen sustitorio para el curso {course.FullName}.";
                return result;
            }

            var substituteExamDetail = await _context.SubstituteExamDetails.Where(x => x.SubstituteExams.Any(y => y.SectionId == section.Id)).FirstOrDefaultAsync();

            if (substituteExamDetail == null)
            {
                var classroom = await _context.Classrooms.FirstOrDefaultAsync();

                substituteExamDetail = new SubstituteExamDetail
                {
                    ClassroomId = classroom.Id,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddMinutes(60),
                };

                await _context.SubstituteExamDetails.AddAsync(substituteExamDetail);
            }

            var studentFiltered = studentsEnabledForSubstituteExam.Where(y => y.StudentId == studentSection.StudentId).FirstOrDefault();

            var substituteExam = await _context.SubstituteExams.Where(x => x.StudentId == studentSection.StudentId && x.SectionId == section.Id).FirstOrDefaultAsync();

            if (substituteExam != null && (substituteExam.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED || substituteExam.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.EVALUATED))
            {
                result.Message = $"Ya se encuentra registrado en el examen sustitutorio.";
                return result;
            }

            if (substituteExam == null)
            {
                substituteExam = new SubstituteExam
                {
                    ExamScore = Convert.ToInt32(studentFiltered.score),
                    SectionId = section.Id,
                    CourseTermId = section.CourseTermId,
                    StudentId = studentFiltered.StudentId,
                    SubstituteExamDetailId = substituteExamDetail.Id
                };

                await _context.SubstituteExams.AddAsync(substituteExam);
            }

            substituteExam.Status = ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED;
            substituteExam.Underpin = "POR TRÁMITE";
            substituteExam.PaymentReceipt = paymentReceipt;

            await _context.SaveChangesAsync();

            result.Succeeded = true;
            return result;
        }

        public async Task<StudentProcedureResult> StudentGradeRecoveryRequest(ClaimsPrincipal user, Guid studentSectionId)
        {
            var result = new StudentProcedureResult();

            var studentSection = await _context.StudentSections.Where(x => x.Id == studentSectionId).FirstOrDefaultAsync();
            var section = await _context.Sections.Where(x => x.Id == studentSection.SectionId).FirstOrDefaultAsync();

            var substituteExams = await _context.SubstituteExams.Where(x => (x.SectionId == section.Id || x.CourseTermId == section.CourseTermId) && x.Status == ConstantHelpers.SUBSTITUTE_EXAM_STATUS.REGISTERED).ToListAsync();

            var confiMinGrade = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MIN_EVALUATION_GRADE).FirstOrDefaultAsync();
            var minGrade = Convert.ToDecimal(confiMinGrade.Value);

            var gradeRecoveryExam = await _context.GradeRecoveryExams.Where(x => x.SectionId == section.Id).FirstOrDefaultAsync();

            if (studentSection.FinalGrade < minGrade)
            {
                result.Message = $"No cumple con la nota mínima para poder dar el examen. Promedio Actual : {studentSection.FinalGrade}.";
                return result;
            }

            if (substituteExams.Any(x => x.StudentId == studentSection.StudentId))
            {
                result.Message = $"Se tiene un examen sustitutorio pendiente.";
                return result;
            }

            if (gradeRecoveryExam == null)
            {
                var classroom = await _context.Classrooms.FirstOrDefaultAsync();

                gradeRecoveryExam = new GradeRecoveryExam
                {
                    ClassroomId = classroom.Id,
                    SectionId = section.Id,
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddMinutes(60)
                };

                await _context.GradeRecoveryExams.AddAsync(gradeRecoveryExam);
            }

            gradeRecoveryExam.Status = ConstantHelpers.GRADE_RECOVERY_EXAM_STATUS.CONFIRMED;

            var gradeRecovery = await _context.GradeRecoveries.Where(x => x.GradeRecoveryExamId == gradeRecoveryExam.Id && x.StudentId == studentSection.StudentId).FirstOrDefaultAsync();

            if (gradeRecovery != null)
            {
                result.Message = $"Ya se encuentra registrado en el examen de recuperación de nota.";
                return result;
            }

            gradeRecovery = new GradeRecovery
            {
                StudentId = studentSection.StudentId,
                GradeRecoveryExamId = gradeRecoveryExam.Id,
                CourseTermId = section.CourseTermId
            };

            await _context.GradeRecoveries.AddAsync(gradeRecovery);

            await _context.SaveChangesAsync();
            result.Succeeded = true;
            return result;
        }

        public async Task<StudentProcedureResult> ExecuteProcedureActivity(ClaimsPrincipal user, UserProcedure userProcedure, StudentUserProcedure studentUserProcedure)
        {
            var result = new REPOSITORY.Repositories.Generals.Templates.Student.StudentProcedureResult();

            switch (studentUserProcedure.ActivityType)
            {
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.CAREER_TRANSFER:
                    result = await StudentCareerTransferRequest(user, studentUserProcedure.StudentId, studentUserProcedure.CareerId.Value, studentUserProcedure.CurriculumId.Value);
                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.ACADEMIC_YEAR_WITHDRAWAL:
                    result = await StudentAcademicYearWithdrawalRequest(user, studentUserProcedure.StudentId);
                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL:
                    result = await StudentCourseWithdrawalRequest(user, studentUserProcedure.StudentSectionId.Value);
                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.RESIGNATION:
                    result = await ResignStudentRequest(user, studentUserProcedure.StudentId, userProcedure.Comment, null);
                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.REENTRY:
                    result = await ReentryStudentRequest(user, studentUserProcedure.StudentId, null);

                    //if (result.Succeeded)
                    //{
                    //    await _paymentService.GenerateStudentReentryPayments(studentUserProcedure.StudentId);

                    //    if (studentUserProcedure.TermId.HasValue)
                    //    {
                    //        var studentUserProcedureTerm = await _termService.Get(studentUserProcedure.TermId.Value);
                    //        var careerShift = await _careerEnrollmentShiftService.Get(studentUserProcedureTerm.Id, studentUserProcedure.CareerId.Value);

                    //        if (careerShift != null && careerShift.WasExecuted)
                    //            await _enrollmentTurnService.GenerateStudentTurn(studentUserProcedureTerm.Id, studentUserProcedure.StudentId);
                    //    }
                    //}

                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.REGISTRATION_RESERVATION:
                    result = await StudentReservationRequest(user, studentUserProcedure.StudentId, "-", null, userProcedure.Comment);
                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.CHANGE_ACADEMIC_PROGRAM:
                    result = await StudentChangeAcademicProgramRequest(user, studentUserProcedure.StudentId, studentUserProcedure.AcademicProgramId.Value);
                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.EXONERATED_COURSE:
                    result = await StudentExoneratedCourseRequest(user, studentUserProcedure.StudentId, studentUserProcedure.CourseId.Value);
                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.EXTRAORDINARY_EVALUATION:
                    result = await StudentExtraordinaryEvaluationRequest(user, studentUserProcedure.StudentId, studentUserProcedure.CourseId.Value);
                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.SUBSTITUTE_EXAM:
                    result = await StudentSubstituteExamRequest(user, studentUserProcedure.StudentSectionId.Value, string.Empty);
                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.GRADE_RECOVERY:
                    result = await StudentGradeRecoveryRequest(user, studentUserProcedure.StudentSectionId.Value);
                    break;
                case ConstantHelpers.PROCEDURE_TASK.ACTIVITY_TYPE.COURSE_WITHDRAWAL_MASSIVE:
                    {
                        var studentSections = await _context.StudentUserProcedureDetails.Where(x => x.StudentUserProcedureId == studentUserProcedure.Id).Where(x => x.StudentSectionId.HasValue).Select(x => x.StudentSectionId.Value).ToListAsync();
                        result = await StudentCourseWithdrawalMassiveRequest(user, studentSections);
                    }
                    break;
            }

            return result;
        }

        #endregion

        public async Task<object> GetAverageGradesByStudentReport(Guid studentId)
        {
            var academicSummaries = await _context.AcademicSummaries
                .Where(x => x.StudentId == studentId)
                .OrderBy(x => x.Term.Year)
                .ThenBy(x => x.Term.Number)
                //.TakeLast(5)
                .Select(x => new
                {
                    x.Term.Year,
                    x.Term.Number,
                    Term = x.Term.Name,
                    x.WeightedAverageGrade
                }).ToListAsync();

            academicSummaries = academicSummaries.TakeLast(5).ToList();
            var report = new
            {
                categories = academicSummaries.Select(x => x.Term).ToList(),
                data = academicSummaries.Select(x => x.WeightedAverageGrade).ToList()
            };

            return report;
        }

        public async Task<List<EnrolledCourseTemplate>> GetEnrolledCoursesAvailableToSubstitueExam(Guid studentId)
        {
            var configs = await _context.Configurations.ToDictionaryAsync(x => x.Key, x => x.Value);
            var min_substitute_examen = decimal.Parse(GetConfigurationValue(configs, CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.MIN_SUBSTITUTE_EXAMEN));

            var query = _context.StudentSections
                                      .Where(f =>
                                          f.StudentId == studentId &&
                                          f.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE &&
                                          f.Status == ConstantHelpers.STUDENT_SECTION_STATES.IN_PROCESS &&
                                          f.FinalGrade >= min_substitute_examen &&
                                          f.FinalGrade < f.Section.CourseTerm.Term.MinGrade &&
                                          f.Section.Code != "EVALUACIÓN EXTRAORDINARIA"
                                          );

            var result = await query
                .Select(x => new EnrolledCourseTemplate
                {
                    StudentSectionId = x.Id,
                    CourseId = x.Section.CourseTerm.CourseId,
                    CourseName = x.Section.CourseTerm.Course.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<EnrolledCourseTemplate>> GetEnrolledCoursesToGradeRecovery(Guid studentId)
        {
            var configs = await _context.Configurations.ToDictionaryAsync(x => x.Key, x => x.Value);
            var minGrade = decimal.Parse(GetConfigurationValue(configs, CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_MIN_EVALUATION_GRADE));
            var evaluationType = Guid.Parse(GetConfigurationValue(configs, CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.EVALUATION_TYPE_GRADE_RECOVERY));

            var confiEnabledApproved = bool.Parse(GetConfigurationValue(configs, CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.GRADE_RECOVERY_ENABLED_TO_APPROVED));
            var enabledToApproved = Convert.ToBoolean(confiEnabledApproved);

            var substituteExamSectionsId = await _context.SubstituteExams
                .Where(x => x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE && x.StudentId == studentId)
                .Select(x => x.SectionId)
                .ToListAsync();

            var query = _context.StudentSections
                .Where(x =>
                x.StudentId == studentId &&
                x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE &&
                x.FinalGrade >= minGrade &&
                !substituteExamSectionsId.Contains(x.SectionId) &&
                x.Grades.Count() >= x.Section.CourseTerm.Evaluations.Count()
                );


            if (!enabledToApproved)
            {
                query = query.Where(x => x.FinalGrade < x.Section.CourseTerm.Term.MinGrade);
            }

            if (evaluationType != Guid.Empty)
            {
                query = query.Where(x => x.Grades.Any(y => y.Evaluation.EvaluationTypeId == evaluationType));
            }

            var result = await query
              .Select(x => new EnrolledCourseTemplate
              {
                  StudentSectionId = x.Id,
                  CourseId = x.Section.CourseTerm.CourseId,
                  CourseName = x.Section.CourseTerm.Course.Name
              })
              .ToListAsync();

            return result;
        }

        public async Task<List<EnrolledCourseTemplate>> GetCoursesAvailableForExoneratedCourse(Guid studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();

            var academicYearCourses = await _context.AcademicYearCourses
               .Where(x => x.CurriculumId == student.CurriculumId && x.IsExonerable)
               .OrderBy(x => x.AcademicYear).ThenBy(x => x.Course.Name)
               .Select(x => new
               {
                   x.CourseId,
                   x.Course.Name,
                   x.Course.Code
               })
               .ToListAsync();

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == student.Id && !x.Withdraw)
               .Select(x => new
               {
                   x.CourseId,
                   x.Approved
               })
               .ToListAsync();

            var academicHistoriesHash = academicHistories.Select(x => x.CourseId).ToList();

            var availableCourses = academicYearCourses.Where(x => !academicHistoriesHash.Contains(x.CourseId)).ToList();

            var result = availableCourses
                .Select(x => new EnrolledCourseTemplate
                {
                    CourseId = x.CourseId,
                    CourseName = $"{x.Code} - {x.Name}"
                })
                .ToList();

            return result;
        }

        public async Task<List<EnrolledCourseTemplate>> GetCoursesAvailableForExtraordinaryEvaluation(Guid studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();

            var academicYearCourses = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == student.CurriculumId)
                .OrderBy(x => x.AcademicYear)
                .ThenBy(x => x.Course.Name)
                .Select(x => new
                {
                    x.CourseId,
                    x.Course.Name,
                    x.Course.Code
                })
                .ToListAsync();

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == student.Id)
                .Select(x => new
                {
                    x.CourseId,
                    x.Approved
                })
                .ToListAsync();

            var disapprovedCourses = academicHistories
                .GroupBy(x => x.CourseId)
                .Where(x => !x.Any(y => y.Approved))
                .Select(x => x.Key)
                .ToList();

            var queryEnrolledCourses = _context.StudentSections
                .Where(x => x.StudentId == studentId && x.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                .AsNoTracking();

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH)
                queryEnrolledCourses = queryEnrolledCourses.Where(x => x.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN);

            var enrolledCourses = await queryEnrolledCourses
                .Select(x => x.Section.CourseTerm.CourseId)
                .ToListAsync();

            var availableCourses = academicYearCourses
                .Where(x => disapprovedCourses.Contains(x.CourseId) && !enrolledCourses.Contains(x.CourseId))
                .ToList();

            var result = availableCourses
                .Select(x => new EnrolledCourseTemplate
                {
                    CourseId = x.CourseId,
                    CourseName = $"{x.Code} - {x.Name}"
                })
                .ToList();

            return result;
        }

        public async Task<int> GetNumberOfApprovedCourses(Guid studentId)
        {
            var student = await _context.Students.Where(x => x.Id == studentId).FirstOrDefaultAsync();
            var academicYearCourses = await _context.AcademicYearCourses.Where(x => x.CurriculumId == student.CurriculumId).Select(x => x.CourseId).ToListAsync();

            var academicHistories = await _context.AcademicHistories.Where(x => x.StudentId == studentId && x.Approved && academicYearCourses.Contains(x.CourseId))
                .CountAsync();

            return academicHistories;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentPaymentStatusDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, byte? type = null, int status = 1, string search = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            Expression<Func<Student, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Curriculum.Code;
                    break;
                case "4":
                    orderByPredicate = (x) => x.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits);
                    break;
                default:
                    break;
            }

            var query = _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.SECRETARY_OF_PROFESSIONAL_SCHOOLS_ADMINISTRATIONS))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                     .AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId).AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (type.HasValue && (type.Value == 1 || type.Value == 2))
            {
                if (type.Value == 1)
                {
                    switch (status)
                    {
                        case 2:
                            query = query.Where(x => x.User.Payments.Any(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)) && x.User.Payments.Where(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                            break;
                        case 3:
                            query = query.Where(x => x.User.Payments.Any(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)) && !x.User.Payments.Where(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                            break;
                        default:
                            query = query.Where(x => x.User.Payments.Any(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)));
                            break;
                    }
                }

                if (type.Value == 2)
                {
                    switch (status)
                    {
                        case 2:
                            query = query.Where(x => x.User.Payments.Any(y => y.TermId == term.Id && (y.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && y.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)) && x.User.Payments.Where(y => y.TermId == term.Id && (y.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && y.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                            break;
                        case 3:
                            query = query.Where(x => x.User.Payments.Any(y => y.TermId == term.Id && (y.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && y.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)) && !x.User.Payments.Where(y => y.TermId == term.Id && (y.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && y.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                            break;
                        default:
                            query = query.Where(x => x.User.Payments.Any(y => y.TermId == term.Id && (y.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && y.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)));
                            break;
                    }
                }
            }
            else
            {
                switch (status)
                {
                    case 2:
                        query = query.Where(x => x.User.Payments.Where(y => y.TermId == term.Id).All(y => y.PaymentDate.HasValue));
                        break;
                    case 3:
                        query = query.Where(x => !x.User.Payments.Where(y => y.TermId == term.Id).All(y => y.PaymentDate.HasValue));
                        break;
                    default:
                        query = query.Where(x => x.User.Payments.Any(y => y.TermId == term.Id));
                        break;
                }
            }


            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper()) || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {
                    code = s.User.UserName,
                    name = s.User.FullName,
                    career = s.Career.Name,
                    faculty = s.Career.Faculty.Name,
                    credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    id = s.Id,
                    curriculum = s.Curriculum.Code,
                    paid = type.HasValue && type.Value == 1 ? s.User.Payments.Where(y => y.TermId == term.Id
                    && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                    || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue)
                    : type.HasValue && type.Value == 2 ? s.User.Payments.Where(y => y.TermId == term.Id
                    && (y.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                    && y.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue)
                    : s.User.Payments.Where(y => y.TermId == term.Id).All(y => y.PaymentDate.HasValue)
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<StudentPaymentStatus>> GetEnrolledStudentPaymentStatusData(ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, byte? type = null, int status = 1, string search = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            var query = _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.SECRETARY_OF_PROFESSIONAL_SCHOOLS_ADMINISTRATIONS))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                     .AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId).AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (type.HasValue && (type.Value == 1 || type.Value == 2))
            {
                if (type.Value == 1)
                {
                    switch (status)
                    {
                        case 2:
                            query = query.Where(x => x.User.Payments.Where(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                            break;
                        case 3:
                            query = query.Where(x => !x.User.Payments.Where(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                            break;
                        default:
                            break;
                    }
                }

                if (type.Value == 2)
                {
                    switch (status)
                    {
                        case 2:
                            query = query.Where(x => x.User.Payments.Where(y => y.TermId == term.Id && (y.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && y.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                            break;
                        case 3:
                            query = query.Where(x => !x.User.Payments.Where(y => y.TermId == term.Id && (y.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && y.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                switch (status)
                {
                    case 2:
                        query = query.Where(x => x.User.Payments.Where(y => y.TermId == term.Id).All(y => y.PaymentDate.HasValue));
                        break;
                    case 3:
                        query = query.Where(x => !x.User.Payments.Where(y => y.TermId == term.Id).All(y => y.PaymentDate.HasValue));
                        break;
                    default:
                        break;
                }
            }


            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper()) || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            var data = await query
                .Select(s => new StudentPaymentStatus
                {
                    Code = s.User.UserName,
                    Name = s.User.FullName,
                    Career = s.Career.Name,
                    Faculty = s.Career.Faculty.Name,
                    Credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    Curriculum = s.Curriculum.Code,
                    Paid = type.HasValue && type.Value == 1 ? s.User.Payments.Where(y => y.TermId == term.Id
                    && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                    || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue)
                    : type.HasValue && type.Value == 2 ? s.User.Payments.Where(y => y.TermId == term.Id
                    && (y.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                    && y.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue)
                    : s.User.Payments.Where(y => y.TermId == term.Id).All(y => y.PaymentDate.HasValue)
                }).ToListAsync();

            return data;
        }


        public async Task<List<StudentPaymentStatus>> GetPostPaymentEnrolledStudentData(ClaimsPrincipal User, Guid termId, Guid? facultyId = null, Guid? careerId = null, int? year = null, int status = 1, string search = null)
        {
            var term = await _context.Terms.FindAsync(termId);

            var query = _context.Students
                .Where(x => x.StudentSections.Any(ss => ss.Section.CourseTerm.TermId == termId))
                .AsNoTracking();

            if (User.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY) || User.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || User.IsInRole(ConstantHelpers.ROLES.SECRETARY_OF_PROFESSIONAL_SCHOOLS_ADMINISTRATIONS))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var qryCareers = _context.Careers
                     .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                     .AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else if (User.IsInRole(ConstantHelpers.ROLES.DEAN) || User.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || User.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var qryCareers = _context.Careers.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId || x.Faculty.AdministrativeAssistantId == userId).AsNoTracking();

                if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                var careers = qryCareers.Select(x => x.Id).ToHashSet();
                query = query.Where(x => careers.Contains(x.CareerId));
            }
            else
            {
                if (facultyId.HasValue && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);
                if (careerId.HasValue && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (year.HasValue)
            {
                if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
                    query = query.Where(x => x.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) == year || (!x.AcademicSummaries.Where(y => y.TermId == termId).Any() && x.CurrentAcademicYear == year));
                else
                    query = query.Where(x => x.CurrentAcademicYear == year);
            }

            switch (status)
            {
                case 2:
                    query = query.Where(x => x.User.Payments.Where(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                    break;
                case 3:
                    query = query.Where(x => !x.User.Payments.Where(y => y.TermId == term.Id && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue));
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.UserName.ToUpper().Contains(search.ToUpper()) || x.User.FullName.ToUpper().Contains(search.ToUpper()));

            var data = await query
                .Select(s => new StudentPaymentStatus
                {
                    Code = s.User.UserName,
                    Name = s.User.FullName,
                    Career = s.Career.Name,
                    Faculty = s.Career.Faculty.Name,
                    Credits = s.StudentSections.Where(ss => ss.Section.CourseTerm.TermId == termId).Sum(ss => ss.Section.CourseTerm.Course.Credits),
                    Curriculum = s.Curriculum.Code,
                    Paid = s.User.Payments.Where(y => y.TermId == term.Id
                    && (y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT
                    || y.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)).All(y => y.PaymentDate.HasValue),
                    AcademicYear = term.Status == ConstantHelpers.TERM_STATES.FINISHED ?
                    s.AcademicSummaries.Where(y => y.TermId == termId).Any() ? s.AcademicSummaries.Where(y => y.TermId == termId).Max(y => y.StudentAcademicYear) : s.CurrentAcademicYear
                    : s.CurrentAcademicYear,
                }).ToListAsync();

            return data;
        }

        public async Task<object> GetSuitableStudentsReportDatatable(Guid? facultyId, Guid? careerId, Guid? programId, int? year, ClaimsPrincipal user = null)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var query = _context.Students
                .FilterActiveStudents()
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.DEAN))
                {
                    query = query.Where(x => x.Career.Faculty.SecretaryId == userId || x.Career.Faculty.DeanId == userId);
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (programId.HasValue && programId != Guid.Empty)
                query = query.Where(x => x.AcademicProgramId == programId);

            if (year.HasValue && year > 0)
                query = query.Where(x => x.CurrentAcademicYear == year);

            var students = await query.ToListAsync();

            var entrant = students.Count(x => x.Status == ConstantHelpers.Student.States.ENTRANT);
            var unbeaten = students.Count(x => x.Status == ConstantHelpers.Student.States.UNBEATEN);
            var regular = students.Count(x => x.Status == ConstantHelpers.Student.States.REGULAR || x.Status == ConstantHelpers.Student.States.TRANSFER);
            var irregular = students.Count(x => x.Status == ConstantHelpers.Student.States.IRREGULAR || x.Status == ConstantHelpers.Student.States.REPEATER || x.Status == ConstantHelpers.Student.States.OBSERVED);

            var data = new List<object>
            {
                new {
                    name = "Ingresantes",
                    count = entrant
                },
                new {
                    name = "Invictos",
                    count = unbeaten
                },
                new {
                    name = "Regulares",
                    count = regular
                },
                new {
                    name = "Irregulares",
                    count = irregular
                },
            };

            return data;
        }

        public async Task<object> GetSanctionedStudentsReportDatatable(Guid? facultyId, Guid? careerId, Guid? programId, int? year, ClaimsPrincipal user = null)
        {
            var query = _context.Students
                .Where(x => x.Status == ConstantHelpers.Student.States.SANCTIONED)
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.DEAN))
                {
                    query = query.Where(x => x.Career.Faculty.SecretaryId == userId || x.Career.Faculty.DeanId == userId);
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (programId.HasValue && programId != Guid.Empty)
                query = query.Where(x => x.AcademicProgramId == programId);

            if (year.HasValue && year > 0)
                query = query.Where(x => x.CurrentAcademicYear == year);

            var sanctionedStudents = await query
                .Select(x => new
                {
                    TermId = x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.TermId).FirstOrDefault(),
                    Term = x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.Term.Name).FirstOrDefault(),
                    EndDate = x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.Term.EndDate).FirstOrDefault(),
                })
                .ToListAsync();

            var terms = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2"))
                .Select(x => new
                {
                    x.Id,
                    x.StartDate
                }).ToListAsync();

            var minTermsRequired = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.SANCTIONED_STUDENT_TERMS_TO_STUDY));

            var santionedStudentsStatus = sanctionedStudents
                .Select(x => terms.Where(y => x.EndDate < y.StartDate && y.StartDate <= DateTime.UtcNow).Count() >= minTermsRequired)
                .ToList();

            var sanctioned = santionedStudentsStatus.Where(x => x == true).Count();

            var data = new List<object>
            {
                new {
                    name = "Sancionados",
                    count = sanctioned
                }
            };

            return data;
        }

        public async Task<List<StudentJobExchangeReportTemplate>> ReportGlobalListData(string dni = null, string userName = null, Guid? termId = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null)
        {
            var query = _context.Students.AsNoTracking();

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (academicProgramId != null)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (!string.IsNullOrEmpty(dni))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    dni = $"\"{dni}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.Dni, dni));
                }
                else
                    query = query.Where(x => x.User.Dni.ToUpper().Contains(dni.ToUpper()));
            }

            if (!string.IsNullOrEmpty(userName))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    userName = $"\"{userName}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.UserName, userName));
                }
                else
                    query = query.Where(x => x.User.UserName.ToUpper().Contains(userName.ToUpper()));
            }

            if (termId != null)
            {
                query = query.Where(x => x.GraduationTermId == termId);
            }

            var data = await query
                .Select(x => new StudentJobExchangeReportTemplate
                {
                    UserName = x.User.UserName,
                    PaternalSurname = x.User.PaternalSurname,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    Career = x.Career.Name
                })
                .ToListAsync();

            return data;
        }

        public async Task<StudentConstancy> GetStudentConstancy(Guid studentId, Guid termId)
        {
            var result = await _context.StudentInformations
                .Where(x => x.StudentId == studentId && x.TermId == termId)
                .Select(x => new StudentConstancy
                {
                    FullName = x.Student.User.FullName,
                    CareerName = x.Student.Career.Name,
                    FacultyName = x.Student.Career.Faculty.Name,
                    TermName = x.Term.Name,
                    UserName = x.Student.User.UserName
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<StudentConstancy> GetStudentLastConstancy(Guid studentId)
        {
            var result = await _context.Students
                .Where(x => x.Id == studentId && x.StudentInformationId != null)
                .Select(x => new StudentConstancy
                {
                    FullName = x.User.FullName,
                    CareerName = x.Career.Name,
                    FacultyName = x.Career.Faculty.Name,
                    TermName = x.StudentInformation.Term.Name,
                    UserName = x.User.UserName
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<JobExchangeGraduatedDataTemplate>> GetJobExchangeReportGraduatedData(string userName = null, string dni = null, string fullName = null, int? studentState = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null, Guid? graduationTermId = null, int? graduationYear = null)
        {
            var query = _context.Students
                           .Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED ||
                                       x.Status == ConstantHelpers.Student.States.BACHELOR ||
                                       x.Status == ConstantHelpers.Student.States.QUALIFIED)
                           .AsNoTracking();

            if (!string.IsNullOrEmpty(userName))
            {
                var userNameTrimmed = userName.Trim();
                query = query.Where(x => x.User.UserName.ToUpper().Contains(userNameTrimmed.ToUpper()));
            }

            if (!string.IsNullOrEmpty(dni))
            {
                var dniTrimmed = dni.Trim();
                query = query.Where(x => x.User.Dni.ToUpper().Contains(dniTrimmed.ToUpper()));
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                var fullNameTrimmed = fullName.Trim();
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    fullNameTrimmed = $"\"{fullNameTrimmed}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, fullNameTrimmed));
                }
                else
                    query = query.Where(x => x.User.Name.ToUpper().Contains(fullNameTrimmed.ToUpper())
                                    || x.User.PaternalSurname.ToUpper().Contains(fullNameTrimmed.ToUpper())
                                    || x.User.MaternalSurname.ToUpper().Contains(fullNameTrimmed.ToUpper()));
            }


            if (studentState != null)
                query = query.Where(x => x.Status == studentState.Value);

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (academicProgramId != null)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (admissionTermId != null)
                query = query.Where(x => x.AdmissionTermId == admissionTermId);

            if (graduationTermId != null)
                query = query.Where(x => x.GraduationTermId == graduationTermId);

            if (graduationYear != null)
                query = query.Where(x => x.GraduationTerm.Year == graduationYear.Value);

            var data = await query
                .Select(x => new JobExchangeGraduatedDataTemplate
                {
                    UserName = x.User.UserName,
                    PaternalSurname = x.User.PaternalSurname ?? "-",
                    MaternalSurname = x.User.MaternalSurname ?? "-",
                    Name = x.User.Name ?? "-",
                    Dni = x.User.Dni ?? "-",
                    FacultyName = x.Career.Faculty.Name,
                    CareerName = x.Career.Name,
                    AcademicProgramName = x.AcademicProgram.Name ?? "-",
                    CurriculumCode = x.Curriculum.Code,
                    StudentState = ConstantHelpers.Student.States.VALUES.ContainsKey(x.Status) ?
                            ConstantHelpers.Student.States.VALUES[x.Status] : "-",
                    CampusName = x.Campus.Name ?? "",
                    AdmissionTermName = x.AdmissionTerm.Name,
                    AdmissionTypeName = x.AdmissionType.Name,
                    FirstEnrollmentTermName = x.FirstEnrollmentTermId != null ? x.FirstEnrollmentTerm.Name : "-",
                    GraduationTermName = x.GraduationTermId.HasValue ? x.GraduationTerm.Name : "-",
                    WeightedAverageCumulative = (x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number)
                                .Select(y => y.WeightedAverageCumulative).FirstOrDefault()).ToString("0.00"),
                    Sex = ConstantHelpers.SEX.ABREV.ContainsKey(x.User.Sex)
                            ? ConstantHelpers.SEX.ABREV[x.User.Sex] : "-",
                    PhoneNumber = x.User.PhoneNumber ?? "-",
                    Email = x.User.Email ?? "-",
                    Address = x.User.Address ?? "-",
                    Department = x.User.Department.Name ?? "-",
                    Province = x.User.Province.Name ?? "-",
                    District = x.User.District.Name ?? "-",
                })
                .ToListAsync();

            return data;

        }

        public async Task<List<JobExchangeStudentDataTemplate>> GetJobExchangeReportStudentData(string userName = null, string dni = null, string fullName = null, Guid? facultyId = null, Guid? careerId = null, Guid? academicProgramId = null, Guid? admissionTermId = null)
        {
            var query = _context.Students
                .AsNoTracking();

            if (!string.IsNullOrEmpty(userName))
            {
                var userNameTrimmed = userName.Trim();
                query = query.Where(x => x.User.UserName.ToUpper().Contains(userNameTrimmed.ToUpper()));
            }

            if (!string.IsNullOrEmpty(dni))
            {
                var dniTrimmed = dni.Trim();
                query = query.Where(x => x.User.Dni.ToUpper().Contains(dniTrimmed.ToUpper()));
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                var fullNameTrimmed = fullName.Trim();
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    fullNameTrimmed = $"\"{fullNameTrimmed}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, fullNameTrimmed));
                }
                else
                    query = query.Where(x => x.User.Name.ToUpper().Contains(fullNameTrimmed.ToUpper())
                                    || x.User.PaternalSurname.ToUpper().Contains(fullNameTrimmed.ToUpper())
                                    || x.User.MaternalSurname.ToUpper().Contains(fullNameTrimmed.ToUpper()));
            }

            if (facultyId != null)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            if (academicProgramId != null)
                query = query.Where(x => x.AcademicProgramId == academicProgramId);

            if (admissionTermId != null)
                query = query.Where(x => x.AdmissionTermId == admissionTermId);

            var data = await query
                .Select(x => new JobExchangeStudentDataTemplate
                {
                    UserName = x.User.UserName,
                    PaternalSurname = x.User.PaternalSurname ?? "-",
                    MaternalSurname = x.User.MaternalSurname ?? "-",
                    Name = x.User.Name ?? "-",
                    Dni = x.User.Dni ?? "-",
                    FacultyName = x.Career.Faculty.Name,
                    CareerName = x.Career.Name,
                    AcademicProgramName = x.AcademicProgram.Name ?? "-",
                    CurriculumCode = x.Curriculum.Code,
                    //Estado de practicas ???
                    HasExperience = x.StudentExperiences.Count == 0 ? "NO" : "SI",
                    CampusName = x.Campus.Name ?? "",
                    AdmissionTermName = x.AdmissionTerm.Name,
                    AdmissionTypeName = x.AdmissionType.Name,
                    FirstEnrollmentTermName = x.FirstEnrollmentTermId != null ? x.FirstEnrollmentTerm.Name : "-",
                    Sex = ConstantHelpers.SEX.ABREV.ContainsKey(x.User.Sex)
                            ? ConstantHelpers.SEX.ABREV[x.User.Sex] : "-",
                    PhoneNumber = x.User.PhoneNumber ?? "-",
                    Email = x.User.Email ?? "-",
                    Address = x.User.Address ?? "-",
                    Department = x.User.Department.Name ?? "-",
                    Province = x.User.Province.Name ?? "-",
                    District = x.User.District.Name ?? "-",
                }).ToListAsync();

            return data;
        }

        public async Task<object> GetJobExchangeStudentGraduatedQuantityChartData()
        {

            var students = _context.Students
                .AsNoTracking();

            int graduatedCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED).CountAsync();
            int bachelorCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR).CountAsync();
            int qualifiedCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED).CountAsync();

            //Report
            var dataReport = new List<StudentReportTemplate>
            {
                new StudentReportTemplate { DataReport = "Graduado", ValueData = graduatedCount },
                new StudentReportTemplate { DataReport = "Bachiller", ValueData = bachelorCount },
                new StudentReportTemplate { DataReport = "Titulado", ValueData = qualifiedCount },
            };

            var data = dataReport.Select(x => new
            {
                name = x.DataReport,
                y = x.ValueData
            }).ToList();

            return new { data };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedQuantityDatatable(DataTablesStructs.SentParameters sentParameters)
        {

            var students = _context.Students.AsNoTracking();

            int graduatedCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED).CountAsync();
            int bachelorCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR).CountAsync();
            int qualifiedCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED).CountAsync();

            //Report
            var data = new List<StudentReportTemplate>
            {
                new StudentReportTemplate { DataReport = "Graduado", ValueData = graduatedCount },
                new StudentReportTemplate { DataReport = "Bachiller", ValueData = bachelorCount },
                new StudentReportTemplate { DataReport = "Titulado", ValueData = qualifiedCount },
            };

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetJobExchangeStudentGraduatedWorkingCareerQuantityChartData(List<int> studentStatus = null, List<Guid> careers = null)
        {
            var studentsQuery = _context.Students
                .Where(x => (x.Status == ConstantHelpers.Student.States.GRADUATED ||
                            x.Status == ConstantHelpers.Student.States.BACHELOR ||
                            x.Status == ConstantHelpers.Student.States.QUALIFIED) &&
                            x.StudentExperiences.Count > 0)
                .AsNoTracking();

            if (studentStatus != null && studentStatus.Count > 0)
            {
                studentsQuery = studentsQuery.Where(x => studentStatus.Contains(x.Status));
            }

            if (careers != null && careers.Count > 0)
            {
                studentsQuery = studentsQuery.Where(x => careers.Contains(x.CareerId));
            }

            var careersQuery = _context.Careers.AsNoTracking();

            if (careers != null && careers.Count > 0)
            {
                careersQuery = careersQuery.Where(x => careers.Contains(x.Id));
            }

            var careersData = await careersQuery
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .OrderBy(x => x.Id)
                .ToListAsync();

            var studentsData = await studentsQuery
                .Select(x => new
                {
                    CareerId = x.CareerId,
                    StudentState = x.Status
                }).ToListAsync();

            var studentStatesConstants = new List<GenericDictionaryTemplate>
            {
                new GenericDictionaryTemplate { Key = ConstantHelpers.Student.States.GRADUATED , Value = ConstantHelpers.Student.States.VALUES[ConstantHelpers.Student.States.GRADUATED]},
                new GenericDictionaryTemplate { Key = ConstantHelpers.Student.States.BACHELOR , Value = ConstantHelpers.Student.States.VALUES[ConstantHelpers.Student.States.BACHELOR]},
                new GenericDictionaryTemplate { Key = ConstantHelpers.Student.States.QUALIFIED , Value = ConstantHelpers.Student.States.VALUES[ConstantHelpers.Student.States.QUALIFIED]}
            };

            if (studentStatus != null && studentStatus.Count > 0)
            {
                studentStatesConstants = studentStatesConstants.Where(x => studentStatus.Any(y => y == x.Key)).ToList();
            }

            var result = new
            {
                categoriesData = careersData.Select(x => x.Name).ToList(),
                seriesData = studentStatesConstants.Select(x => new
                {
                    name = x.Value,
                    data = careersData.Select(y => studentsData.Where(z => z.CareerId == y.Id && z.StudentState == x.Key).Count()).ToList()
                }).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedWorkingCareerQuantityDatatable(DataTablesStructs.SentParameters sentParameters, List<int> studentStatus = null, List<Guid> careers = null)
        {

            var careersQuery = _context.Careers.AsNoTracking();

            var studentsQuery = _context.Students
                .Where(x => (x.Status == ConstantHelpers.Student.States.GRADUATED ||
                            x.Status == ConstantHelpers.Student.States.BACHELOR ||
                            x.Status == ConstantHelpers.Student.States.QUALIFIED) &&
                            x.StudentExperiences.Count > 0)
                .AsNoTracking();

            if (studentStatus != null && studentStatus.Count > 0)
            {
                studentsQuery = studentsQuery.Where(x => studentStatus.Contains(x.Status));
            }

            if (careers != null && careers.Count > 0)
            {
                studentsQuery = studentsQuery.Where(x => careers.Contains(x.CareerId));
                careersQuery = careersQuery.Where(x => careers.Contains(x.Id));
            }

            var data = await careersQuery
                .Select(x => new
                {
                    CareerName = x.Name,
                    GraduatedQuantity = studentsQuery.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.GRADUATED).Count(),
                    BachelorQuantity = studentsQuery.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.BACHELOR).Count(),
                    QualifiedQuantity = studentsQuery.Where(y => y.CareerId == x.Id && y.Status == ConstantHelpers.Student.States.QUALIFIED).Count(),
                }).ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }
        public async Task<object> GetJobExchangeStudentGraduatedWorkingCareerPercentageChartData()
        {
            var studentsQuery = _context.Students
                .Where(x => (x.Status == ConstantHelpers.Student.States.GRADUATED ||
                            x.Status == ConstantHelpers.Student.States.BACHELOR ||
                            x.Status == ConstantHelpers.Student.States.QUALIFIED) &&
                            x.StudentExperiences.Count > 0)
                .AsNoTracking();

            var careers = _context.Careers.AsNoTracking();

            int total = await studentsQuery.CountAsync();

            var carrers = await _context.Careers
                .Select(x => new
                {
                    x.Name,
                    Quantity = studentsQuery.Where(y => y.CareerId == x.Id).Count()
                })
                .ToListAsync();

            var data = carrers.Select(x => new
            {
                name = x.Name,
                y = x.Quantity
            }).ToList();

            return new { data };
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedWorkingCareerPercentageDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            var careersQuery = _context.Careers.AsNoTracking();

            var studentsQuery = _context.Students
                .Where(x => (x.Status == ConstantHelpers.Student.States.GRADUATED ||
                            x.Status == ConstantHelpers.Student.States.BACHELOR ||
                            x.Status == ConstantHelpers.Student.States.QUALIFIED) &&
                            x.StudentExperiences.Count > 0)
                .AsNoTracking();


            var data = await careersQuery
                .Select(x => new
                {
                    CareerName = x.Name,
                    WorkingQuantity = studentsQuery.Where(y => y.CareerId == x.Id).Count(),
                }).ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }


        public async Task<object> GetJobExchangeStudentGraduatedGraduationYearQuantityChartData(int? startYear = null, int? endYear = null)
        {
            var startYearText = "-";
            var endYearText = "-";

            var students = _context.Students.AsNoTracking();

            int totalStudents = await students.CountAsync();
            if (startYear != null)
            {
                students = students.Where(x => x.GraduationTerm.Year >= startYear.Value);
                startYearText = startYear.Value.ToString();
            }


            if (endYear != null)
            {
                students = students.Where(x => x.GraduationTerm.Year <= endYear.Value);
                endYearText = endYear.Value.ToString();
            }


            int graduatedCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED).CountAsync();
            int bachelorCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR).CountAsync();
            int qualifiedCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED).CountAsync();

            //Report
            var dataReport = new List<StudentReportTemplate>
            {
                new StudentReportTemplate { DataReport = "Graduado", ValueData = graduatedCount },
                new StudentReportTemplate { DataReport = "Bachiller", ValueData = bachelorCount },
                new StudentReportTemplate { DataReport = "Titulado", ValueData = qualifiedCount },
            };

            var data = dataReport.Select(x => new
            {
                name = x.DataReport,
                y = x.ValueData
            }).ToList();

            return new { data, title = $"Cantidad de Egresados del {startYearText} al {endYearText}" };
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedGraduationYearQuantityDatatable(DataTablesStructs.SentParameters sentParameters, int? startYear = null, int? endYear = null)
        {
            var students = _context.Students.AsNoTracking();

            if (startYear != null)
                students = students.Where(x => x.GraduationTerm.Year >= startYear.Value);

            if (endYear != null)
                students = students.Where(x => x.GraduationTerm.Year <= endYear.Value);

            int graduatedCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.GRADUATED).CountAsync();
            int bachelorCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.BACHELOR).CountAsync();
            int qualifiedCount = await students.Where(x => x.Status == ConstantHelpers.Student.States.QUALIFIED).CountAsync();

            //Report
            var data = new List<StudentReportTemplate>
            {
                new StudentReportTemplate { DataReport = "Graduado", ValueData = graduatedCount },
                new StudentReportTemplate { DataReport = "Bachiller", ValueData = bachelorCount },
                new StudentReportTemplate { DataReport = "Titulado", ValueData = qualifiedCount },
            };

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }
    }
}

