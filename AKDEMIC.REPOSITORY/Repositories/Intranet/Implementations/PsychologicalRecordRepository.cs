using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
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
    public class PsychologicalRecordRepository : Repository<PsychologicalRecord>, IPsychologicalRecordRepository
    {
        public PsychologicalRecordRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDetailsDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<PsychologicalRecord, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.MedicalAppointment.User.FullName); break;
                case "1":
                    orderByPredicate = ((x) => x.MedicalAppointment.User.UserName); break;
                case "2":
                    orderByPredicate = ((x) => x.MedicalAppointment.User.Email); break;
                default:
                    orderByPredicate = ((x) => x.MedicalAppointment.User.FullName); break;
            }

            var query = _context.PsychologicalRecords.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                  .Select(x => new 
                  {
                      name = x.MedicalAppointment.User.FullName,
                      code = x.MedicalAppointment.User.UserName,
                      email = x.MedicalAppointment.User.Email,
                      //faculty = x.MedicalAppointment.Student.Career.Name,
                      isrehabilitaded = x.MedicalAppointment.PsychologicalRecord.Isrehabilitated
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

        public async Task<object> GetChartReport()
        {
            var result =  _context.PsychologicalRecords
                .AsEnumerable()
                .GroupBy(x=> x.Isrehabilitated)             
                .Select(x => new
                {
                    rehabilitadedname = "Rehabilitados",
                    rehabilitadeds = x.Count(c => c.Isrehabilitated == true),
                    norehabilitadedname = "Por rehabilitar",
                    norehabilitadeds = x.Count(c => c.Isrehabilitated == false)

                }).ToArray();

            return result;
        }
    }
}
