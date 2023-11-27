using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class SupportOfficeUserRepository : Repository<SupportOfficeUser>, ISupportOfficeUserRepository
    {
        public SupportOfficeUserRepository(AkdemicContext context) : base(context)
        {
        }

        public override async Task<SupportOfficeUser> Get(string id)
            => await _context.SupportOfficeUsers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == id);

        public async Task<DataTablesStructs.ReturnedData<SupportOfficeUser>> GetSupportOfficeUsersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? supportOfficeId = null)
        {
            Expression<Func<SupportOfficeUser, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.Dni);

                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.UserName);

                    break;
                case "4":
                    orderByPredicate = ((x) => x.SupportOffice.Name);

                    break;
                default:
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
            }

            var query = _context.SupportOfficeUsers
                .AsNoTracking();


            if (supportOfficeId.HasValue)
                query = query.Where(q => q.SupportOfficeId == supportOfficeId);


            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.User.UserName.ToUpper().Contains(searchValue));

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new SupportOfficeUser
                {
                    SupportOfficeId = x.SupportOfficeId,
                    SupportOffice = new SupportOffice
                    {
                        Name = x.SupportOffice.Name
                    },
                    UserId = x.UserId,
                    User = new ENTITIES.Models.Generals.ApplicationUser
                    {
                        Name = x.User.Name,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        Dni = x.User.Dni,
                        UserName = x.User.UserName,
                        Picture = x.User.Picture,
                        FullName = x.User.FullName
                    }
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<SupportOfficeUser>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<SupportOfficeUser> GetByUser(string userId)
            => await _context.SupportOfficeUsers.Include(x => x.SupportOffice).Where(x => x.UserId == userId).FirstOrDefaultAsync();

        public async Task<SupportOffice> GetSupportOfficeByUser(string userId)
            => await _context.SupportOfficeUsers.Where(x => x.UserId == userId).Select(x => x.SupportOffice).FirstOrDefaultAsync();
  
    }
}
