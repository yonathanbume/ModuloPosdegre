using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.DocumentaryProcedure.Implementations
{
    public class InternalProcedureRepository : Repository<InternalProcedure>, IInternalProcedureRepository
    {
        public InternalProcedureRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE

        private async Task<InternalProcedure> LastSearchTreeInternalProcedure(int? searchTree = null)
        {
            var query = _context.InternalProcedures.AsQueryable();

            if (searchTree != null)
            {
                query = query.Where(x => x.SearchTree == searchTree);
            }

            query = query
                .OrderByDescending(x => x.Number)
                .OrderByDescending(x => x.SearchTree)
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        private async Task<InternalProcedure> LastYearlyInternalProcedure(Guid? dependencyId = null, Guid? documentTypeId = null)
        {
            var query = _context.InternalProcedures.AsQueryable();

            if (dependencyId != null)
            {
                query = query.Where(x => x.DependencyId == dependencyId);
            }

            if (documentTypeId != null)
            {
                query = query.Where(x => x.DocumentTypeId == documentTypeId);
            }

            query = query
                .Where(x => x.CreatedAt.HasValue && x.CreatedAt.Value.Year == DateTime.UtcNow.Year)
                .OrderByDescending(x => x.Number)
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        private async Task<IEnumerable<InternalProcedure>> GetChildInternalProcedures(Guid id, Guid? dependencyId = null, List<InternalProcedure> result = null)
        {
            if (result == null)
            {
                result = new List<InternalProcedure>();
            }

            var internalProcedure = _context.InternalProcedures.Where(x => x.InternalProcedureParentId == id);

            if (dependencyId != null)
            {
                internalProcedure = internalProcedure.Where(x => x.DependencyId == dependencyId);
            }

            var internalProcedure2 = await internalProcedure
                .SelectInternalProcedure(_context)
                .FirstOrDefaultAsync();

            if (internalProcedure2 != null)
            {
                result.Add(internalProcedure2);

                var userInternalProcedures2 = await _context.UserInternalProcedures
                    .Where(x => x.InternalProcedureId == internalProcedure2.Id)
                    .OrderByDescending(x => x.GeneratedId)
                    .ToListAsync();

                for (var i = 0; i < userInternalProcedures2.Count; i++)
                {
                    await GetChildInternalProcedures(userInternalProcedures2[i].InternalProcedureId, dependencyId, result);
                }
            }

            return result;
        }

        private async Task<IEnumerable<InternalProcedure>> GetParentInternalProcedures(Guid id, Guid? dependencyId = null)
        {
            List<InternalProcedure> result = new List<InternalProcedure>();
            var internalProcedure = await _context.InternalProcedures
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            var internalProcedureId = internalProcedure?.InternalProcedureParentId;

            do
            {
                if (internalProcedureId != null)
                {
                    var internalProcedure2 = _context.InternalProcedures.Where(x => x.Id == internalProcedureId);

                    if (dependencyId != null)
                    {
                        internalProcedure2 = internalProcedure2.Where(x => x.DependencyId == dependencyId);
                    }

                    var internalProcedure3 = await internalProcedure2
                        .SelectInternalProcedure(_context)
                        .FirstOrDefaultAsync();
                    internalProcedureId = internalProcedure3.InternalProcedureParentId ?? Guid.Empty;

                    result.Insert(0, internalProcedure3);
                }
                else
                {
                    break;
                }
            }
            while (internalProcedureId != Guid.Empty);

            return result;
        }


        #endregion

        #region PUBLIC

        public async Task<int> CountParentInternalProcedures(Guid id)
        {
            var result = 0;
            var internalProcedure = await _context.InternalProcedures
                .Where(x => x.Id == id)
                .Select(x => new InternalProcedure
                {
                    InternalProcedureParentId = x.InternalProcedureParentId
                })
                .FirstOrDefaultAsync();

            if (internalProcedure != null)
            {
                result++;

                while (internalProcedure.InternalProcedureParentId != null)
                {
                    result++;
                    internalProcedure = await _context.InternalProcedures
                        .Where(x => x.Id == internalProcedure.InternalProcedureParentId)
                        .Select(x => new InternalProcedure
                        {
                            InternalProcedureParentId = x.InternalProcedureParentId
                        })
                        .FirstOrDefaultAsync();
                }
            }

            return result;
        }

        public async Task<int> CountYearlyInternalProceduresByDocumentType(Guid documentTypeId)
        {
            var query = _context.InternalProcedures
                .Where(x => x.DocumentTypeId == documentTypeId && x.CreatedAt.HasValue && x.CreatedAt.Value.Year == DateTime.UtcNow.Year)
                .AsQueryable();

            return await query.CountAsync();
        }

        public async Task<InternalProcedure> FirstSearchTreeInternalProcedureBySearchTree(int searchTree)
        {
            var query = _context.InternalProcedures
                .Where(x => x.InternalProcedureParentId == null)
                .Where(x => x.SearchTree == searchTree)
                .SelectInternalProcedure()
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<InternalProcedure> LastSearchTreeInternalProcedureBySearchTree(int searchTree)
        {
            return await LastSearchTreeInternalProcedure(searchTree);
        }

        public async Task<InternalProcedure> LastSearchTreeInternalProcedure()
        {
            return await LastSearchTreeInternalProcedure();
        }

        public async Task<InternalProcedure> LastYearlyInternalProcedureByDependency(Guid dependencyId)
        {
            return await LastYearlyInternalProcedure(dependencyId);
        }

        public async Task<InternalProcedure> LastYearlyInternalProcedureByDependency(Guid dependencyId, Guid documentTypeId)
        {
            return await LastYearlyInternalProcedure(dependencyId, documentTypeId);
        }

        public async Task<InternalProcedure> LastYearlyInternalProcedureByDocumentType(Guid documentTypeId)
        {
            return await LastYearlyInternalProcedure(null, documentTypeId);
        }

        public async Task<InternalProcedure> LastYearlyInternalProcedureByDocumentType(Guid documentTypeId, Guid dependencyId)
        {
            return await LastYearlyInternalProcedure(dependencyId, documentTypeId);
        }

        public async Task<string> GetNextLastYearlyInternalCodeByDependencyAndDocumentType(Guid dependencyId, Guid documentTypeId) // delete later
        {
            var lastNumberProcedure = await _context.InternalProcedures
                .Where(x => x.DocumentTypeId == documentTypeId && x.CreatedAt.HasValue && x.CreatedAt.Value.Year == DateTime.UtcNow.Year)
                .OrderByDescending(x => x.Number)
                .Select(x => x.Number)
                .FirstOrDefaultAsync();

            var dependency = await _context.Dependencies.Where(x => x.Id == dependencyId).FirstOrDefaultAsync();
            var documentType = await _context.DocumentTypes.Where(x => x.Id == documentTypeId).FirstOrDefaultAsync();

            var code = $"I-{documentType?.Code.ToUpper()}-{(lastNumberProcedure + 1)}-{DateTime.UtcNow.Year}-{GeneralHelpers.GetInstitutionAbbreviation()}-{dependency?.Acronym.ToUpper()}";
            return code;
        }

        public async Task<InternalProcedure> GetInternalProcedure(Guid id)
        {
            var query = _context.InternalProcedures
                .Where(x => x.Id == id)
                .SelectInternalProcedure()
                .AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<InternalProcedure> GetInternalProcedureByUserExternalProcedure(Guid userExternalProcedureId)
        {
            var userExternalProcedure = await _context.UserExternalProcedures
                .Where(x => x.Id == userExternalProcedureId)
                .FirstOrDefaultAsync();
            var result = await _context.InternalProcedures
                .Where(x => x.Id == userExternalProcedure.InternalProcedureId)
                .SelectInternalProcedure(_context)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<InternalProcedure>> GetChildInternalProcedures(Guid id)
        {
            var result = await GetChildInternalProcedures(id);

            return result;
        }

        public async Task<IEnumerable<InternalProcedure>> GetChildInternalProceduresByUserExternalProcedure(Guid userExternalProcedureId)
        {
            var userExternalProcedure = await _context.UserExternalProcedures
                .Where(x => x.Id == userExternalProcedureId)
                .SelectUserExternalProcedure()
                .FirstOrDefaultAsync();
            IEnumerable<InternalProcedure> result = null;

            if (userExternalProcedure?.InternalProcedure != null)
            {
                result = await GetChildInternalProcedures(userExternalProcedure.InternalProcedureId.Value, null);
            }

            return result;
        }

        public async Task<IEnumerable<InternalProcedure>> GetDraftInternalProcedures()
        {
            var query = _context.InternalProcedures
                .Where(x => !x.UserInternalProcedures.Any())
                .SelectInternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<InternalProcedure>> GetDraftInternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null)
        {
            var query = _context.InternalProcedures
                .Where(x => !x.UserInternalProcedures.Any())
                .Where(x => searchValuePredicate.Item1(x).Contains(searchValuePredicate.Item2))
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectInternalProcedure()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<InternalProcedure>> GetDraftInternalProceduresByUser(string userId)
        {
            var query = _context.InternalProcedures
                .Where(x => !x.UserInternalProcedures.Any())
                .Where(x => x.UserId == userId)
                .SelectInternalProcedure()
                .OrderByDescending(x => x.GeneratedId)
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<InternalProcedure>> GetDraftInternalProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null)
        {
            var query = _context.InternalProcedures
                .Where(x => !x.UserInternalProcedures.Any())
                .Where(x => x.UserId == userId);

            if (!string.IsNullOrEmpty(searchValuePredicate.Item2))
                query = query
                .Where(x => searchValuePredicate.Item1(x).Contains(searchValuePredicate.Item2));


            query = query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectInternalProcedure()
                .AsNoTracking();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDraftInternalProceduresDatatableToManagement(DataTablesStructs.SentParameters parameters, string userId, string search)
        {
            var query = _context.InternalProcedures
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => !x.UserInternalProcedures.Any())
                .Where(x => x.UserId == userId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Subject.ToLower().Trim().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .SelectInternalProcedure()
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<InternalProcedure>> GetParentInternalProcedures(Guid id)
        {
            var result = await GetParentInternalProcedures(id, null);

            return result;
        }

        public async Task<IEnumerable<InternalProcedure>> GetParentInternalProceduresByUserExternalProcedure(Guid userExternalProcedureId)
        {
            var userExternalProcedure = await _context.UserExternalProcedures.Where(x => x.Id == userExternalProcedureId).FirstOrDefaultAsync();
            IEnumerable<InternalProcedure> result = null;

            if (userExternalProcedure?.InternalProcedure != null)
            {
                result = await GetParentInternalProcedures(userExternalProcedure.InternalProcedureId.Value, null);
            }

            return result;
        }

        public async Task<IEnumerable<InternalProcedure>> GetPreviewInternalProcedures(Guid id)
        {
            List<InternalProcedure> result = new List<InternalProcedure>();
            var internalProcedure = await _context.InternalProcedures
                .Where(x => x.Id == id)
                .SelectInternalProcedure(_context)
                .FirstOrDefaultAsync();

            result.Add(internalProcedure);

            if (internalProcedure.InternalProcedureParentId != null)
            {
                var internalProcedureParent = await _context.InternalProcedures
                    .Where(x => x.Id == internalProcedure.InternalProcedureParentId)
                    .SelectInternalProcedure(_context)
                    .FirstOrDefaultAsync();

                result.Add(internalProcedureParent);

                if (internalProcedureParent.InternalProcedureParentId != null)
                {
                    var internalProcedure2 = await _context.InternalProcedures
                        .Where(x => x.InternalProcedureParentId == null && x.SearchTree == internalProcedureParent.SearchTree)
                        .SelectInternalProcedure(_context)
                        .FirstOrDefaultAsync();

                    result.Add(internalProcedure2);
                }
            }

            return result;
        }

        public async Task<IEnumerable<InternalProcedure>> GetRemainingInternalProcedures(Guid id)
        {
            var result = new List<InternalProcedure>();
            var internalProcedure = await _context.InternalProcedures
                .Where(x => x.Id == id)
                .SelectInternalProcedure(_context)
                .FirstOrDefaultAsync();
            var internalProcedureId = internalProcedure?.InternalProcedureParent?.InternalProcedureParentId;

            do
            {
                if (internalProcedureId != null)
                {
                    internalProcedure = await _context.InternalProcedures
                        .Where(x => x.Id == internalProcedureId)
                        .SelectInternalProcedure(_context)
                        .FirstOrDefaultAsync();
                    internalProcedureId = internalProcedure.InternalProcedureParentId ?? Guid.Empty;

                    result.Add(internalProcedure);
                }
                else
                {
                    break;
                }
            }
            while (internalProcedureId != Guid.Empty);

            return result.OrderByDescending(x => x.CreatedAt);
        }

        public async Task<IEnumerable<InternalProcedure>> GetSentInternalProcedures()
        {
            var query = _context.InternalProcedures
                .Where(x => x.UserInternalProcedures.Any())
                .SelectInternalProcedure()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<InternalProcedure>> GetSentInternalProceduresDatatable(DataTablesStructs.SentParameters sentParameters, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null)
        {
            var query = _context.InternalProcedures
                .Where(x => x.UserInternalProcedures.Any())
                .Where(x => searchValuePredicate.Item1(x).Contains(searchValuePredicate.Item2))
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectInternalProcedure()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<InternalProcedure>> GetSentInternalProceduresByUser(string userId)
        {
            var query = _context.InternalProcedures
                .Where(x => x.UserInternalProcedures.Any())
                .Where(x => x.UserId == userId)
                .SelectInternalProcedure()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<InternalProcedure>> GetSentInternalProceduresDatatableByUser(DataTablesStructs.SentParameters sentParameters, string userId, Tuple<Func<InternalProcedure, string[]>, string> searchValuePredicate = null)
        {
            var query = _context.InternalProcedures
                .Where(x => x.UserInternalProcedures.Any())
                .Where(x => x.UserId == userId)
                .Where(x => searchValuePredicate.Item1(x).Contains(searchValuePredicate.Item2))
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .SelectInternalProcedure()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<InternalProcedure>> GetInternalProceduresByUser(string userId)
        {
            var query = _context.InternalProcedures
                .Where(x => x.UserId == userId)
                .SelectInternalProcedure()
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task InsertInternalProcedure(InternalProcedure internalProcedure)
        {
            var internalProcedure2 = await LastYearlyInternalProcedure(internalProcedure.DependencyId, internalProcedure.DocumentTypeId);
            var number = internalProcedure2 != null ? internalProcedure2.Number : 0;
            var searchNode = -1;
            var searchTree = -1;

            if (internalProcedure.InternalProcedureParentId != null)
            {
                var tmpInternalProcedure = await _context.InternalProcedures.Where(x => x.Id == internalProcedure.InternalProcedureParentId).FirstOrDefaultAsync();
                searchTree = tmpInternalProcedure.SearchTree;
            }

            var internalProcedure3 = await LastSearchTreeInternalProcedure(searchTree);
            searchNode = internalProcedure3 != null ? internalProcedure3.SearchNode + 1 : 0;

            if (searchTree < 0)
            {
                var internalProcedure4 = await LastSearchTreeInternalProcedure(null);
                searchTree = internalProcedure4 != null ? internalProcedure4.SearchTree + 1 : 0;
            }

            internalProcedure.Number = number + 1;
            internalProcedure.SearchNode = searchNode;
            internalProcedure.SearchTree = searchTree;

            await Insert(internalProcedure);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternalProceduresToAcademicRecordByUserDatatable(DataTablesStructs.SentParameters sentParameters, ClaimsPrincipal user,
            string search = null, Guid? careerId = null, string startdate = null, string enddate = null, int? type = null, bool? onlyObserved = null, int? status = null)
        {
            Expression<Func<RecordHistory, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {

            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = _context.RecordHistories.AsQueryable();

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD_STAFF))
            {
                //query = query.Where(x => x.InternalProcedure.UserInternalProcedures.Any(y => y.UserId == userId));
                query = query.Where(x => x.DerivedUserId == userId || string.IsNullOrEmpty(x.DerivedUserId));
            }

            if (user.IsInRole(ConstantHelpers.ROLES.ACADEMIC_RECORD))
            {
                if (careerId.HasValue)
                    query = query.Where(x => x.Student.CareerId == careerId);

                if (!string.IsNullOrEmpty(startdate))
                {
                    var startdateUTC = ConvertHelpers.DatepickerToUtcDateTime(startdate);
                    query = query.Where(x => x.Date.Date >= startdateUTC.Date);
                }

                if (!string.IsNullOrEmpty(enddate))
                {
                    var enddateUTC = ConvertHelpers.DatepickerToUtcDateTime(enddate);
                    query = query.Where(x => x.Date.Date <= enddateUTC.Date);
                }

                if (type.HasValue)
                {
                    query = query.Where(x => x.Type == type.Value);
                }

                //if (onlyObserved.HasValue && onlyObserved.Value)
                //query = query.Where(x => x.UserProcedure.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.OBSERVATION);
            }

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            //if (!string.IsNullOrEmpty(search))
            //query = query.Where(x => x.UserProcedure.Code.Trim().ToLower().Contains(search.Trim().ToLower()));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    username = x.Student.User.UserName,
                    student = x.Student.User.FullName,
                    code = $"INTE-{ConstantHelpers.RECORDS.VALUES[x.Type].ToUpper()}-{x.Number}-{x.Date.Year.ToString()}",
                    subject = x.Code,
                    isFinished = x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED,
                    isGenerated = x.Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.GENERATED,
                    //userInternalProcedureId = x.UserProcedureId,
                    statusInt = x.Status,
                    status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ALLVALUES.ContainsKey(x.Status) ?
                    ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ALLVALUES[x.Status] : "-",
                    hasReceipt = string.IsNullOrEmpty(x.ReceiptCode) ? false : true,
                    receipt = x.ReceiptCode ?? "-"
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInternalProceduresToStudentbyUserId(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            //Expression<Func<RecordHistory, dynamic>> orderByPredicate = null;
            //switch (sentParameters.OrderColumn)
            //{
            //    case "0":
            //        orderByPredicate = ((x) => x.InternalProcedure.CreatedAt); break;
            //    case "1":
            //        orderByPredicate = ((x) => x.InternalProcedure.Subject); break;
            //    default:
            //        orderByPredicate = ((x) => x.InternalProcedure.CreatedAt); break;
            //}

            //var query = _context.RecordHistories.AsQueryable();
            //query = query.Where(x => x.Student.UserId == userId);

            //int recordsFiltered = await query.CountAsync();
            //var data = await query
            //    .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
            //    .Skip(sentParameters.PagingFirstRecord)
            //    .Take(sentParameters.RecordsPerDraw)
            //    .Select(x => new
            //    {
            //        id = x.Id,
            //        createdAt = x.Date.ToLocalDateFormat(),
            //        academicRecord = x.InternalProcedure.UserInternalProcedures.FirstOrDefault().User.FullName,
            //        code = $"INTE-{x.InternalProcedure.DocumentType.Name.ToUpper()}-{x.InternalProcedure.Number}-{(x.InternalProcedure.CreatedAt.HasValue ? x.InternalProcedure.CreatedAt.Value.Year.ToString() : string.Empty)}",
            //        subject = x.InternalProcedure.Subject,
            //        isFinished = x.InternalProcedure.UserInternalProcedures./*Where(y => y.UserId == userId).Select(y => y.Status).*/FirstOrDefault().Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED,
            //        userInternalProcedureId = x.InternalProcedure.UserInternalProcedures/*.Where(y=>y.UserId == userId).Select(y=>y.Id)*/.FirstOrDefault().Id,
            //        statusInt = x.InternalProcedure.UserInternalProcedures/*.Where(y=>y.UserId == userId).Select(y=>y.Id)*/.FirstOrDefault().Status,
            //        status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ALLVALUES[x.InternalProcedure.UserInternalProcedures/*.Where(y => y.UserId == userId).Select(y => y.Status)*/.FirstOrDefault().Status]
            //    }).ToListAsync();

            //int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = null,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };
        }

        public async Task<int> GetInternalProcedureCountByMonthAndUserId(int month, string userId)
        {
            var today = DateTime.UtcNow;
            var result = await _context.InternalProcedures.Where(x => x.CreatedAt.Value.Month == month && x.CreatedAt.Value.Year == today.Year && x.UserId == userId).CountAsync();
            return result;
        }

        private async Task<IEnumerable<Dependency>> GetDependenciesByUserDependencyUser(string userDependencyUserId)
        {
            var query = _context.UserDependencies
                .Where(x => x.UserId == userDependencyUserId)
                .Select(x => new Dependency
                {
                    Id = x.DependencyId
                })
                .AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetExternalAndInternalProcedure(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            //Expression<Func<RecordHistory, dynamic>> orderByPredicate = null;
            //switch (sentParameters.OrderColumn)
            //{
            //    case "0":
            //        orderByPredicate = ((x) => x.InternalProcedure.CreatedAt); break;
            //    case "1":
            //        orderByPredicate = ((x) => x.InternalProcedure.Subject); break;
            //    default:
            //        orderByPredicate = ((x) => x.InternalProcedure.CreatedAt); break;
            //}

            var queryInternal = _context.InternalProcedures
                    .Where(x => x.UserInternalProcedures.Any())
                    .Where(x => x.UserId == userId)
                    .SelectInternalProcedure()
                    .AsQueryable();

            //var dataInternal = await queryInternal
            // .Skip(sentParameters.PagingFirstRecord)
            // .Take(sentParameters.RecordsPerDraw)
            // .Select(x => new {
            //     subject = x.Subject,
            //     priority = x.Priority,
            //     destination = x.Dependency.Name,
            //     code = x.Code,
            //     duration = x.Duration,
            //     dateCreate = x.ParsedCreatedAt,
            //     dateSent = x.ParsedCreatedAt
            // })
            // .ToListAsync();


            var queryExternal = _context.UserExternalProcedures.AsNoTracking();

            var dependencies = await GetDependenciesByUserDependencyUser(userId);
            queryExternal = queryExternal.Where(x => x.DependencyId == null || dependencies.Contains(x.Dependency));

            var dataExternal = await queryExternal
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    subject = x.UserExternalProcedureRecords.Where(u => u.UserExternalProcedureId == x.Id).Select(u => u.Subject),
                    priority = x.UserExternalProcedureRecords.Where(u => u.UserExternalProcedureId == x.Id).Select(u => u.RecordSubjectTypeId),
                    destination = x.ExternalProcedure.Dependency.Name,
                    code = x.UserExternalProcedureRecords.Where(u => u.UserExternalProcedureId == x.Id).Select(u => u.FullRecordNumber),
                    duration = "3 días",
                    dateCreate = x.UserExternalProcedureRecords.Where(u => u.UserExternalProcedureId == x.Id).Select(u => u.EntryDate),
                    dateSent = x.UserExternalProcedureRecords.Where(u => u.UserExternalProcedureId == x.Id).Select(u => u.EntryDate),
                })
                .ToListAsync();

            //int recordsFiltered = await query.CountAsync();
            //var data = await query
            //    .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
            //    .Skip(sentParameters.PagingFirstRecord)
            //    .Take(sentParameters.RecordsPerDraw)
            //    .Select(x => new
            //    {
            //        id = x.Id,
            //        createdAt = x.Date.ToLocalDateFormat(),
            //        academicRecord = x.InternalProcedure.UserInternalProcedures.FirstOrDefault().User.FullName,
            //        code = $"INTE-{x.InternalProcedure.DocumentType.Name.ToUpper()}-{x.InternalProcedure.Number}-{(x.InternalProcedure.CreatedAt.HasValue ? x.InternalProcedure.CreatedAt.Value.Year.ToString() : string.Empty)}",
            //        subject = x.InternalProcedure.Subject,
            //        isFinished = x.InternalProcedure.UserInternalProcedures./*Where(y => y.UserId == userId).Select(y => y.Status).*/FirstOrDefault().Status == ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ACCEPTED,
            //        userInternalProcedureId = x.InternalProcedure.UserInternalProcedures/*.Where(y=>y.UserId == userId).Select(y=>y.Id)*/.FirstOrDefault().Id,
            //        statusInt = x.InternalProcedure.UserInternalProcedures/*.Where(y=>y.UserId == userId).Select(y=>y.Id)*/.FirstOrDefault().Status,
            //        status = ConstantHelpers.USER_INTERNAL_PROCEDURES.STATUS.ALLVALUES[x.InternalProcedure.UserInternalProcedures/*.Where(y => y.UserId == userId).Select(y => y.Status)*/.FirstOrDefault().Status]
            //    }).ToListAsync();

            //int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = null,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = 0,
                RecordsTotal = 0
            };
        }
        #endregion
    }
}
