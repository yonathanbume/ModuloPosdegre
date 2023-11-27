using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Base;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Resolution;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleResolutionRepository : Repository<ScaleResolution>, IScaleResolutionRepository
    {
        public ScaleResolutionRepository(AkdemicContext context) : base(context) { }

        public async Task<int> GetScaleResolutionsQuantity(string userId, Guid sectionId, Guid resolutionTypeId, string search)
        {
            var sectionResolutionType = await _context.ScaleSectionResolutionTypes.FirstOrDefaultAsync(x =>
                x.ScaleResolutionTypeId == resolutionTypeId && x.ScaleSectionId == sectionId);

            var records = await _context.ScaleResolutions
                .Where(x => (string.IsNullOrWhiteSpace(search)  || x.ResolutionNumber.Contains(search)) &&
                    x.UserId == userId && x.ScaleSectionResolutionTypeId == sectionResolutionType.Id)
                .CountAsync();

            return records;
        }

        public async Task<bool> DeleteBySection(Guid id,byte sectionNumber)
        {
            var resolution = await _context.ScaleResolutions.FirstOrDefaultAsync(x => x.Id == id);
            var section = await _context.ScaleSections.FirstOrDefaultAsync(x => x.SectionNumber == sectionNumber);

            if (section == null || resolution == null) return false;

            var scaleSectionResolutionType = await _context.ScaleSectionResolutionTypes
                .FirstOrDefaultAsync(x => x.Id == resolution.ScaleSectionResolutionTypeId);

            if (scaleSectionResolutionType == null) return false;

            var isActive = await _context.ScaleSectionResolutionTypes
                .AnyAsync(x => x.ScaleSectionId == section.Id 
                && x.ScaleResolutionTypeId == scaleSectionResolutionType.ScaleResolutionTypeId 
                && x.Status == ConstantHelpers.STATES.ACTIVE);

            if (!isActive) return false;

            switch (sectionNumber)
            {
                case ConstantHelpers.RESOLUTION_SECTIONS.CONTRACTS:
                    var contractField = await _context.ScaleExtraContractFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == id);                    
                    _context.ScaleExtraContractFields.Remove(contractField);
                    break;
                case ConstantHelpers.RESOLUTION_SECTIONS.PERFORMANCE_EVALUATION:
                    var evaluationField = await _context.ScaleExtraPerformanceEvaluationFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == id);
                    _context.ScaleExtraPerformanceEvaluationFields.Remove(evaluationField);
                    break;
                case ConstantHelpers.RESOLUTION_SECTIONS.DISPLACEMENT:
                    var displacementField = await _context.ScaleExtraDisplacementFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == id);
                    _context.ScaleExtraDisplacementFields.Remove(displacementField);
                    break;
                case ConstantHelpers.RESOLUTION_SECTIONS.MERIT:
                    var meritField = await _context.ScaleExtraMeritFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == id);
                    _context.ScaleExtraMeritFields.Remove(meritField);
                    break;
                case ConstantHelpers.RESOLUTION_SECTIONS.PROFESSIONAL_EXPERIENCE:
                    var experienceField = await _context.ScaleExtraInstitutionExperienceFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == id);
                    _context.ScaleExtraInstitutionExperienceFields.Remove(experienceField);
                    break;
                case ConstantHelpers.RESOLUTION_SECTIONS.BENEFITS:
                    var benefitField = await _context.ScaleExtraBenefitFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == id);
                    _context.ScaleExtraBenefitFields.Remove(benefitField);
                    break;
                case ConstantHelpers.RESOLUTION_SECTIONS.INVESTIGATION:
                    var investigationField = await _context.ScaleExtraInvestigationFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == id);
                    _context.ScaleExtraInvestigationFields.Remove(investigationField);
                    break;
                case ConstantHelpers.RESOLUTION_SECTIONS.DEMERIT:
                    var demeritField = await _context.ScaleExtraDemeritFields.FirstOrDefaultAsync(x => x.ScaleResolutionId == id);
                    _context.ScaleExtraDemeritFields.Remove(demeritField);
                    break;
            }
            _context.ScaleResolutions.Remove(resolution);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetScaleResolutionsQuantityBySectionResolutionType(Guid sectionResolutionTypeId)
        {
            return await _context.ScaleResolutions.Where(x => x.ScaleSectionResolutionTypeId == sectionResolutionTypeId).CountAsync();
        }

        public async Task<List<ScaleResolution>> GetScaleResolutionsByPaginationParameters(string userId, Guid sectionId, Guid resolutionTypeId, string search, PaginationParameter paginationParameter)
        {
            var sectionResolutionTypeId = (await _context.ScaleSectionResolutionTypes.FirstOrDefaultAsync(x =>
                x.ScaleResolutionTypeId == resolutionTypeId && x.ScaleSectionId == sectionId)).Id;

            var query = _context.ScaleResolutions
                .Where(x => (string.IsNullOrWhiteSpace(search) || x.ResolutionNumber.Contains(search)) &&
                    x.UserId == userId && x.ScaleSectionResolutionTypeId == sectionResolutionTypeId)
                .AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ResolutionNumber) : query.OrderBy(q => q.ResolutionNumber);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ExpeditionDate) : query.OrderBy(q => q.ExpeditionDate);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ResolutionNumber) : query.OrderBy(q => q.ResolutionNumber);
                    break;
            }

            var pagedList = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                .Select(x => new ScaleResolution
                {
                    Id = x.Id,
                    ExpeditionFormattedDate = x.ExpeditionDate.ToLocalDateFormat(),
                    ResolutionNumber = x.ResolutionNumber,
                    ResolutionDocument = x.ResolutionDocument
                }).ToListAsync();

            return pagedList;
        }

        public async Task<List<ScaleResolution>> GetScaleResolutionsBySectionUserId(string userId, Guid sectionId)
        {
            var resolutions = await _context.ScaleResolutions
                .Include(x => x.ScaleSectionResolutionType)
                .ThenInclude(x => x.ScaleSection)
                .Where(x => x.UserId == userId && x.ScaleSectionResolutionType.ScaleSection.Id == sectionId)
                .OrderBy(x => x.ExpeditionDate)
                .AsQueryable()
                .Select(x => new ScaleResolution
                {
                    ResolutionNumber = x.ResolutionNumber,
                    ExpeditionFormattedDate = x.ExpeditionDate.ToLocalDateFormat(),
                    FormattedBeginDate = x.BeginDate.HasValue ? x.BeginDate.Value.ToLocalDateFormat() : null,
                    FormattedEndDate = x.EndDate.HasValue ? x.EndDate.Value.ToLocalDateFormat() : null,
                    BeginDate = x.BeginDate,
                    EndDate = x.EndDate
                })
                .ToListAsync();

            return resolutions;
        }

        public async Task<List<ScaleResolution>> GetQuinquenniumResolutionsBySectionUserId(string userId, Guid sectionId)
        {
            var resolutions = await _context.ScaleResolutions
                .Include(x => x.ScaleSectionResolutionType)
                    .ThenInclude(x => x.ScaleSection)
                .Include(x => x.ScaleSectionResolutionType)
                    .ThenInclude(x => x.ScaleResolutionType)
                .Where(x => x.UserId == userId && x.ScaleSectionResolutionType.ScaleSection.Id == sectionId )
                .ToListAsync();

            return resolutions;
        }

        public async Task<Tuple<int, List<Tuple<ApplicationUser, int>>>> GetUserResolutionsQuantityReportByPaginationParameters(PaginationParameter paginationParameter ,Guid? dedicationId = null , Guid? conditionId = null)
        {
            var baseQuery = _context.Users
                .Where(x => (string.IsNullOrWhiteSpace(paginationParameter.SearchValue) || x.UserName.Contains(paginationParameter.SearchValue) ||
                            x.PaternalSurname.Contains(paginationParameter.SearchValue) || x.MaternalSurname.Contains(paginationParameter.SearchValue) ||
                            x.Name.Contains(paginationParameter.SearchValue) || x.Dni.Contains(paginationParameter.SearchValue)) && 
                            x.UserRoles.Any(y => y.Role.Name.Equals(ConstantHelpers.ROLES.TEACHERS)))
                .AsQueryable();

            //Temporalmente
            var teachers = _context.Teachers.AsQueryable();

            if (dedicationId != null)
                teachers = teachers.Where(x => x.TeacherDedicationId == dedicationId);

            if (conditionId != null)
                baseQuery = baseQuery.Where(x => x.WorkerLaborInformation.WorkerLaborConditionId == conditionId);

            baseQuery = baseQuery.Where(x => teachers.Any(y => y.UserId == x.Id));
            
            var records = await baseQuery.CountAsync();
                       
            var query = baseQuery.Select(x => new
            {
                userName = x.UserName,
                name = x.Name,
                paternalSurname = x.PaternalSurname,
                maternalSurname = x.MaternalSurname,
                dni = x.Dni,
                roles = string.Join(", ", x.UserRoles.Select(d => d.Role.Name)),
                quantity = _context.ScaleResolutions.Count(y => y.UserId == x.Id)
            }).AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.paternalSurname) : query.OrderBy(q => q.paternalSurname);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.maternalSurname) : query.OrderBy(q => q.maternalSurname);
                    break;
                case "2":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.name) : query.OrderBy(q => q.name);
                    break;
                case "3":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.userName) : query.OrderBy(q => q.userName);
                    break;
                case "4":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.dni) : query.OrderBy(q => q.dni);
                    break;
                case "6":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.quantity) : query.OrderBy(q => q.quantity);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.paternalSurname) : query.OrderBy(q => q.paternalSurname);
                    break;
            }

            var data = await query.Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage).ToListAsync();

            var result = data.Select(x => new Tuple<ApplicationUser, int>(new ApplicationUser
            {
                UserName = x.userName,
                Dni = x.dni,
                ConcatenatedRoles = x.roles,
                PaternalSurname = x.paternalSurname,
                MaternalSurname = x.maternalSurname,
                Name = x.name
            }, x.quantity)).ToList();

            return new Tuple<int, List<Tuple<ApplicationUser, int>>>(records, result);
        }

        public async Task<List<Tuple<ApplicationUser, int>>> GetUserResolutionsQuantityReport(string search)
        {
            var data = await _context.Users
                .Where(x => (string.IsNullOrWhiteSpace(search) || x.UserName.Contains(search) ||
                             x.PaternalSurname.Contains(search) || x.MaternalSurname.Contains(search) ||
                             x.Name.Contains(search) || x.Dni.Contains(search)) &&
                            x.UserRoles.Any(y => y.Role.Name.Equals(ConstantHelpers.ROLES.TEACHERS)))
                .Select(x => new
                {
                    userName = x.UserName,
                    name = x.Name,
                    paternalSurname = x.PaternalSurname,
                    maternalSurname = x.MaternalSurname,
                    dni = x.Dni,
                    roles = string.Join(", ", x.UserRoles.Select(d => d.Role.Name)),
                    quantity = _context.ScaleResolutions.Count(y => y.UserId == x.Id)
                })
                .AsQueryable()
                .ToListAsync();
            
            var result = data.Select(x => new Tuple<ApplicationUser, int>(new ApplicationUser
            {
                UserName = x.userName,
                Dni = x.dni,
                ConcatenatedRoles = x.roles,
                PaternalSurname = x.paternalSurname,
                MaternalSurname = x.maternalSurname,
                Name = x.name
            }, x.quantity))
            .ToList();

            return result;
        }

        public async Task<List<ScaleResolution>> GetContractsResolutionsBySectionUserId(string userId, Guid sectionId)
        {
            var resolutions = await _context.ScaleResolutions
                .Include(x => x.ScaleSectionResolutionType)
                .ThenInclude(x => x.ScaleSection)
                .Where(x => x.UserId == userId && x.ScaleSectionResolutionType.ScaleSection.Id == sectionId &&
                                                x.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos")
                .OrderBy(x => x.ExpeditionDate)
                .AsQueryable()
                .Select(x => new ScaleResolution
                {
                    Id = x.Id,
                    ResolutionNumber = x.ResolutionNumber,
                    ExpeditionFormattedDate = x.ExpeditionDate.ToLocalDateFormat(),
                    FormattedBeginDate = x.BeginDate.HasValue ? x.BeginDate.Value.ToLocalDateFormat() : null,
                    FormattedEndDate = x.EndDate.HasValue ? x.EndDate.Value.ToLocalDateFormat() : null,
                    BeginDate = x.BeginDate,
                    EndDate = x.EndDate
                })
                .ToListAsync();

            return resolutions;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllTeacherReportDatatable(DataTablesStructs.SentParameters sentParameters, string startDate = null, string endDate = null, Guid? scaleResolutionTypeId = null)
        {
            //Por alguna razon el vinculo de las tablas parece al reves
            Expression<Func<ScaleExtraContractField, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.ScaleResolution.User.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.ScaleResolution.User.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ScaleResolution.User.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.ScaleResolution.IssueAgency;
                    break;
                case "5":
                    orderByPredicate = (x) => x.ScaleResolution.BeginDate;
                    break;
                case "6":
                    orderByPredicate = (x) => x.ScaleResolution.EndDate;
                    break;
                case "7":
                    orderByPredicate = (x) => x.LaborPosition;
                    break;
            }

            var query = _context.ScaleExtraContractFields
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsQueryable();

            if (scaleResolutionTypeId != null)
                query = query.Where(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionTypeId == scaleResolutionTypeId);

            if (startDate != null)
                query = query.Where(x => x.ScaleResolution.BeginDate >= ConvertHelpers.DatepickerToUtcDateTime(startDate));

            if (endDate != null)
                query = query.Where(x => x.ScaleResolution.EndDate <= ConvertHelpers.DatepickerToUtcDateTime(endDate));

            //Aquellos usuarios que son profesores
            var teachers = _context.Teachers.AsQueryable();

            query = query.Where(x => teachers.Any(y => y.UserId == x.ScaleResolution.UserId));

            int recordsFiltered = await query.CountAsync();

            var dependencies = _context.Dependencies.AsQueryable();

            var data = await query
                .Select(x => new
                {
                    x.ScaleResolution.User.Name,
                    x.ScaleResolution.User.MaternalSurname,
                    x.ScaleResolution.User.PaternalSurname,
                    ScaleResolutionType = x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name,
                    x.ScaleResolution.IssueAgency,
                    BeginDate = x.ScaleResolution.BeginDate.ToLocalDateFormat() ?? "-",
                    EndDate = x.ScaleResolution.EndDate.ToLocalDateFormat() ?? "-",
                    x.LaborPosition,
                    Dependency = x.DependencyId == null ? "-" : dependencies.Where(y => y.Id == x.DependencyId).Select(y => y.Name).FirstOrDefault()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllUserReportDatatable(DataTablesStructs.SentParameters sentParameters, string userId = null)
        {
            //Por alguna razon el vinculo de las tablas parece al reves
            Expression<Func<ScaleExtraContractField, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Id);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.ScaleResolutionId);
                    break;
                default:
                    orderByPredicate = ((x) => x.Id);
                    break;
            }

            var query = _context.ScaleExtraContractFields
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(x => x.ScaleResolution.UserId == userId);

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    query = query.Where(x => x.ScaleResolution.User.Name.ToUpper().Contains(searchValue.ToUpper())
            //                || x.ScaleResolution.User.PaternalSurname.ToUpper().Contains(searchValue.ToUpper())
            //                || x.ScaleResolution.User.MaternalSurname.ToUpper().Contains(searchValue.ToUpper())
            //                || x.ScaleResolution.User.UserName.ToUpper().Contains(searchValue.ToUpper()));
            //}

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.ScaleResolution.User.Name,
                    x.ScaleResolution.User.MaternalSurname,
                    x.ScaleResolution.User.PaternalSurname,
                    ScaleResolutionType = x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name,
                    //x.ScaleResolution.IssueAgency,
                    x.ScaleResolution.BeginDate,
                    x.ScaleResolution.EndDate,
                    x.LaborPosition
                    //Dependency = x.DependencyId == null ? "-" : _context.Dependencies.Where(y => y.Id == x.DependencyId).Select(y => y.Name).FirstOrDefault()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<List<ScaleResolutionInvestigationTemplate>> GetInvestigationScaleResolutionsByPaginationParameters(string userId,Guid sectionId, Guid resolutionTypeId, string search, PaginationParameter paginationParameter)
        {
            var sectionResolutionTypeId = (await _context.ScaleSectionResolutionTypes.FirstOrDefaultAsync(x =>
                x.ScaleResolutionTypeId == resolutionTypeId && x.ScaleSectionId == sectionId)).Id;

            var query = _context.ScaleExtraInvestigationFields
                .Where(x => (string.IsNullOrWhiteSpace(search)  || x.ScaleResolution.ResolutionNumber.Contains(search)) &&
                    x.ScaleResolution.UserId == userId)
                .AsQueryable();

            switch (paginationParameter.SortField)
            {
                case "0":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ScaleResolution.ResolutionNumber) : query.OrderBy(q => q.ScaleResolution.ResolutionNumber);
                    break;
                case "1":
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ScaleResolution.ExpeditionDate) : query.OrderBy(q => q.ScaleResolution.ExpeditionDate);
                    break;
                default:
                    query = paginationParameter.SortOrder.Equals(paginationParameter.BaseOrder) ? query.OrderByDescending(q => q.ScaleResolution.ResolutionNumber) : query.OrderBy(q => q.ScaleResolution.ResolutionNumber);
                    break;
            }

            var pagedList = await query
                .Where(x => x.ScaleResolution.ScaleSectionResolutionTypeId == sectionResolutionTypeId)
                .Skip(paginationParameter.CurrentNumber).Take(paginationParameter.RecordsPerPage)
                .Select(x => new ScaleResolutionInvestigationTemplate
                {
                    Id = x.ScaleResolutionId,
                    ExpeditionFormattedDate = x.ScaleResolution.ExpeditionDate.ToLocalDateFormat(),
                    ResolutionNumber = x.ScaleResolution.ResolutionNumber,
                    ResolutionDocument = x.ScaleResolution.ResolutionDocument,
                    InvestigationParticipationType = x.InvestigationParticipationType.Name
                }).ToListAsync();

            return pagedList;
        }



        public async Task<string> GetWokerStatusDescriptionBySectionNumber(byte contracts)
        {
            var contractQuery = _context.ScaleResolutions
                        .Where(x => x.ScaleSectionResolutionType.ScaleSection.SectionNumber == contracts)
                        .AsQueryable();

            var userContracts = await contractQuery.Where(x => x.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos")
                                .OrderByDescending(x => x.ExpeditionDate)
                                .FirstOrDefaultAsync();

            var userCeses = await contractQuery.Where(x => x.ScaleSectionResolutionType.ScaleResolutionType.Name == "Cese temporal" ||
                                            x.ScaleSectionResolutionType.ScaleResolutionType.Name == "Cese definitivo" ||
                                            x.ScaleSectionResolutionType.ScaleResolutionType.Name == "Cese por fallecimiento")
                                            .OrderByDescending(x => x.ExpeditionDate)
                                            .FirstOrDefaultAsync();

            if (userCeses != null)
            {
                if (userContracts != null)
                {
                    if (userCeses.ExpeditionDate > userContracts.ExpeditionDate)
                        return userCeses.Observation;
                }
                else
                    return userCeses.Observation;
            }
            return string.Empty;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetContractsByUserDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            Expression<Func<ScaleExtraContractField, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ScaleResolution.ResolutionNumber);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.ScaleResolution.ExpeditionDate);
                    break;
            }

            var query = _context.ScaleExtraContractFields
                .Where(x => x.ScaleResolution.UserId == userId)
                .AsQueryable();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    Id = x.ScaleResolutionId,
                    x.ScaleResolution.ResolutionNumber,
                    ExpeditionDate = x.ScaleResolution.ExpeditionDate.ToLocalDateFormat()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetBenefitsByUserDatatable(DataTablesStructs.SentParameters sentParameters, string userId)
        {
            var query = _context.ScaleExtraBenefitFields
                .Where(x => x.ScaleResolution.UserId == userId && (x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Beneficios"
                    || x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Bonificación Familiar"
                    || x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Pensiones"
                    || x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Quinquenios"
                    || x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Reconocimiento por tiempo de servicios"))
                .AsNoTracking();

            Expression<Func<ScaleExtraBenefitField, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.ScaleResolution.ResolutionNumber;
                    break;
                case "1":
                    orderByPredicate = (x) => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ScaleResolution.IssueAgency;
                    break;
                case "3":
                    orderByPredicate = (x) => x.ScaleResolution.ExpeditionDate;
                    break;
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.ScaleResolution.ResolutionNumber,
                    x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name,
                    x.ScaleResolution.IssueAgency,
                    ExpeditionDate = x.ScaleResolution.ExpeditionDate.ToLocalDateFormat()
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
