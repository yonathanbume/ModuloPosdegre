using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace AKDEMIC.CORE.Options
{
    public class DataAnnotationStringLocalizer : IStringLocalizer
    {
        private readonly IStringLocalizer _primaryLocalizer;
        private readonly IStringLocalizer _fallbackLocalizer;

        public DataAnnotationStringLocalizer(IStringLocalizer primaryLocalizer, IStringLocalizer fallbackLocalizer)
        {
            this._primaryLocalizer = primaryLocalizer ?? throw new ArgumentNullException(nameof(primaryLocalizer));
            this._fallbackLocalizer = fallbackLocalizer ?? throw new ArgumentNullException(nameof(fallbackLocalizer));
        }

        public LocalizedString this[string name]
        {
            get
            {
                LocalizedString localizedString = _primaryLocalizer[name];
                if (localizedString.ResourceNotFound)
                {
                    localizedString = _fallbackLocalizer[name];
                }

                return localizedString;
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                LocalizedString localizedString = _primaryLocalizer[name, arguments];
                if (localizedString.ResourceNotFound)
                {
                    localizedString = _fallbackLocalizer[name, arguments];
                }

                return localizedString;
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            => _primaryLocalizer.GetAllStrings(includeParentCultures).Concat(_fallbackLocalizer.GetAllStrings(includeParentCultures));

        public IStringLocalizer WithCulture(CultureInfo culture)
            => new DataAnnotationStringLocalizer(_primaryLocalizer.WithCulture(culture), _fallbackLocalizer.WithCulture(culture));
    }
}
