using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.ConceptDistribution;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ConceptDistributionRepository : Repository<ConceptDistribution>, IConceptDistributionRepository
    {
        public ConceptDistributionRepository(AkdemicContext context) : base(context) { }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptDistributionDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<ConceptDistribution, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.ConceptDistributionDetails.Count);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Concepts.Count);
                    break;
                default:
                    break;
            }

            var query = _context.ConceptDistributions
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            //var payments = await _context.Payments
            //              .IgnoreQueryFilters()
            //              .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.ConceptId.HasValue)
            //              .Select(x => new
            //              {
            //                  x.Total,
            //                  x.Concept.ConceptDistributionId
            //              })
            //              .ToArrayAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    divisions = x.ConceptDistributionDetails.Count(),
                    //total = payments.Where(y => y.ConceptDistributionId == x.Id).Sum(y => y.Total),
                    //payments = payments.Count(y => y.ConceptDistributionId == x.Id) // x.Concepts.Select(y => y.Payments.Count).Count,
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptDistributionReportDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<ConceptDistribution, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.ConceptDistributionDetails.Count);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Concepts.Count);
                    break;
                default:
                    break;
            }

            var query = _context.ConceptDistributions
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var payments = await _context.Payments
                          .IgnoreQueryFilters()
                          .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.ConceptId.HasValue && x.Concept.ConceptDistributionId.HasValue)
                          .Select(x => new
                          {
                              x.Total,
                              x.Concept.ConceptDistributionId
                          })
                          .ToArrayAsync();

            var dataDB = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    divisions = x.ConceptDistributionDetails.Count()
                }).ToListAsync();

            var data = dataDB
                .Select(x => new
                {
                    x.id,
                    x.name,
                    x.divisions,
                    total = payments.Where(y => y.ConceptDistributionId == x.id).Sum(y => y.Total),
                    payments = payments.Count(y => y.ConceptDistributionId == x.id) // x.Concepts.Select(y => y.Payments.Count).Count,
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

        public override async Task DeleteById(Guid id)
        {
            var conceptDistribution = await _context.ConceptDistributions
                .Include(x => x.ConceptDistributionDetails)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            _context.ConceptDistributionDetails.RemoveRange(conceptDistribution.ConceptDistributionDetails);
            await base.DeleteById(id);
        }

        public async Task<ConceptDistribution> GetWithIncludes(Guid id)
        {
            var query = _context.ConceptDistributions
                .Include(x => x.ConceptDistributionDetails)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<ConceptDistributionReportTemplate>> GetConceptDistributionExcel(Guid id)
        {
            var payments = await _context.Payments
                .IgnoreQueryFilters()
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.Concept.ConceptDistributionId == id)
                .ToListAsync();

            var reports = await _context.Concepts
                .IgnoreQueryFilters()
                .Where(x => x.ConceptDistributionId == id)
                .Select(x => new ConceptDistributionReportTemplate
                {
                    Id = x.Id,
                    Concept = x.Description,
                    //Total = payments.Where(p => p.ConceptId == x.Id).Sum(p => p.Total),
                    ConceptDistributions = x.ConceptDistribution.ConceptDistributionDetails
                            .Select(y => new ExcelReportConceptDistribution
                            {
                                DependencyId = y.DependencyId ?? x.DependencyId,
                                Dependency = y.Dependency.Name ?? "Unidad Operativa",
                                Weight = y.Weight,
                                //Amount = payments.Where(p => p.ConceptId == x.Id).Sum(p => p.Total * y.Weight / 100.0M)
                            }).ToList()
                }).ToListAsync();

            foreach (var item in reports)
            {
                item.Total = payments.Where(p => p.ConceptId == item.Id).Sum(p => p.Total);
                item.ConceptDistributions.ForEach(x => x.Amount = payments.Where(p => p.ConceptId == item.Id).Sum(p => p.Total * x.Weight / 100.0M));
            }

            return reports;
        }

        public async Task<bool> HasPayments(Guid id)
        {
            //ConceptDistributionId = id
            var hasPayment = false;

            //Todos los conceptos asi fueran eliminados, ya que el reporte recoge la data aunque haya sido eliminada
            //Y no queremos que afecte el reporte el edit
            var concepts = await _context.Concepts
                .IgnoreQueryFilters()
                .Include(x => x.Payments)
                .Where(x => x.ConceptDistributionId == id)
                .ToListAsync();

            if (concepts.Any(y => y.Payments.Count() > 0)) hasPayment = true;

            return hasPayment;
        }
    }
}
