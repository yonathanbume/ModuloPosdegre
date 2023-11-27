using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Sisco;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Sisco.Template;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Implementations
{
    public class BannerRepository : Repository<Banner>, IBannerRepository
    {
        public BannerRepository(AkdemicContext context) : base(context) { }
        public async Task<DataTablesStructs.ReturnedData<BannerTemplate>> GetAllBannerDatatable(DataTablesStructs.SentParameters sentParameters, string headline = null, byte? status = null)
        {
            Expression<Func<Banner, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Headline;
                    break;
                case "2":
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Status;
                    break;
                case "4":
                    orderByPredicate = (x) => x.SequenceOrder;
                    break;
                default:
                    orderByPredicate = (x) => x.SequenceOrder;
                    break;
            }


            var query = _context.Banners
                .AsNoTracking();

            var recordsFiltered = await query.CountAsync();

            if (!string.IsNullOrEmpty(headline))
            {
                query = query.Where(q => q.Headline.Contains(headline));
            }

            if (status != 0)
                query = query.Where(q => q.Status == status);

            query = query.AsQueryable();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new BannerTemplate
                {
                    Id = x.Id,
                    Headline = x.Headline,
                    UrlImage = x.UrlImage,
                    PublicationDate = $"{x.PublicationDate:dd-MM-yyyy hh:mm:ss tt}",
                    Status = ConstantHelpers.Galery.Status.STATUS[x.Status],
                    SequenceOrder = (x.SequenceOrder.HasValue && x.SequenceOrder.Value != 0) ? ConstantHelpers.Galery.SequenceOrder.SEQUENCEORDER[x.SequenceOrder.Value] : "SIN ORDEN",
                    SequenceOrderId = x.SequenceOrder.HasValue ? x.SequenceOrder.Value : 0
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<BannerTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<BannerTemplate> GetAvailableOrdersAndListSequenceOrder()
        {
            var banner = new BannerTemplate();
            var banners = await _context.Banners.ToListAsync();

            var availableOrders = ConstantHelpers.Galery.SequenceOrder.SEQUENCEORDER
                .Where(x => !banners.Any(b => b.SequenceOrder == x.Key));

            banner.ListSequenceOrder = new SelectList(availableOrders, "Key", "Value");

            return banner;
        }

        public async Task<BannerTemplate> GetBannerById(Guid id)
        {
            var banner = await _context.Banners.FindAsync(id);
            var banners = await _context.Banners.ToListAsync();

            var model = await _context.Banners
                .Where(x => x.Id == id)
                .Select(x => new BannerTemplate
                {
                    Id = x.Id,
                    Headline = x.Headline,
                    Description = x.Description,
                    UrlImage = x.UrlImage,
                    Status = ConstantHelpers.Galery.Status.STATUS[x.Status],
                    UrlDirection = x.UrlDirection,
                    StatusDirection = ConstantHelpers.Galery.Status.STATUS[x.StatusDirection],
                    NameDirection = x.NameDirection,
                    SequenceOrder = x.SequenceOrder.HasValue ? ConstantHelpers.Galery.SequenceOrder.SEQUENCEORDER[x.SequenceOrder.Value] : "",
                    SequenceOrderId = x.SequenceOrder.HasValue ? x.SequenceOrder.Value : 0,
                    StatusDirectionId = x.StatusDirection == 1 ? true : false,
                    StatusId = x.Status == 1 ? true : false
                }).FirstAsync();

            var availableOrders = ConstantHelpers.Galery.SequenceOrder.SEQUENCEORDER
                .Where(x => !banners.Where(b => b.Id != id).Any(b => b.SequenceOrder == x.Key));

            model.ListSequenceOrder = new SelectList(availableOrders, "Key", "Value");
            return model;
        }

        public async Task<List<BannerTemplate>> GetBannersToHome()
        {
            var banners = await _context.Banners
                .Where(x => x.Status == 1)
                .Take(5)
             .Select(x => new BannerTemplate
             {
                 Id = x.Id,
                 Headline = x.Headline,
                 Description = x.Description,
                 UrlImage = x.UrlImage,
                 NameDirection = x.NameDirection,
                 UrlDirection = x.UrlDirection,
                 StatusId = x.Status == 1,
                 StatusDirectionId = x.StatusDirection == 1,
                 SequenceOrder = ConstantHelpers.Galery.SequenceOrder.SEQUENCEORDER[x.SequenceOrder.Value],
                 SequenceOrderId = x.SequenceOrder.Value,
                 ListSequenceOrder = new SelectList("", "Key", "Value")
             }).ToListAsync();


            return banners;
        }

        public async Task<List<Banner>> GetAllBannersActive()
            => await _context.Banners.Where(x => x.Status == 1).ToListAsync();

        public async Task<IEnumerable<object>> GetAvailableOrdersAndListSequenceOrderSelect2()
        {
            var banners = await _context.Banners.ToListAsync();

            var availableOrders = ConstantHelpers.Galery.SequenceOrder.SEQUENCEORDER
                .Where(x => !banners.Any(b => b.SequenceOrder == x.Key));

            var result = availableOrders.Select(x => new
            {
                id = x.Key,
                text = x.Value
            }).ToList();

            result.Insert(0, new { id = 0, text = "SIN ORDEN" });

            return result;
        }
    }
}
