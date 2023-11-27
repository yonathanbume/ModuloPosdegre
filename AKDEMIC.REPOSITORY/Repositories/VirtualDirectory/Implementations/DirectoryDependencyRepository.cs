using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.VirtualDirectory;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.VirtualDirectory.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.VirtualDirectory.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.VirtualDirectory.Implementations
{
    public class DirectoryDependencyRepository : Repository<DirectoryDependency> , IDirectoryDependencyRepository
    {
        public DirectoryDependencyRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPeopleInChargeDatatable(DataTablesStructs.SentParameters sentParameters, Guid dependecyId, string search)
        {
            Expression<Func<DirectoryDependency, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.PhoneNumber);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Email);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Annex);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Charge);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.DirectoryDependencies.Where(x => x.DependencyId == dependecyId).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    phoneNumber = x.PhoneNumber,
                    email = x.Email,
                    annex = x.Annex,
                    charge = ConstantHelpers.VirtualDirectory.DirectoryDependency.Charge.VALUES.ContainsKey(x.Charge) == false
                            ? "Cargo Desconocido"
                            : ConstantHelpers.VirtualDirectory.DirectoryDependency.Charge.VALUES[x.Charge]
                })
                .ToListAsync();

            int recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> HasPersonInCharge(Guid dependencyId, byte charge, Guid? id = null)
            => await _context.DirectoryDependencies.AnyAsync(x => x.DependencyId == dependencyId && x.Charge == charge && x.Id != id);

        public async Task<object> GetPeopleInChargeToDirectory(PaginationParameter paginationParameters, byte filterType, string searchValue)
        {
            IQueryable<PeopleTemplate> query = null;

            switch (filterType)
            {
                //case ConstantHelpers.VirtualDirectory.PORTAL.FILTERS.AREA:
                default:
                    query = _context.DirectoryDependencies
                        .Select(x => new PeopleTemplate
                        {
                            Name = x.Name,
                            PhoneNumber = x.PhoneNumber,
                            Email = x.Email,
                            Annex = x.Annex,
                            Dependency = x.Dependency.Name,
                            Charge = ConstantHelpers.VirtualDirectory.DirectoryDependency.Charge.VALUES.ContainsKey(x.Charge) == false
                                     ? "Cargo Desconocido"
                                     : ConstantHelpers.VirtualDirectory.DirectoryDependency.Charge.VALUES[x.Charge]
                        });
                    break;

                case ConstantHelpers.VirtualDirectory.PORTAL.FILTERS.STAFF:
                    query = _context.Users.Where(x => !x.UserRoles.Any(r => r.Role.Name == ConstantHelpers.ROLES.STUDENTS || r.Role.Name == ConstantHelpers.ROLES.TEACHERS))
                        .Select(x => new PeopleTemplate
                        {
                            Name = x.FullName,
                            PhoneNumber = string.IsNullOrEmpty(x.PhoneNumber) ? "Sin Asignar" : x.PhoneNumber,
                            Email = string.IsNullOrEmpty(x.Email) ? "Sin Asignar" : x.Email,
                            //Dependency = string.Format(", ",x.UserDependencies.Select(y=>y.Dependency.Name).ToArray()),
                            Photo = x.Picture,
                            Charge = x.UserRoles.Select(y=>y.Role.Name).FirstOrDefault()
                        });
                    break;

                case ConstantHelpers.VirtualDirectory.PORTAL.FILTERS.TEACHER:
                    query = _context.Teachers
                        .Select(x => new PeopleTemplate
                        {
                            Name = x.User.FullName,
                            PhoneNumber = string.IsNullOrEmpty(x.User.PhoneNumber) ? "Sin Asignar" : x.User.PhoneNumber,
                            Email = string.IsNullOrEmpty(x.User.Email) ? "Sin Asignar": x.User.Email,
                            Charge = "Docente",
                            Photo = x.User.Picture
                        });
                    break;

            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Contains(searchValue.Trim().ToLower()));

            var recordsFiltered = await query.CountAsync();

            var data = await query.Skip(paginationParameters.CurrentNumber * paginationParameters.RecordsPerPage).Take(paginationParameters.RecordsPerPage).ToListAsync();
            var recordsTotal = data.Count();
            var result = new
            {
                recordsTotal,
                recordsFiltered,
                data
            };

            return result;
        }
    }
}
