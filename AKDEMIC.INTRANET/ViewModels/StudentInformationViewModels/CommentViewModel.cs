using System;

namespace AKDEMIC.INTRANET.ViewModels.StudentInformationViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public DateTime RegisterDate { get; set; }
        public string CommentDescription { get; set; }
        public Guid StudentInformationId { get; set; }
    }
}
