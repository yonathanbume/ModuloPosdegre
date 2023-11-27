using System;

namespace AKDEMIC.ENTITIES.Base.Interfaces
{
    public interface IEntity
    {
        // I don't know how this works, but if the database has a column with the same name
        // a NotMapped property could be used to relate to the fields in the database

        int GeneratedId { get; set; }
        string RelationId { get; set; }
        string SearchId { get; set; }
        DateTime? DeletedAt { get; set; }
        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        string DeletedBy { get; set; }
        string CreatedBy { get; set; }
        string UpdatedBy { get; set; }
    }
}
