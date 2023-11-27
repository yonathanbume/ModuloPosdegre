using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Preuniversitary.Templates
{
    public class PostulantTemplate
    {
        public Guid Id { get; set; }
        public string Document { get; set; }
        public string Code { get; set; }
        public string WebCode { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Term { get; set; }
        public string Career { get; set; }
        public string IsPaid { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
