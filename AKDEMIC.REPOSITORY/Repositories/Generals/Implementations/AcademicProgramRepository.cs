using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.AcademicProgram;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    public class AcademicProgramRepository : Repository<AcademicProgram>, IAcademicProgramRepository
    {
        public AcademicProgramRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<Select2Structs.ResponseParameters> GetAcademicProgramSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<AcademicProgram, Select2Structs.Result>> selectPredicate, Func<AcademicProgram, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.AcademicPrograms
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        private async Task<Select2Structs.ResponseParameters> GetAcademicProgramByCareerSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<AcademicProgram, Select2Structs.Result>> selectPredicate, Func<AcademicProgram, string[]> searchValuePredicate = null, Guid? careerId = null, string searchValue = null)
        {
            var query = _context.AcademicPrograms.AsNoTracking();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Contains(searchValue.Trim().ToLower()));

            return await query.ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        #endregion

        #region  PUBLIC

        public async Task<Select2Structs.ResponseParameters> GetAcademicProgramSelect2(Select2Structs.RequestParameters requestParameters, Guid? selectedId, string searchValue = null)
        {
            return await GetAcademicProgramSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name,
                Selected = x.Id == selectedId
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetAcademicProgramByCareerSelect2(Select2Structs.RequestParameters requestParameters, Guid? selectedId, Guid? careerId, string searchValue = null)
        {
            return await GetAcademicProgramByCareerSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name,
                Selected = x.Id == selectedId
            }, (x) => new[] { x.Name }, careerId, searchValue);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetAcademicProgramByCareerSelect2ClientSide(Guid? careerId = null, Guid? selectedId = null)
        {
            var query = _context.AcademicPrograms.AsQueryable();

            if (careerId.HasValue)
                query = query.Where(x => x.CareerId == careerId);

            var result = await query
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = $"{x.Code} - {x.Name}",
                    Selected = x.Id == selectedId
                })
                .Distinct()
                .ToArrayAsync();

            result = result.OrderByDescending(x => x.Text).ToArray();

            return result;
        }

        public Task<bool> AnyByName(string name, Guid? id = null)
        {
            if (id.HasValue)
            {
                return _context.AcademicPrograms.AnyAsync(x => x.Name.Trim().ToLower().Equals(name.Trim().ToLower()) && x.Id != id);
            }
            else
            {
                return _context.AcademicPrograms.AnyAsync(x => x.Name.Trim().ToLower().Equals(name.Trim().ToLower()));
            }
        }

        public async Task<DataTablesStructs.ReturnedData<AcademicProgramModelATemplate>> GetAllAsModelADataTable(DataTablesStructs.SentParameters sentParameters, Guid? facultyId = null, Guid? careerId = null, string coordinatorId = null, string search = null, ClaimsPrincipal user = null)
        {
            var query = _context.AcademicPrograms.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (user.IsInRole(ConstantHelpers.ROLES.DEAN)||user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                     query = query.Where(x => x.Career.Faculty.DeanId == userId||x.Career.Faculty.SecretaryId == userId);
                }
            }

            if (facultyId.HasValue && facultyId.Value != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId.Value != Guid.Empty)
            {
                query = query.Where(x => x.CareerId == careerId.Value);
            }
            else
            {
                if (!string.IsNullOrEmpty(coordinatorId))
                {
                    var careers = GetCoordinatorCareers(coordinatorId);
                    query = query.Where(x => careers.Any(y => y == x.CareerId));
                }
            }
            

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                query = query.Where(x => x.Name.ToLower().Contains(search) || x.Career.Name.Contains(search) || x.Code.Contains(search));
            }

            var pagedList = query
                .OrderBy(x=>x.Name)
                    .Select(
                        x => new AcademicProgramModelATemplate
                        {
                            Id = x.Id,
                            CareerId = x.CareerId,
                            CareerName = x.Career.Name,
                            SuneduCode = x.SuneduCode,
                            Type = x.Type,
                            Name = x.Name,
                            Code = x.Code,
                            IsProgram = (x.IsProgram ? "Si" : "No")
                        }
                    );

            return await pagedList.ToDataTables<AcademicProgramModelATemplate>(sentParameters);
        }
        public async Task<object> GetAllAsModelA(Guid? facultyId = null, Guid? careerId = null, string coordinatorId = null)
        {
            var query = _context.AcademicPrograms.AsQueryable();

            if (facultyId.HasValue && facultyId.Value != Guid.Empty)
                query = query.Where(x => x.Career.FacultyId == facultyId);

            if (careerId.HasValue && careerId.Value != Guid.Empty)
            {
                query = query.Where(x => x.CareerId == careerId.Value);
            }
            else
            {
                if (!string.IsNullOrEmpty(coordinatorId))
                {
                    var careers = GetCoordinatorCareers(coordinatorId);
                    query = query.Where(x => careers.Any(y => y == x.CareerId));
                }
            }

            var programs = await query
                .Select(
                    x => new
                    {
                        x.Id,
                        CareerName = x.Career.Name,
                        x.Name,
                        x.Code
                    }
                )
                .ToListAsync();

            return programs;
        }

        public async Task<object> GetAsModelB(Guid? id = null)
        {
            var query = _context.AcademicPrograms.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id);

            var program = await query
                .Where(x => x.Id == id)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Name,
                        x.CareerId,
                        x.Code,
                        x.IsProgram
                    }
                )
                .FirstOrDefaultAsync();

            return program;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> AcademicProgramsDatatable(DataTablesStructs.SentParameters sentParameters, Guid? curriculumId = null)
        {
            var query = _context.AcademicPrograms
                .Where(x => x.AcademicProgramCurriculums.Any(y => y.CurriculumId == curriculumId))
                .AsQueryable();

            var pagedList = query.Select(x => new
            {
                code = x.Code,
                name = x.Name
            });


            return await pagedList.ToDataTables<object>(sentParameters);
        }

        public async Task<object> GetAcademicProgramsSelect(Guid cid, bool HasAll = false, List<Guid> CareerIds = null)
        {
            var query = _context.AcademicPrograms
                .AsQueryable();

            if (CareerIds != null)
            {
                query = query.Where(x => CareerIds.Contains(x.CareerId));
                var result = await query.Select(x => new
                {
                    id = x.Id,
                    text = x.Name

                }).ToListAsync();
                return result;
            }
            else
            {
                var result = await query.Where(x => x.CareerId == cid).Select(x => new
                {
                    id = x.Id,
                    text = x.Name

                }).ToListAsync();

                if (HasAll == true)
                {
                    result.Insert(0, new { id = Guid.Empty, text = "Todas" });
                }

                return result;
            }
        }
        public async Task<object> GetCareerAcademicProgramsJson(Guid id, bool onlyWithCourses = false)
        {
            var query = _context.AcademicPrograms
                 .Where(x => x.CareerId == id)
                 .AsNoTracking();

            if (onlyWithCourses)
            {
                //var courses = _context.Courses.Select(x => x.AcademicProgramId).ToHashSet();
                //query = query.Where(x => courses.Contains(x.Id));
                query = query.Where(x => x.Courses.Any());
            }

            var programs = await query
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Name)
                .Select(a => new
                {
                    id = a.Id,
                    text = $"{a.Code}-{a.Name}"
                }).ToListAsync();

            return programs;
        }

        public async Task<object> GetCareerAcademicProgramsByPlan(Guid id)
        {
            var programs = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == id)
                .Select(x => x.Course.AcademicProgramId)
                .ToHashSet();

            var areas = await _context.AcademicPrograms
                 .Where(x => x.AcademicProgramCurriculums.Any(y => y.CurriculumId == id) || programs.Contains(x.Id))
                .Select(a => new
                {
                    id = a.Id,
                    text = a.Code + "-" + a.Name
                })
                .OrderBy(x => x.text)
                .ToListAsync();

            return areas;
        }

        public async Task<IEnumerable<AcademicProgram>> GetAllByCareer(Guid careerId)
        {
            var academicPrograms = await _context.AcademicPrograms
                .Where(x => x.CareerId == careerId)
                .ToListAsync();

            return academicPrograms;
        }

        public async Task<AcademicProgram> GetByCode(string code, Guid? careerId = null)
        {
            var query = _context.AcademicPrograms
                .Where(x => x.Code == code)
                .AsQueryable();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<object> GetCurriculumAcademicProgramsJson(Guid id)
        {
            //var result = await _context.AcademicPrograms.Where
            //     (x => x.AcademicProgramCurriculums.Any(y => y.CurriculumId == id) || x.Career.Curriculums.Any(y => y.AcademicProgramId == ))
            //    .Select(a => new
            //    {
            //        id = a.Id,
            //        text = a.Name
            //    }).OrderBy(x => x.text).ToListAsync();

            var result = await _context.AcademicYearCourses
                .Where(x => x.CurriculumId == id && x.Course.AcademicProgramId.HasValue)
                .Select(x => new {
                    id = x.Course.AcademicProgram.Id,
                    text = x.Course.AcademicProgram.Name
                }).ToListAsync();
            result = result.OrderBy(x => x.text).Distinct().ToList();
            return result;
        }

        public async Task<AcademicProgramReportTemplate> GetAcademicProgramsReportData(Guid termId, Guid facultyId, Guid careerId)
        {
            var term = _context.Terms.AsQueryable();
            var faculty = _context.Faculties.AsQueryable();
            var career = _context.Careers.AsQueryable();

            var academicPrograms = await _context.AcademicPrograms.Where(x => x.CareerId == careerId).ToListAsync();

            var studentSections = _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == termId && x.Student.CareerId == careerId).AsQueryable();

            var data = await studentSections
                .Select(x => new {
                    Key = x.Section.CourseTerm.Course.AcademicProgramId,
                    x.Student.Status,
                    x.StudentId,
                    x.Try
                })
                //.Select(x => new AcademicProgramGroupTemplate
                //{
                //    Id = x.Key.HasValue ? x.Key.Value : Guid.Empty,
                //    Rowspan = 0,
                //    Regular = x.Where(y => y.Student.Status == ConstantHelpers.Student.States.REGULAR && y.Try < 3).Select(y => y.StudentId).Distinct().Count(),
                //    Observed = x.Where(y => y.Student.Status == ConstantHelpers.Student.States.OBSERVED && y.Try < 3).Select(y => y.StudentId).Distinct().Count(),
                //    Reserved = x.Where(y => y.Student.Status == ConstantHelpers.Student.States.RESERVED && y.Try < 3).Select(y => y.StudentId).Distinct().Count(),
                //    Third = x.Where(y => y.Try == 3).Select(y => y.StudentId).Distinct().Count(),
                //    Fourth = x.Where(y => y.Try == 4).Select(y => y.StudentId).Distinct().Count(),
                //    Fifth = x.Where(y => y.Try == 5).Select(y => y.StudentId).Distinct().Count(),
                //    Sixth = x.Where(y => y.Try == 6).Select(y => y.StudentId).Distinct().Count(),
                //    Seventh = x.Where(y => y.Try == 7).Select(y => y.StudentId).Distinct().Count(),
                //    Eighth = x.Where(y => y.Try == 8).Select(y => y.StudentId).Distinct().Count(),
                //    Ninth = x.Where(y => y.Try == 9).Select(y => y.StudentId).Distinct().Count(),
                //    Tenth = x.Where(y => y.Try == 10).Select(y => y.StudentId).Distinct().Count(),
                //    Eleventh = x.Where(y => y.Try == 11).Select(y => y.StudentId).Distinct().Count(),
                //    Twelfth = x.Where(y => y.Try == 12).Select(y => y.StudentId).Distinct().Count(),
                //    Thirteenth = x.Where(y => y.Try == 13).Select(y => y.StudentId).Distinct().Count(),
                //    Fourteenth = x.Where(y => y.Try == 14).Select(y => y.StudentId).Distinct().Count(),
                //    Fifteenth = x.Where(y => y.Try == 15).Select(y => y.StudentId).Distinct().Count(),
                //    SubTotal = 0
                //})
                .ToListAsync();

            var grouped = data
                .GroupBy(x => x.Key)
                .Select(x => new AcademicProgramGroupTemplate
                {
                    Id = x.Key.HasValue ? x.Key.Value : Guid.Empty,
                    Rowspan = 0,
                    Regular = x.Where(y => y.Status == ConstantHelpers.Student.States.REGULAR && y.Try < 3).Select(y => y.StudentId).Distinct().Count(),
                    Observed = x.Where(y => y.Status == ConstantHelpers.Student.States.OBSERVED && y.Try < 3).Select(y => y.StudentId).Distinct().Count(),
                    Reserved = x.Where(y => y.Status == ConstantHelpers.Student.States.RESERVED && y.Try < 3).Select(y => y.StudentId).Distinct().Count(),
                    Third = x.Where(y => y.Try == 3).Select(y => y.StudentId).Distinct().Count(),
                    Fourth = x.Where(y => y.Try == 4).Select(y => y.StudentId).Distinct().Count(),
                    Fifth = x.Where(y => y.Try == 5).Select(y => y.StudentId).Distinct().Count(),
                    Sixth = x.Where(y => y.Try == 6).Select(y => y.StudentId).Distinct().Count(),
                    Seventh = x.Where(y => y.Try == 7).Select(y => y.StudentId).Distinct().Count(),
                    Eighth = x.Where(y => y.Try == 8).Select(y => y.StudentId).Distinct().Count(),
                    Ninth = x.Where(y => y.Try == 9).Select(y => y.StudentId).Distinct().Count(),
                    Tenth = x.Where(y => y.Try == 10).Select(y => y.StudentId).Distinct().Count(),
                    Eleventh = x.Where(y => y.Try == 11).Select(y => y.StudentId).Distinct().Count(),
                    Twelfth = x.Where(y => y.Try == 12).Select(y => y.StudentId).Distinct().Count(),
                    Thirteenth = x.Where(y => y.Try == 13).Select(y => y.StudentId).Distinct().Count(),
                    Fourteenth = x.Where(y => y.Try == 14).Select(y => y.StudentId).Distinct().Count(),
                    Fifteenth = x.Where(y => y.Try == 15).Select(y => y.StudentId).Distinct().Count(),
                    SubTotal = 0
                })
                .ToList();

            foreach (var item in grouped)
            {
                item.SubTotal = grouped.Where(y => y.Id == item.Id)
                    .Select(y => y.Regular + y.Observed + y.Reserved + y.Third + y.Fourth + y.Fifth + y.Sixth + y.Seventh + y.Eighth
                        + y.Ninth + y.Tenth + y.Eleventh + y.Twelfth + y.Thirteenth + y.Fourteenth + y.Fifteenth).FirstOrDefault();

                item.Rowspan = grouped.Where(y => y.Id == item.Id && (y.Regular > 0 || y.Observed > 0 || y.Reserved > 0 || y.Third > 0 || y.Fourth > 0 ||
                            y.Fifth > 0 || y.Sixth > 0 || y.Seventh > 0 || y.Eighth > 0 || y.Ninth > 0 || y.Tenth > 0 ||
                            y.Eleventh > 0 || y.Twelfth > 0 || y.Thirteenth > 0 || y.Fourteenth > 0 || y.Fifteenth > 0)).Count();
            }

            var report = new AcademicProgramReportTemplate
            {
                Term = await term.Where(x => x.Id == termId).Select(x => $"[{x.Year}-SEMESTRE {x.Number}]").FirstOrDefaultAsync(),
                Faculty = await faculty.Where(x => x.Id == facultyId).Select(x => x.Name.ToUpper()).FirstOrDefaultAsync(),
                Career = await career.Where(x => x.Id == careerId).Select(x => x.Name.ToUpper()).FirstOrDefaultAsync(),
                AcademicPrograms = academicPrograms
                    .Select(x => new AcademicProgramGroupTemplate
                    {
                        Name = x.Name.ToUpper(),
                        Rowspan = grouped.Where(y => y.Id == x.Id).Select(y => y.Rowspan).FirstOrDefault(),
                        Regular = grouped.Where(y => y.Id == x.Id).Select(y => y.Regular).FirstOrDefault(),
                        Observed = grouped.Where(y => y.Id == x.Id).Select(y => y.Observed).FirstOrDefault(),
                        Reserved = grouped.Where(y => y.Id == x.Id).Select(y => y.Reserved).FirstOrDefault(),
                        Third = grouped.Where(y => y.Id == x.Id).Select(y => y.Third).FirstOrDefault(),
                        Fourth = grouped.Where(y => y.Id == x.Id).Select(y => y.Fourth).FirstOrDefault(),
                        Fifth = grouped.Where(y => y.Id == x.Id).Select(y => y.Fifth).FirstOrDefault(),
                        Sixth = grouped.Where(y => y.Id == x.Id).Select(y => y.Sixth).FirstOrDefault(),
                        Seventh = grouped.Where(y => y.Id == x.Id).Select(y => y.Seventh).FirstOrDefault(),
                        Eighth = grouped.Where(y => y.Id == x.Id).Select(y => y.Eighth).FirstOrDefault(),
                        Ninth = grouped.Where(y => y.Id == x.Id).Select(y => y.Ninth).FirstOrDefault(),
                        Tenth = grouped.Where(y => y.Id == x.Id).Select(y => y.Tenth).FirstOrDefault(),
                        Eleventh = grouped.Where(y => y.Id == x.Id).Select(y => y.Eleventh).FirstOrDefault(),
                        Twelfth = grouped.Where(y => y.Id == x.Id).Select(y => y.Twelfth).FirstOrDefault(),
                        Thirteenth = grouped.Where(y => y.Id == x.Id).Select(y => y.Thirteenth).FirstOrDefault(),
                        Fourteenth = grouped.Where(y => y.Id == x.Id).Select(y => y.Fourteenth).FirstOrDefault(),
                        Fifteenth = grouped.Where(y => y.Id == x.Id).Select(y => y.Fifteenth).FirstOrDefault(),
                        SubTotal = grouped.Where(y => y.Id == x.Id).Select(y => y.SubTotal).FirstOrDefault()
                    }).ToList()
            };

            return report;
        }

        public async Task LoadCurriculumAcademicProgramJob()
        {
            var curriculums = await _context.Curriculums.ToListAsync();

            foreach (var curriculum in curriculums)
            {
                var academicProgram = await _context.AcademicPrograms.FirstOrDefaultAsync(x => x.CareerId == curriculum.CareerId);

                if (academicProgram == null)
                {
                    academicProgram = new AcademicProgram
                    {
                        CareerId = curriculum.CareerId,
                        Code = "UNICO",
                        Name = "UNICO"
                    };

                    await _context.AcademicPrograms.AddAsync(academicProgram);
                    await _context.SaveChangesAsync();
                }

                curriculum.AcademicProgramId = academicProgram.Id;
            }

            await _context.SaveChangesAsync();
        }
        public async Task LoadStudentsAcademicProgramJob()
        {
            var academicPrograms = await _context.AcademicPrograms.ToListAsync();

            var students = await _context.Students.Where(x => !x.AcademicProgramId.HasValue).ToListAsync();

            foreach (var student in students)
            {
                student.AcademicProgramId = academicPrograms.FirstOrDefault(x => x.CareerId == student.CareerId).Id;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<object> GetAcademicProgramByCoursesSelect2CliendSide(Guid careerId)
        {
            var result = await _context.Courses.Where(x => x.AcademicProgram.CareerId == careerId)
                .OrderBy(x => x.AcademicProgram.Code)
                .ThenBy(x=>x.AcademicProgram.Name)
                .Select(x => new
                {
                    id = x.AcademicProgramId,
                    text = $"{x.AcademicProgram.Code} - {x.AcademicProgram.Name}"
                })
                .Distinct()
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetAcademicProgramByCampusIdSelect2ClientSide(Guid campusId, Guid? selectedId = null)
        {
            var careers = await _context.CampusCareers.Where(x => x.CampusId == campusId).Select(x => x.CareerId).ToArrayAsync();
            var result = await _context.AcademicPrograms.Where(x => careers.Contains(x.CareerId))
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Code}-{x.Name}",
                    selected = x.Id == selectedId
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<object> GetCareersToForum()
        {
            var careers = await _context.AcademicPrograms.Include(x => x.Career).Where(x => x.CareerId != null)
                .Select(
                    x => new
                    {
                        x.CareerId,
                        Text = x.Career.Name
                    }
                )
                .ToListAsync();

            return careers;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByAcademicProgram(DataTablesStructs.SentParameters sentParameters, Guid termId, bool onlyWithStudents = false)
        {
            var query = _context.AcademicPrograms.AsNoTracking();

            if (onlyWithStudents)
                query = query.Where(x => x.Students.Any(y => y.StudentSections.Any(z => z.Section.CourseTerm.TermId == termId)));

            if (sentParameters.RecordsPerDraw != 0)
                query = query
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw);

            var dbdata = await query
                .OrderBy(x => x.Career.Faculty.Name)
                .ThenBy(x => x.Career.Name)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    Faculty = x.Career.Faculty.Name,
                    Career = x.Career.Name,
                    AcademicProgram = x.Name,
                    x.IsProgram
                })
                .ToListAsync();

            var students = await _context.Students
                .Where(x => x.StudentSections.Any(z => z.Section.CourseTerm.TermId == termId))
                .Select(x => x.AcademicProgramId)
                .ToListAsync();

            var data = dbdata
                .Select(x => new
                {
                    x.Faculty,
                    x.Career,
                    AcademicProgram = x.IsProgram ? x.AcademicProgram : x.Career,
                    Total = students.Where(y => y == x.id).Count()
                }).ToList();

            data = data.GroupBy(x => new { x.Faculty, x.Career, x.AcademicProgram })
                .Select(x => new
                {
                    x.Key.Faculty,
                    x.Key.Career,
                    x.Key.AcademicProgram,
                    Total = x.Sum(y => y.Total)
                }).ToList();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
            return result;
        }

        public async Task<List<AcademicProgram>> GetAcademicProgramsByCurriculumId(Guid curriculumId)
        {
            var query = _context.AcademicYearCourses
                .Where(x => x.CurriculumId == curriculumId && x.Course.AcademicProgramId.HasValue)
                .AsNoTracking();

            var result = await query
                .Select(x=>x.Course.AcademicProgram).ToListAsync();

            result = result.Distinct().ToList();

            return result;
        }

        #endregion
    }
}