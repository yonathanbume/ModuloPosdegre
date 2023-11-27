using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InstitutionalWelfare.Implementations
{
    public class WelfareAlertRepository : Repository<WelfareAlert>, IWelfareAlertRepository
    {
        public WelfareAlertRepository(AkdemicContext context):base(context)
        {

        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetAllDatatable(DataTablesStructs.SentParameters sentParameters, int? status = null, string searchValue = null)
        {
            Expression<Func<WelfareAlert, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.RegisterDate);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Subject);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Status);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.Commentary);
                    break;

            }

            var query = _context.WelfareAlerts.AsNoTracking();

            if (status != null) query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchTrim = searchValue.Trim();
                query = query.Where(x => x.User.UserName.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.User.PaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.User.MaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.User.Name.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.User.FullName.ToUpper().Contains(searchTrim.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    RegisterDate = x.RegisterDate.ToLocalDateFormat(),
                    x.Status,
                    StatusText = ConstantHelpers.WELFARE_ALERT.STATUS.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.WELFARE_ALERT.STATUS.VALUES[x.Status] : "",
                    x.Subject,
                    x.Commentary
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
