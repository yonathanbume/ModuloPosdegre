using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Admission.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Admission.Implementations
{
    public class ApplicationTermCampusRepository : Repository<ApplicationTermCampus>, IApplicationTermCampusRepository
    {
        public ApplicationTermCampusRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<Tuple<bool, string>> AddRemoveApplicationTermExamCampusId(Guid applicationTermId, Guid examCampusId, int type)
        {
            var applicationTermCampus = await _context.ApplicationTermCampuses.Where(x => x.ApplicationTermId == applicationTermId && x.CampusId == examCampusId).FirstOrDefaultAsync();

            if (applicationTermCampus == null)
            {
                applicationTermCampus = new ApplicationTermCampus
                {
                    CampusId = examCampusId,
                    ApplicationTermId = applicationTermId,
                };
                if (type == 1)
                    applicationTermCampus.ForExam = true;
                if (type == 2)
                    applicationTermCampus.ToApply = true;
                await _context.ApplicationTermCampuses.AddAsync(applicationTermCampus);
            }
            else
            {
                if (type == 1)
                    applicationTermCampus.ForExam = !applicationTermCampus.ForExam;
                if (type == 2)
                    applicationTermCampus.ToApply = !applicationTermCampus.ToApply;

                if (!applicationTermCampus.ForExam && !applicationTermCampus.ToApply)
                {
                    if (await _context.Postulants.AnyAsync(x => x.ApplicationTermId == applicationTermId
                        && (x.ExamCampusId == examCampusId || x.CampusId == examCampusId)))
                    {
                        return new Tuple<bool, string>(false, "Se encontraron postulantes inscritos en esta sede");
                    }
                    _context.ApplicationTermCampuses.Remove(applicationTermCampus);
                }                    
            }

            await _context.SaveChangesAsync();
            return new Tuple<bool, string>(true, "");
        }

        public async Task<bool> AnyPostulantByApplicationTermIdAndExamCampusId(Guid applicationTermId, Guid examCampusId)
            => await _context.Postulants.AnyAsync(x => x.ApplicationTermId == applicationTermId && (x.ExamCampusId == examCampusId || x.CampusId == examCampusId));

        public async Task<object> GetCampusByApplicationTermSelect(Guid applicationTermId, int? type = null)
        {
            var qry = _context.ApplicationTermCampuses
                .Where(x => x.ApplicationTermId == applicationTermId)
                .AsNoTracking();

            if (type.HasValue)
            {
                if (type == 1) qry = qry.Where(x => x.ForExam);

                if (type == 2) qry = qry.Where(x => x.ToApply);
            }

            var result = await qry
                .OrderBy(x => x.Campus.Name)
                .Select(x => new
                {
                    id = x.CampusId,
                    text = x.Campus.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetCampusesDatatable(DataTablesStructs.SentParameters sentParameters, Guid applicationTermId)
        {
            var query = _context.Campuses.AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    campusName = x.Name,
                    campusCode = x.Code,
                    campusDistrict = x.District.Name ?? "",
                    forExam = x.ApplicationTermCampuses.Any(y => y.ApplicationTermId == applicationTermId && y.ForExam),
                    toApply = x.ApplicationTermCampuses.Any(y => y.ApplicationTermId == applicationTermId && y.ToApply),
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
    }
}
