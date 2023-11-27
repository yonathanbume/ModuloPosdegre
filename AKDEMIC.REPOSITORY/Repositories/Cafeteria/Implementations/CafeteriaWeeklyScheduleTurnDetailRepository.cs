using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class CafeteriaWeeklyScheduleTurnDetailRepository : Repository<CafeteriaWeeklyScheduleTurnDetail>, ICafeteriaWeeklyScheduleTurnDetailRepository
    {
        public CafeteriaWeeklyScheduleTurnDetailRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCafeteriaWeeklyScheduleTurnDetailDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, Guid cafeteriaWeeklyScheduleId )
        {
            var query = _context.CafeteriaWeeklyScheduleTurnDetails.Where(x => x.CafeteriaWeeklyScheduleId == cafeteriaWeeklyScheduleId).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => ConstantHelpers.TURN_TYPE.VALUES[x.Type].Contains(searchValue));
            }           

            var recordsFiltered = await query.CountAsync();
            var data = query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(s => new
                {
                    s.Id,
                    TurnType = ConstantHelpers.TURN_TYPE.VALUES[s.Type],
                    TurnTypeByte = s.Type,
                    FormatedStartTime = s.StartTime.ToLocalDateTimeFormatUtc(),
                    FormatedEndTime = s.EndTime.ToLocalDateTimeFormatUtc(),
                    HasMenu = s.MenuPlateId.HasValue ? true : false,
                    s.MenuPlateId
                })
                .ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<Tuple<bool,string>> ValidateTurnAndHour(int Type, Guid CafeteriaWeeklyScheduleIdParameter, TimeSpan StartTimeParameter, TimeSpan EndTimeParameter, Guid? Id = null)
        {
            var boolToSearch = false;
            if ((StartTimeParameter > EndTimeParameter) || (StartTimeParameter == EndTimeParameter) )
            {
                boolToSearch = true;
                return new Tuple<bool, string>(boolToSearch, "Las horas son inválidas");
            }            
            var query = _context.CafeteriaWeeklyScheduleTurnDetails.Where(x=>x.CafeteriaWeeklyScheduleId == CafeteriaWeeklyScheduleIdParameter).AsQueryable();
            if (Id.HasValue)
            {
                query = query.Where(x => x.Id != Id);
            }

            if (await query.AnyAsync(x => x.Type == Type))
            {
                boolToSearch = true;
                return new Tuple<bool, string>(boolToSearch, "Ya existe un turno igual asignado");
            }
            else
            {
                var tpmData = await query.ToListAsync();
                if (tpmData.Any(x =>
                   ((StartTimeParameter == x.StartTime.ToLocalTimeSpanUtc()) && (EndTimeParameter == x.EndTime.ToLocalTimeSpanUtc()))
                || ((StartTimeParameter == x.StartTime.ToLocalTimeSpanUtc()) && (x.EndTime.ToLocalTimeSpanUtc() <= EndTimeParameter))
                || ((StartTimeParameter <= x.StartTime.ToLocalTimeSpanUtc()) && (x.EndTime.ToLocalTimeSpanUtc() == EndTimeParameter))
                || ((x.StartTime.ToLocalTimeSpanUtc() < StartTimeParameter) && (StartTimeParameter < x.EndTime.ToLocalTimeSpanUtc()))
                || ((x.StartTime.ToLocalTimeSpanUtc() < EndTimeParameter) && (EndTimeParameter < x.EndTime.ToLocalTimeSpanUtc()))
                || ((StartTimeParameter < x.StartTime.ToLocalTimeSpanUtc()) && (EndTimeParameter > x.EndTime.ToLocalTimeSpanUtc()))))
                {
                    boolToSearch = true;
                    return new Tuple<bool, string>(boolToSearch, "Existen cruces con otros turnos existentes");
                }
                else
                {
                    boolToSearch = false;
                    return new Tuple<bool, string>(boolToSearch, "Correcciones válidos");
                }
                   
            }
         
        }
    }
}
