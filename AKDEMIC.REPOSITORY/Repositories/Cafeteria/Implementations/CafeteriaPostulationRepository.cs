using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Cafeteria;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Cafeteria.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Cafeteria.Implementations
{
    public class CafeteriaPostulationRepository : Repository<CafeteriaPostulation>, ICafeteriaPostulationRepository
    {
        public CafeteriaPostulationRepository(AkdemicContext context) : base(context) { }
        public async Task<CafeteriaPostulation> FirstOrDefaultById(Guid id)
        {
            var result = await _context.CafeteriaPostulations.Where(x => x.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllStudentsByIdDatatable(DataTablesStructs.SentParameters sentParameters, Guid serviceTermId,string searchValue = null)
        {
            Expression<Func<CafeteriaPostulation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Student.User.UserName);
                    break;
                case "1":
                    orderByPredicate = (x) => x.Student.User.FullName;
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Student.Career.Name);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Status);
                    break;

            }

            var query = _context.CafeteriaPostulations.AsNoTracking();

            query = query.Where(x => x.CafeteriaServiceTermId == serviceTermId);
            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchTrim = searchValue.Trim();
                query = query.Where(x => x.Student.User.UserName.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.PaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.MaternalSurname.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.Name.ToUpper().Contains(searchTrim.ToUpper()) ||
                            x.Student.User.FullName.ToUpper().Contains(searchTrim.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Student.User.UserName,
                    x.Student.User.FullName,
                    CareerName = x.Student.Career.Name,
                    x.Status,
                    StatusText = ConstantHelpers.CAFETERIA_POSTULATION.VALUES.ContainsKey(x.Status) ?
                        ConstantHelpers.CAFETERIA_POSTULATION.VALUES[x.Status] : "",
                    x.Student.CurrentAcademicYear,
                    x.Student.User.PhoneNumber,
                    x.Student.User.Email,
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

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
