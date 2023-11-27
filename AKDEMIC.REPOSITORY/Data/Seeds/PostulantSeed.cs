using System;
using System.Collections.Generic;
using System.Linq;
using Bogus.DataSets;
using Bogus;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Admission;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class PostulantSeed
    {
        public static Postulant[] Seed(AkdemicContext context)
        {
            var address = new Address("es_MX");
            var date = new Date("es_MX");
            var internet = new Internet("es_MX");
            var name = new Name("es_MX");
            var random = new Random();
            var randomizer = new Randomizer();

            var admissionTypes = context.AdmissionTypes.ToList();
            var applicationTerms = context.ApplicationTerms.ToList();
            var campuses = context.Campuses.ToList();
            var careers = context.Careers.ToList();
            var countries = context.Countries.ToList();
            var departments = context.Departments.ToList();
            var districts = context.Districts.ToList();
            var provinces = context.Provinces.ToList();
            var result = new List<Postulant>();

            for (var i = 0; i < 1000; i++)
            {
                var gender = random.Next(2);
                var firstName = name.FirstName(gender == 0 ? Name.Gender.Male : Name.Gender.Female);
                var lastName = name.LastName();
                var postulant = new Postulant
                {
                    AdmissionTypeId = admissionTypes[random.Next(admissionTypes.Count)].Id,
                    ApplicationTermId = applicationTerms[random.Next(applicationTerms.Count)].Id,
                    BirthCountryId = countries[random.Next(countries.Count)].Id,
                    BirthDepartmentId = departments[random.Next(departments.Count)].Id,
                    BirthDistrictId = districts[random.Next(districts.Count)].Id,
                    BirthProvinceId = provinces[random.Next(provinces.Count)].Id,
                    CampusId = campuses[random.Next(campuses.Count)].Id,
                    CareerId = careers[random.Next(careers.Count)].Id,
                    DepartmentId = departments[random.Next(departments.Count)].Id,
                    DistrictId = districts[random.Next(districts.Count)].Id,
                    NationalityCountryId = countries[random.Next(countries.Count)].Id,
                    ProvinceId = provinces[random.Next(provinces.Count)].Id,
                    Address = address.StreetAddress(),
                    BirthDate = date.Between(DateTime.Now.AddYears(-28), DateTime.Now.AddYears(-17)),
                    BroadcastMedium = random.Next(1, 4),
                    Document = randomizer.String2(8, "0123456789"),
                    Email = internet.Email(firstName.ToLower(), lastName.ToLower()),
                    MaternalSurname = name.LastName(),
                    Name = firstName,
                    PaternalSurname = lastName,
                    RegisterDate = DateTime.Now.AddDays(-5),
                    Sex = gender == 0 ? ConstantHelpers.SEX.MALE : ConstantHelpers.SEX.FEMALE,
                    SecondaryEducationName = "Black School",
                    SecondaryEducationStarts = date.Between(DateTime.Parse("2005-03-01"), DateTime.Parse("2008-03-15")),
                    SecondaryEducationEnds = date.Between(DateTime.Parse("2009-12-01"), DateTime.Parse("2012-12-15"))
                };

                result.Add(postulant);
            }

            return result.ToArray();
        }
    }
}
