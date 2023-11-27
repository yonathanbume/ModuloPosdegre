using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class RecognitionRepository : Repository<Recognition>, IRecognitionRepository
    {
        public RecognitionRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
        {
            Expression<Func<Recognition, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Student.Career.Faculty.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.Recognitions
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
               || x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Student.User.UserName,
                    name = x.Student.User.FullName,
                    faculty = x.Student.Career.Faculty.Name,
                    career = x.Student.Career.Name,
                    resolution = x.Resolution.Number,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    studentId = x.StudentId,
                    file = x.Resolution.FilePath
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentsDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? facultyId = null, Guid? careerId = null, Guid? admissionType = null, ClaimsPrincipal user = null)
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
                    orderByPredicate = (x) => x.Curriculum.Code;
                    break;
                default:
                    break;
            }

            var query = _context.Students
                .AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    if (facultyId.HasValue && facultyId != Guid.Empty) qryCareers = qryCareers.Where(x => x.FacultyId == facultyId);

                    if (careerId.HasValue && careerId != Guid.Empty) qryCareers = qryCareers.Where(x => x.Id == careerId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }
            else
            {
                if (facultyId != null && facultyId != Guid.Empty) query = query.Where(x => x.Career.FacultyId == facultyId);

                if (careerId != null && careerId != Guid.Empty) query = query.Where(x => x.CareerId == careerId);
            }

            if (admissionType.HasValue && admissionType != Guid.Empty)
            {
                query = query.Where(x => x.AdmissionTypeId == admissionType.Value);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
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
                    id = x.Id,
                    code = x.User.UserName,
                    name = x.User.FullName,
                    faculty = x.Career.Faculty.Name,
                    career = x.Career.Name,
                    admissionType = x.AdmissionType.Name,
                    curriculum = x.Curriculum.Code,
                    academicyear = x.CurrentAcademicYear
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

        public override async Task<Recognition> Get(Guid id)
        {
            var recognition = await _context.Recognitions
                .Where(x => x.Id == id)
                .Select(x => new Recognition
                {
                    Id = id,
                    Student = new Student
                    {
                        Id = x.Student.Id,
                        CurrentAcademicYear = x.Student.CurrentAcademicYear,
                        User = new ApplicationUser
                        {
                            UserName = x.Student.User.UserName,
                            Name = x.Student.User.Name,
                            PaternalSurname = x.Student.User.PaternalSurname,
                            MaternalSurname = x.Student.User.MaternalSurname,
                            FullName = x.Student.User.FullName
                        },
                        Career = new Career
                        {
                            Name = x.Student.Career.Name,
                            Faculty = new Faculty
                            {
                                Name = x.Student.Career.Faculty.Name
                            }
                        },
                        Curriculum = new Curriculum
                        {
                            Year = x.Student.Curriculum.Year,
                            Code = x.Student.Curriculum.Code
                        }
                    },
                    Comment = x.Comment,
                    Resolution = new Resolution
                    {
                        Number = x.Resolution.Number,
                        IssueDate = x.Resolution.IssueDate,
                        FilePath = x.Resolution.FilePath
                    }
                    //HasHistory = x.Student.AcademicHistories.Any(),
                    //Term = term.Name,
                })
                .FirstOrDefaultAsync();

            return recognition;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCoursesDataDatatable(DataTablesStructs.SentParameters sentParameters, Guid recognitionId, string searchValue = null)
        {
            var recognition = await _context.Recognitions.FindAsync(recognitionId);

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == recognition.StudentId
                && x.Type == ConstantHelpers.AcademicHistory.Types.CONVALIDATION
                && x.Validated)
                .Select(x => new
                {
                    x.CourseId,
                    x.Grade
                })
                .ToListAsync();

            var courseRecognitions = await _context.CoursesRecognition
                .IgnoreQueryFilters()
                .Where(x => x.RecognitionId == recognitionId)
                .Select(x => new
                {
                    id = x.CourseId,
                    code = x.Course.Code,
                    course = x.Course.Name,
                    credits = x.Course.Credits,
                    x.DeletedAt
                })
                .ToListAsync();

            var data = courseRecognitions
                .Select(x => new
                {
                    x.id,
                    x.code,
                    x.course,
                    x.credits,
                    grade = x.DeletedAt.HasValue ? "Eliminado"
                    : academicHistories.Any(y => y.CourseId == x.id && y.Grade > 0) ? academicHistories.FirstOrDefault(y => y.CourseId == x.id).Grade.ToString()
                    : "Convalidado"
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

        public async Task<Recognition> GetByStudentId(Guid studentId)
        {
            return await _context.Recognitions
                .Include(x => x.Resolution)
                .Include(x => x.CourseRecognitions)
                 .Where(x => x.StudentId == studentId).FirstOrDefaultAsync();
        }
    }
}
