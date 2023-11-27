using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Scale.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.ScaleExtraContractField;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Scale.Implementations
{
    public class ScaleExtraContractFieldRepository : Repository<ScaleExtraContractField>, IScaleExtraContractFieldRepository
    {
        public ScaleExtraContractFieldRepository(AkdemicContext context) : base(context) { }

        public async Task<List<ScaleExtraContractField>> GetByScaleResolutionTypeAndUser(string userId, string resolutionTypeName)
        {
            return await _context.ScaleExtraContractFields
                .Include(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType)
                .Include(x => x.WorkerLaborCondition)
                .Include(x => x.WorkerLaborCategory)
                .Where(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == resolutionTypeName 
                       && x.ScaleResolution.UserId == userId)
                .OrderBy(x => x.ScaleResolution.EndDate).ThenBy(x => x.ScaleResolution.BeginDate)
                .ToListAsync();
        }

        public async Task<List<ScaleExtraContractField>> GetByScaleResolutionTypeAndUser(string userId, List<string> resolutionTypeName)
        {
            return await _context.ScaleExtraContractFields
                .Include(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType)
                .Include(x => x.WorkerLaborCondition)
                .Include(x => x.WorkerLaborCategory)
                .Where(x => resolutionTypeName.Contains(x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name) 
                       && x.ScaleResolution.UserId == userId)
                .OrderBy(x => x.ScaleResolution.EndDate).ThenBy(x => x.ScaleResolution.BeginDate)
                .ToListAsync();
        }

        public async Task<WorkCertificateTemplate> GetContractCertificateData(Guid id)
        {
            var today = DateTime.UtcNow.AddHours(-5);
            var dependecies = await _context.Dependencies.Select(x => new { x.Id, x.Name }).ToListAsync();

            var dataDb = await _context.ScaleExtraContractFields
                .Where(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Contratos" && x.Id == id)
                .Select(x => new 
                {
                    x.ScaleResolution.User.FullName,
                    x.ScaleResolution.User.Dni,
                    x.ScaleResolution.User.Sex,
                    x.ScaleResolution.BeginDate,
                    x.ScaleResolution.EndDate,
                    x.ScaleResolution.ResolutionNumber,
                    x.LaborPosition,
                    x.DependencyId
                }).FirstOrDefaultAsync();

            var userDependency = dependecies.Where(x => x.Id == dataDb.DependencyId).Select(x => x.Name).FirstOrDefault();

            var data = new WorkCertificateTemplate
            {
                ImagePathLogo = "",
                FullName = dataDb.FullName,
                Dni = dataDb.Dni,
                Sex = dataDb.Sex,
                BeginDate = dataDb.BeginDate.ToLocalDateFormat(),
                EndDate = dataDb.EndDate.HasValue ?
                        dataDb.EndDate > today ? "hasta la actualidad" : $"hasta {dataDb.EndDate.ToLocalDateFormat()}"
                        : "",
                LaborPosition = $"{dataDb.LaborPosition}",
                Area = userDependency
            };

            return data;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetContractRecordByUser(DataTablesStructs.SentParameters sentParameters, string userId, string searchValue = null)
        {
            Expression<Func<ScaleExtraContractField, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.ScaleResolution.ResolutionNumber); break;
                case "1":
                    orderByPredicate = ((x) => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.ScaleResolution.BeginDate); break;
                case "3":
                    orderByPredicate = ((x) => x.ScaleResolution.EndDate); break;
                case "4":
                    orderByPredicate = ((x) => x.ScaleResolution.DocumentType); break;
                case "5":
                    orderByPredicate = ((x) => x.LaborPosition); break;
                case "6":
                    orderByPredicate = ((x) => x.WorkerLaborCategory.Name); break;
                case "7":
                    orderByPredicate = ((x) => x.ScaleResolution.Observation); break;
            }

            var query = _context.ScaleExtraContractFields
                .Where(x => x.ScaleResolution.UserId == userId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Select(x => new
                {
                    documentType = ConstantHelpers.SCALERESOLUTION_DOCUMENT_TYPE.VALUES.ContainsKey(x.ScaleResolution.DocumentType)
                                    ? ConstantHelpers.SCALERESOLUTION_DOCUMENT_TYPE.VALUES[x.ScaleResolution.DocumentType] : "No Especificado",
                    beginDate = x.ScaleResolution.BeginDate.ToLocalDateFormat() ?? "-",
                    scaleResolutionType = x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name ?? "-",
                    endDate = x.ScaleResolution.EndDate.ToLocalDateFormat() ?? "-",
                    resolutionNumber = x.ScaleResolution.ResolutionNumber ?? "-",
                    observations = x.ScaleResolution.Observation ?? "-",
                    laborPosition = x.LaborPosition ?? "-",
                    category = x.WorkerLaborCategory.Name ?? "-"
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

        public async Task<ScaleExtraContractField> GetScaleExtraContractFieldByResolutionId(Guid resolutionId)
        {
            return await _context.ScaleExtraContractFields
                .Include(x => x.TeacherDedication)
                .Include(x => x.WorkerLaborCategory)
                .Include(x => x.WorkerLaborCondition)
                .FirstOrDefaultAsync(x => x.ScaleResolutionId == resolutionId);
        }

        public async Task<List<TeacherDataReportViewModel>> GetTeacherDataReportViewModel(Guid facultyId)
        {
            var users = _context.Users.AsQueryable();

            var teachers = _context.Teachers.Where(x => x.Career.FacultyId == facultyId).AsQueryable();

            users = users.Where(x => teachers.Any(y => y.UserId == x.Id));

            var ascensos = _context.ScaleExtraContractFields
                .Where(x => x.ScaleResolution.ScaleSectionResolutionType.ScaleResolutionType.Name == "Ascensos")
                .AsQueryable();


            var result = await users
                .Select(x => new TeacherDataReportViewModel
                {
                    Name = x.Name,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    FullName = x.FullName,
                    EntryDate = x.WorkerLaborInformation.EntryDate.ToLocalDateFormat(),
                    WorkerCategory = x.WorkerLaborInformation.WorkerLaborCategory.Name,
                    DoctoralDegree = x.WorkerDoctoralDegrees.OrderBy(y => y.ExpeditionDate).Select(y => y.Specialty).FirstOrDefault(),
                    ProfessionalTitle = x.WorkerProfessionalTitles.OrderBy(y => y.ExpeditionDate).Select(y => y.Specialty).FirstOrDefault(),
                    MasterDegree = x.WorkerMasterDegrees.OrderBy(y => y.ExpeditionDate).Select(y => y.Specialty).FirstOrDefault(),
                    Dedication = teachers.Where(y => y.UserId == x.Id).Select(y => y.TeacherDedication.Name).FirstOrDefault(),
                    PromotionDate = ascensos
                                .Where(y => y.ScaleResolution.UserId == x.Id)
                                .OrderBy(y => y.ScaleResolution.ExpeditionDate)
                                .Select(y => y.ScaleResolution.ExpeditionDate.ToLocalDateFormat())
                                .FirstOrDefault()
                }).ToListAsync();

            return result;
        }
    }
}
