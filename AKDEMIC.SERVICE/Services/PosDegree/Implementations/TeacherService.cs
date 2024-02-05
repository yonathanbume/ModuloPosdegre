using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Repositories.Generals.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Teacher;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using AKDEMIC.REPOSITORY.Repositories.Scale.Templates.Education;
using AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeacherDedication;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using AKDEMIC.SERVICE.Services.PosDegree.Interfaces;
using AKDEMIC.REPOSITORY.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.SERVICE.Services.PosDegree.Implementations
{
    public  class TeacherService:ITeacherPService
    {
        public readonly ITeacherPRepository _TeacherRepository;
        
        public TeacherService(ITeacherPRepository TeacherRepository) {
        _TeacherRepository= TeacherRepository;
        }

        public async Task DeleteTeacher(Guid id)
        {
            var Teacher = await _TeacherRepository.Get(id);
            await _TeacherRepository.Delete(Teacher);
        }

        public async Task<PosdegreeTeacher> Get(Guid id)
        {
          return  await  _TeacherRepository.Get(id);
        }

        public async Task<object> GetDocenteAllJson()
        {
            return  await _TeacherRepository.GetDocenteAllJson();
        }

        public Task<DataTablesStructs.ReturnedData<object>> GetTeacherDataTable(DataTablesStructs.SentParameters parameters1, string search)
        => _TeacherRepository.GetTeacherDataTable(parameters1,search);

        public async Task Insert(PosdegreeTeacher entity)
        {
            await _TeacherRepository.Insert(entity);
            
        }
    }
}
