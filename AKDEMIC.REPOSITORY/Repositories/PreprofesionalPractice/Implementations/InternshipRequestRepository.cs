using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PreprofesionalPractice;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.PreprofesionalPractice.Implementations
{
    public class InternshipRequestRepository : Repository<InternshipRequest>, IInternshipRequestRepository
    {
        public InternshipRequestRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDataDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
        {
            Expression<Func<InternshipRequest, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Student.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                default:
                    break;
            }

            var query = _context.InternshipValidationRequests
                .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.STUDENTS))
                {
                    query = query.Where(x => x.Student.UserId == userId);
                }

                if (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR))
                {
                    var careers = await _context.Careers.Where(x => x.CareerDirectorId == userId)
                        .Select(x => x.Id).ToListAsync();
                    query = query.Where(x => careers.Contains(x.Student.CareerId));
                }

                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    var faculties = await _context.AcademicDepartments.Where(x => x.AcademicDepartmentDirectorId == userId)
                        .Select(x => x.FacultyId).ToListAsync();
                    query = query.Where(x => faculties.Contains(x.Student.Career.FacultyId)
                    && (x.Status == ConstantHelpers.PreprofesionalPractice.InternshipRequest.Status.VALIDATED
                    || x.Status == ConstantHelpers.PreprofesionalPractice.InternshipRequest.Status.ASSIGNED));
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Student.User.UserName.ToUpper().Contains(searchValue.ToUpper())
                || x.Student.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                || x.WorkCenter.ToUpper().Contains(searchValue.ToUpper()));
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
                    fullname = x.Student.User.FullName,
                    status = x.Status,
                    workCenter = x.WorkCenter,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    address = x.Address,
                    adviserId = x.AdviserId,
                    adviser = !string.IsNullOrEmpty(x.AdviserId) ? $"{x.Adviser.FullName}" : "-",
                    dutyManager = x.DutyManager,
                    information = x.DutyManagerInformation,
                    schedule = x.Schedule,
                    createdAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    adviserSuggestion = !string.IsNullOrEmpty(x.AdviserSuggestionId) ? $"{x.AdviserSuggestion.FullName}" : "-",
                    postulantFiles = x.InternshipRequestFiles.Where(y=>y.Type == ConstantHelpers.PreprofesionalPractice.InternshipRequestFile.Type.POSTULANT).Select(y=>new
                    {
                        y.Id,
                        y.Name,
                        y.FileUrl
                    })
                    .ToList()
                })
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatableBySupervisor(DataTablesStructs.SentParameters sentParameters, byte? status, string supervisorId, string searchValue = null, ClaimsPrincipal user = null)
        {
            Expression<Func<InternshipRequest, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Student.Career.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.InternshipValidationRequests
                //.Where(x=> x.AdviserId == supervisorId)
                .AsNoTracking();

            if (user == null || !user.IsInRole(ConstantHelpers.ROLES.SUPERADMIN))
                query = query.Where(x => x.AdviserId == supervisorId);

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Student.User.FullName.ToLower().Trim().Contains(searchValue.Trim().ToLower()) || x.Student.User.UserName.ToLower().Trim().Contains(searchValue.Trim().ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.FullName,
                    x.Student.User.UserName,
                    career = x.Student.Career.Name,
                    workCenter = x.WorkCenter,
                    x.Status,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    address = x.Address,
                    dutyManager = x.DutyManager,
                    information = x.DutyManagerInformation,
                    schedule = x.Schedule,
                    createdAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    //file = x.File
                })
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
    }
}
