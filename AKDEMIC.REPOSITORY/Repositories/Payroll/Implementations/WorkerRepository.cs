using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Template.WorkerAssistance;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class WorkerRepository : Repository<Worker>, IWorkerRepository
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public WorkerRepository(AkdemicContext context, UserManager<ApplicationUser> userManager) 
            : base(context)
        {
            _userManager = userManager;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters,byte type ,string searchValue = null)
        {
            Expression<Func<Worker, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.WorkerLaborInformation.WorkerCode);
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.RawFullName;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.WorkerLaborInformation.EntryDate);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.User.WorkerLaborInformation.LaborType);
                    break;
            }

            var query = _context.Workers.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.WorkerLaborInformation.WorkerCode.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                                    x.User.Name.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                                    x.User.MaternalSurname.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                                    x.User.PaternalSurname.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                                    x.User.Dni.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()));

            }

            if(type != 0)
            {
                query = query.Where(x => x.User.WorkerLaborInformation.LaborType == type);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.User.WorkerLaborInformation.WorkerCode,
                    x.User.FullName,
                    x.User.Dni,
                    EntryDate = x.User.WorkerLaborInformation.EntryDate.HasValue ? x.User.WorkerLaborInformation.EntryDate.ToLocalDateFormat() : "",
                    LaborType = x.User.WorkerLaborInformation.LaborType.HasValue ? ConstantHelpers.USER_LABOR_INFORMATION.LABOR_TYPES.VALUES[x.User.WorkerLaborInformation.LaborType.Value] : "-",
                    LaborCondition = x.User.WorkerLaborInformation.WorkerLaborCondition != null ? x.User.WorkerLaborInformation.WorkerLaborCondition.Name : "-",
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

        public override async Task Delete(Worker worker)
        {
            if(!string.IsNullOrEmpty(worker.UserId))
            {
                var user = await _userManager.FindByIdAsync(worker.UserId);
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.DeleteAsync(user);
                await _context.SaveChangesAsync();
            }
            await base.Delete(worker);
        }

        public override async Task DeleteById(Guid workerId)
        {
            var worker = await Get(workerId);
            if (!string.IsNullOrEmpty(worker.UserId))
            {
                var user = await _userManager.FindByIdAsync(worker.UserId);
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                await _userManager.DeleteAsync(user);
                await _context.SaveChangesAsync();
            }
            await base.Delete(worker);
        }

        public async Task<IEnumerable<Worker>> GetWorkers()
            => await _context.Workers.Include(x => x.User).ToListAsync();

        public async Task<Worker> GetWithWorkerLaborInformationById(Guid workerId)
            => await _context.Workers
            .Include(x => x.User)
                .ThenInclude(x => x.WorkerLaborInformation)
            .Include(x => x.User.WorkerLaborInformation.BirthDistrict)
            .Include(x => x.User.WorkerLaborInformation.BirthProvince)
            .Include(x => x.User.WorkerLaborInformation.BirthDepartment)
            .Include(x => x.User.WorkerLaborInformation.BirthCountry)
            .Include(x => x.User.WorkerLaborInformation.ResidenceDistrict)
            .Include(x => x.User.WorkerLaborInformation.ResidenceProvince)
            .Include(x => x.User.WorkerLaborInformation.ResidenceDepartment)
            .Include(x => x.User.WorkerLaborInformation.ResidenceCountry)
            .Include(x => x.User.WorkerLaborInformation.WorkerLaborRegime)
            .Include(x => x.User.WorkerLaborInformation.WorkerManagementPosition)
            .Include(x => x.User.WorkerLaborInformation.WorkerPositionClassification)
            .Include(x => x.User.WorkerLaborInformation.WorkerCapPosition)
            .Include(x => x.WorkArea)
            .Include(x => x.WorkerOcupation)
            .Include(x => x.WorkerHistories)
            .FirstOrDefaultAsync(x => x.Id == workerId);

        public async Task DeleteBankAccount(Guid workerId)
        {
            var worker = await _context.Workers
                .FirstOrDefaultAsync(x => x.Id == workerId);
            worker.BankId = null;
            worker.AccountNumber = null;
            worker.AccountType = null;
            worker.CTSBankId = null;
            worker.CTSAccountNumber = null;
            await _context.SaveChangesAsync();
        }

        public async Task<(IList<Worker> pagedList, int count)> GetWorkersByPaginationParameters(PaginationParameter paginationParameter,
            string code, string names, string surnames, int? laborType, DateTime? entryDate = null, DateTime? outDate = null)
        {
            var query = _context.Workers.AsQueryable();
            
            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(x => x.User.WorkerLaborInformation.WorkerCode.Contains(paginationParameter.SearchValue) ||
                                    x.User.Name.Contains(paginationParameter.SearchValue) ||
                                    x.User.MaternalSurname.Contains(paginationParameter.SearchValue) ||
                                    x.User.PaternalSurname.Contains(paginationParameter.SearchValue) ||
                                    x.User.Dni.Contains(paginationParameter.SearchValue));

            if (!string.IsNullOrEmpty(code))
                query = query.Where(x => x.User.WorkerLaborInformation.WorkerCode.Contains(code));
            if (!string.IsNullOrEmpty(names))
                query = query.Where(x => x.User.Name.Contains(names));
            if (!string.IsNullOrEmpty(surnames))
                query = query.Where(x => x.User.PaternalSurname.Contains(surnames) || x.User.MaternalSurname.Contains(surnames));
            if (laborType.HasValue)
                query = query.Where(x => x.User.WorkerLaborInformation.LaborType.HasValue ? x.User.WorkerLaborInformation.LaborType.Value == laborType.Value : false);
            if (entryDate.HasValue)
                query = query.Where(x => x.User.WorkerLaborInformation.EntryDate.HasValue ? x.User.WorkerLaborInformation.EntryDate.Value >= entryDate.Value : false);

            var count = await query.CountAsync();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.User.WorkerLaborInformation.WorkerCode)
                        : query.OrderBy(x => x.User.WorkerLaborInformation.WorkerCode);
                    break;
                case "1":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.User.FullName)
                        : query.OrderBy(x => x.User.FullName);
                    break;
                case "2":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.User.Dni)
                        : query.OrderBy(x => x.User.Dni);
                    break;
                case "3":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.User.WorkerLaborInformation.EntryFormattedDate)
                        : query.OrderBy(x => x.User.WorkerLaborInformation.EntryFormattedDate);
                    break;
                case "4":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.User.WorkerLaborInformation.LaborTypeString)
                        : query.OrderBy(x => x.User.WorkerLaborInformation.LaborTypeString);
                    break;
            }

            var result = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(x => new Worker
                {
                    Id = x.Id,
                    User = new ApplicationUser
                    {
                        Name = x.User.Name,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        Dni = x.User.Dni,
                        WorkerLaborInformation = new ENTITIES.Models.Scale.Entities.WorkerLaborInformation
                        {
                            WorkerCode = x.User.WorkerLaborInformation.WorkerCode,
                            EntryFormattedDate = x.User.WorkerLaborInformation.EntryDate.HasValue ? x.User.WorkerLaborInformation.EntryDate.ToDateFormat() : null,
                            LaborTypeString = x.User.WorkerLaborInformation.LaborType.HasValue ? ConstantHelpers.USER_LABOR_INFORMATION.LABOR_TYPES.VALUES[x.User.WorkerLaborInformation.LaborType.Value] : null
                        }
                    }
                }).ToListAsync();

            return (result, count);
        }

        public async Task<IEnumerable<Worker>> GetAllActive()
            => await _context.Workers.Where(x => x.WorkerHistories
                .OrderByDescending(w => w.StartDate).Select(w => !w.IsFinished).FirstOrDefault())
                .ToListAsync();

        public async Task<IEnumerable<Guid>> GetActiveIds()
            => await _context.Workers.Where(x => x.WorkerHistories
                .OrderByDescending(w => w.StartDate).Select(w => !w.IsFinished).FirstOrDefault())
                .Select(x => x.Id)
                .ToListAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Worker, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.User.Dni;
                    break;
            }

            var query = _context.Workers
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchTrimmed = searchValue.Trim();
                query = query.Where(x => x.User.PaternalSurname.ToUpper().Contains(searchTrimmed.ToUpper()) ||
                                        x.User.MaternalSurname.ToUpper().Contains(searchTrimmed.ToUpper()) ||
                                        x.User.Name.ToUpper().Contains(searchTrimmed.ToUpper()) ||
                                        x.User.UserName.ToUpper().Contains(searchTrimmed.ToUpper()) ||
                                        x.User.Dni.ToUpper().Contains(searchTrimmed.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.User.UserName,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    x.User.Name,
                    x.User.Dni
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkersAssistanceDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null) 
        {
            Expression<Func<Worker, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.Dni;
                    break;
            }

            var assists = await _context.WorkingDays
                .Where(x => x.RegisterDate.Date == DateTime.UtcNow.Date)
                .OrderByDescending(y => y.RegisterDate)
                .ToArrayAsync();

            var query = _context.Workers
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchTrimmed = searchValue.Trim();
                query = query.Where(x => x.User.PaternalSurname.ToUpper().Contains(searchTrimmed.ToUpper()) ||
                                        x.User.MaternalSurname.ToUpper().Contains(searchTrimmed.ToUpper()) ||
                                        x.User.Name.ToUpper().Contains(searchTrimmed.ToUpper()) ||
                                        x.User.FullName.ToUpper().Contains(searchTrimmed.ToUpper()) ||
                                        x.User.UserName.ToUpper().Contains(searchTrimmed.ToUpper()) ||
                                        x.User.Dni.ToUpper().Contains(searchTrimmed.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.User.UserName,
                    x.User.FullName,
                    x.User.Dni
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.UserName,
                    x.FullName,
                    x.Dni,
                    startDate = assists.Where(y => y.UserId == x.UserId && y.StartTime.HasValue).Select(y => y.StartTime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                    endDate = assists.Where(y => y.UserId == x.UserId && y.Endtime.HasValue).Select(y => y.Endtime.Value.ToLocalDateTimeFormatUtc()).FirstOrDefault(),
                    isAbsent = assists.Where(y => y.UserId == x.UserId).Select(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.ABSENT).FirstOrDefault(),
                    isLate = assists.Where(y => y.UserId == x.UserId).Select(y => y.Status == ConstantHelpers.WORKING_DAY.STATUS.LATE).FirstOrDefault(),
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerAssistanceDetailDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string searchStartDate = null, string searchEndDate = null)
        {
            Expression<Func<WorkingDay, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.RegisterDate;
                    break;
                case "1":
                    orderByPredicate = (x) => x.StartTime;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Endtime;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Status;
                    break;
            }

            var query = _context.WorkingDays.Where(x => x.UserId == userId).AsNoTracking();

            if (!string.IsNullOrEmpty(searchStartDate))
            {
                var startDate = ConvertHelpers.DatepickerToUtcDateTime(searchStartDate);
                query = query.Where(x => x.RegisterDate.Date >= startDate.Date);
            }

            if (!string.IsNullOrEmpty(searchEndDate))
            {
                var endDate = ConvertHelpers.DatepickerToUtcDateTime(searchEndDate);
                query = query.Where(x => x.RegisterDate.Date <= endDate.Date);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    RegisterDate = x.RegisterDate.ToLocalDateFormat(),
                    StartTime = x.StartTime.HasValue ? x.StartTime.Value.ToLocalDateTimeFormatUtc() : "",
                    Endtime = x.Endtime.HasValue ? x.Endtime.Value.ToLocalDateTimeFormatUtc() : "",
                    StatusText = ConstantHelpers.WORKING_DAY.STATUS.VALUES.ContainsKey(x.Status) ? ConstantHelpers.WORKING_DAY.STATUS.VALUES[x.Status] : ""
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

        public async Task<WorkerReportTemplate> GetWorkerAssistanceInformation(Guid id, string searchStartDate = null, string searchEndDate = null)
        {
            var worker = await _context.Workers.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (worker == null)
                return null;

            var query = _context.WorkingDays.Where(x => x.UserId == worker.UserId).AsNoTracking();
            var startDateText = "";
            var endDateText = "";

            if (!string.IsNullOrEmpty(searchStartDate))
            {
                var startDate = ConvertHelpers.DatepickerToUtcDateTime(searchStartDate);
                query = query.Where(x => x.RegisterDate.Date >= startDate.Date);
                startDateText = startDate.Date.ToDateFormat();
            }

            if (!string.IsNullOrEmpty(searchEndDate))
            {
                var endDate = ConvertHelpers.DatepickerToUtcDateTime(searchEndDate);
                query = query.Where(x => x.RegisterDate.Date <= endDate.Date);
                endDateText = endDate.Date.ToDateFormat();
            }


            var result = await _context.Workers
                .Where(x => x.Id == id)
                .Select(x => new WorkerReportTemplate
                {
                    FullName = x.User.FullName,
                    UserName = x.User.UserName,
                    StartDate = startDateText,
                    EndDate = endDateText,
                    Assistances = query
                        .Select(y => new WorkerAssistanceTemplate
                        {
                            RegisterDate = y.RegisterDate.ToLocalDateFormat(),
                            StartTime = y.StartTime.HasValue ? y.StartTime.Value.ToLocalDateTimeFormatUtc() : "",
                            Endtime = y.Endtime.HasValue ? y.Endtime.Value.ToLocalDateTimeFormatUtc() : "",
                            StatusText = ConstantHelpers.WORKING_DAY.STATUS.VALUES.ContainsKey(y.Status) ? ConstantHelpers.WORKING_DAY.STATUS.VALUES[y.Status] : ""
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
