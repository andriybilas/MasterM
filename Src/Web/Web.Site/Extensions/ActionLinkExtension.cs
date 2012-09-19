using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Web.Site
{
	public static class ActionLinkExtension
	{
		private static string GetScript(string linkText)
		{
			return @"<script type='text/javascript' language='javascript'>$('.type-submit-"+ linkText + "').click(function () {$(this).closest('form').submit(); return false; });</script>";
		}

        private static string GetCinfirmationEnabledScript(string linkText, string text)
        {
            return @"<script type='text/javascript' language='javascript'>$('.type-submit-confirm-" + linkText + "' ).click( function () { if(confirm('" + text + "')) { $(this).closest('form').submit(); } return false;  });</script>";
        }

        public static MvcHtmlString ActionLinkSubmit(this HtmlHelper htmlHelper, string linkText, string confirmationMessage = null, IDictionary<string, object> htmlAttributes = null)
        {
            TagBuilder builder2 = new TagBuilder("a")
            {
                InnerHtml = !string.IsNullOrEmpty(linkText) ? HttpUtility.HtmlEncode(linkText) : string.Empty
            };

            TagBuilder builder = builder2;

            if(htmlAttributes != null)
                builder.MergeAttributes<string, object>(htmlAttributes);
            
            builder.MergeAttribute("href", "#");

            string submitPrefix = linkText.Replace(' ', '-');

            if (string.IsNullOrEmpty(confirmationMessage))
                builder.AddCssClass(string.Format("type-submit-{0}", submitPrefix));
            else
                builder.AddCssClass(string.Format("type-submit-confirm-{0}", submitPrefix));

            string script = string.IsNullOrEmpty(confirmationMessage) ? GetScript(submitPrefix) : GetCinfirmationEnabledScript(submitPrefix, confirmationMessage);
            string result = string.Format("{0} {1}", builder.ToString(TagRenderMode.Normal), script);
            return MvcHtmlString.Create(result);            
        }

        public static MvcHtmlString ActionLinkSubmit(this HtmlHelper htmlHelper, string linkText, ButtonType type, string confirmationMessage = null, IDictionary<string, object> htmlAttributes = null)
        {
            TagBuilder builder2 = new TagBuilder("a");
            //builder2.AddCssClass("t-button t-grid-update t-button-icon");


            switch (type)
            {
                case ButtonType.Button:
                    {
                        builder2.InnerHtml = !string.IsNullOrEmpty(linkText) ? HttpUtility.HtmlEncode(linkText) : string.Empty;
                    }
                    break;
                case ButtonType.Image:
                    {
                        var span = new TagBuilder("span");
                        span.AddCssClass("t-icon t-update");
                        builder2.InnerHtml = span.ToString();
                        break;
                    }
                case ButtonType.Link:
                    {
                        break;
                    }
            }
           

            TagBuilder builder = builder2;

            if (htmlAttributes != null)
                builder.MergeAttributes<string, object>(htmlAttributes);

            builder.MergeAttribute("href", "#");
            
            string submitPrefix = linkText.Replace(' ', '-');

            if (string.IsNullOrEmpty(confirmationMessage))
                builder.MergeAttribute("class", string.Format("type-submit-{0} t-button t-grid-update t-button-icon", submitPrefix));
            else
                builder.MergeAttribute("class", string.Format("type-submit-confirm-{0} t-button t-grid-update t-button-icon", submitPrefix));

            htmlHelper.EnableClientValidation(true);
            //builder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(htmlHelper.ViewData.Model));

            string script = string.IsNullOrEmpty(confirmationMessage) ? GetScript(submitPrefix) : GetCinfirmationEnabledScript(submitPrefix, confirmationMessage);
            string result = string.Format("{0} {1}", builder.ToString(TagRenderMode.Normal), script);
            return MvcHtmlString.Create(result);          
            
        }
	}
}