﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Evaluation;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Evaluation.Templates;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.REPOSITORY.Repositories.Evaluation.Implementations
{
    public class RegisterCulturalActivityRepository : Repository<RegisterCulturalActivity>, IRegisterCulturalActivityRepository
    {
        public RegisterCulturalActivityRepository(AkdemicContext context) : base(context)
        {
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetParticipantsByCulturalActivityIdDataTable(DataTablesStructs.SentParameters sentParameters, Guid id, string search)
        {
            Expression<Func<RegisterCulturalActivity, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Date); break;
                case "1":
                    orderByPredicate = ((x) => x.User.UserName); break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "3":
                    orderByPredicate = ((x) => x.User.Dni); break;
                case "4":
                    orderByPredicate = ((x) => x.User.PhoneNumber); break;
                default:
                    orderByPredicate = ((x) => x.User.UserName); break;
            }

            IQueryable<RegisterCulturalActivity> query = _context.RegisterCulturalActivities.Where(x => x.CulturalActivityId == id).AsNoTracking();

            int filteredcount = await query.CountAsync();
            query = query.OrderBy(x => x.Name);

            int recordsFiltered = await query.CountAsync();
            var data = await query
                .OrderByDescendingCondition(sentParameters.OrderDirection == ConstantHelpers.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION, orderByPredicate)
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .Select(x => new
                {
                    name = x.IsInternal ? x.User.FullName : $"{x.PaternalSurname} {x.MaternalSurname}, {x.Name}",
                    dni = x.IsInternal ? x.User.Dni : x.Dni,
                    date = x.Date.ToString("dd/MM/yyyy"),
                    cellphone = x.IsInternal ? x.User.PhoneNumber : x.Cellphone,
                    user = x.IsInternal ? "Interno" : "Externo"
                }).ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IEnumerable<MemberRegister>> GetParticipantsByCulturalActivityId(Guid id)
        {
            var result = await _context.RegisterCulturalActivities.Where(x => x.CulturalActivityId == id)
                .Select(x => new MemberRegister
                {
                    FullName = x.IsInternal ? x.User.FullName : $"{x.PaternalSurname} {x.MaternalSurname}, {x.Name}",
                    Dni = x.IsInternal ? x.User.Dni : x.Dni,
                    Email = x.IsInternal ? x.User.Email : x.Email,
                    RegisterDate = x.Date.ToString("dd/MM/yyyy"),
                    Mobile = x.IsInternal ? x.User.PhoneNumber : x.Cellphone,
                    Type = x.IsInternal ? "Interno" : "Externo"
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<bool> IsRegistered(Guid activityId,string dni, bool isId)
        {
            RegisterCulturalActivity result = new RegisterCulturalActivity();

            if (isId)
            {
                result = await _context.RegisterCulturalActivities.Where(x=>x.CulturalActivityId == activityId).FirstOrDefaultAsync(x => x.IsInternal && x.UserId == dni);
            }
            else
            {
                result = await _context.RegisterCulturalActivities.Where(x => x.CulturalActivityId == activityId).FirstOrDefaultAsync(x => !x.IsInternal && x.Dni == dni);
            }

            if (result != null)
            {
                return true;
            }

            return false;
        }
    }
}
