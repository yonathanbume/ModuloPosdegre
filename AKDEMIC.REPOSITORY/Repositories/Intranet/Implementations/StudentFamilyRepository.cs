using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Implementations
{
    public class StudentFamilyRepository : Repository<StudentFamily>, IStudentFamilyRepository
    {
        public StudentFamilyRepository(AkdemicContext context) : base(context) { }

        public async Task<IEnumerable<StudentFamily>> GetAllStudentFamilysWithData()
        {
            var result = await _context.StudentFamilies.Include(x => x.Student).ThenInclude(x => x.User).ToListAsync();

            return result;
        }

        public async Task<List<StudentFamily>> GetStudentFamilyByStudentId(Guid studentId)
        {
            var result = await _context.StudentFamilies.Where(x => x.StudentId == studentId).ToListAsync();

            return result;
        }

        public async Task<StudentFamily> GetStudentFamilyByUsername(string username)
        {
            var result = await _context.StudentFamilies.Include(x => x.Student).ThenInclude(x => x.User).Where(x => x.Student.User.UserName == username).FirstOrDefaultAsync();

            return result;
        }

        public async Task<object> GetStudentFamilySelectById(Guid id)
        {
            var result = await _context.StudentFamilies.Where(x => x.Id == id)
                .Select(X => new {
                    id = X.Id,
                    name = X.Name,
                    paternalname = X.PaternalName,
                    maternalname = X.MaternalName,
                    birthday = X.Birthday.ToLocalDateFormat(),
                    relationship = X.RelationshipInt,
                    civilstatus = X.CivilStatusInt,
                    degreeinstruction = X.DegreeInstructionInt,
                    certificated = X.Certificated,
                    occupation = X.Occupation,
                    workcenter = X.WorkCenter,
                    location = X.Location,
                    diseaseType = X.DiseaseType,
                    isSick = X.IsSick,
                    surgicalIntervention = X.SurgicalIntervention,
                    X.Community,
                    X.Partiality,
                    X.PopulatedCenter,
                    X.SecondOccupation,
                    X.LivingRuralArea
                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<object> GetStudentFamilyByStudentIdSelect(Guid studentId)
        {
            var result = await _context.StudentFamilies.Where(x => x.StudentId == studentId)
                .Select(X => new
                {
                    id = X.Id,
                    name = X.Name,
                    paternalname = X.PaternalName,
                    maternalname = X.MaternalName,
                    birthday = X.Birthday.ToLocalDateFormat(),
                    relationship = ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.TYPE.Where(x => x.Key == X.RelationshipInt).FirstOrDefault().Value,
                    civilstatus = ConstantHelpers.CIVIL_STATUS.VALUES.Where(x => x.Key == X.CivilStatusInt).FirstOrDefault().Value,
                    degreeinstruction = ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.TYPE.Where(x => x.Key == X.DegreeInstructionInt).FirstOrDefault().Value,
                    certificated = X.Certificated,
                    occupation = X.Occupation,
                    workcenter = X.WorkCenter,
                    location = X.Location,
                    isSick = X.IsSick,
                    diseaseType = X.DiseaseType,
                    surgicalIntervention = X.SurgicalIntervention
                }).ToListAsync();

            return result;
        }

        public async Task<StudentFamily> GetStudentFamilyByStudentInformationInclude(Guid studentInformationId)
        {
            var result = await _context.StudentFamilies.Include(x => x.Student).ThenInclude(x => x.User).Where(x => x.Student.StudentInformationId == studentInformationId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<object> GetStudentFamilySelecByIdInformation(Guid id)
        {
            var result = await _context.StudentFamilies.Where(x => x.Id == id)
                .Select(X => new
                {
                    id = X.Id,
                    name = X.Name,
                    paternalname = X.PaternalName,
                    maternalname = X.MaternalName,
                    birthday = X.Birthday.ToLocalDateFormat(),
                    relationship = X.RelationshipInt,
                    civilstatus = X.CivilStatusInt,
                    degreeinstruction = X.DegreeInstructionInt,
                    certificated = X.Certificated,
                    occupation = X.Occupation,
                    workcenter = X.WorkCenter,
                    location = X.Location,
                    diseaseType = X.DiseaseType,
                    isSick = X.IsSick,
                    surgicalIntervention = X.SurgicalIntervention

                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetAllFamilyMembersFromStudentDatatable(DataTablesStructs.SentParameters sentParameters, Guid studentId)
        {
            Expression<Func<StudentFamily, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.PaternalName); break;
                case "2":
                    orderByPredicate = ((x) => x.MaternalName); break;
            }

            var query = _context.StudentFamilies
                .Where(x => x.StudentId == studentId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    paternalname = x.PaternalName,
                    maternalname = x.MaternalName,
                    birthday = x.Birthday.ToLocalDateFormat(),
                    relationship = ConstantHelpers.STUDENT_FAMILY.RELATIONSHIPS.TYPE[x.RelationshipInt],
                    civilstatus = ConstantHelpers.CIVIL_STATUS.VALUES[x.CivilStatusInt],
                    degreeinstruction = ConstantHelpers.STUDENT_FAMILY.CERTIFICATES.TYPE[x.DegreeInstructionInt],
                    certificated = x.Certificated,
                    occupation = x.Occupation,
                    workcenter = x.WorkCenter,
                    location = x.Location,
                    isSick = x.IsSick,
                    diseaseType = x.DiseaseType,
                    surgicalIntervention = x.SurgicalIntervention
                })
                .ToListAsync();

            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
