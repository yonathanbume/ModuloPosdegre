using System;

namespace AKDEMIC.REPOSITORY.Repositories.InterestGroup.Templates.MeetingFile
{
    public class MeetingFileTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UploadDate { get; set; }
        public string UrlFile { get; set; }
        public byte Type { get; set; }
    }
}
