using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Quibuk
{
    public class QuibukAuthorityField
    {
        public int id { get; set; }
        [Required]
        [StringLength(3)]
        //[Column(TypeName = "nvarchar(3)")]
        public string tag_number { get; set; }
        [StringLength(191)]
        //[Column(TypeName = "nvarchar(191)")]
        public string control_data { get; set; }
        [StringLength(1)]
        //[Column(TypeName = "nvarchar(1)")]
        public string ind1 { get; set; }
        [StringLength(1)]
        //[Column(TypeName = "nvarchar(1)")]
        public string ind2 { get; set; }
        [Required]
        public int id_autoridad { get; set; }
        public QuibukAuthoritie QuibukAuthoritie { get; set; }
        //[Column(TypeName = "DateTime")]
        public DateTime created_at { get; set; }
        //[Column(TypeName = "DateTime")]
        public DateTime updated_at { get; set; }

    }
}
