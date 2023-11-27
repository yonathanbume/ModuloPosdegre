using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Enrollment.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Implementations
{
    public class ChannelCareerRepository : Repository<ChannelCareer>, IChannelCareerRepository
    {
        public ChannelCareerRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ChannelCareer>> GetAllByChannelId(Guid id, Guid? appTermAdmissionTypeId)
        {
            var vacants = new List<Select2Structs.Result>();
            var appTermAdmissionType = await _context.ApplicationTermAdmissionTypes.Where(x => x.Id == appTermAdmissionTypeId).FirstOrDefaultAsync();
            var result = new List<ChannelCareer>();
            if (appTermAdmissionTypeId.HasValue)
            {
                vacants = await _context.Vacants
               .Where(v => v.CareerApplicationTerm.ApplicationTermId == appTermAdmissionType.ApplicationTermId && v.Number > 0)
               .Select(x => new Select2Structs.Result
               {
                   Id = x.CareerApplicationTerm.CareerId,
                   Text = x.CareerApplicationTerm.Career.Name
               })
               .ToListAsync();
            }

            var campuscareers = await _context.ChannelCareers.Include(x => x.Career).Where(x => x.ChannelId == id).ToListAsync();


            if (appTermAdmissionTypeId.HasValue)
            {
                result = campuscareers.Where(x => vacants.Any(y => y.Id == x.CareerId)).ToList();
            }
            else
            {
                result = campuscareers;
            }

            return result;
            //return await _context.ChannelCareers
            //    .Include(x=>x.Career)
            //      .Where(c => c.ChannelId == id).ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetChannelCareersDatatable(DataTablesStructs.SentParameters sentParameters, Guid channelId)
        {
            Expression<Func<ChannelCareer, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Career.Name;
                    break;
                default:
                    break;
            }

            var query = _context.ChannelCareers
                .Where(x => x.ChannelId == channelId)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.CareerId,
                    name = x.Career.Name
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

        public async Task<object> GetAcademicProgramsSelect2ByChannelId(Guid id)
        {
            var careers = await _context.ChannelCareers.Where(x => x.ChannelId == id).Select(x => x.CareerId).ToArrayAsync();
            var result = await _context.AcademicPrograms.Where(x => careers.Contains(x.CareerId))
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Code}-{x.Name}"
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<IEnumerable<ChannelCareer>> GetAllByChannelId(Guid id, Guid applicationTermId, Guid admissionTypeId)
        {
            var vacants = new List<Select2Structs.Result>();
            var appTermAdmissionType = await _context.ApplicationTermAdmissionTypes
                .Where(x => x.ApplicationTermId == applicationTermId && x.AdmissionTypeId == admissionTypeId)
                .FirstOrDefaultAsync();

            var result = new List<ChannelCareer>();
            if (appTermAdmissionType != null)
            {
                vacants = await _context.Vacants
                    .Where(v => v.CareerApplicationTerm.ApplicationTermId == appTermAdmissionType.ApplicationTermId && v.Number > 0)
                    .Select(x => new Select2Structs.Result
                    {
                        Id = x.CareerApplicationTerm.CareerId,
                        Text = x.CareerApplicationTerm.Career.Name
                    })
                    .ToListAsync();
            }

            var campuscareers = await _context.ChannelCareers
                .Include(x => x.Career)
                .Where(x => x.ChannelId == id)
                .ToListAsync();

            if (appTermAdmissionType != null)
                result = campuscareers.Where(x => vacants.Any(y => y.Id == x.CareerId)).ToList();
            else result = campuscareers;

            return result;
        }
    }
}
