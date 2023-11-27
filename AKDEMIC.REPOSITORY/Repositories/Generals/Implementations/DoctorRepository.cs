using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Implementations
{
    //public class DoctorRepository 
    //{
        //public DoctorRepository(AkdemicContext context) : base(context) { }

        //public async Task<IEnumerable<Select2Structs.Result>> GetDoctorsSelect2ClientSide(Guid? specialityId = null, ClaimsPrincipal user = null)
        //{
        //    var query = _context.Doctors.Include(x => x.User).AsNoTracking();

        //    if (specialityId.HasValue)
        //        query = query.Where(x => x.DoctorSpecialtyId == specialityId);

        //    if (user != null)
        //    {
        //        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        query = query.Where(x => x.UserId != userId);
        //    }

        //    var result = await query
        //        .Select(x => new Select2Structs.Result
        //        {
        //            Id = x.Id,
        //            Text = $"{x.DoctorSpecialty.Description} - {x.User.FullName}"
        //        }).ToArrayAsync();

        //    return result;
        //}
    //}
}
