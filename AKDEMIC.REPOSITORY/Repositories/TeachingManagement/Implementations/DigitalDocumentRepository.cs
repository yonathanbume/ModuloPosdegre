using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Implementations
{
    public sealed class DigitalDocumentRepository : Repository<DigitalDocument>, IDigitalDocumentRepository
    {
        public DigitalDocumentRepository(AkdemicContext context) : base(context) { }

        async Task<object> IDigitalDocumentRepository.GetAllAsModelA()
        {
            var query = _context.DigitalDocuments.AsQueryable();

            var result = await query.Select(x => new
            {

                id = x.Id,
                title = x.Title,
                x.FileUrl,
                careerName = x.Career.Name,
                type = ConstantHelpers.DIGITAL_DOCUMENTS.TYPES.VALUES[x.Type],
                classD = ConstantHelpers.DIGITAL_DOCUMENTS.CLASS.VALUES[x.Class],
            }).ToListAsync();

            return result;
        }

        async Task<object> IDigitalDocumentRepository.GetAsModelB(Guid? id, string userToVerify)
        {
            var query = _context.DigitalDocuments.AsQueryable();

            if (id.HasValue)
                query = query.Where(x => x.Id == id.Value);

            var result = await query.Select(x => new
            {

                id = x.Id,
                title = x.Title,
                x.Year,
                x.FileUrl,
                x.Type,
                x.Class,
                x.UserId,
                nResolution = x.ResolutionNumber,
                x.CareerId,
                isOwn = x.UserId == userToVerify
            })
            .FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetDigitalDocumentToTeacherDatatable(DataTablesStructs.SentParameters parameters, string teacherId)
        {
            var careers = await _context.Teachers.Where(x => x.UserId == teacherId).Select(x => x.AcademicDepartment.Faculty.Careers).FirstOrDefaultAsync();
            var careersId = careers.Select(x => x.Id).ToHashSet();

            var query = _context.DigitalDocuments
                .Where(x => careersId.Contains(x.CareerId))
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            var data = await query
                .Skip(parameters.PagingFirstRecord)
                .Take(parameters.RecordsPerDraw)
                .Select(x => new
                {
                    id = x.Id,
                    x.Title,
                    x.FileUrl,
                    type = ConstantHelpers.DIGITAL_DOCUMENTS.TYPES.VALUES[x.Type],
                    classD = ConstantHelpers.DIGITAL_DOCUMENTS.CLASS.VALUES[x.Class],
                })
                .ToListAsync();

            var recordsTotal = data.Count;

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