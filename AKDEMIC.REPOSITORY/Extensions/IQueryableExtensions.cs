using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.REPOSITORY.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<ApplicationUser> SelectApplicationUser(this IQueryable<ApplicationUser> applicationUser)
        {
            return applicationUser.Select(x => new ApplicationUser
            {
                Id = x.Id,
                Address = x.Address,
                BirthDate = x.BirthDate,
                Campaigns = x.Campaigns,
                Connections = x.Connections,
                Dni = x.Dni,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                //FavoriteCompanies = x.FavoriteCompanies,
                IsActive = x.IsActive,
                MaternalSurname = x.MaternalSurname,
                Name = x.Name,
                PaternalSurname = x.PaternalSurname,
                PhoneNumber = x.PhoneNumber,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                Picture = x.Picture,
                Sex = x.Sex,
                UserName = x.UserName,
                CreatedAt = x.CreatedAt,
                DeletedAt = x.DeletedAt,
                UpdatedAt = x.UpdatedAt
            });
        }

        public static IQueryable<Classifier> SelectClassifier(this IQueryable<Classifier> classifiers)
        {
            return classifiers.Select(x => new Classifier
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Code = x.Code,
                Description = x.Description,
                Name = x.Name,
                Parent = x.Parent != null ? new Classifier
                {
                    Code = x.Parent.Code,
                    Description = x.Parent.Description,
                    Name = x.Parent.Name
                } : null
            });
        }

        public static IQueryable<Dependency> SelectDependency(this IQueryable<Dependency> dependencies)
        {
            return dependencies.Select(x => new Dependency
            {
                Id = x.Id,
                UserId = x.UserId,
                Name = x.Name,
                Acronym = x.Acronym,
                Signature = x.Signature,
                User = x.User != null ? new ApplicationUser
                {
                    Dni = x.User.Dni,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    Sex = x.User.Sex
                } : null
            });
        }

        public static IQueryable<Dependency> SelectDependency(this IQueryable<Dependency> dependencies, AkdemicContext context, Guid procedureId)
        {
            return dependencies.Select(x => new Dependency
            {
                Id = x.Id,
                UserId = x.UserId,
                Name = x.Name,
                Acronym = x.Acronym,
                Signature = x.Signature,
                IsProcedureDependency = context.ProcedureDependencies.Any(y => y.ProcedureId == procedureId && y.DependencyId == x.Id),
                User = x.User != null ? new ApplicationUser
                {
                    Dni = x.User.Dni,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    Sex = x.User.Sex
                } : null
            });
        }

        public static IQueryable<DocumentType> SelectDocumentType(this IQueryable<DocumentType> documentTypes)
        {
            return documentTypes.Select(x => new DocumentType
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            });
        }

        public static IQueryable<DocumentaryRecordType> SelectDocumentaryRecordType(this IQueryable<DocumentaryRecordType> documentaryRecordTypes)
        {
            return documentaryRecordTypes.Select(x => new DocumentaryRecordType
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            });
        }

        public static IQueryable<EnrollmentReservation> SelectEnrollmentReservation(this IQueryable<EnrollmentReservation> enrollmentReservations)
        {
            return enrollmentReservations.Select(x => new EnrollmentReservation
            {
                Id = x.Id,
                UserProcedureId = x.UserProcedureId,
                UserProcedure = new UserProcedure
                {
                    UserId = x.UserProcedure.UserId,
                    ProcedureId = x.UserProcedure.ProcedureId,
                    Status = x.UserProcedure.Status,
                    User = new ApplicationUser
                    {
                        MaternalSurname = x.UserProcedure.User.MaternalSurname,
                        Name = x.UserProcedure.User.Name,
                        PaternalSurname = x.UserProcedure.User.PaternalSurname,
                        FullName = x.UserProcedure.User.FullName
                    },
                    CreatedAt = x.UserProcedure.CreatedAt
                }
            });
        }

        public static IQueryable<ExternalProcedure> SelectExternalProcedure(this IQueryable<ExternalProcedure> externalProcedures)
        {
            return externalProcedures.Select(x => new ExternalProcedure
            {
                Id = x.Id,
                ClassifierId = x.ClassifierId,
                DependencyId = x.DependencyId,
                Code = x.Code,
                Comment = x.Comment,
                Cost = x.Cost,
                IsTransparency = x.IsTransparency,
                Name = x.Name,
                ConceptId = x.ConceptId,
                Concept = x.Concept != null ? new Concept
                {
                    Amount = x.Concept.Amount,
                    Description = x.Concept.Description
                } : null,
                StaticType = x.StaticType,
                Classifier = x.Classifier != null ? new Classifier
                {
                    Code = x.Classifier.Code,
                    Description = x.Classifier.Description,
                    Name = x.Classifier.Name
                } : null,
                Dependency = new Dependency
                {
                    Acronym = x.Dependency.Acronym,
                    Name = x.Dependency.Name
                },
                CreatedAt = x.CreatedAt
            });
        }

        public static IQueryable<ExternalUser> SelectExternalUser(this IQueryable<ExternalUser> externalUser)
        {
            return externalUser.Select(x => new ExternalUser
            {
                Id = x.Id,
                BirthDate = x.BirthDate,
                DocumentNumber = x.DocumentNumber,
                Name = x.Name,
                MaternalSurname = x.MaternalSurname,
                PaternalSurname = x.PaternalSurname,
                PhoneNumber = x.PhoneNumber
            });
        }

        public static IQueryable<InternalProcedure> SelectInternalProcedure(this IQueryable<InternalProcedure> internalProcedures)
        {
            return internalProcedures.Select(x => new InternalProcedure
            {
                Id = x.Id,
                DependencyId = x.DependencyId,
                DocumentTypeId = x.DocumentTypeId,
                InternalProcedureParentId = x.InternalProcedureParentId,
                UserId = x.UserId,
                Content = x.Content,
                Number = x.Number,
                Pages = x.Pages,
                Priority = x.Priority,
                SearchNode = x.SearchNode,
                SearchTree = x.SearchTree,
                Code = x.Code,
                Subject = x.Subject,
                Dependency = new Dependency
                {
                    Acronym = x.Dependency.Acronym,
                    Name = x.Dependency.Name
                },
                DocumentType = new DocumentType
                {
                    Code = x.DocumentType.Code,
                    Name = x.DocumentType.Name
                },
                InternalProcedureParent = x.InternalProcedureParentId != null ? new InternalProcedure
                {
                    DependencyId = x.InternalProcedureParent.DependencyId,
                    DocumentTypeId = x.InternalProcedureParent.DocumentTypeId,
                    InternalProcedureParentId = x.InternalProcedureParent.InternalProcedureParentId,
                    UserId = x.InternalProcedureParent.UserId,
                    Content = x.InternalProcedureParent.Content,
                    Number = x.InternalProcedureParent.Number,
                    Pages = x.InternalProcedureParent.Pages,
                    Priority = x.InternalProcedureParent.Priority,
                    Subject = x.InternalProcedureParent.Subject,
                    Dependency = new Dependency
                    {
                        Id = x.InternalProcedureParent.Dependency.Id,
                        Acronym = x.InternalProcedureParent.Dependency.Acronym,
                        Name = x.InternalProcedureParent.Dependency.Name
                    },
                } : null,
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

        public static IQueryable<InternalProcedure> SelectInternalProcedure(this IQueryable<InternalProcedure> internalProcedures, AkdemicContext context)
        {
            return internalProcedures.Select(x => new InternalProcedure
            {
                Id = x.Id,
                DependencyId = x.DependencyId,
                DocumentTypeId = x.DocumentTypeId,
                InternalProcedureParentId = x.InternalProcedureParentId,
                UserId = x.UserId,
                Content = x.Content,
                Code = x.Code,
                Number = x.Number,
                Pages = x.Pages,
                Priority = x.Priority,
                SearchNode = x.SearchNode,
                SearchTree = x.SearchTree,
                Subject = x.Subject,
                HasFiles = context.InternalProcedureFiles.Any(y => y.InternalProcedureId == x.Id),
                HasReferences = context.InternalProcedureReferences.Any(y => y.InternalProcedureId == x.Id),
                Dependency = new Dependency
                {
                    Acronym = x.Dependency.Acronym,
                    Name = x.Dependency.Name
                },
                DocumentType = new DocumentType
                {
                    Code = x.DocumentType.Code,
                    Name = x.DocumentType.Name
                },
                InternalProcedureParent = x.InternalProcedureParentId != null ? new InternalProcedure
                {
                    DependencyId = x.InternalProcedureParent.DependencyId,
                    DocumentTypeId = x.InternalProcedureParent.DocumentTypeId,
                    InternalProcedureParentId = x.InternalProcedureParent.InternalProcedureParentId,
                    UserId = x.InternalProcedureParent.UserId,
                    Content = x.InternalProcedureParent.Content,
                    Number = x.InternalProcedureParent.Number,
                    Pages = x.InternalProcedureParent.Pages,
                    Priority = x.InternalProcedureParent.Priority,
                    Subject = x.InternalProcedureParent.Subject,
                    Dependency = new Dependency
                    {
                        Acronym = x.InternalProcedureParent.Dependency.Acronym,
                        Name = x.InternalProcedureParent.Dependency.Name
                    },
                    DocumentType = new DocumentType
                    {
                        Code = x.InternalProcedureParent.DocumentType.Code,
                        Name = x.InternalProcedureParent.DocumentType.Name
                    },
                    User = new ApplicationUser
                    {
                        MaternalSurname = x.InternalProcedureParent.User.MaternalSurname,
                        Name = x.InternalProcedureParent.User.Name,
                        PaternalSurname = x.InternalProcedureParent.User.PaternalSurname,
                        FullName = x.InternalProcedureParent.User.FullName
                    }
                } : null,
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

        public static IQueryable<InternalProcedureFile> SelectInternalProcedureFile(this IQueryable<InternalProcedureFile> internalProcedureFile)
        {
            return internalProcedureFile.Select(x => new InternalProcedureFile
            {
                Id = x.Id,
                FileName = x.FileName,
                Path = x.Path,
                Size = x.Size
            });
        }

        public static IQueryable<InternalProcedureReference> SelectInternalProcedureReference(this IQueryable<InternalProcedureReference> internalProcedureReference)
        {
            return internalProcedureReference.Select(x => new InternalProcedureReference
            {
                Id = x.Id,
                Reference = x.Reference
            });
        }

        public static IQueryable<Procedure> SelectProcedure(this IQueryable<Procedure> procedures)
        {
            return procedures.Select(x => new Procedure
            {
                Id = x.Id,
                ClassifierId = x.ClassifierId,
                ConceptId = x.ConceptId,
                HasPicture = x.HasPicture,
                ProcedureCategoryId = x.ProcedureCategoryId,
                ProcedureSubcategoryId = x.ProcedureSubcategoryId,
                Code = x.Code,
                Duration = x.Duration,
                LegalBase = x.LegalBase,
                Name = x.Name,
                Description = x.Description,
                MaximumRequestByTerm = x.MaximumRequestByTerm,
                Type = x.Type,
                DependencyId = x.DependencyId,
                Enabled = x.Enabled,
                EnabledEndDate = x.EnabledEndDate,
                EnabledStartDate = x.EnabledStartDate,
                Dependency = x.Dependency != null ? new Dependency
                {
                    Id = x.Dependency.Id,
                    Name = x.Dependency.Name
                } : null,
                StartDependencyId = x.StartDependencyId,
                StartDependency = x.StartDependency != null ? new Dependency
                {
                    Id = x.StartDependency.Id,
                    Name = x.StartDependency.Name
                } : null,
                Score = x.Score,
                StaticType = x.StaticType,
                ProcedureRequirementsCostSum = x.ConceptId.HasValue ? x.Concept.Amount : x.ProcedureRequirements.Sum(y => y.Cost),
                Concept = x.Concept != null ? new Concept
                {
                    Code = x.Concept.Code,
                    Description = x.Concept.Description,
                    Amount = x.Concept.Amount
                } : null,
                Classifier = x.Classifier != null ? new Classifier
                {
                    Code = x.Classifier.Code,
                    Description = x.Classifier.Description,
                    Name = x.Classifier.Name
                } : null,
                ProcedureCategory = (x.ProcedureCategory != null ? new ProcedureCategory
                {
                    Name = x.ProcedureCategory.Name
                } : null),
                ProcedureSubcategory = (x.ProcedureSubcategory != null ? new ProcedureSubcategory
                {
                    Name = x.ProcedureSubcategory.Name
                } : null)
            });
        }

        public static IQueryable<ProcedureDependency> SelectProcedureDependency(this IQueryable<ProcedureDependency> procedureDependencies)
        {
            return procedureDependencies.Select(x => new ProcedureDependency
            {
                Id = x.Id,
                DependencyId = x.DependencyId,
                ProcedureId = x.ProcedureId,
                Dependency = new Dependency
                {
                    Name = x.Dependency.Name
                },
                Procedure = new Procedure
                {
                    Name = x.Procedure.Name
                }
            });
        }

        public static IQueryable<ProcedureRequirement> SelectProcedureRequirement(this IQueryable<ProcedureRequirement> procedureRequirements)
        {
            return procedureRequirements.Select(x => new ProcedureRequirement
            {
                Id = x.Id,
                ProcedureId = x.ProcedureId,
                Code = x.Code,
                Cost = x.Cost,
                Name = x.Name,
                SystemValidationType = x.SystemValidationType,
                Type = x.Type,
                SystemvalidationTypeStr = x.SystemValidationType.HasValue ? ConstantHelpers.PROCEDURE_REQUIREMENTS.SYSTEM_VALIDATION_TYPE.VALUES[x.SystemValidationType.Value] : string.Empty,
                Procedure = new Procedure
                {
                    Name = x.Procedure.Name
                }
            });
        }

        public static IQueryable<ProcedureResolution> SelectProcedureResolution(this IQueryable<ProcedureResolution> procedureResolutions)
        {
            return procedureResolutions.Select(x => new ProcedureResolution
            {
                Id = x.Id,
                DependencyId = x.DependencyId,
                ProcedureId = x.ProcedureId,
                PresentationTerm = x.PresentationTerm,
                ResolutionTerm = x.ResolutionTerm,
                ResolutionType = x.ResolutionType,
                Dependency = new Dependency
                {
                    Name = x.Dependency.Name
                },
                Procedure = new Procedure
                {
                    Name = x.Procedure.Name
                }
            });
        }

        public static IQueryable<ProcedureRole> SelectProcedureRole(this IQueryable<ProcedureRole> procedureRoles)
        {
            return procedureRoles.Select(x => new ProcedureRole
            {
                Id = x.Id,
                ProcedureId = x.ProcedureId,
                RoleId = x.RoleId,
                Procedure = new Procedure
                {
                    Name = x.Procedure.Name
                },
                Role = new ApplicationRole
                {
                    Name = x.Procedure.Name
                }
            });
        }

        public static IQueryable<ProcedureCategory> SelectProcedureCategory(this IQueryable<ProcedureCategory> procedureCategories)
        {
            return procedureCategories.Select(x => new ProcedureCategory
            {
                Id = x.Id,
                Name = x.Name,
                StaticType = x.StaticType
            });
        }

        public static IQueryable<ProcedureSubcategory> SelectProcedureSubcategory(this IQueryable<ProcedureSubcategory> procedureSubcategories)
        {
            return procedureSubcategories.Select(x => new ProcedureSubcategory
            {
                Id = x.Id,
                ProcedureCategoryId = x.ProcedureCategoryId,
                Name = x.Name,
                ProcedureCategory = new ProcedureCategory
                {
                    Name = x.ProcedureCategory.Name
                }
            });
        }

        public static IQueryable<RecordSubjectType> SelectRecordSubjectType(this IQueryable<RecordSubjectType> recordSubjectTypes)
        {
            return recordSubjectTypes.Select(x => new RecordSubjectType
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            });
        }

        public static IQueryable<UIT> SelectUIT(this IQueryable<UIT> uits)
        {
            return uits.Select(x => new UIT
            {
                Id = x.Id,
                Value = x.Value,
                Year = x.Year
            });
        }

        public static IQueryable<UserDependency> SelectUserDependency(this IQueryable<UserDependency> userDependencies)
        {
            return userDependencies.Select(x => new UserDependency
            {
                Id = x.Id,
                DependencyId = x.DependencyId,
                UserId = x.UserId,
                Dependency = new Dependency
                {
                    Id = x.Dependency.Id,
                    Acronym = x.Dependency.Acronym,
                    Name = x.Dependency.Name,
                    Signature = x.Dependency.Signature
                },
                User = new ApplicationUser
                {
                    Dni = x.User.Dni,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    Sex = x.User.Sex,
                    FullName = x.User.FullName
                }
            });
        }

        public static IQueryable<UserExternalProcedure> SelectUserExternalProcedure(this IQueryable<UserExternalProcedure> userExternalProcedures)
        {
            return userExternalProcedures.Select(x => new UserExternalProcedure
            {
                Id = x.Id,
                DependencyId = x.DependencyId,
                ExternalProcedureId = x.ExternalProcedureId,
                ExternalUserId = x.ExternalUserId,
                InternalProcedureId = x.InternalProcedureId,
                PaymentId = x.PaymentId,
                TermId = x.TermId,
                Number = x.Number,
                Observation = x.Observation,
                Status = x.Status,
                Dependency = new Dependency
                {
                    Acronym = x.Dependency.Acronym,
                    Name = x.Dependency.Name
                },
                ExternalProcedure = new ExternalProcedure
                {
                    Id = x.ExternalProcedure.Id,
                    ClassifierId = x.ExternalProcedure.ClassifierId,
                    DependencyId = x.ExternalProcedure.DependencyId,
                    Code = x.ExternalProcedure.Code,
                    Cost = x.ExternalProcedure.Cost,
                    IsTransparency = x.ExternalProcedure.IsTransparency,
                    Name = x.ExternalProcedure.Name,
                    StaticType = x.ExternalProcedure.StaticType,
                    Classifier = x.ExternalProcedure.Classifier != null ? new Classifier
                    {
                        Code = x.ExternalProcedure.Classifier.Code,
                        Description = x.ExternalProcedure.Classifier.Description,
                        Name = x.ExternalProcedure.Classifier.Name
                    } : null,
                    Dependency = new Dependency
                    {
                        Acronym = x.ExternalProcedure.Dependency.Acronym,
                        Name = x.ExternalProcedure.Dependency.Name
                    },
                    CreatedAt = x.ExternalProcedure.CreatedAt,
                    
                },
                ExternalUser = new ExternalUser
                {
                    DocumentNumber = x.ExternalUser.DocumentNumber,
                    MaternalSurname = x.ExternalUser.MaternalSurname,
                    Name = x.ExternalUser.Name,
                    PaternalSurname = x.ExternalUser.PaternalSurname,
                    Address = x.ExternalUser.Address,
                    PhoneNumber = x.ExternalUser.PhoneNumber,
                    PhoneNumberOptional = x.ExternalUser.PhoneNumberOptional,
                    DocumentType = x.ExternalUser.DocumentType,
                    Email = x.ExternalUser.Email,
                    BirthDate = x.ExternalUser.BirthDate,
                    FullName = x.ExternalUser.FullName
                    
                },
                InternalProcedure = x.InternalProcedure != null ? new InternalProcedure
                {
                    Content = x.InternalProcedure.Content,
                    IsTransparency = x.InternalProcedure.IsTransparency,
                    Number = x.InternalProcedure.Number,
                    Pages = x.InternalProcedure.Pages,
                    Priority = x.InternalProcedure.Priority,
                    Subject = x.InternalProcedure.Subject,
                    CreatedAt = x.CreatedAt,
                    
                } : null,
                Payment = x.Payment != null ? new Payment
                {
                    Discount = x.Payment.Discount,
                    IgvAmount = x.Payment.IgvAmount,
                    Quantity = x.Payment.Quantity,
                    SubTotal = x.Payment.SubTotal,
                    Status = x.Payment.Status,
                    Total = x.Payment.Total,
                    OperationCodeB = x.Payment.OperationCodeB,
                    PaymentDate = x.Payment.PaymentDate
                } : null,
                Term = x.Term != null ? new Term
                {
                    Name = x.Term.Name
                } : null,
                GeneratedId = x.GeneratedId,
                CreatedAt = x.CreatedAt
            });
        }

        public static IQueryable<UserExternalProcedureRecord> SelectUserExternalProcedureRecord(this IQueryable<UserExternalProcedureRecord> userExternalProcedureRecords)
        {
            return userExternalProcedureRecords.Select(x => new UserExternalProcedureRecord
            {
                Id = x.Id,
                DocumentTypeId = x.DocumentTypeId,
                DocumentaryRecordTypeId = x.DocumentaryRecordTypeId,
                RecordSubjectTypeId = x.RecordSubjectTypeId,
                UserExternalProcedureId = x.UserExternalProcedureId,
                EntryDate = x.EntryDate,
                FullRecordNumber = x.FullRecordNumber,
                Pages = x.Pages,
                RecordNumber = x.RecordNumber,
                Subject = x.Subject,
                DocumentaryRecordType = new DocumentaryRecordType
                {
                    Code = x.DocumentaryRecordType.Code,
                    Name = x.DocumentaryRecordType.Name
                },
                DocumentType = x.DocumentType != null ? new DocumentType
                {
                    Code = x.DocumentType.Code,
                    Name = x.DocumentType.Name
                } : null,
                RecordSubjectType = new RecordSubjectType
                {
                    Code = x.RecordSubjectType.Code,
                    Name = x.RecordSubjectType.Name
                },
                UserExternalProcedure = new UserExternalProcedure
                {
                    DependencyId = x.UserExternalProcedure.DependencyId,
                    ExternalProcedureId = x.UserExternalProcedure.ExternalProcedureId,
                    ExternalUserId = x.UserExternalProcedure.ExternalUserId,
                    InternalProcedureId = x.UserExternalProcedure.InternalProcedureId,
                    PaymentId = x.UserExternalProcedure.PaymentId,
                    TermId = x.UserExternalProcedure.TermId,
                    Number = x.UserExternalProcedure.Number,
                    Observation = x.UserExternalProcedure.Observation,
                    Status = x.UserExternalProcedure.Status,
                    Dependency = x.UserExternalProcedure.Dependency != null ? new Dependency
                    {
                        Acronym = x.UserExternalProcedure.Dependency.Acronym,
                        Name = x.UserExternalProcedure.Dependency.Name,
                        Signature = x.UserExternalProcedure.Dependency.Signature
                    } : null,
                    ExternalProcedure = new ExternalProcedure
                    {
                        ClassifierId = x.UserExternalProcedure.ExternalProcedure.ClassifierId,
                        DependencyId = x.UserExternalProcedure.ExternalProcedure.DependencyId,
                        Code = x.UserExternalProcedure.ExternalProcedure.Code,
                        Comment = x.UserExternalProcedure.ExternalProcedure.Comment,
                        Cost = x.UserExternalProcedure.ExternalProcedure.Cost,
                        IsTransparency = x.UserExternalProcedure.ExternalProcedure.IsTransparency,
                        Name = x.UserExternalProcedure.ExternalProcedure.Name,
                        StaticType = x.UserExternalProcedure.ExternalProcedure.StaticType,
                        Classifier = x.UserExternalProcedure.ExternalProcedure.Classifier != null ? new Classifier
                        {
                            Code = x.UserExternalProcedure.ExternalProcedure.Classifier.Code,
                            Description = x.UserExternalProcedure.ExternalProcedure.Classifier.Description,
                            Name = x.UserExternalProcedure.ExternalProcedure.Classifier.Name
                        } : null,
                        Dependency = new Dependency
                        {
                            Id = x.UserExternalProcedure.ExternalProcedure.Dependency.Id,
                            Name = x.UserExternalProcedure.ExternalProcedure.Dependency.Name
                        },
                        CreatedAt = x.UserExternalProcedure.ExternalProcedure.CreatedAt
                    },
                    ExternalUser = new ExternalUser
                    {
                        DocumentNumber = x.UserExternalProcedure.ExternalUser.DocumentNumber,
                        MaternalSurname = x.UserExternalProcedure.ExternalUser.MaternalSurname,
                        Name = x.UserExternalProcedure.ExternalUser.Name,
                        PaternalSurname = x.UserExternalProcedure.ExternalUser.PaternalSurname,
                        FullName = x.UserExternalProcedure.ExternalUser.FullName
                    },
                    InternalProcedure = x.UserExternalProcedure.InternalProcedure != null ? new InternalProcedure
                    {
                        Content = x.UserExternalProcedure.InternalProcedure.Content,
                        IsTransparency = x.UserExternalProcedure.InternalProcedure.IsTransparency,
                        Number = x.UserExternalProcedure.InternalProcedure.Number,
                        Pages = x.UserExternalProcedure.InternalProcedure.Pages,
                        Priority = x.UserExternalProcedure.InternalProcedure.Priority,
                        Subject = x.UserExternalProcedure.InternalProcedure.Subject,
                        CreatedAt = x.UserExternalProcedure.CreatedAt
                    } : null,
                    Payment = x.UserExternalProcedure.Payment != null ? new Payment
                    {
                        Discount = x.UserExternalProcedure.Payment.Discount,
                        IgvAmount = x.UserExternalProcedure.Payment.IgvAmount,
                        Quantity = x.UserExternalProcedure.Payment.Quantity,
                        SubTotal = x.UserExternalProcedure.Payment.SubTotal,
                        Status = x.UserExternalProcedure.Payment.Status,
                        Total = x.UserExternalProcedure.Payment.Total
                    } : null,
                    Term = x.UserExternalProcedure.Term != null ? new Term
                    {
                        Name = x.UserExternalProcedure.Term.Name
                    } : null,
                    GeneratedId = x.UserExternalProcedure.GeneratedId
                },
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });
        }

        public static IQueryable<UserExternalProcedureRecordDocument> SelectUserExternalProcedureRecordDocument(this IQueryable<UserExternalProcedureRecordDocument> userExternalProcedureRecordDocuments)
        {
            return userExternalProcedureRecordDocuments.Select(x => new UserExternalProcedureRecordDocument
            {
                Id = x.Id,
                UserExternalProcedureRecordId = x.UserExternalProcedureRecordId,
                DocumentBytesSize = x.DocumentBytesSize,
                DocumentUrl = x.DocumentUrl,
                Name = x.Name,
                UserExternalProcedureRecord = new UserExternalProcedureRecord
                {
                    EntryDate = x.UserExternalProcedureRecord.EntryDate,
                    FullRecordNumber = x.UserExternalProcedureRecord.FullRecordNumber,
                    Pages = x.UserExternalProcedureRecord.Pages,
                    RecordNumber = x.UserExternalProcedureRecord.RecordNumber,
                    Subject = x.UserExternalProcedureRecord.Subject
                },
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });
        }

        public static IQueryable<UserInternalProcedure> SelectUserInternalProcedure(this IQueryable<UserInternalProcedure> userInternalProcedures)
        {
            return userInternalProcedures.Select(x => new UserInternalProcedure
            {
                Id = x.Id,
                DependencyId = x.DependencyId,
                InternalProcedureId = x.InternalProcedureId,
                UserId = x.UserId,
                IsDerived = x.IsDerived,
                Observation = x.Observation,
                Status = x.Status,
                Duration = x.Duration,
                FinishAt = x.FinishAt,
                UserExternalProcedureCode = x.InternalProcedure.UserExternalProcedures.Select(y=>y.UserExternalProcedureRecords.Select(z=>z.FullRecordNumber).FirstOrDefault()).FirstOrDefault(),
                ParsedFinishAt = x.FinishAt.ToLocalDateFormat(),
                
                Dependency = new Dependency
                {
                    Acronym = x.Dependency.Acronym,
                    Name = x.Dependency.Name
                },
                InternalProcedure = new InternalProcedure
                {
                    Code = x.InternalProcedure.Code ?? x.InternalProcedure.LegacyCode,
                    DependencyId = x.InternalProcedure.DependencyId,
                    DocumentTypeId = x.InternalProcedure.DocumentTypeId,
                    InternalProcedureParentId = x.InternalProcedure.InternalProcedureParentId,
                    UserId = x.InternalProcedure.UserId,
                    Content = x.InternalProcedure.Content,
                    Number = x.InternalProcedure.Number,
                    IsTransparency = x.InternalProcedure.IsTransparency,
                    Pages = x.InternalProcedure.Pages,
                    Priority = x.InternalProcedure.Priority,
                    SearchNode = x.InternalProcedure.SearchNode,
                    SearchTree = x.InternalProcedure.SearchTree,
                    Subject = x.InternalProcedure.Subject,
                    CreatedAt = x.InternalProcedure.CreatedAt,
                    GeneratedId = x.InternalProcedure.GeneratedId,
                    FromExternal = x.InternalProcedure.FromExternal,
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
                    FullName = string.IsNullOrEmpty(x.User.FullName) ? $"{x.User.PaternalSurname} {x.User.MaternalSurname} {x.User.Name}" : x.User.FullName,
                },
                DependencyParentId = x.DependencyParentId,
                CreatedAt = x.CreatedAt,
                GeneratedId = x.GeneratedId,
                UpdatedAt = x.UpdatedAt
            });
        }

        public static IQueryable<UserInternalProcedure> SelectUserInternalProcedure(this IQueryable<IGrouping<dynamic, UserInternalProcedure>> userInternalProcedures)
        {
            return userInternalProcedures.Select(x => x
                .Select(y => new UserInternalProcedure
                {
                    Id = y.Id,
                    DependencyId = y.DependencyId,
                    InternalProcedureId = y.InternalProcedureId,
                    UserId = y.UserId,
                    IsDerived = y.IsDerived,
                    Observation = y.Observation,
                    Status = y.Status,
                    Dependency = new Dependency
                    {
                        Acronym = y.Dependency.Acronym,
                        Name = y.Dependency.Name
                    },
                    InternalProcedure = new InternalProcedure
                    {
                        DependencyId = y.InternalProcedure.DependencyId,
                        DocumentTypeId = y.InternalProcedure.DocumentTypeId,
                        InternalProcedureParentId = y.InternalProcedure.InternalProcedureParentId,
                        UserId = y.InternalProcedure.UserId,
                        Content = y.InternalProcedure.Content,
                        Number = y.InternalProcedure.Number,
                        IsTransparency = y.InternalProcedure.IsTransparency,
                        Pages = y.InternalProcedure.Pages,
                        Priority = y.InternalProcedure.Priority,
                        SearchNode = y.InternalProcedure.SearchNode,
                        SearchTree = y.InternalProcedure.SearchTree,
                        Subject = y.InternalProcedure.Subject,
                        CreatedAt = y.InternalProcedure.CreatedAt,
                        Dependency = new Dependency
                        {
                            Acronym = y.InternalProcedure.Dependency.Acronym,
                            Name = y.InternalProcedure.Dependency.Name
                        },
                        DocumentType = new DocumentType
                        {
                            Code = y.InternalProcedure.DocumentType.Code,
                            Name = y.InternalProcedure.DocumentType.Name
                        },
                        User = new ApplicationUser
                        {
                            MaternalSurname = y.InternalProcedure.User.MaternalSurname,
                            Name = y.InternalProcedure.User.Name,
                            PaternalSurname = y.InternalProcedure.User.PaternalSurname,
                            FullName = y.InternalProcedure.User.FullName
                        },
                        InternalProcedureParent = y.InternalProcedure.InternalProcedureParent != null ? new InternalProcedure
                        {
                            UserId = y.InternalProcedure.InternalProcedureParent.UserId
                        } : null
                    },
                    User = new ApplicationUser
                    {
                        MaternalSurname = y.User.MaternalSurname,
                        Name = y.User.Name,
                        PaternalSurname = y.User.PaternalSurname,
                        FullName = y.User.FullName
                    },
                    CreatedAt = y.CreatedAt,
                    GeneratedId = y.GeneratedId
                })
                .OrderByDescending(y => y.GeneratedId)
                .FirstOrDefault()
            );
        }

        public static IQueryable<UserNotification> SelectUserNotification(this IQueryable<UserNotification> userNotifications)
        {
            return userNotifications.Select(x => new UserNotification
            {
                Id = x.Id,
                NotificationId = x.NotificationId,
                UserId = x.UserId,
                IsRead = x.IsRead,
                ReadDate = x.ReadDate,
                Notification = new Notification
                {
                    BackgroundStateClass = x.Notification.BackgroundStateClass,
                    SendDate = x.Notification.SendDate,
                    State = x.Notification.State,
                    Text = x.Notification.Text,
                    Url = x.Notification.Url
                },
                User = new ApplicationUser
                {
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    FullName = x.User.FullName
                }
            });
        }

        public static IQueryable<UserProcedure> SelectUserProcedure(this IQueryable<UserProcedure> userProcedures)
        {
            return userProcedures.Select(x => new UserProcedure
            {
                Id = x.Id,
                DependencyId = x.DependencyId,
                PaymentId = x.PaymentId,
                ProcedureId = x.ProcedureId,
                TermId = x.TermId,
                UserId = x.UserId,
                Comment = x.Comment,
                DNI = x.DNI,
                Status = x.Status,
                UrlImage = x.UrlImage,
                ObservationStatus = x.ObservationStatus,
                Dependency = x.Dependency != null ? new Dependency
                {
                    Acronym = x.Dependency.Acronym,
                    Name = x.Dependency.Name
                } : null,
                Payment = x.Payment != null ? new Payment
                {
                    OperationCodeB = x.Payment.OperationCodeB,
                    Description = x.Payment.Description,
                    Invoice = x.Payment.Invoice,
                    PaymentDate = x.Payment.PaymentDate,
                    Discount = x.Payment.Discount,
                    IgvAmount = x.Payment.IgvAmount,
                    Quantity = x.Payment.Quantity,
                    SubTotal = x.Payment.SubTotal,
                    Status = x.Payment.Status,
                    Total = x.Payment.Total,
                    Concept = x.Payment.Concept != null ? new Concept
                    {
                        Code = x.Payment.Concept.Code,
                        Amount = x.Payment.Concept.Amount,
                        Description = x.Payment.Concept.Description
                    }  : null,
                } : null,
                Procedure = new Procedure
                {
                    Duration = x.Procedure.Duration,
                    Name = x.Procedure.Name,
                    StaticType = x.Procedure.StaticType,
                    HasPicture = x.Procedure.HasPicture,
                    ProcedureRequirementsCostSum = x.Procedure.ProcedureRequirementsCostSum,
                    Id = x.ProcedureId,
                    ConceptId = x.Procedure.ConceptId,
                },
                Term = x.Term != null ? new Term
                {
                    Name = x.Term.Name
                } : null,
                User = new ApplicationUser
                {
                    Id = x.UserId,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    FullName = x.User.FullName,
                    UserName = x.User.UserName,
                    Email = x.User.Email,
                    Dni = x.User.Dni,
                    PhoneNumber = x.User.PhoneNumber
                },
                RecordHistoryId = x.RecordHistoryId,
                RecordHistory = x.RecordHistory != null ? new RecordHistory
                {
                    Id = x.RecordHistory.Id,
                    StartAcademicYear = x.RecordHistory.StartAcademicYear,
                    EndAcademicYear = x.RecordHistory.EndAcademicYear,
                    StartTerm = x.RecordHistory.StartTerm,
                    EndTerm = x.RecordHistory.EndTerm,
                    RecordTermId = x.RecordHistory.RecordTermId
                } : null,
                StudentUserProcedureId = x.StudentUserProcedureId,
                StudentUserProcedure = x.StudentUserProcedure != null? new StudentUserProcedure
                {
                    AcademicProgramId = x.StudentUserProcedure.AcademicProgramId,
                    CareerId = x.StudentUserProcedure.CareerId,
                    ActivityType = x.StudentUserProcedure.ActivityType,
                    CurriculumId = x.StudentUserProcedure.CurriculumId,
                    StudentId = x.StudentUserProcedure.StudentId,
                    StudentSectionId = x.StudentUserProcedure.StudentSectionId,
                    Id = x.StudentUserProcedure.Id
                } : null,
                //UserProcedureDerivations = (ICollection<UserProcedureDerivation>)(x.UserProcedureDerivations != null ? new UserProcedureDerivation
                //{
                //    Dependency = new Dependency
                //    {
                //        Acronym = x.UserProcedureDerivations.Where(i => i.UserProcedureId == x.Id).Select(x => x.Dependency.Acronym).FirstOrDefault(),
                //        Name = x.UserProcedureDerivations.Where(i => i.UserProcedureId == x.Id).Select(x => x.Dependency.Name).FirstOrDefault(),
                //    }
                //} : null),
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                GeneratedId = x.GeneratedId
            });
        }

        public static IQueryable<UserProcedureDerivation> SelectUserProcedureDerivation(this IQueryable<UserProcedureDerivation> userProcedureDerivations)
        {
            return userProcedureDerivations.Select(x => new UserProcedureDerivation
            {
                Id = x.Id,
                DependencyId = x.DependencyId,
                DependencyFromId = x.DependencyFromId,
                UserId = x.UserId,
                UserProcedureId = x.UserProcedureId,
                Observation = x.Observation,
                ProcedureTask = new ProcedureTask
                {
                    Description = x.ProcedureTask.Description,
                },
                DependencyFrom = new Dependency
                {
                    Name = x.Dependency.Name
                },
                Dependency = new Dependency
                {
                    Name = x.Dependency.Name
                },
                User = new ApplicationUser
                {
                    Name = x.User.FullName,
                    FullName = x.User.FullName
                },
                UserProcedure = new UserProcedure
                {
                    Status = x.UserProcedure.Status
                },
                CreatedAt = x.CreatedAt                
            });
        }

        public static IQueryable<UserProcedureRecord> SelectUserProcedureRecord(this IQueryable<UserProcedureRecord> userProcedureRecords)
        {
            return userProcedureRecords.Select(x => new UserProcedureRecord
            {
                Id = x.Id,
                DocumentTypeId = x.DocumentTypeId,
                DocumentaryRecordTypeId = x.DocumentaryRecordTypeId,
                RecordSubjectTypeId = x.RecordSubjectTypeId,
                UserProcedureId = x.UserProcedureId,
                EntryDate = x.EntryDate,
                FullRecordNumber = x.FullRecordNumber,
                Pages = x.Pages,
                RecordNumber = x.RecordNumber,
                Subject = x.Subject,
                DocumentType = new DocumentType
                {
                    Code = x.DocumentType.Code,
                    Name = x.DocumentType.Name
                },
                DocumentaryRecordType = new DocumentaryRecordType
                {
                    Code = x.DocumentaryRecordType.Code,
                    Name = x.DocumentaryRecordType.Name
                },
                RecordSubjectType = new RecordSubjectType
                {
                    Code = x.RecordSubjectType.Code,
                    Name = x.RecordSubjectType.Name
                },
                UserProcedure = new UserProcedure
                {
                    DependencyId = x.UserProcedure.DependencyId,
                    PaymentId = x.UserProcedure.PaymentId,
                    ProcedureId = x.UserProcedure.ProcedureId,
                    TermId = x.UserProcedure.TermId,
                    UserId = x.UserProcedure.UserId,
                    Comment = x.UserProcedure.Comment,
                    Observation = x.UserProcedure.Observation,
                    Status = x.UserProcedure.Status,
                    Dependency = x.UserProcedure.Dependency != null ? new Dependency
                    {
                        Acronym = x.UserProcedure.Dependency.Acronym,
                        Name = x.UserProcedure.Dependency.Name,
                        Signature = x.UserProcedure.Dependency.Signature
                    } : null,
                    Payment = x.UserProcedure.Payment != null ? new Payment
                    {
                        Discount = x.UserProcedure.Payment.Discount,
                        IgvAmount = x.UserProcedure.Payment.IgvAmount,
                        Quantity = x.UserProcedure.Payment.Quantity,
                        SubTotal = x.UserProcedure.Payment.SubTotal,
                        Status = x.UserProcedure.Payment.Status,
                        Total = x.UserProcedure.Payment.Total
                    } : null,
                    Procedure = new Procedure
                    {
                        Name = x.UserProcedure.Procedure.Name
                    },
                    Term = x.UserProcedure.Term != null ? new Term
                    {
                        Name = x.UserProcedure.Term.Name
                    } : null,
                    User = new ApplicationUser
                    {
                        Dni = x.UserProcedure.User.Dni,
                        MaternalSurname = x.UserProcedure.User.MaternalSurname,
                        Name = x.UserProcedure.User.Name,
                        PaternalSurname = x.UserProcedure.User.PaternalSurname,
                        FullName = x.UserProcedure.User.FullName
                    },
                    GeneratedId = x.UserProcedure.GeneratedId
                },
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });
        }

        public static IQueryable<UserProcedureRecordRequirement> SelectUserProcedureRecordRequirement(this IQueryable<UserProcedureRecordRequirement> userProcedureRecordRequirements)
        {
            return userProcedureRecordRequirements.Select(x => new UserProcedureRecordRequirement
            {
                Id = x.Id,
                ProcedureRequirementId = x.ProcedureRequirementId,
                UserProcedureRecordId = x.UserProcedureRecordId,
                ProcedureRequirement = new ProcedureRequirement
                {
                    Code = x.ProcedureRequirement.Code,
                    Cost = x.ProcedureRequirement.Cost,
                    Name = x.ProcedureRequirement.Name
                },
                UserProcedureRecord = new UserProcedureRecord
                {
                    EntryDate = x.UserProcedureRecord.EntryDate,
                    FullRecordNumber = x.UserProcedureRecord.FullRecordNumber,
                    Pages = x.UserProcedureRecord.Pages,
                    RecordNumber = x.UserProcedureRecord.RecordNumber,
                    Subject = x.UserProcedureRecord.Subject
                }
            });
        }

        public static async Task<DataTablesStructs.ReturnedData<T2>> ToDataTables2<T1, T2, T3>(this IQueryable<T1> query, DataTablesStructs.SentParameters sentParameters, Expression<Func<T1, T3>> keySelector, Expression<Func<T1, T2>> selectPredicate = null)
        {
            var data = new List<T2>();
            var recordsFiltered = 0;

            if (keySelector != null)
            {
                query = query.OrderByCondition(sentParameters.OrderDirection, keySelector);
            }

            if (query is IAsyncQueryProvider)
            {
                recordsFiltered = await query.CountAsync();
                data = await query
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .Select(selectPredicate)
                    .ToListAsync();
            }
            else
            {
                recordsFiltered = await query.CountAsync();
                data = await query
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .Select(selectPredicate)
                    .ToListAsync();
            }

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<T2>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public static async Task<DataTablesStructs.ReturnedData<T2>> ToDataTables2<T1, T2>(this IQueryable<T1> query, DataTablesStructs.SentParameters sentParameters, Expression<Func<T1, T2>> selectPredicate = null)
        {
            var data = new List<T2>();
            var recordsFiltered = 0;

            if (query is IAsyncQueryProvider)
            {
                recordsFiltered = await query.CountAsync();
                data = await query
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .Select(selectPredicate)
                    .ToListAsync();
            }
            else
            {
                recordsFiltered = await query.CountAsync();
                data = await query
                    .Skip(sentParameters.PagingFirstRecord)
                    .Take(sentParameters.RecordsPerDraw)
                    .Select(selectPredicate)
                    .ToListAsync();
            }

            var recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<T2>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
        public static async Task<DataTablesStructs.ReturnedData<T>> ToDataTables<T>(this IQueryable<T> query, DataTablesStructs.SentParameters sentParameters, Expression<Func<T, T>> selectPredicate = null,string search = null)
        {
            var recordsTotal = await query.CountAsync();
            var data = query
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);

            if (selectPredicate != null)
            {
                if (!string.IsNullOrEmpty(search))
                    data = data.Select(selectPredicate, search);
                else
                    data = data.Select(selectPredicate);
            }

            var tmpData = await data.ToListAsync();
            var recordsFiltered = tmpData.Count;

            return new DataTablesStructs.ReturnedData<T>
            {
                Data = tmpData,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsTotal,
                //RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal,
            };
        }


        public static async Task<Select2Structs.ResponseParameters> ToSelect2<T>(this IQueryable<T> query, Select2Structs.RequestParameters requestParameters, Expression<Func<T, Select2Structs.Result>> selectPredicate, int pageSize = 10, string searchValue = null)
        {
            var currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;
            var results = await query
                .Select(selectPredicate, searchValue)
                .Skip(currentPage * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = results.Count >= pageSize
                },
                Results = results
            };
        }
        /// <summary>
        /// Realiza una busqueda Full-Text en los campos indicados. (Debe habilitar Full-Text en las columnas)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="searchValuePredicate"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static IQueryable<ApplicationUser> WhereUserFullText(this IQueryable<ApplicationUser> query,
            string searchValue
            )
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.FullName, searchValue) || EF.Functions.Contains(x.UserName, searchValue));
                }
                else
                {
                    searchValue = searchValue.ToUpper();
                    query = query.Where(x => x.Dni.ToUpper().Contains(searchValue) || x.FullName.ToUpper().Contains(searchValue) || x.UserName.ToUpper().Contains(searchValue) || x.Document.ToUpper().Contains(searchValue));
                }
            }
            return query;
        }

        public static IQueryable<Concept> WhereUserFullText(this IQueryable<Concept> query,
            string searchValue
            )
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                if (ConstantHelpers.GENERAL.DATABASES.FULLTEXT_ENABLED)
                {
                    searchValue = $"\"{searchValue}*\"";
                    query = query.Where(x => EF.Functions.Contains(x.Description, searchValue) || EF.Functions.Contains(x.Code, searchValue));
                }
                else
                {
                    searchValue = searchValue.ToUpper();
                    query = query.Where(x => x.Description.ToUpper().Contains(searchValue) || x.Code.ToUpper().Contains(searchValue));
                }
            }
            return query;
        }
    }
}
