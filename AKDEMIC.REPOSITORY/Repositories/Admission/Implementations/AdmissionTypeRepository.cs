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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class AdmissionTypeRepository : Repository<AdmissionType>, IAdmissionTypeRepository
    {
        public AdmissionTypeRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<Select2Structs.Result>> GetAdmissionTypesSelect2ClientSide(bool includeTitle = false)
        {
            var result = await _context.AdmissionTypes
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                })
                .OrderBy(x => x.Text)
                .ToListAsync();

            if (includeTitle)
                result.Insert(0, new Select2Structs.Result { Id = new Guid(), Text = "Todas" });
            return result;
        }
        public async Task<AdmissionTypeDescount> GetAdmissionTypeDescounts(Guid admissionTypeId, Guid currentApplicationTermId)
        {
            var result = await _context.AdmissionTypeDescounts.FirstOrDefaultAsync(a =>
                       a.AdmissionTypeId == admissionTypeId && a.TermId == currentApplicationTermId);

            return result;
        }

        public async Task<List<Select2Structs.Result>> GetAdmissionTypeJson()
        {
            var result = await _context.AdmissionTypes
                .Where(at => at.IsPermanent)
                .OrderBy(x => x.Name)
                .Select(at => new Select2Structs.Result
                {
                    Id = at.Id,
                    Text = at.Name
                }).ToListAsync();
            return result;
        }

        public async Task<AdmissionType> GetNameByCellExcel(string cell)
        {
            var admissionType = await _context.AdmissionTypes.FirstOrDefaultAsync(x => x.Name.ToUpper() == cell.ToUpper());
            return admissionType;
        }

        public async Task<object> GetAmissionTypeIrregular()
        {
            var result = await _context.AdmissionTypes
                .Where(x => x.Type == ConstantHelpers.ADMISSION_MODE.EXTRAORDINARY)
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                }).OrderBy(x => x.text).Distinct().ToListAsync();

            result.Insert(0, new { id = new Guid(), text = "Todas" });

            return result;
        }

        public async Task<Guid> GetGuidFirst()
        {
            var result = await _context.AdmissionTypes.FirstOrDefaultAsync();

            return result.Id;
        }

        public async Task<object> GetAdmissionTypesTerm(Guid termId)
        {
            var result = await (from a in _context.AdmissionTypes
                                    //where a.IsValid
                                from d in _context.AdmissionTypeDescounts.Where(d => d.AdmissionTypeId == a.Id && d.TermId == termId).DefaultIfEmpty()
                                select new
                                {
                                    id = a.Id,
                                    name = a.Name,
                                    descount = d != null ? d.Descount : 0.00f
                                }).ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAdmissionTypesCategories(DataTablesStructs.SentParameters sentParameters, string search, bool showInactive)
        {
            Expression<Func<AdmissionType, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Abbreviation); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.Resolution); break;
                case "7":
                    orderByPredicate = ((x) => x.Resolution); break;
                default:
                    break;
            }

            var query = _context.AdmissionTypes
                .Where(x => x.Type != ConstantHelpers.ADMISSION_MODE.AGREEMENT)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                var normalizedSearch = search.Normalize().ToUpper();
                query = query.Where(x => x.Name.ToUpper().Contains(normalizedSearch));
            }

            if (!showInactive)
                query = query.Where(x => x.IsActive);

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
                cost =x.IsExonerated ? "Exonerado" : $"Pub: S/. {x.PublicSchoolConcept.Amount:0.00}<br /> Priv: S/. {x.PrivateSchoolConcept.Amount:0.00}",
                publicConceptId = x.PublicSchoolConceptId,
                privateConceptId = x.PrivateSchoolConceptId

            }).ToListAsync();

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public async Task<List<AdmissionType>> GetByAbbreviation(string v)
        {
            return await _context.AdmissionTypes.Where(x => x.Abbreviation == v).ToListAsync();
        }

        public async Task<AdmissionType> GetAgreementAdmissionType()
            => await _context.AdmissionTypes.Where(x => x.Type == ConstantHelpers.ADMISSION_MODE.AGREEMENT).FirstOrDefaultAsync();
    }
}
