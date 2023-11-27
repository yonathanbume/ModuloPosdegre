using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Structs;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Reservation.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Reservation.Implementations
{
    public class EnvironmentRepository : Repository<ENTITIES.Models.Reservations.Environment>, IEnvironmentRepository
    {
        public EnvironmentRepository(AkdemicContext context) : base(context) { }

        public async Task<bool> AnyByCode(string code, Guid? ignoredId = null)
            => await _context.Environments.AnyAsync(x => x.Code == code && x.Id != ignoredId);

        public async Task<object> GetAllByFilters(bool? isActive = null, bool? reservationExternal = null)
        {
            var query = _context.Environments.AsQueryable();

            if (isActive.HasValue && isActive.Value)
                query = query.Where(x => x.IsActive);

            if (reservationExternal.HasValue && reservationExternal.Value)
            {
                query = query.Where(x => x.MaxReservationExternal > 0);
                var result = await query.Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.Tusne,
                    x.Capacity,
                    x.Price
                })
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Name)
                .ToArrayAsync();

                return result;
            }
            else
            {
                var result = await query.Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.Tusne,
                    x.Capacity,
                    Price = x.Price == 0 ? "Sin costo" : $"S/. {x.Price.ToString("0.00")}"
                })
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Name)
                .ToListAsync();

                return result;
            }
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetEnvironmentsSelect2ClientSide()
        {
            var result = await _context.Environments
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .ToArrayAsync();

            return result;
        }
    }
}
