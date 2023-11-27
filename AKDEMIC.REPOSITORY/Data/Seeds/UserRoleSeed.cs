using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using Bogus.DataSets;
using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class UserRoleSeed
    {
        public static Tuple<ApplicationUser, string[], string>[] Seed(AkdemicContext context)
        {
            var address = new Address("es_MX");
            var date = new Date("es_MX");
            var name = new Name("es_MX");
            var password = "UnambaOtI.2023#";

            var result = new List<Tuple<ApplicationUser, string[], string>>()
            {
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "admin.admision@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "admin.admision", Dni = "96385274", BirthDate = DateTime.Parse("1990-10-10") },
                    new string[] { ConstantHelpers.ROLES.ADMISSION_ADMIN },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "admin.escalafon@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "admin.escalafon", Dni = "74756756", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.ESCALAFON_ADMIN },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "admin.procesos@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "admin.procesos", Dni = "71252284", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.PROCESSES_ADMIN },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "admin.finanzas@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "admin.finanzas", Dni = "91222284", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.ECONOMIC_MANAGEMENT_ADMIN },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "admin.encuesta@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "admin.encuesta", Dni = "71252285", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.SURVEY_ADMIN },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "admision@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "admision", Dni = "71252286", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.ADMISSION },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "alumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "alumno", Dni = "74804724", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "alumno.idioma@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "alumno.idioma", Dni = "74804725", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.LANGUAGE_STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cmalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "cmalumno", Dni = "74804726", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "dcalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "dcalumno", Dni = "74804727", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "fcalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "fcalumno", Dni = "74804728", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "gsalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "gsalumno", Dni = "74804729", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "gtalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "gtalumno", Dni = "74804730", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "htalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "htalumno", Dni = "74804731", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "mcalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "mcalumno", Dni = "74804732", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "olalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "olalumno", Dni = "74804733", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "pwalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "pwalumno", Dni = "74804734", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "pyalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "pyalumno", Dni = "74804735", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "qsalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "qsalumno", Dni = "74804736", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "smalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "smalumno", Dni = "74804737", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "vhalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "vhalumno", Dni = "74804738", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "vpalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "vpalumno", Dni = "74804739", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "zgalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "zgalumno", Dni = "74804740", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "ztalumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "ztalumno", Dni = "74804741", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.STUDENTS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "asistente.academico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "asistente.academico", Dni = "71654848", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.ACADEMIC_ASSISTANT },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "autoridades@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "autoridad", Dni = "79134554", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.AUTORITY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "bienestar@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "bienestar", Dni = "74756757", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.INSTITUTIONAL_WELFARE, ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "bibliotecario@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "bibliotecario", Dni = "74756757", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.LIBRARIAN },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cajero@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "cajero", Dni = "74577567", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.CASHIER },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cajero.analisisclinico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "cajero.analisisclinico", Dni = "84804731", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.CASHIER, ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cajero.cepu@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "cajero.cepu", Dni = "84804731", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.CASHIER, ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cajero.clinica@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "cajero.clinica", Dni = "84804732", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.CASHIER, ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cajero.editorial@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "cajero.editorial", Dni = "84804733", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.CASHIER, ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cajero.idiomas@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "cajero.idiomas", Dni = "84804731", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.CASHIER, ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cajero.medico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "cajero.medico", Dni = "84804731", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.CASHIER, ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cajero.laboratorio@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "cajero.laboratorio", Dni = "84804731", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.CASHIER, ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cajero.taller@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "cajero.taller", Dni = "84804731", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.CASHIER, ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "centro.costo@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "centro.costo", Dni = "76154147", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Centro de Costo" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "comedor@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "comedor", Dni = "74564567", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Comedor" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "consultas.datos@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "consultas.datos", Dni = "76321832", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Consultas de Datos" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "consultas.matricula@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "consultas.matricula", Dni = "77784871", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Consultas Matricula" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "consultas.utd@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "consultas.utd", Dni = "79515415", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Consultas UTD" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "coordinador.tutorias@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "coordinador.tutorias", Dni = "71253384", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Coordinador de Tutorias" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "decano@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "decano", Dni = "72441656", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Decano" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "departamento.academico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "departamento.academico", Dni = "78499412", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Departamento Académico" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "dependencia@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "dependencia", Dni = "76145124", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "dependencia.biblioteca@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "dependencia.biblioteca", Dni = "76145125", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "dependencia.centro.medico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "dependencia.centro.medico", Dni = "76145126", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "dependencia.mesa.partes@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "dependencia.mesa.partes", Dni = "76145127", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "dependencia.secretaria.general@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "dependencia.secretaria.general", Dni = "76145128", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "dependencia.soportetecnico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "usuario.soportetecnico", Dni = "79999624", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] {ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "dependencia.tesoreria@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "dependencia.tesoreria", Dni = "76145129", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "deporte.cultura@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "deporte.cultura", Dni = "77765465", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Deporte y Cultura" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "direccion@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "direccion", Dni = "72156156", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Dirección" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "director.carrera@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "director.carrera", Dni = "70345984", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Director de Carrera" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "director.escuela@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "director.escuela", Dni = "78784151", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Director de Escuela" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "docente@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "docente", Dni = "79786523", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.TEACHERS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "agdocente@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "agdocente", Dni = "79786524", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.TEACHERS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "apdocente@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "apdocente", Dni = "79786525", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.TEACHERS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "cidocente@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "cidocente", Dni = "79786526", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.TEACHERS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "frdocente@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "frdocente", Dni = "79786527", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.TEACHERS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "jedocente@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "jedocente", Dni = "79786528", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.TEACHERS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "jmdocente@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "jmdocente", Dni = "79786529", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.TEACHERS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "mcdocente@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "mcdocente", Dni = "79786530", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.TEACHERS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "radocente@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "radocente", Dni = "79786531", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.TEACHERS },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "doctor@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "doctor", Dni = "75804790", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Tópico" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "empresa@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "empresa", Dni = "70345984", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Empresa" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "empresa2@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "empresa2", Dni = "70345984", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Empresa" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "empresa3@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "empresa3", Dni = "70345984", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Empresa" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "ingresos.gastos@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "ingresos.gastos", Dni = "71252123", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.INCOMES },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "jefe.registros@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "jefe.registros", Dni = "74747156", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Jefe de Registros" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "mesa.partes@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "mesa.partes", Dni = "74754634", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "nutricionista@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "nutricionista", Dni = "78923485", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Nutrición" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "obstetra@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "obstetricia", Dni = "72398479", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Obstetricia" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "oficina@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "oficina", Dni = "75456486", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Oficina" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "oficina.adquisicion@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "oficina.adquisicion", Dni = "79621554", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Oficina de Adquisiciones" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "oficina.presupuesto@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "oficina.presupuesto", Dni = "75415454", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Oficina de Presupuesto" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "oficina.procesos.seleccion@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "oficina.procesos.seleccion", Dni = "74124456", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Oficina de Procesos de Selección" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "proveedor@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "proveedor", Dni = "74154515", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Proveedor" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "psicologo@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "psicologo", Dni = "71239879", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Psicología" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "registrador@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "registrador", Dni = "72476165", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Registrador" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "secretaria.direcciones.escuelas.profesionales@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "secretaria.direcciones.escuelas.profesionales", Dni = "75745984", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Secretaría de las Direcciones de las Escuelas Profesionales" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "secretaria.vicepresidencias.presidencias.administracion@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "secretaria.vicepresidencias.presidencias.administracion", Dni = "76444541", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Secretaría de las Vicepresidencias, Presidencias y Administración" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "secretaria.servicios.generales@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "secretaria.servicios.generales", Dni = "73164165", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Secretaría de Servicios Generales" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "soporte.usuarios@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "soporte.usuarios", Dni = "77441449", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Soporte de Usuarios" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "soporte.docentes@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "soporte.docentes", Dni = "79684687", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Soporte Docentes" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "superadmin@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "superadmin", Dni = "71252283", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Superadmin" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "supervisor@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "supervisor", Dni = "77454685", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Supervisor", ConstantHelpers.ROLES.DEPENDENCY },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "supervisor.vrac@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "supervisor.vrac", Dni = "75641656", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Supervisor VRAC" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "tesorero@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "tesorero", Dni = "77451546", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Tesoreria", ConstantHelpers.ROLES.DEPENDENCY, ConstantHelpers.ROLES.CASHIER },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "topico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "topico", Dni = "70345984", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Tópico" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "unidad.programacion@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "unidad.programacion", Dni = "71465152", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Unidad de Programación" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "soporte.psicologia@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Female), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.FEMALE, UserName = "soporte.psicologia", Dni = "72253355", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Usuario de Oficina de Soporte" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "prealumno@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "prealumno", Dni = "74804777", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Alumnos Pre Universitario" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "prealumno1@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "prealumno1", Dni = "74866624", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Alumnos Pre Universitario" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "predocente@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "predocente", Dni = "74888524", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Docentes Pre Universitario" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "predocente1@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "predocente1", Dni = "79999624", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Docentes Pre Universitario" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "predocente2@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "predocente2", Dni = "74866100", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Docentes Pre Universitario" },
                    password
                ),
                //HELP DESK USERS
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "soporte.tecnico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "soporte.tecnico", Dni = "79999624", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Técnico de Soporte Técnico" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "admin.soportetecnico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "admin.soportetecnico", Dni = "79999624", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Administrador de Soporte Técnico" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "coordinador.seguimiento@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "coordinador.seguimiento", Dni = "79999699", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] {"Coordinador de Seguimiento" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "encargado.proyeccion@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "encargado.proyeccion", Dni = "79999699", BirthDate = date.Between(DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-30)) },
                    new string[] {"Encargado de Proyección Social" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "encargado.evaluacion@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "encargado.evaluacion", Dni = "79999699", BirthDate = date.Between(DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-30)) },
                    new string[] {"Encargado de Evaluación de Extensión Universitaria" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "almacen@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "almacen", Dni = "79999699", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] {"Almacén" },
                    password
                ),
                     new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "almacen.comedor@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "almacen.comedor", Dni = "79988699", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] {"Almacén Comedor" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "logistica@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "logistica", Dni = "22894726", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Logística" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "direccion.academica@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "direccion.academica", Dni = "61431205", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Dirección General Académica" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "ventanilla.academica@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "ventanilla.academica", Dni = "54851247", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Ventanilla Académica" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "administrador.chat@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "administrador.chat", Dni = "54851247", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { "Administrador del chat" },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "consulta.reportes@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "consulta.reportes", Dni = "54444247", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.REPORT_QUERIES },
                    password
                ),
                new Tuple<ApplicationUser, string[], string>
                (
                    new ApplicationUser { Address = address.StreetAddress(), Email = "coordinador.admin@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(Name.Gender.Male), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), Sex = ConstantHelpers.SEX.MALE, UserName = "coordinador.admin", Dni = "54444247", BirthDate = date.Between(DateTime.Now.AddYears(-60), DateTime.Now.AddYears(-30)) },
                    new string[] { ConstantHelpers.ROLES.TUTORING_COORDINADOR_ADMIN },
                    password
                ),
                //new Tuple<ApplicationUser, string[], string>
                //(
                //    new ApplicationUser { Address = address.StreetAddress(), Email = "gestor.secciones@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), UserName = "gestor.secciones", Dni = "70345984", BirthDate = DateTime.Parse("1990-10-10") },
                //    new string[] { "Gestor de secciones" },
                //    password
                //),
                //new Tuple<ApplicationUser, string[], string>
                //(
                //    new ApplicationUser { Address = address.StreetAddress(), Email = "asistente.academico@enchufate.pe", EmailConfirmed = true, Name = name.FirstName(), MaternalSurname = name.LastName(), PaternalSurname = name.LastName(), UserName = "asistente.academico", Dni = "70345984", BirthDate = DateTime.Parse("1990-10-10") },
                //    new string[] { "Asistente Académico" },
                //    password
                //),
            };

            var result2 = new List<Tuple<ApplicationUser, string[], string>>();

            for (var i = 0; i < result.Count; i++)
            {
                var resultItem = result[i];
                var applicationUser = resultItem.Item1;
                resultItem.Item1.FullName = $"{applicationUser.PaternalSurname} {applicationUser.MaternalSurname}, {applicationUser.Name}";

                result2.Add(resultItem);
            }

            return result2.ToArray();
        }
    }
}
