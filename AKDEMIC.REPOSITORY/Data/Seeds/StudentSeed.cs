using System;
using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class StudentSeed
    {
        public static Student[] Seed(AkdemicContext context)
        {
            var careers = context.Careers.ToList();
            var users = context.Users.ToList();
            var admissionType = context.AdmissionTypes.Where(x => x.Abbreviation == "R").ToList();            
            var terms = context.Terms.Where(x => x.Status == 1).ToList();
            var curriculums = context.Curriculums.ToList();

            var result = new List<Student>()
            {
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "alumno").Id, AdmissionTypeId = admissionType[0].Id , AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id},
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "alumno.idioma").Id, AdmissionTypeId = admissionType[0].Id , AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "cmalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "dcalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "fcalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "gsalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "gtalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "htalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "mcalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "olalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "pwalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "pyalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "qsalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "smalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "vhalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "vpalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "zgalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id },
                new Student { CareerId = careers.First().Id, RegisterDate = DateTime.UtcNow, UserId = users.FirstOrDefault(x => x.UserName == "ztalumno").Id, AdmissionTypeId = admissionType[0].Id, AdmissionTermId = terms[0].Id, CurriculumId = curriculums.First(x => x.IsActive).Id }
            };
            return result.ToArray();

            //return new Student[] { };
        }
    }
}
