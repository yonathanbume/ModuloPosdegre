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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Implementations
{
    public class LinkRepository : Repository<Link>, ILinkRepository
    {
        public LinkRepository(AkdemicContext context) : base(context) { }
        public async Task<DataTablesStructs.ReturnedData<LinkTemplate>> GetAllLinkDatatable(DataTablesStructs.SentParameters sentParameters, int type, string title = null, byte? status = null)
        {
            Expression<Func<Link, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Title;
                    break;
                case "2":
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
            }


            var query = _context.Links
                .AsNoTracking();

            if (type == 1)
                query = query.Where(x => x.Type.Contains("CARRERA"));
            if (type == 2)
                query = query.Where(x => x.Type.Contains("ENLACE"));

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Title.Contains(title));
            }

            if (status != 0)
                query = query.Where(q => q.Status == status);

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new LinkTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    UrlImage = x.UrlImage,
                    PublicationDate = $"{x.PublicationDate:dd-MM-yyyy hh:mm:ss tt}",
                    Status = ConstantHelpers.Galery.Status.STATUS[x.Status],
                    Type = x.Type
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<LinkTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<DataTablesStructs.ReturnedData<LinkTemplate>> GetAllNetworkDatatable(DataTablesStructs.SentParameters sentParameters, string title = null, byte? status = null)
        {
            Expression<Func<Link, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Title;
                    break;
                case "2":
                    orderByPredicate = (x) => x.UrlDirection;
                    break;
                case "3":
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Status;
                    break;
                default:
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
            }


            var query = _context.Links
                .AsNoTracking();

            query = query.Where(x => x.Type.Contains("REDES"));

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Title.Contains(title));
            }

            if (status != 0)
                query = query.Where(q => q.Status == status);

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new LinkTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    UrlImage = x.UrlImage,
                    UrlDirection = x.UrlDirection,
                    PublicationDate = $"{x.PublicationDate:dd-MM-yyyy hh:mm:ss tt}",
                    Status = ConstantHelpers.Galery.Status.STATUS[x.Status],
                    Type = x.Type
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<LinkTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<LinkTemplate> GetLinkById(Guid id)
        {
            var model = await _context.Links
                .Where(x => x.Id == id)
                .Select(x => new LinkTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    UrlImage = x.UrlImage,
                    Status = ConstantHelpers.Galery.Status.STATUS[x.Status],
                    UrlDirection = x.UrlDirection,
                    StatusId = x.Status == 1 ? true : false
                }).FirstAsync();

            return model;
        }

        public async Task<LinkTemplate> GetLinkByIdWithOther(Guid id)
        {

            var model = await _context.Links
                .Where(x => x.Id == id)
                .Select(x => new LinkTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    Status = ConstantHelpers.Galery.Status.STATUS[x.Status],
                    UrlDirection = x.UrlDirection,
                    UrlImage = x.UrlImage,
                    StatusId = x.Status == 1 ? true : false,
                    Other = x.Title
                }).FirstAsync();

            return model;
        }

        public async Task<List<LinkTemplate>> GetLinkToHome()
        {
            var links = await _context.Links
             .Select(x => new LinkTemplate
             {
                 Id = x.Id,
                 UrlImage = x.UrlImage,
                 StatusId = x.Status == 1 ? true : false,
                 UrlDirection = x.UrlDirection,
                 Type = x.Type
             }).ToListAsync();

            return links;
        }
    }
}
