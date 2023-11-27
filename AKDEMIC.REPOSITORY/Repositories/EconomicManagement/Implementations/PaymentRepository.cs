using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Payment;
using DocumentFormat.OpenXml.Math;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Implementations
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AkdemicContext context) : base(context)
        {
        }

        #region private        

        //public Expression<Func<Payment, dynamic>> SelectPayments()
        //{
        //    return (x) => new
        //    {
        //        Id = x.Id,
        //        User = x.User.FullName,
        //        DNI = x.User.Dni,
        //        ConceptName = x.Description,
        //        StatusCreated = _context.RegistryPatterns.Any(s => s.PaymentId == x.Id),
        //        CurrentAcademicYear = _context.Students.Where(s => s.UserId == x.UserId).FirstOrDefault().CurrentAcademicYear

        //    };
        //}

        public Expression<Func<RegistryPattern, dynamic>> SelectConceptsGenerates()
        {
            return (x) => new
            {
                FullnName = x.Student.User.FullName,
                FacultyName = x.Student.Career.Faculty.Name,
                CareerName = x.Student.Career.Name
            };
        }

        public Func<Payment, string[]> GetPaymentsDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.User.Name + "",
                x.User.MaternalSurname + "",
                x.User.PaternalSurname + "",
                x.Description + "",
                x.User.Dni + ""
            };
        }

        public Func<RegistryPattern, string[]> GetConceptsGenerateSearchValuePredicate()
        {
            return (x) => new[]
            {
               x.Student.User.Name + "",
               x.Student.User.MaternalSurname + "",
               x.Student.User.PaternalSurname + "",
               x.Student.Career.Name + "",
               x.Student.Career.Faculty.Name + ""
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentsByConceptListAsync(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            //var ConceptGuids = new List<Guid>();

            //var configurationBachelorType = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_TYPE_BACHELOR).FirstOrDefaultAsync();
            //var configurationBachelor = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_BACHELOR).FirstOrDefaultAsync();

            //var configurationConceptDegreeProffesionalExperience = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE).FirstOrDefaultAsync();
            //var configurationConceptDegreeSufficiencyExam = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM).FirstOrDefaultAsync();
            //var configurationConceptDegreeSupportTesis = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUPPORT_TESIS).FirstOrDefaultAsync();

            //if (configurationBachelor != null && !String.IsNullOrEmpty(configurationBachelor.Value))
            //{
            //    var guidBachelor = new Guid(configurationBachelor.Value);
            //    ConceptGuids.Add(guidBachelor);
            //}
            //if (configurationConceptDegreeProffesionalExperience != null && !String.IsNullOrEmpty(configurationConceptDegreeProffesionalExperience.Value))
            //{
            //    var guidConceptTitleDegreeProffesionalExperience = new Guid(configurationConceptDegreeProffesionalExperience.Value);
            //    ConceptGuids.Add(guidConceptTitleDegreeProffesionalExperience);

            //}
            //if (configurationConceptDegreeSufficiencyExam != null && !String.IsNullOrEmpty(configurationConceptDegreeSufficiencyExam.Value))
            //{
            //    var guidConceptTitleDegreeSufficiencyExam = new Guid(configurationConceptDegreeSufficiencyExam.Value);
            //    ConceptGuids.Add(guidConceptTitleDegreeSufficiencyExam);

            //}
            //if (configurationConceptDegreeSupportTesis != null && !String.IsNullOrEmpty(configurationConceptDegreeSupportTesis.Value))
            //{
            //    var guidConceptTitleDegreeSupportTesis = new Guid(configurationConceptDegreeSupportTesis.Value);
            //    ConceptGuids.Add(guidConceptTitleDegreeSupportTesis);
            //}
            //var query = _context.Payments.Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.User.UserRoles.Any(s => s.Role.Name == "Alumnos"))
            //            .Where(x => ConceptGuids.Contains(x.ConceptId.Value)).AsNoTracking();
            var query = _context.RegistryPatterns.AsNoTracking();

            var recordsFiltered = 0;

            recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    user = x.Student.User.FullName,
                    dni = x.Student.User.Dni,
                    concept_name = _context.Concepts.Where(y => y.Id == x.ConceptId).FirstOrDefault().Description,
                    //status_created = _context.RegistryPatterns.Any(s => s.PaymentId == x.Id),
                    currentAcademicYear = x.Student.CurrentAcademicYear
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

        public async Task<DataTablesStructs.ReturnedData<object>> ConceptGenerateByRegistryPatternAsync(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, Expression<Func<RegistryPattern, dynamic>> selectPredicate = null, Func<RegistryPattern, string[]> searchValuePredicate = null, string searchValue = null)
        {
            var query = _context.RegistryPatterns
                                                 .Include(x => x.Student.Career.Faculty)
                                                 .Include(x => x.Student.User).AsQueryable();

            var configurationBachelorType = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_TYPE_BACHELOR).FirstOrDefaultAsync();
            var configurationBachelor = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_BACHELOR).FirstOrDefaultAsync();
            var configurationConceptDegreeProffesionalExperience = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_PROFESSIONAL_EXPERIENCE).FirstOrDefaultAsync();
            var configurationConceptDegreeSufficiencyExam = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUFFICIENCY_EXAM).FirstOrDefaultAsync();
            var configurationConceptDegreeSupportTesis = await _context.Configurations.Where(x => x.Key == ConstantHelpers.Configuration.DegreeManagement.CONCEPT_PROFESSIONAL_DEGREE_SUPPORT_TESIS).FirstOrDefaultAsync();


            var listGuid = new List<Guid>();


            if (configurationBachelor != null && !String.IsNullOrEmpty(configurationBachelor.Value))
            {
                var guidBachelor = new Guid(configurationBachelor.Value);
                listGuid.Add(guidBachelor);

            }
            if (configurationConceptDegreeProffesionalExperience != null && !String.IsNullOrEmpty(configurationConceptDegreeProffesionalExperience.Value))
            {
                var guidConceptTitleDegreeProffesionalExperience = new Guid(configurationConceptDegreeProffesionalExperience.Value);
                listGuid.Add(guidConceptTitleDegreeProffesionalExperience);

            }
            if (configurationConceptDegreeSufficiencyExam != null && !String.IsNullOrEmpty(configurationConceptDegreeSufficiencyExam.Value))
            {
                var guidConceptTitleDegreeSufficiencyExam = new Guid(configurationConceptDegreeSufficiencyExam.Value);
                listGuid.Add(guidConceptTitleDegreeSufficiencyExam);


            }
            if (configurationConceptDegreeSupportTesis != null && !String.IsNullOrEmpty(configurationConceptDegreeSupportTesis.Value))
            {
                var guidConceptTitleDegreeSupportTesis = new Guid(configurationConceptDegreeSupportTesis.Value);
                listGuid.Add(guidConceptTitleDegreeSupportTesis);

            }
            //query = query.Where(x => listGuid.Contains(x.UserProcedure.ProcedureId));
            if (facultyId.HasValue)
            {
                query = query.Where(x => x.Student.Career.FacultyId == facultyId.Value);
            }
            if (careerId.HasValue)
            {
                query = query.Where(x => x.Student.CareerId == careerId.Value);
            }
            query = query.WhereSearchValue(searchValuePredicate, searchValue)
                .AsNoTracking();

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }
        #endregion


        //public override async Task Insert(Payment payment)
        //{
        //    await base.Insert(payment);

        //    var course_withdrwal_concept_auto = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT_AUTO));
        //    var cycle_withdrwal_concept_auto = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT_AUTO));
        //    var substitute_exam_concept_auto = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT_AUTO));

        //    var course_withdrwal_concept = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT));
        //    var cycle_withdrwal_concept = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT));
        //    var substitute_exam_concept = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT));

        //    if ((payment.ConceptId == course_withdrwal_concept && course_withdrwal_concept_auto) ||
        //        (payment.ConceptId == cycle_withdrwal_concept && cycle_withdrwal_concept_auto) ||
        //        (payment.ConceptId == substitute_exam_concept && substitute_exam_concept_auto))
        //    {
        //        await _context.PaymentConceptStatuses.AddAsync(new PaymentConceptStatus
        //        {
        //            PaymentId = payment.Id,
        //            Status = ConstantHelpers.PAYMENT.CONCEPTSTATUS.APPROVED,
        //        });
        //    }
        //    else
        //    {
        //        await _context.PaymentConceptStatuses.AddAsync(new PaymentConceptStatus
        //        {
        //            PaymentId = payment.Id,
        //            Status = ConstantHelpers.PAYMENT.CONCEPTSTATUS.PENDING,
        //        });
        //    }
        //}

        public async Task<IEnumerable<Payment>> GetPendingPayments(string code)
        {
            return await _context.Payments
                .IgnoreQueryFilters()
                .Where(x => x.User.NormalizedUserName == code.Normalize() && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING)
                .Include(x => x.Payments)
                .Include(x => x.Term)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPostulantPayments(Guid postulantId)
        {
            return await _context.Payments
                .IgnoreQueryFilters()
                .Where(x => x.EntityId == postulantId)
                .Include(x => x.Payments)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPendingExternalPayments(string code)
        {
            return await _context.UserExternalProcedures
            .Where(x => x.ExternalUser.DocumentNumber.Normalize() == code.Normalize() && x.Payment.Status == ConstantHelpers.PAYMENT.STATUS.PENDING).Select(x => x.Payment).ToListAsync();
        }

        public async Task<IEnumerable<(int day, string accounting, decimal total)>> GetEducationalRateSummaryByYearAndMonth(int year, int month)
        {
            var data = await _context.Payments
                .Where(x => x.IssueDate.Year == year &&
                            x.IssueDate.Month == month &&
                            x.Type == ConstantHelpers.PAYMENT.TYPES.PROCEDURE &&
                            x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .Join(_context.UserProcedures, p => p.EntityId, u => u.Id, (p, u) => new { Payment = p, UserProcedure = u })
                .GroupBy(x => new { x.Payment.IssueDate.Day, x.UserProcedure.Procedure.Classifier.Code })
                .Select(x => new
                {
                    day = x.Key.Day,
                    accounting = x.Key.Code,
                    total = x.Sum(g => g.Payment.Total)
                })
                .ToListAsync();

            var result = data.AsEnumerable()
                .Select(x => (day: x.day, accounting: x.accounting, total: x.total));

            return result;

            //var result = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
            //    .Select(x => new
            //    {
            //        day = x,
            //        total = _context.Payments.Where(p => p.IssueDate.ToDefaultTimeZone().Year == year &&
            //        p.IssueDate.ToDefaultTimeZone().Month == month &&
            //        p.IssueDate.ToDefaultTimeZone().Day == x &&
            //        p.Type == ConstantHelpers.PAYMENT.TYPES.PROCEDURE &&
            //        p.Status == ConstantHelpers.PAYMENT.STATUS.PAID).Sum(p => p.Total)
            //    }).ToList();
            //return result;
        }

        public async Task<int> DegreePaymentCount(bool isIntegrated, List<Guid> resultsToSearch)
        {
            if (isIntegrated)
            {
                var query = _context.UserProcedures.Where(x => (x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.ACCEPTED ||
                x.Status == ConstantHelpers.USER_PROCEDURES.STATUS.ARCHIVED) && x.User.UserRoles.Any(s => s.Role.Name == "Alumnos"))
                .Where(x => resultsToSearch.Contains(x.ProcedureId));
                return await query.CountAsync();
            }
            else
            {
                var query = _context.Payments.Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.User.UserRoles.Any(s => s.Role.Name == "Alumnos"))
                    .Where(x => resultsToSearch.Contains(x.ConceptId.Value));
                return await query.CountAsync();
            }
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentsByConceptList(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            return await GetPaymentsByConceptListAsync(sentParameters, searchValue);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> ConceptGenerateByRegistryPattern(DataTablesStructs.SentParameters sentParameters, Guid? facultyId, Guid? careerId, System.Linq.Expressions.Expression<Func<RegistryPattern, dynamic>> selectPredicate = null, Func<RegistryPattern, string[]> searchValuePredicate = null, string searchValue = null)
        {
            return await ConceptGenerateByRegistryPatternAsync(sentParameters, facultyId, careerId, SelectConceptsGenerates(), GetConceptsGenerateSearchValuePredicate(), searchValue);
        }

        public async Task<IEnumerable<Payment>> GetAllByUser(string userId, Guid termId)
        {
            var query = _context.Payments
                .Where(x => x.UserId == userId && x.TermId == termId)
                .Include(x => x.Invoice)
                .Include(x => x.Concept)
                .AsNoTracking();
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Payment>> GetAllByUser(string userId, byte? status = null, byte? type = null)
        {
            var query = _context.Payments
                .IgnoreQueryFilters()
                .Where(x => x.UserId == userId)
                .Include(x => x.Invoice)
                .Include(x => x.Concept)
                .Include(x => x.UserProcedures)
                .AsNoTracking();

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);
            if (type.HasValue)
                query = query.Where(x => x.Type == type.Value);

            return await query.ToListAsync();
        }
        public async Task<object> GetPaymentsHome(string userId)
        {
            var payments = _context.Payments.Where(x => x.UserId == userId && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING)
                 .OrderByDescending(x => x.IssueDate)
                 .Take(3)
                 .Select(x => new
                 {
                     description = x.Description,
                     total = x.Total,
                     issueDate = x.IssueDate
                 });

            return payments;
        }
        public async Task<bool> ExistsAnotherExtemporaneousPaymentForUser(string userId)
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            var payments = _context.Payments.Any(x => x.UserId == userId && x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT && x.PaymentDate >= term.StartDate && x.PaymentDate <= term.EndDate);

            return payments;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatablePayment(DataTablesStructs.SentParameters sentParameters, byte? status = null, bool onlyConfigs = false, string search = null)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Type;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.IssueDate;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Total * 1.00M;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    break;
            }

            var query = _context.Payments
                .AsNoTracking();

            if (status.HasValue) query = query.Where(x => x.Status == status);

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();
                query = query.Where(x => x.Description.ToUpper().Contains(search) || x.User.FullName.Contains(search) || x.User.UserName.Contains(search));
            }

            //var course_withdrwal_concept_auto = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT_AUTO));
            //var cycle_withdrwal_concept_auto = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT_AUTO));
            //var substitute_exam_concept_auto = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT_AUTO));
            //var course_withdrwal_concept = Guid.Empty;
            //var cycle_withdrwal_concept = Guid.Empty;
            //var substitute_exam_concept = Guid.Empty;
            //if (onlyConfigs)
            //{
            //    course_withdrwal_concept = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.COURSE_WITHDRWAL_CONCEPT));
            //    cycle_withdrwal_concept = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.CYCLE_WITHDRWAL_CONCEPT));
            //    substitute_exam_concept = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.IntranetManagement.SUBSTITUTE_EXAM_CONCEPT));

            //    ///
            //    var query2 = query;
            //    var query3 = query;
            //    var query4 = query;
            //    if (course_withdrwal_concept!= Guid.Empty)
            //        query2 = query.Where(x => x.ConceptId ==  course_withdrwal_concept);

            //    if (cycle_withdrwal_concept!= Guid.Empty)
            //        query3 = query.Where(x => x.ConceptId ==  cycle_withdrwal_concept);

            //    if (substitute_exam_concept!= Guid.Empty)
            //        query4 = query.Where(x => x.ConceptId ==substitute_exam_concept);

            //    query = query2.Union(query3.Union(query4));
            //    ///
            //}

            var typeConst = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION;
            var recordsFiltered = await query.CountAsync();
            query = query.AsQueryable();

            var db = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    userid = x.UserId,
                    code = x.User.UserName,
                    user = x.User.FullName,
                    type = x.Type,
                    concept = x.Description,
                    totalamount = x.Total * 1.00M,
                    discount = x.Discount,
                    issueDate = x.IssueDate.ToShortDateString(),
                    status = x.Status
                }).ToListAsync();

            var data = db.Select(x => new
            {
                x.id,
                x.userid,
                x.code,
                x.user,
                type = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.type],
                x.concept,
                x.totalamount,
                x.discount,
                x.issueDate,
                x.status
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

        public async Task<List<Payment>> GetPaymentsReport(byte status, string date)
        {
            var payments = await _context.Payments
                     .Include(x => x.Invoice.PettyCash.User)
                     .Where(x => x.Status == status && x.InvoiceId != null && x.UserId != null && x.Invoice.Date.ToString("dd/MM/yyyy").Equals(date))
                     .ToListAsync();

            return payments;
        }
        public async Task<List<Payment>> GetPaymentIncludeUserByStatusAndInvoiceId(byte status, Guid invoiceId)
        {
            var _payment = await _context.Payments.Include(x => x.User)
            .Where(x => x.Status == status && x.InvoiceId == invoiceId).ToListAsync();
            return _payment;


        }

        public IQueryable<Payment> GetPaymentQuery(List<Guid?> userProcedures)
        {
            var paymentsQuery = _context.Payments.AsNoTracking().Where(x => userProcedures.Contains(x.Id));

            return paymentsQuery;
        }

        public async Task<object> GetIncomes(List<Concept> concepts)
        {
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

            return incomes;
        }

        public async Task<List<Payment>> GetPaymentsAnyConcept(List<Concept> concepts)
        {
            var incomes = await _context.Payments.Include(x => x.Invoice)
            .Where(x => concepts.Any(c => c.Id == x.EntityId)).ToListAsync();

            return incomes;
        }
        public async Task<object> GetPaymentReport(List<Concept> concepts)
        {
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

            return incomes;
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentDatatableReport(DataTablesStructs.SentParameters sentParameters, string search)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Concept.Code;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Concept.Classifier.Code;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Total;
                    break;
                case "5":
                    orderByPredicate = (x) => x.PaymentDate;
                    break;
                default:
                    orderByPredicate = (x) => x.Concept.Code;
                    break;
            }


            var query = _context.Payments
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .OrderBy(x => x.Concept.Code)
                .Select(x => new
                {
                    id = x.Id,
                    code = x.Concept.Code,
                    concept = x.Description,
                    account = x.Concept.Classifier.Code,
                    totalAmount = x.Total,
                    date = x.PaymentDate.HasValue ? x.PaymentDate.Value.ToLocalDateTimeFormat() : "---"
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

        public IQueryable<Payment> PaymentsQry(DateTime date)
            => _context.Payments.Where(x => x.ConceptId.HasValue && x.PaymentDate.HasValue && x.PaymentDate.Value.Date == date).AsQueryable();
        public IQueryable<Payment> PaymentsQryConcept()
            => _context.Payments.Where(x => x.ConceptId.HasValue
            //&& x.Status == CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PAID
            ).AsQueryable();
        public async Task<object> GetOutstandingDebts(string userId)
        {
            var data = await _context.Payments
            .Where(x => x.UserId == userId && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING)
            .Select(x => new
            {
                id = x.Id,
                type = x.Type,
                concept = x.Description,
                totalamount = x.Total * 1.00M,
                discount = x.Discount,
                issueDate = x.IssueDate
            }).ToListAsync();

            var result = data
                .Select(x => new
                {
                    x.id,
                    type = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.type],
                    x.concept,
                    x.totalamount,
                    x.discount,
                    issueDate = x.issueDate.ToShortDateString()
                }).ToList();

            return result;
        }

        public async Task<List<Payment>> GetdbPayments(Guid[] payments)
            => await _context.Payments.Where(x => payments.Contains(x.Id)).ToListAsync();
        public async Task<object> GetPayedDebtDetail(Guid id)
        {
            var result = await _context.Payments
                .Where(i => i.InvoiceId == id)
                .Select(i => new
                {
                    id = i.Id,
                    concept = i.Description,
                    cost = i.Total
                }).ToListAsync();

            return result;
        }
        public async Task<List<Payment>> GetInvoiceDetailList(Guid id)
            => await _context.Payments
            .IgnoreQueryFilters()
             .Where(x => x.InvoiceId == id)
            .Include(x => x.Concept)
            .ToListAsync();
        public async Task<List<Payment>> GetPaymentByListConcept(IEnumerable<Concept> concepts)
        {
            var hash = concepts.Select(x => x.Id).ToHashSet();
            var result = await _context.Payments.Where(x => hash.Contains(x.ConceptId.Value)).ToListAsync();
            return result;
        }

        public async Task<List<Payment>> GetDetailsDocument(Guid id)
        {
            var details = await _context.Payments.Where(x => x.InvoiceId == id).ToListAsync();

            return details;
        }
        public async Task<List<Payment>> GetPaymentsByIdList(Guid id)
            => await _context.Payments
            .Where(x => x.InvoiceId == id)
            .Include(x => x.Incomes)
            .ToListAsync();

        public async Task<List<Payment>> GetPaymentByListPaymentGuid(List<Guid> payments)
            => await _context.Payments
            .IgnoreQueryFilters()
            .Where(x => payments.Contains(x.Id))
            .Include(x => x.Payments)
            .Include(x => x.Concept.AccountingPlan)
            .Include(x => x.Concept.CurrentAccount)
            .ToListAsync();
        public async Task<List<Payment>> GetPaymentListByInvoiceId(Guid id)
            => await _context.Payments
            .IgnoreQueryFilters()
               .Where(i => i.InvoiceId == id)
            .Include(x => x.Concept)
            .Include(x => x.Concept.AccountingPlan)
            .Include(x => x.Concept.CurrentAccount)
            .Include(x => x.CurrentAccount)
            .ToListAsync();

        public async Task<List<Payment>> GetPaymentWithDataById(Guid id)
            => await _context.Payments
            .Include(x => x.Invoice)
            .Include(x => x.CurrentAccount)
            .Include(x => x.Concept).ThenInclude(x => x.Classifier)
            .Include(x => x.Concept.Dependency)
            .Include(x => x.Concept.AccountingPlan)
            .Include(x => x.Concept.CurrentAccount)
            .Where(x => x.InvoiceId == id).ToListAsync();

        public async Task<List<Payment>> GetPaymentWithData()
            => await _context.Payments
                .Include(x => x.Invoice)
                .Include(x => x.Concept).ThenInclude(x => x.Classifier).ToListAsync();

        public async Task<List<Payment>> GetInvoiceByPettyCashId(Guid id)
            => await _context.Payments.IgnoreQueryFilters()
            .Where(x => x.Invoice.PettyCashId == id && x.Invoice.Canceled)
            .Include(x => x.Concept.Classifier)
            .Include(x => x.Concept.AccountingPlan)
            .Include(x => x.Invoice)
            .ToListAsync();

        public async Task<List<Payment>> GetByPettyCashBookId(Guid pettyCashBookId)
        {
            var book = await _context.PettyCashBooks.FindAsync(pettyCashBookId);

            var startDate = book.Date.Date.ToUtcDateTime();
            var endDate = book.Date.Date.AddDays(1).AddTicks(-1).ToUtcDateTime();

            var payments = await _context.Payments
                .IgnoreQueryFilters()
                .Where(x => startDate <= x.Invoice.PettyCash.InitialDate && x.Invoice.PettyCash.InitialDate <= endDate
                            && x.Invoice.PettyCash.UserId == book.UserId && x.Invoice.Canceled)
                .Include(x => x.Concept.AccountingPlan)
                .ToListAsync();

            return payments;
        }


        public async Task<List<Payment>> GetInvoiceByPettyCashIdV2(Guid id)
            => await _context.Payments
            .Where(x => x.Invoice.PettyCashId == id)
            .Include(x => x.Concept.Classifier)
            .Include(x => x.Concept.AccountingPlan)
            .Include(x => x.Concept.CurrentAccount)
            .Include(x => x.Invoice.PettyCash)
            .ToListAsync();


        public async Task<object> GetAssociatePaymentsGet(string userId, byte? status = null)
        {
            var userDependencies = await _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToListAsync();
            var procedureDependencies = await _context.ProcedureDependencies.AsNoTracking().Where(x => userDependencies.Contains(x.DependencyId)).Select(x => x.ProcedureId).ToListAsync();
            var userProcedures = await _context.UserProcedures.AsNoTracking().Where(x => procedureDependencies.Contains(x.ProcedureId)).Select(x => x.PaymentId).ToListAsync();

            var paymentsQuery = _context.Payments.AsNoTracking().Where(x => userProcedures.Contains(x.Id));
            if (status.HasValue)
            {
                paymentsQuery = paymentsQuery.Where(x => x.Status == status);
            }
            var payments = await paymentsQuery.Select(x => new
            {
                id = x.Id,
                type = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.Type],
                concept = x.Description,
                totalamount = x.Total * 1.00M,
                discount = x.Discount,
                issueDate = x.IssueDate.ToShortDateString(),
                status = x.Status
            }).ToListAsync();

            return payments;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetBankPaymentDatatableReport(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, Guid? formatId = null, ClaimsPrincipal user = null, string search = null)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Concept.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Concept.AccountingPlan.Code;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.PaymentDate;
                    break;
                case "5":
                    orderByPredicate = (x) => x.PaymentDate.Value.TimeOfDay;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Total;
                    break;
                default:
                    break;
            }

            var query = _context.Payments
                .Where(x => x.IsBankPayment)
                .AsNoTracking();

            if (startDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate >= startDate);
            if (endDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate <= endDate);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEPENDENCY))
                {
                    var dependencies = _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToHashSet();
                    query = query.Where(x => x.ConceptId.HasValue && dependencies.Contains(x.Concept.DependencyId));
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => (x.ConceptId.HasValue && x.Concept.Description.ToUpper().Contains(search.ToUpper())) || (x.UserId != null && x.User.UserName.ToUpper().Contains(search.ToUpper())));
            }

            if (formatId.HasValue && formatId != Guid.Empty)
            {
                query = query.Where(x => x.EntityLoadFormatId == formatId);
            }

            var recordsFiltered = await query.CountAsync();

            var concepts = await _context.Concepts.Select(x => x.Id).ToListAsync();

            query = query.AsQueryable();

            var data = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
               .OrderBy(x => x.Concept.Code)
               .Select(x => new
               {
                   id = x.Id,
                   code = x.Concept.Code,
                   concept = x.Description,
                   account = x.Concept.AccountingPlan.Code,
                   totalAmount = x.Total,
                   date = x.PaymentDate.HasValue ? $"{x.PaymentDate.ToDefaultTimeZone().Value:dd/MM/yyyy}" : "---",
                   time = x.PaymentDate.HasValue ? $"{x.PaymentDate.ToDefaultTimeZone().Value:HH:mm:ss}" : "---",
                   user = x.UserId != null ? x.User.UserName : "---"
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

        public async Task<decimal> GetBankPaymentTotalAmount(DateTime? startDate = null, DateTime? endDate = null, ClaimsPrincipal user = null, string search = null)
        {
            var query = _context.Payments
                .Where(x => x.IsBankPayment)
                .AsNoTracking();

            if (startDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate.Value >= startDate.Value);
            if (endDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate.Value <= endDate.Value);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEPENDENCY))
                {
                    var dependencies = _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToHashSet();
                    query = query.Where(x => x.ConceptId.HasValue && dependencies.Contains(x.Concept.DependencyId));
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => (x.ConceptId.HasValue && x.Concept.Description.ToUpper().Contains(search.ToUpper())) || (x.UserId != null && x.User.UserName.ToUpper().Contains(search.ToUpper())));
            }

            var totalAmount = await query.SumAsync(x => x.Total);

            return totalAmount;
        }

        public async Task<IEnumerable<Payment>> GetAllBankPaymentByDate(string issueDate)
        {
            var date = ConvertHelpers.DatepickerToDatetime(issueDate);

            var data = await _context.Payments
               .Where(x => x.IsBankPayment && x.PaymentDate.HasValue && x.PaymentDate.Value.Date == date.Date)
               .Include(x => x.Concept)
               .ToArrayAsync();

            return data;
        }

        public async Task<DateTime?> GetLastBankBatchDate()
        {
            var lastPayment = await _context.Payments
               .Where(x => x.IsBankPayment)
               .OrderByDescending(x => x.PaymentDate)
               .FirstOrDefaultAsync();

            return lastPayment != null ? lastPayment.PaymentDate : null;
        }

        public async Task<DateTime?> GetLastBankBatchDate(Guid formatId)
        {
            var lastPayment = await _context.Payments
               .Where(x => x.IsBankPayment && x.EntityLoadFormatId.HasValue && x.EntityLoadFormatId == formatId)
               .OrderByDescending(x => x.PaymentDate)
               .FirstOrDefaultAsync();

            var lastPaymentsToValid = await _context.PaymentToValidates
               .Where(x => x.EntityLoadFormatId.HasValue && x.EntityLoadFormatId == formatId)
               .OrderByDescending(x => x.Date)
               .FirstOrDefaultAsync();

            var lastDate = (DateTime?)null;
            if (lastPayment != null) lastDate = lastPayment.PaymentDate;
            if (lastPaymentsToValid != null)
            {
                if (lastDate.HasValue && lastPaymentsToValid.Date > lastDate.Value)
                    lastDate = lastPaymentsToValid.Date;
                else if (!lastDate.HasValue) lastDate = lastPaymentsToValid.Date;
            }

            return lastDate;
        }

        public async Task<List<DateTime>> GetAllBankPaymentDates()
        {
            var datetimes = await _context.Payments
               .Where(x => x.IsBankPayment && x.PaymentDate.HasValue)
               .Select(x => x.PaymentDate.Value)
               .ToListAsync();

            datetimes = datetimes.Select(x => x.ToDefaultTimeZone()).ToList();

            var dates = datetimes
               .GroupBy(x => x.Date)
               .Select(x => x.Key)
               .ToList();

            return dates;
        }

        public async Task<List<DateTime>> GetAllBankPaymentDates(Guid formatId)
        {
            var datetimes = await _context.Payments
               .Where(x => x.IsBankPayment && x.PaymentDate.HasValue && x.EntityLoadFormatId.HasValue && x.EntityLoadFormatId == formatId)
               .Select(x => x.PaymentDate.Value)
               .ToListAsync();

            var paymentsToValidateDates = await _context.PaymentToValidates
                .Where(x => x.EntityLoadFormatId.HasValue && x.EntityLoadFormatId == formatId)
                .Select(x => x.Date)
                .ToListAsync();

            datetimes.AddRange(paymentsToValidateDates);

            datetimes = datetimes.Select(x => x.ToDefaultTimeZone()).ToList();

            var dates = datetimes
               .GroupBy(x => x.Date)
               .Select(x => x.Key)
               .ToList();

            return dates;
        }

        public async Task<IEnumerable<Payment>> GetAllBankPaymentsByDateRange(Guid formatId, string startDate, string endDate, ClaimsPrincipal user = null, string search = null)
        {
            var start = DateTime.ParseExact(startDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToUtcDateTime();
            var end = DateTime.ParseExact(endDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            end = end.AddDays(1).AddTicks(-1).ToUtcDateTime();

            var query = _context.Payments
               .Where(x => x.IsBankPayment && x.EntityLoadFormatId == formatId
               && x.PaymentDate.HasValue && start <= x.PaymentDate.Value && x.PaymentDate.Value <= end)
               .AsNoTracking();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEPENDENCY))
                {
                    var dependencies = _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToHashSet();
                    query = query.Where(x => x.ConceptId.HasValue && dependencies.Contains(x.Concept.DependencyId));
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => (x.ConceptId.HasValue && x.Concept.Description.ToUpper().Contains(search.ToUpper())) || (x.UserId != null && x.User.UserName.ToUpper().Contains(search.ToUpper())));
            }

            var data = await query
               .Include(x => x.Concept)
               .Include(x => x.CurrentAccount)
               .Include(x => x.User)
               .Include(x => x.ExternalUser)
               .ToListAsync();

            return data;
        }

        public async Task<IEnumerable<Payment>> GetAllBankPaymentsByDateRange(Guid formatId, string startDate, string endDate, Guid conceptId, ClaimsPrincipal user = null)
        {
            var start = DateTime.ParseExact(startDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToUtcDateTime();
            var end = DateTime.ParseExact(endDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            end = end.AddDays(1).AddTicks(-1).ToUtcDateTime();

            var query = _context.Payments
               .Where(x => x.IsBankPayment && x.EntityLoadFormatId == formatId
               && x.ConceptId == conceptId && x.PaymentDate.HasValue
               && start <= x.PaymentDate.Value && x.PaymentDate.Value <= end)
               .AsQueryable();

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEPENDENCY))
                {
                    var dependencies = _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToHashSet();
                    query = query.Where(x => x.ConceptId.HasValue && dependencies.Contains(x.Concept.DependencyId));
                }
            }

            var data = await query
               .Include(x => x.Concept)
               .Include(x => x.CurrentAccount)
               .Include(x => x.User)
               .Include(x => x.ExternalUser)
               .ToArrayAsync();

            return data;
        }

        public async Task<object> GetAllBankPaymentsTreasuryDatatable(Guid formatId, string startDate, string endDate, ClaimsPrincipal user = null, string search = null)
        {
            var query = _context.Payments
               .Where(x => x.IsBankPayment && x.EntityLoadFormatId == formatId && x.PaymentDate.HasValue)
               .AsNoTracking();

            if (!string.IsNullOrEmpty(startDate))
            {
                var start = DateTime.ParseExact(startDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToUtcDateTime();
                query = query.Where(x => start <= x.PaymentDate.Value);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                var end = DateTime.ParseExact(endDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                end = end.AddDays(1).AddTicks(-1).ToUtcDateTime();
                query = query.Where(x => x.PaymentDate.Value <= end);
            }

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEPENDENCY) && !user.IsInRole(ConstantHelpers.ROLES.TREASURY))
                {
                    var dependencies = _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToHashSet();
                    query = query.Where(x => x.ConceptId.HasValue && dependencies.Contains(x.Concept.DependencyId));
                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => (x.ConceptId.HasValue && x.Concept.Description.ToUpper().Contains(search.ToUpper())) || (x.UserId != null && x.User.UserName.ToUpper().Contains(search.ToUpper())));
            }

            var payments = await query
                .Select(x => new
                {
                    date = x.PaymentDate,
                    //date = $"{x.PaymentDate:dd/MM/yyyy}",
                    //datestring = $"{x.PaymentDate:yyyyMMdd}",
                    code = x.Concept.BankCode,
                    concept = x.Concept.Description,
                    amount = x.Total,
                    id = x.ConceptId,
                    hasInvoice = x.InvoiceId.HasValue,
                    formatId = x.EntityLoadFormatId
                })
               .ToListAsync();

            var format = await _context.EntityLoadFormats.FirstOrDefaultAsync(x => x.Id == formatId);
            var transactionConceptId = format.TransactionConceptId;

            var data = payments
                .GroupBy(x => new { x.date, x.id, x.code, x.concept })
                .Select(x => new
                {
                    x.Key.id,
                    date = $"{x.Key.date.ToDefaultTimeZone():dd/MM/yyyy}",
                    datestring = $"{x.Key.date.ToDefaultTimeZone():yyyyMMdd}",
                    code = x.Key.id == transactionConceptId ? "" : x.Key.code,
                    x.Key.concept,
                    transactions = x.Key.id == transactionConceptId ? 0 : x.Count(),
                    total = x.Sum(y => y.amount),
                    isBankTransaction = x.Key.id == transactionConceptId,
                    hasInvoice = x.Any(y => y.hasInvoice)
                }).ToList();

            return data;
        }


        public async Task<List<Payment>> GetPaymentWithDataByPettyCashIdClosed(Guid id)
            => await _context.Payments
                        .Where(x => x.InvoiceId.HasValue && x.Invoice.PettyCashId == id && !x.Invoice.Annulled && x.Invoice.Canceled)
            .Include(x => x.Invoice)
            .Include(x => x.Concept.Classifier)
            .Include(x => x.Concept.AccountingPlan)
            .Include(x => x.Concept.CurrentAccount)
            .Include(x => x.CurrentAccount)
                .ToListAsync();
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> WereProceduresPaid(string userId)
        {
            var onlyCurrentPayments = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.VALIDATE_ONLY_CURRENT_PAYMENTS_ENROLLMENT));

            if (onlyCurrentPayments)
                return !await _context.Payments.AnyAsync(x => x.UserId == userId
                                && (x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)
                                && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING
                                && x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            return !await _context.Payments.AnyAsync(x => x.UserId == userId
                && (x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)
                && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING);
        }

        public async Task<bool> WereOtherProceduresPaid(string userId)
        {
            var onlyCurrentPayments = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.VALIDATE_ONLY_CURRENT_PAYMENTS_ENROLLMENT));

            if (onlyCurrentPayments)
                return !await _context.Payments.AnyAsync(x => x.UserId == userId
                && (x.Type != CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && x.Type != CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT
                 && x.Type != CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.EXONERATED_COURSE_ENROLLMENT)
                && x.Status == CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PENDING
                && x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            return !await _context.Payments.AnyAsync(x => x.UserId == userId
                && (x.Type != CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && x.Type != CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT
                 && x.Type != CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.EXONERATED_COURSE_ENROLLMENT)
                && x.Status == CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PENDING);
        }

        public async Task<List<Payment>> NotPaidProcedures()
        {
            return await _context.Payments.Where(x => (x.Type == CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT || x.Type == CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT)
                && x.Status != CORE.Helpers.ConstantHelpers.PAYMENT.STATUS.PAID).ToListAsync();
        }
        public async Task<List<Payment>> GetByUserProcedures(List<UserProcedure> userProcedures)
        {
            return await _context.Payments.Where(x => userProcedures.Any(p => p.PaymentId == x.Id)).ToListAsync();
        }
        public async Task CreateEnrollmentConceptsJob(Guid termId)
        {
            var students = await _context.Students.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.TermId == termId)).ToListAsync();

            var academicHistories = await _context.AcademicHistories.Where(x => x.TermId == termId).Include(x => x.Course).ToListAsync();

            var payments = new List<Payment>();

            //GENERAR DERECHOS DE MATRICULA
            #region Crea pagos matricula
            //Validar si esta activa la modalidad de pago previo a matricularse

            var firstTimeConceptId = Guid.Parse("25169006-167D-4ABF-F602-08D701536DFC");
            var secondTimeConceptId = Guid.Parse("7F22290A-5B72-451C-F603-08D701536DFC");
            var thirdTimeConceptId = Guid.Parse("6E82075E-5C08-49C8-F604-08D701536DFC");

            var firstTimeConcept = await _context.Concepts.FirstOrDefaultAsync(x => x.Id == firstTimeConceptId);

            if (firstTimeConcept != null)
            {
                var secondTimeConcept = await _context.Concepts.FirstOrDefaultAsync(x => x.Id == secondTimeConceptId);
                var thirdTimeConcept = await _context.Concepts.FirstOrDefaultAsync(x => x.Id == thirdTimeConceptId);

                var firstDisaprovedId = firstTimeConceptId;
                var firstDisaprovedDescription = firstTimeConcept.Description;
                var firstDisaprovedCost = firstTimeConcept.Amount;
                var firstDisaprovedSubtotal = firstTimeConcept.Amount;
                var firstDisaprovedIgv = 0.00M;
                if (firstTimeConcept.IsTaxed)
                {
                    firstDisaprovedSubtotal = firstDisaprovedCost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                    firstDisaprovedIgv = firstDisaprovedCost - firstDisaprovedSubtotal;
                }

                var secondDisaprovedId = secondTimeConceptId;
                var secondDisaprovedDescription = secondTimeConcept.Description;
                var secondDisaprovedCost = secondTimeConcept.Amount;
                var secondDisaprovedSubtotal = secondTimeConcept.Amount;
                var secondDisaprovedIgv = 0.00M;
                if (secondTimeConcept.IsTaxed)
                {
                    secondDisaprovedSubtotal = secondDisaprovedCost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                    secondDisaprovedIgv = secondDisaprovedCost - secondDisaprovedSubtotal;
                }

                var thirdDisaprovedId = thirdTimeConceptId;
                var thirdDisaprovedDescription = thirdTimeConcept.Description;
                var thirdDisaprovedCost = thirdTimeConcept.Amount;
                var thirdDisaprovedSubtotal = thirdTimeConcept.Amount;
                var thirdDisaprovedIgv = 0.00M;
                if (thirdTimeConcept.IsTaxed)
                {
                    thirdDisaprovedSubtotal = thirdDisaprovedCost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                    thirdDisaprovedIgv = thirdDisaprovedCost - thirdDisaprovedSubtotal;
                }

                var enrollmentConcepts = await _context.EnrollmentConcepts
                      .Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ADDITIONAL_CONCEPT)
                      .Include(x => x.Concept)
                      .ToListAsync();

                foreach (var student in students)
                {
                    //GENERAR DERECHO DE MATRICULA
                    if (student.Status == CORE.Helpers.ConstantHelpers.Student.States.UNBEATEN
                        || student.Status == CORE.Helpers.ConstantHelpers.Student.States.HIGH_PERFORMANCE
                        || student.Status == CORE.Helpers.ConstantHelpers.Student.States.REGULAR
                        || student.Status == CORE.Helpers.ConstantHelpers.Student.States.IRREGULAR
                        || student.Status == CORE.Helpers.ConstantHelpers.Student.States.REPEATER
                        || student.Status == CORE.Helpers.ConstantHelpers.Student.States.OBSERVED
                        || student.Status == CORE.Helpers.ConstantHelpers.Student.States.TRANSFER)
                    {
                        //GENERAR CONCEPTO REGULAR
                        foreach (var item in enrollmentConcepts.Where(x => !x.Condition.HasValue || x.Condition == student.Condition))
                        {
                            var total = item.Concept.Amount;
                            var subtotal = total;
                            var igv = 0.00M;

                            var isTaxed = item.Concept.IsTaxed;
                            var description = item.Concept.Description;
                            var conceptId = item.ConceptId;

                            if (enrollmentConcepts.Any(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ADDITIONAL_CONCEPT_REPLACEMENT
                            && x.ConceptToReplaceId == item.ConceptId && x.CareerId == student.CareerId))
                            {
                                var conceptReplace = enrollmentConcepts.FirstOrDefault(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ADDITIONAL_CONCEPT_REPLACEMENT
                                && x.ConceptToReplaceId == item.ConceptId && x.CareerId == student.CareerId);

                                total = conceptReplace.Concept.Amount;
                                subtotal = total;
                                igv = 0.00M;

                                isTaxed = conceptReplace.Concept.IsTaxed;
                                description = conceptReplace.Concept.Description;
                                conceptId = conceptReplace.ConceptId;
                            }

                            if (isTaxed)
                            {
                                subtotal = total / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                                igv = total - subtotal;
                            }

                            payments.Add(new Payment
                            {
                                Description = description,
                                SubTotal = subtotal,
                                IgvAmount = igv,
                                Discount = 0.00M,
                                Total = total,
                                EntityId = conceptId,
                                Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                UserId = student.UserId,
                                ConceptId = conceptId
                            });
                        }

                        //GENERAR CONCEPTO POR CURSOS DESAPROBADOS

                        var disaprovedCourses = academicHistories.Where(x => x.StudentId == student.Id && !x.Approved).ToList();

                        foreach (var course in disaprovedCourses)
                        {
                            if (course.Try == 1)
                            {
                                var cost = firstDisaprovedCost * course.Course.Credits;
                                var subtotal = cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                                var igv = cost - subtotal;

                                var disaprovedPayment = new Payment
                                {
                                    Description = $"{firstDisaprovedDescription} ({course.Course.Code}-{course.Course.Name})",
                                    SubTotal = subtotal,
                                    IgvAmount = igv,
                                    Discount = 0.00M,
                                    Total = cost,
                                    EntityId = firstDisaprovedId,
                                    Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = firstDisaprovedId
                                };

                                payments.Add(disaprovedPayment);
                            }

                            if (course.Try == 2)
                            {
                                var cost = secondDisaprovedCost * course.Course.Credits;
                                var subtotal = cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                                var igv = cost - subtotal;

                                var disaprovedPayment = new Payment
                                {
                                    Description = $"{secondDisaprovedDescription} ({course.Course.Code}-{course.Course.Name})",
                                    SubTotal = subtotal,
                                    IgvAmount = igv,
                                    Discount = 0.00M,
                                    Total = cost,
                                    EntityId = secondDisaprovedId,
                                    Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = secondDisaprovedId
                                };

                                payments.Add(disaprovedPayment);
                            }

                            if (course.Try == 3)
                            {
                                var cost = thirdDisaprovedCost * course.Course.Credits;
                                var subtotal = cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                                var igv = cost - subtotal;

                                var disaprovedPayment = new Payment
                                {
                                    Description = $"{thirdDisaprovedDescription} ({course.Course.Code}-{course.Course.Name})",
                                    SubTotal = subtotal,
                                    IgvAmount = igv,
                                    Discount = 0.00M,
                                    Total = cost,
                                    EntityId = thirdDisaprovedId,
                                    Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = thirdDisaprovedId
                                };

                                payments.Add(disaprovedPayment);
                            }
                        }
                    }
                }
            }
            #endregion

            await _context.Payments.AddRangeAsync(payments);

            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPartialPaymentDatatable(DataTablesStructs.SentParameters sentParameters, string search = null)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Type;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "3":
                    orderByPredicate = (x) => (x.Total - x.Payments.Sum(y => y.Total) * 1.00M);
                    break;
                case "4":
                    orderByPredicate = (x) => x.IssueDate;
                    break;
            }

            var query = _context.Payments
                .Where(x => x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING && x.IsPartialPayment)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Description.ToUpper().Contains(search.ToUpper()) ||
                                         x.User.UserName.ToUpper().Contains(search.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    type = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.Type],
                    description = x.Description,
                    userName = x.User.UserName,
                    totalAmount = x.Total - x.Payments.Sum(y => y.Total) * 1.00M,
                    issueDate = x.IssueDate.ToShortDateString()
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

        public async Task<Payment> GetWithIncludes(Guid id)
        {
            var query = _context.Payments
                .Include(x => x.Payments)
                .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Payment> GetPhantomPaymentByVoucher(DateTime date, string voucher, decimal amount)
        {
            var payments = await _context.Payments
                 .Where(x => (string.IsNullOrEmpty(x.UserId) || x.ExternalUserId == null) && x.OperationCodeB.ToUpper() == voucher.ToUpper() && x.Total == amount)
                 .ToListAsync();

            return payments
                 .Where(x => x.PaymentDate.Value.ToDefaultTimeZone().Date == date.Date)
                 .FirstOrDefault();
        }

        public async Task<Payment> GetPhantomPaymentByVoucher(string voucher, DateTime date)
        {
            return await _context.Payments
                 .Where(x => x.OperationCodeB.ToUpper() == voucher.ToUpper() && x.PaymentDate.Value.Date == date.Date)
                 .FirstOrDefaultAsync();
        }
        public async Task<List<Payment>> GetPhantomPaymentByDocument(string document)
        {
            return await _context.Payments
                 .Where(x => x.ExternalUserId.HasValue && !x.WasBankPaymentUsed && x.ExternalUser.DocumentNumber == document)
                 .ToListAsync();
        }
        public async Task<bool> AnyWithDateOperationAndTotal(DateTime datetime, string sequence, decimal amount)
            => await _context.Payments.AnyAsync(x => x.PaymentDate.Value.Date == datetime.Date && x.OperationCodeB == sequence && x.Total == amount && (string.IsNullOrEmpty(x.UserId)));
        public async Task<bool> AnyWithDateOperationAndTotalExternal(DateTime datetime, string sequence, decimal amount)
            => await _context.Payments.AnyAsync(x => x.PaymentDate.Value.Date == datetime.Date && x.OperationCodeB == sequence && x.Total == amount && x.ExternalUserId.HasValue);
        public async Task<Payment> GetByDateOperationAndTotal(DateTime datetime, string sequence, decimal amount)
            => await _context.Payments.Where(x => x.PaymentDate.Value.Date == datetime && x.OperationCodeB == sequence && x.Total == amount).FirstOrDefaultAsync();
        public async Task<DataTablesStructs.ReturnedData<object>> GetPhantomPaymentsDataTable(SentParameters sentParamters, string searchValue)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;

            switch (sentParamters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.OperationCodeB;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Type;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "3":
                    orderByPredicate = (x) => x.PaymentDate;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Total;
                    break;
            }

            var query = _context.Payments
                //.Where(x => string.IsNullOrEmpty(x.UserId) && x.ExternalUserId == null)
                .Where(x => string.IsNullOrEmpty(x.UserId))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.OperationCodeB.ToUpper().Contains(searchValue.ToUpper()) ||
                x.Description.ToUpper().Contains(searchValue.ToUpper()));
            }

            var recordsFiltered = await query.CountAsync();

            var db = await query
                .OrderByCondition(sentParamters.OrderDirection, orderByPredicate)
                .Skip(sentParamters.PagingFirstRecord)
                .Take(sentParamters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    type = x.Type,
                    description = x.Description ?? "",
                    totalAmount = x.Total,
                    issueDate = x.IssueDate.ToShortDateString() ?? "",
                    code = string.IsNullOrEmpty(x.OperationCodeB) ? "" : x.OperationCodeB,
                    paymentDate = x.PaymentDate.HasValue ? $"{x.PaymentDate:dd/MM/yyy}" : "",
                    document = x.ExternalUserId.HasValue ? x.ExternalUser.DocumentNumber : ""
                }).ToListAsync();

            var data = db.Select(x => new
            {
                x.id,
                type = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.type] ?? "",
                x.description,
                x.totalAmount,
                x.issueDate,
                x.code,
                x.paymentDate
            }).ToList();
            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParamters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        private string GetConfigurationValue(Dictionary<string, string> list, string key)
        {
            return list.ContainsKey(key) ? list[key] :

                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES.ContainsKey(key) ?
                CORE.Helpers.ConstantHelpers.Configuration.IntranetManagement.DEFAULT_VALUES[key] : "";
        }

        public async Task<object> GetStudentEnrollmentPaymentsDatatable(Guid studentId)
        {
            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.UserId,
                    x.CareerNumber,
                    x.User.Document
                }).FirstOrDefaultAsync();

            var payments = await _context.Payments
                .IgnoreQueryFilters()
                .Where(x => x.UserId == student.UserId
                && (x.Type == CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)
                //&& x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING
                && x.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                .Select(x => new
                {
                    procedureId = x.ConceptId,
                    name = x.Description,
                    cost = x.Total,// + extraCost solo ira en el base
                    unitCost = x.Concept.Amount,
                    paid = x.Status == ConstantHelpers.PAYMENT.STATUS.PAID,
                    exonerated = x.WasExonerated
                })
             .ToListAsync();

            var result = payments
                .Select(x => new
                {
                    x.procedureId,
                    x.name,
                    x.cost,
                    x.unitCost,
                    quantity = (x.unitCost > 0) ? x.cost / x.unitCost : 0,
                    x.paid,
                    x.exonerated
                }).ToList();

            result = result.OrderBy(x => x.name).ToList();
            return result;
        }

        public async Task InsertDirectedCourseStudentPayment(Guid directedCourseStudentId, string UserId)
        {
            var directedCourseConfiguration = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.DIRECTED_COURSE_COST_CONCEPT);

            if (!string.IsNullOrEmpty(directedCourseConfiguration))
            {
                var conceptId = Guid.Parse(directedCourseConfiguration);
                var concept = await _context.Concepts.FindAsync(conceptId);

                var total = concept.Amount;
                var subtotal = total;
                var igvAmount = 0.00M;

                var courseCode = await _context.StudentSections
                    .Where(x => x.Id == directedCourseStudentId)
                    .Select(x => x.Section.CourseTerm.Course.Code)
                    .FirstOrDefaultAsync();

                if (concept.IsTaxed)
                {
                    subtotal = total / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                    igvAmount = total - subtotal;
                }

                var payment = new Payment
                {
                    Description = $"{concept.Description} - {courseCode}",
                    EntityId = directedCourseStudentId,
                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                    UserId = UserId,
                    SubTotal = subtotal,
                    IgvAmount = igvAmount,
                    Discount = 0.00M,
                    Total = total,
                    ConceptId = concept.Id
                };

                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteDirectedCourseStudentPayments(Guid directedCourseStudentId)
        {
            var payments = await _context.Payments
             .Where(x => x.EntityId == directedCourseStudentId && x.Type == CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING)
             .ToListAsync();

            _context.Payments.RemoveRange(payments);
            await _context.SaveChangesAsync();
        }

        public async Task<List<IncomeReceiptTemplate>> GetIncomeReceiptReportData(DateTime start, DateTime end, int? type = null)
        {
            var qryPayments = _context.Payments.AsNoTracking();

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
                .Where(x => x.PaymentDate.HasValue && start <= x.PaymentDate.Value && x.PaymentDate.Value <= end && x.ConceptId.HasValue && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .Select(x => new
                {
                    x.Concept.AccountingPlan.Code,
                    x.Concept.AccountingPlan.Name,
                    CurrentAccount = x.CurrentAccountId.HasValue ? x.CurrentAccount.Name : "---",
                    PaymentDate = x.PaymentDate.Value,
                    x.Total,
                    Classifier = x.Concept.Classifier.Code
                })
                .ToListAsync();

            payments = payments.Where(x => x.Code != null).ToList();

            payments = payments
                .Select(x => new
                {
                    x.Code,
                    x.Name,
                    x.CurrentAccount,
                    PaymentDate = x.PaymentDate.ToDefaultTimeZone(),
                    x.Total,
                    x.Classifier
                }).ToList();

            var accountingPlanCategories = await _context.AccountingPlans
                .Where(x => x.Code.Length == 4)
                .ToListAsync();

            var days = payments
                .GroupBy(x => x.PaymentDate.Date)
                .Select(x => new IncomeReceiptTemplate
                {
                    Date = x.Key.Date,
                    Total = x.Sum(x => x.Total),
                    Categories = accountingPlanCategories
                        .Where(y => x.Any(z => z.Code.StartsWith(y.Code)))
                        .Select(y => new IncomeReceiptCategoryTemplate
                        {
                            Code = y.Code,
                            Name = y.Name,
                            Accounts = x
                                        .Where(z => z.Code.StartsWith(y.Code))
                                        .GroupBy(z => new { z.Code, z.Name })
                                        .Select(z => new IncomeReceiptCategoryDetailTemplate
                                        {
                                            Code = z.Key.Code,
                                            Name = z.Key.Name,
                                            Classifier = z.First().Classifier,
                                            Total = z.Sum(w => w.Total)
                                        }).OrderBy(z => z.Code).ToList(),
                            Total = x
                                    .Where(z => z.Code.StartsWith(y.Code))
                                    .Sum(z => z.Total)
                        }).OrderBy(y => y.Code).ToList(),
                    CurrentAccounts = x.GroupBy(y => y.CurrentAccount)
                                        .Select(y => new IncomeReceiptAccountTemplate
                                        {
                                            Key = y.Key,
                                            Total = y.Sum(z => z.Total)
                                        }).ToList()
                }).OrderBy(x => x.Date).ToList();

            return days;
        }

        public async Task ValidateUsedPayments(List<Payment> payments)
        {
            //posible uso para tramites
            var clientPayments = payments
                .Where(x => x.ExternalUserId.HasValue)
                .GroupBy(x => x.ExternalUserId)
                .Select(x => new
                {
                    ExternalUserId = x.Key,
                    Payments = x.ToList()
                }).ToList();

            var invoices = await _context.Invoices
                .Where(x => x.PaymentType == ConstantHelpers.Treasury.Invoice.PaymentType.VOUCHER
                && !string.IsNullOrEmpty(x.Voucher) && x.VoucherDate.HasValue
                && !x.Payments.Any(y => y.WasBankPaymentUsed))
                .ToListAsync();

            foreach (var item in clientPayments)
            {
                var externalUser = await _context.ExternalUsers.FindAsync(item.ExternalUserId);

                //check admission payments
                var previousPayment = bool.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.AdmissionManagement.PAYMENT_BANK));

                if (!previousPayment)
                {
                    var postulant = await _context.Postulants
                        .Where(x => x.Document == externalUser.DocumentNumber && x.ApplicationTerm.Status == ConstantHelpers.TERM_STATES.ACTIVE)
                        .FirstOrDefaultAsync();
                    if (postulant != null)
                    {
                        var pendingPayments = await _context.Payments
                        .Where(x => x.Type == ConstantHelpers.PAYMENT.TYPES.POSTULANT_PAYMENT && x.Status == ConstantHelpers.PAYMENT.STATUS.PENDING)
                        .ToListAsync();

                        if (item.Payments.Sum(x => x.Total) >= pendingPayments.Sum(x => x.Total))
                        {
                            postulant.PaidAdmission = true;

                            var startDate = item.Payments.OrderBy(x => x.IssueDate).Select(x => x.IssueDate).FirstOrDefault();
                            var endDate = item.Payments.OrderByDescending(x => x.IssueDate).Select(x => x.IssueDate).FirstOrDefault();
                            var phantomPayments = await _context.Payments
                                .Where(x => x.ExternalUserId == item.ExternalUserId && x.IsBankPayment && !x.WasBankPaymentUsed
                                && startDate <= x.IssueDate && x.IssueDate <= endDate)
                                .ToListAsync();

                            foreach (var payment in phantomPayments)
                            {
                                payment.Type = ConstantHelpers.PAYMENT.TYPES.POSTULANT_PAYMENT;
                                payment.EntityId = postulant.Id;
                                payment.WasBankPaymentUsed = true;
                            }

                            _context.Payments.RemoveRange(pendingPayments);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                foreach (var payment in item.Payments)
                {
                    var invoice = invoices.FirstOrDefault(x =>
                        x.Voucher == payment.OperationCodeB
                        && x.VoucherDate.Value.Date == payment.PaymentDate.ToDefaultTimeZone().Value.Date
                        && x.TotalAmount <= payment.Total);

                    if (invoice != null)
                    {
                        var invoiceNumber = $"{invoice.Series} - {invoice.Number.ToString().PadLeft(ConstantHelpers.Treasury.Invoice.CORRELATIVE_PADLEFT, '0')}";

                        var invoicePayments = await _context.Payments.Where(x => x.InvoiceId == invoice.Id).ToListAsync();
                        _context.Payments.RemoveRange(invoicePayments);

                        var phantomPayment = await _context.Payments.FindAsync(payment.Id);
                        phantomPayment.WasBankPaymentUsed = true;
                        phantomPayment.Type = invoicePayments.FirstOrDefault().Type;
                        phantomPayment.InvoiceId = invoice.Id;

                        var prevIncomes = await _context.Incomes.Where(x => x.Invoice == invoiceNumber).ToListAsync();
                        _context.Incomes.RemoveRange(prevIncomes);

                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task<ReturnedData<object>> GetPaymentByUserDatatable(DataTablesStructs.SentParameters sentParameters, string UserId, int? status = null, string searchValue = null)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Description);
                    break;
                default:
                    orderByPredicate = ((x) => x.Status);
                    break;
            }

            var query = _context.Payments.Where(x => x.UserId == UserId).AsNoTracking();

            if (status != null)
                query = query.Where(x => x.Status == status);

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    Type = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION.ContainsKey(x.Type) ? ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.Type] : "-",
                    x.Description,
                    PaymentDate = x.PaymentDate.HasValue ? x.PaymentDate.Value.ToLocalDateTimeFormat() : "-",
                    x.Total,
                    x.Status,
                    Term = x.TermId.HasValue ? x.Term.Name : "-"
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

        public async Task GenerateStudentReentryPayments(Guid studentId)
        {
            var enrollmentPaymentMethod = byte.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.ENROLLMENT_PAYMENT_METHOD));
            if (enrollmentPaymentMethod == 2)
                return;

            var reentryConceptConfiguration = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REENTRY_COST_CONCEPT);
            if (string.IsNullOrEmpty(reentryConceptConfiguration)) return;

            var reentryConceptId = Guid.Parse(reentryConceptConfiguration);
            var reentryConcept = await _context.Concepts.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == reentryConceptId);
            var reentryCost = reentryConcept.Amount;
            var reentrySubTotal = reentryCost;
            var reentryIgvAmount = 0.00M;
            if (reentryConcept.IsTaxed)
            {
                reentrySubTotal = reentryCost / (1.00M + ConstantHelpers.Treasury.IGV);
                reentryIgvAmount = reentryCost - reentrySubTotal;
            }

            var reservationConceptId = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_PROCEDURE));
            var reservationConcept = await _context.Concepts.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == reservationConceptId);

            var reservationIsRenewable = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_RENEWABLE_PER_SEMESTER));
            //var reservationTimeLimit = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_TIME_LIMIT));

            var terms = await _context.Terms.ToListAsync();
            var term = terms.FirstOrDefault(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);

            var student = await _context.Students
                .Where(x => x.Id == studentId)
                .Select(x => new
                {
                    x.Id,
                    x.CurriculumId,
                    x.UserId,
                    x.Status,
                    x.CareerId,
                    x.Career.FacultyId,
                    x.AdmissionTypeId
                }).FirstOrDefaultAsync();

            var academicYearCourses = await _context.AcademicYearCourses
                        .Where(x => x.CurriculumId == student.CurriculumId)
                        .Select(x => new
                        {
                            x.AcademicYear,
                            x.CourseId
                        })
                        .ToListAsync();

            var academicHistories = await _context.AcademicHistories
                .Where(x => x.StudentId == student.Id)
                .Select(x => new
                {
                    x.StudentId,
                    x.CourseId,
                    x.Course.Credits,
                    x.TermId,
                    TermYear = x.Term.Year,
                    TermNumber = x.Term.Number,
                    TermName = x.Term.Name,
                    x.Approved,
                    x.Validated,
                    x.Term.StartDate,
                    x.Try,
                    x.Course.Code
                }).ToListAsync();

            var curriculumAcademicHistories = academicHistories
                .Where(x => academicYearCourses.Any(y => y.CourseId == x.CourseId))
                .ToList();

            var academicSummaries = curriculumAcademicHistories
                          .Where(x => !x.Validated && (x.TermNumber == "1" || x.TermNumber == "2"))
                          .GroupBy(x => new { x.TermYear, x.TermNumber, x.StartDate, x.TermName })
                          .Select(x => new
                          {
                              Year = x.Key.TermYear,
                              Number = x.Key.TermNumber,
                              Courses = x.ToList(),
                              x.Key.TermName,
                              x.Key.StartDate,
                              DisapprovedCredits = x.Where(y => !y.Approved).Sum(y => y.Credits)
                          }).ToList();

            var lastSummary = academicSummaries.OrderBy(x => x.Year).ThenBy(x => x.Number).ThenBy(x => x.StartDate).LastOrDefault();
            if (lastSummary == null) return;

            var oldPayments = await _context.Payments
                .Where(x => x.UserId == student.UserId && x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT)
                .ToListAsync();
            _context.Payments.RemoveRange(oldPayments);
            await _context.SaveChangesAsync();

            var payments = new List<Payment>();

            var disapprovedCourseConcepts = await _context.DisapprovedCourseConcepts
              .IgnoreQueryFilters()
              .Include(x => x.Concept)
              .ToListAsync();


            var calculateAcademicYears = terms.Where(x => (x.Number == "1" || x.Number == "2") && lastSummary.StartDate < x.StartDate && x.StartDate < term.StartDate).Count();

            if (reservationIsRenewable)
            {
                var enrollmentReservations = await _context.EnrollmentReservations
                    .Where(x => x.StudentId == student.Id)
                    .Select(x => x.TermId)
                    .ToListAsync();

                calculateAcademicYears = terms.Where(x => (x.Number == "1" || x.Number == "2") && lastSummary.StartDate < x.StartDate && x.StartDate < term.StartDate && !enrollmentReservations.Contains(x.Id)).Count();
            }

            if (lastSummary.Courses.Any(x => x.Try >= 3)) calculateAcademicYears = calculateAcademicYears - 1;

            if (calculateAcademicYears > 0)
            {
                var total = calculateAcademicYears * reentryConcept.Amount;
                var subtotal = total;
                var igv = 0.00M;
                if (reentryConcept.IsTaxed)
                {
                    subtotal = total / (1.00M + ConstantHelpers.Treasury.IGV);
                    igv = total - subtotal;
                }

                payments.Add(new Payment
                {
                    Description = reentryConcept.Description,
                    SubTotal = subtotal,
                    IgvAmount = igv,
                    Discount = 0.00M,
                    Total = total,
                    EntityId = Guid.Empty,
                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                    UserId = student.UserId,
                    ConceptId = reentryConcept.Id,
                    TermId = term.Id
                });
            }

            if (ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMAD
                && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAJMA
                && ConstantHelpers.GENERAL.Institution.Value != ConstantHelpers.Institution.UNAMBA)
            {
                var disapprovedCourses = lastSummary.Courses.Where(x => !academicHistories.Any(y => y.CourseId == x.CourseId && y.Approved)).ToList();

                foreach (var disapprovedCourse in disapprovedCourses)
                {
                    var lastTry = academicHistories
                        .Where(x => x.CourseId == disapprovedCourse.CourseId)
                        .OrderBy(x => x.Try)
                        .Select(x => x.Try)
                        .Last();

                    var concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == disapprovedCourse.Try + 1 && x.AdmissionTypeId == student.AdmissionTypeId);
                    if (concept == null)
                        concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == disapprovedCourse.Try + 1);
                    if (concept == null) continue;

                    var total = concept.IsCostPerCredit ? disapprovedCourse.Credits * concept.Concept.Amount : concept.Concept.Amount;
                    var subtotal = total;
                    var igv = 0.00M;
                    if (concept.Concept.IsTaxed)
                    {
                        subtotal = total / (1.00M + ConstantHelpers.Treasury.IGV);
                        igv = total - subtotal;
                    }

                    payments.Add(new Payment
                    {
                        Description = $"{concept.Concept.Description} - {disapprovedCourse.Code}",
                        SubTotal = subtotal,
                        IgvAmount = igv,
                        Discount = 0.00M,
                        Total = total,
                        EntityId = disapprovedCourse.CourseId,
                        Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                        UserId = student.UserId,
                        ConceptId = concept.Concept.Id,
                        TermId = term.Id
                    });
                }
            }

            _context.Payments.AddRange(payments);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Payment>> GetPaymentsByUser(string userId, byte? status, Guid? conceptId)
        {
            var query = _context.Payments.Where(x => x.UserId == userId).AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            if (conceptId.HasValue)
                query = query.Where(x => x.ConceptId == conceptId);

            return await query.Include(x => x.Term).Include(x => x.Invoice).Include(x => x.Payments).ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByClientId(Guid clientId, byte? status)
        {
            var query = _context.Payments
                .Where(x => x.ExternalUserId == clientId)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            return await query.Include(x => x.Term).Include(x => x.Invoice).Include(x => x.Payments).ToListAsync();
        }

        public async Task UpdateStudentEnrollmentPayments(Guid studentId)
        {
            await CreateStudentEnrollmentPayments(studentId);
        }

        public async Task<ReturnedData<object>> GetExoneratedPaymentsToUserDatatable(SentParameters sentParameters, string userName, string searchValue = null)
        {
            var query = _context.Payments
                .Where(x => x.User.UserName == userName && x.WasExonerated && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .AsNoTracking();

            if (!String.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.IssueDate)
                .Select(x => new
                {
                    id = x.Id,
                    type = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION.ContainsKey(x.Type) ? ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.Type] : "",
                    concept = x.IsPartialPayment ? $"{x.Description} - Pago Parcial" : x.Description,
                    totalamount = x.IsPartialPayment ? (x.Total - x.Payments.Sum(y => y.Total) * 1.00M) : x.Total * 1.00M,
                    discount = x.Discount,
                    isPartialPayment = x.IsPartialPayment,
                    issueDate = x.IssueDate.ToShortDateString(),
                    paymentDate = x.PaymentDate.HasValue ? x.PaymentDate.Value.ToShortDateString() : "-",
                    isConcept = ConstantHelpers.PAYMENT.TYPES.CONCEPT == x.Type
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


        public async Task GenerateStudentEnrollmentPayments()
        {
            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Status == ConstantHelpers.TERM_STATES.ACTIVE);
            if (term == null) return;

            //-------- OBTENER LOS CONCEPTOS ---------------
            var regularConceptId = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REGULAR_ENROLLMENT_PROCEDURE));

            var exonerateUnbeatenStudents = bool.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXONERATE_REGULAR_STUDENTS_FROM_PAYMENT));
            var unbeatenConceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.REGULAR_ENROLLMENT_PROCEDURE));
            if (!exonerateUnbeatenStudents) unbeatenConceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.UNBEATEN_STUDENT_ENROLLMENT_PROCEDURE));

            var unbeatenConcept = await _context.Concepts.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == unbeatenConceptId);
            var unbeatenEnrollmentCost = unbeatenConcept.Amount;
            var unbeatenEnrollmentSubTotal = unbeatenEnrollmentCost;
            var unbeatenEnrollmentIgvAmount = 0.00M;
            if (unbeatenConcept.IsTaxed)
            {
                unbeatenEnrollmentSubTotal = unbeatenEnrollmentCost / (1.00M + ConstantHelpers.Treasury.IGV);
                unbeatenEnrollmentIgvAmount = unbeatenEnrollmentCost - unbeatenEnrollmentSubTotal;
            }

            var disapprovedCourseConcepts = await _context.DisapprovedCourseConcepts.IgnoreQueryFilters().Include(x => x.Concept).ToListAsync();
            var admissionConceptId = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.ADMISSION_ENROLLMENT_PROCEDURE));

            //CONCEPTOS UNAMAD
            var oneDisapprovedCourseConceptId = new Guid("666892be-c64e-4851-6b67-08d8672e2958");
            var twoDisapprovedCourseConceptId = new Guid("b543dbd4-3461-4d8e-6b68-08d8672e2958");
            var threeDisapprovedCourseConceptId = new Guid("4513db1d-6ec2-47d5-6b69-08d8672e2958");

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA)
            {
                oneDisapprovedCourseConceptId = new Guid("79F67FA8-F3A2-1DE7-9181-A67B7542122C");
                twoDisapprovedCourseConceptId = new Guid("7F3BDAE4-CEBB-4523-D777-2B0674A318D5");
                threeDisapprovedCourseConceptId = new Guid("1C097916-885A-AF0F-6FB5-E6087EB1B2DC");
            }

            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMBA)
            {
                oneDisapprovedCourseConceptId = new Guid("08db525e-6632-4fa5-83bb-2894609f966d");
                twoDisapprovedCourseConceptId = new Guid("08db525e-7661-48f7-8251-1831cde8aea8");
                threeDisapprovedCourseConceptId = new Guid("08db525e-89ea-430b-85c8-2423cb117313");
            }

            var enrollmentConcepts = await _context.EnrollmentConcepts
               .IgnoreQueryFilters()
               .Select(x => new
               {
                   x.ConceptId,
                   x.Concept.Description,
                   x.Concept.Amount,
                   x.Concept.IsTaxed,
                   x.Type,
                   x.CareerId,
                   x.AdmissionTypeId,
                   x.Condition,
                   x.ConceptToReplaceId,
                   ConceptToReplace = x.ConceptToReplaceId.HasValue ? x.ConceptToReplace.Description : ""
               }).ToListAsync();

            var enableExtraPayment = bool.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.ENABLE_ADDITIONAL_PAYMENT_FOR_EXTRA_ACADEMIC_YEARS));
            var extraPaymentConceptId = Guid.Empty;
            var gracePeriod = int.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXTRA_ACADEMIC_YEARS_GRACE_PERIOD));

            if (enableExtraPayment)
                extraPaymentConceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.ADDITIONAL_PAYMENT_FOR_EXTRA_ACADEMIC_YEARS_CONCEPT));

            var regularTerm = await _context.Terms
                  .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2"))
                  .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number)
                  .FirstOrDefaultAsync();

            var summerTerm = await _context.Terms
                .Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && x.IsSummer)
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Number).FirstOrDefaultAsync();

            var validateSummer = false;

            if (regularTerm == null && summerTerm != null) validateSummer = true;
            else if (summerTerm == null) validateSummer = false;
            else validateSummer = summerTerm.StartDate > regularTerm.StartDate;

            var isExtemporaneous = term.EnrollmentEndDate.ToDefaultTimeZone().Date < DateTime.UtcNow.ToDefaultTimeZone().Date;
            var extemporaneousEnrollmentconceptId = Guid.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXTEMPORANEOUS_ENROLLMENT_SURCHARGE_PROCEDURE));
            var extemporaneousEnrollmentModality = byte.Parse(await GetConfigurationValue(CORE.Helpers.ConstantHelpers.Configuration.Enrollment.EXTEMPORANEOUS_ENROLLMENT_MODALITY));

            var students = await _context.Students
                .FilterActiveStudents()
                //.Where(x => x.Status == ConstantHelpers.Student.States.ENTRANT)
                .Where(x => !x.User.Payments.Any(y => y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && y.TermId == term.Id && y.Status == ConstantHelpers.PAYMENT.STATUS.PAID))
                .Select(x => new
                {
                    x.Id,
                    x.CareerId,
                    x.Career.FacultyId,
                    x.UserId,
                    x.CurriculumId,
                    x.Status,
                    x.AdmissionTypeId,
                    x.AdmissionType.IsExoneratedEnrollment,
                    AdmissionTypeAbbrev = x.AdmissionType.Abbreviation,
                    AdmissionTermStartDate = x.AdmissionTerm.StartDate,
                    x.Condition,
                    x.Benefit,
                    x.CurrentAcademicYear
                })
                .ToListAsync();

            var oldPayments = await _context.Payments
                .Where(x => x.TermId == term.Id
                && (x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT)
                && x.Status != ConstantHelpers.PAYMENT.STATUS.PAID)
                .ToListAsync();
            oldPayments = oldPayments.Where(x => students.Any(y => y.UserId == x.UserId)).ToList();
            _context.RemoveRange(oldPayments);

            var allAcademicHistories = await _context.AcademicHistories
                .Where(x => !x.Student.User.Payments.Any(y => y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && y.TermId == term.Id && y.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                && !x.Withdraw)
                .Select(x => new
                {
                    x.StudentId,
                    x.CourseId,
                    x.Approved,
                    x.Try,
                    x.TermId,
                    x.Course.Credits,
                    x.Course.Code,
                    x.Validated,
                    TermNumber = x.Term.Number,
                    TermName = x.Term.Name,
                    TermYear = x.Term.Year,
                    x.Term.StartDate
                }).ToListAsync();

            var allAcademicYearCourses = await _context.AcademicYearCourses
                .Select(x => new
                {
                    x.Curriculum.CareerId,
                    x.CurriculumId,
                    x.CourseId,
                    x.AcademicYear
                }).ToListAsync();

            var allAcademicSummaries = await _context.AcademicSummaries
                .Where(x => !x.WasWithdrawn && !x.Term.IsSummer
                && !x.Student.User.Payments.Any(y => y.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && y.TermId == term.Id && y.Status == ConstantHelpers.PAYMENT.STATUS.PAID))
                .Select(x => new
                {
                    x.StudentId,
                    x.CurriculumId
                }).ToListAsync();

            var careers = await _context.Careers
                .Select(x => new
                {
                    x.Id,
                    x.FacultyId
                })
                .ToListAsync();

            var studentSections = await _context.StudentSections
                .Where(x => x.Section.CourseTerm.TermId == term.Id)
                .Select(x => new
                {
                    x.StudentId,
                    x.SectionId,
                    x.Section.CourseTerm.Course.Credits
                }).ToListAsync();

            #region Alumnos exonerados
            var exemptedStudents = new List<Guid>();

            var exemptFirstPlaces = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.EXEMPT_FIRST_PLACES_FROM_PAYMENTS));
            if (exemptFirstPlaces)
            {
                var firstPlaceQuantity = int.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.FIRST_PLACES_QUANTITY));
                var exemptType = byte.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.PAYMENT_EXEMPTION_TYPE));

                if (exemptType == 1)
                {
                    foreach (var career in careers)
                    {
                        var academicSummaries = await _context.AcademicSummaries
                            .Where(x => x.TermId == regularTerm.Id && x.CareerId == career.Id)
                            .OrderByDescending(x => x.WeightedAverageGrade)
                            .Select(x => new
                            {
                                x.StudentId,
                                x.WeightedAverageGrade,
                                x.CareerId,
                                x.Career.FacultyId,
                                x.StudentAcademicYear
                            }).ToListAsync();

                        var cont = 0;
                        var lastGrade = -1.0M;
                        var i = 0;

                        while (cont < firstPlaceQuantity && i < students.Count())
                        {
                            exemptedStudents.Add(academicSummaries[i].StudentId);

                            if (lastGrade != academicSummaries[i].WeightedAverageGrade)
                            {
                                lastGrade = academicSummaries[i].WeightedAverageGrade;
                                cont++;
                            }

                            i++;
                        }
                    }

                }
                else if (exemptType == 2)
                {
                    foreach (var career in careers)
                    {
                        var academicSummaries = await _context.AcademicSummaries
                            .Where(x => x.TermId == regularTerm.Id && x.CareerId == career.Id)
                            .Select(x => new
                            {
                                x.StudentId,
                                x.WeightedAverageGrade,
                                x.CareerId,
                                x.Career.FacultyId,
                                x.StudentAcademicYear
                            }).ToListAsync();

                        var academicYears = allAcademicYearCourses
                            .Where(x => x.CareerId == career.Id)
                            .Select(x => x.AcademicYear)
                            .Distinct()
                            .ToList();

                        foreach (var academicYear in academicYears)
                        {
                            var yearStudents = academicSummaries
                                .Where(x => x.StudentAcademicYear == academicYear)
                                .OrderByDescending(x => x.WeightedAverageGrade)
                                .ToArray();

                            var cont = 0;
                            var lastGrade = -1.0M;
                            var i = 0;

                            while (cont < firstPlaceQuantity && i < yearStudents.Count())
                            {
                                exemptedStudents.Add(yearStudents[i].StudentId);

                                if (lastGrade != yearStudents[i].WeightedAverageGrade)
                                {
                                    lastGrade = yearStudents[i].WeightedAverageGrade;
                                    cont++;
                                }

                                i++;
                            }
                        }
                    }

                }
                else if (exemptType == 3)
                {
                    var faculties = careers.Select(x => x.FacultyId).Distinct().ToList();

                    foreach (var faculty in faculties)
                    {
                        var academicSummaries = await _context.AcademicSummaries
                            .Where(x => x.TermId == regularTerm.Id && x.Career.FacultyId == faculty)
                            .OrderByDescending(x => x.WeightedAverageGrade)
                            .Select(x => new
                            {
                                x.StudentId,
                                x.WeightedAverageGrade,
                                x.CareerId,
                                x.Career.FacultyId,
                                x.StudentAcademicYear
                            }).ToListAsync();

                        var cont = 0;
                        var lastGrade = -1.0M;
                        var i = 0;

                        while (cont < firstPlaceQuantity && i < academicSummaries.Count())
                        {
                            exemptedStudents.Add(academicSummaries[i].StudentId);

                            if (lastGrade != academicSummaries[i].WeightedAverageGrade)
                            {
                                lastGrade = academicSummaries[i].WeightedAverageGrade;
                                cont++;
                            }

                            i++;
                        }
                    }
                }
            }

            #endregion

            var terms = await _context.Terms.Where(x => x.Status == ConstantHelpers.TERM_STATES.FINISHED && (x.Number == "1" || x.Number == "2")).ToListAsync();

            var reentryConceptIdString = await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REENTRY_COST_CONCEPT);
            var reentryConceptId = (Guid?)null;
            Concept reentryConcept = null;

            if (!string.IsNullOrEmpty(reentryConceptIdString))
            {
                reentryConceptId = Guid.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.REENTRY_COST_CONCEPT));
                reentryConcept = await _context.Concepts.FindAsync(reentryConceptId);
            }

            //var enrollmentFeeStudents = new List<StudentEnrollmentFee>();
            var payments = new List<Payment>();
            var concepts = await _context.Concepts.IgnoreQueryFilters().ToListAsync();
            //var enrollmentFees = await _context.EnrollmentFees.Where(x => x.TermId == term.Id).ToListAsync();

            var reservationIsRenewable = bool.Parse(await GetConfigurationValue(ConstantHelpers.Configuration.Enrollment.RESERVATION_RENEWABLE_PER_SEMESTER));
            var allEnrollmentReservations = await _context.EnrollmentReservations.ToListAsync();

            foreach (var student in students)
            {
                var isExempted = exemptedStudents.Contains(student.Id);

                var discountPercentage = 0;
                if (student.Benefit != ConstantHelpers.Student.Benefit.NONE)
                    discountPercentage = ConstantHelpers.Student.Benefit.DISCOUNTS[student.Benefit];

                var academicYearCourses = allAcademicYearCourses
                    .Where(x => x.CurriculumId == student.CurriculumId)
                    .ToList();
                var createRegularConcept = true;

                if (isExtemporaneous && !studentSections.Any(x => x.StudentId == student.Id))
                {
                    if (extemporaneousEnrollmentModality == 2) createRegularConcept = false;

                    var conceptId = extemporaneousEnrollmentconceptId;

                    var extemporaneousConcept = enrollmentConcepts
                    .OrderByDescending(x => x.CareerId)
                    .ThenByDescending(x => x.AdmissionTypeId)
                    .FirstOrDefault(x => (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                    && (!x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId)
                    && x.Type == ConstantHelpers.EnrollmentConcept.Type.EXTEMPORANEOUS_ENROLLMENT_CONCEPT);
                    if (extemporaneousConcept != null) conceptId = extemporaneousConcept.ConceptId;

                    var concept = await _context.Concepts.FindAsync(conceptId);

                    var discount = isExempted ? 0.00M : concept.Amount * discountPercentage / 100.0M;

                    var total = isExempted ? 0.00M : concept.Amount - discount;
                    var subtotal = total;
                    var igv = 0.00M;

                    if (concept.IsTaxed)
                    {
                        subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                        igv = total + discount - subtotal;
                    }

                    payments.Add(new Payment
                    {
                        Description = concept.Description,
                        SubTotal = subtotal,
                        IgvAmount = igv,
                        Discount = discount,
                        Total = total,
                        EntityId = null,
                        Type = ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT,
                        UserId = student.UserId,
                        ConceptId = concept.Id,
                        TermId = term.Id,
                        CurrentAccountId = concept.CurrentAccountId
                    });
                }

                if (student.Status == ConstantHelpers.Student.States.TRANSFER || student.Status == ConstantHelpers.Student.States.ENTRANT)
                {
                    if (createRegularConcept)
                    {
                        var studentConceptId = admissionConceptId;

                        var specialConcept = enrollmentConcepts
                            .OrderByDescending(x => x.CareerId)
                            .ThenByDescending(x => x.AdmissionTypeId)
                            .ThenByDescending(x => x.Condition)
                            .FirstOrDefault(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ENROLLMENT_CONCEPT
                            && (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                            && (x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId)
                            && (!x.Condition.HasValue || x.Condition == student.Condition));

                        if (specialConcept != null) studentConceptId = specialConcept.ConceptId;

                        var concept = concepts.FirstOrDefault(x => x.Id == studentConceptId);

                        var discount = concept.Amount * discountPercentage / 100.0M;

                        var enrollmentCost = concept.Amount;
                        //if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNCP
                        //    && (student.Status == ConstantHelpers.Student.States.REPEATER
                        //    || student.Condition == ConstantHelpers.Student.Condition.TRANSITORY_PAYER
                        //    || student.Condition == ConstantHelpers.Student.Condition.PERMANENT_PAYER))
                        //{
                        //    enrollmentCost += 30.0M;
                        //}
                        enrollmentCost -= discount;
                        var enrollmentSubTotal = enrollmentCost;
                        var enrollmentIgvAmount = 0.00M;

                        if (concept.IsTaxed)
                        {
                            enrollmentSubTotal = discount + enrollmentCost / (1.00M + ConstantHelpers.Treasury.IGV);
                            enrollmentIgvAmount = enrollmentCost + discount - enrollmentSubTotal;
                        }

                        //if (enrollmentFees.Any(x => x.AdmissionTypeId == student.AdmissionTypeId) && !student.IsExoneratedEnrollment)
                        //    enrollmentFeeStudents.Add(new StudentEnrollmentFee
                        //    {
                        //        Amount = enrollmentCost,
                        //        ConceptId = studentConceptId,
                        //        StudentId = student.Id,
                        //        TermId = term.Id
                        //    });
                        //else
                        payments.Add(new Payment
                        {
                            Id = Guid.NewGuid(),
                            Description = concept.Description,
                            SubTotal = student.IsExoneratedEnrollment ? 0.00M : enrollmentSubTotal,
                            IgvAmount = student.IsExoneratedEnrollment ? 0.00M : enrollmentIgvAmount,
                            Discount = student.IsExoneratedEnrollment ? 0.00M : discount,
                            Total = student.IsExoneratedEnrollment ? 0.00M : enrollmentCost,
                            EntityId = Guid.Empty,
                            Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId,
                            ConceptId = studentConceptId,
                            TermId = term.Id
                        });
                    }

                    //GENERAR CONCEPTO ADMISION
                    foreach (var item in enrollmentConcepts.Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ADDITIONAL_CONCEPT && (!x.Condition.HasValue || x.Condition == student.Condition)))
                    {
                        var discount = item.Amount * discountPercentage / 100.0M;

                        var total = item.Amount - discount;
                        var subtotal = total;
                        var igv = 0.00M;

                        var isTaxed = item.IsTaxed;
                        var description = item.Description;
                        var conceptId = item.ConceptId;

                        if (enrollmentConcepts.Any(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ADDITIONAL_CONCEPT_REPLACEMENT
                        && x.ConceptToReplaceId == item.ConceptId && x.CareerId == student.CareerId))
                        {
                            var conceptReplace = enrollmentConcepts.FirstOrDefault(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.ADMISSION_ADDITIONAL_CONCEPT_REPLACEMENT
                            && x.ConceptToReplaceId == item.ConceptId && x.CareerId == student.CareerId);

                            total = isExempted ? 0.00M : conceptReplace.Amount;
                            discount = total * discountPercentage / 100.0M;
                            total -= discount;
                            subtotal = total;
                            igv = 0.00M;

                            isTaxed = conceptReplace.IsTaxed;
                            description = conceptReplace.Description;
                            conceptId = conceptReplace.ConceptId;
                        }

                        if (isTaxed)
                        {
                            subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                            igv = total + discount - subtotal;
                        }

                        payments.Add(new Payment
                        {
                            Id = Guid.NewGuid(),
                            Description = description,
                            SubTotal = subtotal,
                            IgvAmount = igv,
                            Discount = discount,
                            Total = total,
                            EntityId = Guid.Empty,
                            Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId,
                            ConceptId = conceptId,
                            TermId = term.Id
                        });
                    }
                }
                else
                {
                    var academicHistories = allAcademicHistories
                        .Where(x => x.StudentId == student.Id)
                        .ToList();

                    var allSummaries = academicHistories
                             .Where(x => !x.Validated && (x.TermNumber == "1" || x.TermNumber == "2"))
                             .GroupBy(x => new { x.TermYear, x.TermNumber, x.StartDate, x.TermName })
                             .Select(x => new
                             {
                                 Year = x.Key.TermYear,
                                 Number = x.Key.TermNumber,
                                 x.Key.TermName,
                                 x.Key.StartDate,
                                 Courses = x.ToList()
                             }).ToList();

                    var lastSummary = allSummaries.OrderBy(x => x.Year).ThenBy(x => x.Number).ThenBy(x => x.StartDate).LastOrDefault();

                    if (lastSummary != null && lastSummary.TermName != regularTerm.Name)
                    {
                        var calculateAcademicYears = terms.Where(x => lastSummary.StartDate < x.StartDate && x.StartDate < term.StartDate).Count();
                        if (lastSummary.Courses.Any(x => x.Try >= 3 && !x.Approved)) calculateAcademicYears -= 1;

                        if (reservationIsRenewable)
                        {
                            var enrollmentReservations = allEnrollmentReservations
                                .Where(x => x.StudentId == student.Id)
                                .Select(x => x.TermId)
                                .ToList();

                            calculateAcademicYears = terms.Where(x => lastSummary.StartDate < x.StartDate && x.StartDate < term.StartDate && !enrollmentReservations.Contains(x.Id)).Count();
                        }

                        if (reentryConcept != null && calculateAcademicYears > 0)
                        {
                            var total = calculateAcademicYears * reentryConcept.Amount;
                            var discount = total * discountPercentage / 100.0M;

                            total = total - discount;
                            var subtotal = total;
                            var igv = 0.00M;
                            if (reentryConcept.IsTaxed)
                            {
                                subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                igv = total + discount - subtotal;
                            }

                            payments.Add(new Payment
                            {
                                Id = Guid.NewGuid(),
                                Description = reentryConcept.Description,
                                SubTotal = subtotal,
                                IgvAmount = igv,
                                Discount = discount,
                                Total = total,
                                EntityId = Guid.Empty,
                                Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                UserId = student.UserId,
                                ConceptId = reentryConcept.Id,
                                TermId = term.Id
                            });
                            createRegularConcept = true;
                        }

                        var disapprovedCourses = lastSummary.Courses.Where(x => !academicHistories.Any(y => y.CourseId == x.CourseId && y.Approved)).ToList();

                        if (disapprovedCourses.Any())
                        {
                            createRegularConcept = true;

                            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD
                                || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA
                                || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMBA)
                            {
                                var concept = new Concept();
                                switch (disapprovedCourses.Count)
                                {
                                    case 1:
                                        concept = concepts.FirstOrDefault(x => x.Id == oneDisapprovedCourseConceptId);
                                        break;
                                    case 2:
                                        concept = concepts.FirstOrDefault(x => x.Id == twoDisapprovedCourseConceptId);
                                        break;
                                    default:
                                        concept = concepts.FirstOrDefault(x => x.Id == threeDisapprovedCourseConceptId);
                                        break;
                                }

                                var totalCredits = disapprovedCourses.Sum(x => x.Credits);

                                var total = isExempted ? 0.00M : totalCredits * concept.Amount;
                                var discount = total * discountPercentage / 100.0M;

                                total -= discount;
                                var subtotal = total;
                                var igv = 0.00M;
                                if (concept.IsTaxed)
                                {
                                    subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                    igv = total + discount - subtotal;
                                }

                                var coursePayment = new Payment
                                {
                                    Id = Guid.NewGuid(),
                                    Description = $"{concept.Description} {totalCredits} cred.",
                                    SubTotal = subtotal,
                                    IgvAmount = igv,
                                    Discount = discount,
                                    Total = total,
                                    EntityId = null,
                                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = concept.Id,
                                    TermId = term.Id
                                };

                                payments.Add(coursePayment);
                            }
                            else
                            {
                                foreach (var item in disapprovedCourses)
                                {
                                    var lastTry = academicHistories
                                          .Where(x => x.CourseId == item.CourseId)
                                          .OrderBy(x => x.Try)
                                          .Select(x => x.Try)
                                          .Last();

                                    var concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == lastTry + 1 && x.AdmissionTypeId == student.AdmissionTypeId);
                                    if (concept == null)
                                        concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == lastTry + 1);
                                    if (concept == null) continue;

                                    var total = concept.IsCostPerCredit ? item.Credits * concept.Concept.Amount : concept.Concept.Amount;
                                    var discount = total * discountPercentage / 100.0M;

                                    total -= discount;
                                    var subtotal = total;
                                    var igv = 0.00M;
                                    if (concept.Concept.IsTaxed)
                                    {
                                        subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                        igv = total + discount - subtotal;
                                    }

                                    payments.Add(new Payment
                                    {
                                        Id = Guid.NewGuid(),
                                        Description = $"{concept.Concept.Description} - {item.Code}",
                                        SubTotal = subtotal,
                                        IgvAmount = igv,
                                        Discount = discount,
                                        Total = total,
                                        EntityId = item.CourseId,
                                        Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                        UserId = student.UserId,
                                        ConceptId = concept.Concept.Id,
                                        TermId = term.Id
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        var curriculumCourses = academicYearCourses
                        .Where(x => x.CurriculumId == student.CurriculumId)
                        .Select(x => x.CourseId)
                        .ToHashSet();

                        var regularDisapprovedCourses = academicHistories
                            .Where(x => curriculumCourses.Contains(x.CourseId) && !x.Approved && regularTerm != null && x.TermId == regularTerm.Id)
                            .ToList();

                        if (validateSummer)
                        {
                            var summerCourses = academicHistories
                                .Where(x => x.TermId == summerTerm.Id)
                                .ToList();

                            var summerDisapprovedCourses = summerCourses
                                .Where(x => curriculumCourses.Contains(x.CourseId) && !x.Approved)
                                .ToList();

                            if (summerDisapprovedCourses.Any())
                            {
                                createRegularConcept = true;

                                if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD
                                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA
                                    || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMBA)
                                {
                                    var concept = new Concept();
                                    switch (summerDisapprovedCourses.Count)
                                    {
                                        case 1:
                                            concept = concepts.FirstOrDefault(x => x.Id == oneDisapprovedCourseConceptId);
                                            break;
                                        case 2:
                                            concept = concepts.FirstOrDefault(x => x.Id == twoDisapprovedCourseConceptId);
                                            break;
                                        default:
                                            concept = concepts.FirstOrDefault(x => x.Id == threeDisapprovedCourseConceptId);
                                            break;
                                    }

                                    var totalCredits = summerDisapprovedCourses.Sum(x => x.Credits);

                                    var total = isExempted ? 0.00M : totalCredits * concept.Amount;
                                    var discount = total * discountPercentage / 100.0M;

                                    total -= discount;
                                    var subtotal = total;
                                    var igv = 0.00M;
                                    if (concept.IsTaxed)
                                    {
                                        subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                        igv = total + discount - subtotal;
                                    }

                                    var coursePayment = new Payment
                                    {
                                        Id = Guid.NewGuid(),
                                        Description = $"{concept.Description} {totalCredits} cred.",
                                        SubTotal = subtotal,
                                        IgvAmount = igv,
                                        Discount = discount,
                                        Total = total,
                                        EntityId = null,
                                        Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                        UserId = student.UserId,
                                        ConceptId = concept.Id,
                                        TermId = term.Id
                                    };

                                    payments.Add(coursePayment);
                                }
                                else
                                {
                                    foreach (var item in summerDisapprovedCourses.Where(x => x.StudentId == student.Id).ToList())
                                    {
                                        var concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == item.Try + 1 && x.AdmissionTypeId == student.AdmissionTypeId);
                                        if (concept == null)
                                            concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == item.Try + 1 && !x.AdmissionTypeId.HasValue);
                                        if (concept == null) continue;

                                        var total = isExempted ? 0.00M : concept.IsCostPerCredit ? item.Credits * concept.Concept.Amount : concept.Concept.Amount;
                                        var discount = total * discountPercentage / 100.0M;

                                        total -= discount;
                                        var subtotal = total;
                                        var igv = 0.00M;
                                        if (concept.Concept.IsTaxed)
                                        {
                                            subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                            igv = total + discount - subtotal;
                                        }

                                        var coursePayment = new Payment
                                        {
                                            Id = Guid.NewGuid(),
                                            Description = concept.Concept.Description,
                                            SubTotal = subtotal,
                                            IgvAmount = igv,
                                            Discount = discount,
                                            Total = total,
                                            EntityId = item.CourseId,
                                            Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                            UserId = student.UserId,
                                            ConceptId = concept.Concept.Id,
                                            TermId = term.Id
                                        };

                                        payments.Add(coursePayment);
                                    }
                                }
                            }

                            regularDisapprovedCourses = regularDisapprovedCourses
                                .Where(x => !summerCourses.Any(y => y.CourseId == x.CourseId))
                                .ToList();
                        }

                        if (regularDisapprovedCourses.Any())
                        {
                            createRegularConcept = true;

                            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD
                                || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAJMA
                                || ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMBA)
                            {
                                var concept = new Concept();
                                switch (regularDisapprovedCourses.Count)
                                {
                                    case 1:
                                        concept = concepts.FirstOrDefault(x => x.Id == oneDisapprovedCourseConceptId);
                                        break;
                                    case 2:
                                        concept = concepts.FirstOrDefault(x => x.Id == twoDisapprovedCourseConceptId);
                                        break;
                                    default:
                                        concept = concepts.FirstOrDefault(x => x.Id == threeDisapprovedCourseConceptId);
                                        break;
                                }

                                var totalCredits = regularDisapprovedCourses.Sum(x => x.Credits);

                                var total = isExempted ? 0.00M : totalCredits * concept.Amount;
                                var discount = total * discountPercentage / 100.0M;

                                total -= discount;
                                var subtotal = total;
                                var igv = 0.00M;
                                if (concept.IsTaxed)
                                {
                                    subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                    igv = total + discount - subtotal;
                                }

                                var coursePayment = new Payment
                                {
                                    Id = Guid.NewGuid(),
                                    Description = $"{concept.Description} {totalCredits} cred.",
                                    SubTotal = subtotal,
                                    IgvAmount = igv,
                                    Discount = discount,
                                    Total = total,
                                    EntityId = null,
                                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = concept.Id,
                                    TermId = term.Id
                                };

                                payments.Add(coursePayment);
                            }
                            else
                            {
                                foreach (var item in regularDisapprovedCourses.Where(x => x.StudentId == student.Id).ToList())
                                {
                                    var concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == item.Try + 1 && x.AdmissionTypeId == student.AdmissionTypeId);
                                    if (concept == null)
                                        concept = disapprovedCourseConcepts.FirstOrDefault(x => x.Try == item.Try + 1 && !x.AdmissionTypeId.HasValue);
                                    if (concept == null) continue;

                                    var total = isExempted ? 0.00M : concept.IsCostPerCredit ? item.Credits * concept.Concept.Amount : concept.Concept.Amount;
                                    var discount = total * discountPercentage / 100.0M;

                                    total -= discount;
                                    var subtotal = total;
                                    var igv = 0.00M;
                                    if (concept.Concept.IsTaxed)
                                    {
                                        subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                                        igv = total + discount - subtotal;
                                    }

                                    var coursePayment = new Payment
                                    {
                                        Id = Guid.NewGuid(),
                                        Description = concept.Concept.Description,
                                        SubTotal = subtotal,
                                        IgvAmount = igv,
                                        Discount = discount,
                                        Total = total,
                                        EntityId = item.CourseId,
                                        Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                        UserId = student.UserId,
                                        ConceptId = concept.Concept.Id,
                                        TermId = term.Id
                                    };

                                    payments.Add(coursePayment);
                                }
                            }
                        }
                    }

                    //GENERAR CONCEPTO REGULAR
                    foreach (var item in enrollmentConcepts.Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ADDITIONAL_CONCEPT && (!x.Condition.HasValue || x.Condition == student.Condition)))
                    {
                        var total = isExempted ? 0.00M : item.Amount;
                        var discount = total * discountPercentage / 100.0M;

                        total -= discount;
                        var subtotal = total;
                        var igv = 0.00M;

                        var isTaxed = item.IsTaxed;
                        var description = item.Description;
                        var conceptId = item.ConceptId;

                        if (enrollmentConcepts.Any(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ADDITIONAL_CONCEPT_REPLACEMENT
                        && x.ConceptToReplaceId == item.ConceptId && x.CareerId == student.CareerId))
                        {
                            var conceptReplace = enrollmentConcepts.FirstOrDefault(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ADDITIONAL_CONCEPT_REPLACEMENT
                            && x.ConceptToReplaceId == item.ConceptId && x.CareerId == student.CareerId);

                            total = isExempted ? 0.00M : conceptReplace.Amount;
                            discount = total * discountPercentage / 100.0M;
                            total -= discount;
                            subtotal = total;
                            igv = 0.00M;

                            isTaxed = conceptReplace.IsTaxed;
                            description = conceptReplace.Description;
                            conceptId = conceptReplace.ConceptId;
                        }

                        if (isTaxed)
                        {
                            subtotal = discount + total / (1.00M + ConstantHelpers.Treasury.IGV);
                            igv = total + discount - subtotal;
                        }

                        payments.Add(new Payment
                        {
                            Id = Guid.NewGuid(),
                            Description = description,
                            SubTotal = subtotal,
                            IgvAmount = igv,
                            Discount = discount,
                            Total = total,
                            EntityId = Guid.Empty,
                            Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                            UserId = student.UserId,
                            ConceptId = conceptId,
                            TermId = term.Id
                        });
                    }

                    if (createRegularConcept)
                    {
                        isExempted = isExempted || student.IsExoneratedEnrollment;
                        if (student.Status == ConstantHelpers.Student.States.UNBEATEN || student.Status == ConstantHelpers.Student.States.HIGH_PERFORMANCE || (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNAMAD && student.Status == ConstantHelpers.Student.States.REGULAR))
                        {
                            if (!exonerateUnbeatenStudents || student.Condition == ConstantHelpers.Student.Condition.PERMANENT_PAYER || student.Condition == ConstantHelpers.Student.Condition.SECOND_CAREER)
                            {
                                var studentConceptId = unbeatenConceptId;

                                var specialConcept = enrollmentConcepts
                                    .OrderByDescending(x => x.CareerId)
                                    .ThenByDescending(x => x.AdmissionTypeId)
                                    .ThenByDescending(x => x.Condition)
                                    .FirstOrDefault(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.UNBEATEN_ENROLLMENT_CONCEPT
                                    && (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                                    && (x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId)
                                    && (!x.Condition.HasValue || x.Condition == student.Condition));
                                if (specialConcept != null) studentConceptId = specialConcept.ConceptId;

                                var concept = concepts.FirstOrDefault(x => x.Id == studentConceptId);
                                var enrollmentCost = concept.Amount;
                                //if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNCP
                                //    && (student.Status == ConstantHelpers.Student.States.REPEATER 
                                //    || student.Condition == ConstantHelpers.Student.Condition.TRANSITORY_PAYER 
                                //    || student.Condition == ConstantHelpers.Student.Condition.PERMANENT_PAYER))
                                //{
                                //    enrollmentCost += 30.0M;
                                //}
                                var discount = enrollmentCost * discountPercentage / 100.0M;

                                enrollmentCost -= discount;
                                var enrollmentSubTotal = enrollmentCost;
                                var enrollmentIgvAmount = 0.00M;
                                if (concept.IsTaxed)
                                {
                                    enrollmentSubTotal = discount + enrollmentCost / (1.00M + ConstantHelpers.Treasury.IGV);
                                    enrollmentIgvAmount = enrollmentCost + discount - enrollmentSubTotal;
                                }

                                //if (enrollmentFees.Any(x => x.AdmissionTypeId == student.AdmissionTypeId) && !isExempted && !student.IsExoneratedEnrollment)
                                //    enrollmentFeeStudents.Add(new StudentEnrollmentFee
                                //    {
                                //        Amount = enrollmentCost,
                                //        ConceptId = studentConceptId,
                                //        StudentId = student.Id,
                                //        TermId = term.Id
                                //    });
                                //else
                                {
                                    var payment = new Payment
                                    {
                                        Id = Guid.NewGuid(),
                                        Description = concept.Description,
                                        SubTotal = isExempted || student.IsExoneratedEnrollment ? 0.00M : enrollmentSubTotal,
                                        IgvAmount = isExempted || student.IsExoneratedEnrollment ? 0.00M : enrollmentIgvAmount,
                                        Discount = isExempted || student.IsExoneratedEnrollment ? 0.00M : discount,
                                        Total = isExempted || student.IsExoneratedEnrollment ? 0.00M : enrollmentCost,
                                        EntityId = Guid.Empty,
                                        Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                        UserId = student.UserId,
                                        ConceptId = studentConceptId,
                                        TermId = term.Id,
                                        CurrentAccountId = concept.CurrentAccountId
                                    };
                                    payments.Add(payment);
                                }
                            }
                        }
                        else
                        {
                            var studentConceptId = regularConceptId;

                            if (student.Status == ConstantHelpers.Student.States.ENTRANT || student.Status == ConstantHelpers.Student.States.TRANSFER)
                                studentConceptId = admissionConceptId;

                            var specialConcept = enrollmentConcepts
                                .OrderByDescending(x => x.CareerId)
                                .ThenByDescending(x => x.AdmissionTypeId)
                                .ThenByDescending(x => x.Condition)
                                .FirstOrDefault(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.REGULAR_ENROLLMENT_CONCEPT
                                && (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                                && (x.AdmissionTypeId.HasValue && x.AdmissionTypeId == student.AdmissionTypeId)
                                && (!x.Condition.HasValue || x.Condition == student.Condition));
                            if (specialConcept != null) studentConceptId = specialConcept.ConceptId;

                            var concept = concepts.FirstOrDefault(x => x.Id == studentConceptId);
                            var enrollmentCost = concept.Amount;

                            //if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNCP
                            //    && (student.Status == ConstantHelpers.Student.States.REPEATER
                            //    || student.Condition == ConstantHelpers.Student.Condition.TRANSITORY_PAYER
                            //    || student.Condition == ConstantHelpers.Student.Condition.PERMANENT_PAYER))
                            //{
                            //    enrollmentCost += 30.0M;
                            //}

                            var discount = enrollmentCost * discountPercentage / 100.0M;

                            enrollmentCost -= discount;
                            var enrollmentSubTotal = enrollmentCost;
                            var enrollmentIgvAmount = 0.00M;
                            if (concept.IsTaxed)
                            {
                                enrollmentSubTotal = discount + enrollmentCost / (1.00M + ConstantHelpers.Treasury.IGV);
                                enrollmentIgvAmount = enrollmentCost + discount - enrollmentSubTotal;
                            }

                            //if (enrollmentFees.Any(x => x.AdmissionTypeId == student.AdmissionTypeId) && !isExempted && !student.IsExoneratedEnrollment)
                            //    enrollmentFeeStudents.Add(new StudentEnrollmentFee
                            //    {
                            //        Amount = enrollmentCost,
                            //        ConceptId = studentConceptId,
                            //        StudentId = student.Id,
                            //        TermId = term.Id
                            //    });
                            //else
                            {
                                var payment = new Payment
                                {
                                    Id = Guid.NewGuid(),
                                    Description = concept.Description,
                                    SubTotal = isExempted || student.IsExoneratedEnrollment ? 0.00M : enrollmentSubTotal,
                                    IgvAmount = isExempted || student.IsExoneratedEnrollment ? 0.00M : enrollmentIgvAmount,
                                    Discount = isExempted || student.IsExoneratedEnrollment ? 0.00M : discount,
                                    Total = isExempted || student.IsExoneratedEnrollment ? 0.00M : enrollmentCost,
                                    EntityId = Guid.Empty,
                                    Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = studentConceptId,
                                    TermId = term.Id,
                                    CurrentAccountId = concept.CurrentAccountId
                                };
                                payments.Add(payment);
                            }
                        }
                    }

                    if (enableExtraPayment)
                    {
                        var academicYearsEnrolled = 0;
                        if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
                        {
                            academicYearsEnrolled = await _context.Terms
                                .CountAsync(x => student.AdmissionTermStartDate <= x.StartDate && x.Status == ConstantHelpers.TERM_STATES.FINISHED && !x.IsSummer);
                        }
                        else
                        {
                            academicYearsEnrolled = await _context.AcademicSummaries
                                .Where(x => x.StudentId == student.Id && x.CurriculumId == student.CurriculumId
                                && !x.Term.IsSummer && !x.WasWithdrawn)
                                .CountAsync();
                        }

                        var curriculumAcademicYears = academicYearCourses.Where(x => x.CurriculumId == student.CurriculumId).OrderByDescending(x => x.AcademicYear).Select(x => x.AcademicYear).FirstOrDefault();
                        if (academicYearsEnrolled + 1 > curriculumAcademicYears + gracePeriod)
                        {
                            var studentExtraPaymentConceptId = extraPaymentConceptId;
                            var extraEnrollmentConcepts = enrollmentConcepts.Where(x => x.Type == ConstantHelpers.EnrollmentConcept.Type.EXTRA_ENROLLMENT_CONCEPT).ToList();
                            var extraConcept = extraEnrollmentConcepts
                                .OrderByDescending(x => x.CareerId)
                                .ThenByDescending(x => x.AdmissionTypeId)
                                .FirstOrDefault(x => (!x.CareerId.HasValue || x.CareerId == student.CareerId)
                                && (!x.AdmissionTypeId.HasValue || x.AdmissionTypeId == student.AdmissionTypeId));
                            if (extraConcept != null) studentExtraPaymentConceptId = extraConcept.ConceptId;

                            var extraPaymentConcept = concepts.FirstOrDefault(x => x.Id == studentExtraPaymentConceptId);
                            var extraPaymentConceptCost = extraPaymentConcept.Amount;

                            if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
                            {
                                var enrolledCredits = studentSections
                                    .Where(x => x.StudentId == student.Id)
                                    .Sum(x => x.Credits);
                                extraPaymentConceptCost = extraPaymentConcept.Amount * enrolledCredits;
                            }
                            var discount = extraPaymentConceptCost * discountPercentage / 100.0M;

                            extraPaymentConceptCost -= discount;
                            var extraPaymentConceptSubTotal = extraPaymentConceptCost;
                            var extraPaymentConceptIgvAmount = 0.00M;

                            if (extraPaymentConcept.IsTaxed)
                            {
                                extraPaymentConceptSubTotal = discount + extraPaymentConceptCost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                                extraPaymentConceptIgvAmount = extraPaymentConceptCost + discount - extraPaymentConceptSubTotal;
                            }

                            payments.Add(new Payment
                            {
                                Id = Guid.NewGuid(),
                                Description = extraPaymentConcept.Description,
                                SubTotal = extraPaymentConceptSubTotal,
                                IgvAmount = extraPaymentConceptIgvAmount,
                                Discount = discount,
                                Total = extraPaymentConceptCost,
                                EntityId = Guid.Empty,
                                Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                UserId = student.UserId,
                                ConceptId = studentExtraPaymentConceptId,
                                TermId = term.Id,
                                CurrentAccountId = extraPaymentConcept.CurrentAccountId
                            });
                        }
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH)
                    {
                        var summary = await _context.AcademicSummaries
                            .Where(x => x.StudentId == student.Id && !x.Term.IsSummer && !x.WasWithdrawn && x.CurriculumId == student.CurriculumId)
                            .OrderByDescending(x => x.Term.Year)
                            .ThenByDescending(x => x.Term.Number)
                            .Select(x => new
                            {
                                x.WeightedAverageGrade,
                                x.Term.MinGrade
                            })
                            .FirstOrDefaultAsync();

                        if (lastSummary != null && summary.WeightedAverageGrade < summary.MinGrade)
                        {
                            //cobro concepto adicional por promedio desaprobado
                            var conceptId = new Guid("08d9ea5c-c9a7-498c-8fcd-9d00cd085913");
                            if (student.AdmissionTypeAbbrev == "GYTDNU") conceptId = new Guid("08d9ea5c-d3ce-46ac-8492-2d25baece45c");

                            var concept = await _context.Concepts.FindAsync(conceptId);
                            if (concept != null)
                            {
                                var cost = concept.Amount;
                                var discount = cost * discountPercentage / 100.0M;

                                cost -= discount;
                                var subTotal = cost;
                                var igvAmount = 0.00M;

                                if (concept.IsTaxed)
                                {
                                    subTotal = discount + cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                                    igvAmount = cost + discount - subTotal;
                                }

                                payments.Add(new Payment
                                {
                                    Description = concept.Description,
                                    SubTotal = subTotal,
                                    IgvAmount = igvAmount,
                                    Discount = discount,
                                    Total = cost,
                                    EntityId = null,
                                    Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = conceptId,
                                    TermId = term.Id,
                                    CurrentAccountId = concept.CurrentAccountId
                                });
                            }
                        }
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSCH)
                    {
                        var enrolledCredits = studentSections
                           .Where(x => x.StudentId == student.Id)
                           .Sum(x => x.Credits);

                        if (student.AdmissionTypeAbbrev == "GYTDNU" && enrolledCredits > 0)
                        {
                            var conceptId = new Guid("08d9f6ed-e520-46e5-89dd-9212aa711e25");
                            var concept = await _context.Concepts.FindAsync(conceptId);
                            if (concept != null)
                            {
                                var cost = concept.Amount * enrolledCredits;
                                var discount = cost * discountPercentage / 100.0M;

                                cost -= discount;
                                var subTotal = cost;
                                var igvAmount = 0.00M;

                                if (concept.IsTaxed)
                                {
                                    subTotal = discount + cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                                    igvAmount = cost + discount - subTotal;
                                }

                                payments.Add(new Payment
                                {
                                    Description = $"{concept.Description} {enrolledCredits} cred.",
                                    SubTotal = subTotal,
                                    IgvAmount = igvAmount,
                                    Discount = discount,
                                    Total = cost,
                                    EntityId = null,
                                    Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = conceptId,
                                    TermId = term.Id,
                                    CurrentAccountId = concept.CurrentAccountId
                                });
                            }
                        }
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.UNSM)
                    {
                        var enrolledCredits = await _context.StudentSections
                           .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == term.Id)
                           .SumAsync(x => x.Section.CourseTerm.Course.Credits);

                        if (student.AdmissionTypeAbbrev == "SEGUNDA PROFESIONALIZACION" && enrolledCredits > 0)
                        {
                            var conceptId = new Guid("08da01e4-216d-4ed4-8764-4756b4f2e819");
                            var concept = await _context.Concepts.FindAsync(conceptId);
                            if (concept != null)
                            {
                                var cost = concept.Amount * enrolledCredits;
                                var discount = cost * discountPercentage / 100.0M;

                                cost -= discount;
                                var subTotal = cost;
                                var igvAmount = 0.00M;

                                if (concept.IsTaxed)
                                {
                                    subTotal = discount + cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                                    igvAmount = cost + discount - subTotal;
                                }

                                payments.Add(new Payment
                                {
                                    Description = $"{concept.Description} {enrolledCredits} cred.",
                                    SubTotal = subTotal,
                                    IgvAmount = igvAmount,
                                    Discount = discount,
                                    Total = cost,
                                    EntityId = null,
                                    Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = conceptId,
                                    TermId = term.Id,
                                    CurrentAccountId = concept.CurrentAccountId
                                });
                            }
                        }
                    }

                    if (ConstantHelpers.GENERAL.Institution.Value == ConstantHelpers.Institution.ENSDF)
                    {
                        var studentCourses = await _context.StudentSections
                           .Where(x => x.StudentId == student.Id && x.Section.CourseTerm.TermId == term.Id)
                           .Select(x => x.Section.CourseTerm.CourseId)
                           .ToListAsync();
                        var curriculumCourses = await _context.AcademicYearCourses
                            .Where(x => x.CurriculumId == student.CurriculumId && x.AcademicYear != student.CurrentAcademicYear)
                            .Select(x => x.CourseId).ToListAsync();

                        var additionalCourses = studentCourses.Where(x => curriculumCourses.Contains(x)).Count();

                        if (additionalCourses > 0)
                        {
                            var conceptId = new Guid("08db1677-f3e4-4dee-8cbf-a5176957cf86");
                            if (isExtemporaneous)
                                conceptId = new Guid("08db1677-f3e4-4e0a-8810-7c940c0fb58d");

                            var concept = await _context.Concepts.FindAsync(conceptId);
                            if (concept != null)
                            {
                                var cost = concept.Amount * additionalCourses;
                                var discount = cost * discountPercentage / 100.0M;

                                cost -= discount;
                                var subTotal = cost;
                                var igvAmount = 0.00M;

                                if (concept.IsTaxed)
                                {
                                    subTotal = discount + cost / (1.00M + CORE.Helpers.ConstantHelpers.Treasury.IGV);
                                    igvAmount = cost + discount - subTotal;
                                }

                                payments.Add(new Payment
                                {
                                    Description = $"{concept.Description} {additionalCourses} Cursos",
                                    SubTotal = subTotal,
                                    IgvAmount = igvAmount,
                                    Discount = discount,
                                    Total = cost,
                                    EntityId = null,
                                    Type = CORE.Helpers.ConstantHelpers.PAYMENT.TYPES.ENROLLMENT,
                                    UserId = student.UserId,
                                    ConceptId = conceptId,
                                    TermId = term.Id,
                                    CurrentAccountId = concept.CurrentAccountId
                                });
                            }
                        }
                    }

                }
            }

            var paidPayments = await _context.Payments
                .Where(x => x.TermId == term.Id && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .ToListAsync();

            var newPayments = new List<Payment>();
            foreach (var item in payments)
            {
                var paidPayment = paidPayments.FirstOrDefault(x => x.UserId == item.UserId && x.ConceptId == item.ConceptId && x.Total == item.Total);

                if (paidPayment != null)
                {
                    if (paidPayment.IsBankPayment) paidPayment.WasBankPaymentUsed = true;
                    paidPayment.Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT;
                }
                else
                    newPayments.Add(item);
            }

            //var newEnrollmentFeeStudents = new List<StudentEnrollmentFee>();
            //foreach (var item in enrollmentFeeStudents)
            //{
            //    var student = students.FirstOrDefault(x => x.Id == item.StudentId);
            //    var paidPayment = paidPayments.FirstOrDefault(x => x.UserId == student.UserId && x.ConceptId == item.ConceptId && x.Total == item.Amount);

            //    if (paidPayment != null)
            //    {
            //        if (paidPayment.IsBankPayment) paidPayment.WasBankPaymentUsed = true;
            //        paidPayment.Type = ConstantHelpers.PAYMENT.TYPES.ENROLLMENT;
            //    }
            //    else
            //        newEnrollmentFeeStudents.Add(item);
            //}

            //await _context.StudentEnrollmentFees.AddRangeAsync(newEnrollmentFeeStudents);
            await _context.Payments.AddRangeAsync(newPayments);
            await _context.SaveChangesAsync();
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetBankLoadDatatableReport(int year)
        {
            var firstDate = new DateTime(year, 1, 1).ToUtcDateTime();
            var lastDate = new DateTime(year + 1, 1, 1);
            lastDate = lastDate.AddTicks(-1).ToUtcDateTime();

            var query = _context.Payments
                .Where(x => x.IsBankPayment && x.PaymentDate.HasValue
                && x.PaymentDate.Value >= firstDate
                && x.PaymentDate.Value <= lastDate)
                .AsNoTracking();

            var data = await query
               .Select(x => x.PaymentDate.Value.ToDefaultTimeZone().Month)
               .ToListAsync();

            var months = ConstantHelpers.MONTHS.VALUES
                .Select(x => new
                {
                    id = x.Key,
                    name = x.Value,
                    loaded = data.Contains(x.Key)
                }).ToList();

            var recordsTotal = months.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = months,
                //DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetBankLoadFullCalendar(int year, int month)
        {
            var firstDay = new DateTime(year, month, 1).ToUtcDateTime();
            var lastDay = new DateTime(year, month, 1).AddMonths(1);
            lastDay = lastDay.AddTicks(-1).ToUtcDateTime();

            var payments = await _context.Payments
                .Where(x => x.IsBankPayment && x.PaymentDate.HasValue
                && x.PaymentDate.Value >= firstDay
                && x.PaymentDate.Value <= lastDay)
                .OrderByDescending(x => x.PaymentDate)
                .Select(x => $"{x.PaymentDate.Value.ToDefaultTimeZone():yyyy-MM-dd}")
                .ToListAsync();

            var result = payments
                .GroupBy(x => x)
                .Select(x => new
                {
                    title = x.Count(),
                    description = "Pagos efectuados",
                    allDay = true,
                    start = x.Key,
                    stick = true
                }).ToList();

            return result;
        }

        public async Task<object> GetBankLoadYearsSelect2()
        {
            var years = await _context.Payments
                .Where(x => x.IsBankPayment && x.PaymentDate.HasValue)
                .GroupBy(x => x.PaymentDate.Value.Year)
                .Select(x => x.Key)
                .ToListAsync();

            var result = years
                .OrderBy(x => x)
                .Select(x => new
                {
                    id = x,
                    text = x.ToString()
                }).ToList();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConceptIncomePaymentDatatableReport(DataTablesStructs.SentParameters sentParameters, string searchValue = null, DateTime? startDate = null, DateTime? endDate = null, string userId = null)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Concept.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Concept.Description;
                    break;
                default:
                    orderByPredicate = (x) => x.Concept.Code;
                    break;
            }

            var qryPayment = _context.Payments
                .Where(x => x.ConceptId.HasValue && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(userId))
                qryPayment = qryPayment.Where(x => x.Invoice.PettyCash.UserId == userId);

            if (startDate.HasValue) qryPayment = qryPayment.Where(x => x.PaymentDate >= startDate);
            if (endDate.HasValue) qryPayment = qryPayment.Where(x => x.PaymentDate <= endDate);

            if (!string.IsNullOrEmpty(searchValue)) qryPayment = qryPayment.Where(x => x.Concept.Code.ToUpper().Contains(searchValue.ToUpper()) || x.Concept.Description.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await qryPayment
                .GroupBy(x => x.ConceptId)
                .Select(x => x.Key)
                .CountAsync();

            var data = await qryPayment
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    code = x.Concept.Code,
                    description = x.Concept.Description,
                    amount = x.Total,
                    id = x.ConceptId
                }).ToListAsync();

            var data2 = data
                .GroupBy(x => new { x.id, x.description, x.code })
                .Select(x => new
                {
                    x.Key.code,
                    x.Key.description,
                    amount = x.Sum(y => y.amount)
                }).ToList();


            var recordsTotal = data2.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data2,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<decimal> GetConceptIncomePaymentDatatableReportTotalAmount(string search, DateTime? startDate = null, DateTime? endDate = null, string userId = null)
        {
            var qryPayment = _context.Payments
                .Where(x => x.ConceptId.HasValue && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(userId))
                qryPayment = qryPayment.Where(x => x.Invoice.PettyCash.UserId == userId);

            if (startDate.HasValue) qryPayment = qryPayment.Where(x => x.PaymentDate >= startDate);
            if (endDate.HasValue) qryPayment = qryPayment.Where(x => x.PaymentDate <= endDate);

            if (!string.IsNullOrEmpty(search))
                qryPayment = qryPayment.Where(x => x.Concept.Code.ToUpper().Contains(search.ToUpper()) || x.Concept.Description.ToUpper().Contains(search.ToUpper()));

            var result = await qryPayment.SumAsync(x => x.Total);

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetStudentPaymentDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, Guid? conceptId = null, ClaimsPrincipal user = null, byte? type = null, string search = null)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.User.Students.Select(y => y.Career.Name).FirstOrDefault();
                    break;
                case "4":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Total;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Invoice.Number;
                    break;
                case "7":
                    orderByPredicate = (x) => x.PaymentDate;
                    break;
                default:
                    break;
            }

            var query = _context.Payments
                .Where(x => x.UserId != null && x.User.Students.Any() && x.ConceptId.HasValue && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .AsNoTracking();

            if (startDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate.Value >= startDate.Value);
            if (endDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate.Value <= endDate.Value);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEPENDENCY))
                {
                    var dependencies = _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToHashSet();
                    query = query.Where(x => x.ConceptId.HasValue && dependencies.Contains(x.Concept.DependencyId));
                }
            }

            if (conceptId.HasValue && conceptId != Guid.Empty)
                query = query.Where(x => x.ConceptId.HasValue && x.ConceptId == conceptId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => (x.ConceptId.HasValue && x.Concept.Description.ToUpper().Contains(search.ToUpper())) || (x.UserId != null && x.User.UserName.ToUpper().Contains(search.ToUpper())));

            if (type.HasValue && (type.Value == 1 || type.Value == 2))
            {
                if (type.Value == 1)
                    query = query.Where(x => x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT);

                if (type.Value == 2)
                    query = query.Where(x => x.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && x.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT);
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
               //.OrderBy(x => x.Concept.Code)
               .Select(x => new
               {
                   id = x.Id,
                   user = x.User.UserName,
                   name = x.User.FullName,
                   career = x.User.Students.Select(y => y.Career.Name).FirstOrDefault(),
                   concept = string.IsNullOrEmpty(x.Concept.Description) ? x.Description : x.Concept.Description,
                   totalAmount = x.Total,
                   invoice = x.InvoiceId.HasValue ? $"{x.Invoice.Series}-{x.Invoice.Number.ToString().PadLeft(ConstantHelpers.Treasury.Invoice.CORRELATIVE_PADLEFT, '0')}"
                   : !string.IsNullOrEmpty(x.OperationCodeB) ? x.OperationCodeB : "---",
                   date = x.PaymentDate,
                   type = x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT ? "Matrícula" : "Otros",
               }).ToListAsync();

            var data2 = data
                .Select(x => new
                {
                    x.id,
                    x.user,
                    x.name,
                    x.career,
                    x.concept,
                    x.totalAmount,
                    x.invoice,
                    date = x.date.HasValue ? x.date.ToLocalDateFormat() : "---",
                    x.type
                }).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data2,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<StudentPaymentTemplate>> GetStudentPaymentData(DateTime? startDate = null, DateTime? endDate = null, Guid? conceptId = null, ClaimsPrincipal user = null, byte? type = null, string search = null)
        {
            var query = _context.Payments
                .Where(x => x.UserId != null && x.User.Students.Any() && x.ConceptId.HasValue && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .AsNoTracking();

            if (startDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate.Value >= startDate.Value);
            if (endDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate.Value <= endDate.Value);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (user.IsInRole(ConstantHelpers.ROLES.DEPENDENCY))
                {
                    var dependencies = _context.UserDependencies.Where(x => x.UserId == userId).Select(x => x.DependencyId).ToHashSet();
                    query = query.Where(x => x.ConceptId.HasValue && dependencies.Contains(x.Concept.DependencyId));
                }
            }

            if (conceptId.HasValue && conceptId != Guid.Empty)
                query = query.Where(x => x.ConceptId.HasValue && x.ConceptId == conceptId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => (x.ConceptId.HasValue && x.Concept.Description.ToUpper().Contains(search.ToUpper())) || (x.UserId != null && x.User.UserName.ToUpper().Contains(search.ToUpper())));

            if (type.HasValue && (type.Value == 1 || type.Value == 2))
            {
                if (type.Value == 1)
                    query = query.Where(x => x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT);

                if (type.Value == 2)
                    query = query.Where(x => x.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && x.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT);
            }

            var data = await query
               .Select(x => new StudentPaymentTemplate
               {
                   User = x.User.UserName,
                   FullName = x.User.FullName,
                   Career = x.User.Students.Select(y => y.Career.Name).FirstOrDefault(),
                   Concept = string.IsNullOrEmpty(x.Concept.Description) ? x.Description : x.Concept.Description,
                   Amount = x.Total,
                   Invoice = x.InvoiceId.HasValue ? $"{x.Invoice.Series}-{x.Invoice.Number.ToString().PadLeft(ConstantHelpers.Treasury.Invoice.CORRELATIVE_PADLEFT, '0')}"
                   : !string.IsNullOrEmpty(x.OperationCodeB) ? x.OperationCodeB : "---",
                   Date = x.PaymentDate,
                   Type = x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT ? "Matrícula" : "Otros",
               }).ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentsDetailedByCashierDatatable(DataTablesStructs.SentParameters sentParameters, DateTime? startDate, DateTime? endDate, string cashierId)
        {
            var query = _context.Payments
                .Where(x => x.ConceptId.HasValue && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(cashierId))
                query = query.Where(x => x.Invoice.PettyCash.UserId == cashierId);

            if (startDate.HasValue) query = query.Where(x => x.PaymentDate >= startDate);
            if (endDate.HasValue) query = query.Where(x => x.PaymentDate <= endDate);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    cashier = x.Invoice.PettyCash.User.FullName,
                    invoice = x.InvoiceId.HasValue ? $"{x.Invoice.Series}-{x.Invoice.Number.ToString().PadLeft(ConstantHelpers.Treasury.Invoice.CORRELATIVE_PADLEFT, '0')}"
                    : !string.IsNullOrEmpty(x.OperationCodeB) ? x.OperationCodeB : "-",
                    client = x.User.FullName ?? x.ExternalUser.FullName,
                    date = x.PaymentDate.ToLocalDateTimeFormat(),
                    totalAmount = x.Total,
                    conceptDescription = x.Concept.Description,
                    conceptCode = x.Concept.Code
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

        public async Task<List<PaymentsDetailedTemplate>> GetPaymentsDetailedByCashierTemplate(DateTime? startDate, DateTime? endDate, string cashierId)
        {
            var query = _context.Payments.AsNoTracking();

            if (!string.IsNullOrEmpty(cashierId))
                query = query.Where(x => x.Invoice.PettyCash.UserId == cashierId);

            if (startDate.HasValue) query = query.Where(x => x.PaymentDate >= startDate);
            if (endDate.HasValue) query = query.Where(x => x.PaymentDate <= endDate);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new PaymentsDetailedTemplate
                {
                    Type = ConstantHelpers.Treasury.DocumentType.NAMES[x.Invoice.DocumentType],
                    Serie = x.InvoiceId.HasValue ? x.Invoice.Series : "-",
                    Number = x.InvoiceId.HasValue ? x.Invoice.Number.ToString().PadLeft(ConstantHelpers.Treasury.Invoice.CORRELATIVE_PADLEFT, '0') : "-",
                    PaymentDate = x.PaymentDate.ToLocalDateTimeFormat(),
                    Client = x.User.FullName ?? x.ExternalUser.FullName,
                    ClientDNI = x.User.Document ?? x.User.Dni ?? x.ExternalUser.DocumentNumber,
                    AccountingPlan = x.Concept.AccountingPlan.Code,
                    Concept = x.Concept.Code,
                    Quantity = x.Quantity,
                    TotalAmount = $"S/. {x.Total:F}",
                    TotalDecimal = x.Total
                }).ToListAsync();

            return data;
        }

        public async Task<Payment> GetPaymentByOperationCodeToValidateProcedure(string userId, DateTime date, string operationCodeB, decimal amount, bool exactAmount = true)
        {
            var query = _context.Payments
                .Where(x => x.PaymentDate.Value.AddHours(-5).Date == date.Date && x.OperationCodeB.ToLower().Trim() == operationCodeB.ToLower().Trim())
                .AsNoTracking();

            query = query.Where(x => x.UserId == userId);

            if (exactAmount)
            {
                query = query.Where(x => x.Total == amount);
            }
            else
            {
                query = query.Where(x => x.Total >= amount);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetUnusedPaymentsDatatable(DataTablesStructs.SentParameters parameters, string userId, decimal? minAmount, Guid? conceptId = null)
        {
            var query = _context.Payments
                .OrderByDescending(x => x.CreatedAt)
                .AsNoTracking();

            if (conceptId.HasValue)
                query = query.Where(x => x.ConceptId == conceptId);

            if (minAmount.HasValue)
                query = query.Where(x => x.Total >= minAmount);

            query = query.Where(x => x.UserId == userId && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && !x.WasBankPaymentUsed).AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
             .Skip(parameters.PagingFirstRecord)
             .Take(parameters.RecordsPerDraw)
             .Select(x => new
             {
                 x.Id,
                 createdAt = x.CreatedAt.ToLocalDateTimeFormat(),
                 paymentDate = x.PaymentDate.ToLocalDateTimeFormat(),
                 operationCodeB = x.OperationCodeB ?? x.Description,
                 x.Description,
                 x.Total
             }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnyByEntityId(Guid entityId)
            => await _context.Payments.AnyAsync(x => x.EntityId == entityId);


        public async Task<DataTablesStructs.ReturnedData<object>> GetPaymentReportDatatableData(DataTablesStructs.SentParameters sentParameters, DateTime? startDate = null, DateTime? endDate = null, byte type = 0, Guid? formatId = null, string userId = null)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Invoice.Series;
                    break;
                case "1":
                    orderByPredicate = (x) => x.InvoiceId.HasValue ? x.Invoice.Number.ToString() : x.OperationCodeB;
                    break;
                case "2":
                    orderByPredicate = (x) => x.PaymentDate;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.Document;
                    break;
                case "4":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Concept.Classifier.Name;
                    break;
                case "6":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "7":
                    orderByPredicate = (x) => x.Quantity;
                    break;
                case "8":
                    orderByPredicate = (x) => x.Total;
                    break;
                case "9":
                    orderByPredicate = (x) => x.Concept.AccountingPlan.Code;
                    break;
                case "10":
                    orderByPredicate = (x) => x.Invoice.PettyCash.User.FullName;
                    break;
                default:
                    break;
            }

            var query = _context.Payments
                .Where(x => x.PaymentDate.HasValue)
                .AsNoTracking();

            if (startDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate.Value >= startDate.Value);
            if (endDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate.Value <= endDate.Value);

            if (type == 1) // pagos de banco
            {
                query = query.Where(x => x.IsBankPayment);
                if (formatId.HasValue && formatId != Guid.Empty)
                    query = query.Where(x => x.EntityLoadFormatId == formatId);
            }

            if (type == 2)
            {
                query = query.Where(x => x.InvoiceId.HasValue);
                if (!string.IsNullOrEmpty(userId) && userId != Guid.Empty.ToString())
                    query = query.Where(x => x.Invoice.PettyCash.UserId == userId);
            }

            var recordsFiltered = await query.CountAsync();

            var data = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
               .Select(x => new
               {
                   id = x.Id,
                   series = x.InvoiceId.HasValue ? x.Invoice.Series : "---",
                   number = x.InvoiceId.HasValue ? x.Invoice.Number.ToString("D6") : x.OperationCodeB,

                   date = x.PaymentDate.HasValue ? $"{x.PaymentDate.ToDefaultTimeZone().Value:dd/MM/yyyy}" : "---",

                   document = x.ExternalUserId.HasValue ? x.ExternalUser.DocumentNumber : x.UserId != null ? x.User.Document : "---",
                   client = x.ExternalUserId.HasValue ? x.ExternalUser.FullName : x.UserId != null ? x.User.FullName : "---",

                   classifier = x.ConceptId.HasValue ? x.Concept.Classifier.Code : "---",
                   concept = x.Description,
                   quantity = x.Quantity,
                   total = x.Total,

                   account = x.ConceptId.HasValue ? x.Concept.AccountingPlan.Code : "---",

                   cashier = x.InvoiceId.HasValue ? x.Invoice.PettyCash.User.FullName : "---"
               }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<PaymentReportTemplate>> GetPaymentReportData(DateTime? startDate = null, DateTime? endDate = null, byte type = 0, Guid? formatId = null, string userId = null)
        {
            var query = _context.Payments
                .Where(x => x.PaymentDate.HasValue)
                .AsNoTracking();

            if (startDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate.Value >= startDate.Value);
            if (endDate.HasValue) query = query.Where(x => x.PaymentDate.HasValue && x.PaymentDate.Value <= endDate.Value);

            if (type == 1) // pagos de banco
            {
                query = query.Where(x => x.IsBankPayment);
                if (formatId.HasValue && formatId != Guid.Empty)
                    query = query.Where(x => x.EntityLoadFormatId == formatId);
            }

            if (type == 2) // pagos en caja
            {
                query = query.Where(x => x.InvoiceId.HasValue);
                if (!string.IsNullOrEmpty(userId) && userId != Guid.Empty.ToString())
                    query = query.Where(x => x.Invoice.PettyCash.UserId == userId);
            }

            var data = await query
               .Select(x => new PaymentReportTemplate
               {
                   DocumentType = x.InvoiceId.HasValue ? x.Invoice.DocumentType : 0,
                   Series = x.InvoiceId.HasValue ? x.Invoice.Series : "---",
                   Number = x.InvoiceId.HasValue ? x.Invoice.Number.ToString("D6") : x.OperationCodeB,
                   Date = x.PaymentDate.HasValue ? x.PaymentDate.Value : DateTime.UtcNow,
                   Document = x.ExternalUserId.HasValue ? x.ExternalUser.DocumentNumber : x.UserId != null ? x.User.Document : "---",
                   ClientName = x.ExternalUserId.HasValue ? x.ExternalUser.FullName : x.UserId != null ? x.User.FullName : "---",
                   Classifier = x.ConceptId.HasValue ? x.Concept.Classifier.Code : "---",
                   Concept = x.Description,
                   Quantity = x.Quantity,
                   Total = x.Total,
                   AccountingPlan = x.ConceptId.HasValue ? x.Concept.AccountingPlan.Code : "---",
                   Cashier = x.InvoiceId.HasValue ? x.Invoice.PettyCash.User.FullName : "---",
                   Dependency = x.Incomes.Any() ? x.Incomes.Select(y => y.Dependency.Name).FirstOrDefault() : "---"
               }).ToListAsync();

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExoneratedEnrollmentPaymentsDatatableData(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, int? academicYear = null, string searchValue = null)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    //orderByPredicate = (x) => x.User.UserName;
                    break;
                case "3":
                    //orderByPredicate = (x) => x.User.UserName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Total;
                    break;
                default:
                    break;
            }

            var query = _context.Payments
                .Where(x => x.TermId == termId && !string.IsNullOrEmpty(x.UserId)
                && x.WasExonerated && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID
                && (x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper())
                || x.User.UserName.ToUpper().Contains(searchValue.ToUpper())
                || x.User.FullName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var payments = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .Skip(sentParameters.PagingFirstRecord)
               .Take(sentParameters.RecordsPerDraw)
               .Select(x => new
               {
                   id = x.Id,
                   userId = x.UserId,
                   user = x.User.UserName,
                   fullname = x.User.FullName,
                   //career = x.User.Students.Select(y => y.Career.Name).FirstOrDefault(),
                   //academicYear = x.User.Students.Select(y => y.CurrentAcademicYear).FirstOrDefault(),
                   type = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION.ContainsKey(x.Type) ? ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.Type] : "",
                   concept = x.Description,
                   totalamount = x.Total * 1.00M,
                   issueDate = x.IssueDate.ToShortDateString(),
                   paymentDate = x.PaymentDate.HasValue ? x.PaymentDate.Value.ToShortDateString() : "-",
               })
                .ToListAsync();

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Id == termId);
            if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
            {
                var qryStudents = _context.AcademicSummaries
                    .Where(x => x.TermId == termId)
                    .AsNoTracking();

                if (careerId.HasValue && careerId != Guid.Empty)
                    qryStudents = qryStudents.Where(x => x.CareerId == careerId);

                if (academicYear.HasValue && academicYear > 0)
                    qryStudents = qryStudents.Where(x => x.StudentAcademicYear == academicYear);

                var students = await qryStudents
                    .Select(x => new
                    {
                        x.Student.UserId,
                        x.Student.User.UserName,
                        x.Student.User.FullName,
                        x.StudentAcademicYear,
                        Career = x.Career.Name
                    })
                    .ToListAsync();

                var data = payments
                   .Where(x => students.Any(y => y.UserId == x.userId))
                   .Select(x => new
                   {
                       x.id,
                       x.user,
                       x.fullname,
                       career = students.FirstOrDefault(y => y.UserId == x.userId).Career,
                       academicYear = students.FirstOrDefault(y => y.UserId == x.userId).StudentAcademicYear,
                       x.type,
                       x.concept,
                       x.totalamount,
                       x.issueDate,
                       x.paymentDate
                   })
                    .ToList();

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsFiltered
                };
            }
            else
            {
                var qryStudents = _context.Students
                    .AsNoTracking();

                if (careerId.HasValue && careerId != Guid.Empty)
                    qryStudents = qryStudents.Where(x => x.CareerId == careerId);

                if (academicYear.HasValue && academicYear > 0)
                    qryStudents = qryStudents.Where(x => x.CurrentAcademicYear == academicYear);

                var students = await qryStudents
                    .Select(x => new
                    {
                        x.UserId,
                        x.User.UserName,
                        x.User.FullName,
                        x.CurrentAcademicYear,
                        Career = x.Career.Name
                    })
                    .ToListAsync();

                var data = payments
                   .Where(x => students.Any(y => y.UserId == x.userId))
                    .Select(x => new
                    {
                        x.id,
                        x.user,
                        x.fullname,
                        career = students.FirstOrDefault(y => y.UserId == x.userId).Career,
                        academicYear = students.FirstOrDefault(y => y.UserId == x.userId).CurrentAcademicYear,
                        x.type,
                        x.concept,
                        x.totalamount,
                        x.issueDate,
                        x.paymentDate
                    })
                    .ToList();

                return new DataTablesStructs.ReturnedData<object>
                {
                    Data = data,
                    DrawCounter = sentParameters.DrawCounter,
                    RecordsFiltered = recordsFiltered,
                    RecordsTotal = recordsFiltered
                };
            }
        }

        public async Task<List<StudentPaymentTemplate>> GetExoneratedEnrollmentPaymentsData(Guid termId, Guid? careerId = null, int? academicYear = null, string searchValue = null)
        {
            var query = _context.Payments
                .Where(x => x.TermId == termId && !string.IsNullOrEmpty(x.UserId)
                && x.WasExonerated && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID
                && (x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper())
                || x.User.UserName.ToUpper().Contains(searchValue.ToUpper())
                || x.User.FullName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var payments = await query
               .Select(x => new
               {
                   id = x.Id,
                   userId = x.UserId,
                   user = x.User.UserName,
                   fullname = x.User.FullName,
                   type = ConstantHelpers.PAYMENT.TYPES.DESCRIPTION.ContainsKey(x.Type) ? ConstantHelpers.PAYMENT.TYPES.DESCRIPTION[x.Type] : "",
                   concept = x.Description,
                   totalamount = x.Total * 1.00M,
                   issueDate = x.IssueDate.ToShortDateString(),
                   paymentDate = x.PaymentDate.HasValue ? x.PaymentDate.Value.ToShortDateString() : "-",
               })
                .ToListAsync();

            var term = await _context.Terms.FirstOrDefaultAsync(x => x.Id == termId);
            if (term.Status == ConstantHelpers.TERM_STATES.FINISHED)
            {
                var qryStudents = _context.AcademicSummaries
                    .Where(x => x.TermId == termId)
                    .AsNoTracking();

                if (careerId.HasValue && careerId != Guid.Empty)
                    qryStudents = qryStudents.Where(x => x.CareerId == careerId);

                if (academicYear.HasValue && academicYear > 0)
                    qryStudents = qryStudents.Where(x => x.StudentAcademicYear == academicYear);

                var students = await qryStudents
                    .Select(x => new
                    {
                        x.Student.UserId,
                        x.Student.User.UserName,
                        x.Student.User.FullName,
                        x.StudentAcademicYear,
                        Career = x.Career.Name
                    })
                    .ToListAsync();

                var data = payments
                   .Where(x => students.Any(y => y.UserId == x.userId))
                   .Select(x => new StudentPaymentTemplate
                   {
                       User = x.user,
                       FullName = x.fullname,
                       Career = students.FirstOrDefault(y => y.UserId == x.userId).Career,
                       AcademicYear = students.FirstOrDefault(y => y.UserId == x.userId).StudentAcademicYear,
                       Concept = x.concept,
                       Amount = x.totalamount,
                   })
                    .ToList();

                return data;
            }
            else
            {
                var qryStudents = _context.Students
                    .AsNoTracking();

                if (careerId.HasValue && careerId != Guid.Empty)
                    qryStudents = qryStudents.Where(x => x.CareerId == careerId);

                if (academicYear.HasValue && academicYear > 0)
                    qryStudents = qryStudents.Where(x => x.CurrentAcademicYear == academicYear);

                var students = await qryStudents
                    .Select(x => new
                    {
                        x.UserId,
                        x.User.UserName,
                        x.User.FullName,
                        x.CurrentAcademicYear,
                        Career = x.Career.Name
                    })
                    .ToListAsync();

                var data = payments
                   .Where(x => students.Any(y => y.UserId == x.userId))
                   .Select(x => new StudentPaymentTemplate
                   {
                       User = x.user,
                       FullName = x.fullname,
                       Career = students.FirstOrDefault(y => y.UserId == x.userId).Career,
                       AcademicYear = students.FirstOrDefault(y => y.UserId == x.userId).CurrentAcademicYear,
                       Concept = x.concept,
                       Amount = x.totalamount,
                   })
                    .ToList();

                return data;
            }
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExoneratedPaymentsDatatableData(DataTablesStructs.SentParameters sentParameters, Guid termId, Guid? careerId = null, byte? type = null, string searchValue = null)
        {
            Expression<Func<Payment, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    //orderByPredicate = (x) => x.User.UserName;
                    break;
                case "3":
                    //orderByPredicate = (x) => x.User.UserName;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "5":
                    orderByPredicate = (x) => x.Total;
                    break;
                default:
                    break;
            }

            var query = _context.Payments
                .Where(x => x.TermId == termId && !string.IsNullOrEmpty(x.UserId)
                && x.WasExonerated && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .AsNoTracking();

            if (type.HasValue)
            {
                if (type.Value == 1) // pagos matricula
                    query = query.Where(x => x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT);
                if (type.Value == 2) // pagos matricula
                    query = query.Where(x => x.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && x.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT);
            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper())
                || x.User.UserName.ToUpper().Contains(searchValue.ToUpper())
                || x.User.FullName.ToUpper().Contains(searchValue.ToUpper()));

            var payments = await query
               .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               //.Skip(sentParameters.PagingFirstRecord)
               //.Take(sentParameters.RecordsPerDraw)
               .Select(x => new
               {
                   id = x.Id,
                   userId = x.UserId,
                   user = x.User.UserName,
                   fullname = x.User.FullName,
                   type = x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT ? "Matrícula" : "Otros",
                   concept = x.Description,
                   totalamount = x.Total * 1.00M,
                   issueDate = x.IssueDate.ToShortDateString(),
                   paymentDate = x.PaymentDate.HasValue ? x.PaymentDate.Value.ToShortDateString() : "-",

               })
               .ToListAsync();

            var qryStudents = _context.Students.AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
                qryStudents = qryStudents.Where(x => x.CareerId == careerId);

            var students = await qryStudents
                .Select(x => new
                {
                    x.UserId,
                    x.User.UserName,
                    x.User.FullName,
                    x.CurrentAcademicYear,
                    Career = x.Career.Name
                })
                .ToListAsync();

            var data = payments
               .Where(x => students.Any(y => y.UserId == x.userId))
               .Select(x => new
               {
                   x.id,
                   x.user,
                   x.fullname,
                   career = students.FirstOrDefault(y => y.UserId == x.userId).Career,
                   academicYear = students.FirstOrDefault(y => y.UserId == x.userId).CurrentAcademicYear,
                   x.type,
                   x.concept,
                   x.totalamount,
                   x.issueDate,
                   x.paymentDate,
               })
                .ToList();

            var recordsFiltered = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<StudentPaymentTemplate>> GetExoneratedPaymentsData(Guid termId, Guid? careerId = null, byte? type = null, string searchValue = null)
        {
            var query = _context.Payments
                .Where(x => x.TermId == termId && !string.IsNullOrEmpty(x.UserId)
                && x.WasExonerated && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .AsNoTracking();

            if (type.HasValue)
            {
                if (type.Value == 1) // pagos matricula
                    query = query.Where(x => x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT);
                if (type.Value == 2) // pagos matricula
                    query = query.Where(x => x.Type != ConstantHelpers.PAYMENT.TYPES.ENROLLMENT && x.Type != ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT);
            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Description.ToUpper().Contains(searchValue.ToUpper())
                || x.User.UserName.ToUpper().Contains(searchValue.ToUpper())
                || x.User.FullName.ToUpper().Contains(searchValue.ToUpper()));

            var recordsFiltered = await query.CountAsync();

            var payments = await query
               .Select(x => new
               {
                   id = x.Id,
                   userId = x.UserId,
                   user = x.User.UserName,
                   fullname = x.User.FullName,
                   type = x.Type == ConstantHelpers.PAYMENT.TYPES.ENROLLMENT || x.Type == ConstantHelpers.PAYMENT.TYPES.EXTEMPORANEOUS_ENROLLMENT ? "Matrícula" : "Otros",
                   concept = x.Description,
                   totalamount = x.Total * 1.00M,
                   issueDate = x.IssueDate.ToShortDateString(),
                   paymentDate = x.PaymentDate.HasValue ? x.PaymentDate.Value.ToShortDateString() : "-",
               })
                .ToListAsync();

            var qryStudents = _context.Students
                                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
                qryStudents = qryStudents.Where(x => x.CareerId == careerId);

            var students = await qryStudents
                .Select(x => new
                {
                    x.UserId,
                    x.User.UserName,
                    x.User.FullName,
                    x.CurrentAcademicYear,
                    Career = x.Career.Name
                })
                .ToListAsync();

            var data = payments
               .Where(x => students.Any(y => y.UserId == x.userId))
               .Select(x => new StudentPaymentTemplate
               {
                   User = x.user,
                   FullName = x.fullname,
                   Career = students.FirstOrDefault(y => y.UserId == x.userId).Career,
                   AcademicYear = students.FirstOrDefault(y => y.UserId == x.userId).CurrentAcademicYear,
                   Concept = x.concept,
                   Amount = x.totalamount,
                   Type = x.type,
               })
                .ToList();

            return data;
        }

        public async Task<List<IncomeReceiptTemplate>> GetClassifierReportData(DateTime start, DateTime end, int? type = null, bool publicSector = false)
        {
            var classifierCategoryDigits = 3;
            var classifierSubcategoryDigits = 6;
            var classifierChildDigits = 7;

            var classifiers = await _context.Classifiers
                .Where(x => !string.IsNullOrEmpty(x.Code))
                .Select(x => new
                {
                    x.Code,
                    x.Name
                })
                .ToListAsync();

            var categories = classifiers
                .Where(x => x.Code.Split('.').Length == classifierCategoryDigits)
                .ToList();

            var subcategories = classifiers
                .Where(x => x.Code.Split('.').Length == classifierSubcategoryDigits)
                .ToList();

            var qryPayments = _context.Payments.AsNoTracking();

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

            if (publicSector)
                qryPayments = qryPayments.Where(x => x.ExternalUserId.HasValue && x.ExternalUser.IsPublicSector);

            var data = await qryPayments
                .Where(x => x.PaymentDate.HasValue
                && start <= x.PaymentDate.Value && x.PaymentDate.Value <= end
                && x.ConceptId.HasValue && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .Select(x => new
                {
                    ClassifierCode = x.Concept.Classifier.Code,
                    Classifier = x.Concept.Classifier.Name,
                    PaymentDate = x.PaymentDate.Value,
                    x.Total,
                    x.IgvAmount
                })
                .ToListAsync();

            data = data
                .Where(x => x.ClassifierCode != null && (x.ClassifierCode.Split('.').Length == classifierSubcategoryDigits
                || x.ClassifierCode.Split('.').Length == classifierChildDigits))
                .ToList();

            var payments = data
                .Select(x => new
                {
                    ClassifierCategory = categories.Where(y => x.ClassifierCode.Contains($"{y.Code}.")).Select(y => y.Code).FirstOrDefault(),
                    ClassifierSubcategory = subcategories.Where(y => x.ClassifierCode.StartsWith($"{y.Code}.") || x.ClassifierCode == y.Code).Select(y => y.Code).FirstOrDefault(),
                    x.ClassifierCode,
                    x.Classifier,
                    PaymentDate = x.PaymentDate.ToDefaultTimeZone(),
                    x.Total,
                    x.IgvAmount
                })
                .ToList();
            payments = payments.Where(x => !string.IsNullOrEmpty(x.ClassifierCategory)).ToList();

            var days = payments
                .GroupBy(x => x.PaymentDate.Date)
                .Select(x => new IncomeReceiptTemplate
                {
                    Date = x.Key.Date,
                    Total = x.Sum(x => x.Total),
                    Categories = x
                        .GroupBy(y => y.ClassifierCategory)
                        .Select(y => new IncomeReceiptCategoryTemplate
                        {
                            Code = y.Key,
                            Name = categories.FirstOrDefault(p => p.Code == y.Key).Name,
                            Accounts = y
                                .GroupBy(z => z.ClassifierSubcategory)
                                .Select(z => new IncomeReceiptCategoryDetailTemplate
                                {
                                    Code = z.Key,
                                    Name = subcategories.FirstOrDefault(q => q.Code == z.Key).Name,
                                    Total = z.Sum(w => w.Total),
                                    //Details = z
                                    //    .GroupBy(a => new { a.ClassifierCode, a.Classifier })
                                    //    .Select(a => new ClassifierIncomeDetailTemplate
                                    //    {
                                    //        Code = a.Key.ClassifierCode,
                                    //        Name = a.Key.Classifier,
                                    //        Total = a.Sum(b => b.Total),
                                    //    })
                                    //    .OrderBy(a => a.Code)
                                    //    .ToList(),
                                })
                                .OrderBy(z => z.Code)
                                .ToList(),
                            Total = y.Sum(z => z.Total),
                        })
                        .OrderBy(y => $"{y.Code}.")
                        .ToList(),
                })
                .OrderBy(x => x.Date)
                .ToList();

            return days;
        }


        public async Task<IncomeReceiptTemplate> GetClassifierReportDataRange(DateTime start, DateTime end, int? type = null, bool publicSector = false)
        {
            var classifierCategoryDigits = 3;
            var classifierSubcategoryDigits = 6;
            var classifierChildDigits = 7;

            var classifiers = await _context.Classifiers
                .Where(x => !string.IsNullOrEmpty(x.Code))
                .Select(x => new
                {
                    x.Code,
                    x.Name
                })
                .ToListAsync();

            var categories = classifiers
                .Where(x => x.Code.Split('.').Length == classifierCategoryDigits)
                .ToList();

            var subcategories = classifiers
                .Where(x => x.Code.Split('.').Length == classifierSubcategoryDigits)
                .ToList();

            var qryPayments = _context.Payments.AsNoTracking();

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

            if (publicSector)
                qryPayments = qryPayments.Where(x => x.ExternalUserId.HasValue && x.ExternalUser.IsPublicSector);

            var data = await qryPayments
                .Where(x => x.PaymentDate.HasValue
                && start <= x.PaymentDate.Value && x.PaymentDate.Value <= end
                && x.ConceptId.HasValue && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID)
                .Select(x => new
                {
                    ClassifierCode = x.Concept.Classifier.Code,
                    Classifier = x.Concept.Classifier.Name,
                    PaymentDate = x.PaymentDate.Value,
                    x.Total,
                    x.IgvAmount
                })
                .ToListAsync();

            data = data.Where(x => x.ClassifierCode != null
            && (x.ClassifierCode.Split('.').Count() == classifierChildDigits || x.ClassifierCode.Split('.').Count() == classifierSubcategoryDigits))
                .ToList();

            var payments = data
                .Select(x => new
                {
                    ClassifierParent = categories.Where(y => x.ClassifierCode.Contains($"{y.Code}.")).Select(y => y.Code).FirstOrDefault(),
                    ClassifierSubcategory = subcategories.Where(y => x.ClassifierCode.StartsWith($"{y.Code}.") || x.ClassifierCode == y.Code).Select(y => y.Code).FirstOrDefault(),
                    x.ClassifierCode,
                    x.Classifier,
                    PaymentDate = x.PaymentDate.ToDefaultTimeZone(),
                    x.Total,
                    x.IgvAmount
                })
                .ToList();

            payments = payments.Where(x => !string.IsNullOrEmpty(x.ClassifierParent)).ToList();

            var result = new IncomeReceiptTemplate
            {
                Date = start.Date,
                Total = payments.Sum(x => x.Total),
                Categories = payments
                        .GroupBy(y => y.ClassifierParent)
                        .Select(y => new IncomeReceiptCategoryTemplate
                        {
                            Code = y.Key,
                            Name = categories.FirstOrDefault(p => p.Code == y.Key).Name,
                            Accounts = y
                                .GroupBy(z => z.ClassifierSubcategory)
                                .Select(z => new IncomeReceiptCategoryDetailTemplate
                                {
                                    Code = z.Key,
                                    Name = subcategories.FirstOrDefault(p => p.Code == z.Key).Name,
                                    Total = z.Sum(w => w.Total),
                                    IgvAmount = z.Sum(w => w.IgvAmount),
                                }).OrderBy(z => z.Code).ToList(),
                            Total = y.Sum(z => z.Total),
                        })
                        .OrderBy(y => $"{y.Code}.")
                        .ToList(),
            };

            return result;
        }


        public async Task<DataTablesStructs.ReturnedData<object>> GetPostulantPaymentDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId, Guid? careerId, Guid? admissionTypeId)
        {
            var query = _context.Postulants
                .Where(x => x.ApplicationTermId == applicationTermId)
                .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (admissionTypeId.HasValue && admissionTypeId != Guid.Empty)
                query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            var postulants = await query
                .Select(x => new
                {
                    x.Id,
                    x.Document,
                    x.FullName,
                    Career = x.Career.Name,
                    AdmissionType = x.AdmissionType.Name
                })
                .ToListAsync();

            var postulantHash = postulants.Select(x => x.Id).ToHashSet();

            var payments = await _context.Payments
                .Where(x => /*x.ExternalUserId.HasValue && */x.UserId == null && x.EntityId.HasValue && x.ConceptId.HasValue && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.PaymentDate.HasValue)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.Total,
                    x.PaymentDate,
                    EntityId = x.EntityId.Value,
                    x.OperationCodeB,
                    Series = x.InvoiceId.HasValue ? x.Invoice.Series : "",
                    Number = x.InvoiceId.HasValue ? x.Invoice.Number : 0
                })
                .ToListAsync();

            var data = payments
                .Where(x => postulantHash.Contains(x.EntityId))
                .Select(x => new
                {
                    document = postulants.FirstOrDefault(y => y.Id == x.EntityId).Document,
                    name = postulants.FirstOrDefault(y => y.Id == x.EntityId).FullName,
                    career = postulants.FirstOrDefault(y => y.Id == x.EntityId).Career,
                    admissionType = postulants.FirstOrDefault(y => y.Id == x.EntityId).AdmissionType,
                    description = x.Description,
                    total = x.Total,
                    date = x.PaymentDate.ToLocalDateFormat(),
                    invoice = string.IsNullOrEmpty(x.OperationCodeB) ? $"{x.Series}-{x.Number:000000}" : x.OperationCodeB
                })
                .ToList();

            var recordsFiltered = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<PostulantPaymentTemplate>> GetPostulantPaymentData(Guid applicationTermId, Guid? careerId, Guid? admissionTypeId)
        {
            var query = _context.Postulants
               .Where(x => x.ApplicationTermId == applicationTermId)
               .AsNoTracking();

            if (careerId.HasValue && careerId != Guid.Empty)
                query = query.Where(x => x.CareerId == careerId);

            if (admissionTypeId.HasValue && admissionTypeId != Guid.Empty)
                query = query.Where(x => x.AdmissionTypeId == admissionTypeId);

            var postulants = await query
                .Select(x => new
                {
                    x.Id,
                    x.Document,
                    x.FullName,
                    Career = x.Career.Name,
                    AdmissionType = x.AdmissionType.Name,
                    ApplicationTerm = x.ApplicationTerm.Name
                })
                .ToListAsync();

            var postulantHash = postulants.Select(x => x.Id).ToHashSet();

            var payments = await _context.Payments
                .Where(x => /*x.ExternalUserId.HasValue &&*/x.UserId == null && x.EntityId.HasValue && x.ConceptId.HasValue && x.Status == ConstantHelpers.PAYMENT.STATUS.PAID && x.PaymentDate.HasValue)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.Total,
                    x.PaymentDate,
                    EntityId = x.EntityId.Value,
                    x.OperationCodeB,
                    x.Invoice.Series,
                    x.Invoice.Number
                })
                .ToListAsync();

            var data = payments
                .Where(x => postulantHash.Contains(x.EntityId))
                .Select(x => new PostulantPaymentTemplate
                {
                    Document = postulants.FirstOrDefault(y => y.Id == x.EntityId).Document,
                    FullName = postulants.FirstOrDefault(y => y.Id == x.EntityId).FullName,
                    Career = postulants.FirstOrDefault(y => y.Id == x.EntityId).Career,
                    AdmissionType = postulants.FirstOrDefault(y => y.Id == x.EntityId).AdmissionType,
                    ApplicationTerm = postulants.FirstOrDefault(y => y.Id == x.EntityId).ApplicationTerm,
                    Description = x.Description,
                    Total = x.Total,
                    PaymentDate = x.PaymentDate.ToLocalDateFormat(),
                    Invoice = string.IsNullOrEmpty(x.OperationCodeB) ? $"{x.Series}-{x.Number:000000}" : x.OperationCodeB
                })
                .ToList();

            return data;
        }

        public async Task<object> GetCashierDailyIncomeDatatableData(DateTime date, string userId)
        {
            var start = date.Date.ToUtcDateTime();
            var end = start.AddDays(1).AddTicks(-1);

            var firstDayMonth = (new DateTime(start.Year, start.Month, 1)).ToUtcDateTime();

            var payments = await _context.Payments
                .Where(x => x.InvoiceId.HasValue
                && x.Invoice.PettyCash.UserId == userId
                && x.ConceptId.HasValue
                && x.PaymentDate >= firstDayMonth && x.PaymentDate <= end)
                .Select(x => new
                {
                    x.Concept.ClassifierId,
                    x.Concept.Classifier.Code,
                    x.Concept.Classifier.Name,
                    x.PaymentDate,
                    x.Total
                })
                .ToListAsync();

            var data = payments
                .GroupBy(x => new { x.ClassifierId, x.Code, x.Name })
                .Select(x => new
                {
                    id = x.Key.ClassifierId,
                    code = x.Key.Code,
                    name = x.Key.Name,
                    prevAmount = x.Where(y => y.PaymentDate >= firstDayMonth && y.PaymentDate < start).Sum(y => y.Total),
                    amount = x.Where(y => y.PaymentDate >= start && y.PaymentDate <= end).Sum(y => y.Total),
                    total = x.Sum(y => y.Total)
                })
                .OrderBy(x => x.code)
                .ToList();

            return data;
        }


        public async Task<CashierDailyIncomeTemplate> GetCashierDailyIncomeData(DateTime date, string userId)
        {
            var start = date.Date.ToUtcDateTime();
            var end = start.AddDays(1).AddTicks(-1);

            var firstDayMonth = (new DateTime(start.Year, start.Month, 1)).ToUtcDateTime();

            var payments = await _context.Payments
                .Where(x => x.InvoiceId.HasValue
                && x.Invoice.PettyCash.UserId == userId
                && x.ConceptId.HasValue
                && x.PaymentDate >= firstDayMonth && x.PaymentDate <= end)
                .Select(x => new
                {
                    x.Concept.ClassifierId,
                    x.Concept.Classifier.Code,
                    x.Concept.Classifier.Name,
                    x.PaymentDate,
                    x.Total,
                    //x.Invoice.Series,
                    x.Invoice.Number
                })
                .ToListAsync();

            var data = payments
                .GroupBy(x => new { x.ClassifierId, x.Code, x.Name })
                .Select(x => new CashierDailyIncomeDetailTemplate
                {
                    Code = x.Key.Code,
                    Name = x.Key.Name,
                    PrevAmount = x.Where(y => y.PaymentDate >= firstDayMonth && y.PaymentDate < start).Sum(y => y.Total),
                    Amount = x.Where(y => y.PaymentDate >= start && y.PaymentDate <= end).Sum(y => y.Total),
                    Total = x.Sum(y => y.Total)
                })
                .OrderBy(x => x.Code)
                .ToList();

            var model = new CashierDailyIncomeTemplate
            {
                Details = data
            };

            model.FirstSeries = payments
                .Where(y => y.PaymentDate >= start && y.PaymentDate <= end)
                //.OrderBy(y => y.PaymentDate)
                //.Select(x => x.Series)
                .OrderBy(y => y.Number)
                .Select(x => $"{x.Number:000000}")
                .FirstOrDefault();

            model.LastSeries = payments
                .Where(y => y.PaymentDate >= start && y.PaymentDate <= end)
                //.OrderByDescending(y => y.PaymentDate)
                //.Select(x => x.Series)
                .OrderByDescending(y => y.Number)
                .Select(x => $"{x.Number:000000}")
                .FirstOrDefault();

            return model;
        }
    }
}