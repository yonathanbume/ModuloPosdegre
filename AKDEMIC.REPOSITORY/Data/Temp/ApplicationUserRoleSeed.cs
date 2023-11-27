using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.REPOSITORY.Data.Temp
{
    public class ApplicationUserRoleSeed2
    {
        public static ApplicationUserRole[] Seed(AkdemicContext context)
        {
            var roles = context.Roles.ToList();
            var users = context.Users.ToList();

            var result = new List<ApplicationUserRole>()
            {
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Superadmin").Id, UserId = users[0].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Administrador de Procesos").Id, UserId = users[1].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Administrador Encuesta").Id, UserId = users[2].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Admisión").Id, UserId = users[3].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[4].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[5].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[6].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[7].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[8].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[9].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[10].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[11].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[12].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[13].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[14].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[15].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[16].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[17].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[18].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[19].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos").Id, UserId = users[20].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Doctores").Id, UserId = users[21].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Autoridades").Id, UserId = users[22].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Consultas de Datos").Id, UserId = users[23].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Consultas Matricula").Id, UserId = users[24].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Consultas UTD").Id, UserId = users[25].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Decano").Id, UserId = users[26].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Departamento Académico").Id, UserId = users[27].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Director de Escuela").Id, UserId = users[28].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Docentes").Id, UserId = users[29].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Docentes").Id, UserId = users[30].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Docentes").Id, UserId = users[31].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Docentes").Id, UserId = users[32].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Docentes").Id, UserId = users[33].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Docentes").Id, UserId = users[34].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Docentes").Id, UserId = users[35].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Docentes").Id, UserId = users[36].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Docentes").Id, UserId = users[37].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Empresa").Id, UserId = users[46].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Jefe de Registros").Id, UserId = users[38].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Registrador").Id, UserId = users[39].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Soporte de Usuarios").Id, UserId = users[40].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Soporte Docentes").Id, UserId = users[41].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Supervisor").Id, UserId = users[42].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Supervisor VRAC").Id, UserId = users[43].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Tesoreria").Id, UserId = users[44].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Administrador de Escalafón").Id, UserId = users[45].Id },
                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Asistente Académico").Id, UserId = users[45].Id },

                new ApplicationUserRole { RoleId = roles.FirstOrDefault(x => x.Name == "Alumnos de idioma").Id, UserId = users[46].Id },

            };

            return result.ToArray();
        }
    }
}
