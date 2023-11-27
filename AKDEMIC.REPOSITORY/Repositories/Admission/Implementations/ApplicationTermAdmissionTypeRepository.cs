using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class ApplicationTermAdmissionTypeRepository : Repository<ApplicationTermAdmissionType>, IApplicationTermAdmissionTypeRepository
    {
        public ApplicationTermAdmissionTypeRepository(AkdemicContext context) : base(context)
        {
        }

        public override async Task<ApplicationTermAdmissionType> Get(Guid id)
        {
            return await _context.ApplicationTermAdmissionTypes.Include(x => x.ApplicationTerm.Term).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ApplicationTermAdmissionType>> GetAllByApplicationTermId(Guid applicationTermId)
        {
            var result = await _context.ApplicationTermAdmissionTypes
                .Where(x => x.ApplicationTermId == applicationTermId)
                .Include(x => x.AdmissionType)
                .ToListAsync();
            return result;
        }

        public async Task<object> GetByApplicationTermId(DataTablesStructs.SentParameters sentParameters, Guid id, int type, bool isAllChecked, string searchValue)
        {
            var query = _context.AdmissionTypes
                .Where(x => x.IsActive && x.Type != ConstantHelpers.ADMISSION_MODE.AGREEMENT)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.Contains(searchValue));

            if (type > -1)
                query = query.Where(x => x.Type == type);

            var recordsFiltered = await query.CountAsync();

            var data = await query
                        .Skip(sentParameters.PagingFirstRecord)
                        .Take(sentParameters.RecordsPerDraw)
                        .Select(x => new
                        {
                            id = x.Id,
                            name = x.Name,
                            abreviation = x.Abbreviation,
                            //isValid = x.IsValid,
                            resolution = x.Resolution,
                            resolutionDate = x.ResolutionDate.ToString("dd/MM/yyyy"),
                            isExonerated = x.IsExonerated,
                            type = x.Type,
                            validityStart = x.ValidityStart.HasValue ? x.ValidityStart.Value.ToString("dd/MM/yyyy") : null,
                            validityEnd = x.ValidityEnd.HasValue ? x.ValidityEnd.Value.ToString("dd/MM/yyyy") : null,
                            cost = $"Pub: {x.PublicSchoolConcept.Amount}/Priv: {x.PrivateSchoolConcept.Amount}",
                            @checked = x.ApplicationTermAdmissionTypes.Any(y => y.ApplicationTermId == id)
                        }).ToListAsync();
            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }
        public async Task<object> GetByApplicationTermId(Guid id)
        {
            var query = _context.AdmissionTypes.Where(x => x.ApplicationTermAdmissionTypes.Any(y => y.ApplicationTermId == id)).AsNoTracking();


            return await query
                        .Select(x => new
                        {
                            id = x.Id,
                            name = x.Name,
                            abreviation = x.Abbreviation,
                            type = x.Type
                        }).ToListAsync();
        }

        public async Task<object> GetByApplicationTermIdSelect2(Guid id)
        {
            var query = _context.ApplicationTermAdmissionTypes
                .Where(y => y.ApplicationTermId == id
                && (y.AdmissionType.PublicSchoolConceptId.HasValue || y.AdmissionType.PrivateSchoolConceptId.HasValue || y.AdmissionType.IsExonerated))
                .OrderBy(x=>x.AdmissionType.Name)
                .AsNoTracking();

            return await query
                        .Select(x => new
                        {
                            id = x.Id,
                            text = x.AdmissionType.Name
                        }).ToListAsync();
        }

        public async Task SaveApplicationTermAdmissionTypes(Guid id, bool isCheckAll, List<Guid> lstToAdd, List<Guid> lstToAvoid)
        {
            if (!isCheckAll)
            {
                if (lstToAdd.Count() == 0 && lstToAvoid.Count() == 0)
                {
                    var ApplicationTermAdmissionTypes = await _context.ApplicationTermAdmissionTypes.Where(x => x.ApplicationTermId == id).ToListAsync();
                    _context.ApplicationTermAdmissionTypes.RemoveRange(ApplicationTermAdmissionTypes);
                }
                else
                {
                    var list = new List<ApplicationTermAdmissionType>();
                    var list2 = new List<ApplicationTermAdmissionType>();
                    foreach (var item in lstToAdd)
                    {
                        if (!await _context.ApplicationTermAdmissionTypes.AnyAsync(x => x.ApplicationTermId == id && x.AdmissionTypeId == item))
                        {
                            list.Add(new ApplicationTermAdmissionType
                            {
                                ApplicationTermId = id,
                                AdmissionTypeId = item
                            });
                        }

                    }
                    await _context.ApplicationTermAdmissionTypes.AddRangeAsync(list);
                    foreach (var item in lstToAvoid)
                    {
                        var ApplicationTermAdmissionTypes = await _context.ApplicationTermAdmissionTypes.FirstOrDefaultAsync(x => x.ApplicationTermId == id && x.AdmissionTypeId == item);
                        list2.Add(ApplicationTermAdmissionTypes);
                    }
                    _context.ApplicationTermAdmissionTypes.RemoveRange(list2);
                }
            }
            else
            {
                var query = _context.AdmissionTypes.ToList();
                var list = new List<ApplicationTermAdmissionType>();
                foreach (var item in query)
                {
                    var exist = await _context.ApplicationTermAdmissionTypes.AnyAsync(x => x.ApplicationTermId == id && x.AdmissionTypeId == item.Id);
                    if (!exist)
                    {
                        list.Add(new ApplicationTermAdmissionType
                        {
                            ApplicationTermId = id,
                            AdmissionTypeId = item.Id
                        });
                    }
                }
                await _context.ApplicationTermAdmissionTypes.AddRangeAsync(list);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddRemoveApplicationTermAdmissionType(Guid applicationTermId, Guid admissionTypeId)
        {
            var applicationTermAdmissionType = await _context.ApplicationTermAdmissionTypes.Where(x => x.ApplicationTermId == applicationTermId && x.AdmissionTypeId == admissionTypeId).FirstOrDefaultAsync();

            if(applicationTermAdmissionType is null)
            {
                applicationTermAdmissionType = new ApplicationTermAdmissionType
                {
                    AdmissionTypeId = admissionTypeId,
                    ApplicationTermId = applicationTermId
                };

                await _context.ApplicationTermAdmissionTypes.AddAsync(applicationTermAdmissionType);
            }
            else
            {
                _context.ApplicationTermAdmissionTypes.Remove(applicationTermAdmissionType);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GeApplicationTermAdmissionTypesDatatable(DataTablesStructs.SentParameters sentParameters,Guid applicationTermId, string searchValue)
        {
            var query = _context.ApplicationTermAdmissionTypes.Where(x=>x.ApplicationTermId == applicationTermId).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.AdmissionType.Name.Contains(searchValue));

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    name = x.AdmissionType.Name,
                    abreviation = x.AdmissionType.Abbreviation,
                    resolution = x.AdmissionType.Resolution,
                    resolutionDate = x.AdmissionType.ResolutionDate.ToString("dd/MM/yyyy"),
                    isExonerated = x.AdmissionType.IsExonerated,
                    type = x.AdmissionType.Type,
                    validityStart = x.AdmissionType.ValidityStart.HasValue ? x.AdmissionType.ValidityStart.Value.ToString("dd/MM/yyyy") : null,
                    validityEnd = x.AdmissionType.ValidityEnd.HasValue ? x.AdmissionType.ValidityEnd.Value.ToString("dd/MM/yyyy") : null,
                    cost = $"Pub: {x.AdmissionType.PublicSchoolConcept.Amount}/Priv: {x.AdmissionType.PrivateSchoolConcept.Amount}",
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

        public async Task<bool> AnyPostulantByApplicationTermIdAndAdmissionTypeId(Guid applicationTermId, Guid admissionTypeId)
            => await _context.Postulants.AnyAsync(x => x.ApplicationTermId == applicationTermId && x.AdmissionTypeId == admissionTypeId);
    }
}
