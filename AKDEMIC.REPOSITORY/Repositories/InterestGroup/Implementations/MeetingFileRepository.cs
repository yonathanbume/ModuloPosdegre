using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.InterestGroup;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Extensions;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.MeetingFile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Implementations
{
    public class MeetingFileRepository : Repository<MeetingFile>, IMeetingFileRepository
    {
        public MeetingFileRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<MeetingFile>> GetMeetingFilesByMeetingId(Guid meetingId)
        {
            return await _context.MeetingFiles.Where(x => x.MeetingId == meetingId).ToArrayAsync();
        }

        public async Task<DataTablesStructs.ReturnedData<MeetingFileTemplate>> GetMeetingFilesDatatable(DataTablesStructs.SentParameters parameters, Guid id)
        {
            return await GetMeetingFilesDatatable(parameters, id, null, GetMeetingDatatableOrderByPredicate(parameters), GetMeetingDatatableSearchValuePredicate());
        }

        #region PIVATE
        private Expression<Func<MeetingFileTemplate, dynamic>> GetMeetingDatatableOrderByPredicate(DataTablesStructs.SentParameters sentParameters)
        {
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    return ((x) => x.Name);
                case "1":
                    return ((x) => x.UploadDate);
                default:
                    return ((x) => x.UploadDate);
            }
        }

        private Func<MeetingFileTemplate, string[]> GetMeetingDatatableSearchValuePredicate()
        {
            return (x) => new[]
            {
                x.Name+"",
                x.UploadDate+"",
            };
        }

        private async Task<DataTablesStructs.ReturnedData<MeetingFileTemplate>> GetMeetingFilesDatatable(
            DataTablesStructs.SentParameters sentParameters, Guid id,
            Expression<Func<MeetingFileTemplate, MeetingFileTemplate>> selectPredicate = null,
            Expression<Func<MeetingFileTemplate, dynamic>> orderByPredicate = null,
            Func<MeetingFileTemplate, string[]> searchValuePredicate = null)
        {
            var query = _context.MeetingFiles.Where(x => x.MeetingId == id)
                .AsQueryable();

            var result = query
                        .Select(x => new MeetingFileTemplate
                        {
                            Id = x.Id,
                            Name = x.Name,
                            UploadDate = $"{x.UploadDate.ToDefaultTimeZone():dd-MM-yyyy}",
                            UrlFile = x.UrlFile,
                            Type = x.Type
                        })
                        .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                        .AsNoTracking();

            return await result.ToDataTables(sentParameters, selectPredicate);
        }
        #endregion
    }
}
