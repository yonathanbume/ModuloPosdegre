using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.EnrollmentSurveyStudent;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class EnrollmentSurveyStudentRepository : Repository<EnrollmentSurveyStudent>, IEnrollmentSurveyStudentRepository
    {
        public EnrollmentSurveyStudentRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> HasSurveyCompleted(Guid studentId, Guid termId)
            => await _context.EnrollmentSurveyStudents.AnyAsync(x => x.StudentId == studentId && x.TermId == termId);

        public async Task<ChartJSTemplate> GetSurveyChart(Guid? termId, Guid? facultyId, Guid? careerId, ClaimsPrincipal user = null)
        {
            if(!termId.HasValue || termId == Guid.Empty)
            {
                var term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
                termId = term.Id;
            }

            var query = _context.EnrollmentSurveyStudents.Where(x => x.TermId == termId).AsQueryable();

            if (user.IsInRole(ConstantHelpers.ROLES.DEAN)||user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.Student.Career.Faculty.DeanId == userId||x.Student.Career.Faculty.SecretaryId == userId);
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.Student.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.Student.CareerId == careerId);

            var result = new ChartJSTemplate
            {
                Categories = new string[]
                {
                    "¿Cuenta con laptop o computadora?",
                    "¿Cuenta con smartphone?",
                    "¿Cuenta con acceso a internet permanente?"
                },
                YesNoData = new List<DataTemplate>
                {
                    new DataTemplate
                    {
                        Name = "Sí",
                        Data = new int[]
                        {
                            await query.Where(x=>x.HasComputerOrLaptop).CountAsync(),
                            await query.Where(x=>x.HasSmartphone).CountAsync(),
                            await query.Where(x=>x.HasInternet).CountAsync(),
                        }
                    },
                     new DataTemplate
                    {
                        Name = "No",
                        Data = new int[]
                        {
                            await query.Where(x=>!x.HasComputerOrLaptop).CountAsync(),
                            await query.Where(x=>!x.HasSmartphone).CountAsync(),
                            await query.Where(x=>!x.HasInternet).CountAsync(),
                        }
                    }
                },
                InternetConnectionTypeData = new
                {
                    Name = "Cant. Alumnos",
                    Data = new List<object[]>
                    {
                        new object[]
                        {
                            ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.VALUES[ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.DATAPLAN],
                            await query.Where(x=>x.InternetConnectionType == ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.DATAPLAN).CountAsync()
                        },
                        new object[]
                        {
                            ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.VALUES[ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.ADSL],
                            await query.Where(x=>x.InternetConnectionType == ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.ADSL).CountAsync()
                        },
                        new object[]
                        {
                            ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.VALUES[ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.HFC],
                            await query.Where(x=>x.InternetConnectionType == ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.HFC).CountAsync()
                        },
                        new object[]
                        {
                            ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.VALUES[ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.WIRELESS_INTERNET],
                            await query.Where(x=>x.InternetConnectionType == ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.WIRELESS_INTERNET).CountAsync()
                        }
                    }
                }
            };

            return result;;
        }

        public async Task<ChartPieJSTemplate> GetSurveyStudentsByCareerChart(Guid? termId, ClaimsPrincipal user = null)
        {
            Term term = null;
            if (!termId.HasValue || termId == Guid.Empty)
            {
                term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
                termId = term.Id;
            }
            else
            {
                term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            }

            var data = await _context.EnrollmentSurveyStudents.Where(x => x.TermId == termId).GroupBy(x => x.Student.Career.Name)
                .Select(x => new DataPieTemplate
                {
                    Name = x.Key,
                    Y = x.Count()
                }).ToListAsync();

            var result = new ChartPieJSTemplate
            {
                Data = data,
                Title = term.Name
            };

            return result;
        }

        public async Task<DetailedReportTemplate> GetDetailedReport(Guid? termId)
        {
            Term term = null;
            if (!termId.HasValue || termId == Guid.Empty)
            {
                term = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE).FirstOrDefaultAsync();
                termId = term.Id;
            }
            else
            {
                term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            }

            var result = new DetailedReportTemplate();

            var data = await _context.EnrollmentSurveyStudents
                .Where(x => x.TermId == termId)
                .Where(x=>x.Student.StudentSections.Any(y=>y.Section.CourseTerm.TermId == termId))
                .Select(x => new AnswerTemplate
                {
                    UserName = x.Student.User.UserName,
                    Career = x.Student.Career.Name,
                    FullName = x.Student.User.FullName,
                    HasComputerOrLaptop = x.HasComputerOrLaptop ? "Sí " : "No",
                    HasInternet = x.HasInternet ? "Sí " : "No",
                    HasSmartphone = x.HasSmartphone ? "Sí " : "No",
                    InternetConnectionType = ConstantHelpers.ENROLLMENT_SURVEY.INTERNET_CONNECTION_TYPE.VALUES[x.InternetConnectionType]
                })
                .ToListAsync();

            result.TermName = term.Name;
            result.Answers = data;

            return result;
        }
    }
}
