using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using Hangfire.MemoryStorage.Entities;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Helpers.ConstantHelpers;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(AkdemicContext context) : base(context) { }

        #region PRIVATE 

        private async Task<DataTablesStructs.ReturnedData<Company>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, Expression<Func<Company, Company>> selectPredicate = null, Expression<Func<Company, dynamic>> orderByPredicate = null, Func<Company, string[]> searchValuePredicate = null,
            Guid? sectorId = null, string searchValue = null)
        {
            IQueryable<Company> query = _context.Companies.Where(x => x.IsExternalRequest == false)
                .WhereSearchValue(searchValuePredicate, searchValue)
                .AsQueryable();

            if (sectorId.HasValue)
            {
                query = query.Where(x => x.SectorId == sectorId);
            }

            query = query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            return await query.ToDataTables(sentParameters, selectPredicate);
        }

        #endregion

        #region PUBLIC

        public async Task<DataTablesStructs.ReturnedData<Company>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? sectorId = null, string searchValue = null)
        {
            Expression<Func<Company, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Sector.Description);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.RUC);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.Name);
                    break;
            }

            return await GetCompaniesDatatable(sentParameters, (x) => new Company
            {
                Id = x.Id,
                Sector = new Sector
                {
                    Description = x.Sector.Description
                },
                RUC = x.RUC,
                SectorId = x.SectorId,
                User = new ApplicationUser
                {
                    Name = x.User.Name
                }
            }, orderByPredicate, (x) => new[] { x.User.Name, x.RUC }, sectorId, searchValue);
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetAllCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, Guid? sectorId, Guid? lineId, string company)
        {
            Expression<Func<Company, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Sector.Description);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.RUC);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.CompanySize.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.CompanyType.Name);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.EconomicActivity.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.Name);
                    break;
            }

           var query = _context.Companies.AsQueryable();

            if (!String.IsNullOrEmpty(company)) {
                query = query.Where(x => x.User.Name.ToUpper().Contains(company.ToUpper()));
            }

            if (sectorId.HasValue)
            {
                query = query.Where(x => x.SectorId == sectorId);
            }

            if (lineId.HasValue)
            {
                query = query.Where(x => x.User.UserResearchLines.Any(y=>y.ResearchLineId == lineId));
            }

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    Id = x.Id,
                    Sector = new Sector
                    {
                        Description = x.Sector.Description
                    },
                    CompanySize = new CompanySize
                    {
                        Name = x.CompanySize.Name
                    },
                    CompanyType = new CompanyType
                    {
                        Name = x.CompanyType.Name
                    },
                    EconomicActivity = new EconomicActivity
                    {
                        Name = x.EconomicActivity.Name
                    },
                    RUC = x.RUC,
                    SectorId = x.SectorId,
                    User = new ApplicationUser
                    {
                        Id = x.User.Id,
                        Name = x.User.Name
                    }
                }).ToListAsync();

            int recordsTotal = data.Count;

            var lockedUsers = _context.LockedUsers
               .OrderBy(x => x.DateTime)
               .AsEnumerable()
               .Where(x => data.Any(y => y.User.Id == x.UserId))
               .ToList();

            var _data = data.Select(x => new
            {
                x.Id,
                x.Sector,
                x.CompanySize,
                x.CompanyType,
                x.EconomicActivity,
                x.RUC,
                x.SectorId,
                x.User,
                text = lockedUsers.Where(y => y.UserId == x.User.Id).Select(y => y.Description).FirstOrDefault(),
                locked = lockedUsers.FirstOrDefault(y => y.UserId == x.User.Id) == null ? false : lockedUsers.FirstOrDefault(y => y.UserId == x.User.Id).Status
            }).ToList();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = _data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<bool> AnyCompanyExistByName(Guid companyId, string Name)
        {
            return await _context.Companies.Where(x => x.IsExternalRequest == false).Include(x => x.User).AnyAsync(x => x.Id != companyId && x.User.Name == Name);
        }

        public async Task<bool> AnyCompanyExistByRUC(Guid companyId, string RUC)
        {
            return await _context.Companies.Where(x => x.IsExternalRequest == false).AnyAsync(x => x.Id != companyId && x.RUC == RUC);
        }

        public async Task<object> CompaniesReport1(byte type, List<Guid> careers)
        {
            var categories = new List<string>();
            var data = new List<int>();

            var companies = _context.Companies.Where(x => x.IsExternalRequest == false).AsNoTracking();
            var jobOffers = _context.JobOffers.AsNoTracking();

            if (careers.Count > 0)
            {
                companies = companies.Where(x => x.JobOffers.Any(y => y.JobOfferCareers.Any(z => careers.Contains(z.CareerId))));
                jobOffers = jobOffers.Where(x => x.JobOfferCareers.Any(y => careers.Contains(y.CareerId)));
            }


            var lstCompanies = await companies
                .Select(x => new
                {
                    company = x.User.Name,
                    count = type == 0
                        ? jobOffers.Where(y => y.CompanyId == x.Id).Count()
                        : jobOffers.Where(y => y.Status == type && y.CompanyId == x.Id).Count()
                }).OrderByDescending(x => x.count)
                .Take(5)
                .ToListAsync();

            for (int i = 0; i < lstCompanies.Count(); i++)
            {
                categories.Add(lstCompanies[i].company);
                data.Add(lstCompanies[i].count);
            }

            return new
            {
                categories,
                data
            };

        }

        public async Task<object> CompaniesReport2(bool isCoordinator, List<Guid> careers)
        {
            List<string> categories = new List<string>();
            List<int> data = new List<int>();

            if (isCoordinator)
            {
                List<string> companies1 = await _context.JobOfferApplications.Include(x => x.JobOffer.Company)
                    .Where(x=>x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED)
                    .Where(x => x.JobOffer.JobOfferCareers.Any(z => careers.Contains(z.CareerId)))
                    .Select(x => x.JobOffer.Company.User.Name)
                    .ToListAsync();

                for (int i = 0; i < companies1.Count; i++)
                {
                    if (categories.FirstOrDefault(x => x == companies1[i]) == null)
                    {
                        categories.Add(companies1[i]);
                        data.Add(1);
                    }
                    else
                    {
                        int index = categories.IndexOf(companies1[i]);
                        data[index] = data[index] + 1;
                    }
                }
            }
            else
            {
                List<string> companies = await _context.JobOfferApplications.Include(x => x.JobOffer.Company).Where(x => x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED).Select(x => x.JobOffer.Company.User.Name).ToListAsync();
                for (int i = 0; i < companies.Count; i++)
                {
                    if (categories.FirstOrDefault(x => x == companies[i]) == null)
                    {
                        categories.Add(companies[i]);
                        data.Add(1);
                    }
                    else
                    {
                        int index = categories.IndexOf(companies[i]);
                        data[index] = data[index] + 1;
                    }
                }

            }

            return new
            {
                categories,
                data
            };
        }

        public async Task<decimal> CompaniesReport3(bool isCoordinator, List<Guid> careers, Guid? companyId)
        {
            decimal indicator = 0;
            if (companyId == null)
            {
                if (isCoordinator)
                {
                    int postulantesC = await _context.JobOfferApplications
                        .Where(x => x.JobOffer.JobOfferCareers.Any(z => careers.Contains(z.CareerId))).CountAsync();
                    int contratadosC = await _context.JobOfferApplications
                        .Where(x => x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED && x.JobOffer.JobOfferCareers.Any(z => careers.Contains(z.CareerId))).CountAsync();

                    if (postulantesC > 0)
                    {
                        decimal val = Convert.ToDecimal(contratadosC) / Convert.ToDecimal(postulantesC);
                        indicator = Decimal.Round((val * 100), 2);
                    }
                }
                else
                {
                    int postulantes = await _context.JobOfferApplications
                        .CountAsync();
                    int contratados = await _context.JobOfferApplications
                        .Where(x => x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED).CountAsync();
                    if (postulantes > 0)
                    {
                        decimal val = Convert.ToDecimal(contratados) / Convert.ToDecimal(postulantes);
                        indicator = Decimal.Round((val * 100), 2);
                    }
                }

            }
            else
            {
                if (isCoordinator)
                {
                    int postulantesC = await _context.JobOfferApplications
                        .Include(x => x.JobOffer.Company)
                        .Where(x => x.JobOffer.Company.Id == companyId && x.JobOffer.JobOfferCareers.Any(z => careers.Contains(z.CareerId))).CountAsync();
                    int contratadosC = await _context.JobOfferApplications
                        .Include(x => x.JobOffer.Company)
                        .Where(x => x.JobOffer.Company.Id == companyId && x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED && x.JobOffer.JobOfferCareers.Any(z => careers.Contains(z.CareerId))).CountAsync();
                    if (postulantesC > 0)
                    {
                        decimal val = Convert.ToDecimal(contratadosC) / Convert.ToDecimal(contratadosC);
                        indicator = Decimal.Round((val * 100), 2);
                    }
                }
                else
                {
                    int postulantes = await _context.JobOfferApplications
                        .Include(x => x.JobOffer.Company)
                        .Where(x => x.JobOffer.Company.Id == companyId).CountAsync();
                    int contratados = await _context.JobOfferApplications
                        .Include(x => x.JobOffer.Company)
                        .Where(x => x.JobOffer.Company.Id == companyId && x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED).CountAsync();
                    if (postulantes > 0)
                    {
                        decimal val = Convert.ToDecimal(contratados) / Convert.ToDecimal(postulantes);
                        indicator = Decimal.Round((val * 100), 2);
                    }
                }

            }
            return indicator;
        }

        public async Task<IEnumerable<Company>> GetAllWithIncludes()
        {
            var query = _context.Companies.Where(x => x.IsExternalRequest == false)
                .Include(x => x.User)
                .Include(x => x.Sector)
                .Include(x => x.JobOffers)
                .Include(x => x.Sedes)
                .Include(x => x.ChannelContacts)
                .Include(x => x.CompanySize)
                .Include(x => x.CompanyType)
                .Include(x => x.EconomicActivity);


            return await query.ToListAsync();
        }

        public async Task<Company> GetByUserId(string userId)
        {
            var query = _context.Companies
                .Include(x => x.User)                
                .Include(x => x.CompanySize)
                .Include(x => x.EconomicActivity)
                .Include(x => x.CompanyType)
                .Include(x => x.JobOffers)
                .ThenInclude(x => x.JobOfferApplications)
                .Include(x => x.Sector)
                .Where(x => x.UserId == userId && x.IsExternalRequest == false);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, bool IsExternalRequest, string searchValue = null)
        {
            Expression<Func<Company, dynamic>> orderByPredicate = null;

            //switch (sentParameters.OrderColumn)
            //{
            //    case "0":
            //        orderByPredicate = ((x) => x.Id);
            //        break;
            //    case "1":
            //        orderByPredicate = ((x) => x.UserId);
            //        break;
            //    default:
            //        orderByPredicate = ((x) => x.Id);
            //        break;
            //}
            IQueryable<Company> query = _context.Companies
                .Where(x => x.IsExternalRequest == IsExternalRequest).OrderByDescending(x => x.User.CreatedAt)
                //.OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.RUC.Contains(searchValue) || x.User.Name.Contains(searchValue));
            }

            Expression<Func<Company, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                x.Id,
                x.User.Name,
                x.RUC,
                CreatedAt = x.User.CreatedAt.ToLocalDateFormat(),
                StateDescription = ConstantHelpers.EXTERNAL_REQUEST_STATES.VALUES[x.State],
                State = x.State
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCompaniesDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null)
        {
            Expression<Func<Company, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.RUC);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Sector.Description);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.CompanySize.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.CompanyType.Name);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.EconomicActivity.Name);
                    break;
                default:
                    orderByPredicate = ((x) => x.User.Name);
                    break;
            }
            IQueryable<Company> query = _context.Companies.OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.RUC.Contains(searchValue) || x.User.Name.Contains(searchValue));
            }

            Expression<Func<Company, dynamic>> selectPredicate = null;
            selectPredicate = (x) => new
            {
                name = x.User.Name,
                ruc = x.RUC,
                sector = x.Sector.Description,
                size = x.CompanySize.Name,
                type = x.CompanyType.Name,
                activity = x.EconomicActivity.Name
            };

            return await query.ToDataTables2(sentParameters, selectPredicate);
        }

        public async Task<Company> GetWithIncludes(Guid id)
        {
            IQueryable<Company> query = _context.Companies
                .Where(x => x.IsExternalRequest == false)
                    .Include(x => x.User)
                    .Include(x => x.Sector)
                    .Include(x => x.JobOffers)
                    .Include(x => x.Sedes)
                    .Include(x => x.ChannelContacts)
                    .Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<object> GetCompaniesSelect2()
        {
            var result = _context.Companies
                .Where(x => x.IsExternalRequest == false)
                .Select(x => new { id = x.Id, text = x.User.Name });

            return await result.ToListAsync();
        }

        public async Task<bool> AnyByRUC(string RUC, Guid? id = null)
        {
            return await _context.Companies.Where(x => x.IsExternalRequest).AnyAsync(x => x.RUC == RUC && x.Id != id);
        }

        public IQueryable<Company> GetQueryable()
        {
            return _context.Companies.AsQueryable();
        }

        public async Task<object> CustomDetail(Guid id)
        {
            var result = await _context.Companies
                                .Where(x => x.Id == id)
                                .Select(x => new
                                {
                                    image = x.User.Picture,
                                    description = x.Description,
                                    ruc = x.RUC,
                                    area = x.Sector.Description ?? "-",
                                    name = x.User.Name,
                                    number = x.Phone ?? "-",
                                    year = x.FormationTime.ToString("dd/MM/yyyy"),
                                    size = x.CompanySize.Name,
                                    channelContact = x.ChannelContacts
                                        .Select(y => new
                                        {
                                            name = y.Description
                                        }).ToList(),
                                    companyImages = x.ImageCompanies
                                        .Select(z=> new { 
                                            z.Description,
                                            z.Picture                                    
                                        }).ToList(),
                                    sedes = x.Sedes
                                        .Select(y => new
                                        {
                                            name = y.Name,
                                            location = $"{y.District.Province.Department.Name} {y.District.Province.Name} {y.District.Name}",
                                            description = y.Description
                                        }).ToList(),
                                    companies = _context.Companies.Where(y => !y.IsExternalRequest && y.SectorId == x.SectorId && y.Id != x.Id)
                                        .Select(y => new
                                        {
                                            id = y.Id,
                                            image = y.User.Picture,
                                            name = y.User.Name,
                                            sector = y.Sector.Description ?? "-",
                                            size = y.CompanySize.Name,
                                            description = y.Description
                                        }).Take(3).ToList()
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<object> GetCompaniesByUserSelect2ClientSide(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var coordinatorSchool = await _context.CoordinatorCareers.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (user.IsInRole(ConstantHelpers.ROLES.JOBEXCHANGE_COORDINATOR))
            {
                return new { items = await _context.Companies.Where(x => x.JobOffers.Any(y => y.JobOfferCareers.Any(z => z.CareerId == coordinatorSchool.CareerId))).Select(x => new { id = x.Id, text = x.User.Name }).OrderBy(x => x.text).ToListAsync()};
            }

            return new { items = await _context.Companies.Select(x => new { id = x.Id, text = x.User.Name }).OrderBy(x => x.text).ToListAsync()};
        }

        public async Task<object> GetCompaniesByStudentExperiencesUserSelect2ClientSide(string userId)
        {
            var companies = await _context.Companies
                .Where(x => x.StudentExperiences.Any(y => y.Student.UserId == userId))
                .Distinct()
                .Select(x => new
                {
                    Id = x.UserId,
                    Text = x.User.Name
                })
                .ToListAsync();

            return companies;
        }

        public async Task<object> GetCompaniesByStudentJobOfferApplicationAceptedSelect2ClientSide(string userId)
        {
            var companies = await _context.JobOfferApplications
                .Where(x => x.Student.UserId == userId && x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED)
                .Select(x => new
                {
                    Id = x.JobOffer.Company.UserId,
                    Text = x.JobOffer.Company.User.Name
                })
                .Distinct()
                .ToListAsync();

            return companies;
        }
       
        public async Task<bool> GetByCompanySize(Guid companySizeId)
        {
            return await _context.Companies.AnyAsync(x => x.CompanySizeId == companySizeId);
        }

        public async Task<bool> GetByCompanyType(Guid companyTypeId)
        {
            return await _context.Companies.AnyAsync(x => x.CompanyTypeId == companyTypeId);
        }

        public async Task<bool> GetBySector(Guid sectorId)
        {
            return await _context.Companies.AnyAsync(x => x.SectorId == sectorId);
        }

        public async Task<bool> GetByEconomicActivity(Guid economicActivityId)
        {
            return await _context.Companies.AnyAsync(x => x.EconomicActivityId == economicActivityId);
        }

        public async Task<object> GetCompaniesWithOutAgreement()
        {
            var result = await _context.Companies
                .Where(x => x.AgreementId == null)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.User.Name
                }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Company>> GetAllWithOutAgreement()
        {
            return await _context.Companies.Where(x => x.AgreementId == null).ToListAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        //NO PONER INCLUDES :v
        public async Task<Company> GetCompanyByUserId(string userId, bool isExternalRequest = false)
        {
            return await _context.Companies.Where(x => x.UserId == userId && x.IsExternalRequest == isExternalRequest)
                .FirstOrDefaultAsync();
        }

        public async Task<Select2Structs.ResponseParameters> GetCompaniesSelect2(Select2Structs.RequestParameters requestParameters, string searchValue = null)
        {
            var query = _context.Companies
                            .Where(x => !x.IsExternalRequest)
                            .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.User.FullName, searchValue));
                }
                else
                    query = query.Where(x => x.User.FullName.ToLower().Contains(searchValue.Trim().ToLower()));
            }

            return await query.ToSelect2(requestParameters, (x) => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.User.Name
            }, ConstantHelpers.SELECT2.DEFAULT.PAGE_SIZE);
        }

        public async Task<Company> GetByRuc(string ruc)
        {
            var company = await _context.Companies
                .Where(x => x.RUC == ruc)
                .FirstOrDefaultAsync();

            return company;
        }

        public async Task<bool> IsExternalRequest(string userId)
        {
            var result = await _context.Companies
                .AnyAsync(x => x.UserId == userId && x.IsExternalRequest);
            return result;
        }

        public async Task<List<CompanyTemplate>> GetAllCompanyTemplateData(bool? isExternalRequest = null)
        {
            var query = _context.Companies.AsNoTracking();

            if (isExternalRequest != null)
                query = query.Where(x => x.IsExternalRequest == isExternalRequest);

            var result = await query
                .Select(x => new CompanyTemplate
                {
                    Ruc = x.RUC,
                    FullName = x.User.FullName,
                    CompanySizeName = x.CompanySize.Name,
                    CompanyTypeName = x.CompanyType.Name,
                    SectorName = x.SectorId != null ? x.Sector.Description: "",
                    Email = x.User.Email,
                    PhoneNumber = x.Phone,
                    EconomicActivityName = x.EconomicActivity.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetHomePendingCompaniesDatatable(DataTablesStructs.SentParameters sentParameters)
        {
            Expression<Func<Company, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.RUC);
                    break;
            }

            var query = _context.Companies.Where(x => x.IsExternalRequest).AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    userName = x.User.UserName,
                    fullName = x.User.FullName,
                    x.RUC
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

        public async Task<object> GetJobExchangeJobOfferReportChart(int jobOfferStatus = 0, List<Guid> careers = null)
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var companies = _context.Companies.Where(x => !x.IsExternalRequest).AsNoTracking();
            var jobOffers = _context.JobOffers.AsNoTracking();

            if (jobOfferStatus != 0)
            {
                jobOffers = jobOffers.Where(x => x.Status == jobOfferStatus);
            }

            if (careers != null && careers.Count > 0 )
            {
                companies = companies.Where(x => x.JobOffers.Any(y => y.JobOfferCareers.Any(z => careers.Contains(z.CareerId))));
                jobOffers = jobOffers.Where(x => x.JobOfferCareers.Any(y => careers.Contains(y.CareerId)));
            }


            var lstCompanies = await companies
                .Select(x => new
                {
                    company = x.User.Name,
                    count = jobOffers.Where(y => y.CompanyId == x.Id).Count()
                }).OrderByDescending(x => x.count)
                .Take(5)
                .ToListAsync();

            for (int i = 0; i < lstCompanies.Count(); i++)
            {
                categoriesData.Add(lstCompanies[i].company);
                seriesData.Add(lstCompanies[i].count);
            }

            return new
            {
                categoriesData,
                seriesData
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferReportDatatable(DataTablesStructs.SentParameters sentParameters, int jobOfferStatus = 0, List<Guid> careers = null)
        {

            var companies = _context.Companies.Where(x => !x.IsExternalRequest).AsNoTracking();
            var jobOffers = _context.JobOffers.AsNoTracking();

            if (jobOfferStatus != 0)
            {
                jobOffers = jobOffers.Where(x => x.Status == jobOfferStatus);
            }

            if (careers != null && careers.Count > 0)
            {
                companies = companies.Where(x => x.JobOffers.Any(y => y.JobOfferCareers.Any(z => careers.Contains(z.CareerId))));
                jobOffers = jobOffers.Where(x => x.JobOfferCareers.Any(y => careers.Contains(y.CareerId)));
            }


            var data = await companies
                .Select(x => new
                {
                    company = x.User.Name,
                    count = jobOffers.Where(y => y.CompanyId == x.Id).Count()
                }).ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<object> GetJobExchangeJobOfferApplicationReportChart()
        {
            var categoriesData = new List<string>();
            var seriesData = new List<int>();

            var companies = _context.Companies.Where(x => !x.IsExternalRequest).AsNoTracking();
            var JobOfferApplications = _context.JobOfferApplications.Where(x => x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED).AsNoTracking();


            var lstCompanies = await companies
                .Select(x => new
                {
                    company = x.User.Name,
                    count = JobOfferApplications.Where(y => y.JobOffer.CompanyId == x.Id).Count()
                })
                .ToListAsync();

            for (int i = 0; i < lstCompanies.Count(); i++)
            {
                categoriesData.Add(lstCompanies[i].company);
                seriesData.Add(lstCompanies[i].count);
            }

            return new
            {
                categoriesData,
                seriesData
            };
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetJobExchangeJobOfferApplicationReportDatatable(DataTablesStructs.SentParameters sentParameters)
        {

            var companies = _context.Companies.Where(x => !x.IsExternalRequest).AsNoTracking();
            var JobOfferApplications = _context.JobOfferApplications.Where(x => x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED).AsNoTracking();

            var data = await companies
                .Select(x => new
                {
                    company = x.User.Name,
                    count = JobOfferApplications.Where(y => y.JobOffer.CompanyId == x.Id).Count()
                }).ToListAsync();

            var recordsTotal = data.Count();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<decimal> GetJobExchangeJobOfferIndicatorReport(Guid? companyId = null)
        {
            decimal indicator = 0;
            var query = _context.JobOfferApplications.AsNoTracking();

            if (companyId != null)
                query = query.Where(x => x.JobOffer.CompanyId == companyId);

            int postulantCount = await query.CountAsync();
            int postulantAcceptedCount = await query.Where(x => x.Status == ConstantHelpers.JobOfferApplication.Status.ACCEPTED).CountAsync();

            if (postulantCount > 0)
            {
                decimal val = Convert.ToDecimal(postulantAcceptedCount) / Convert.ToDecimal(postulantCount);
                indicator = Decimal.Round((val * 100), 2);
            }

            return indicator;
        }


        #endregion

    }
}
