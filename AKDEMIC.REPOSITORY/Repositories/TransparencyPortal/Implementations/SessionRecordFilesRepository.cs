using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Portal;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TransparencyPortal.Implementations
{
    public class SessionRecordFilesRepository : Repository<SessionRecordFile>, ISessionRecordFilesRepository
    {
        public SessionRecordFilesRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<object> GetById(Guid id)
        {
            var result = await _context.SessionRecordFiles
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.SessionRecordId,
                    FacultyEnabled = x.SessionRecord.OrderByFaculty,
                    x.Name,
                    Date = x.Date.HasValue ? x.Date.ToLocalDateFormat() : "",
                    x.FacultyId,
                    FacultyName = x.Faculty.Name,
                    x.IsLink,
                    x.Url,
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<SessionRecordFile>> GetBySessionRecordId(Guid id)
        {
            return await _context.SessionRecordFiles.Where(x => x.SessionRecordId == id).Include(x => x.Faculty).ToListAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid sessionRecordId, Guid? facultyId, DateTime? startDate, DateTime? endDate, string search)
        {
            var query = _context.SessionRecordFiles
                .Where(x => x.SessionRecordId == sessionRecordId)
                .OrderBy(x=>x.Faculty.Name).ThenBy(x=>x.Date)
                .AsNoTracking();

            if (facultyId.HasValue && facultyId != Guid.Empty)
                query = query.Where(x => x.FacultyId == facultyId);

            if (startDate.HasValue)
                query = query.Where(x => x.Date.HasValue && x.Date.Value.Date >= startDate.Value.Date);

            if (endDate.HasValue)
                query = query.Where(x => x.Date.HasValue && x.Date.Value.Date <= endDate.Value.Date);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new 
                {
                    x.Id,
                    x.Name,
                    x.FacultyId,
                    faculty = x.FacultyId.HasValue ? x.Faculty.Name : "ARCHIVOS DEL ACTA DE SESIÓN",
                    date = x.Date.ToLocalDateFormat(),
                    x.Url,
                    x.IsLink
                })
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

        public async Task<DataTablesStructs.ReturnedData<object>> GetFilesDatatable(DataTablesStructs.SentParameters parameters, Guid sessionRecordId, Guid? facultyId = null, DateTime? startDate = null, DateTime? endDate = null, string search = null)
        {
            Expression<Func<SessionRecordFile, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Date); break;
                case "2":
                    orderByPredicate = ((x) => x.Faculty.Name); break;
            }

            var query = _context.SessionRecordFiles
                .Where(x => x.SessionRecordId == sessionRecordId)
                .AsNoTracking();

            if (facultyId != null)
                query = query.Where(x => x.FacultyId == facultyId);

            if (startDate != null)
                query = query.Where(x => x.Date.HasValue && x.Date.Value.Date >= startDate.Value.Date);

            if (endDate != null)
                query = query.Where(x => x.Date.HasValue && x.Date.Value.Date <= endDate.Value.Date);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByCondition(parameters.OrderDirection, orderByPredicate)
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    x.Url,
                    facultyId = x.FacultyId,
                    faculty = x.FacultyId.HasValue ? x.Faculty.Name : null,
                    date = x.Date.HasValue ? x.Date.ToLocalDateFormat() : "-",
                    x.IsLink
                })
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
    }
}
