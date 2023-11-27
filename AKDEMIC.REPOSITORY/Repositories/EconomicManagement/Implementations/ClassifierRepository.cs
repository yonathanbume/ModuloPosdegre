using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Helpers;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Classifier;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ClassifierRepository : Repository<Classifier>, IClassifierRepository
    {
        public ClassifierRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<DataTablesStructs.ReturnedData<Classifier>> GetClassifiersDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<Classifier, Classifier>> selectPredicate = null, Expression<Func<Classifier, dynamic>> orderByPredicate = null, Func<Classifier, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Classifiers.AsNoTracking();
            var recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(selectPredicate, searchValue)
                .ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<Classifier>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        private async Task<Select2Structs.ResponseParameters> GetClassifiersSelect2(Select2Structs.RequestParameters requestParameters, Expression<Func<Classifier, Select2Structs.Result>> selectPredicate = null, Func<Classifier, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.Classifiers.AsNoTracking();
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

        public async Task<IEnumerable<Classifier>> GetClassifiers()
        {
            var query = _context.Classifiers.SelectClassifier();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<Classifier>> GetClassifiersDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Classifier, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);

                    break;
                case "1":
                    orderByPredicate = ((x) => x.Code);

                    break;
                case "2":
                    orderByPredicate = ((x) => x.Description);

                    break;
                default:
                    orderByPredicate = ((x) => x.Name);

                    break;
            }

            return await GetClassifiersDatatable(sentParameters, ExpressionHelpers.SelectClassifier(), orderByPredicate, (x) => new[] { x.Code, x.Description, x.Name }, searchValue);
        }

        public async Task<Select2Structs.ResponseParameters> GetClassifiersSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            return await GetClassifiersSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            }, (x) => new[] { x.Name }, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassifiersDatatableEconomic(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<Classifier, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CreatedAt.ToLocalDateTimeFormat();
                    break;
                default:
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }


            var query = _context.Classifiers
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Code.ToUpper().Contains(search.ToUpper()) || x.Description.ToUpper().Contains(search.ToUpper()));

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    description = x.Description,
                    date = x.CreatedAt.ToLocalDateTimeFormat()
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

        public async Task<object> GetClassifiersEconomicBy(Guid id)
        {
            var query = _context.Classifiers.AsQueryable();

            if (id != Guid.Empty)
            {
                query = query.Where(x => x.Id != id);
            }

            var data = await query.Select(x => new
            {
                id = x.Id,
                text = x.Code
            }).OrderBy(x => x.text).ToListAsync();

            data.Insert(0, new { id = new Guid(), text = "---" });

            return data;
        }

        public async Task<object> GetClassifiersEdit(Guid id)
        {
            var model = await _context.Classifiers
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    name = x.Name,
                    description = x.Description,
                    paternId = x.ParentId == null ? Guid.Empty : x.ParentId,
                }).FirstOrDefaultAsync();

            return model;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetClassifiersDatatableReport(DataTablesStructs.SentParameters sentParameters, string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null, bool showAll = false)
        {
            Expression<Func<Classifier, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    //orderByPredicate = (x) => x.Concepts.Sum(c => c.Payments.Sum(p => p.Total));
                    break;
                default:
                    orderByPredicate = (x) => x.Code;
                    break;
            }

            var qryPayments = _context.Payments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.ConceptId.HasValue)
                .AsNoTracking();

            if (startDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate >= startDate);
            if (endDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate <= endDate);

            if (type.HasValue && type != 0) 
            {
                switch (type)
                {
                    case 1: qryPayments = qryPayments.Where(x => x.IsBankPayment || (x.InvoiceId.HasValue && x.Invoice.PaymentType != ConstantHelpers.Treasury.Invoice.PaymentType.CASH));break;
                    case 2: qryPayments = qryPayments.Where(x => x.InvoiceId.HasValue && x.Invoice.PaymentType == ConstantHelpers.Treasury.Invoice.PaymentType.CASH);break;
                    default:
                        break;
                }
            }

            var payments = await qryPayments
                .Select(x => new
                {
                    x.ConceptId,
                    x.Total
                })
                .ToListAsync();

            var conceptPayments = payments
                .GroupBy(x => x.ConceptId)
                .Select(x => new
                {
                    ConceptId = x.Key,
                    Total = x.Sum(y => y.Total)
                }).ToList();

            var query = _context.Classifiers
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Code.Contains(search) || x.Name.Contains(search));

            var classifiers = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                //.Skip(sentParameters.PagingFirstRecord)
                //.Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    Concepts = x.Concepts.Select(y => y.Id).ToList()
                }).ToListAsync();

            var data = classifiers
                .Select(x => new
                {
                    id = x.Id,
                    account = x.Code,
                    name = x.Name,
                    searchStartDate = startDate == null ? "" : startDate.ToLocalDateFormat(),
                    searchEndDate = endDate == null ? "" : endDate.ToLocalDateFormat(),
                    //totalAmount = x.Concepts.Sum(c => payments.Where(p => p.ConceptId == c.Id).Sum(p => p.Total)) //_context.Payments.Where(p => x.Concepts.Any(c => c.Id == p.EntityId)).Sum(p => p.Total)
                    totalAmount = conceptPayments.Where(y => x.Concepts.Contains(y.ConceptId.Value)).Sum(y => y.Total)
                }).ToList();

            if (sentParameters.OrderColumn == "2")
            {
                if (sentParameters.OrderColumn == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION)
                    data = data.OrderByDescending(x => x.totalAmount).ToList();
                else
                    data = data.OrderBy(x => x.totalAmount).ToList();
            }

            if (!showAll) data = data.Where(x => x.totalAmount > 0).ToList();

            var recordsTotal = data.Count;

            data = data
                 //.Skip(sentParameters.PagingFirstRecord)
                 //.Take(sentParameters.RecordsPerDraw)
                 .ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<decimal> GetClassifiersDatatableReportTotalAmount(string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null)
        {
            var qryPayments = _context.Payments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.ConceptId.HasValue)
                .AsNoTracking();

            if (startDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate >= startDate);
            if (endDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate <= endDate);

            if (type.HasValue && type != 0)
            {
                switch (type)
                {
                    case 1: qryPayments = qryPayments.Where(x => x.IsBankPayment || (x.InvoiceId.HasValue && x.Invoice.PaymentType != ConstantHelpers.Treasury.Invoice.PaymentType.CASH)); break;
                    case 2: qryPayments = qryPayments.Where(x => x.InvoiceId.HasValue && x.Invoice.PaymentType == ConstantHelpers.Treasury.Invoice.PaymentType.CASH); break;
                    default:
                        break;
                }
            }

            var payments = await qryPayments
                .Select(x => new
                {
                    x.ConceptId,
                    x.Total
                })
                .ToListAsync();

            var conceptPayments = payments
                .GroupBy(x => x.ConceptId)
                .Select(x => new
                {
                    ConceptId = x.Key,
                    Total = x.Sum(y => y.Total)
                }).ToList();

            var query = _context.Classifiers
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Code.Contains(search) || x.Name.Contains(search));

            var classifiers = await query
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    Concepts = x.Concepts.Select(y => y.Id).ToList()
                }).ToListAsync();

            var data = classifiers
                .Select(x => new
                {
                    id = x.Id,
                    totalAmount = conceptPayments.Where(y => x.Concepts.Contains(y.ConceptId.Value)).Sum(y => y.Total)
                }).ToList();

            return data.Sum(x => x.totalAmount);
        }

        public async Task<List<ClassifierTemplate>> GetClassifiersReportData(string search, DateTime? startDate = null, DateTime? endDate = null, int? type = null, bool showAll = false)
        {
            var qryPayments = _context.Payments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.ConceptId.HasValue)
                .AsNoTracking();

            if (startDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate >= startDate);
            if (endDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate <= endDate);

            if (type.HasValue && type != 0)
            {
                switch (type)
                {
                    case 1: qryPayments = qryPayments.Where(x => x.IsBankPayment || (x.InvoiceId.HasValue && x.Invoice.PaymentType != ConstantHelpers.Treasury.Invoice.PaymentType.CASH)); break;
                    case 2: qryPayments = qryPayments.Where(x => x.InvoiceId.HasValue && x.Invoice.PaymentType == ConstantHelpers.Treasury.Invoice.PaymentType.CASH); break;
                    default:
                        break;
                }
            }

            var payments = await qryPayments
                .Select(x => new
                {
                    x.ConceptId,
                    x.Total
                })
                .ToListAsync();

            var conceptPayments = payments
                .GroupBy(x => x.ConceptId)
                .Select(x => new
                {
                    ConceptId = x.Key,
                    Total = x.Sum(y => y.Total)
                }).ToList();

            var query = _context.Classifiers
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search)) query = query.Where(x => x.Code.Contains(search) || x.Name.Contains(search));

            var classifiers = await query
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.ParentId,
                    Concepts = x.Concepts.Select(y => y.Id).ToList()
                }).ToListAsync();

            var data = classifiers
                .Select(x => new ClassifierTemplate
                {
                    Id = x.Id,
                    ParentId = x.ParentId,
                    Code = x.Code,
                    Name = x.Name,
                    Count = conceptPayments.Where(y => x.Concepts.Contains(y.ConceptId.Value)).Count(),
                    Total = conceptPayments.Where(y => x.Concepts.Contains(y.ConceptId.Value)).Sum(y => y.Total)
                }).ToList();

            if (!showAll) data = data.Where(x => x.Total > 0).ToList();

            return data;
        }

        public async Task<object> GetAreaOrCareerJson()
        {
            var data = await _context.Classifiers
                .Select(c => new
                {
                    id = c.Id,
                    text = $"{c.Code} - {c.Name}"
                }).ToListAsync();

            var result = new
            {
                items = data
            };

            return result;
        }

        public async Task<Classifier> GetByCode(string code)
            => await _context.Classifiers.Where(x => x.Code == code).FirstOrDefaultAsync();
        public async Task UpdateClassifierRelationsJob()
        {
            var classifiers = await _context.Classifiers
                .OrderBy(x => x.Code)
                .ToListAsync();

            foreach (var classifier in classifiers)
            {
                var classifierCode = classifier.Code;
                var classifierCodeLastSeparator = classifierCode.LastIndexOf(".");

                if (classifierCodeLastSeparator > 0)
                {
                    var parentCode = classifierCode.Substring(0, classifierCodeLastSeparator);
                    var parentClassifier = classifiers
                        .Where(x => x.Code == parentCode)
                        .FirstOrDefault();

                    if (parentClassifier != null)
                    {
                        classifier.ParentId = parentClassifier.Id;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptsByClassifierDatatable(DataTablesStructs.SentParameters sentParameters, Guid classifierId, string search)
        {
            var query = _context.Concepts.Where(x => x.ClassifierId == classifierId).AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Code.ToLower().Trim() == search.Trim().ToLower());

            var recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Amount,
                    x.Description
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

        public async Task<List<PaymentTemplate>> GetPaymentsByClassifierId(Guid classifierId, DateTime? startDate = null, DateTime? endDate = null, int? type = null)
        {
            var qryPayments = _context.Payments
                    .AsNoTracking();

            if (startDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate >= startDate);
            if (endDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate <= endDate);

            if (type.HasValue && type != 0)
            {
                switch (type)
                {
                    case 1: qryPayments = qryPayments.Where(x => x.IsBankPayment || (x.InvoiceId.HasValue && x.Invoice.PaymentType != ConstantHelpers.Treasury.Invoice.PaymentType.CASH)); break;
                    case 2: qryPayments = qryPayments.Where(x => x.InvoiceId.HasValue && x.Invoice.PaymentType == ConstantHelpers.Treasury.Invoice.PaymentType.CASH); break;
                    default:
                        break;
                }
            }

            var payments = await qryPayments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.ConceptId.HasValue && x.Concept.ClassifierId == classifierId)
                .Select(x => new PaymentTemplate
                {
                    User = string.IsNullOrEmpty(x.UserId) ? x.ExternalUser.FullName : x.User.FullName,
                    Description = x.Description,
                    IssueDate = x.IssueDate.ToLocalDateTimeFormat(),
                    PaymentDate = x.PaymentDate.ToLocalDateTimeFormat(),
                    Quantity = x.Quantity,
                    TotalAmount = x.Total,
                    CreatedBy = x.CreatedBy,
                    PaymentType = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION.ContainsKey(x.Type) ? ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.Type] : "-",
                    Serie = x.Invoice.Series,
                    Number = x.Invoice.Number
                })
                .ToListAsync();

            return payments;
        }
        #endregion
    }
}
