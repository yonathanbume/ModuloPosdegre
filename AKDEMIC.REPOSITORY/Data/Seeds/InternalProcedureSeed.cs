using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class InternalProcedureSeed
    {
        public static InternalProcedure[] Seed(AkdemicContext context)
        {
            var dependencies = context.Dependencies.ToList();
            var documentTypes = context.DocumentTypes.ToList();
            var users = context.Users.ToList();

            var result = new List<InternalProcedure>()
            {
                new InternalProcedure { DependencyId = dependencies[0].Id, DocumentTypeId = documentTypes[0].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia").Id, Content = "Contenido 1", Number = 1, Pages = 1, Priority = 1, SearchNode = 0, SearchTree = 0, Subject = "Asunto 1", CreatedAt = DateTime.UtcNow },
                new InternalProcedure { DependencyId = dependencies[1].Id, DocumentTypeId = documentTypes[0].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia").Id, Content = "Contenido 2", Number = 1, Pages = 1, Priority = 1, SearchNode = 0, SearchTree = 1, Subject = "Asunto 2", CreatedAt = DateTime.UtcNow },
                new InternalProcedure { DependencyId = dependencies[2].Id, DocumentTypeId = documentTypes[0].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia").Id, Content = "Contenido 3", Number = 1, Pages = 1, Priority = 1, SearchNode = 0, SearchTree = 2, Subject = "Asunto 3", CreatedAt = DateTime.UtcNow },
                new InternalProcedure { DependencyId = dependencies[3].Id, DocumentTypeId = documentTypes[0].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia").Id, Content = "Contenido 4", Number = 1, Pages = 1, Priority = 1, SearchNode = 0, SearchTree = 3, Subject = "Asunto 4", CreatedAt = DateTime.UtcNow },
                new InternalProcedure { DependencyId = dependencies[4].Id, DocumentTypeId = documentTypes[0].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia").Id, Content = "Contenido 5", Number = 1, Pages = 1, Priority = 1, SearchNode = 0, SearchTree = 4, Subject = "Asunto 5", CreatedAt = DateTime.UtcNow },
                new InternalProcedure { DependencyId = dependencies[0].Id, DocumentTypeId = documentTypes[1].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.biblioteca").Id, Content = "Contenido 6",  Number = 2, Pages = 1, Priority = 1, SearchNode = 0, SearchTree = 5, Subject = "Asunto 6", CreatedAt = DateTime.UtcNow },
                new InternalProcedure { DependencyId = dependencies[1].Id, DocumentTypeId = documentTypes[0].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.centro.medico").Id, Content = "Contenido 7",  Number = 2, Pages = 1, Priority = 1, SearchNode = 0, SearchTree = 6, Subject = "Asunto 7", CreatedAt = DateTime.UtcNow },
                new InternalProcedure { DependencyId = dependencies[2].Id, DocumentTypeId = documentTypes[1].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.mesa.partes").Id, Content = "Contenido 8",  Number = 2, Pages = 1, Priority = 1, SearchNode = 0, SearchTree = 7, Subject = "Asunto 8", CreatedAt = DateTime.UtcNow },
                new InternalProcedure { DependencyId = dependencies[3].Id, DocumentTypeId = documentTypes[0].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.secretaria.general").Id, Content = "Contenido 9",  Number = 2, Pages = 1, Priority = 1, SearchNode = 0, SearchTree = 8, Subject = "Asunto 9", CreatedAt = DateTime.UtcNow },
                new InternalProcedure { DependencyId = dependencies[4].Id, DocumentTypeId = documentTypes[1].Id, UserId = users.FirstOrDefault(x => x.UserName == "dependencia.tesoreria").Id, Content = "Contenido 10",  Number = 2, Pages = 1, Priority = 1, SearchNode = 0, SearchTree = 9, Subject = "Asunto 10", CreatedAt = DateTime.UtcNow }
            };

            return result.ToArray();
        }
    }
}
