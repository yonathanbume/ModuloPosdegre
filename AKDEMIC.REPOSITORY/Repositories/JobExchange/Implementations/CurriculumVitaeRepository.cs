using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.JobExchange.Templates;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.JobExchange.Implementations
{
    public class CurriculumVitaeRepository:Repository<CurriculumVitae> , ICurriculumVitaeRepository
    {
        public CurriculumVitaeRepository(AkdemicContext context) : base(context){ }

        public async Task<CurriculumVitae> GetByStudent(Guid studentId)
        {
            var query = _context.CurriculumVitaes
                .Where(x => x.StudentId == studentId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<CurriculumVitaeTemplate> GetCurriculumVitae(Guid id)
        {
            var result = await _context.CurriculumVitaes
                .Where(x => x.Id == id)
                .Select(x => new CurriculumVitaeTemplate
                {
                    Id = x.Id,
                    StudentId = x.StudentId,
                    Birthday = x.Student.User.BirthDate.ToLocalDateFormat(),
                    Career = x.Student.Career.Name,
                    CivilStatus = ConstantHelpers.CIVIL_STATUS.VALUES.ContainsKey(x.Student.User.CivilStatus) ?
                         ConstantHelpers.CIVIL_STATUS.VALUES[x.Student.User.CivilStatus] : "",
                    Dni = x.Student.User.Dni,
                    DisabilityCertificatePath = x.DisabilityCertificatePath,
                    Name = x.Student.User.Name,
                    PaternalSurName = x.Student.User.PaternalSurname,
                    MaternalSurName = x.Student.User.MaternalSurname,
                    Department = x.Student.User.Department.Name,
                    Province = x.Student.User.Province.Name,
                    District = x.Student.User.District.Name,
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.Student.User.Sex) ?
                        ConstantHelpers.SEX.VALUES[x.Student.User.Sex] : "",
                    PhoneNumber = x.Student.User.PhoneNumber,
                    DriverLicenseCode = x.DriverLicenseCode,
                    DriverLicenseCategory = x.DriverLicenseCategory,
                    TiutionNumber = x.TiutionNumber,
                    TiutionStatus = x.TiutionStatus,
                    Description = x.Description,
                    Linkedin = x.Linkedin,
                    File = x.File
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<CurriculumVitaeTemplate> GetCurriculumVitaeByStudentId(Guid studentId)
        {
            var result = await _context.CurriculumVitaes
                .Where(x => x.StudentId == studentId)
                .Select(x => new CurriculumVitaeTemplate
                {
                    Id = x.Id,
                    StudentId = x.StudentId,
                    Birthday = x.Student.User.BirthDate.ToLocalDateFormat(),
                    Career = x.Student.Career.Name,
                    CivilStatus = ConstantHelpers.CIVIL_STATUS.VALUES.ContainsKey(x.Student.User.CivilStatus) ?
                         ConstantHelpers.CIVIL_STATUS.VALUES[x.Student.User.CivilStatus] : "",
                    Dni = x.Student.User.Dni,
                    DisabilityCertificatePath = x.DisabilityCertificatePath,
                    Name = x.Student.User.Name,
                    PaternalSurName = x.Student.User.PaternalSurname,
                    MaternalSurName = x.Student.User.MaternalSurname,
                    Department = x.Student.User.Department.Name,
                    Province = x.Student.User.Province.Name,
                    District = x.Student.User.District.Name,
                    Sex = ConstantHelpers.SEX.VALUES.ContainsKey(x.Student.User.Sex) ?
                        ConstantHelpers.SEX.VALUES[x.Student.User.Sex] : "",
                    PhoneNumber = x.Student.User.PhoneNumber,
                    DriverLicenseCode = x.DriverLicenseCode,
                    DriverLicenseCategory = x.DriverLicenseCategory,
                    TiutionNumber = x.TiutionNumber,
                    TiutionStatus = x.TiutionStatus,
                    Description = x.Description,
                    Linkedin = x.Linkedin,
                    File = x.File
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
