using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.REPOSITORY.Data;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace AKDEMIC.REPOSITORY.Helpers
{
    public static class ExpressionHelpers
    {
        public static Expression<Func<ApplicationRole, ApplicationRole>> SelectApplicationRole()
        {
            return (x) => new ApplicationRole
            {
                Id = x.Id,
                ConcurrencyStamp = x.ConcurrencyStamp,
                IsStatic = x.IsStatic
            };
        }

        public static Expression<Func<ApplicationRole, ApplicationRole>> SelectApplicationRole(AkdemicContext context, Guid procedureId)
        {
            return (x) => new ApplicationRole
            {
                Id = x.Id,
                ConcurrencyStamp = x.ConcurrencyStamp,
                IsInProcedure = context.ProcedureRoles.Any(y => y.ProcedureId == procedureId && y.RoleId == x.Id),
                IsStatic = x.IsStatic
            };
        }

        public static Expression<Func<Classifier, Classifier>> SelectClassifier()
        {
            return (x) => new Classifier
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Code = x.Code,
                Description = x.Description,
                Name = x.Name,
                Parent = (x.Parent != null ? new Classifier
                {
                    Code = x.Parent.Code,
                    Description = x.Parent.Description,
                    Name = x.Parent.Name
                } : null)
            };
        }

        public static Expression<Func<Dependency, Dependency>> SelectDependency()
        {
            return (x) => new Dependency
            {
                Id = x.Id,
                UserId = x.UserId,
                Name = x.Name,
                Acronym = x.Acronym,
                Signature = x.Signature,
                SuperiorDependencyId = x.SuperiorDependencyId,
                FacultyId = x.FacultyId,
                CareerId = x.CareerId,
                SuperiorDependency = (x.SuperiorDependency != null ? new Dependency
                {
                    Name = x.SuperiorDependency.Name
                } : null),
                User = (x.User != null ? new ApplicationUser
                {
                    Dni = x.User.Dni,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    Sex = x.User.Sex,
                    FullName = x.User.FullName,

                } : null),
                ResolutiveActsDirector = new ApplicationUser
                {
                    Id = x.ResolutiveActsDirector.Id,
                    FullName = x.ResolutiveActsDirector.FullName,
                    UserName = x.ResolutiveActsDirector.UserName
                },
                AreaManager = x.DirectoryDependencies.Where(y=>y.Charge == ConstantHelpers.VirtualDirectory.DirectoryDependency.Charge.RESPONSIBLE).Select(y=>y.Name).FirstOrDefault(),
                Secretary = x.DirectoryDependencies.Where(y => y.Charge == ConstantHelpers.VirtualDirectory.DirectoryDependency.Charge.SECRETARY).Select(y => y.Name).FirstOrDefault(),
                DirectoryDependencies = x.DirectoryDependencies.ToList()
            };
        }

        public static Expression<Func<Dependency, Dependency>> SelectDependency(AkdemicContext context, Guid procedureId)
        {
            return (x) => new Dependency
            {
                Id = x.Id,
                UserId = x.UserId,
                Name = x.Name,
                Acronym = x.Acronym,
                Signature = x.Signature,
                IsProcedureDependency = context.ProcedureDependencies.Any(y => y.DependencyId == x.Id && y.ProcedureId == procedureId),
                User = (x.User != null ? new ApplicationUser
                {
                    Dni = x.User.Dni,
                    MaternalSurname = x.User.MaternalSurname,
                    Name = x.User.Name,
                    PaternalSurname = x.User.PaternalSurname,
                    Sex = x.User.Sex
                } : null)
            };
        }

        public static Expression<Func<DocumentaryRecordType, DocumentaryRecordType>> SelectDocumentaryRecordType()
        {
            return (x) => new DocumentaryRecordType
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            };
        }

        public static Expression<Func<DocumentType, DocumentType>> SelectDocumentType()
        {
            return (x) => new DocumentType
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            };
        }

        public static Expression<Func<ExternalProcedure, ExternalProcedure>> SelectExternalProcedure()
        {
            return (x) => new ExternalProcedure
            {
                Id = x.Id,
                ClassifierId = x.ClassifierId,
                DependencyId = x.DependencyId,
                Code = x.Code,
                Comment = x.Comment,
                Cost = x.Cost,
                IsTransparency = x.IsTransparency,
                Name = x.Name,
                StaticType = x.StaticType,
                ConceptId = x.ConceptId,
                Concept = x.Concept != null ? new Concept
                {
                    Description = x.Concept.Description,
                    Amount = x.Concept.Amount
                } : null,
                Classifier = x.Classifier != null ? new Classifier
                {
                    Code = x.Classifier.Code,
                    Description = x.Classifier.Description,
                    Name = x.Classifier.Name
                } : null,
                Dependency = x.Dependency != null ? new Dependency
                {
                    Acronym = x.Dependency.Acronym,
                    Name = x.Dependency.Name
                } : null,
                UITId = x.UITId,
                UIT = x.UIT != null ? new UIT
                {
                    Id = x.UIT.Id,
                    Year = x.UIT.Year,
                    Value = x.UIT.Value
                } : null
            };
        }

        public static Expression<Func<ExternalUser, ExternalUser>> SelectExternalUser()
        {
            return (x) => new ExternalUser
            {
                Id = x.Id,
                BirthDate = x.BirthDate,
                DocumentNumber = x.DocumentNumber,
                Name = x.Name,
                MaternalSurname = x.MaternalSurname,
                PaternalSurname = x.PaternalSurname,
                PhoneNumber = x.PhoneNumber
            };
        }

        public static Expression<Func<Faculty, Faculty>> SelectFaculty()
        {
            return (x) => new Faculty
            {
                Id = x.Id,
                IsValid = x.IsValid,
                Code = x.Code,
                SuneduCode = x.SuneduCode,
                Abbreviation = x.Abbreviation,
                Name = x.Name,
                DecanalResolutionDate = x.DecanalResolutionDate,
                DecanalResolutionNumber = x.DecanalResolutionNumber,
                RectoralResolutionDate = x.RectoralResolutionDate,
                RectoralResolutionNumber = x.RectoralResolutionNumber,
                DeanId = x.DeanId,
                SecretaryId = x.SecretaryId,
                DeanGrade = x.DeanGrade,
                Dean = new ApplicationUser
                {
                    FullName = x.Dean.FullName
                },
                Secretary = new ApplicationUser
                {
                    FullName = x.Secretary.FullName
                },
                AdministrativeAssistant = new ApplicationUser
                {
                    FullName = x.AdministrativeAssistant.FullName
                },
                DeanResolutionFile = x.DeanResolutionFile,
                DeanResolution = x.DeanResolution
            };
        }

        public static Expression<Func<InternalProcedureFile, InternalProcedureFile>> SelectInternalProcedureFile()
        {
            return (x) => new InternalProcedureFile
            {
                Id = x.Id,
                FileName = x.FileName,
                Path = x.Path,
                Size = x.Size
            };
        }

        public static Expression<Func<InternalProcedureReference, InternalProcedureReference>> SelectInternalProcedureReference()
        {
            return (x) => new InternalProcedureReference
            {
                Id = x.Id,
                Reference = x.Reference
            };
        }

        public static Expression<Func<Procedure, Procedure>> SelectProcedure()
        {
            return (x) => new Procedure
            {
                Id = x.Id,
                ClassifierId = x.ClassifierId,
                ProcedureCategoryId = x.ProcedureCategoryId,
                ProcedureSubcategoryId = x.ProcedureSubcategoryId,
                Code = x.Code,
                Duration = x.Duration,
                LegalBase = x.LegalBase,
                Name = x.Name,
                Score = x.Score,
                StaticType = x.StaticType,
                ProcedureRequirementsCostSum = x.ProcedureRequirements.Sum(y => y.Cost),
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
            };
        }

        public static Expression<Func<ProcedureCategory, ProcedureCategory>> SelectProcedureCategory()
        {
            return (x) => new ProcedureCategory
            {
                Id = x.Id,
                Name = x.Name,
                StaticType = x.StaticType
            };
        }

        public static Expression<Func<ProcedureSubcategory, ProcedureSubcategory>> SelectProcedureSubcategory()
        {
            return (x) => new ProcedureSubcategory
            {
                Id = x.Id,
                ProcedureCategoryId = x.ProcedureCategoryId,
                Name = x.Name,
                StaticType = x.StaticType,
                ProcedureCategory = (x.ProcedureCategory != null ? new ProcedureCategory
                {
                    Name = x.ProcedureCategory.Name
                } : null)
            };
        }

        public static Expression<Func<ProcedureRequirement, ProcedureRequirement>> SelectProcedureRequirement()
        {
            return (x) => new ProcedureRequirement
            {
                Id = x.Id,
                ProcedureId = x.ProcedureId,
                Code = x.Code,
                Cost = x.Cost,
                Name = x.Name,
                Type = x.Type,
                SystemValidationType = x.SystemValidationType
            };
        }

        public static Expression<Func<ProcedureResolution, ProcedureResolution>> SelectProcedureResolution()
        {
            return (x) => new ProcedureResolution
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
            };
        }

        public static Expression<Func<Order, Order>> SelectOrder()
        {
            return (x) => new Order
            {
                Id = x.Id,
                Cost = x.Cost,
                EndDate = x.EndDate,
                StartDate = x.StartDate,
                Description = x.Description,
                FileName = x.FileName,
                Path = x.Path,
                Size = x.Size,
                Status = x.Status,
                Title = x.Title
            };
        }

        public static Expression<Func<OrderChangeHistory, OrderChangeHistory>> SelectOrderChange()
        {
            return (x) => new OrderChangeHistory
            {
                Id = x.Id,
                OrderId = x.OrderId,
                Description = x.Description
            };
        }

        public static Expression<Func<OrderChangeFileHistory, OrderChangeFileHistory>> SelectOrderChangeFile()
        {
            return (x) => new OrderChangeFileHistory
            {
                Id = x.Id,
                OrderChangeId = x.OrderChangeId,
                FileName = x.FileName,
                Path = x.Path,
                Size = x.Size
            };
        }

        public static Expression<Func<UIT, UIT>> SelectUIT()
        {
            return (x) => new UIT
            {
                Id = x.Id,
                Value = x.Value,
                Year = x.Year
            };
        }

        public static Expression<Func<Requirement, Requirement>> SelectRequirement()
        {
            return (x) => new Requirement
            {
                Id = x.Id,
                //OrderId = x.OrderId,
                SupplierId = x.SupplierId,
                UserId = x.UserId,
                Description = x.Description,
                Folio = x.Folio,
                Need = x.Need,
                Subject = x.Subject
            };
        }

        public static Expression<Func<RequirementFile, RequirementFile>> SelectRequirementFile()
        {
            return (x) => new RequirementFile
            {
                Id = x.Id,
                RequirementId = x.RequirementId,
                FileName = x.FileName,
                Path = x.Path,
                Size = x.Size
            };
        }

        public static Expression<Func<RequirementSupplier, RequirementSupplier>> SelectRequirementSupplier()
        {
            return (x) => new RequirementSupplier
            {
                Id = x.Id,
                RequirementId = x.RequirementId,
                SupplierId = x.SupplierId,
                Requirement = new Requirement
                {
                    //OrderId = x.Requirement.OrderId,
                    SupplierId = x.Requirement.SupplierId,
                    UserId = x.Requirement.UserId,
                    Description = x.Requirement.Description,
                    Folio = x.Requirement.Folio,
                    Need = x.Requirement.Need,
                    Subject = x.Requirement.Subject
                },
                Supplier = new Supplier
                {
                    UserId = x.Supplier.UserId,
                    Name = x.Supplier.Name,
                    RUC = x.Supplier.RUC
                }
            };
        }

        public static Expression<Func<Survey, object>> GeneralSurvey()
        {
            return (x) => new
            {
                x.Id,
                x.Code,
                Title = x.Name,
                Career = x.CareerId == null ? "Sin Asignar" : x.Career.Name,
                Answers = x.SurveyUsers.Where(y => y.DateTime != null).Count(),
                StatusId = x.State,
                Status = ConstantHelpers.SURVEY_STATES.VALUES.ContainsKey(x.State) == false
                        ? "Desconocido"
                        : ConstantHelpers.SURVEY_STATES.VALUES[x.State],
                PublishDate = x.PublicationDate.ToLocalTime().ToString("dd/MM/yyyy"),
                FinishDate = x.FinishDate.ToLocalTime().ToString("dd/MM/yyyy")
            };
        }
        public static Expression<Func<Survey, object>> GenericSurvey()
        {
            return (x) => new
            {
                x.Id,
                x.Code,
                x.Name,
                x.Description,
                x.State,
                StateText = ConstantHelpers.SURVEY_STATES.VALUES.ContainsKey(x.State) == false
                        ? "Desconocido"
                        : ConstantHelpers.SURVEY_STATES.VALUES[x.State],
                publicationDate = x.PublicationDate.ToString("dd/MM/yyyy"),
                finishDate = x.FinishDate.ToString("dd/MM/yyyy")
            };
        }

        public static Expression<Func<Survey, object>> JobExchangeSurvey()
        {
            return (x) => new
            {
                x.Id,
                Title = x.Name,
                x.Code,
                Career = x.Career == null ? "" : x.Career.Name,
                HasQuestions = x.SurveyItems.Where(y => y.Questions.Any(z => z.SurveyItemId == y.Id)).Count() == 0 ? false : true,
                StatusId = x.State,
                Status = ConstantHelpers.SURVEY_STATES.VALUES.ContainsKey(x.State) == false
                        ? "Desconocido"
                        : ConstantHelpers.SURVEY_STATES.VALUES[x.State]
            };
        }

        public static Expression<Func<ApplicationUser, object>> JobExchangeUsersSentSurvey()
        {
            return (x) => new
            {
                x.Id,
                User = x.UserName,
                Name = x.FullName,
                Role = x.UserRoles.Select(y => y.Role.Name).FirstOrDefault()
            };
        }

        public static Expression<Func<ApplicationUser, object>> IntranetUsersSentSurvey()
        {
            return (x) => new
            {
                x.Id,
                x.FullName,
                x.Email,
            };
        }

        public static Expression<Func<SurveyUser, object>> AnswerByUserSurvey()
        {
            return (x) => new
            {
                surveyUserId = x.Id,
                x.User.FullName,
                x.User.Email
            };
        }

        public static Expression<Func<SurveyUser, object>> SurveyUserAnswer()
        {
            return (x) => new
            {
                x.Id,
                x.User.Name,
                x.User.PaternalSurname,
                x.User.MaternalSurname,
                x.User.Email
            };
        }

        public static Expression<Func<Supplier, Supplier>> SelectSupplier()
        {
            return (x) => new Supplier
            {
                Id = x.Id,
                UserId = x.UserId,
                Name = x.Name,
                RUC = x.RUC,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                State = x.State,
                StateText = ConstantHelpers.ECONOMICMANAGEMENT.SUPPLIERCATEGORYSTATES.STATES.ContainsKey(x.State) ?
                    ConstantHelpers.ECONOMICMANAGEMENT.SUPPLIERCATEGORYSTATES.STATES[x.State] : "Desconocido",
                SupplierCategoryId = x.SupplierCategoryId,
                SupplierCategory = new SupplierCategory
                {
                    Name = x.SupplierCategory.Name
                },
                User = new ApplicationUser
                {
                    Dni = x.UserId != null ? x.User.Dni : "--",
                    MaternalSurname = x.UserId != null ? x.User.MaternalSurname : "--",
                    Name = x.UserId != null ? x.User.Name : "--",
                    PaternalSurname = x.UserId != null ? x.User.PaternalSurname : "--",
                    Sex = x.UserId != null ? x.User.Sex : ConstantHelpers.SEX.MALE,
                    FullName = x.UserId != null ? x.User.FullName : "--"
                }
            };
        }

        public static Expression<Func<UserExternalProcedure, UserExternalProcedure>> SelectUserExternalProcedure()
        {
            return (x) => new UserExternalProcedure
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
                UserExternalProcedureRecords= x.UserExternalProcedureRecords.Where(y=>y.UserExternalProcedureId == x.Id).Select(y=> new UserExternalProcedureRecord
                {
                    Id = y.Id,
                    UserExternalProcedureRecordDocuments = y.UserExternalProcedureRecordDocuments.ToList()
                }).ToList(),
                ExternalProcedure = new ExternalProcedure
                {
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
                        Id = x.Dependency.Id,
                        Name = x.Dependency.Name
                    },
                    CreatedAt = x.ExternalProcedure.CreatedAt
                },
                ExternalUser = new ExternalUser
                {
                    DocumentNumber = x.ExternalUser.DocumentNumber,
                    MaternalSurname = x.ExternalUser.MaternalSurname,
                    Name = x.ExternalUser.Name,
                    PaternalSurname = x.ExternalUser.PaternalSurname,
                    FullName = x.ExternalUser.FullName
                },
                InternalProcedure = x.InternalProcedure != null ? new InternalProcedure
                {
                    Content = x.InternalProcedure.Content,
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
                    Total = x.Payment.Total
                } : null,
                Term = x.Term != null ? new Term
                {
                    Name = x.Term.Name
                } : null,
                GeneratedId = x.GeneratedId,
                CreatedAt = x.CreatedAt,
                Code = x.UserExternalProcedureRecords.Any() ? x.UserExternalProcedureRecords.Select(y => y.FullRecordNumber).FirstOrDefault() : "-"
                //Code = x.UserExternalProcedureRecords.Any() ? x.UserExternalProcedureRecords.Select(y => y.FullRecordNumber).FirstOrDefault() : $"EXTE-{x.Number}-{(x.CreatedAt.HasValue ? x.CreatedAt.Value.Year.ToString() : null)}{(x.Dependency != null ? "-" : null)}{x.Dependency.Acronym.ToUpper()}"
            };
        }

        public static Expression<Func<UserExternalProcedureRecord, UserExternalProcedureRecord>> SelectUserExternalProcedureRecord()
        {
            return (x) => new UserExternalProcedureRecord
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                DocumentTypeId = x.DocumentTypeId,
                RecordSubjectTypeId = x.RecordSubjectTypeId,
                UserExternalProcedureId = x.UserExternalProcedureId,
                Attachments = x.Attachments,
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
                    Number = x.UserExternalProcedure.Number,
                    Observation = x.UserExternalProcedure.Observation,
                    Status = x.UserExternalProcedure.Status,
                    Term = new Term
                    {
                        Name = x.UserExternalProcedure.Term.Name
                    },
                    Dependency = new Dependency
                    {
                        Acronym = x.UserExternalProcedure.Dependency.Acronym
                    },
                    ExternalProcedure = new ExternalProcedure
                    {
                        IsTransparency = x.UserExternalProcedure.ExternalProcedure.IsTransparency,
                        Name = x.UserExternalProcedure.ExternalProcedure.Name,
                        StaticType = x.UserExternalProcedure.ExternalProcedure.StaticType
                    },
                    ExternalUser = new ExternalUser
                    {
                        MaternalSurname = x.UserExternalProcedure.ExternalUser.MaternalSurname,
                        Name = x.UserExternalProcedure.ExternalUser.Name,
                        FullName = x.UserExternalProcedure.ExternalUser.FullName,
                        PaternalSurname = x.UserExternalProcedure.ExternalUser.PaternalSurname
                    }
                }
            };
        }

        public static Expression<Func<UserExternalProcedureRecordDocument, UserExternalProcedureRecordDocument>> SelectUserExternalProcedureRecordDocument()
        {
            return (x) => new UserExternalProcedureRecordDocument
            {
                Id = x.Id,
                DocumentBytesSize = x.DocumentBytesSize,
                DocumentUrl = x.DocumentUrl,
                Name = x.Name
            };
        }

        public static Expression<Func<RecordSubjectType, RecordSubjectType>> SelectRecordSubjectType()
        {
            return (x) => new RecordSubjectType
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            };
        }

        public static Expression<Func<UserRequirement, UserRequirement>> SelectUserRequirement()
        {
            return (x) => new UserRequirement
            {
                Id = x.Id,
                RequirementId = x.RequirementId,
                RoleId = x.RoleId,
                Comment = x.Comment,
                Cost = x.Cost,
                IsPAC = x.IsPAC,
                Status = x.Status,
                Requirement = new Requirement
                {
                    Description = x.Requirement.Description,
                    Folio = x.Requirement.Folio,
                    Need = x.Requirement.Need,
                    Subject = x.Requirement.Subject
                },
                Role = new ApplicationRole
                {
                    Name = x.Role.Name
                }
            };
        }

        public static Expression<Func<UserRequirementFile, UserRequirementFile>> SelectUserRequirementFile()
        {
            return (x) => new UserRequirementFile
            {
                Id = x.Id,
                UserRequirementId = x.UserRequirementId,
                FileName = x.FileName,
                Path = x.Path,
                Size = x.Size
            };
        }
    }
}
