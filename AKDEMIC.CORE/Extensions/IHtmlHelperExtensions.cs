using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Extensions
{
    public static class IHtmlHelperExtensions
    {
        public static async Task<IHtmlContent> PartialForAsync<TModel, TProperty>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string partialViewName, string prefixName = "")
        {
            ModelExpressionProvider modelExpressionProvider = (ModelExpressionProvider)helper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));
            var model = modelExpressionProvider.CreateModelExpression(helper.ViewData, expression).Model;
            var viewData = new ViewDataDictionary(helper.ViewData)
            {
                TemplateInfo = { HtmlFieldPrefix = prefixName }
            };

            return await helper.PartialAsync(partialViewName, model, viewData);
        }

        public static async Task<IHtmlContent> PartialForAsync<TModel>(this IHtmlHelper<TModel> helper, string partialViewName, string prefixName = "")
        {
            var viewData = new ViewDataDictionary(helper.ViewData)
            {
                TemplateInfo = { HtmlFieldPrefix = prefixName }
            };
            viewData.Model = null;

            return await helper.PartialAsync(partialViewName, viewData);
        }
    }
}
