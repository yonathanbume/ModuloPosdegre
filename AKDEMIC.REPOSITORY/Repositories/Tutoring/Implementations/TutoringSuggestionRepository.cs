using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Tutoring;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Tutoring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Implementations
{
    public class TutoringSuggestionRepository : Repository<TutoringSuggestion>, ITutoringSuggestionRepository
    {
        public TutoringSuggestionRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<TutoringSuggestion> pagedList, int count)> GetAllByPaginationParameter(PaginationParameter paginationParameter, string role = null)
        {
            var query = _context.TutoringSuggestions.AsQueryable();

            if (!string.IsNullOrEmpty(role))
                query = query.Where(x => x.User.UserRoles.Any(ur => ur.Role.NormalizedName == role));

            if (!string.IsNullOrEmpty(paginationParameter.SearchValue))
                query = query.Where(x => x.Title.Contains(paginationParameter.SearchValue) ||
                                    x.User.Name.Contains(paginationParameter.SearchValue) ||
                                    x.User.PaternalSurname.Contains(paginationParameter.SearchValue) ||
                                    x.User.MaternalSurname.Contains(paginationParameter.SearchValue) ||
                                    x.User.Dni.Contains(paginationParameter.SearchValue) ||
                                    x.User.UserName.Contains(paginationParameter.SearchValue));

            var count = await query.CountAsync();
            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.User.UserName)
                        : query.OrderBy(x => x.User.UserName);
                    break;
                case "1":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.User.Dni)
                        : query.OrderBy(x => x.User.Dni);
                    break;
                case "2":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.User.RawFullName)
                        : query.OrderBy(x => x.User.RawFullName);
                    break;
                case "3":
                    query = paginationParameter.SortOrder == paginationParameter.BaseOrder
                        ? query.OrderByDescending(x => x.Title)
                        : query.OrderBy(x => x.Title);
                    break;
            }

            var result = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(x => new TutoringSuggestion
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    User = new ENTITIES.Models.Generals.ApplicationUser
                    {
                        Name = x.User.Name,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname
                    },
                    Title = x.Title
                }).ToListAsync();

            return (result, count);
        }

        public override async Task<TutoringSuggestion> Get(Guid id)
            => await _context.TutoringSuggestions
                .Where(x => x.Id == id)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetTutoringSuggestionsDatatable(DataTablesStructs.SentParameters sentParameters, byte? status = null, string searchValue = null, string role = null)
        {
            Expression<Func<TutoringSuggestion, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Title);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt);

                    break;
                case "3":
                    orderByPredicate = ((x) => _context.Teachers.Any(y => y.UserId == x.UserId) ? _context.Teachers.Where(y => y.UserId == x.UserId).Select(a => a.Career.Name).FirstOrDefault() : _context.Students.Where(y => y.UserId == x.UserId).Select(a => a.Career.Name).FirstOrDefault());

                    break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt);

                    break;
            }
            var query = _context.TutoringSuggestions.Where(x => x.Status != 0)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(role))
                query = query.Where(x => x.User.UserRoles.Any(ur => ur.Role.NormalizedName == role));

            if (status.HasValue)
                query = query.Where(x => x.Status == status);
            if (!string.IsNullOrEmpty(searchValue))
                query = query
                    .Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper())
                            || x.Title.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();
            
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new 
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    User = new ENTITIES.Models.Generals.ApplicationUser
                    {
                        Name = x.User.Name,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        FullName = x.User.FullName
                    },
                    Title = x.Title,
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    Status = ConstantHelpers.TUTORING.STATUSSUGGESTION.VALUES[x.Status],
                    Career = _context.Teachers.Any(y => y.UserId == x.UserId) ? _context.Teachers.Where(y => y.UserId == x.UserId).Select(a => a.Career.Name).FirstOrDefault() : _context.Students.Where(y => y.UserId == x.UserId).Select(a => a.Career.Name).FirstOrDefault()
                })
                .ToListAsync();

            var recordsTotal = data.Count;

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
