using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class InternshipRequestRepository : Repository<InternshipRequest>, IInternshipRequestRepository
    {
        public InternshipRequestRepository(AkdemicContext context) : base(context) { }
    
        public async Task<DataTablesStructs.ReturnedData<object>> GetInternshipRequestDatatable(DataTablesStructs.SentParameters sentParameters,string userId, string searchValue = null)
        {
            var query = _context.InternshipRequests.Where(x=>x.StudentExperience.Student.UserId == userId)
                 .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.StudentExperience.Company.User.Name.Contains(searchValue) ||
                                        x.StudentExperience.CompanyName.Contains(searchValue) ||
                                        x.StudentExperience.Company.User.FullName.Contains(searchValue));
            }

            Expression<Func<InternshipRequest, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                ConvalidationType = x.ConvalidationTypeString,
                CompanyName = (x.StudentExperience.CompanyId.HasValue) ? (!String.IsNullOrEmpty(x.StudentExperience.Company.User.PaternalSurname) && !String.IsNullOrEmpty(x.StudentExperience.Company.User.MaternalSurname)) ? x.StudentExperience.Company.User.FullName : x.StudentExperience.Company.User.Name : x.StudentExperience.CompanyName,
                StartDate = x.StudentExperience.StartDate.ToLocalDateFormat(),
                EndDate = x.StudentExperience.EndDate.HasValue ? x.StudentExperience.EndDate.ToLocalDateFormat(): "Hasta la actualidad",
                status = x.Status,
                statusText = ConstantHelpers.INTERNSHIPREQUEST.Status.VALUES.ContainsKey(x.Status) ?
                    ConstantHelpers.INTERNSHIPREQUEST.Status.VALUES[x.Status] : ""
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentExperienceDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            var query = _context.StudentExperiences.Where(x => x.Student.UserId == userId && x.HasReportConfirmation == false)
                 .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Company.User.Name.Contains(searchValue) ||
                                        x.CompanyName.Contains(searchValue) || x.Company.User.FullName.Contains(searchValue));
            }

            Expression<Func<StudentExperience, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                CompanyName = (x.CompanyId.HasValue) ? (!String.IsNullOrEmpty(x.Company.User.PaternalSurname) && !String.IsNullOrEmpty(x.Company.User.MaternalSurname)) ? x.Company.User.FullName : x.Company.User.Name : x.CompanyName,
                StartDate = x.StartDate.ToLocalDateFormat(),
                EndDate = x.EndDate.HasValue ? x.EndDate.ToLocalDateFormat() : "Hasta la actualidad"
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternshipRequestDatatableTotal(DataTablesStructs.SentParameters sentParameters, List<Guid> careers, int ConvalidationType = 0, string searchValue = null)
        {
            var query = _context.InternshipRequests
                 .AsNoTracking();
            if (ConvalidationType > 0)
            {
                query = query.Where(x => x.ConvalidationType == ConvalidationType);
            }

            if (careers != null && careers.Count > 0)
            {
                query = query.Where(x =>careers.Contains(x.StudentExperience.Student.CareerId));
            }


            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.StudentExperience.Company.User.Name.Contains(searchValue)|| x.StudentExperience.Student.User.FullName.Contains(searchValue));
            }

            Expression<Func<InternshipRequest, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                ConvalidationType = x.ConvalidationTypeString,
                Student = x.StudentExperience.Student.User.FullName,
                Career = x.StudentExperience.Student.Career.Name,
                CompanyName = (x.StudentExperience.CompanyId.HasValue) ? (!String.IsNullOrEmpty(x.StudentExperience.Company.User.PaternalSurname) && !String.IsNullOrEmpty(x.StudentExperience.Company.User.MaternalSurname)) ? x.StudentExperience.Company.User.FullName : x.StudentExperience.Company.User.Name : x.StudentExperience.CompanyName,
                StartDate = x.StudentExperience.StartDate.ToLocalDateFormat(),
                EndDate = x.StudentExperience.EndDate.HasValue ? x.StudentExperience.EndDate.ToLocalDateFormat() : "Hasta la actualidad",
                status = x.Status,
                statusText = ConstantHelpers.INTERNSHIPREQUEST.Status.VALUES.ContainsKey(x.Status) ?
                    ConstantHelpers.INTERNSHIPREQUEST.Status.VALUES[x.Status] : ""
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternshipEmployeeDatatable(DataTablesStructs.SentParameters sentParameters, string userId, Guid? careerId = null, string searchValue = null)
        {
            Expression<Func<Student, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.PaternalSurname);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.MaternalSurname);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Career.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.User.Email);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.Name);
                    break;
            }

            var query = _context.Students.Include(x=>x.User).Include(x=>x.Career).AsQueryable();

            var interShip = await _context.InternshipRequests
                .Where(x => x.StudentExperience.Company.UserId == userId && x.IsActive)
                .Select(x => new
                {
                    x.StudentExperience.Student.UserId,
                    x.ConvalidationTypeString
                })
                .Distinct().ToListAsync();

            if (careerId != null)
                query = query.Where(x => x.CareerId == careerId);

            var ids = interShip.Select(x => x.UserId);
            query = query.Where(x => ids.Contains(x.UserId));

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.User.PaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.User.MaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Career.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.User.Email.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .AsEnumerable()
                .Select(x => new
                {
                    x.User.Name,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    Career = x.Career.Name,
                    x.User.Email,
                    ConvalidationType = interShip.FirstOrDefault(y=> y.UserId.Equals(x.UserId)).ConvalidationTypeString
                    //interShip.Where(y => y.UserId == x.UserId).Select(y => y.ConvalidationTypeString).FirstOrDefault()
                })           
                .ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnyByStudentExperience(Guid studentExperienceId)
        {
            var result = await _context.InternshipRequests.AnyAsync(x => x.StudentExperienceId == studentExperienceId);
            return result;
        }

        public async Task<List<InternshipRequestTemplate>> GetInternshipRequestData(List<Guid> careers)
        {
            var query = _context.InternshipRequests
                .AsNoTracking();

            if (careers != null && careers.Count > 0)
            {
                query = query.Where(x => careers.Contains(x.StudentExperience.Student.CareerId));
            }

            var result = await query
                .Select(x => new InternshipRequestTemplate
                {
                    ConvalidationType = ConstantHelpers.INTERNSHIPREQUEST.Type.VALUES.ContainsKey(x.ConvalidationType)
                            ? ConstantHelpers.INTERNSHIPREQUEST.Type.VALUES[x.ConvalidationType] : "Desconocido",
                    StudentFullName = x.StudentExperience.Student.User.FullName,
                    CareerName = x.StudentExperience.Student.Career.Name,
                    CompanyName = (x.StudentExperience.CompanyId.HasValue) ? (!String.IsNullOrEmpty(x.StudentExperience.Company.User.PaternalSurname) && !String.IsNullOrEmpty(x.StudentExperience.Company.User.MaternalSurname)) ? x.StudentExperience.Company.User.FullName : x.StudentExperience.Company.User.Name : x.StudentExperience.CompanyName,
                    StartDate = x.StudentExperience.StartDate.ToLocalDateFormat(),
                    EndDate = x.StudentExperience.EndDate.HasValue ? x.StudentExperience.EndDate.ToLocalDateFormat() : "Hasta la actualidad",
                    StatusText = ConstantHelpers.INTERNSHIPREQUEST.Status.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.INTERNSHIPREQUEST.Status.VALUES[x.Status] : ""
                })
                .ToListAsync();

            return result;
        }

        public async Task<List<InternshipRequestTemplate>> GetInternshipRequestStudentData(Guid studentId)
        {
            var query = _context.InternshipRequests
                .Where(x => x.StudentExperience.StudentId == studentId)
                .AsNoTracking();

            var result = await query
                .Select(x => new InternshipRequestTemplate
                {
                    ConvalidationType = ConstantHelpers.INTERNSHIPREQUEST.Type.VALUES.ContainsKey(x.ConvalidationType)
                            ? ConstantHelpers.INTERNSHIPREQUEST.Type.VALUES[x.ConvalidationType] : "Desconocido",
                    StudentFullName = x.StudentExperience.Student.User.FullName,
                    CareerName = x.StudentExperience.Student.Career.Name,
                    CompanyName = (x.StudentExperience.CompanyId.HasValue) ? (!String.IsNullOrEmpty(x.StudentExperience.Company.User.PaternalSurname) && !String.IsNullOrEmpty(x.StudentExperience.Company.User.MaternalSurname)) ? x.StudentExperience.Company.User.FullName : x.StudentExperience.Company.User.Name : x.StudentExperience.CompanyName,
                    StartDate = x.StudentExperience.StartDate.ToLocalDateFormat(),
                    EndDate = x.StudentExperience.EndDate.HasValue ? x.StudentExperience.EndDate.ToLocalDateFormat() : "Hasta la actualidad",
                    StatusText = ConstantHelpers.INTERNSHIPREQUEST.Status.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.INTERNSHIPREQUEST.Status.VALUES[x.Status] : ""
                })
                .ToListAsync();

            return result;
        }
    }
}
