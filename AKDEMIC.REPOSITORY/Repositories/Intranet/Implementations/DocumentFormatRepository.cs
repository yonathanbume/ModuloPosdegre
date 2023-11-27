using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.DocumentFormat;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class DocumentFormatRepository : Repository<AKDEMIC.ENTITIES.Models.Intranet.DocumentFormat>, IDocumentFormatRepository
    {
        public DocumentFormatRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByRecordType(byte type)
            => await _context.DocumentFormats.AnyAsync(x => x.Id == type);

        public async Task<DataTablesStructs.ReturnedData<object>> GetDocumentFormatsDatatable(DataTablesStructs.SentParameters parameters)
        {
            var query = _context.DocumentFormats.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    x.Id,
                    Name = ConstantHelpers.RECORDS.VALUES[x.Id],
                })
                .ToListAsync();

            int recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DocumentFormatTemplate> GetParsedDocumentFormat(byte recordType, Guid studentId, Guid? termId = null)
        {
            var documentFormat = await _context.DocumentFormats.Where(x => x.Id == recordType)
                .Select(x=> new DocumentFormatTemplate
                {
                    Id = x.Id,
                    Content = x.Content,
                    Title = x.Title
                })
                .FirstOrDefaultAsync();

            if (documentFormat == null)
                return null;

            if (string.IsNullOrEmpty(documentFormat.Content))
            {
                documentFormat.Content = string.Empty;
                return documentFormat;
            }

            var studentInformation = await _context.Students.Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.User.FullName,
                    x.User.Document,
                    Faculty = x.Career.Faculty.Name,
                    Career = x.Career.Name,
                    x.CurrentAcademicYear,
                    LastWeightedAverage = x.AcademicSummaries.OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number).Select(y => y.WeightedAverageGrade).FirstOrDefault(),
                    AdmissionTerm = x.AdmissionTerm.Name,
                    GraduationTerm = x.GraduationTerm.Name,
                    FirstEnrollmentTerm = x.StudentSections.OrderBy(y => y.Section.CourseTerm.Term.Year).ThenBy(y => y.Section.CourseTerm.Term.Number).Select(y => y.Section.CourseTerm.Term.Name).FirstOrDefault(),
                    ApprovedCredits = x.Curriculum.AcademicYearCourses.Where(y => x.AcademicHistories.Where(z => z.Approved).Select(z => z.CourseId).Contains(y.CourseId)).Sum(y => y.Course.Credits),
                    StatusText = ConstantHelpers.Student.States.VALUES[x.Status],
                    Curriculum = x.Curriculum.Code,
                    x.User.UserName,
                    FirstEnrollmentDate = x.FirstEnrollmentDate
                })
                .FirstOrDefaultAsync();

            var reg = new Regex(@"\{{(.*?)\}}");

            var variablesFound = Regex.Matches(documentFormat.Content, @"\{{(.*?)\}}").Cast<Match>().Select(m => m.Groups[1].Value).ToList();

            foreach (var item in variablesFound)
            {
                var variable = ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.VALUES.Where(x => x.ToLower().Trim() == item.ToLower().Trim()).FirstOrDefault();

                switch (variable)
                {
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_FULLNAME:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_FULLNAME + "}}", studentInformation.FullName);
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_DNI:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_DNI + "}}", studentInformation.Document);
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_FACULTY:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_FACULTY + "}}", studentInformation.Faculty);
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_CAREER:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_CAREER + "}}", studentInformation.Career);
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_TERM_SPECIFIC:
                        if (termId.HasValue)
                        {
                            var term = await _context.Terms.Where(x => x.Id == termId).Select(x => x.Name).FirstOrDefaultAsync();
                            documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_TERM_SPECIFIC + "}}", term);
                        }
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.WEIGHTED_AVERAGE:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.WEIGHTED_AVERAGE + "}}", studentInformation.LastWeightedAverage.ToString("0.00"));
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.ADMISSION_TERM:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.ADMISSION_TERM + "}}", studentInformation.AdmissionTerm);
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.GRADUATION_TERM:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.GRADUATION_TERM + "}}", studentInformation.GraduationTerm);
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.FIRST_ENROLLMENT_TERM:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.FIRST_ENROLLMENT_TERM + "}}", studentInformation.FirstEnrollmentTerm);
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.APPROVED_CREDITS:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.APPROVED_CREDITS + "}}", studentInformation.ApprovedCredits.ToString("#0.00"));
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_STATUS:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_STATUS + "}}", studentInformation.StatusText);
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_CURRICULUM:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_CURRICULUM + "}}", studentInformation.Curriculum);
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_USERNAME:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.STUDENT_USERNAME + "}}", studentInformation.UserName);
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.WEIGHTED_AVERAGE_TERM_SPECIFIC:
                        if (termId.HasValue)
                        {
                            var grade = await _context.AcademicSummaries.Where(x =>x.StudentId == studentId &&  x.TermId == termId).Select(x => x.WeightedAverageGrade).FirstOrDefaultAsync();
                            documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.WEIGHTED_AVERAGE_TERM_SPECIFIC + "}}", grade.ToString("#0.00"));
                        }
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.FIRST_ENROLLMENT_DATE:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.FIRST_ENROLLMENT_DATE + "}}", studentInformation.FirstEnrollmentDate.ToLocalDateFormat());
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.FIRST_ENROLLMENT_DATE_TEXT:

                        if (studentInformation.FirstEnrollmentDate.HasValue)
                        {
                            var localFirstEnrollmentDate = studentInformation.FirstEnrollmentDate.ToDefaultTimeZone();
                            documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.FIRST_ENROLLMENT_DATE_TEXT + "}}", $"{studentInformation.FirstEnrollmentDate.Value.Day} de {ConstantHelpers.MONTHS.VALUES[studentInformation.FirstEnrollmentDate.Value.Month]} del {studentInformation.FirstEnrollmentDate.Value.Year}");
                        }
                        else
                        {
                            documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.FIRST_ENROLLMENT_DATE_TEXT + "}}", "-");
                        }
                        break;
                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.CURRENT_ACADEMIC_YEAR_STUDENT:
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.CURRENT_ACADEMIC_YEAR_STUDENT + "}}", studentInformation.CurrentAcademicYear.ToString());
                        break;

                    case ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.ACADEMIC_YEAR_STUDENT_TERM_SPECIFIC:
                        var academicYearHistory = await _context.AcademicSummaries.Where(x => x.StudentId == studentId && x.TermId == termId).Select(x => x.StudentAcademicYear).FirstOrDefaultAsync();
                        documentFormat.Content = documentFormat.Content.Replace("{{" + ConstantHelpers.DOCUMENT_FORMAT.VARIABLES.ACADEMIC_YEAR_STUDENT_TERM_SPECIFIC + "}}", academicYearHistory.ToString());
                        break;

                    default:
                        break;
                }

            }

            return documentFormat;
        }
    }
}
