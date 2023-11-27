using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Sisco.Template
{
    public class EditShortcutTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PublicationDate { get; set; }
        public string UrlDirection { get; set; }
        public List<SubShortcutTemplate> ListSubShortcut { get; set; }
        public EditShortcutTemplate()
        {
            ListSubShortcut = new List<SubShortcutTemplate>();
        }
    }
}
