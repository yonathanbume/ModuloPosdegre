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
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class VacantRepository : Repository<Vacant>, IVacantRepository
    {
        public VacantRepository(AkdemicContext context) : base(context) { }

        public async Task<object> GetCareerTermsVacanciesChart(int? year, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            var vacants = _context.Vacants.AsQueryable();
            var careersQuery = _context.Careers.AsNoTracking();

            if (year.HasValue && year > 0)
                vacants = vacants.Where(x => x.CareerApplicationTerm.ApplicationTerm.Term.Year == year);

            if (applicationTermId != null)
                vacants = vacants.Where(x => x.CareerApplicationTerm.ApplicationTermId == applicationTermId);

            if (admissionTypeId != null)
                vacants = vacants.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    vacants = vacants.Where(x => x.CareerApplicationTerm.Career.QualityCoordinatorId == userId);
                    careersQuery = careersQuery.Where(x => x.QualityCoordinatorId == userId);
                }
            }


            //Usaremos Carreras temporalmente
            if (careerId != null)
            {
                vacants = vacants.Where(x => x.CareerApplicationTerm.CareerId == careerId);
            }

            var careers = await careersQuery
                .Select(x => new
                {
                    Career = x.Name,
                    Accepted = vacants.Where(y => y.CareerApplicationTerm.CareerId == x.Id).Sum(y => y.Number)
                })
                .OrderByDescending(x => x.Accepted)
                .ThenBy(x => x.Career)
                .ToListAsync();

            var result = new
            {
                categories = careers.Select(x => x.Career).ToList(),
                data = careers.Select(x => x.Accepted).ToList()
            };

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCareerTermsVacanciesDatatable(DataTablesStructs.SentParameters sentParameters, int? year, Guid? applicationTermId = null, Guid? careerId = null, Guid? admissionTypeId = null, ClaimsPrincipal user = null)
        {
            var vacants = _context.Vacants.AsNoTracking();

            if (year.HasValue && year > 0)
                vacants = vacants.Where(x => x.CareerApplicationTerm.ApplicationTerm.Term.Year == year);

            if (applicationTermId != null)
                vacants = vacants.Where(x => x.CareerApplicationTerm.ApplicationTermId == applicationTermId);

            if (admissionTypeId != null)
                vacants = vacants.Where(x => x.AdmissionTypeId == admissionTypeId);

            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(ConstantHelpers.ROLES.QUALITY_COORDINATOR))
                {
                    vacants = vacants.Where(x => x.CareerApplicationTerm.Career.QualityCoordinatorId == userId);
                }
            }

            //Usaremos Carreras temporalmente
            if (careerId != null)
            {
                vacants = vacants.Where(x => x.CareerApplicationTerm.CareerId == careerId);
            }

            var recordsFiltered = await vacants
                    .Select(x => new { x.CareerApplicationTerm.CareerId, x.AdmissionTypeId, x.CareerApplicationTerm.ApplicationTermId })
                    .Distinct()
                    .CountAsync();

            var data = await vacants
                    .Select(x => new
                    {
                        x.CareerApplicationTerm.CareerId,
                        Career = x.CareerApplicationTerm.Career.Name,
                        x.AdmissionTypeId,
                        AdmissionType = x.AdmissionType.Name,
                        x.CareerApplicationTerm.ApplicationTermId,
                        ApplicationTerm = x.CareerApplicationTerm.ApplicationTerm.Term.Name,
                        x.Number
                    })
                    .GroupBy(x => new { x.CareerId, x.Career, x.AdmissionTypeId, x.AdmissionType, x.ApplicationTermId, x.ApplicationTerm, x.Number })
                    .Select(x => new
                    {
                        x.Key.Career,
                        x.Key.AdmissionType,
                        x.Key.ApplicationTerm,
                        Accepted = x.Sum(y => y.Number)
                    })
                    .OrderByDescending(x => x.Accepted)
                    .ThenBy(x => x.Career)
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

        public async Task<List<Vacant>> GetVacantsIncludeCareerAdmissionTerm(CareerApplicationTerm careerTerm)
        {
            var vacants = await _context.Vacants
            .Include(x => x.CareerApplicationTerm)
            .Include(x => x.AdmissionType)
            .Include(x => x.CareerApplicationTerm.Career)
            .Where(x => x.CareerApplicationTerm == careerTerm).ToListAsync();

            return vacants;
        }

        public async Task<IEnumerable<Vacant>> GetAllVacantsWithData(Guid careerApplicationTermId, Guid? academicProgramId = null)
        {
            var qry = _context.Vacants
                .Where(v => v.CareerApplicationTermId == careerApplicationTermId)
                .AsQueryable();

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                qry = qry.Where(x => x.AcademicProgramId == academicProgramId);
            else qry = qry.Where(x => !x.AcademicProgramId.HasValue);

            var vacants = await qry
                .Include(x => x.CareerApplicationTerm)
                .Include(x => x.AdmissionType)
                .Include(x => x.CareerApplicationTerm.Career)
                .ToListAsync();

            return vacants;
        }
        public async Task<IEnumerable<Select2Structs.Result>> GetAvailableCareersSelect2(Guid applicationTermId)
        {
            var result = await _context.Vacants
                .Where(v => v.CareerApplicationTerm.ApplicationTermId == applicationTermId && v.Number > 0)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.CareerApplicationTerm.CareerId,
                    Text = x.CareerApplicationTerm.Career.Name,
                })
                .ToListAsync();

            result = result.Distinct().OrderBy(x => x.Text).ToList();

            return result;
        }

        public async Task<IEnumerable<Select2Structs.Result>> GetAcademicProgramVacantsSelect2(Guid applicationTermId, Guid careerId)
        {
            var vacantsByAcademicProgram = await _context.Vacants
                .Where(v => v.CareerApplicationTerm.ApplicationTermId == applicationTermId
                && v.Number > 0
                && v.CareerApplicationTerm.CareerId == careerId)
                .Select(x => x.AcademicProgramId)
                .ToListAsync();

            var academicPrograms = await _context.AcademicPrograms
                .Where(x => x.CareerId == careerId)
                .Select(x => new Select2Structs.Result
                {
                    Id = x.Id,
                    Text = x.Name
                }).ToListAsync();

            if (vacantsByAcademicProgram.Any(x => x.HasValue))
                academicPrograms = academicPrograms.Where(x => vacantsByAcademicProgram.Contains(x.Id)).ToList();

            var result = academicPrograms.OrderBy(x => x.Text).ToList();

            return result;
        }


        public async Task<IEnumerable<Select2Structs.Result>> GetAvailableCampusesSelect2(Guid applicationTermId, Guid careerId, Guid? academicProgramId = null)
        {
            var qry = _context.Vacants
                .Where(x => x.CareerApplicationTerm.ApplicationTermId == applicationTermId
                && x.CareerApplicationTerm.CareerId == careerId
                && x.Number > 0
                && x.CareerApplicationTerm.CampusId.HasValue)
                .AsNoTracking();

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
            {
                qry = qry.Where(x => x.AcademicProgramId == academicProgramId || !x.AcademicProgramId.HasValue);
            }

            var result = await qry
                .Select(x => new Select2Structs.Result
                {
                    Id = x.CareerApplicationTerm.Campus.Id,
                    Text = x.CareerApplicationTerm.Campus.Name
                }).ToListAsync();

            result = result.Distinct().OrderBy(x => x.Text).ToList();

            return result;
        }

        public async Task<List<Vacant>> GetVacantByIdList(Guid id)
        {
            var result = await _context.Vacants
            .Where(x => x.CareerApplicationTermId == id).ToListAsync();

            return result;
        }


        public async Task<bool> IsCareerAvailableInCampus(Guid campusId, Guid careerId, Guid? academicProgramId = null)
        {
            var qry = _context.Vacants
            .Where(x => x.CareerApplicationTerm.CampusId == campusId
            && x.CareerApplicationTerm.CareerId == careerId)
            .AsNoTracking();

            if (academicProgramId.HasValue && academicProgramId != Guid.Empty)
                qry = qry.Where(x => x.AcademicProgramId == academicProgramId || !x.AcademicProgramId.HasValue);

            var result = await qry.AnyAsync();
            return result;
        }
    }
}
