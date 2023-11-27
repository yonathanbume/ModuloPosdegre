using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Career;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.Forum;
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
    public class CareerRepository : Repository<Career>, ICareerRepository
    {
        public CareerRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE
        private async Task<Select2Structs.ResponseParameters> GetCareerSelect2(Select2Structs.RequestParameters requestParameters, Guid? facultyId, Expression<Func<Career, Select2Structs.Result>> selectPredicate = null, string searchValue = null, ClaimsPrincipal user = null)
        {
            var query = _context.Careers
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR)
                    || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)
                    || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x =>
                    x.AcademicCoordinatorId == userId ||
                    x.AcademicSecretaryId == userId ||
                    x.CareerDirectorId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
                {
                    query = query.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId || x.Faculty.AdministrativeAssistantId == userId);
                }
                else

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_COORDINATOR))
                {
                    var careers = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId || x.AcademicDepartmentCoordinatorId == userId)
                        .Select(x => x.Career.Id).ToListAsync();

                    query = query.Where(x => careers.Contains(x.Id));
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Code.ToUpper().Contains(searchValue.ToUpper()));

            if (facultyId.HasValue)
                query = query.Where(x => x.FacultyId == facultyId.Value);

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

        private async Task<Select2Structs.ResponseParameters> GetCareerByUserIdSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Career, Select2Structs.Result>> selectPredicate = null, string userId = null, string searchValue = null)
        {
            var CareerId = await _context.Teachers.Where(x => x.UserId == userId).Select(x => x.CareerId).FirstOrDefaultAsync();
            var query = _context.Careers
                .Where(x => x.Id == CareerId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

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

        private async Task<Select2Structs.ResponseParameters> GetCareerByUserCoordinatorIdSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Career, Select2Structs.Result>> selectPredicate = null, string userId = null, string searchValue = null, Guid? facultyId = null)
        {
            var query = GetCareerByUserCoordinatorIdQuery(userId, searchValue);

            if (facultyId.HasValue)
                query = query.Where(x => x.FacultyId == facultyId);

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

        private IQueryable<Career> GetCareerByUserCoordinatorIdQuery(string userId, string searchValue)
        {
            var query = _context.Careers
                .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Code.ToUpper().Contains(searchValue.ToUpper()));

            return query;
        }

        #endregion

        #region PUBLIC
        public override async Task<Career> Get(Guid id)
        {
            return await _context.Careers.Include(x => x.CareerHistories).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Career>> GetCareerByUserCoordinatorId(string userId, string searchValue = null)
        {
            return await GetCareerByUserCoordinatorIdQuery(userId, searchValue).ToListAsync();
        }

        public async Task<Select2Structs.ResponseParameters> GetCareerSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null, Guid? facultyId = null, ClaimsPrincipal user = null)
        {
            return await GetCareerSelect2(requestParameters, facultyId, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, searchValue, user);
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetCareerSelect2ByAcademicCoordinatorClientSide(string academicCoordinatorId, Guid? selectedId = null)
        {
            var result = await _context.Careers.Where(x => x.AcademicCoordinatorId == academicCoordinatorId || x.CareerDirectorId == academicCoordinatorId)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name,
                    Selected = x.Id == selectedId
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<Select2Structs.ResponseParameters> GetCareerByUserIdSelect2(Select2Structs.RequestParameters requestParameters, string userId = null, string searchValue = null, Guid? selectedId = null)
        {
            return await GetCareerByUserIdSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name,
                Selected = x.Id == selectedId
            }, userId, searchValue);
        }
        public async Task<Select2Structs.ResponseParameters> GetCareerByUserCoordinatorIdSelect2(Select2Structs.RequestParameters requestParameters, string userId = null, string searchValue = null, Guid? selectedId = null, Guid? facultyId = null)
        {
            return await GetCareerByUserCoordinatorIdSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name,
                Selected = x.Id == selectedId
            }, userId, searchValue, facultyId);
        }

        public async Task<IEnumerable<Career>> GetAllData(string search = null)
        {
            var query = _context.Careers
                .Include(x => x.Faculty)
                //.Include(x=>x.Students)
                //.ThenInclude(x=>x.StudentSections)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.Name.ToUpper().Contains(search));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Career>> GettAllData2(string search = null)
        {
            var query = _context.Careers
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query
                    .Where(x => x.Name.ToUpper().Contains(search));

            return await query.ToListAsync();
        }

        public async Task<object> GetCareerWithCountTutorAndTutorStudents()
        {
            var query = await _context.Careers
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name
                })
                .ToListAsync();
            var tutors = await _context.Tutors.ToListAsync();
            var tutoringStudents = await _context.TutoringStudents.Include(x => x.Student).ToListAsync();


            var result = query
                .Select(x => new
                {
                    name = x.Code,
                    longName = x.Name,
                    tutors = tutors.Where(y => y.CareerId == x.Id).Count(),
                    tutoringStudents = tutoringStudents.Where(y => y.Student.CareerId == x.Id).Count()
                })
                .ToList();

            return result;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<Career, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    break;
            }

            var query = _context.Careers
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            if (sentParameters.RecordsPerDraw > 0)
            {
                var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    facultyId = x.FacultyId,
                    faculty = x.Faculty.Name,
                    graduateProfile = x.GraduateProfile,
                    generalInformation = x.GeneralInformation,
                    comments = x.Comments,
                    decanalResolution = x.DecanalResolutionNumber,
                    rectoralResolution = x.RectoralResolutionNumber
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
            else
            {
                var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    facultyId = x.FacultyId,
                    faculty = x.Faculty.Name,
                    graduateProfile = x.GraduateProfile,
                    generalInformation = x.GeneralInformation,
                    comments = x.Comments,
                    decanalResolution = x.DecanalResolutionNumber,
                    rectoralResolution = x.RectoralResolutionNumber
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
        }

        public async Task<object> GetCareerSelect2ClientSide(Guid? careerId = null, Guid? facultyId = null, bool hasAll = false, string coordinatorId = null, ClaimsPrincipal user = null)
        {
            var query = _context.Careers.AsQueryable();

            if (facultyId.HasValue)
                query = query.Where(x => x.FacultyId == facultyId);

            if (careerId.HasValue)
                query = query.Where(x => x.Id == careerId);

            if (!string.IsNullOrEmpty(coordinatorId))
            {
                query = query.Where(x => x.AcademicCoordinatorId == coordinatorId || x.CareerDirectorId == coordinatorId);
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var careers = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartment.CareerId).ToArrayAsync();
                    query = query.Where(x => careers.Contains(x.Id));
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x =>
                        x.AcademicCoordinatorId == userId ||
                        x.CareerDirectorId == userId ||
                        x.AcademicSecretaryId == userId
                        );
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId);
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
                {
                    var careers = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId).Select(x => x.CareerId).ToListAsync();
                    query = query.Where(x => careers.Contains(x.Id));
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    query = query.Where(x => x.QualityCoordinatorId == userId);
                }
            }

            var result = await query
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            if (hasAll)
                result.Insert(0, new { id = new Guid(), text = "Todas" });

            return result;
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetEquivalenceDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
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
                default:
                    break;
            }

            var query = _context.Curriculums
                .Where(c => (c.IsActive || c.IsNew) && c.Career.Curriculums.Any(y => y.Year < c.Year))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Career.Name.ToUpper().Contains(searchValue.ToUpper()) || x.Career.Code.ToUpper().Contains(searchValue.ToUpper()));

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));
                }
            }

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
                    curriculum = $"{x.Year}-{x.Code}",
                    isActive = x.IsActive,
                    isNew = x.IsNew
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

        public async Task<Tuple<List<Career>, List<Faculty>>> GetCareerFacultiesIdByCoordinator(string academicCoordinatorId)
        {
            var careers = await _context.Careers.Include(x => x.Faculty).Where(x => x.AcademicCoordinatorId == academicCoordinatorId).ToListAsync();
            var faculties = careers.Select(x => x.Faculty).Distinct().ToList();
            return new Tuple<List<Career>, List<Faculty>>(careers, faculties);
        }

        public Task<object> GetCareerSelect2ClientSide(Guid? careerId = null)
        {
            throw new NotImplementedException();
        }

        public async Task<object> GetSelect2ByFaculty(Guid studentId)
        {
            var careers = await _context.Careers
           .Where(c => c.FacultyId == studentId)
           .Select(c => new
           {
               id = c.Id,
               text = c.Name
           }).ToListAsync();

            careers.Insert(0, new { id = Guid.Empty, text = "Todas" });

            return careers;
        }

        public async Task<object> GetCareersToForum()
        {
            var careers = await _context.Careers
              .Select(
                  x => new
                  {
                      x.Id,
                      Text = x.Name
                  }
              )
              .ToListAsync();

            return careers;
        }

        public async Task<List<ForumCareer>> GetForumCareer(ForumTemplate model)
        {
            var forumcareers = model.Careers.Any(x => x == Guid.Empty) ?
                await _context.Careers.Select(
                    x => new ForumCareer
                    {
                        CareerId = x.Id
                    }
                    ).ToListAsync() : model.Careers.Select(
                        x => new ForumCareer
                        {
                            CareerId = x
                        }
                    ).ToList();

            return forumcareers;
        }

        public async Task<object> GetCareerSelect2Curriculum(Guid fid, List<Guid> FacultyIds = null)
        {
            var query = _context.Careers.AsQueryable();
            if (FacultyIds != null)
            {
                query = query.Where(x => FacultyIds.Contains(x.FacultyId));
                var result = await query.Select(x => new
                {
                    id = x.Id,
                    text = x.Name

                }).ToListAsync();
                return result;
            }
            else
            {
                var result = await query.Where(x => x.FacultyId == fid).Select(x => new
                {
                    id = x.Id,
                    text = x.Name

                }).ToListAsync();
                return result;
            }



        }

        public async Task<object> GetCareerAcademicSecretaryByCareerId(Guid careerId)
        {
            var query = _context.Careers
                .Where(x => x.Id == careerId)
                .Select(x => new
                {
                    id = x.AcademicSecretaryId,
                    title = x.AcademicSecretary.FullName
                })
                .AsQueryable();

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task<object> GetCareerAcademicCoordinatorByCareerId(Guid careerId)
        {
            var query = _context.Careers
                .Where(x => x.Id == careerId)
                .Select(x => new
                {
                    id = x.AcademicCoordinatorId,
                    title = x.AcademicCoordinator.FullName
                })
                .AsQueryable();

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<ModelATemplate>> GetAllAsModelA(string search = null)
        {
            var query = _context.Careers
            .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));
            }

            var result = await query
                .Select(x => new ModelATemplate
                {
                    Id = x.Id,
                    Name = x.Name,

                    AcademicCoordinator = !string.IsNullOrEmpty(x.AcademicCoordinatorId) ? x.AcademicCoordinator.FullName : "--",
                    AcademicCoordinatorId = x.AcademicCoordinatorId,

                    AcademicSecretary = !string.IsNullOrEmpty(x.AcademicSecretaryId) ? x.AcademicSecretary.FullName : "--",
                    AcademicSecretaryId = x.AcademicSecretaryId,

                    CareerDirector = !string.IsNullOrEmpty(x.CareerDirectorId) ? x.CareerDirector.FullName : "--",
                    CareerDirectorId = x.CareerDirectorId,

                    AcademicDepartmentDirector = !string.IsNullOrEmpty(x.AcademicDepartmentDirectorId) ? x.AcademicDepartmentDirector.FullName : "--",
                    AcademicDepartmentDirectorId = x.AcademicDepartmentDirectorId,
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return result;
        }
        public async Task<List<HisotricTemplate>> GetHistory(Guid id)
        {
            var query = _context.CareerHistories
                .Where(x => x.CareerId == id)
            .AsNoTracking();

            var result = await query
                .OrderBy(x => x.CreatedAt)
                .Select(x => new HisotricTemplate
                {
                    Id = x.Id,
                    Date = x.CreatedAt.ToLocalDateFormat(),
                    AcademicCoordinator = !string.IsNullOrEmpty(x.CoordinatorId) ? x.Coordinator.FullName : "--",
                    AcademicCoordinatorId = x.CoordinatorId,

                    AcademicSecretary = !string.IsNullOrEmpty(x.SecretaryId) ? x.Secretary.FullName : "--",
                    AcademicSecretaryId = x.SecretaryId,

                    CareerDirector = !string.IsNullOrEmpty(x.CareerDirectorId) ? x.CareerDirector.FullName : "--",
                    CareerDirectorId = x.CareerDirectorId,

                    AcademicDepartmentDirector = !string.IsNullOrEmpty(x.AcademicDepartmentDirectorId) ? x.AcademicDepartmentDirector.FullName : "--",
                    AcademicDepartmentDirectorId = x.AcademicDepartmentDirectorId,
                })
                .ToListAsync();

            return result;
        }
        public async Task<object> GetAllAsSelect2ClientSide(Guid? facultyId = null, bool includeTitle = false, Guid? careerId = null, string coordinatorId = null, ClaimsPrincipal user = null)
        {
            var query = _context.Careers.AsQueryable();

            if (facultyId.HasValue && facultyId.Value != Guid.Empty)
                query = query.Where(x => x.FacultyId == facultyId);

            if (careerId.HasValue && careerId.Value != Guid.Empty)
                query = query.Where(x => x.Id == careerId.Value);

            if (!string.IsNullOrEmpty(coordinatorId))
            {
                query = query.Where(x => x.AcademicCoordinatorId == coordinatorId || x.AcademicSecretaryId == coordinatorId || x.CareerDirectorId == coordinatorId || x.AcademicDepartmentDirectorId == coordinatorId);
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                    user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    query = query.Where(x =>
                    x.AcademicCoordinatorId == userId ||
                    x.CareerDirectorId == userId ||
                    x.AcademicDepartmentDirectorId == userId ||
                    x.AcademicSecretaryId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        query = query.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId);
                    }
                }
            }


            var careers = await query
           .Select(c => new
           {
               id = c.Id,
               text = c.Name
           })
           .OrderBy(x => x.text)
           .ToListAsync();

            if (includeTitle)
                careers.Insert(0, new { id = Guid.Empty, text = "Todas" });

            return careers;
        }

        public async Task<Guid> GetIdByCoordinatorOrSecretary(string academicCoordinatorId, string academicSecretaryId)
        {
            var careerId = await _context.Careers
                .Where(x => x.AcademicCoordinatorId == academicCoordinatorId || x.AcademicSecretaryId == academicSecretaryId) //ERROR
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            return careerId;
        }

        public async Task<List<Career>> GetAllByfacultyId(Guid? facultyId = null)
        {
            var query = _context.Careers.AsNoTracking();

            if (facultyId != null)
                query = query.Where(x => x.FacultyId == facultyId);

            var result = await query
                .OrderBy(x => x.Name)
                .ToListAsync();

            return result;
        }

        public async Task<object> GetEnrollmentShiftDatatableClientSide()
        {
            var onlyRegulars = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ENROLLMENT_TURN_ONLY_FOR_REGULAR));

            var query = _context.Careers.AsNoTracking();

            var filterRecordsAsync = query.CountAsync();

            if (onlyRegulars)
            {
                var result = await query
                    .Select(c => new
                    {
                        career = c.Name,
                        faculty = c.Faculty.Name,
                        students = c.Students.Where(s =>
                            s.Status == ConstantHelpers.Student.States.REGULAR ||
                            s.Status == ConstantHelpers.Student.States.UNBEATEN).Count(),
                        id = c.Id
                    })
                    .ToListAsync();

                var filterRecords = await filterRecordsAsync;

                return new
                {
                    draw = ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.DRAW_COUNTER,
                    recordsTotal = filterRecords,
                    recordsFiltered = filterRecords,
                    data = result
                };
            }
            else
            {
                var result = await query
                   .Select(c => new
                   {
                       career = c.Name,
                       faculty = c.Faculty.Name,
                       students = c.Students.Where(s =>
                           s.Status == ConstantHelpers.Student.States.ENTRANT ||
                           s.Status == ConstantHelpers.Student.States.REGULAR ||
                           s.Status == ConstantHelpers.Student.States.TRANSFER ||
                           s.Status == ConstantHelpers.Student.States.IRREGULAR ||
                           s.Status == ConstantHelpers.Student.States.REPEATER ||
                           s.Status == ConstantHelpers.Student.States.UNBEATEN ||
                           s.Status == ConstantHelpers.Student.States.OBSERVED).Count(),
                       id = c.Id
                   })
                   .ToListAsync();

                var filterRecords = await filterRecordsAsync;

                return new
                {
                    draw = ConstantHelpers.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.DRAW_COUNTER,
                    recordsTotal = filterRecords,
                    recordsFiltered = filterRecords,
                    data = result
                };
            }
        }

        public async Task<Career> GetCareerReport(Guid careerId)
        {
            var career = await _context.Careers.Where(c => c.Id == careerId).Include(c => c.Faculty).FirstOrDefaultAsync();

            return career;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetCareersDatatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user, string userId)
        {
            Expression<Func<Career, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Faculty.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Students.Count(s => s.CurrentAcademicYear == 1 && s.Status == CORE.Helpers.ConstantHelpers.Student.States.ENTRANT);
                    break;
                default:
                    orderByPredicate = (x) => x.Name;
                    break;
            }


            var query = _context.Careers.AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (user.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR))
            {
                var career = await _context.Careers.FirstOrDefaultAsync(x => x.AcademicCoordinatorId == userId);
                var careerId = career != null ? career.Id : Guid.Empty;

                query = query.Where(x => x.Id == careerId);
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(c => new
                {
                    id = c.Id,
                    name = c.Name,
                    faculty = c.Faculty.Name,
                    postulants = c.Students.Count(s => s.CurrentAcademicYear == 1 && s.Status == CORE.Helpers.ConstantHelpers.Student.States.ENTRANT),
                    active = c.EntrantEnrollments.Any(ee => ee.TermId == termId),
                    finished = c.EntrantEnrollments.Any(ee => ee.TermId == termId) && c.EntrantEnrollments.FirstOrDefault(ee => ee.TermId == termId).Finished
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareers2Datatable(DataTablesStructs.SentParameters sentParameters, Guid termId, ClaimsPrincipal user, string userId, Guid facultyId)
        {
            Expression<Func<Career, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Faculty.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Students.Count(s => s.CurrentAcademicYear == 1 && s.Status == ConstantHelpers.TERM_STATES.ACTIVE);
                    break;
                default:
                    orderByPredicate = (x) => x.Name;
                    break;
            }


            var query = _context.Careers.AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (user.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR))
            {
                var career = await _context.Careers.FirstOrDefaultAsync(x => x.AcademicCoordinatorId == userId);
                var careerId = career != null ? career.Id : Guid.Empty;

                query = query.Where(x => x.Id == careerId);
            }
            else
            {
                query = query.Where(x => x.FacultyId == facultyId);
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(c => new
                {
                    id = c.Id,
                    name = c.Name,
                    faculty = c.Faculty.Name,
                    postulants = c.Students.Count(s => s.CurrentAcademicYear == 1 && s.Status == CORE.Helpers.ConstantHelpers.Student.States.ENTRANT),
                    active = c.EntrantEnrollments.Any(ee => ee.TermId == termId),
                    finished = c.EntrantEnrollments.Any(ee => ee.TermId == termId) && c.EntrantEnrollments.FirstOrDefault(ee => ee.TermId == termId).Finished
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

        public async Task<object> GetCareerAcademicDepartmentDirectorByCareerId(Guid careerId)
        {
            var query = _context.Careers
                .Where(x => x.Id == careerId)
                .Select(x => new
                {
                    id = x.AcademicDepartmentDirectorId,
                    title = x.AcademicDepartmentDirector.FullName
                })
                .AsQueryable();

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task<object> GetCareerAcademicCareerDirectorByCareerId(Guid careerId)
        {
            var query = _context.Careers
                .Where(x => x.Id == careerId)
                .Select(x => new
                {
                    id = x.CareerDirectorId,
                    title = x.CareerDirector.FullName
                })
                .AsQueryable();

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Career>> GetAllWithData()
        {
            var result = await _context.Careers
                .Include(x => x.Faculty)
                .Include(x => x.Curriculums)
                .ToListAsync();
            return result;
        }

        public async Task<object> GetCareersJson(string q, ClaimsPrincipal user = null)
        {
            var query = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    if (!string.IsNullOrEmpty(userId))
                        query = query.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId);
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_COORDINATOR))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        var careers = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId || x.AcademicDepartmentCoordinatorId == userId).Select(x => x.CareerId).ToListAsync();
                        query = query.Where(x => careers.Contains(x.Id));
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.TUTORING_COORDINATOR))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {

                        var coordinatorCareerId = await _context.TutoringCoordinators
                            .Where(x => x.UserId == userId)
                            .Select(x => x.CareerId)
                            .ToListAsync();

                        query = query = query.Where(x => coordinatorCareerId.Contains(x.Id));
                    }
                }
                else if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        query = query.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId || x.Faculty.AdministrativeAssistantId == userId);
                    }
                }
            }

            if (!string.IsNullOrEmpty(q))
                query = query.Where(c => c.Name.Equals(q));

            var result = query
                .AsEnumerable()
             .OrderBy(x => x.Name)
             .Select(c => new
             {
                 id = c.Id,
                 text = c.Name
             })
             .ToList();


            return result;
        }

        public async Task<object> GetCareerAndAreaJson()
        {
            var careers = await _context.Careers
                .Select(c => new
                {
                    id = c.Id,
                    text = c.Name
                }).ToListAsync();

            var areas = await _context.Areas
                .Select(a => new
                {
                    id = a.Id,
                    text = a.Name
                }).ToListAsync();

            var result = new
            {
                items = new[] {
                    new { text = "Áreas", children = areas },
                    new { text = "Carreras", children = careers }
                }
            };

            return result;
        }

        public async Task<object> GetCareersByIdJson(Guid id)
        {
            var career = await _context.Careers
                .Where(c => c.Id.Equals(id))
                .Select(c => new
                {
                    id = c.Id,
                    text = c.Name
                }).FirstOrDefaultAsync();

            return career;
        }

        public async Task<object> GetCareersByFacultyIdJson(Guid id, bool hasAll = false, ClaimsPrincipal user = null)
        {
            var query = _context.Careers
                .Where(x => x.FacultyId == id)
                .AsNoTracking();


            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                    query = query.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId || x.AcademicSecretaryId == userId);
            }

            var result = query
                .AsEnumerable()
                .OrderBy(x => x.Code)
                .Select(c => new
                {
                    id = c.Id,
                    text = $"{c.Code} - {c.Name}"
                })
                .ToList();

            if (hasAll)
                result.Insert(0, new { id = new Guid(), text = "Todas" });

            return result;
        }

        public async Task<object> GetCareersByFacultyIdWithAllJson(Guid id)
        {
            var careers = await _context.Careers
                 .Where(c => c.FacultyId == id)
                 .Select(c => new
                 {
                     id = c.Id,
                     text = c.Name
                 }).OrderBy(x => x.text).ToListAsync();

            careers.Insert(0, new { id = new Guid(), text = "Todas" });
            return careers;

        }

        public async Task<Career> GetNameByCellExcel(string cell)
        {
            var result = await _context.Careers.Where(x => x.Code == cell).FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<Career>> GetCareerFacultyById(Guid id)
        {
            var result = await _context.Careers.Where(x => x.Id == id).Include(x => x.Faculty).ToListAsync();

            return result;
        }
        public async Task<Guid> GetGuidFirst()
        {
            var result = await _context.Careers.FirstOrDefaultAsync();
            return result.Id;
        }
        public async Task<object> GetCareers()
        {
            var result = await _context.Careers
            .Select(x => new
            {
                id = x.Id,
                name = x.Name
            })
            .ToListAsync();

            return result;
        }

        public async Task<object> GetAllCareerWithStudentQty()
        {
            return await _context.Careers.Select(x => new
            {
                Id = x.Faculty.Id,
                Faculty = x.Faculty.Name,
                Career = x.Name,
                Curriculum = x.Curriculums.FirstOrDefault(c => c.IsActive).Code,
                QuantityStudents = x.Students.Count()
            }).OrderBy(x => x.Faculty).ToListAsync();
        }
        public async Task<object> GetAllCareerWithStudentQty(Guid id, int year)
        {
            var postulants = await _context.Postulants.Where(c => c.AdmissionTypeId == id && c.ApplicationTerm.StartDate.Year == year).ToListAsync();

            return _context.Careers
                .Include(x => x.Faculty)
                .AsEnumerable()
                .Select(x => new
                {
                    NameFaculty = x.Faculty.Name,
                    Name = x.Name,
                    QuantitytPostulants = postulants.Count(c => c.CareerId == x.Id),
                    QuantitytAdmitted = postulants.Count(c => c.CareerId == x.Id && c.Accepted)
                })
                .OrderBy(x => x.NameFaculty)
                .ToList();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetEnrolledStudentsByCareer(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, Guid? termId, Guid? campusId = null, bool onlyWithStudents = false)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Restart();

            var query = _context.Careers
                .AsQueryable();
            //var query = _context.Careers.AsNoTracking();

            if (user.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = await GetCareerByUserCoordinatorId(userId);

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.Id));
                }
            }
            if (user.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId);
                }
            }

            if (onlyWithStudents)
                if (campusId.HasValue && campusId != Guid.Empty)
                    query = query.Where(x => x.Students.Any(y => y.StudentSections.Any(z => z.Section.CourseTerm.TermId == termId) && y.CampusId == campusId));
                else
                    query = query.Where(x => x.Students.Any(y => y.StudentSections.Any(z => z.Section.CourseTerm.TermId == termId)));

            //query = query.Where(x => _context.Students.Where(y => y.StudentSections.Any(z => z.Section.CourseTerm.TermId == termId) && y.CareerId == x.Id).Count() > 0);

            int recordsFiltered = query.Count();

            //var data = await query
            //    .OrderBy(x => x.Faculty.Name)
            //    .ThenBy(x => x.Name)
            //    .Skip(sentParameters.PagingFirstRecord)
            //    .Take(sentParameters.RecordsPerDraw)
            //    .Select(x => new
            //    {
            //        id = x.Id,
            //        Faculty = x.Faculty.Name,
            //        Career = x.Name,
            //        //Total = _context.Students.Where(y => y.StudentSections.Any(z => z.Section.CourseTerm.TermId == termId) && y.CareerId == x.Id).Count()
            //        Total = x.Students.Where(y => y.StudentSections.Any(z => z.Section.CourseTerm.TermId == termId)).Count()
            //    })
            //    .ToListAsync();

            var dbdata = await query
                .OrderBy(x => x.Faculty.Name)
                .ThenBy(x => x.Name)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    Faculty = x.Faculty.Name,
                    Career = x.Name
                })
                .ToListAsync();

            var queryStudents = _context.Students
                .Where(x => x.StudentSections.Any(z => z.Section.CourseTerm.TermId == termId
                && z.Status != ConstantHelpers.STUDENT_SECTION_STATES.WITHDRAWN))
                .AsNoTracking();

            if (campusId.HasValue && campusId != Guid.Empty)
                queryStudents = queryStudents.Where(x => x.CampusId == campusId);

            var students = await queryStudents
                .Select(x => x.CareerId)
                .ToListAsync();

            var data = dbdata
                .Select(x => new
                {
                    x.id,
                    x.Faculty,
                    x.Career,
                    Total = students.Where(y => y == x.id).Count()
                }).ToList();

            int recordsTotal = data.Count;

            var time = stopwatch.Elapsed;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return result;
        }

        public async Task<List<Career>> GetCareersByClaim(ClaimsPrincipal user)
        {
            var query = _context.Careers.AsQueryable();

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (
                user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                  user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY)
                )
            {
                query = query.Where(x =>
                x.AcademicCoordinatorId == userId ||
                x.CareerDirectorId == userId ||
                x.AcademicSecretaryId == userId);
            }

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY))
            {
                var careersId = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentSecretaryId == userId || x.AcademicDepartmentDirectorId == userId)
                    .Select(x => x.CareerId).ToListAsync();

                query = query.Where(x => careersId.Contains(x.Id));
            }

            var result = await query.Include(x => x.Faculty).ToListAsync();
            return result;
        }

        public async Task<object> GetDetailReport(Guid termId, ClaimsPrincipal user)
        {
            var query = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ADMINISTRATIVE_ASSISTANT))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        query = query.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId || x.Faculty.AdministrativeAssistantId == userId);
                    }
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        query = query.Where(x => x.CareerDirectorId == userId || x.AcademicCoordinatorId == userId || x.AcademicSecretaryId == userId);
                    }
                }
            }

            var result = await query
                 .Select(x => new
                 {
                     id = x.Id,
                     faculty = x.Faculty.Name,
                     code = x.Code,
                     career = x.Name,
                     enrolled = x.Students.Where(y => y.StudentSections.Any(z => z.Section.CourseTerm.TermId == termId && !z.Section.IsDirectedCourse)).Count()
                 }).ToListAsync();

            return result;
        }
        public async Task<List<Career>> GetList(Guid careerId)
            => await _context.Careers
                .Where(x => x.Id == careerId).ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareersDatatableByClaim(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user, string searchValue, bool? withAcreditationsActive = null)
        {

            Expression<Func<Career, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Faculty.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Code;
                    break;
            }

            var query = _context.Careers.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(CORE.Helpers.ConstantHelpers.ROLES.CAREER_DIRECTOR))
                {
                    var careers = await _context.Careers.Where(x => x.CareerDirectorId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => careers.Contains(x.Id));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    var careers = await _context.Careers.Where(x => x.AcademicDepartmentDirectorId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => careers.Contains(x.Id));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId);
                }
            }

            if (withAcreditationsActive.HasValue && withAcreditationsActive.Value)
            {
                var now = DateTime.UtcNow.ToDefaultTimeZone();
                query = query.Where(x => x.CareerAccreditations.Any(y => y.StartDate.Date <= now && y.EndDate.Date >= now));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToLower().Trim().Contains(searchValue.Trim().ToLower()));
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(c => new
                {
                    id = c.Id,
                    name = c.Name,
                    code = c.Code,
                    faculty = c.Faculty.Name,
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

        public async Task<bool> AnyAcademicDepartmentByCareerDirector(string userId)
            => await _context.AcademicDepartments.AnyAsync(x => x.Career.CareerDirectorId == userId);

        public async Task<object> GetCarrerConditionSelect2(ClaimsPrincipal user)
        {
            var query = _context.Careers.AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
                {
                    var careers = await _context.CoordinatorCareers.Where(x => x.UserId == userId).Select(x => x.CareerId).ToArrayAsync();
                    query = query.Where(x => careers.Contains(x.Id));
                }
                if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                {
                    var careerId = await _context.Students.Where(x => x.UserId == userId).Select(x => x.CareerId).FirstOrDefaultAsync();
                    query = query.Where(x => x.Id == careerId);
                }
            }

            var result = await query
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            result.Insert(0, new { id = Guid.Empty, text = "Todas" });

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetNumberOfApprovedStudentsDatatable(DataTablesStructs.SentParameters parameters, Guid? termId, Guid? facultyId = null, Guid? careerId = null, ClaimsPrincipal? user = null)
        {

            var term = await _context.Terms.Where(x => x.Id == termId).FirstOrDefaultAsync();

            var query = _context.Careers.AsNoTracking();


            if (facultyId != null)
                query = query.Where(x => x.FacultyId == facultyId);

            if (careerId != null)
                query = query.Where(x => x.Id == careerId);
            else
            {
                if (user != null)
                {
                    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                    {
                        query = query.Where(x => x.QualityCoordinatorId == userId);
                    }
                }
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    career = x.Name,
                    faculty = x.Faculty.Name,
                    approved = _context.AcademicSummaries.Where(y => y.TermId == term.Id && y.CareerId == x.Id && y.WeightedAverageGrade >= term.MinGrade).Count(),
                    disapproved = _context.AcademicSummaries.Where(y => y.TermId == term.Id && y.CareerId == x.Id && y.WeightedAverageGrade < term.MinGrade).Count(),
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareersQualityCoordinatorDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Career, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Faculty.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.QualityCoordinator.FullName;
                    break;
                default:
                    orderByPredicate = (x) => x.Code;
                    break;
            }

            var query = _context.Careers.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                var search = searchValue.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Trim().Contains(search) ||
                        x.Faculty.Name.ToLower().Trim().Contains(search) ||
                        x.Code.ToLower().Trim().Contains(search) ||
                        x.QualityCoordinator.FullName.ToLower().Trim().Contains(search) ||
                        x.QualityCoordinator.Name.ToLower().Trim().Contains(search) ||
                        x.QualityCoordinator.PaternalSurname.ToLower().Trim().Contains(search) ||
                        x.QualityCoordinator.MaternalSurname.ToLower().Trim().Contains(search)
                        );
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    career = x.Name,
                    code = x.Code,
                    faculty = x.Faculty.Name,
                    qualityCoordinator = x.QualityCoordinator == null ? "No Asignado" : x.QualityCoordinator.FullName
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

        public async Task<List<Career>> GetFacultiesCareersByDean(string deanId)
        {
            var careers = await _context.Careers
                .Include(x => x.Faculty)
                .Where(x => x.Faculty.DeanId == deanId)
                .ToListAsync();

            return careers;
        }

        public async Task<List<Career>> GetFacultiesCareersBySecretary(string secretaryId)
        {
            var careers = await _context.Careers
                .Include(x => x.Faculty)
                .Where(x => x.Faculty.SecretaryId == secretaryId)
                .ToListAsync();

            return careers;
        }

        public async Task<bool> AnyByCode(string code, Guid? id = null)
        {           
            return await _context.Careers.AnyAsync(x => x.Code.ToUpper() == code.ToUpper() && x.Id != id);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentGraduatedCareerQuantityReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null, int? searchStartYear = null, int? searchEndYear = null)
        {
            var query = _context.Careers.AsNoTracking();
            var students = _context.Students.AsNoTracking();

            if (careerId != null)
                query = query.Where(x => x.Id == careerId);

            if (searchStartYear != null)
                students = students.Where(x => x.GraduationTerm.Year >= searchStartYear);

            if (searchEndYear != null)
                students = students.Where(x => x.GraduationTerm.Year <= searchEndYear);

            var data = await query
                .Select(x => new
                {
                    career = x.Name,
                    graduatedCount = students.Where(y => y.Status == ConstantHelpers.Student.States.GRADUATED && y.CareerId == x.Id).Count()
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeStudentBachelorQualifiedCareerQuantityReportDatatable(DataTablesStructs.SentParameters sentParameters, Guid? careerId = null)
        {
            var query = _context.Careers.AsNoTracking();
            var students = _context.Students.AsNoTracking();

            if (careerId != null)
                query = query.Where(x => x.Id == careerId);

            var data = await query
                .Select(x => new
                {
                    career = x.Name,
                    bachelorCount = students.Where(y => y.Status == ConstantHelpers.Student.States.BACHELOR && y.CareerId == x.Id).Count(),
                    qualifiedCount = students.Where(y => y.Status == ConstantHelpers.Student.States.QUALIFIED && y.CareerId == x.Id).Count(),
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
    }
    #endregion
}