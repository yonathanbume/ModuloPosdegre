using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class UnitMeasurementRepository: Repository<UnitMeasurement>, IUnitMeasurementRepository
    {
        public UnitMeasurementRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUnitMeasurementDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            
            var query = _context.UnitMeasurements                
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.Contains(searchValue));
            }

            Expression<Func<UnitMeasurement, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                id = x.Id,
                description = x.Description

            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<object> GetUnitMeasurementSelect()
        {
            var result = _context.UnitMeasurements.Select(x => new
            {
                x.Id,
                text = x.Description
            });
            return await result.ToListAsync();
        }
    }
}
