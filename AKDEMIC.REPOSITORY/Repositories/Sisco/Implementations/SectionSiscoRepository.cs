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
    public class SectionSiscoRepository : Repository<SectionSisco>, ISectionSiscoRepository
    {
        public SectionSiscoRepository(AkdemicContext context) : base(context) { }
        public async Task<DataTablesStructs.ReturnedData<SectionSiscoTemplate>> GetAllSectionDatatable(DataTablesStructs.SentParameters sentParameters, string title = null, byte? status = null)
        {
            Expression<Func<SectionSisco, dynamic>> orderByPredicate = null;

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
                case "4":
                    orderByPredicate = (x) => x.SequenceOrder;
                    break;
                default:
                    orderByPredicate = (x) => x.PublicationDate;
                    break;
            }


            var query = _context.SectionsSisco
                .AsNoTracking();

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
                .Select(x => new SectionSiscoTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    UrlImage = x.UrlImage,
                    PublicationDate = $"{x.PublicationDate:dd-MM-yyyy hh:mm:ss tt}",
                    Status = ConstantHelpers.Galery.Status.STATUS[x.Status],
                    SequenceOrder = ConstantHelpers.Section.SequenceOrder.SEQUENCEORDER[x.SequenceOrder]
                }).ToListAsync();

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<SectionSiscoTemplate>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<SectionSiscoTemplate> GetAvailableOrdersAndListSequenceOrder()
        {
            var section = new SectionSiscoTemplate();
            var sections = await _context.SectionsSisco.ToListAsync();

            var availableOrders = ConstantHelpers.Section.SequenceOrder.SEQUENCEORDER
                .Where(x => !sections.Any(b => b.SequenceOrder == x.Key));

            section.ListSequenceOrder = new SelectList(availableOrders, "Key", "Value");

            return section;
        }

        public async Task<SectionSiscoTemplate> GetSectionById(Guid id)
        {
            var section = await _context.Sections.FindAsync(id);
            var sections = await _context.Sections.ToListAsync();

            var model = await _context.SectionsSisco
                .Where(x => x.Id == id)
                .Select(x => new SectionSiscoTemplate
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    UrlImage = x.UrlImage,
                    Status = ConstantHelpers.Galery.Status.STATUS[x.Status],
                    UrlDirection = x.UrlDirection,
                    StatusDirection = ConstantHelpers.Galery.Status.STATUS[x.StatusDirection],
                    NameDirection = x.NameDirection,
                    SequenceOrder = ConstantHelpers.Section.SequenceOrder.SEQUENCEORDER[x.SequenceOrder],
                    SequenceOrderId = x.SequenceOrder,
                    StatusDirectionId = x.StatusDirection == 1 ? true : false,
                    StatusId = x.Status == 1 ? true : false
                }).FirstAsync();


            model.ListSequenceOrder = new SelectList(ConstantHelpers.Section.SequenceOrder.SEQUENCEORDER, "Key", "Value");
            return model;
        }

        public async Task<List<SectionSiscoTemplate>> GetSectionToHome()
        {
            var sections = await _context.SectionsSisco
            .Select(x => new SectionSiscoTemplate
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                UrlImage = x.UrlImage,
                SequenceOrderId = x.SequenceOrder,
                NameDirection = x.NameDirection,
                UrlDirection = x.UrlDirection,
                StatusDirectionId = x.StatusDirection == 1 ? true : false,
                StatusId = x.Status == 1 ? true : false,
                ListSequenceOrder = new SelectList("", "Key", "Value")
            }).ToListAsync();

            return sections;
        }
    }
}
