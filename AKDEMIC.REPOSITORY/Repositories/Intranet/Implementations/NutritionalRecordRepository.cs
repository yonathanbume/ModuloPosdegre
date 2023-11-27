using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class NutritionalRecordRepository : Repository<NutritionalRecord> , INutritionalRecordRepository
    {
        public NutritionalRecordRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetReportWaist(decimal n_minimo, decimal n_maximo)
        {
            var query = _context.NutritionalRecords.AsNoTracking();

            if (n_minimo != -1)
            {
                query = query.Where(x => x.WaistCircumference >= n_minimo);
            }

            if (n_maximo != -1)
            {
                query = query.Where(x => x.WaistCircumference <= n_maximo);
            }

            var model = await query
                .Where(x => x.CurrentStatus)
                .Select(x => new 
                {
                    x.MedicalAppointment.UserId,
                    x.MedicalAppointment.User.Sex,
                })
                .Distinct()
                .ToListAsync();

            if (model.Any())
            {
                var result = model
                .GroupBy(x => new { x.Sex })
                .Select(x => new
                {
                      //m = 0 , f =1
                      //sexo = x.Key.Sex == ConstantHelpers.SEX.MALE ? "Hombres" : "Mujeres",
                      Man = x.Count(s => x.Key.Sex == ConstantHelpers.SEX.MALE),
                    Woman = x.Count(s => x.Key.Sex == ConstantHelpers.SEX.FEMALE)

                }).First();
                
                return result;
            }
            else
            {
                var data = new
                {
                    Man = 0,
                    Woman = 0
                };

                return data;
            }
        }

        public async Task<object> GetDatatableArmsDetail(DataTablesStructs.SentParameters sentParameters, decimal n_minimo, decimal n_maximo)
        {
            Expression<Func<NutritionalRecord, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.MedicalAppointment.User.UserName); break;
                case "1":
                    orderByPredicate = ((x) => x.MedicalAppointment.User.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.MedicalAppointment.User.Email); break;
                default:
                    orderByPredicate = ((x) => x.MedicalAppointment.User.UserName); break;
            }

            var query = _context.NutritionalRecords.AsNoTracking();

            if (n_minimo != -1)
            {
                query = query.Where(x => x.WaistCircumference >= n_minimo);
            }

            if (n_maximo != -1)
            {
                query = query.Where(x => x.WaistCircumference <= n_maximo);
            }

            query = query
                .Where(x => x.CurrentStatus)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new 
                  {
                      username = x.MedicalAppointment.User.UserName,
                      fullname = x.MedicalAppointment.User.FullName,
                      email = x.MedicalAppointment.User.Email,
                      sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.MedicalAppointment.User.Sex) ?
                        ConstantHelpers.SEX.VALUES[x.MedicalAppointment.User.Sex] : "-"
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
    }
}
