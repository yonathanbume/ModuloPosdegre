using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class MenuPlateRepository : Repository<MenuPlate>, IMenuPlateRepository
    {
        public MenuPlateRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task AddSupply(MenuPlateSupply menuPlateSupply)
        {
            //var exist = await _context.MenuPlateSupplies
            //                            .FirstOrDefaultAsync(x => x.MenuPlateId == menuPlateSupply.MenuPlateId &&
            //                                           x.SupplyId == menuPlateSupply.SupplyId);
            //if (exist != null)
            //    exist.Quantity += menuPlateSupply.Quantity;
            //else
            //    await _context.MenuPlateSupplies.AddAsync(menuPlateSupply);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupplyById(Guid supplyId)
        {
            var supply = await _context.MenuPlateSupplies.FirstOrDefaultAsync(x => x.Id == supplyId);
            _context.Remove(supply);
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<MenuPlate>> GetMenuDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue)
        {
            Expression<Func<MenuPlate, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                default:
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.MenuPlates
                            .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<MenuPlate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<MenuPlateSupply>> GetMenuSuppliesDatatable(DataTablesStructs.SentParameters sentParameters, Guid id, string searchValue)
        {
            Expression<Func<MenuPlateSupply, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.ProviderSupply.Supply.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Quantity;
                    break;
                default:
                    orderByPredicate = (x) => x.ProviderSupply.Supply.Name;
                    break;
            }

            var query = _context.MenuPlateSupplies
                            //.Include(x => x.Supply.UnitMeasurement)
                            .Where(x => x.MenuPlateId == id)
                            .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.ProviderSupply.Supply.Name.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<MenuPlateSupply>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<Guid> InsertAndReturnId(MenuPlate menu)
        {
            await _context.MenuPlates.AddAsync(menu);
            await _context.SaveChangesAsync();
            return menu.Id;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetMenusSelect2ClientSide(Guid? menuId)
        {
            var result = await _context.MenuPlates
                            .Select(x => new Select2Structs.Result
                            {
                                Id = x.Id,
                                Text = x.Name,
                            })
                            .ToArrayAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetMenuPlatesDatatable(DataTablesStructs.SentParameters sentParameters, Guid MenuPlateId, string searchValue)
        {

            var query = _context.MenuPlateSupplies.Include(x=>x.ProviderSupply.Supply).Where(x=>x.MenuPlateId == MenuPlateId).AsNoTracking();      

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x =>
                x.ProviderSupply.Supply.Name.Contains(searchValue)); 
            }

            Expression<Func<MenuPlateSupply, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                id = x.Id,
                x.ProviderSupply.Supply.Name,
                x.Quantity
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<Tuple<bool, string>> AddMenuPlateSupply(Guid MenuPlateId, Guid ProviderSupplyId, Guid CafeteriaScheduleId, decimal Quantity, byte TurnId)
        {
            if (Quantity == 0)
            {
                return new Tuple<bool, string>(false, "La cantidad ingresada tiene que ser mayor a cero");
            }

            var stockQuantitySum = await _context.CafeteriaStocks.Where(x => x.TurnId == TurnId && x.ProviderSupplyId == ProviderSupplyId).SumAsync(x => x.Quantity);

            if (stockQuantitySum == 0)
            {
                var menuPlateSupplyEntity = new MenuPlateSupply()
                {
                    MenuPlateId = MenuPlateId,
                    ProviderSupplyId = ProviderSupplyId,
                    Quantity = Quantity,
                    TurnId = TurnId
                };
                await _context.MenuPlateSupplies.AddAsync(menuPlateSupplyEntity);
                await _context.SaveChangesAsync();
                return new Tuple<bool, string>(true, "Se agregó correctamente");
            }

            if ( Quantity <= stockQuantitySum)
            {        
                if (await _context.CafeteriaWeeklyScheduleTurnDetails.Where(x=>x.CafeteriaWeeklyScheduleId == CafeteriaScheduleId).AnyAsync(x=>x.MenuPlate.menuPlateSupplies.Any(s=>s.ProviderSupplyId == ProviderSupplyId && s.TurnId == TurnId)))
                {
                    return new Tuple<bool, string>(false, "Ya existe el insumo en el detalle del menú");
                }
                else
                {
                    var menuPlateSupplyEntity = new MenuPlateSupply()
                    {
                        MenuPlateId = MenuPlateId,
                        ProviderSupplyId = ProviderSupplyId,
                        Quantity = Quantity,
                        TurnId = TurnId
                    };
                    await _context.MenuPlateSupplies.AddAsync(menuPlateSupplyEntity);
                    await _context.SaveChangesAsync();
                    return new Tuple<bool, string>(true, "Se agregó correctamente");
                }

                
            }
            else
            {
                return new Tuple<bool, string>(false, "La cantidad del insumo a agregar sobrepasa el límite del stock en el turno : " + CORE.Helpers.ConstantHelpers.TURN_TYPE.VALUES[TurnId]);
            }
            
        }

        public async Task DeleteMenuSupplyById(Guid menuSupplyId)
        {
            var entity = await _context.MenuPlateSupplies.Where(x => x.Id == menuSupplyId).FirstOrDefaultAsync();
            _context.MenuPlateSupplies.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
