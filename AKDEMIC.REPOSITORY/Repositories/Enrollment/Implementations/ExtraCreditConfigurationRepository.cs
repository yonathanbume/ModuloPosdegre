using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class ExtraCreditConfigurationRepository : Repository<ExtraCreditConfiguration>, IExtraCreditConfigurationRepository
    {
        public ExtraCreditConfigurationRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetDataDatatable()
        {
            var extraCreditModality = await GetIntConfigurationValue(ConstantHelpers.Configuration.Enrollment.EXTRA_CREDITS_MODALITY);

            var query = _context.ExtraCreditConfigurations
              .AsNoTracking();

            if (extraCreditModality == ConstantHelpers.Configuration.Enrollment.ExtraCreditModality.UNBEATEN_STUDENTS)
            {
                var data = await query
                    .Where(x => x.AverageGradeStart.HasValue && x.AverageGradeEnd.HasValue)
                    .Select(x => new
                    {
                        id = x.Id,
                        name = $"{x.AverageGradeStart} - {x.AverageGradeEnd}",
                        credits = x.Credits,
                        start = x.AverageGradeStart,
                        end = x.AverageGradeEnd,
                        type = x.MeritType
                    })
                    .ToListAsync();

                var recordsTotal = data.Count;

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    //DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsTotal,
                    RecordsTotal = recordsTotal
                };
            }
            else
            {
                var data = await query
                    .Where(x => x.MeritType.HasValue)
                    .Select(x => new
                    {
                        id = x.Id,
                        name = ConstantHelpers.ACADEMIC_ORDER.VALUES[x.MeritType.Value],
                        credits = x.Credits,
                        start = x.AverageGradeStart,
                        end = x.AverageGradeEnd,
                        type = x.MeritType
                    })
                    .ToListAsync();

                var recordsTotal = data.Count;

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    //DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsTotal,
                    RecordsTotal = recordsTotal
                };
            }
        }
    }
}
