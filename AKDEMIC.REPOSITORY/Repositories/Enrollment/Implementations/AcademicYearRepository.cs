namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    //public class AcademicYearRepository : Repository<AcademicYear>, IAcademicYearRepository
    //{
    //    public AcademicYearRepository(AkdemicContext context) : base(context) { }

    //    public async Task<IEnumerable<AcademicYear>> GetAllWithAcademicYearCoursesByCurriculum(Guid curriculumId)
    //        => await _context.AcademicYears
    //        .Where(x => x.CurriculumId == curriculumId)
    //        .OrderBy(x => x.Number)
    //        .Select(x => new AcademicYear
    //        {
    //            Id = x.Id,
    //            Number = x.Number,
    //            AcademicYearCourses = x.AcademicYearCourses.Select(ay => new AcademicYearCourse
    //            {
    //                Course = new Course
    //                {
    //                    Name = ay.Course.Name,
    //                    Code = ay.Course.Code,
    //                    Credits = ay.Course.Credits
    //                }
    //            }).ToList()
    //        }).ToListAsync();

    //    public async Task<IEnumerable<AcademicYear>> GetAllWithAcademicYearCoursesAndHistoriesByCurriculumAndStudent(Guid curriculumId, Guid studentId)
    //        => await _context.AcademicYears
    //        .Where(x => x.CurriculumId == curriculumId)
    //        .OrderBy(x => x.Number)
    //        .Select(x => new AcademicYear
    //        {
    //            Number = x.Number,
    //            AcademicYearCourses = x.AcademicYearCourses
    //            .OrderBy(ay => ay.Course.Credits)
    //            .ThenBy(ay => ay.Course.Code)
    //            .Select(ay => new AcademicYearCourse
    //            {
    //                Course = new Course
    //                {
    //                    Name = ay.Course.Name,
    //                    Code = ay.Course.Code,
    //                    Credits = ay.Course.Credits,
    //                    AcademicHistories = ay.Course.AcademicHistories
    //                    .Where(ah => ah.StudentId == studentId)
    //                    .OrderBy(ah => ah.Term.StartDate)
    //                    .Select(ah => new AcademicHistory
    //                    {
    //                        Approved = ah.Approved,
    //                        Validated = ah.Validated,
    //                        Grade = ah.Grade,
    //                        TermId = ah.TermId,
    //                        Term = new ENTITIES.Models.Enrollment.Term
    //                        {
    //                            Name = ah.Term.Name,
    //                            Number = ah.Term.Number,
    //                            Year = ah.Term.Year
    //                        }
    //                    }).ToList()
    //                }
    //            }).ToList()
    //        }).ToListAsync();

    //    public async Task<IEnumerable<AcademicYear>> GetAllByCurriculum(Guid curriculumId)
    //        => await _context.AcademicYears
    //        .Where(x => x.CurriculumId == curriculumId)
    //        .OrderBy(x => x.Number)
    //        .Select(x => new AcademicYear
    //        {
    //            Id = x.Id,
    //            Number = x.Number
    //        }).ToListAsync();

    //    public Task<bool> AnyByCurriculumIdAndNumber(Guid curriculumId, byte number)
    //        => _context.AcademicYears.AnyAsync(x => x.CurriculumId == curriculumId && x.Number == number);

    //    public async Task<IEnumerable<AcademicYearTemplateA>> GetAllAsModelA(Guid? curriculumId = null, string academicProgramCode = null)
    //    {
    //        if (string.IsNullOrEmpty(academicProgramCode))
    //        {
    //            var modelAsync = await _context.AcademicYears
    //                .Where(x => x.CurriculumId == curriculumId)
    //                .Select(x => new AcademicYearTemplateA
    //                {
    //                    Id = x.Id,
    //                    Number = x.Number,
    //                    Courses = x.AcademicYearCourses.Select(y => new AcademicYearTemplateACourse
    //                    {
    //                        Id = y.Id,
    //                        CourseId = y.CourseId,
    //                        Course = y.Course.Name,
    //                        IsElective = y.IsElective,
    //                        PreRequisites = y.PreRequisites.Select(p => new AcademicYearTemplateAPreRequesite
    //                        {
    //                            Id = p.Id,
    //                            Course = p.Course.Name
    //                        }).ToList()
    //                    }).ToList()
    //                }).OrderBy(x => x.Number).ToListAsync();

    //            return modelAsync;
    //        }
    //        else
    //        {
    //            if (academicProgramCode == "00")
    //            {
    //                var modelAsync = await _context.AcademicYears
    //                    .Where(x => x.CurriculumId == curriculumId)
    //                    .Select(x => new AcademicYearTemplateA
    //                    {
    //                        Id = x.Id,
    //                        Number = x.Number,
    //                        Courses = x.AcademicYearCourses.Where(y => y.Course.AcademicProgram.Code == "00").Select(y => new AcademicYearTemplateACourse
    //                        {
    //                            Id = y.Id,
    //                            CourseId = y.CourseId,
    //                            Course = y.Course.Name,
    //                            IsElective = y.IsElective,
    //                            PreRequisites = y.PreRequisites.Select(p => new AcademicYearTemplateAPreRequesite
    //                            {
    //                                Id = p.Id,
    //                                Course = p.Course.Name
    //                            }).ToList()
    //                        }).ToList()
    //                    }).OrderBy(x => x.Number).ToListAsync();

    //                return modelAsync;
    //            }
    //            else
    //            {
    //                var modelAsync = await _context.AcademicYears
    //                    .Where(x => x.CurriculumId == curriculumId)
    //                    .Select(x => new AcademicYearTemplateA
    //                    {
    //                        Id = x.Id,
    //                        Number = x.Number,
    //                        Courses = x.AcademicYearCourses.Where(y => y.Course.AcademicProgram.Code == "00" || y.Course.AcademicProgram.Code == academicProgramCode).Select(y => new AcademicYearTemplateACourse
    //                        {
    //                            Id = y.Id,
    //                            CourseId = y.CourseId,
    //                            Course = y.Course.Name,
    //                            IsElective = y.IsElective,
    //                            PreRequisites = y.PreRequisites.Select(p => new AcademicYearTemplateAPreRequesite
    //                            {
    //                                Id = p.Id,
    //                                Course = p.Course.Name
    //                            }).ToList()
    //                        }).ToList()
    //                    }).OrderBy(x => x.Number).ToListAsync();

    //                return modelAsync;
    //            }
    //        }
    //    }

    //    public async Task<object> GetAllAsSelect2(Guid? careerId = null, bool isActive = false, string coordinatorId = null)
    //    {
    //        if (!string.IsNullOrEmpty(coordinatorId))
    //        {
    //            var careers = GetCoordinatorCareers(coordinatorId);

    //            var result2 = await _context.AcademicYears
    //                .Where(ay => careers.Any(y => y == ay.Curriculum.CareerId) && ay.Curriculum.IsActive == isActive)
    //                .Select(ay => new
    //                {
    //                    //id = ay.Id,
    //                    id = ay.Number,
    //                    text = ay.Number
    //                })
    //                .OrderBy(x => x.text)
    //                .Distinct()
    //                .ToListAsync();

    //            return result2;
    //        }

    //        var result = await _context.AcademicYears
    //            .Where(ay => ay.Curriculum.CareerId == careerId.Value && ay.Curriculum.IsActive == isActive)
    //            .Select(ay => new
    //            {
    //                //id = ay.Id,
    //                id = ay.Number,
    //                text = ay.Number
    //            })
    //            .OrderBy(x => x.text)
    //            .Distinct()
    //            .ToListAsync();

    //        return result;
    //    }

    //    public async Task<object> GetCareerAcademicYear(Guid id)
    //    {
    //        var result = await _context.AcademicYears
    //            .Where(ay => ay.Curriculum.CareerId == id && ay.Curriculum.IsActive)
    //            .Select(ay => new
    //            {
    //                id = ay.Id,
    //                text = ay.Number
    //            }).ToListAsync();

    //        return result;
    //    }
    //    public async Task<object> GetAllCareerAcademicYear(Guid id)
    //    {
    //        var result = await _context.AcademicYears
    //            .Where(ay => ay.Curriculum.CareerId == id)
    //            .OrderBy(x => x.Number)
    //            .Select(ay => new
    //            {
    //                id = ay.Number,
    //                text = ay.Number
    //            })
    //            .Distinct()
    //            .ToListAsync();

    //        return result;
    //    }

    //    public async Task<object> GetCurriculumAcademicYearJson(Guid id)
    //    {
    //        var result = await _context.AcademicYears
    //        .Where(ay => ay.CurriculumId == id)
    //        .OrderBy(x => x.Number)
    //        .Select(ay => new
    //        {
    //            id = ay.Number,
    //            text = ay.Number
    //        })
    //        .ToListAsync();

    //        return result;
    //    }

    //    public async Task<object> GetAllAcademicYearsByCareer(Guid careerId)
    //    {
    //        var query = _context.Students.AsNoTracking();

    //        query = query.Where(x => x.CareerId == careerId);

    //        var academicYears = await query
    //            .GroupBy(x => x.CurrentAcademicYear)
    //            .Select(x => x.Key).ToListAsync();

    //        var result = academicYears.Select(x => new
    //        {
    //            id = x,
    //            text = x.ToString()
    //        })
    //        .OrderBy(x => x.id)
    //        .ToList();

    //        result.Insert(0, new { id = -1, text = "Todos" });

    //        return result;
    //    }
    //}
}