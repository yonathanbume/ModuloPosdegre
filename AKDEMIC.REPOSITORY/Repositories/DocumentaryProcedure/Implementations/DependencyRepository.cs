using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Template;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Dependency;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class DependencyRepository : Repository<Dependency>, IDependencyRepository
    {
        public DependencyRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<Dependency>> GetDependenciesDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null, Expression<Func<Dependency, Dependency>> selectPredicate = null, Expression<Func<Dependency, dynamic>> orderByPredicate = null, Func<Dependency, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Dependencies
                .Include(x => x.User)
                .Include(x => x.SuperiorDependency)
                .AsNoTracking();

            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower().Trim()) || x.Acronym.ToLower().Contains(searchValue.ToLower().Trim()));
            }

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(selectPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<Dependency>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetDependenciesSelect2Configuration(Select2Structs.RequestParameters requestParameters, List<Guid> userDependencies = null, bool derivationTypeConfig = false, Func<Dependency, string[]> searchValuePredicate = null, string searchValue = null, Guid? userDependencyId = null)
        {
            var query = _context.Dependencies
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Contains(searchValue.Trim().ToLower()));

            var results = new List<Select2Structs.Result>();
            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;

            if (derivationTypeConfig)
            {
                if (userDependencyId.HasValue)
                {
                    var userDependency = await _context.Dependencies.Where(x => x.Id == userDependencyId).FirstOrDefaultAsync();

                    if (userDependency.SuperiorDependencyId.HasValue)
                    {
                        query = query.Where(x => x.Id == userDependency.SuperiorDependencyId);
                    }
                    else
                    {

                        var result = new List<Dependency>();
                        var superiorDependencies = await _context.Dependencies.Where(x => !x.SuperiorDependencyId.HasValue).ToListAsync();

                        result.AddRange(superiorDependencies);

                        var dependency = await GetHierarchicalTree(userDependency.Id);

                        var currentDependecy = dependency;
                        result.Add(currentDependecy);

                        while (currentDependecy.Dependencies.Any())
                        {
                            result.Add(currentDependecy.Dependencies.First());
                            currentDependecy = currentDependecy.Dependencies.First();
                        }

                        if (!string.IsNullOrEmpty(searchValue))
                            result = result.Where(x => x.Name.ToLower().Contains(searchValue.ToLower())).ToList();

                        results = result
                            .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                            .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                            .Select(x => new Select2Structs.Result
                            {
                                Id = x.Id,
                                Text = x.Name
                            })
                            .ToList();

                        return new Select2Structs.ResponseParameters
                        {
                            Pagination = new Select2Structs.Pagination
                            {
                                More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                            },
                            Results = results
                        };

                    }

                }

                results = await query
                    .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                    .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                    .Select(x => new Select2Structs.Result
                    {
                        Id = x.Id,
                        Text = x.Name
                    }, searchValue)
                    .ToListAsync();
            }
            else
            {
                results = await query
               .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
               .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
               .Select(x => new Select2Structs.Result
               {
                   Id = x.Id,
                   Text = x.Name
               })
               .ToListAsync();
            }

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };

        }

        private async Task<Select2Structs.ResponseParameters> GetDependenciesSelect2(Select2Structs.RequestParameters requestParameters, string userId = null, Expression<Func<Dependency, Select2Structs.Result>> selectPredicate = null, Expression<Func<Dependency, string[]>> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Dependencies
                .Include(x => x.SuperiorDependency)
                .AsNoTracking();

            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {

            }

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Select(selectPredicate, searchValue, searchValuePredicate)
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetExternalProcedureDependenciesSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Dependency, Select2Structs.Result>> selectPredicate = null, Func<Dependency, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Dependencies
                .Where(x => _context.ExternalProcedures.Any(y => y.DependencyId == x.Id))
                .AsNoTracking();

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetProcedureDependenciesSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Dependency, Select2Structs.Result>> selectPredicate = null, Func<Dependency, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Dependencies
                .Where(x => _context.ProcedureDependencies.Any(y => y.DependencyId == x.Id))
                .AsNoTracking();

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        #endregion

        #region PUBLIC

        public async Task<Select2Structs.ResponseParameters> GetDependencyDirectorsSelect2(Select2Structs.RequestParameters requestParameters, string search)
        {
            var query = _context.Dependencies.Where(x => !string.IsNullOrEmpty(x.UserId)).AsNoTracking();

            //var query = _context.Users.Join(_context.Dependencies.Where(x=>!string.IsNullOrEmpty(x.UserId)),
            //        au => au.Id,
            //        d => d.UserId,
            //        (au, d) => new
            //        {
            //            au.Id,
            //            au.FullName
            //        }
            //    ).AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.FullName.ToLower().Trim().Contains(search.ToLower().Trim()));

            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;

            var results = await query
               .Select(x=> new Select2Structs.Result
               {
                   Id = x.User.Id,
                   Text = x.User.FullName
               })
               .Distinct()
               .Skip(currentPage * ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
               .Take(ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE)
               .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = results
            };
        }

        public async Task<List<Dependency>> GetDependenciesByCareer(Guid careerId)
            => await _context.Dependencies.Where(x => x.CareerId == careerId).ToListAsync();

        public async Task<List<Dependency>> GetDependenciesByFaculty(Guid facultyId)
            => await _context.Dependencies.Where(x => x.FacultyId == facultyId).ToListAsync();

        public async Task<bool> AnyDependencyByAcronym(string acronym)
        {
            var query = _context.Dependencies.Where(x => x.Acronym.ToUpper().Equals(acronym.ToUpper()));
            return await query.AnyAsync();
        }

        public async Task<bool> AnyDependencyByAcronymIgnoreQueryFilters(string acronym)
        {
            var query = _context.Dependencies.IgnoreQueryFilters().Where(x => x.Acronym.ToUpper().Equals(acronym.ToUpper()));
            return await query.AnyAsync();
        }

        public async Task<bool> AnyDependencyByName(string name, Guid? ignoredId = null)
        {
            var query = _context.Dependencies.Where(x => x.Id != ignoredId && x.Name.ToUpper().Equals(name.ToUpper()));

            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Dependency>> GetDependencies()
        {
            var query = _context.Dependencies.SelectDependency();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Dependency>> GetDependenciesByProcedure(Guid procedureId)
        {
            var query = _context.Dependencies.SelectDependency(_context, procedureId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Dependency>> GetDependenciesByUserDependencyUser(string userDependencyUserId)
        {
            var query = _context.UserDependencies
                .Where(x => x.UserId == userDependencyUserId)
                .Select(x => new Dependency
                {
                    Id = x.DependencyId,
                    Name = x.Dependency.Name,
                    Acronym = x.Dependency.Acronym,
                    Signature = x.Dependency.Signature,
                });

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<Dependency>> GetDependenciesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Dependency, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Acronym);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetDependenciesDatatable(sentParameters, null, ExpressionHelpers.SelectDependency(), orderByPredicate, (x) => new[] { x.Acronym, x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<Dependency>> GetDependenciesDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            Expression<Func<Dependency, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Acronym);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetDependenciesDatatable(sentParameters, userId, ExpressionHelpers.SelectDependency(), orderByPredicate, (x) => new[] { x.Acronym, x.Name }, searchValue);
        }
        public async Task<List<DependencyExcelTemplate>> GetDependenciesDatatableExcel(string userId = null, string searchValue = null)
        {
            var query = _context.Dependencies
               .Include(x => x.User)
               .Include(x => x.SuperiorDependency).ThenInclude(x => x.User)
               .AsNoTracking();

            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query.Select(x => new DependencyExcelTemplate
            {
                DependencyName = x.Name,
                Acronym = x.Acronym,
                Director = x.User.FullName,
                SuperiorityDependency = x.SuperiorDependency.Name,
                AreaManager = _context.DirectoryDependencies.Where(y => y.DependencyId == x.Id && y.Charge == ConstantHelpers.VirtualDirectory.DirectoryDependency.Charge.RESPONSIBLE).Select(y => y.Name).FirstOrDefault(),
                Secretary = _context.DirectoryDependencies.Where(y => y.DependencyId == x.Id && y.Charge == ConstantHelpers.VirtualDirectory.DirectoryDependency.Charge.SECRETARY).Select(y => y.Name).FirstOrDefault()

            }).ToListAsync();

            var recordsTotal = data.Count;

            return data;
        }
        public async Task<DataTablesStructs.ReturnedData<Dependency>> GetProcedureDependenciesDatatableByProcedure(DataTablesStructs.SentParameters sentParameters, Guid procedureId, string searchValue = null)
        {
            Expression<Func<Dependency, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Acronym);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetDependenciesDatatable(sentParameters, null, ExpressionHelpers.SelectDependency(_context, procedureId), orderByPredicate, (x) => new[] { x.Acronym, x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetDependenciesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {

            return await GetDependenciesSelect2(requestParameters, null, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetDependeciesHigher(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            var query = _context.Dependencies
                .Include(x => x.SuperiorDependency)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                }
                else
                    query = query.Where(x => x.Name.Contains(searchValue));
                //query = query.WhereSearchValue((x) => new[] { x.User.FullName }, searchValue);
            }

            Expression<Func<Dependency, Select2Structs.Result>> selectPredicate = (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            };

            return await query.OrderBy(x => x.Name).ToSelect2(requestParameters, selectPredicate, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }
        public async Task<Select2Structs.ResponseParameters> GetDependenciesSelect2Configuration(Select2Structs.RequestParameters requestParameters, List<Guid> userDependencies = null, bool derivationConfiguration = false, string searchValue = null, Guid? userDependencyId = null)
        {
            return await GetDependenciesSelect2Configuration(requestParameters, userDependencies, derivationConfiguration, (x) => new[] { x.Name }, searchValue, userDependencyId);
        }

        public async Task<Select2Structs.ResponseParameters> GetDependenciesSelect2ByUser(Select2Structs.RequestParameters requestParameters, string userId, string searchValue = null)
        {
            return await GetDependenciesSelect2(requestParameters, userId, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetExternalProcedureDependenciesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetExternalProcedureDependenciesSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetProcedureDependenciesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetProcedureDependenciesSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetUserInternalProcedureDependenciesSelect2ByInternalProcedure(Select2Structs.RequestParameters requestParameters, Guid internalProcedureId, string searchValue = null)
        {
            return await GetDependenciesSelect2(requestParameters, null, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Selected = _context.UserInternalProcedures.Where(y => y.DependencyId == x.Id && y.InternalProcedureId == internalProcedureId).Any(),
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<object> GetAllAsSelect2ClientSide()
        {
            var dependencies = await _context.Dependencies
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            return dependencies;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetReportBalanceByCostOfCenterDatatable(DataTablesStructs.SentParameters sentParameters)
        {

            Expression<Func<Dependency, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Acronym);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.Dependencies.Include(x => x.Incomes).ThenInclude(x => x.Payment).AsNoTracking();


            var recordsFiltered = await query.CountAsync();

            var data = await query
                            .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                            .Select(x => new
                            {
                                id = x.Id,
                                code = x.Acronym,
                                name = x.Name,
                                //income = _context.Payments.Where(p => x.Concepts.Any(c => c.Id == p.EntityId)).Sum(c => c.Total),
                                income = x.Incomes.Sum(y => y.Amount),
                                //income = x.Incomes.Sum(y => y.Amount),
                                expenses = x.Expenses.Sum(e => e.Amount),
                                provision = x.ExpenditureProvisions.Where(p => p.Status == ConstantHelpers.ExpenditureProvision.Status.PENDING).Sum(p => p.Amount)
                            }).ToListAsync();


            var data2 = data
                .Select(x => new
                {
                    Id = x.id,
                    Code = x.code,
                    Dependency = x.name,
                    Balance = x.income - x.expenses,
                    Available = x.income - x.expenses - x.provision
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            var recordsTotal = data2.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data2,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<List<BalanceByCostOfCenterTemplate>> GetReportBalanceByCostOfCenter()
        {
            var query = _context.Dependencies.Include(x => x.Incomes).ThenInclude(x => x.Payment).AsNoTracking();


            var recordsFiltered = await query.CountAsync();

            var data = await query
                            .Select(x => new
                            {
                                id = x.Id,
                                code = x.Acronym,
                                name = x.Name,
                                income = x.Incomes.Sum(y => y.Amount),
                                expenses = x.Expenses.Sum(e => e.Amount),
                                provision = x.ExpenditureProvisions.Where(p => p.Status == ConstantHelpers.ExpenditureProvision.Status.PENDING).Sum(p => p.Amount)
                            }).ToListAsync();


            var data2 = data
                .Select(x => new BalanceByCostOfCenterTemplate
                {
                    Id = x.id,
                    Code = x.code,
                    Dependency = x.name,
                    Balance = x.income - x.expenses,
                    Available = x.income - x.expenses - x.provision
                })
                .ToList();

            return data2;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetReportIncomeByCostOfCenterDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Dependency, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Acronym);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.Dependencies
                .Select(x => new
                {
                    id = x.Id,
                    account = x.Acronym,
                    name = x.Name,
                    totalAmount = x.Incomes.DefaultIfEmpty().Sum(y => y.Amount)
                })
                .AsQueryable();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.account.Contains(searchValue) || x.name.Contains(searchValue));
            }

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDependencyDatatableReport(DataTablesStructs.SentParameters sentParameters, 
            string search, DateTime? startDate = null, DateTime? endDate = null, bool showAll = false)
        {
            Expression<Func<Dependency, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Acronym;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    break;
            }

            var qryIncomes = _context.Incomes.AsNoTracking();

            if (startDate.HasValue) qryIncomes = qryIncomes.Where(x => x.Date >= startDate);
            if (endDate.HasValue) qryIncomes = qryIncomes.Where(x => x.Date <= endDate);

            var incomes = await qryIncomes.ToListAsync();

            var query = _context.Dependencies
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search)) 
                query = query.Where(x => x.Acronym.ToUpper().Contains(search.ToUpper()) || x.Name.ToUpper().Contains(search.ToUpper()));

            var dbdata = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    account = x.Acronym,
                    name = x.Name
                })
                .ToListAsync();

            var data = dbdata
                .Select(x => new
                {
                    x.id,
                    x.account,
                    x.name,
                    totalAmount = incomes.Where(y => y.DependencyId == x.id).Sum(y => y.Amount)
                }).ToList();

            if (!showAll) data = data.Where(x => x.totalAmount > 0).ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<decimal> GetDependencyDatatableReportTotalAmount(string search, DateTime? startDate = null, DateTime? endDate = null)
        {
            var qryIncomes = _context.Incomes.AsNoTracking();

            if (startDate.HasValue) qryIncomes = qryIncomes.Where(x => x.Date >= startDate);
            if (endDate.HasValue) qryIncomes = qryIncomes.Where(x => x.Date <= endDate);

            if (!string.IsNullOrEmpty(search))
            {
                var dependencies = _context.Dependencies
                    .Where(x => x.Acronym.ToUpper().Contains(search.ToUpper()) || x.Name.ToUpper().Contains(search.ToUpper()))
                    .Select(x => x.Id).ToHashSet();

                qryIncomes = qryIncomes.Where(x => dependencies.Contains(x.DependencyId));
            }

            var result = await qryIncomes.SumAsync(x => x.Amount);

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDependencyDatatableReportManual(DataTablesStructs.SentParameters sentParameters, int? year = null)
        {
            Expression<Func<Dependency, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Acronym;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    //orderByPredicate = (x) => x.Acronym;
                    break;
            }

            var currentYear = DateTime.Now.Year;
            if (year.HasValue && year > 0) currentYear = year.Value;

            var query = _context.Dependencies
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var payments = await _context.Payments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID
                && x.ConceptId.HasValue
                && x.PaymentDate.HasValue
                && x.PaymentDate.Value.Year == currentYear)
                .IgnoreQueryFilters()
                .Select(x => new
                {
                    x.Concept.DependencyId,
                    PaymentDate = x.PaymentDate.Value,
                    x.Total
                })
                .ToListAsync();

            var data = query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .AsEnumerable()
                .Select(x => new
                {
                    id = x.Id,
                    account = x.Acronym,
                    name = x.Name,
                    january = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 1).Sum(p => p.Total), // x.Concepts.Sum(c => c.Payments.Where(p => p.PaymentDate.HasValue && p.PaymentDate.Value.Year == currentYear && p.PaymentDate.Value.Month == 1).Sum(p => p.Total)),
                    february = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 2).Sum(p => p.Total),
                    march = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 3).Sum(p => p.Total),
                    april = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 4).Sum(p => p.Total),
                    may = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 5).Sum(p => p.Total),
                    june = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 6).Sum(p => p.Total),
                    july = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 7).Sum(p => p.Total),
                    august = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 8).Sum(p => p.Total),
                    september = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 9).Sum(p => p.Total),
                    october = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 10).Sum(p => p.Total),
                    november = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 11).Sum(p => p.Total),
                    december = payments.Where(p => p.DependencyId == x.Id && p.PaymentDate.Month == 12).Sum(p => p.Total),
                    totalAmount = payments.Where(p => p.DependencyId == x.Id).Sum(p => p.Total)
                }).ToList();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<object> GetBudgetBalance()
        {
            var data = await _context.Dependencies
                  .Select(x => new
                  {
                      id = x.Id,
                      code = x.Acronym,
                      name = x.Name,
                      income = _context.Payments.Where(p => x.Concepts.Any(c => c.Id == p.EntityId)).Sum(c => c.Total),
                      expenses = x.Expenses.Sum(e => e.Amount),
                      provision = x.ExpenditureProvisions.Where(p => p.Status == ConstantHelpers.ExpenditureProvision.Status.PENDING).Sum(p => p.Amount)
                  }).ToListAsync();

            var pagedList = data
                .Select(x => new
                {
                    x.id,
                    code = x.code,
                    dependency = x.name,
                    balance = x.income - x.expenses,
                    available = x.income - x.expenses - x.provision
                }).ToList();

            return pagedList;
        }

        public async Task<List<Dependency>> GetAllWithData()
        {
            var result = await _context.Dependencies.Include(x => x.Concepts).Include(x => x.Expenses).Include(x => x.ExpenseOutputs).Include(x => x.ExpenditureProvisions).ToListAsync();

            return result;
        }
        public async Task<Dependency> GetDependencyByAcronym(string acronym)
            => await _context.Dependencies.FirstOrDefaultAsync(x => x.Acronym == acronym);

        public async Task<object> GetCostCenterSelect()
        {
            var data = await _context.Dependencies
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            data.Insert(0, new { id = new Guid(), text = "Todas" });

            return data;
        }

        public async Task<object> GetDependenciesJson()
        {
            var data = await _context.Dependencies
             .Select(x => new
             {
                 id = x.Id,
                 text = x.Name
             }).OrderBy(x => x.text).ToListAsync();

            var result = new
            {
                items = data
            };

            return result;
        }

        public async Task<object> GetDependeciesAllJson()
        {
            var data = await _context.Dependencies
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).ToListAsync();

            data.Insert(0, new { id = new Guid(), text = "Todas" });

            var result = new
            {
                items = data
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDependenciesObjectDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<Dependency, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Acronym);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName);

                    break;
                default:
                    break;
            }

            var query = _context.Dependencies.AsQueryable();

            var total = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.Trim().ToLower().Contains(search.Trim().ToLower()));

            var data = await query
             .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
             .Skip(sentParameters.PagingFirstRecord)
             .Take(sentParameters.RecordsPerDraw)
             .Select(x => new
             {
                 id = x.Id,
                 acronym = x.Acronym,
                 name = x.Name,
                 x.IsActive
             })
             .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = total,
                RecordsTotal = recordsTotal
            };
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Dependency> GetDependencyByName(string name)
            => await _context.Dependencies.Where(x => x.Name == name).FirstOrDefaultAsync();

        public async Task<DataTablesStructs.ReturnedData<object>> GetBudgetBalanceReport(Guid? dependencyId = null, DateTime? month = null, ClaimsPrincipal user = null)
        {
            var cutDependency = await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.EconomicManagement.CUT_DEPENDENCY);
            var cutDependencyId = (Guid?)null;
            if (!string.IsNullOrEmpty(cutDependency)) cutDependencyId = Guid.Parse(cutDependency);

            var qryDependencies = _context.Dependencies.AsNoTracking();
            var qryIncomes = _context.Incomes.AsNoTracking();
            var qryTransfers = _context.BalanceTransfers.AsNoTracking();
            var qryExpenses = _context.Expenses.AsNoTracking();
            var qryExpenseOutputs = _context.ExpenseOutputs.AsNoTracking();
            var qryExpenditureProvisions = _context.ExpenditureProvisions.AsNoTracking();

            if (dependencyId.HasValue && dependencyId != Guid.Empty)
            {
                qryDependencies = qryDependencies.Where(x => x.Id == dependencyId);
                qryIncomes = qryIncomes.Where(x => x.DependencyId == dependencyId);
                qryTransfers = qryTransfers.Where(x => x.FromDependencyId == dependencyId || x.ToDependencyId == dependencyId);
                qryExpenses = qryExpenses.Where(x => x.DependencyId == dependencyId);
                qryExpenseOutputs = qryExpenseOutputs.Where(x => x.DependencyId == dependencyId);
                qryExpenditureProvisions = qryExpenditureProvisions.Where(x => x.DependencyId == dependencyId);
            }

            if (month.HasValue)
            {
                var firstDayOfMonth = new DateTime(month.Value.Year, month.Value.Month, 1).ToUtcDateTime();
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddTicks(-1);

                qryIncomes = qryIncomes.Where(x => firstDayOfMonth <= x.Date && x.Date <= lastDayOfMonth);
                qryTransfers = qryTransfers.Where(x => firstDayOfMonth <= x.Date && x.Date <= lastDayOfMonth);
                qryExpenses = qryExpenses.Where(x => firstDayOfMonth <= x.Date && x.Date <= lastDayOfMonth);
                qryExpenseOutputs = qryExpenseOutputs.Where(x => firstDayOfMonth <= x.CreatedAt && x.CreatedAt <= lastDayOfMonth);
                //qryExpenditureProvisions = qryExpenditureProvisions.Where(x => firstDayOfMonth <= x.Date && x.Date <= lastDayOfMonth);
            }

            var dependencies = await qryDependencies.ToListAsync();

            var incomes = await qryIncomes.ToListAsync();
            var transfers = await qryTransfers.ToListAsync();
            var expenses = await qryExpenses.ToListAsync();
            var expenseOutputs = await qryExpenseOutputs.ToListAsync();
            var expenditureProvisions = await qryExpenditureProvisions.ToListAsync();

            var outcomeAccountIncomes = new List<Income>();
            if (user != null && user.IsInRole(ConstantHelpers.ROLES.INCOMES))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var accounts = await _context.UserCurrentAccounts.Where(x => x.UserId == userId).ToListAsync();
                var incomesAccount = accounts.Any(x => !x.IsExpenseAccount) ? accounts.FirstOrDefault(x => !x.IsExpenseAccount).CurrentAccountId : (Guid?)null;
                var outcomesAccount = accounts.Any(x => x.IsExpenseAccount) ? accounts.FirstOrDefault(x => x.IsExpenseAccount).CurrentAccountId : (Guid?)null;

                if (outcomesAccount != null) outcomeAccountIncomes = incomes.Where(x => x.CurrentAccountId.HasValue && x.CurrentAccountId == outcomesAccount).ToList();
                incomes = incomes.Where(x => x.CurrentAccountId.HasValue && x.CurrentAccountId == incomesAccount).ToList();
            }

            var data = dependencies
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Acronym,
                    name = x.Name,
                    prevbalance = expenses.Where(e => e.DependencyId == x.Id && !e.IsCanceled && e.Type == ConstantHelpers.Treasury.ExpenseTypes.PreviousBalance).Sum(e => e.Amount),

                    //cta. ingresos ingresos y gastos
                    income = incomes.Where(p => p.DependencyId == x.Id && p.Type == ConstantHelpers.Treasury.Income.Type.INCOME).Sum(c => c.Amount),
                    devolutions = expenses.Where(e => e.DependencyId == x.Id && !e.IsCanceled && e.Type == ConstantHelpers.Treasury.ExpenseTypes.Devolution).Sum(e => e.Amount)
                        + transfers.Where(y => y.FromDependencyId == x.Id && y.IsCutTransfer && y.CutType == ConstantHelpers.Treasury.CutType.Outcome).Sum(y => y.Amount),

                    //cta. egresos ingresos y gastos
                    canceled = expenses.Where(e => e.DependencyId == x.Id && e.IsCanceled || e.Type == ConstantHelpers.Treasury.ExpenseTypes.Annulment).Sum(e => e.Amount)
                        + transfers.Where(y => y.ToDependencyId == x.Id && y.IsCutTransfer && y.CutType == ConstantHelpers.Treasury.CutType.Income).Sum(y => y.Amount)
                        + outcomeAccountIncomes.Where(p => p.DependencyId == x.Id && p.Type == ConstantHelpers.Treasury.Income.Type.INCOME).Sum(c => c.Amount),
                    expenses = expenses.Where(e => e.DependencyId == x.Id && !e.IsCanceled && e.Type == ConstantHelpers.Treasury.ExpenseTypes.Expense).Sum(e => e.Amount)
                        + expenseOutputs.Where(e => e.DependencyId == x.Id).Sum(e => e.Amount),

                    transfers = transfers.Where(y => y.ToDependencyId == x.Id && !y.IsCutTransfer).Sum(y => y.Amount) - transfers.Where(y => y.FromDependencyId == x.Id && !y.IsCutTransfer).Sum(y => y.Amount),
                    provision = expenditureProvisions.Where(p => p.DependencyId == x.Id && p.Status == ConstantHelpers.ExpenditureProvision.Status.PENDING).Sum(p => p.Amount)
                }).ToList();

            data = data.Where(x => x.income > 0 || x.id == cutDependencyId).ToList();

            var pagedList = data
                .Select(x => new
                {
                    x.id,
                    x.code,
                    dependency = x.name,

                    x.prevbalance,

                    x.income,
                    x.devolutions,

                    x.canceled,
                    x.expenses,

                    x.transfers,
                    x.provision,
                    balance = x.prevbalance + x.income - x.devolutions + x.canceled - x.expenses + x.transfers,
                    available = x.prevbalance + x.income - x.devolutions + x.canceled - x.expenses + x.transfers - x.provision
                }).ToList();

            var filterRecords = pagedList.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = pagedList,
                DrawCounter = 0,
                RecordsFiltered = filterRecords,
                RecordsTotal = filterRecords,
            }; ;
        }

        public async Task<decimal> GetAvailableAmount(Guid dependencyId)
        {
            var incomes = await _context.Incomes.Where(x => x.DependencyId == dependencyId).ToListAsync();
            var expenses = await _context.Expenses.Where(x => x.DependencyId == dependencyId).ToListAsync();
            var provisions = await _context.ExpenditureProvisions.Where(x => x.DependencyId == dependencyId && x.Status == ConstantHelpers.ExpenditureProvision.Status.PENDING).ToListAsync();
            var transfers = await _context.BalanceTransfers.Where(x => x.FromDependencyId == dependencyId || x.ToDependencyId == dependencyId).ToListAsync();

            var availableAmount = incomes.Sum(x => x.Amount)
                - expenses.Sum(x => x.Amount)
                - transfers.Where(x => x.FromDependencyId == dependencyId).Sum(x => x.Amount)
                + transfers.Where(x => x.ToDependencyId == dependencyId).Sum(x => x.Amount)
                - provisions.Sum(x => x.Amount);

            return availableAmount;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetBudgetBalanceKardexReport(Guid? dependencyId = null, DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null)
        {
            var qryDependencies = _context.Dependencies.AsNoTracking();
            var qryIncomes = _context.Incomes.AsNoTracking();
            var qryTransfers = _context.BalanceTransfers.AsNoTracking();
            var qryExpenses = _context.Expenses.AsNoTracking();
            var qryExpenseOutputs = _context.ExpenseOutputs.AsNoTracking();
            var qryExpenditureProvisions = _context.ExpenditureProvisions.AsNoTracking();

            if (dependencyId.HasValue && dependencyId != Guid.Empty)
            {
                qryDependencies = qryDependencies.Where(x => x.Id == dependencyId);
                qryIncomes = qryIncomes.Where(x => x.DependencyId == dependencyId);
                qryTransfers = qryTransfers.Where(x => x.FromDependencyId == dependencyId || x.ToDependencyId == dependencyId);
                qryExpenses = qryExpenses.Where(x => x.DependencyId == dependencyId);
                qryExpenseOutputs = qryExpenseOutputs.Where(x => x.DependencyId == dependencyId);
                qryExpenditureProvisions = qryExpenditureProvisions.Where(x => x.DependencyId == dependencyId);
            }

            if (startDate.HasValue)
            {
                qryIncomes = qryIncomes.Where(x => startDate <= x.Date);
                qryTransfers = qryTransfers.Where(x => startDate <= x.Date);
                qryExpenses = qryExpenses.Where(x => startDate <= x.Date);
                qryExpenseOutputs = qryExpenseOutputs.Where(x => startDate <= x.CreatedAt);
                //qryExpenditureProvisions = qryExpenditureProvisions.Where(x => startDate <= x.Date);
            }

            if (endDate.HasValue)
            {
                qryIncomes = qryIncomes.Where(x => x.Date <= endDate);
                qryTransfers = qryTransfers.Where(x => x.Date <= endDate);
                qryExpenses = qryExpenses.Where(x => x.Date <= endDate);
                qryExpenseOutputs = qryExpenseOutputs.Where(x => x.CreatedAt <= endDate);
                //qryExpenditureProvisions = qryExpenditureProvisions.Where(x => x.Date <= endDate);
            }

            var dependenciesHash = _context.Dependencies.Select(x => x.Id).ToHashSet();

            var incomes = await qryIncomes.Include(x => x.Dependency).ToListAsync();
            var transfers = await qryTransfers.Include(x => x.FromDependency).Include(x => x.ToDependency).ToListAsync();
            var expenses = await qryExpenses.Include(x => x.Dependency).ToListAsync();
            var expenseOutputs = await qryExpenseOutputs.Include(x => x.Dependency).ToListAsync();
            var expenditureProvisions = await qryExpenditureProvisions.Include(x => x.Dependency).ToListAsync();

            if (user != null && user.IsInRole(ConstantHelpers.ROLES.INCOMES))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var accounts = await _context.UserCurrentAccounts.Where(x => x.UserId == userId).ToListAsync();
                var incomesAccount = accounts.Any(x => !x.IsExpenseAccount) ? accounts.FirstOrDefault(x => !x.IsExpenseAccount).CurrentAccountId : (Guid?)null;
                var outcomesAccount = accounts.Any(x => x.IsExpenseAccount) ? accounts.FirstOrDefault(x => x.IsExpenseAccount).CurrentAccountId : (Guid?)null;

                incomes = incomes.Where(x => x.CurrentAccountId.HasValue && (x.CurrentAccountId == incomesAccount || x.CurrentAccountId == outcomesAccount)).ToList();
            }

            var dataIncomes = incomes
                .Where(x => x.Dependency != null)
                .Select(x => new
                {
                    orderDate = x.Date,
                    date = x.Date.ToLocalDateFormat(),
                    siaf = "",
                    invoice = $"R-{x.Invoice}",
                    document = "",
                    order = "",
                    concept = x.Description,
                    unit = x.Dependency.Name,
                    unitId = x.DependencyId,
                    month = "",
                    transfer = 0.00M,
                    provision = 0.00M,
                    income = x.Amount,
                    expense = 0.00M
                }).ToList();

            var intransfers = transfers
                .Where(x => x.FromDependency != null && x.ToDependency != null && !x.IsCutTransfer)
                .Select(x => new
                {
                    orderDate = x.CreatedAt.Value,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    siaf = "",
                    invoice = "",
                    document = "",
                    order = "",
                    concept = $"Transferencia Interna (De: {x.FromDependency.Name})",
                    unit = x.ToDependency.Name,
                    unitId = x.ToDependencyId,
                    month = $"",
                    transfer = x.Amount,
                    provision = 0.00M,
                    income = 0.00M,
                    expense = 0.00M
                })
                .ToList();

            var outtransfers = transfers
                .Where(x => x.FromDependency != null && x.ToDependency != null && !x.IsCutTransfer)
                .Select(x => new
                {
                    orderDate = x.CreatedAt.Value,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    siaf = "",
                    invoice = "",
                    document = "",
                    order = "",
                    concept = $"Transferencia Interna (A: {x.ToDependency.Name})",
                    unit = x.FromDependency.Name,
                    unitId = x.FromDependencyId,
                    month = $"",
                    transfer = x.Amount * -1.0M,
                    provision = 0.00M,
                    income = 0.00M,
                    expense = 0.00M
                })
                .ToList();

            var outcuttransfers = transfers
              .Where(x => x.FromDependency != null && x.IsCutTransfer && x.CutType == ConstantHelpers.Treasury.CutType.Outcome)
              .Select(x => new
              {
                  orderDate = x.CreatedAt.Value,
                  date = x.Date.ToLocalDateFormat(),
                  siaf = "",
                  invoice = "",
                  document = x.ReferenceDocument,
                  order = x.Order,
                  concept = x.Concept,
                  unit = x.FromDependency.Name,
                  unitId = x.FromDependencyId,
                  month = $"{x.Date:MM/yyyy}",
                  transfer = 0.00M,
                  provision = 0.00M,
                  income = 0.00M,
                  expense = x.Amount
              })
              .ToList();

            var incuttransfers = transfers
              .Where(x => x.FromDependency != null && x.IsCutTransfer && x.CutType == ConstantHelpers.Treasury.CutType.Income)
              .Select(x => new
              {
                  orderDate = x.CreatedAt.Value,
                  date = x.Date.ToLocalDateFormat(),
                  siaf = "",
                  invoice = "",
                  document = x.ReferenceDocument,
                  order = x.Order,
                  concept = x.Concept,
                  unit = x.FromDependency.Name,
                  unitId = x.FromDependencyId,
                  month = $"{x.Date:MM/yyyy}",
                  transfer = 0.00M,
                  provision = 0.00M,
                  income = x.Amount,
                  expense = 0.00M
              })
              .ToList();

            var previousBalances = expenses
                .Where(x => x.Dependency != null && x.Type == ConstantHelpers.Treasury.ExpenseTypes.PreviousBalance)
                .Select(x => new
                {
                    orderDate = x.Date,
                    date = x.Date.ToLocalDateFormat(),
                    siaf = x.Code,
                    invoice = $"R-{x.Invoice}",
                    document = x.ReferenceDocument,
                    order = x.Order,
                    concept = x.Concept,
                    unit = x.Dependency.Name,
                    unitId = x.DependencyId,
                    month = $"{x.Month:MM/yyyy}",
                    transfer = 0.00M,
                    provision = 0.00M,
                    income = x.Amount,
                    expense = 0.00M
                })
                .ToList();

            var devolutions = expenses
                .Where(x => x.Dependency != null && x.Type == ConstantHelpers.Treasury.ExpenseTypes.Devolution)
                .Select(x => new
                {
                    orderDate = x.Date,
                    date = x.Date.ToLocalDateFormat(),
                    siaf = x.Code,
                    invoice = $"R-{x.Invoice}",
                    document = x.ReferenceDocument,
                    order = x.Order,
                    concept = x.Concept,
                    unit = x.Dependency.Name,
                    unitId = x.DependencyId,
                    month = $"{x.Month:MM/yyyy}",
                    transfer = 0.00M,
                    provision = 0.00M,
                    income = 0.00M,
                    expense = x.Amount
                })
                .ToList();

            var dataExpenses = expenses
                .Where(x => x.Dependency != null && x.Type == ConstantHelpers.Treasury.ExpenseTypes.Expense)
                .Select(x => new
                {
                    orderDate = x.Date,
                    date = x.Date.ToLocalDateFormat(),
                    siaf = x.Code,
                    invoice = $"G-{x.Invoice}",
                    document = x.ReferenceDocument,
                    order = x.Order,
                    concept = x.Concept,
                    unit = x.Dependency.Name,
                    unitId = x.DependencyId,
                    month = $"{x.Month:MM/yyyy}",
                    transfer = 0.00M,
                    provision = 0.00M,
                    income = 0.00M,
                    expense = x.Amount
                })
                .ToList();

            var annuled = expenses
                .Where(x => x.Dependency != null && x.Type == ConstantHelpers.Treasury.ExpenseTypes.Annulment)
                .Select(x => new
                {
                    orderDate = x.Date,
                    date = x.Date.ToLocalDateFormat(),
                    siaf = x.Code,
                    invoice = $"G-{x.Invoice}",
                    document = x.ReferenceDocument,
                    order = x.Order,
                    concept = x.Concept,
                    unit = x.Dependency.Name,
                    unitId = x.DependencyId,
                    month = $"{x.Month:MM/yyyy}",
                    transfer = 0.00M,
                    provision = 0.00M,
                    income = x.Amount,
                    expense = 0.00M
                })
                .ToList();

            var provision = expenditureProvisions
                .Where(x => x.Dependency != null && x.Status == ConstantHelpers.ExpenditureProvision.Status.PENDING)
                .Select(x => new
                {
                    orderDate = x.CreatedAt.Value,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    siaf = "",
                    invoice = "",
                    document = "",
                    order = "",
                    concept = x.Concept,
                    unit = x.Dependency.Name,
                    unitId = x.DependencyId,
                    month = "",
                    transfer = 0.00M,
                    provision = x.Amount,
                    income = 0.00M,
                    expense = 0.00M
                }).ToList();

            var dataexpenseOutputs = expenseOutputs
                .Where(x => x.Dependency != null)
                .Select(x => new
                {
                    orderDate = x.CreatedAt.Value,
                    date = x.CreatedAt.ToLocalDateFormat(),
                    siaf = "",
                    invoice = "",
                    document = "",
                    order = "",
                    concept = x.Description,
                    unit = x.Dependency.Name,
                    unitId = x.DependencyId,
                    month = "",
                    transfer = 0.00M,
                    provision = 0.00M,
                    income = 0.00M,
                    expense = x.Amount
                }).ToList();

            var data = dataIncomes;
            data.AddRange(previousBalances);
            data.AddRange(devolutions);
            data.AddRange(dataExpenses);
            data.AddRange(annuled);
            data.AddRange(provision);
            data.AddRange(dataexpenseOutputs);
            data.AddRange(intransfers);
            data.AddRange(outtransfers);
            data.AddRange(outcuttransfers);
            data.AddRange(incuttransfers);

            var filterRecords = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = 0,
                RecordsFiltered = filterRecords,
                RecordsTotal = filterRecords,
            };
        }

        public async Task<List<UserDependency>> GetByUser(string userId)
            => await _context.UserDependencies.Where(x => x.UserId == userId).ToListAsync();

        public async Task<Dependency> GetHierarchicalTree(Guid dependencyId)
        {
            var dependencies = await _context.Dependencies
                .Select(x => new Dependency
                {
                    SuperiorDependency = new Dependency(),
                    SuperiorDependencyId = x.SuperiorDependencyId,
                    Name = x.Name,
                    Id = x.Id
                }).ToListAsync();

            //1. ubicar la dependencia top
            var topDependency = dependencies.FirstOrDefault(x => x.Id == dependencyId);
            var parentId = topDependency.SuperiorDependencyId;
            while (parentId.HasValue)
            {
                topDependency = dependencies.FirstOrDefault(x => x.Id == parentId);
                parentId = topDependency.SuperiorDependencyId;
            }

            //2. comenzar a llenar a partir de ahi
            var dependency = GetHierarchicalTreeBranch(dependencies, topDependency.Id);

            return dependency;
        }

        private Dependency GetHierarchicalTreeBranch(List<Dependency> dependencies, Guid dependencyId)
        {
            var dependency = dependencies.FirstOrDefault(x => x.Id == dependencyId);
            dependency.Dependencies = new List<Dependency>();

            var childrens = dependencies.Where(x => x.SuperiorDependencyId == dependencyId).ToList();
            foreach (var item in childrens)
            {
                dependency.Dependencies.Add(GetHierarchicalTreeBranch(dependencies, item.Id));
            }

            return dependency;
        }


        #endregion
    }
}
