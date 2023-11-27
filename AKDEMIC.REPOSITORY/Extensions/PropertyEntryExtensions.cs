using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Extensions
{
    public static class PropertyEntryExtensions
    {
        public static void SetCurrentValue(this PropertyEntry propertyEntry, object currentValue, bool useModifiedProperty = true, bool useTemporaryProperty = true)
        {
            if (
                propertyEntry != null &&
                (!useModifiedProperty || (useModifiedProperty && propertyEntry.IsModified)) &&
                (!useTemporaryProperty || (useTemporaryProperty && propertyEntry.IsTemporary))
            )
            {
                propertyEntry.CurrentValue = currentValue;
            }
        }
    }
}
