using System;

namespace AKDEMIC.ENTITIES.Models.Sisco
{
    public class SubShortcut
    {
        public Guid Id { get; set; }
        public Guid? ShortcutId { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string UrlDirection { get; set; }

        public Shortcut Shortcut { get; set; }
    }
}
