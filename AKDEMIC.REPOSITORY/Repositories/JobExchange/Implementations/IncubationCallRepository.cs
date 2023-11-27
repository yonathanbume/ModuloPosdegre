using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class IncubationCallRepository: Repository<IncubationCall>, IIncubationCallRepository
    {
        public IncubationCallRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAceptedDatatable(DataTablesStructs.SentParameters sentParameters, string rolId, string searchValue = null)
        {
            var query = _context.IncubationCalls.Include(x => x.User).Where(x => x.IsAccepted == true && x.User.UserRoles.Any(s => s.Role.Name == ConstantHelpers.ROLES.STUDENTS || s.Role.Name == ConstantHelpers.ROLES.ENTERPRISE)).AsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(rolId))
            {
                query = query.Where(x => x.User.UserRoles.Any(s => s.RoleId == rolId));
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.Name.Contains(searchValue));
            }

            Expression<Func<IncubationCall, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                user = x.User.UserRoles.Any(s => s.Role.Name == ConstantHelpers.ROLES.STUDENTS) ? x.User.FullName : x.User.Name
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAdmin2Datatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            var query = _context.IncubationCalls.Include(x => x.User)
                .Where(x => x.User.UserRoles.Any(s => s.Role.Name == ConstantHelpers.ROLES.SUPERADMIN || s.Role.Name == ConstantHelpers.ROLES.JOB_EXCHANGE_ADMIN))
                .OrderByDescending(x=>x.CreatedAt)
                .AsNoTracking();
               

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.Contains(searchValue));
            }

            Expression<Func<IncubationCall, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                Description = x.Description
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<IncubationCall> GetByUser(string userId)
        {
            var query = _context.IncubationCalls
                .Where(x => x.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallAdminDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<IncubationCall, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.UserId);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.IncubationCalls
                .Where(x => x.User.UserRoles.Any(s => s.Role.Name == ConstantHelpers.ROLES.SUPERADMIN || s.Role.Name == ConstantHelpers.ROLES.JOB_EXCHANGE_ADMIN))
                .OrderByDescending(x=>x.CreatedAt)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.Contains(searchValue));
            }

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    Date = x.CreatedAt.ToLocalDateTimeFormat()
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallEnterpriseDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<IncubationCall, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.UserId);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.IncubationCalls
                .Where(x => x.User.UserRoles.Any(s => s.Role.Name == ConstantHelpers.ROLES.ENTERPRISE))
                .OrderByDescending(x=>x.CreatedAt)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.Name.Contains(searchValue));
            }

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    CompanyName = x.User.Name,
                    x.IsAccepted,
                    Date = x.CreatedAt.ToLocalDateFormat()
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubationCallNotAdminDatatable(DataTablesStructs.SentParameters sentParameters, string rolId, string searchValue = null)
        {
            Expression<Func<IncubationCall, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.UserId);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.IncubationCalls
                .Where(x => x.User.UserRoles.Any(s => s.Role.Name == ConstantHelpers.ROLES.STUDENTS || s.Role.Name == ConstantHelpers.ROLES.ENTERPRISE))
                .OrderByDescending(x=>x.CreatedAt)
                //.OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(rolId))
            {
                query = query.Where(x => x.User.UserRoles.Any(s => s.RoleId == rolId));
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.FullName.Contains(searchValue) 
                            || x.User.Name.Contains(searchValue));
            }

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    user = x.User.UserRoles.Any(s => s.Role.Name == ConstantHelpers.ROLES.STUDENTS) ? x.User.FullName : x.User.Name,
                    x.IsAccepted,
                    x.Description,
                    Date = x.CreatedAt.ToLocalDateTimeFormat()
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

        public async Task<IncubationCall> IncubationCallAdmin()
        {
            var result = _context.IncubationCalls.Where(x => x.User.UserRoles.Any(s => s.Role.Name == ConstantHelpers.ROLES.SUPERADMIN));
            return await result.FirstOrDefaultAsync();
        }
    }
}
