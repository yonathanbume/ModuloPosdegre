// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.INTRANET.Areas.Api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AkdemicContext _context;

        public StudentController(
            AkdemicContext context
            )
        {
            _context = context;
        }

        [HttpGet("cantidad-matriculados-sexo")]
        public async Task<IActionResult> GetEnrolledStudentBySex(string termName)
        {
            var query = _context.Students.AsNoTracking();

            if (string.IsNullOrEmpty(termName))
            {
                query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE));
            }
            else
            {
                query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.Term.Name == termName));
            }

            var data = await query
                .GroupBy(x => x.User.Sex)
                .Select(x => new
                {
                    key = ConstantHelpers.SEX.VALUES[x.Key],
                    count = x.Count()
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("cantidad-matriculados")]
        public async Task<IActionResult> GetEnrolledStudent(string termName)
        {
            var query = _context.Students.AsNoTracking();

            if (string.IsNullOrEmpty(termName))
            {
                query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.Term.Status == ConstantHelpers.TERM_STATES.ACTIVE));
            }
            else
            {
                query = query.Where(x => x.StudentSections.Any(y => y.Section.CourseTerm.Term.Name == termName));
            }

            var students = await query.CountAsync();

            return Ok(students);
        }
    }
}
