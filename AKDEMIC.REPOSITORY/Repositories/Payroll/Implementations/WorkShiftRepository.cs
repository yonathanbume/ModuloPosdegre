using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Payroll.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Payroll.Implementations
{
    public class WorkShiftRepository : Repository<WorkShift>, IWorkShiftRepository
    {
        public WorkShiftRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllWorkShiftsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<WorkShift, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "3":
                    orderByPredicate = ((x) => x.StartTime);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.EndTime);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.Tolerance);
                    break;
            }

            var query = _context.WorkShifts.AsNoTracking();


            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper())
                                    || x.Code.ToString().Contains(searchValue.ToUpper()));
            }



            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Description,
                    StartTimeStr = x.StartTime.ToLocalDateTimeFormat(),
                    EndTimeStr = x.EndTime.ToLocalDateTimeFormat(),
                    x.Tolerance
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


    }
}
