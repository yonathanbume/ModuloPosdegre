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
using Microsoft.AspNetCore.Mvc.Rendering;
using AKDEMIC.WEBSERVICE.Services.Chat.Models.Request;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringCoordinatorRepository : Repository<TutoringCoordinator>, ITutoringCoordinatorRepository
    {
        public TutoringCoordinatorRepository(AkdemicContext context) : base(context) { }

        public async Task<TutoringCoordinator> GetWithUserById(string tutoringCoordinatorId)
            => await _context.TutoringCoordinators
                .Where(x => x.UserId == tutoringCoordinatorId)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

        public override async Task DeleteById(string tutoringCoordinatorId)
        {
            var tutoringCoordinator = await _context.TutoringCoordinators
                .Include(x => x.User)
                .ThenInclude(x => x.UserRoles)
                .Where(x => x.UserId == tutoringCoordinatorId)
                .FirstOrDefaultAsync();
            if(tutoringCoordinator.User.UserRoles.Any())
                _context.UserRoles.RemoveRange(tutoringCoordinator.User.UserRoles);
            _context.Users.Remove(tutoringCoordinator.User);
            await _context.SaveChangesAsync();
            await base.Delete(tutoringCoordinator);
        }

        public async Task<DataTablesStructs.ReturnedData<TutoringCoordinator>> GetTutoringCoordinatorsDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null, Guid? careerId = null)
        {
            Expression<Func<TutoringCoordinator, dynamic>> orderByPredicate = null;

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
                    orderByPredicate = ((x) => x.Career.Name);

                    break;
                default:
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
            }
            var query = _context.TutoringCoordinators
                    .AsNoTracking();
            if (careerId.HasValue)
                query = query.Where(q => q.CareerId == careerId);


            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.User.UserName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new TutoringCoordinator
                {
                    CareerId = x.CareerId,
                    Career = new ENTITIES.Models.Generals.Career
                    {
                        Name = x.Career.Name
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
                        FullName = x.User.FullName,
                        Teachers = x.User.Teachers.Select(y => new ENTITIES.Models.Generals.Teacher 
                        {
                            TeacherDedication = new ENTITIES.Models.TeachingManagement.TeacherDedication
                            {
                                Name = y.TeacherDedication.Name
                            },
                        }).ToList()
                    }
                })
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<TutoringCoordinator>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public SelectList GetTypeTime()
        {
            var result = new SelectList(ConstantHelpers.TUTORING.TYPECOORDINATOR.VALUES, "Key", "Value");

            return result;
        }

        public async Task<TutoringCoordinator> GetByCareerId(Guid careerId)
            => await _context.TutoringCoordinators.Where(x => x.CareerId == careerId).FirstOrDefaultAsync();
        public async Task<TutoringCoordinator> GetWithData(string id)
            => await _context.TutoringCoordinators.Include(x => x.Career).Include(x => x.User).Where(x => x.UserId == id).FirstOrDefaultAsync();
        public async Task<bool> AnyByCareerId(Guid careerId)
            => await _context.TutoringCoordinators.AnyAsync(x => x.CareerId == careerId);

        public async Task<string> GetCareerByUserId(string userId)
        {
            return await _context.TutoringCoordinators.Where(x => x.UserId == userId).Select(x => x.Career.Name).FirstOrDefaultAsync();
        }
    }
}
