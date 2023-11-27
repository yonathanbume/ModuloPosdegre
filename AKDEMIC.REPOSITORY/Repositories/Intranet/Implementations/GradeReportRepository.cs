using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.GradeReport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class GradeReportRepository : Repository<GradeReport>, IGradeReportRepository
    {
        public GradeReportRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId, int gradeType, string searchValue = null)
        {
            var query = _context.GradeReports                
                .OrderByDescending(x=>x.CreatedAt)
                .AsNoTracking();
            if (gradeType > 0)
            {
                query = query.Where(x => x.GradeType == gradeType);
            }

            if (careerId.HasValue)
            {
                query = query.Where(x => x.Student.CareerId == careerId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.Name.Contains(searchValue)|| x.Student.User.PaternalSurname.Contains(searchValue)|| x.Student.User.MaternalSurname.Contains(searchValue));
            }

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.Name,
                    x.Student.User.PaternalSurname,
                    x.Student.User.MaternalSurname,
                    CareerName = x.Student.Career.Name,
                    Date = x.Date.ToLocalDateTimeFormat(),
                    type = x.GradeType >0 ?  ConstantHelpers.GRADE_INFORM.DegreeType.VALUES[x.GradeType] : "--"
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

        public async Task<GradeReport> GetGradeReportWithIncludes(Guid id)
        {
            var result = await _context.GradeReports.
                Where(x => x.Id == id)
                .Include(x => x.Student.User)
                .Include(x => x.Student.Career.Faculty)
                .Include(x => x.Student.Curriculum)
                .Include(x => x.Student.AcademicProgram)
                .Include(x => x.Student.AdmissionTerm)
                .Include(x => x.Student.GraduationTerm).FirstOrDefaultAsync();             
            return result;
        }

        public async Task<object> GetStudentByUserName(string username)
        {
            var result = await _context.Students.Include(x=>x.User)
                .Include(x => x.Career.Faculty)
                .Include(x => x.AcademicProgram)
                .Include(x => x.Curriculum)
                .Include(x => x.AdmissionTerm)
                .Include(x => x.GraduationTerm)
                .Include(x => x.RecordHistories)
                .Where(x => x.User.UserName == username && x.RecordHistories.Any(y=>y.Type == ConstantHelpers.RECORDS.BACHELOR ))
                .Select(x => new
                {
                    x.Id,
                    x.User.FullName,
                    Code = x.User.UserName,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,                    
                    x.User.Name,
                    FacultyName = x.Career.Faculty.Name,
                    CareerName = x.Career.Name,
                    CurricularSystem = ConstantHelpers.CURRICULUM.STUDY_REGIME.VALUES[x.Curriculum.StudyRegime],
                    Curriculum = x.Curriculum.Name,
                    AcademicProgram = x.AcademicProgram.Name,
                    Year = DateTime.UtcNow.Year,
                    Number = x.RecordHistories.Where(y=>y.Type == ConstantHelpers.RECORDS.BACHELOR).FirstOrDefault().Code,
                    Date = DateTime.UtcNow.ToLocalDateFormat(),
                    AdmissionTermId = x.AdmissionTerm.Id,
                    GraduationTermId = x.GraduationTermId,
                    HasGraduationTerm = x.GraduationTermId.HasValue,
                    PromotionGrade = _context.AcademicSummaries.Include(y=>y.Term).Where(y=>y.StudentId == x.Id).OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number)
                            .Select(y => y.WeightedAverageGrade).FirstOrDefault().ToString("0.00"),
                    Credits = _context.AcademicHistories.Where(s=> s.Approved && s.StudentId == x.Id && s.Course.AcademicYearCourses.Any(y=> y.CurriculumId == x.CurriculumId)).Sum(s=> s.Course.Credits)

                }).FirstOrDefaultAsync();
            return result;
        }


        public async Task<StudentInfoTemplate> GetStudentByStudentId(Guid studentId)
        {
            var result = await _context.Students.Include(x => x.User)
                .Include(x => x.Career.Faculty)
                .Include(x => x.AcademicProgram)
                .Include(x => x.Curriculum)
                .Include(x => x.AdmissionTerm)
                .Include(x => x.GraduationTerm)
                .Include(x => x.RecordHistories)
                .Where(x => x.Id == studentId)
                .Select(x => new StudentInfoTemplate
                {
                  Career = x.Career.Name,
                  Credits = _context.AcademicHistories.Where(s => s.Approved && s.StudentId == x.Id && s.Course.AcademicYearCourses.Any(y => y.CurriculumId == x.CurriculumId)).Sum(s => s.Course.Credits),
                  FullName = x.User.FullName
                
                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<string> GetPromotionGrade(Guid studentId)
        {
            var result = _context.AcademicSummaries.Include(y => y.Term).Where(y => y.StudentId == studentId).OrderByDescending(y => y.Term.Year).ThenByDescending(y => y.Term.Number)
                             .Select(y => y.WeightedAverageGrade.ToString("0.00"));
            return await result.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistGradeReport(Guid studentId, int gradeType)
        {
            return await _context.GradeReports.AnyAsync(x => x.GradeType == gradeType && x.StudentId == studentId);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetGradeReportByStudentDatatable(DataTablesStructs.SentParameters sentParameters,  int gradeType, string userId, string searchValue = null)
        {
            var query = _context.GradeReports
               .Where(x => x.GradeType == gradeType && x.Student.UserId == userId)
               .OrderByDescending(x => x.CreatedAt)
               .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.Name.Contains(searchValue) || x.Student.User.PaternalSurname.Contains(searchValue) || x.Student.User.MaternalSurname.Contains(searchValue));
            }

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.Name,
                    x.Student.User.PaternalSurname,
                    x.Student.User.MaternalSurname,
                    CareerName = x.Student.Career.Name,
                    Date = x.Date.ToLocalDateFormat(),
                    Status = ConstantHelpers.GRADE_INFORM.STATUS.VALUES[x.Status]

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

        public async Task<object> GetStudentByUserNameBachelor(string username)
        {
            
            var gradeReportStudent = await _context.GradeReports.Include(x=>x.Student.RecordHistories).Where(x => x.Student.User.UserName == username && x.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR && x.Student.RecordHistories.Any(y => y.Type == ConstantHelpers.RECORDS.JOBTITLE))
                 .Select(x => new
                 {
                     Id = x.StudentId,
                     x.Student.User.FullName,
                     Code = x.Student.User.UserName,
                     x.Student.User.PaternalSurname,
                     x.Student.User.MaternalSurname,
                     x.Student.User.Name,
                     FacultyName = x.Student.Career.Faculty.Name,
                     CareerName = x.Student.Career.Name,
                     CurricularSystem = ConstantHelpers.CURRICULUM.STUDY_REGIME.VALUES[x.Student.Curriculum.StudyRegime],
                     Curriculum = x.Student.Curriculum.Name,
                     AcademicProgram = x.Student.AcademicProgram.Name,
                     Year = DateTime.UtcNow.Year,
                     Date = DateTime.UtcNow.ToLocalDateFormat(),
                     AdmissionTermId = x.Student.AdmissionTermId,
                     GraduationTermId = x.Student.GraduationTermId,
                     GraduationDate = x.GraduationDate.ToLocalDateFormat(),
                     PromotionGrade = x.PromotionGrade,
                     x.Credits,
                     x.YearsStudied,
                     x.SemesterStudied,
                     x.BachelorOrigin,
                     x.PedagogicalTitleOrigin,
                     x.StudyModality,
                     x.OriginDegreeCountry,
                     x.Observation,
                     x.ResearchWork,
                     x.ResearchWorkURL,
                     x.Number

                 }).FirstOrDefaultAsync();
            return gradeReportStudent;
        }

        public async Task<GradeReport> GetGradeReportBachelor(Guid studentId)
        {
            var gradeReportStudentBachelor = await _context.GradeReports.Where(x => x.StudentId == studentId && x.GradeType == ConstantHelpers.GRADE_INFORM.DegreeType.BACHELOR).FirstOrDefaultAsync();
            return gradeReportStudentBachelor;
        }

        public async Task<string> GetEndDateByTermId(Guid termId)
        {
            var result = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();
            return result.EndDate.ToLocalDateFormat();
        }

        public async Task<GradeReport> GetByStudentIdAndGradeType(Guid studentId, int gradeType)
        {
            var result = await _context.GradeReports
                .Where(x => x.StudentId == studentId && x.GradeType == gradeType)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}

