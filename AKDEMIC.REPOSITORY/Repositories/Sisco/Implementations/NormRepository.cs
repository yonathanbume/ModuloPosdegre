using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Implementations
{
    public class NormRepository : Repository<Norm>, INormRepository
    {
        public NormRepository(AkdemicContext context) : base(context) { }
        public async Task<DataTablesStructs.ReturnedData<NormTemplate>> GetAllNormDatatable(DataTablesStructs.SentParameters sentParameters, SearchNormTemplate search)
        {
            Expression<Func<Norm, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.StandardNumber;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Sumilla;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Type;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Status;
                    break;
                case "4":
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
                case "5":
                    orderByPredicate = (x) => x.TransmissionDate;
                    break;
                default:
                    orderByPredicate = (x) => x.StandardNumber;
                    break;
            }


            var query = _context.Norms.Where(x => x.Status != 3)
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(search.StandardNumber))
            {
                query = query.Where(x => x.StandardNumber.Contains(search.StandardNumber));
            }

            if (!string.IsNullOrEmpty(search.Sumilla))
            {
                query = query.Where(x => x.Sumilla.Contains(search.Sumilla));
            }

            if (search.Type != 0)
            {
                query = query.Where(x => x.Type == search.Type);
            }

            if (search.Status != 0)
            {
                query = query.Where(x => x.Status == search.Status);
            }

            //var today = DateTime.Today;

            if (!string.IsNullOrEmpty(search.StartPublicationDate) && !string.IsNullOrEmpty(search.EndPublicationDate))
            {
                query = query.Where(x => x.PublicationDate >= DateTime.ParseExact(search.StartPublicationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                                    && x.PublicationDate <= DateTime.ParseExact(search.EndPublicationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            else
            {
                if (!string.IsNullOrEmpty(search.StartPublicationDate))
                {
                    query = query.Where(x => x.PublicationDate >= DateTime.ParseExact(search.StartPublicationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }

                if (!string.IsNullOrEmpty(search.EndPublicationDate))
                {
                    query = query.Where(x => x.PublicationDate >= DateTime.ParseExact(search.EndPublicationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }
            }

            if (!string.IsNullOrEmpty(search.StartTransmissionDate) && !string.IsNullOrEmpty(search.EndTransmissionDate))
            {
                query = query.Where(x => x.TransmissionDate >= DateTime.ParseExact(search.StartTransmissionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                                    && x.TransmissionDate <= DateTime.ParseExact(search.EndTransmissionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            else
            {
                if (!string.IsNullOrEmpty(search.StartTransmissionDate))
                {
                    query = query.Where(x => x.TransmissionDate >= DateTime.ParseExact(search.StartTransmissionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }

                if (!string.IsNullOrEmpty(search.EndTransmissionDate))
                {
                    query = query.Where(x => x.TransmissionDate >= DateTime.ParseExact(search.EndTransmissionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                }
            }

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new NormTemplate
                {
                    Id = x.Id,
                    StandardNumber = x.StandardNumber,
                    Sumilla = x.Sumilla,
                    Type = ConstantHelpers.Norms.Type.TYPE[x.Type],
                    Status = ConstantHelpers.Norms.Status.STATUS[x.Status],
                    PublicationDate = $"{x.PublicationDate:dd/MM/yyyy}",
                    TransmissionDate = $"{x.TransmissionDate:dd/MM/yyyy}",
                    UrlPdf = x.UrlPdf,
                    UrlWord = x.UrlWord,
                    ListStatus = new SelectList("", "Key", "Value"),
                    ListType = new SelectList("", "Key", "Value")
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<NormTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<NormTemplate> GetNormById(Guid id)
        {
            var norm = await _context.Norms.FindAsync(id);
            var norms = await _context.Norms.ToListAsync();

            var model = await _context.Norms
                .Where(x => x.Id == id)
                .Select(x => new NormTemplate
                {
                    Id = x.Id,
                    StandardNumber = x.StandardNumber,
                    Sumilla = x.Sumilla,
                    Type = ConstantHelpers.Norms.Type.TYPE[x.Type],
                    Status = ConstantHelpers.Norms.Status.STATUS[x.Status],
                    StatusId = x.Status,
                    TypeId = x.Type,
                    PublicationDate = $"{x.PublicationDate:dd/MM/yyyy}",
                    TransmissionDate = $"{x.TransmissionDate:dd/MM/yyyy}",
                    UrlPdf = x.UrlPdf,
                    UrlWord = x.UrlWord
                }).FirstAsync();

            model.StatusId = norm.Status;
            model.TypeId = norm.Type;
            model.ListStatus = new SelectList(ConstantHelpers.Norms.Status.STATUS, "Key", "Value");
            model.ListType = new SelectList(ConstantHelpers.Norms.Type.TYPE, "Key", "Value");
            return model;
        }
    }
}
