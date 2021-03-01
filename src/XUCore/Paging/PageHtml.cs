using System;
using XUCore.Webs;

namespace XUCore.Paging
{
    /// <summary>
    /// page分页html
    /// </summary>
    public static class PageHtml
    {
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string BuildPageHtml<T>(this PagedModel<T> model, Func<int, string> urlBuilder)
        {
            if (model == null) return string.Empty;

            return Build(model.PageNumber, model.TotalPages, urlBuilder);
        }
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string BuildPageHtml<T>(this PagedModel<T> model, bool showFirstAndLastPage, Func<int, string> urlBuilder)
        {
            if (model == null) return string.Empty;

            return Build(model.PageNumber, model.TotalPages, showFirstAndLastPage, urlBuilder);
        }
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="numberPagesToDisplay">显示几页</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string BuildPageHtml<T>(this PagedModel<T> model, int numberPagesToDisplay, bool showFirstAndLastPage, Func<int, string> urlBuilder)
        {
            if (model == null) return string.Empty;

            return Build(model.PageNumber, model.TotalPages, numberPagesToDisplay, showFirstAndLastPage, PagerLanguage.Default, urlBuilder);
        }
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="numberPagesToDisplay"></param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="language"></param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string BuildPageHtml<T>(this PagedModel<T> model, int numberPagesToDisplay, bool showFirstAndLastPage, PagerLanguage language, Func<int, string> urlBuilder)
        {
            if (model == null) return string.Empty;

            return Build(model.PageNumber, model.TotalPages, numberPagesToDisplay, showFirstAndLastPage, language, urlBuilder);
        }

        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, Func<int, string> urlBuilder)
        {
            return Build(pageNumber, totalPages, 7, "current", string.Empty, true, PagerLanguage.Default, urlBuilder);
        }

        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, bool showFirstAndLastPage, Func<int, string> urlBuilder)
        {
            return Build(pageNumber, totalPages, 7, "current", string.Empty, showFirstAndLastPage, PagerLanguage.Default, urlBuilder);
        }

        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="numberPagesToDisplay">显示几页</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="language">语言</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, int numberPagesToDisplay, bool showFirstAndLastPage, PagerLanguage language, Func<int, string> urlBuilder)
        {
            return Build(pageNumber, totalPages, numberPagesToDisplay, "current", string.Empty, showFirstAndLastPage, language, urlBuilder);
        }

        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="numberPagesToDisplay">显示几页</param>
        /// <param name="cssClassForCurrentPage">显示当前页的css（current）</param>
        /// <param name="cssClassForPage">显示费当前页的css</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="language">语言</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, int numberPagesToDisplay,
            string cssClassForCurrentPage, string cssClassForPage, bool showFirstAndLastPage,
            PagerLanguage language, Func<int, string> urlBuilder)
        {
            Pager pager = new Pager(pageNumber, totalPages,
                new PagerSettings(numberPagesToDisplay, cssClassForCurrentPage,
                    cssClassForPage, showFirstAndLastPage, language));
            return pager.ToHtml(urlBuilder);
        }



        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string BuildPageHtml<T>(this PagedModel<T> model, Func<int, UrlBuilder> urlBuilder)
        {
            if (model == null) return string.Empty;

            return Build(model.PageNumber, model.TotalPages, urlBuilder);
        }
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string BuildPageHtml<T>(this PagedModel<T> model, bool showFirstAndLastPage, Func<int, UrlBuilder> urlBuilder)
        {
            if (model == null) return string.Empty;

            return Build(model.PageNumber, model.TotalPages, showFirstAndLastPage, urlBuilder);
        }
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="numberPagesToDisplay">显示几页</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string BuildPageHtml<T>(this PagedModel<T> model, int numberPagesToDisplay, bool showFirstAndLastPage, Func<int, UrlBuilder> urlBuilder)
        {
            if (model == null) return string.Empty;

            return Build(model.PageNumber, model.TotalPages, numberPagesToDisplay, showFirstAndLastPage, PagerLanguage.Default, urlBuilder);
        }
        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="numberPagesToDisplay"></param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="language"></param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string BuildPageHtml<T>(this PagedModel<T> model, int numberPagesToDisplay, bool showFirstAndLastPage, PagerLanguage language, Func<int, UrlBuilder> urlBuilder)
        {
            if (model == null) return string.Empty;

            return Build(model.PageNumber, model.TotalPages, numberPagesToDisplay, showFirstAndLastPage, language, urlBuilder);
        }


        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, Func<int, UrlBuilder> urlBuilder)
        {
            return Build(pageNumber, totalPages, 7, "current", string.Empty, true, PagerLanguage.Default, urlBuilder);
        }

        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, bool showFirstAndLastPage, Func<int, UrlBuilder> urlBuilder)
        {
            return Build(pageNumber, totalPages, 7, "current", string.Empty, showFirstAndLastPage, PagerLanguage.Default, urlBuilder);
        }

        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="numberPagesToDisplay">显示几页</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="language">语言</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, int numberPagesToDisplay, bool showFirstAndLastPage, PagerLanguage language, Func<int, UrlBuilder> urlBuilder)
        {
            return Build(pageNumber, totalPages, numberPagesToDisplay, "current", string.Empty, showFirstAndLastPage, language, urlBuilder);
        }

        /// <summary>
        /// web 分页HTML
        /// </summary>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="totalPages">总页数</param>
        /// <param name="numberPagesToDisplay">显示几页</param>
        /// <param name="cssClassForCurrentPage">显示当前页的css（current）</param>
        /// <param name="cssClassForPage">显示费当前页的css</param>
        /// <param name="showFirstAndLastPage">是否显示第一页和最后一页</param>
        /// <param name="language">语言</param>
        /// <param name="urlBuilder">url规则</param>
        /// <returns></returns>
        public static string Build(int pageNumber, int totalPages, int numberPagesToDisplay,
            string cssClassForCurrentPage, string cssClassForPage, bool showFirstAndLastPage,
            PagerLanguage language, Func<int, UrlBuilder> urlBuilder)
        {
            Pager pager = new Pager(pageNumber, totalPages,
                new PagerSettings(numberPagesToDisplay, cssClassForCurrentPage,
                    cssClassForPage, showFirstAndLastPage, language));
            return pager.ToHtml((ndx) => urlBuilder.Invoke(ndx).ToString());
        }
    }
}