using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;

namespace AKDEMIC.REPOSITORY.Data.Seeds.DocumentaryProcedure
{
    public class UserDependencySeed
    {
        public static UserDependency[] Seed (AkdemicContext context)
        {
            var dependencies = context.Dependencies.ToList();
            var users = context.Users.ToList();

            var result = new List<UserDependency>()
            {
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Biblioteca").Id, UserId = users.FirstOrDefault(x => x.UserName == "bienestar").Id },
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Biblioteca").Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia").Id },
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Centro Médico").Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia").Id },
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Mesa de Partes").Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia").Id },
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Secretaria General").Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia").Id },

                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Tesorería").Id, UserId = users.FirstOrDefault(x => x.UserName == "tesorero").Id },

                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Biblioteca").Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.biblioteca").Id },

                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Centro Médico").Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.centro.medico").Id },
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Mesa de Partes").Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.mesa.partes").Id },
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Secretaria General").Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.secretaria.general").Id },
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Tesorería").Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.tesoreria").Id },
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Centro Médico").Id, UserId = users.FirstOrDefault(x => x.UserName == "doctor").Id },
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Mesa de Partes").Id, UserId = users.FirstOrDefault(x => x.UserName == "mesa.partes").Id },
                //help desk dependencies
                new UserDependency() { DependencyId = dependencies.FirstOrDefault(x => x.Name == "Oficina").Id, UserId = users.FirstOrDefault(x => x.UserName == "soporte.tecnico").Id }
            };

            return result.ToArray();
        }
    }
}
