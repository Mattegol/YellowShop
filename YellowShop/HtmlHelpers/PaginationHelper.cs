using System;
using System.Text;
using System.Web.Mvc;
using YellowShop.Models;

namespace YellowShop.HtmlHelpers
{
    public static class PaginationHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper htmlHelper,
                                              PaginationModel pagination,
                                              Func<int, string> pageUrl)
        {

            var result = new StringBuilder();
            for (var i = 1; i <= pagination.TotalPages; i++)
            {
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagination.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag);
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}