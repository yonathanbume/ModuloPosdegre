﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserProcedureRecord : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid DocumentaryRecordTypeId { get; set; }
        public Guid? DocumentTypeId { get; set; }
        public Guid RecordSubjectTypeId { get; set; }
        public Guid UserProcedureId { get; set; }

        [Required]
        public int Attachments { get; set; } = 0;

        [Required]
        public DateTime EntryDate { get; set; }

        [Required]
        public string FullRecordNumber { get; set; }

        [Required]
        public int Pages { get; set; }

        [Required]
        public int RecordNumber { get; set; }

        [Required]
        public string Subject { get; set; }

        [NotMapped]
        public string FormattedEntryDate => EntryDate.ToLocalDateFormat();

        public DocumentaryRecordType DocumentaryRecordType { get; set; }
        public DocumentType DocumentType { get; set; }
        public RecordSubjectType RecordSubjectType { get; set; }
        public UserProcedure UserProcedure { get; set; }
    }
}
