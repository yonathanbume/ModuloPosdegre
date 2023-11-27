using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class AgreementFormatRepository: Repository<AgreementFormat> , IAgreementFormatRepository
    {
        public AgreementFormatRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatByCompanyDatatable(DataTablesStructs.SentParameters sentParameters, Guid companyId, string searchValue = null)
        {
            Expression<Func<AgreementFormat, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Title); break;
                case "1":
                    orderByPredicate = ((x) => x.Observations); break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var query = _context.AgreementFormats
                .Where(x => x.CompanyId == companyId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Title.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Observations.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    x.State,
                    StateText = ConstantHelpers.AGREEMENTFORMAT.STATES.VALUES.ContainsKey(x.State) ? ConstantHelpers.AGREEMENTFORMAT.STATES.VALUES[x.State] : "",
                    x.FilePath,
                    x.Observations
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatDatatable(DataTablesStructs.SentParameters sentParameters, int? state = null, string searchValue = null)
        {
            Expression<Func<AgreementFormat, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Title); break;
                case "1":
                    orderByPredicate = ((x) => x.Observations); break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var query = _context.AgreementFormats.AsNoTracking();

            if (state != null)
            {
                query = query.Where(x => x.State == state.Value);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Title.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Observations.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    CreatedAt = x.CreatedAt == null ? "" : x.CreatedAt.ToLocalDateTimeFormat(),
                    Company = x.Company == null ? "" : x.Company.User.FullName,
                    ApprovedAt = x.ApprovedAt == null ? "" : x.ApprovedAt.ToLocalDateTimeFormat(),
                    x.State,
                    StateText = ConstantHelpers.AGREEMENTFORMAT.STATES.VALUES.ContainsKey(x.State) ? ConstantHelpers.AGREEMENTFORMAT.STATES.VALUES[x.State] : "",
                    x.FilePath                    
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllAgreementFormatNotAcceptedDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<AgreementFormat, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Title); break;
                case "1":
                    orderByPredicate = ((x) => x.Observations); break;
                case "2":
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var query = _context.AgreementFormats
                .Where(x => x.State == ConstantHelpers.AGREEMENTFORMAT.STATES.PENDING || x.State == ConstantHelpers.AGREEMENTFORMAT.STATES.OBSERVATED)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Title.ToUpper().Contains(searchValue.ToUpper()) ||
                                x.Observations.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    x.State,
                    StateText = ConstantHelpers.AGREEMENTFORMAT.STATES.VALUES.ContainsKey(x.State) ? ConstantHelpers.AGREEMENTFORMAT.STATES.VALUES[x.State] : "",
                    x.FilePath,
                    CompanyName = x.Company.User.Name ?? "",
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

        public async Task<object> GetById(Guid id)
        {
            var result = await _context.AgreementFormats
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    x.FilePath,
                    x.Observations,
                    x.State,
                    StateText = ConstantHelpers.AGREEMENTFORMAT.STATES.VALUES.ContainsKey(x.State) ? ConstantHelpers.AGREEMENTFORMAT.STATES.VALUES[x.State]: "",
                    CompanyName = x.Company.User.Name ?? "",
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
