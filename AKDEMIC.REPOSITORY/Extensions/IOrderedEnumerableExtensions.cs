using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.REPOSITORY.Extensions
{
    public static class IOrderedEnumerableExtensions
    {
        public static IEnumerable<UserInternalProcedure> SelectUserInternalProcedure(this IOrderedEnumerable<UserInternalProcedure> userInternalProcedures)
        {
            return userInternalProcedures.Select(x => new UserInternalProcedure
            {
                Id = x.Id,
                DependencyId = x.DependencyId,
                InternalProcedureId = x.InternalProcedureId,
                UserId = x.UserId,
                Observation = x.Observation,
                Status = x.Status,
                Duration = x.Duration,
                Dependency = new Dependency
                {
                    Acronym = x.Dependency.Acronym,
                    Name = x.Dependency.Name
                },
                InternalProcedure = new InternalProcedure
                {
                    DependencyId = x.InternalProcedure.DependencyId,
                    DocumentTypeId = x.InternalProcedure.DocumentTypeId,
                    InternalProcedureParentId = x.InternalProcedure.InternalProcedureParentId,
                    UserId = x.InternalProcedure.UserId,
                    Content = x.InternalProcedure.Content,
                    Number = x.InternalProcedure.Number,
                    Pages = x.InternalProcedure.Pages,
                    Priority = x.InternalProcedure.Priority,
                    SearchNode = x.InternalProcedure.SearchNode,
                    SearchTree = x.InternalProcedure.SearchTree,
                    Subject = x.InternalProcedure.Subject,
                    CreatedAt = x.InternalProcedure.CreatedAt,
                    Dependency = new Dependency
                    {
                        Acronym = x.InternalProcedure.Dependency.Acronym,
                        Name = x.InternalProcedure.Dependency.Name
                    },
                    DocumentType = new DocumentType
                    {
                        Code = x.InternalProcedure.DocumentType.Code,
                        Name = x.InternalProcedure.DocumentType.Name
                    },
                    User = new ApplicationUser
                    {
                        MaternalSurname = x.InternalProcedure.User.MaternalSurname,
                        Name = x.InternalProcedure.User.Name,
                        PaternalSurname = x.InternalProcedure.User.PaternalSurname,
                        FullName = x.InternalProcedure.User.FullName
                    },
                    InternalProcedureParent = x.InternalProcedure.InternalProcedureParent != null ? new InternalProcedure
                    {
                        UserId = x.InternalProcedure.InternalProcedureParent.UserId
                    } : null
                },
                User = new ApplicationUser
                {
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    FullName = x.User.FullName
                },
                CreatedAt = x.CreatedAt,
                GeneratedId = x.GeneratedId
            });
        }
    }
}
