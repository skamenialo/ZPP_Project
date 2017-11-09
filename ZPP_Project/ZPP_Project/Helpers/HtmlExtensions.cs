using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZPP_Project.Helpers
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString ZPPValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors, string message, object htmlAttributes)
        {
            var formContext = htmlHelper.ViewContext.ClientValidationEnabled ? htmlHelper.ViewContext.FormContext : null;
            if (formContext == null && htmlHelper.ViewData.ModelState.IsValid)
            {
                return null;
            }

            string messageSpan;
            if (!string.IsNullOrEmpty(message))
            {
                TagBuilder spanTag = new TagBuilder("span");
                spanTag.SetInnerText(message);
                messageSpan = spanTag.ToString(TagRenderMode.Normal) + Environment.NewLine;
            }
            else
            {
                messageSpan = null;
            }

            var htmlSummary = new StringBuilder();
            TagBuilder unorderedList = new TagBuilder("ul");

            IEnumerable<ModelState> modelStates = null;
            if (excludePropertyErrors)
            {
                ModelState ms;
                htmlHelper.ViewData.ModelState.TryGetValue(htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, out ms);
                if (ms != null)
                {
                    modelStates = new ModelState[] { ms };
                }
            }
            else
            {
                modelStates = htmlHelper.ViewData.ModelState.Values;
            }

            if (modelStates != null)
            {
                foreach (ModelState modelState in modelStates)
                {
                    foreach (ModelError modelError in modelState.Errors)
                    {
                        string errorText = GetUserErrorMessageOrDefault(htmlHelper.ViewContext.HttpContext, modelError, null /* modelState */);
                        if (!String.IsNullOrEmpty(errorText))
                        {
                            TagBuilder listItem = new TagBuilder("li");
                            listItem.InnerHtml = errorText;
                            htmlSummary.AppendLine(listItem.ToString(TagRenderMode.Normal));
                        }
                    }
                }
            }

            if (htmlSummary.Length == 0)
            {
                htmlSummary.AppendLine(@"<li style=""display:none""></li>");
            }

            unorderedList.InnerHtml = htmlSummary.ToString();

            TagBuilder divBuilder = new TagBuilder("div");
            divBuilder.AddCssClass((htmlHelper.ViewData.ModelState.IsValid) ? HtmlHelper.ValidationSummaryValidCssClassName : HtmlHelper.ValidationSummaryCssClassName);
            RouteValueDictionary attributes = new RouteValueDictionary(htmlAttributes);
            if (attributes.ContainsKey("class"))
                divBuilder.AddCssClass(attributes["class"].ToString());
            divBuilder.MergeAttributes(attributes);
            divBuilder.InnerHtml = messageSpan + unorderedList.ToString(TagRenderMode.Normal);

            if (formContext != null)
            {
                // client val summaries need an ID
                divBuilder.GenerateId("validationSummary");
                formContext.ValidationSummaryId = divBuilder.Attributes["id"];
                formContext.ReplaceValidationSummary = !excludePropertyErrors;
            }
            return MvcHtmlString.Create(divBuilder.ToString());
        }

        private static string GetUserErrorMessageOrDefault(HttpContextBase httpContext, ModelError error, ModelState modelState)
        {
            if (!String.IsNullOrEmpty(error.ErrorMessage))
            {
                return error.ErrorMessage;
            }
            if (modelState == null)
            {
                return null;
            }

            string attemptedValue = (modelState.Value != null) ? modelState.Value.AttemptedValue : null;
            return String.Format(CultureInfo.CurrentCulture, "The value {0} is invalid.", attemptedValue);
        }

        public static string GetDisplayName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {

            Type type = typeof(TModel);

            MemberExpression memberExpression = (MemberExpression)expression.Body;
            string propertyName = ((memberExpression.Member is PropertyInfo) ? memberExpression.Member.Name : null);

            // First look into attributes on a type and it's parents
            DisplayAttribute attr;
            attr = (DisplayAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

            // Look for [MetadataType] attribute in type hierarchy
            // http://stackoverflow.com/questions/1910532/attribute-isdefined-doesnt-see-attributes-applied-with-metadatatype-class
            if (attr == null)
            {
                MetadataTypeAttribute metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
                if (metadataType != null)
                {
                    var property = metadataType.MetadataClassType.GetProperty(propertyName);
                    if (property != null)
                    {
                        attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                    }
                }
            }
            return (attr != null) ? attr.Name : String.Empty;


        }
    }
}