using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.CORE.Overrides
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExtensionsAttribute : ValidationAttribute
    {
        private List<string> AllowedExtensions { get; set; }

        public ExtensionsAttribute(string fileExtensions)
        {
            AllowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ConstantHelpers.MESSAGES.VALIDATION.FILE_EXTENSIONS, name,
                string.Join(',', AllowedExtensions));
        }

        public override bool IsValid(object value)
        {
            if (!(value is IFormFile file))
            {
                return true;
            }

            var fileName = file.FileName.ToLower();
            var tmp = AllowedExtensions.Any(y => fileName.EndsWith(y.ToLower()));

            return tmp;
        }
    }
}
