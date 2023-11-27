using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.VirtualDirectory;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.VirtualDirectory.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.VirtualDirectory.Implementations
{
    public class InstitutionalInformationRepository : Repository<InstitutionalInformation>, IInstitutionalInformationRepository
    {
        public InstitutionalInformationRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInstitutionalInformationDatatable(DataTablesStructs.SentParameters sentParameters, int? type = null, Guid? dependencyId = null, DateTime? publishDate = null, string search = null)
        {
            Expression<Func<InstitutionalInformation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Dependency.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Description);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Type);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.PublishDate);
                    break;
                default:
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var query = _context.InstitutionalInformations.AsQueryable();

            if (type.HasValue)
                query = query.Where(x => x.Type == type);

            if (dependencyId.HasValue && dependencyId != Guid.Empty)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (publishDate.HasValue)
                query = query.Where(x => x.PublishDate.Date == publishDate.Value.Date);

            if(!string.IsNullOrEmpty(search))
                query = query.Where(x=>x.Name.ToLower().Contains(search.ToLower().Trim()) || x.Description.ToLower().Contains(search.ToLower()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    dependency = x.Dependency.Name,
                    name = x.Name,
                    description = x.Description,
                    type = ConstantHelpers.VirtualDirectory.InstitutionalInformation.Type.VALUES.ContainsKey(x.Type) == false
                                ? "Tipo Desconocido"
                                : ConstantHelpers.VirtualDirectory.InstitutionalInformation.Type.VALUES[x.Type],
                    publishDate = x.PublishDate.ToString("dd/MM/yyyy")
                })
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
