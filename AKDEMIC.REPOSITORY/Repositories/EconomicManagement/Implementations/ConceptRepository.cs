using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Classifier;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Concept;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class ConceptRepository : Repository<Concept>, IConceptRepository
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public ConceptRepository(AkdemicContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<Concept>> GetConceptsWithUser() => await _context.Concepts.Include(x => x.Classifier).ToListAsync();

        public async Task<object> GetConceptsJson(string term)
        {
            var qry = _context.Concepts.AsQueryable();

            if (term != null) qry = qry.Where(x => x.Code.ToUpper().Contains(term.ToUpper()) || x.Description.ToUpper().Contains(term.ToUpper()));

            var concepts = await qry
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Description)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Code}-{x.Description}",
                    price = x.Amount,
                    accountingplan = x.AccountingPlan.Code,
                    isTaxed = x.IsTaxed,
                    isFixed = x.IsFixedAmount
                }).ToListAsync();

            return concepts;
        }

        public async Task<object> GetConceptsWithAccountingPlanJson(string term, ClaimsPrincipal user = null)
        {
            var qry = _context.Concepts.Where(x => x.IsEnabled).AsNoTracking();

            if (user != null && user.IsInRole(ConstantHelpers.ROLES.CASHIER))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var cashierDependencies = _context.CashierDependencies
                        .Where(x => x.UserId == userId)
                        .Select(x => x.DependencyId)
                        .ToHashSet();

                    qry = qry.Where(x => cashierDependencies.Contains(x.DependencyId));
                }
            }

            Expression<Func<Concept, dynamic>> searchFilter = (x) => new
            {
                x.Description,
                AccountingPlan = x.AccountingPlan.Code
            };

            var data = await qry
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Description,
                    x.Amount,
                    AccountingPlan = x.AccountingPlan.Code,
                    x.IsTaxed,
                    x.IsFixedAmount,
                    x.CurrentAccountId,
                    x.DependencyId
                },
                term, searchFilter).ToListAsync();

            var concepts = data
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.AccountingPlan} - {x.Description}",
                    price = x.Amount,
                    accountingplan = x.AccountingPlan,
                    isTaxed = x.IsTaxed,
                    isFixed = x.IsFixedAmount,
                    accountId = x.CurrentAccountId,
                    dependencyId = x.DependencyId
                }).OrderBy(x => x.text).ToList();

            return concepts;
        }

        public async Task<object> GetExtemporaneousEnrollmentDatatable(ClaimsPrincipal user = null)
        {
            var students = new HashSet<string>();

            var query = _context.Students.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));

                    students = query.Select(x => x.UserId).ToHashSet();
                }
            }

            var conceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXTEMPORANEOUS_ENROLLMENT_SURCHARGE_PROCEDURE));

            var enrollmentConcepts = await _context.EnrollmentConcepts
                .Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.EXTEMPORANEOUS_ENROLLMENT_CONCEPT)
                .Select(x => x.ConceptId).ToListAsync();
            enrollmentConcepts.Add(conceptId);
            var hash = enrollmentConcepts.ToHashSet();

            var terms = await _context.Terms
                .Select(x => new
                {
                    x.Name,
                    x.StartDate,
                    x.EndDate
                }).ToListAsync();

            var queryPayments = _context.Payments
                .Where(x => x.ConceptId.HasValue && hash.Contains(x.ConceptId.Value) && x.Status == CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PAID && x.PaymentDate.HasValue)
                .AsNoTracking();

            if (students.Any()) queryPayments = queryPayments.Where(x => students.Contains(x.UserId));

            var result = await queryPayments
            .Select(x => new
            {
                id = x.Id,
                name = x.User.FullName,
                code = x.User.UserName,
                term = x.TermId.HasValue ? x.Term.Name : "-",
                date = x.PaymentDate.ToLocalDateFormat()
            })
            .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExtemporaneousEnrollmentDatatableServerSide(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.PaymentDate); break;
                case "1":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "3":
                    orderByPredicate = ((x) => x.PaymentDate); break;
                default:
                    orderByPredicate = ((x) => x.PaymentDate); break;
            }

            var students = new HashSet<string>();

            var query = _context.Students.AsNoTracking();

            if (user != null && (user.IsInRole(ConstantHelpers.ROLES.CAREER_DIRECTOR) || user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_COORDINATOR)))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var qryCareers = _context.Careers
                        .Where(x => x.AcademicCoordinatorId == userId || x.CareerDirectorId == userId)
                        .AsNoTracking();

                    var careers = qryCareers.Select(x => x.Id).ToHashSet();

                    query = query.Where(x => careers.Contains(x.CareerId));

                    students = query.Select(x => x.UserId).ToHashSet();
                }
            }

            var conceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXTEMPORANEOUS_ENROLLMENT_SURCHARGE_PROCEDURE));

            var enrollmentConcepts = await _context.EnrollmentConcepts
                .Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.EXTEMPORANEOUS_ENROLLMENT_CONCEPT)
                .Select(x => x.ConceptId).ToListAsync();
            enrollmentConcepts.Add(conceptId);
            var hash = enrollmentConcepts.ToHashSet();

            var terms = await _context.Terms
                .Select(x => new
                {
                    x.Name,
                    x.StartDate,
                    x.EndDate
                }).ToListAsync();

            var queryPayments = _context.Payments
                .Where(x => x.ConceptId.HasValue && hash.Contains(x.ConceptId.Value) && x.Status == CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PAID && x.PaymentDate.HasValue)
                .AsNoTracking();

            if (students.Any()) queryPayments = queryPayments.Where(x => students.Contains(x.UserId));

            int recordsFiltered = await queryPayments.CountAsync();

            var bdData = await queryPayments
                //.OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.User.FullName,
                    code = x.User.UserName,
                    issueDate = x.IssueDate,
                    //term = terms.Where(t => t.StartDate < x.IssueDate && x.IssueDate < t.EndDate).Select(t => t.Name).FirstOrDefault(),
                    date = x.PaymentDate.ToLocalDateFormat()
                })
                .ToListAsync();

            var data = bdData
                .Select(x => new
                {
                    x.id,
                    x.name,
                    x.code,
                    term = terms.Where(t => t.StartDate < x.issueDate && x.issueDate < t.EndDate).Select(t => t.Name).FirstOrDefault(),
                    x.date
                }).ToList();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<Select2Structs.ResponseParameters> GetConceptSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            IQueryable<Concept> query = _context.Concepts
                .WhereUserFullText(searchValue)
                .AsNoTracking();

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = $"{x.Code} - {x.Description} - {x.Amount}"
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }
        public async Task<Concept> GetConceptIncludeLoadByConcept(string concept)
        {
            var result = await _context.Concepts.Include(x => x.Classifier).FirstOrDefaultAsync(x => x.BankCode == concept);
            return result;
        }

        public async Task<List<Concept>> GetConceptListByDependecyId(Guid id)
        {
            var concepts = await _context.Concepts
                .Where(x => x.DependencyId == id)
                .ToListAsync();

            return concepts;
        }

        public async Task<List<Concept>> GetConceptsByClassifierId(Guid id)
        {
            var concepts = await _context.Concepts
                .Where(x => x.ClassifierId == id)
                .ToListAsync();

            return concepts;
        }

        public IQueryable<Concept> ConceptsQry()
            => _context.Concepts.AsQueryable();

        public async Task<DataTablesStructs.ReturnedData<object>> GetPurchaseOrderDatatable(DataTablesStructs.SentParameters sentParameters, Guid? dependencyId = null)
        {
            Expression<Func<Concept, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;

                default:
                    orderByPredicate = (x) => x.Dependency.Name;
                    break;
            }


            var query = _context.Concepts
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (dependencyId.HasValue && dependencyId != Guid.Empty)
            {
                query = query.Where(x => x.DependencyId == dependencyId);
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Description,
                    dependency = x.Dependency.Name
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

        public async Task<decimal> GetConceptoByDocument(Guid? id = null)
        {
            var result = await _context.Concepts.Where(y => y.Id == (id)).Select(y => y.Amount).FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConectpsDatatable(DataTablesStructs.SentParameters sentParameters, string userId, string search)
        {
            Expression<Func<Concept, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.AccountingPlan.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Amount;
                    break;
                default:
                    //orderByPredicate = (x) => x.Classifier.Code;
                    break;
            }

            var cashierDependenciesId = await _context.CashierDependencies
                .Where(x => x.UserId == userId)
                .Select(x => x.DependencyId)
                .ToListAsync();

            var query = _context.Concepts
                  .Where(x => x.IsEnabled && cashierDependenciesId.Contains(x.DependencyId))
                  .AsNoTracking();

            //if (!string.IsNullOrEmpty(search)) 
            //    query = query.Where(x => x.Description.ToUpper().Contains(search.ToUpper()) || x.AccountingPlan.Code.Contains(search));

            //var recordsFiltered = await query.CountAsync();

            var recordsFiltered = await query
                .Select(x => new
                {
                    id = x.Id,
                    accounting = x.AccountingPlan.Code,
                    description = x.Description,
                    amount = x.Amount,
                    isTaxed = x.IsTaxed,
                    isFixed = x.IsFixedAmount
                }, search).CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    accounting = x.AccountingPlan.Code,
                    description = x.Description,
                    amount = x.Amount,
                    isTaxed = x.IsTaxed,
                    isFixed = x.IsFixedAmount,
                    accountId = x.CurrentAccountId
                }, search)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null)
        {
            Expression<Func<Concept, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.AccountingPlan.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Amount;
                    break;
                default:
                    //orderByPredicate = (x) => x.Classifier.Code;
                    break;
            }

            //var course_withdrwal_concept_auto = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT_AUTO));
            //var cycle_withdrwal_concept_auto = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT_AUTO));
            //var substitute_exam_concept_auto = bool.Parse(await GetConfigurationValue (ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT_AUTO));

            //var course_withdrwal_concept = Guid.Empty;
            //var cycle_withdrwal_concept = Guid.Empty;
            //var substitute_exam_concept = Guid.Empty;

            //Guid.TryParse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT), out course_withdrwal_concept);
            //Guid.TryParse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT), out cycle_withdrwal_concept);
            //Guid.TryParse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT), out substitute_exam_concept);

            var configs = await _context.Configurations.ToDictionaryAsync(x => x.Key, x => x.Value);

            var course_withdrwal_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT_AUTO));
            var cycle_withdrwal_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT_AUTO));
            var substitute_exam_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT_AUTO));

            var course_withdrwal_concept = Guid.Empty;
            var cycle_withdrwal_concept = Guid.Empty;
            var substitute_exam_concept = Guid.Empty;
            Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT), out course_withdrwal_concept);
            Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT), out cycle_withdrwal_concept);
            Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT), out substitute_exam_concept);

            var query = _context.Concepts
                .AsNoTracking();

            //if (course_withdrwal_concept != Guid.Empty)
            //    query = query.Where(x => x.Id == course_withdrwal_concept);

            //if (cycle_withdrwal_concept != Guid.Empty)
            //    query = query.Where(x => x.Id == cycle_withdrwal_concept);

            //if (substitute_exam_concept != Guid.Empty)
            //    query = query.Where(x => x.Id == substitute_exam_concept);

            if (user != null && !user.IsInRole(ConstantHelpers.ROLES.TREASURY))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var cashierDependenciesId = await _context.CashierDependencies
                    .Where(x => x.UserId == userId)
                    .Select(x => x.DependencyId)
                    .ToListAsync();

                query = query.Where(x => (x.IsEnabled && cashierDependenciesId.Contains(x.DependencyId))
                || x.Id == course_withdrwal_concept || x.Id == cycle_withdrwal_concept || x.Id == substitute_exam_concept);
            }
            else
                query = query.Where(x => x.IsEnabled || x.Id == course_withdrwal_concept || x.Id == cycle_withdrwal_concept || x.Id == substitute_exam_concept);

            var recordsFiltered = await query.Select(x => new
            {
                id = x.Id,
                classifier = x.AccountingPlan.Code,
                code = x.Code,
                description = x.Description,
                amount = x.Amount,
                isAutomatic =
                    (
                        x.Id == course_withdrwal_concept ? course_withdrwal_concept_auto :
                        x.Id == cycle_withdrwal_concept ? cycle_withdrwal_concept_auto :
                        x.Id == substitute_exam_concept ? substitute_exam_concept_auto :
                        false
                    ),
                requestType =
                    (
                     x.Id == course_withdrwal_concept ? 1 :
                     x.Id == cycle_withdrwal_concept ? 2 :
                     x.Id == substitute_exam_concept ? 3 :
                     0
                    )
            }, search).CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    classifier = x.AccountingPlan.Code,
                    code = x.Code,
                    description = x.Description,
                    amount = x.Amount,
                    isAutomatic =
                    (
                        x.Id == course_withdrwal_concept ? course_withdrwal_concept_auto :
                        x.Id == cycle_withdrwal_concept ? cycle_withdrwal_concept_auto :
                        x.Id == substitute_exam_concept ? substitute_exam_concept_auto :
                        false
                    ),
                    requestType =
                    (
                     x.Id == course_withdrwal_concept ? 1 :
                     x.Id == cycle_withdrwal_concept ? 2 :
                     x.Id == substitute_exam_concept ? 3 :
                     0
                    )
                }, search)
                 .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<object> GetUserConcepts(string userId, string term)
        {
            var cashierDependenciesId = await _context.CashierDependencies
                .Where(x => x.UserId == userId).Select(x => x.DependencyId)
                .ToListAsync();

            var concepts = await _context.Concepts
                .Where(x => cashierDependenciesId.Contains(x.DependencyId) && x.Description.Contains(term))
               .Select(x => new
               {
                   id = x.Id,
                   text = $"{x.Classifier.Code} - {x.Description}"
               }).ToListAsync();

            var result = new
            {
                items = concepts
            };

            return result;
        }

        public async Task<decimal> GetUnitaryAmount(Guid id)
         => await _context.Concepts.Where(y => y.Id == (id)).Select(y => y.Amount).FirstOrDefaultAsync();

        public async Task<Concept> GetWithIncludes(Guid id)
        {
            var concept = await _context.Concepts
                .IgnoreQueryFilters()
                .Where(x => x.Id == id && !x.DeletedAt.HasValue)
                .Include(x => x.ConceptDistribution)
                .ThenInclude(x => x.ConceptDistributionDetails)
                .Include(x => x.AccountingPlan)
                .Include(x => x.CurrentAccount)
                .Include(x => x.Dependency)
                .FirstOrDefaultAsync();

            return concept;
        }

        public async Task<bool> HasPayments(Guid id)
        {
            bool isValid = false;

            var payments = await _context.Payments
                .Where(x => x.ConceptId == id).CountAsync();

            if (payments > 0)
                isValid = true;

            return isValid;
        }

        public async Task<object> GetReportBudgetBalance(Guid id)
        {
            var concepts = await _context.Concepts
                .Where(x => x.DependencyId == id)
                .ToListAsync();

            var incomes = await _context.Payments
                .Where(x => concepts.Any(c => c.Id == x.EntityId))
                .Select(x => new
                {
                    date = x.PaymentDate.ToLocalDateFormat(),
                    siaf = "",
                    invoice = x.InvoiceId.HasValue ? $"{x.Invoice.Series}-{x.Invoice.Number}" : "",
                    document = "",
                    order = "",
                    concept = x.Description,
                    month = "",
                    provision = 0.00M,
                    income = x.Total,
                    expense = 0.00M
                })
                .ToListAsync();

            var expenses = await _context.Expenses
                .Where(x => x.DependencyId == id)
                .Select(x => new
                {
                    date = x.Date.ToLocalDateFormat(),
                    siaf = x.Code,
                    invoice = x.Invoice,
                    document = x.ReferenceDocument,
                    order = x.Order,
                    concept = x.Concept,
                    month = $"{x.Date:MM/yyyy}",
                    provision = 0.00M,
                    income = 0.00M,
                    expense = x.Amount
                })
                .ToListAsync();

            var provision = await _context.ExpenditureProvisions
                .Where(x =>
                x.DependencyId == id &&
                x.Status == ConstantHelpers.ExpenditureProvision.Status.PENDING)
                .Select(x => new
                {
                    date = x.CreatedAt.ToLocalDateFormat(),
                    siaf = "",
                    invoice = "",
                    document = "",
                    order = "",
                    concept = x.Concept,
                    month = "",
                    provision = x.Amount,
                    income = 0.00M,
                    expense = 0.00M
                }).ToListAsync();

            var data = incomes;

            data.AddRange(expenses);

            data.AddRange(provision);

            return data;
        }

        public async Task UpdateExtemporaneousEnrollmentConcept(Guid studentId)
        {
            var student = await _context.Students.FindAsync(studentId);

            var conceptId = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.General.INTEGRATED_SYSTEM));

            var concepts = await _context.Payments
                  .Where(x => x.ConceptId == conceptId && x.UserId == student.UserId
                  && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.PaymentDate.HasValue)
                  .ToListAsync();

            foreach (var item in concepts)
            {
                item.Type = ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT_PROCESSED;
                item.Status = ConstantHelpers.PAYMENT.STATUS.PAID;
            };

            await _context.SaveChangesAsync();

            //return procedures;

            //await _context.SaveChangesAsync();
            //return true;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptsDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user = null, string search = null, string code = null, byte? status = null, Guid? classifierId = null, Guid? dependencyId = null, bool? showTaxedOnly = null)
        {
            Expression<Func<Concept, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Description);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Amount);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Classifier.Code);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Dependency.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.Concepts
                .IgnoreQueryFilters() //ignora los filtros de tablas relacionadas como clasificadores que luego afectan el select
                .Where(x => !x.DeletedAt.HasValue)
                .AsNoTracking();

            if (user != null && !user.IsInRole(ConstantHelpers.ROLES.INCOMES) && !user.IsInRole(ConstantHelpers.ROLES.TREASURY) && !user.IsInRole(ConstantHelpers.ROLES.SUPERADMIN) && !user.IsInRole(ConstantHelpers.ROLES.BANK_COLLECTION) && !user.IsInRole(ConstantHelpers.ROLES.ECONOMIC_MANAGEMENT_ADMIN))
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var cashierDependencies = _context.CashierDependencies
                        .Where(x => x.UserId == userId)
                        .Select(x => x.DependencyId)
                        .ToHashSet();

                    query = query.Where(x => cashierDependencies.Contains(x.DependencyId));
                }
            }

            if (classifierId.HasValue && classifierId != Guid.Empty)
                query = query.Where(x => x.ClassifierId == classifierId);

            if (dependencyId.HasValue && dependencyId != Guid.Empty)
                query = query.Where(x => x.DependencyId == dependencyId);

            if (showTaxedOnly.HasValue && showTaxedOnly == true)
                query = query.Where(x => x.IsTaxed);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Description.ToUpper().Contains(search.ToUpper()));

            if (!string.IsNullOrEmpty(code))
                query = query.Where(x => x.Code.ToUpper().Contains(code.ToUpper()));

            if (status.HasValue)
            {
                switch (status.Value)
                {
                    case 2: query = query.Where(x => x.IsEnabled); break;
                    case 3: query = query.Where(x => !x.IsEnabled); break;
                    default:
                        break;
                }
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Code,
                    description = x.Description,
                    amount = x.Amount,
                    accounting = x.Classifier.Code,
                    accountingId = x.Classifier.Id,
                    dependency = x.Dependency.Name,
                    dependencyId = x.Dependency.Id,
                    isTaxed = x.IsTaxed,
                    isDivided = x.IsDividedAmount,
                    conceptDistributionId = x.ConceptDistributionId,
                    accountingPlan = x.AccountingPlanId,
                    currentAccount = x.CurrentAccountId,
                    isEnabled = x.IsEnabled
                }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<IEnumerable<Concept>> GetAllWithIncludes()
        {
            var result = await _context.Concepts
                .Include(x => x.ConceptDistribution).ThenInclude(x => x.ConceptDistributionDetails)
                .Include(x => x.AccountingPlan)
                .Include(x => x.Classifier)
                .Include(x => x.CurrentAccount)
                .Include(x => x.Dependency).ToListAsync();

            return result;
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> AnyWithCodeAndDescription(string code, string description)
        {
            return await _context.Concepts.AnyAsync(x => x.Code == code && x.Description == description);
        }
        private string GetConfigurationValue(Dictionary<string, string> list, string key)
        {
            return list.ContainsKey(key) ? list[key] :

                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[key] : "";
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllConceptsDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<Concept, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.AccountingPlan.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Amount;
                    break;
            }

            var query = _context.Concepts
                .Where(x => x.IsEnabled)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                var searchValue = search.Trim().ToUpper();
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue) ||
                                x.Description.ToUpper().Contains(searchValue) ||
                                x.AccountingPlan.Code.ToUpper().Contains(searchValue));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    classifier = x.AccountingPlan.Code,
                    code = x.Code,
                    description = x.Description,
                    amount = x.Amount
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
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

        public async Task<List<Concept>> GetEnrollmentRelatedConcepts()
        {
            var concepts = new List<Concept>();

            var admissionEnrollmentConceptId = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ADMISSION_ENROLLMENT_PROCEDURE);
            if (!string.IsNullOrEmpty(admissionEnrollmentConceptId))
            {
                var id = Guid.Parse(admissionEnrollmentConceptId);
                var concept = await _context.Concepts.FirstOrDefaultAsync(x => x.Id == id);
                if (concept != null) concepts.Add(concept);
            }

            var extemporaneousEnrollmentConceptId = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.EXTEMPORANEOUS_ENROLLMENT_SURCHARGE_PROCEDURE);
            if (!string.IsNullOrEmpty(extemporaneousEnrollmentConceptId))
            {
                var id = Guid.Parse(extemporaneousEnrollmentConceptId);
                var concept = await _context.Concepts.FirstOrDefaultAsync(x => x.Id == id);
                if (concept != null) concepts.Add(concept);
            }

            var regularEnrollmentConceptId = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REGULAR_ENROLLMENT_PROCEDURE);
            if (!string.IsNullOrEmpty(regularEnrollmentConceptId))
            {
                var id = Guid.Parse(regularEnrollmentConceptId);
                var concept = await _context.Concepts.FirstOrDefaultAsync(x => x.Id == id);
                if (concept != null) concepts.Add(concept);
            }

            var specialEnrollmentConceptId = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.SPECIAL_ENROLLMENT_PROCEDURE);
            if (!string.IsNullOrEmpty(specialEnrollmentConceptId))
            {
                var id = Guid.Parse(specialEnrollmentConceptId);
                var concept = await _context.Concepts.FirstOrDefaultAsync(x => x.Id == id);
                if (concept != null) concepts.Add(concept);
            }

            var unbeatenEnrollmentConceptId = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENT_ENROLLMENT_PROCEDURE);
            if (!string.IsNullOrEmpty(unbeatenEnrollmentConceptId))
            {
                var id = Guid.Parse(unbeatenEnrollmentConceptId);
                var concept = await _context.Concepts.FirstOrDefaultAsync(x => x.Id == id);
                if (concept != null) concepts.Add(concept);
            }

            var directedCourseConceptId = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.DIRECTED_COURSE_COST_CONCEPT);
            if (!string.IsNullOrEmpty(directedCourseConceptId))
            {
                var id = Guid.Parse(directedCourseConceptId);
                var concept = await _context.Concepts.FirstOrDefaultAsync(x => x.Id == id);
                if (concept != null) concepts.Add(concept);
            }

            var extraConceptId = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ADDITIONAL_PAYMENT_FOR_EXTRA_ACADEMIC_YEARS_CONCEPT);
            if (!string.IsNullOrEmpty(extraConceptId))
            {
                var id = Guid.Parse(extraConceptId);
                var concept = await _context.Concepts.FirstOrDefaultAsync(x => x.Id == id);
                if (concept != null) concepts.Add(concept);
            }

            var disapprovedConcepts = await _context.DisapprovedCourseConcepts.Select(x => x.Concept).ToListAsync();
            concepts.AddRange(disapprovedConcepts);

            var enrollmentConcepts = await _context.EnrollmentConcepts.Select(x => x.Concept).ToListAsync();
            concepts.AddRange(enrollmentConcepts);

            concepts = concepts.Distinct().ToList();

            return concepts;
        }
                
        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptReportDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null, byte type = 0)
        {
            Expression<Func<Concept, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.AccountingPlan.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Classifier.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Description;
                    break;
                //case "4":
                //    orderByPredicate = (x) => x.Amount;
                //    break;
                case "6":
                    orderByPredicate = (x) => x.CurrentAccount.Code;
                    break;
                default:
                    orderByPredicate = (x) => x.Description;
                    break;
            }

            var query = _context.Concepts
                .AsNoTracking();

            var paymentQuery = _context.Payments
                .Where(x => x.ConceptId.HasValue)
                .AsNoTracking();

            if (type == 1)
                paymentQuery = paymentQuery.Where(x => x.IsBankPayment);

            if (type == 2)
                paymentQuery = paymentQuery.Where(x => !x.IsBankPayment);

            if (startDate.HasValue)
            {
                query = query.Where(x => x.Payments.Any(y => y.PaymentDate >= startDate));
                paymentQuery = paymentQuery.Where(x => x.PaymentDate >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(x => x.Payments.Any(y => y.PaymentDate <= endDate));
                paymentQuery = paymentQuery.Where(x => x.PaymentDate <= endDate);
            }

            var payments = await paymentQuery
                .Select(x => new
                {
                    x.ConceptId,
                    x.Total
                })
                .ToListAsync();

            if (user != null && user.IsInRole(ConstantHelpers.ROLES.CASHIER))
            {
                var configs = await _context.Configurations.ToDictionaryAsync(x => x.Key, x => x.Value);

                var course_withdrwal_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT_AUTO));
                var cycle_withdrwal_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT_AUTO));
                var substitute_exam_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT_AUTO));

                var course_withdrwal_concept = Guid.Empty;
                var cycle_withdrwal_concept = Guid.Empty;
                var substitute_exam_concept = Guid.Empty;
                Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT), out course_withdrwal_concept);
                Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT), out cycle_withdrwal_concept);
                Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT), out substitute_exam_concept);

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var cashierDependenciesId = await _context.CashierDependencies
                    .Where(x => x.UserId == userId)
                    .Select(x => x.DependencyId)
                    .ToListAsync();

                query = query.Where(x => cashierDependenciesId.Contains(x.DependencyId)
                || x.Id == course_withdrwal_concept || x.Id == cycle_withdrwal_concept || x.Id == substitute_exam_concept);
            }

            var concepts = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    accountingPlan = x.AccountingPlan.Code,
                    classifier = x.Classifier.Code,
                    currentAccount = x.CurrentAccount.Name,
                    code = x.Code,
                    description = x.Description
                }, search)
                .ToListAsync();

            var data = concepts
                .Select(x => new
                {
                    x.id,
                    x.accountingPlan,
                    x.classifier,
                    x.currentAccount,
                    x.code,
                    x.description,
                    count = payments.Where(y => y.ConceptId == x.id).Count(),
                    total = payments.Where(y => y.ConceptId == x.id).Sum(y => y.Total),
                    searchStartDate = startDate == null ? "" : startDate.ToLocalDateFormat(),
                    searchEndDate = endDate == null ? "" : endDate.ToLocalDateFormat()
                }).ToList();

            var recordsFiltered = data.Where(x => x.count > 0).Count();

            data = data
                .Where(x => x.count > 0)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<decimal> GetConceptReportTotalAmount(DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null, byte type = 0)
        {
            var paymentQuery = _context.Payments
                .Where(x => x.ConceptId.HasValue)
                .AsNoTracking();

            if (type == 1)
                paymentQuery = paymentQuery.Where(x => x.IsBankPayment);

            if (type == 2)
                paymentQuery = paymentQuery.Where(x => !x.IsBankPayment);

            if (startDate.HasValue) paymentQuery = paymentQuery.Where(x => x.PaymentDate >= startDate);

            if (endDate.HasValue) paymentQuery = paymentQuery.Where(x => x.PaymentDate <= endDate);

            if (user != null && user.IsInRole(ConstantHelpers.ROLES.CASHIER))
            {
                var configs = await _context.Configurations.ToDictionaryAsync(x => x.Key, x => x.Value);

                var course_withdrwal_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT_AUTO));
                var cycle_withdrwal_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT_AUTO));
                var substitute_exam_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT_AUTO));

                var course_withdrwal_concept = Guid.Empty;
                var cycle_withdrwal_concept = Guid.Empty;
                var substitute_exam_concept = Guid.Empty;
                Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT), out course_withdrwal_concept);
                Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT), out cycle_withdrwal_concept);
                Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT), out substitute_exam_concept);

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var cashierDependenciesId = await _context.CashierDependencies
                    .Where(x => x.UserId == userId)
                    .Select(x => x.DependencyId)
                    .ToListAsync();

                paymentQuery = paymentQuery.Where(x => cashierDependenciesId.Contains(x.Concept.DependencyId)
                || x.ConceptId == course_withdrwal_concept || x.ConceptId == cycle_withdrwal_concept || x.ConceptId == substitute_exam_concept);

            }

            var total = await paymentQuery.SumAsync(x => x.Total);

            return total;
        }
        
        public async Task<List<ConceptReportTemplate>> GetConceptReportExcelData(DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null, byte type = 0)
        {
            var query = _context.Concepts
                //.Where(x => x.Payments.Any())
                .AsNoTracking();

            var paymentQuery = _context.Payments
                .Where(x => x.ConceptId.HasValue)
                .AsNoTracking();

            if (type == 1)
                paymentQuery = paymentQuery.Where(x => x.IsBankPayment);

            if (type == 2)
                paymentQuery = paymentQuery.Where(x => !x.IsBankPayment);

            if (startDate.HasValue)
                paymentQuery = paymentQuery.Where(x => x.PaymentDate >= startDate);

            if (endDate.HasValue)
                paymentQuery = paymentQuery.Where(x => x.PaymentDate <= endDate);

            var payments = await paymentQuery
                .Select(x => new
                {
                    x.ConceptId,
                    x.Total
                })
                .ToListAsync();

            if (user != null && user.IsInRole(ConstantHelpers.ROLES.CASHIER))
            {
                var configs = await _context.Configurations.ToDictionaryAsync(x => x.Key, x => x.Value);

                var course_withdrwal_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT_AUTO));
                var cycle_withdrwal_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT_AUTO));
                var substitute_exam_concept_auto = bool.Parse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT_AUTO));

                var course_withdrwal_concept = Guid.Empty;
                var cycle_withdrwal_concept = Guid.Empty;
                var substitute_exam_concept = Guid.Empty;
                Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT), out course_withdrwal_concept);
                Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT), out cycle_withdrwal_concept);
                Guid.TryParse(GetConfigurationValue(configs, ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT), out substitute_exam_concept);

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var cashierDependenciesId = await _context.CashierDependencies
                    .Where(x => x.UserId == userId)
                    .Select(x => x.DependencyId)
                    .ToListAsync();

                query = query.Where(x => cashierDependenciesId.Contains(x.DependencyId)
                || x.Id == course_withdrwal_concept || x.Id == cycle_withdrwal_concept || x.Id == substitute_exam_concept);
            }

            var concepts = await query
                .Select(x => new
                {
                    id = x.Id,
                    accountingPlan = x.AccountingPlan.Code,
                    classifier = x.Classifier.Code,
                    currentAccount = x.CurrentAccount.Name,
                    code = x.Code,
                    description = x.Description
                }, search)
                .ToListAsync();

            var data = concepts
                .Select(x => new ConceptReportTemplate
                {
                    AccountingPlan = x.accountingPlan,
                    Classifier = x.classifier,
                    CurrentAccount = x.currentAccount,
                    Code = x.code,
                    Name = x.description,
                    Count = payments.Where(y => y.ConceptId == x.id).Count(),
                    Total = payments.Where(y => y.ConceptId == x.id).Sum(y => y.Total)
                }).ToList();

            data = data.Where(x => x.Count > 0).ToList();

            return data;
        }

        public async Task<List<PaymentTemplate>> GetPaymentsByConceptId(Guid id, DateTime? startDate = null, DateTime? endDate = null, byte type = 0)
        {
            var qryPayments = _context.Payments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.ConceptId == id)
                .AsNoTracking();

            if (type == 1)
                qryPayments = qryPayments.Where(x => x.IsBankPayment);

            if (type == 2)
                qryPayments = qryPayments.Where(x => !x.IsBankPayment);

            if (startDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate >= startDate);

            if (endDate.HasValue) qryPayments = qryPayments.Where(x => x.PaymentDate <= endDate);

            var payments = await qryPayments
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
                    Serie = x.InvoiceId.HasValue ? $"{x.Invoice.Series}-{x.Invoice.Number:00000000}" : x.OperationCodeB 
                })
                .ToListAsync();

            return payments;
        }

    }
}
