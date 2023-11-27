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
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class WorkerTrainingRepository : Repository<WorkerTraining>, IWorkerTrainingRepository
    {
        public WorkerTrainingRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersWorkerTrainingDatatble(DataTablesStructs.SentParameters parameters, int? userType = null, string searchValue = null)
        {
            Expression<Func<WorkerTraining, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.PaternalSurname); break;
                case "2":
                    orderByPredicate = ((x) => x.User.MaternalSurname); break;
                case "3":
                    orderByPredicate = ((x) => x.User.Name); break;
                case "4":
                    orderByPredicate = ((x) => x.Institution); break;
                case "5":
                    orderByPredicate = ((x) => x.StartDate); break;
                case "6":
                    orderByPredicate = ((x) => x.EndDate); break;
                case "7":
                    orderByPredicate = ((x) => x.TotalHours); break;

            }

            var query = _context.WorkerTrainings
                .AsQueryable();

            if (userType != null)
                query = query.Where(x => x.User.Type == userType);


            if (!string.IsNullOrEmpty(searchValue))
            {
                var searchTrim = searchValue.ToUpper().Trim();
                query = query.Where(x => x.Institution.ToUpper().Contains(searchTrim) ||
                            x.User.PaternalSurname.ToUpper().Contains(searchTrim) ||
                            x.User.MaternalSurname.ToUpper().Contains(searchTrim) ||
                            x.User.Name.ToUpper().Contains(searchTrim) ||
                            x.User.FullName.ToUpper().Contains(searchTrim));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.User.UserName,
                    x.User.PaternalSurname,
                    x.User.MaternalSurname,
                    x.User.Name,
                    x.Institution,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    type = ConstantHelpers.WORKER_TRAINING_TYPE.VALUES[x.Type],
                    x.TotalHours
                })
                .ToListAsync();

            int recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetWorkerTrainingDatatble(DataTablesStructs.SentParameters parameters, string userId ,string searchValue)
        {
            Expression<Func<WorkerTraining, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Institution); break;
                case "1":
                    orderByPredicate = ((x) => x.StartDate); break;
                case "2":
                    orderByPredicate = ((x) => x.EndDate); break;
                case "3":
                    orderByPredicate = ((x) => x.TotalHours); break;
                default:
                    orderByPredicate = ((x) => x.Institution); break;
            }

            var query = _context.WorkerTrainings
                .Where(x=>x.UserId == userId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Institution.ToLower().Contains(searchValue.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();

            var dataDB = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Institution,
                    x.StartDate,
                    x.EndDate,
                    x.Type,
                    x.TotalHours
                })
                .ToArrayAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.Id,
                    x.Institution,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    type = ConstantHelpers.WORKER_TRAINING_TYPE.VALUES[x.Type],
                    x.TotalHours
                })
                .ToList();

            int recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
