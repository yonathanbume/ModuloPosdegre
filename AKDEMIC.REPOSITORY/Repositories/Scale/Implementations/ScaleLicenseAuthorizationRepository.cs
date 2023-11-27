using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleLicenseAuthorization;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleLicenseAuthorizationRepository : Repository<ScaleLicenseAuthorization>, IScaleLicenseAuthorizationRepository
    {
        public ScaleLicenseAuthorizationRepository(AkdemicContext context) : base(context) { }

        public async Task<int> GetLicenseAuthorizationsQuantity(string userId)
        {
            var records = await _context.ScaleLicenseAuthorizations
                .Where(x => x.UserId == userId)
                .CountAsync();

            return records;
        }

        public async Task<List<ScaleLicenseAuthorization>> GetLicenseAuthorizationsByPaginationParameters(string userId, PaginationParameter paginationParameter)
        {
            var query = _context.ScaleLicenseAuthorizations
                .Where(x => x.UserId == userId)
                .AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ResolutionNumber) : query.OrderBy(q => q.ResolutionNumber);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Reason) : query.OrderBy(q => q.Reason);
                    break;
                case "2":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ExpeditionDate) : query.OrderBy(q => q.ExpeditionDate);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ResolutionNumber) : query.OrderBy(q => q.ResolutionNumber);
                    break;
            }

            var pagedList = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                .Select(x => new ScaleLicenseAuthorization
                {
                    Id = x.Id,
                    ResolutionNumber = x.ResolutionNumber,
                    Reason = x.Reason,
                    ExpeditionFormattedDate = x.ExpeditionDate.ToLocalDateFormat(),
                    ResolutionDocument = x.ResolutionDocument
                }).ToListAsync();

            return pagedList;
        }

        public async Task<Tuple<int, List<ScaleLicenseAuthorization>>> GetLicenseAuthorizationsReportByPaginationParameters(PaginationParameter paginationParameter)
        {
            var query = _context.ScaleLicenseAuthorizations
                .Where(x => string.IsNullOrWhiteSpace(paginationParameter.SearchValue) || x.Reason.Contains(paginationParameter.SearchValue) ||
                            x.ResolutionNumber.Contains(paginationParameter.SearchValue) || x.User.PaternalSurname.Contains(paginationParameter.SearchValue) ||
                            x.User.MaternalSurname.Contains(paginationParameter.SearchValue) || x.User.Name.Contains(paginationParameter.SearchValue))
                .AsQueryable();

            var records = await query.CountAsync();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.User.PaternalSurname) : query.OrderBy(q => q.User.PaternalSurname);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ResolutionNumber) : query.OrderBy(q => q.ResolutionNumber);
                    break;
                case "2":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.Reason) : query.OrderBy(q => q.Reason);
                    break;
                case "3":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ExpeditionDate) : query.OrderBy(q => q.ExpeditionDate);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ResolutionNumber) : query.OrderBy(q => q.ResolutionNumber);
                    break;
            }

            var pagedList = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                .Select(x => new ScaleLicenseAuthorization
                {
                    Id = x.Id,
                    ResolutionNumber = x.ResolutionNumber,
                    Reason = x.Reason,
                    ExpeditionFormattedDate = x.ExpeditionDate.ToLocalDateFormat(),
                    ResolutionDocument = x.ResolutionDocument,
                    User = new ApplicationUser
                    {
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        Name = x.User.Name,
                        FullName = x.User.FullName
                    }
                }).ToListAsync();

            return new Tuple<int, List<ScaleLicenseAuthorization>>(records, pagedList);
        }

        public async Task<List<ScaleLicenseAuthorization>> GetLicenseAuthorizationsReport(string search)
        {
            return await _context.ScaleLicenseAuthorizations
                .Include(x => x.User)
                .Include(x => x.LicenseResolutionType)
                .Where(x => string.IsNullOrWhiteSpace(search) || x.Reason.Contains(search) ||
                            x.ResolutionNumber.Contains(search) || x.User.PaternalSurname.Contains(search) ||
                            x.User.MaternalSurname.Contains(search) || x.User.Name.Contains(search))
                .ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetLicensesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, ClaimsPrincipal user = null)
        {
            Expression<Func<ScaleLicenseAuthorization, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ResolutionNumber); break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.ExpeditionDate); break;
            }

            var query = _context.ScaleLicenseAuthorizations.AsNoTracking();

            if(user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_DEPARTMENT_DIRECTOR))
                {
                    var careers = await _context.Careers.Where(x => x.AcademicDepartmentDirectorId == userId).Select(x => x.Id).ToListAsync();
                    var teachers = _context.Teachers.Where(x => x.CareerId.HasValue && careers.Contains(x.CareerId.Value)).Select(x=>x.UserId);
                    query = query.Where(x => teachers.Contains(x.UserId));
                }
            }

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.User.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.User.PaternalSurname.ToUpper().Contains(searchValue.ToUpper())||
                                        x.User.MaternalSurname.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.ResolutionNumber.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.ResolutionNumber,
                    x.Reason,
                    ExpeditionFormattedDate = x.ExpeditionDate.ToLocalDateFormat(),
                    x.ResolutionDocument,
                    User = new
                    {
                        x.User.PaternalSurname,
                        x.User.MaternalSurname,
                        x.User.Name,
                        x.User.FullName
                    }
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<List<TeacherLicenseTemplate>> GetLicenseRecordReport(Guid facultyId)
        {
            var teachers = _context.Teachers.Where(x => x.AcademicDepartment.FacultyId == facultyId).AsQueryable();

            var users = _context.Users
                .Where(x => teachers.Any(y => y.UserId == x.Id))
                .AsQueryable();

            var result = await users
                    .Select(x => new TeacherLicenseTemplate
                    {
                        Name = x.Name,
                        PaternalSurname = x.PaternalSurname,
                        MaternalSurname = x.MaternalSurname,
                        FullName = x.FullName,
                        Address = x.Address,
                        PhoneNumber = x.PhoneNumber,
                        Dni = x.Dni,
                        Licenses = x.ScaleLicenseAuthorizations
                            .Select(y => new LicenseRecordTemplate
                            {
                                ResolutionNumber = y.ResolutionNumber,
                                Reason = y.Reason,
                                BeginDate = y.BeginDate.ToLocalDateFormat(),
                                EndDate = y.EndDate.ToLocalDateFormat(),
                                Observation = y.Observation
                            }).ToList()
                    }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<ScaleLicenseAuthorization>> GetAllByUserIdAndRemunerateState(string userId, bool isRemunerated)
        {
            var query = _context.ScaleLicenseAuthorizations
                .Where(x => x.UserId == userId && x.IsRemunerated == isRemunerated);

            return await query.ToListAsync();               
        }

        public async Task<IEnumerable<ScaleLicenseAuthorization>> GetAllByUserId(string userId)
        {
            var query = _context.ScaleLicenseAuthorizations
                .Where(x => x.UserId == userId);

            return await query.ToListAsync();
        }

        public async Task<int> GetTotalLicenseTimeByUser(string userId)
        {
            //Fecha de hoy
            var today = DateTime.UtcNow;

            //Si no cuenta con un contrato...
            int result = -1;

            var lastContract = await _context.ScaleResolutions
                .Where(x =>x.UserId == userId 
                && x.ScaleSectionResolutionType.ScaleSection.SectionNumber == ConstantHelpers.RESOLUTION_SECTIONS.CONTRACTS
                && x.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos"
                && (today < x.EndDate || x.EndDate == null))
                .OrderBy(x => x.BeginDate)
                .FirstOrDefaultAsync();


            if (lastContract != null)
            {


                //Si la fecha del contrato esta antes/izquierda o despues/derecha de la fecha actual 
                var izquierda = false;

                if (today.DayOfYear < lastContract.BeginDate.Value.DayOfYear) izquierda = true;

                var yearDiff = today.Year - lastContract.BeginDate.Value.Year;
                var minDate = today;
                if (izquierda)
                {
                    minDate = lastContract.BeginDate.Value.AddYears(yearDiff);
                }
                else
                {
                    minDate = lastContract.BeginDate.Value.AddYears(-1);
                }

                var maxDate = minDate.AddYears(1);
                var authorizations = await _context.ScaleLicenseAuthorizations
                        .Where(x => x.UserId == userId &&
                                    minDate > x.BeginDate &&
                                    maxDate < x.BeginDate)
                        .Select(x => new
                        {
                            x.EndDate,
                            x.BeginDate
                        }).ToListAsync();


                result = authorizations.Sum(x => (x.EndDate - x.BeginDate).Days);
            }

            return result;
        }
    }
}
