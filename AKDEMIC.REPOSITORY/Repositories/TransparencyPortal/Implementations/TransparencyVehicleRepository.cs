using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class TransparencyVehicleRepository : Repository<TransparencyVehicle>, ITransparencyVehicleRepository
    {
        public TransparencyVehicleRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetTransparencyVehicleDataTable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<TransparencyVehicle, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Ruc); break;
                case "1":
                    orderByPredicate = ((x) => x.Year); break;
                case "2":
                    orderByPredicate = ((x) => x.Month); break;
                case "3":
                    orderByPredicate = ((x) => x.VehicleClass); break;
                case "4":
                    orderByPredicate = ((x) => x.Type); break;
                case "5":
                    orderByPredicate = ((x) => x.DriverName); break;
                case "6":
                    orderByPredicate = ((x) => x.Area); break;
                case "7":
                    orderByPredicate = ((x) => x.Activity); break;
                case "8":
                    orderByPredicate = ((x) => x.FuelType); break;
                case "9":
                    orderByPredicate = ((x) => x.DistanceTraveled); break;
                case "10":
                    orderByPredicate = ((x) => x.FuelCost); break;
                case "11":
                    orderByPredicate = ((x) => x.SoatEndDate); break;
                case "12":
                    orderByPredicate = ((x) => x.VehicleRegistrationNumber); break;
                case "13":
                    orderByPredicate = ((x) => x.Observations); break;
            }

            var query = _context.TransparencyVehicles.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Ruc,
                    x.Year,
                    x.Month,
                    VehicleClass = ConstantHelpers.TEMPLATES.VEHICLECLASS.VALUES.ContainsKey(x.VehicleClass)
                            ? ConstantHelpers.TEMPLATES.VEHICLECLASS.VALUES[x.VehicleClass] : "Desconocido",
                    x.Type,
                    x.DriverName,
                    x.Area,
                    x.Activity,
                    x.FuelType,
                    DistanceTraveled = $"{x.DistanceTraveled.ToString("0.00")} KM",
                    FuelCost = $"S/. {x.FuelCost.ToString("0.00")}",
                    SoatEndDate = x.SoatEndDate.ToLocalDateFormat(),
                    x.VehicleRegistrationNumber,
                    x.Observations
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }
    }
}
