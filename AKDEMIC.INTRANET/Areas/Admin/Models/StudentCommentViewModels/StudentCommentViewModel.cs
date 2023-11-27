using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.StudentCommentViewModels
{
    public class StudentCommentViewModel
    {
        public Guid Id { get; set; }
        public DateTime RegisterDate { get; set; }
        public string CommentDescription { get; set; }
        public Guid StudentInformationId { get; set; }
    }
}
