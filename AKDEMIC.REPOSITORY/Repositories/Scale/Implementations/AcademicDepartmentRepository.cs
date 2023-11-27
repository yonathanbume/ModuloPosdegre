using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class AcademicDepartmentRepository : Repository<AcademicDepartment>, IAcademicDepartmentRepository
    {
        public AcademicDepartmentRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAcademicDeparmentDataTable(DataTablesStructs.SentParameters sentParameters,int? status, Guid? facultyId, string searchValue = null)
        {
            Expression<Func<AcademicDepartment, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Faculty.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Status;
                    break;
            }

            var query = _context.AcademicDepartments.AsNoTracking();
           
            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.FacultyId == facultyId);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper())
                                    || x.Faculty.Name.ToUpper().Contains(searchValue.ToUpper()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    faculty = x.Faculty.Name,
                    name = x.Name,
                    status = x.Status,
                    career = x.Career.Name,
                    director = x.AcademicDepartmentDirector.FullName,
                    secretary = x.AcademicDepartmentSecretary.FullName,
                    coordinator = x.AcademicDepartmentCoordinator.FullName
                })
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

        public async Task<IEnumerable<AcademicDepartment>> GetAll(string searchValue = null, bool? onlyActive = null)
        {
            var query = _context.AcademicDepartments.AsQueryable();

            if (onlyActive.HasValue && onlyActive.Value)
                query = query.Where(x => x.Status == ConstantHelpers.STATES.ACTIVE);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Contains(searchValue.Trim().ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<bool> AnyByNameAndFacultyId(string name, Guid facultyId,Guid? ignoredId = null)
            => await _context.AcademicDepartments.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.FacultyId == facultyId && x.Id != ignoredId);

        public async Task<Select2Structs.ResponseParameters> GetAcademicDepartmentSelect2(Select2Structs.RequestParameters parameters, ClaimsPrincipal user, string searchValue)
        {
            var query = _context.AcademicDepartments.AsNoTracking();

            if(user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR) ||
                   user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) ||
                   user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_SECRETARY))
                {
                    var careers = await _context.Careers.Where(x => 
                    x.AcademicCoordinatorId == userId ||
                    x.CareerDirectorId == userId ||
                    x.AcademicSecretaryId == userId
                    ).Select(x=>x.Id).ToListAsync();

                    query = query.Where(x => x.CareerId.HasValue && careers.Contains(x.CareerId.Value));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN)||user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => x.Career.Faculty.DeanId == userId||x.Career.Faculty.SecretaryId == userId);
                }

                if(user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_COORDINATOR))
                {
                    query = query.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentSecretaryId == userId || x.AcademicDepartmentCoordinatorId == userId);
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            var currentPage = parameters.CurrentPage != 0 ? parameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(x=> new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
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

        public async Task<object> GetAcademicDepartmentSelect2ClientSide(ClaimsPrincipal user, Guid? facultyId = null)
        {
            var query = _context.AcademicDepartments.AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.TUTORING_COORDINATOR))
                {
                    var coordinator = await _context.TutoringCoordinators.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                    if (coordinator != null)
                    {
                        query = query.Where(x => x.CareerId == coordinator.CareerId || x.CareerId == null);
                    }
                }

                if (user.IsInRole(ConstantHelpers.ROLES.DEAN) || user.IsInRole(ConstantHelpers.ROLES.DEAN_SECRETARY))
                {
                    query = query.Where(x => (x.CareerId.HasValue && (x.Career.Faculty.DeanId == userId || x.Career.Faculty.SecretaryId == userId))
                    || (x.Faculty.DeanId == userId || x.Faculty.SecretaryId == userId));
                }
                
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_SECRETARY) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_COORDINATOR))
                    query = query.Where(x => x.AcademicDepartmentDirectorId == userId || x.AcademicDepartmentCoordinatorId == userId || x.AcademicDepartmentSecretaryId == userId);

                if (user.IsInRole((ConstantHelpers.ROLES.CAREER_DIRECTOR)) ||
                   user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR))
                {
                    var careers = await _context.Careers.Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId).Select(x => x.Id).ToListAsync();
                    query = query.Where(x => x.CareerId.HasValue && careers.Contains(x.CareerId.Value));
                }
                
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
                {
                    var academicDepartments = await _context.AcademicRecordDepartments.Where(x => x.UserId == userId).Select(x => x.AcademicDepartmentId).ToListAsync();
                    query = query.Where(x => academicDepartments.Contains(x.Id));
                }
            }

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.FacultyId == facultyId);

            var result = await query
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<AcademicDepartment>> GetDataList()
        {
            var result = await _context.AcademicDepartments
                .Select(x => new AcademicDepartment
                {
                    Name = x.Name,
                    AcademicDepartmentDirector = new ENTITIES.Models.Generals.ApplicationUser
                    {
                        FullName = x.AcademicDepartmentDirector.FullName
                    },
                    AcademicDepartmentSecretary = new ENTITIES.Models.Generals.ApplicationUser
                    {
                        FullName = x.AcademicDepartmentSecretary.FullName
                    },
                    AcademicDepartmentCoordinator = new ENTITIES.Models.Generals.ApplicationUser
                    {
                        FullName = x.AcademicDepartmentCoordinator.FullName
                    },
                    Career = new ENTITIES.Models.Generals.Career
                    {
                        Name = x.Career.Name
                    },
                    Faculty = new ENTITIES.Models.Enrollment.Faculty
                    {
                        Name = x.Faculty.Name
                    }
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<AcademicDepartment>> GetCareerAll()
        {
            var query = _context.AcademicDepartments
                .Include(x => x.Career)
                .Where(x => x.CareerId != null).AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<object> GetCareersToForum()
        {
            var careers = await _context.AcademicDepartments.Include(x => x.Career).Where(x => x.CareerId != null)
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

        public async Task<object> GetAcademicDepartmentSelect()
        {
            var result = await _context.AcademicDepartments
                .Where(x => x.Status == ConstantHelpers.STATES.ACTIVE)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            return result;
        }
    }
}
