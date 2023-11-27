using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace AKDEMIC.CORE.Overrides
{
    public class CustomHtmlGenerator : DefaultHtmlGenerator
    {
        public CustomHtmlGenerator(IAntiforgery antiforgery, IOptions<MvcViewOptions> optionsAccessor, IModelMetadataProvider metadataProvider, IUrlHelperFactory urlHelperFactory, HtmlEncoder htmlEncoder, ValidationHtmlAttributeProvider validationAttributeProvider) : base(antiforgery, optionsAccessor, metadataProvider, urlHelperFactory, htmlEncoder, validationAttributeProvider)
        {
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override TagBuilder GenerateActionLink(ViewContext viewContext, string linkText, string actionName, string controllerName, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes)
        {
            return base.GenerateActionLink(viewContext, linkText, actionName, controllerName, protocol, hostname, fragment, routeValues, htmlAttributes);
        }

        public override IHtmlContent GenerateAntiforgery(ViewContext viewContext)
        {
            return base.GenerateAntiforgery(viewContext);
        }

        public override TagBuilder GenerateCheckBox(ViewContext viewContext, ModelExplorer modelExplorer, string expression, bool? isChecked, object htmlAttributes)
        {
            return base.GenerateCheckBox(viewContext, modelExplorer, expression, isChecked, htmlAttributes);
        }

        public override TagBuilder GenerateForm(ViewContext viewContext, string actionName, string controllerName, object routeValues, string method, object htmlAttributes)
        {
            return base.GenerateForm(viewContext, actionName, controllerName, routeValues, method, htmlAttributes);
        }

        public override TagBuilder GenerateHidden(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object value, bool useViewData, object htmlAttributes)
        {
            return base.GenerateHidden(viewContext, modelExplorer, expression, value, useViewData, htmlAttributes);
        }

        public override TagBuilder GenerateHiddenForCheckbox(ViewContext viewContext, ModelExplorer modelExplorer, string expression)
        {
            return base.GenerateHiddenForCheckbox(viewContext, modelExplorer, expression);
        }

        public override TagBuilder GenerateLabel(ViewContext viewContext, ModelExplorer modelExplorer, string expression, string labelText, object htmlAttributes)
        {
            return base.GenerateLabel(viewContext, modelExplorer, expression, labelText, htmlAttributes);
        }

        public override TagBuilder GeneratePageForm(ViewContext viewContext, string pageName, string pageHandler, object routeValues, string fragment, string method, object htmlAttributes)
        {
            return base.GeneratePageForm(viewContext, pageName, pageHandler, routeValues, fragment, method, htmlAttributes);
        }

        public override TagBuilder GeneratePageLink(ViewContext viewContext, string linkText, string pageName, string pageHandler, string protocol, string hostname, string fragment, object routeValues, object htmlAttributes)
        {
            return base.GeneratePageLink(viewContext, linkText, pageName, pageHandler, protocol, hostname, fragment, routeValues, htmlAttributes);
        }

        public override TagBuilder GeneratePassword(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object value, object htmlAttributes)
        {
            return base.GeneratePassword(viewContext, modelExplorer, expression, value, htmlAttributes);
        }

        public override TagBuilder GenerateRadioButton(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object value, bool? isChecked, object htmlAttributes)
        {
            return base.GenerateRadioButton(viewContext, modelExplorer, expression, value, isChecked, htmlAttributes);
        }

        public override TagBuilder GenerateRouteLink(ViewContext viewContext, string linkText, string routeName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            return base.GenerateRouteLink(viewContext, linkText, routeName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        public override TagBuilder GenerateSelect(ViewContext viewContext, ModelExplorer modelExplorer, string optionLabel, string expression, IEnumerable<SelectListItem> selectList, ICollection<string> currentValues, bool allowMultiple, object htmlAttributes)
        {
            return base.GenerateSelect(viewContext, modelExplorer, optionLabel, expression, selectList, currentValues, allowMultiple, htmlAttributes);
        }

        public override TagBuilder GenerateTextArea(ViewContext viewContext, ModelExplorer modelExplorer, string expression, int rows, int columns, object htmlAttributes)
        {
            return base.GenerateTextArea(viewContext, modelExplorer, expression, rows, columns, htmlAttributes);
        }

        public override TagBuilder GenerateTextBox(ViewContext viewContext, ModelExplorer modelExplorer, string expression, object value, string format, object htmlAttributes)
        {
            return base.GenerateTextBox(viewContext, modelExplorer, expression, value, format, htmlAttributes);
        }

        public override TagBuilder GenerateValidationMessage(ViewContext viewContext, ModelExplorer modelExplorer, string expression, string message, string tag, object htmlAttributes)
        {
            return base.GenerateValidationMessage(viewContext, modelExplorer, expression, message, tag, htmlAttributes);
        }

        public override TagBuilder GenerateValidationSummary(ViewContext viewContext, bool excludePropertyErrors, string message, string headerTag, object htmlAttributes)
        {
            return base.GenerateValidationSummary(viewContext, excludePropertyErrors, message, headerTag, htmlAttributes);
        }

        public override ICollection<string> GetCurrentValues(ViewContext viewContext, ModelExplorer modelExplorer, string expression, bool allowMultiple)
        {
            return base.GetCurrentValues(viewContext, modelExplorer, expression, allowMultiple);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override void AddPlaceholderAttribute(ViewDataDictionary viewData, TagBuilder tagBuilder, ModelExplorer modelExplorer, string expression)
        {
            base.AddPlaceholderAttribute(viewData, tagBuilder, modelExplorer, expression);
        }

        protected override void AddValidationAttributes(ViewContext viewContext, TagBuilder tagBuilder, ModelExplorer modelExplorer, string expression)
        {
            base.AddValidationAttributes(viewContext, tagBuilder, modelExplorer, expression);
        }

        protected override TagBuilder GenerateFormCore(ViewContext viewContext, string action, string method, object htmlAttributes)
        {
            return base.GenerateFormCore(viewContext, action, method, htmlAttributes);
        }

        protected override TagBuilder GenerateInput(ViewContext viewContext, InputType inputType, ModelExplorer modelExplorer, string expression, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, string format, IDictionary<string, object> htmlAttributes)
        {
            //setId = false;

            return base.GenerateInput(viewContext, inputType, modelExplorer, expression, value, useViewData, isChecked, setId, isExplicitValue, format, htmlAttributes);
        }

        protected override TagBuilder GenerateLink(string linkText, string url, object htmlAttributes)
        {
            return base.GenerateLink(linkText, url, htmlAttributes);
        }
    }
}
