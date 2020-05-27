namespace XUCore.Paging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/12 12:46:09
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Text;

    /// <summary>
    /// Buider class that builds the pager in Url mode.
    /// </summary>
    public class PagerBuilderWeb : IPagerBuilderWeb
    {
        private static IPagerBuilderWeb _instance;
        private static readonly object _syncRoot = new object();

        /// <summary>
        /// Get singleton instance.
        /// </summary>
        /// <returns></returns>
        public static IPagerBuilderWeb Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new PagerBuilderWeb();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Builds the entire html for the specified page index / total pages.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="urlBuilder">The URL builder.</param>
        /// <returns></returns>
        public string Build(int pageIndex, int totalPages, Func<int, string> urlBuilder)
        {
            Pager pager = Pager.Get(pageIndex, totalPages, PagerSettings.Default);
            return Build(pager, PagerSettings.Default, urlBuilder);
        }

        /// <summary>
        /// Builds the entire html for the specified page index / total pages.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="settings">The settings for the pager.</param>
        /// <param name="urlBuilder">The URL builder.</param>
        /// <returns></returns>
        public string Build(int pageIndex, int totalPages, PagerSettings settings, Func<int, string> urlBuilder)
        {
            Pager pager = Pager.Get(pageIndex, totalPages, settings);
            return Build(pager, pager.Settings, urlBuilder);
        }

        /// <summary>
        /// Build the entire html for the pager.
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="settings"></param>
        /// <param name="urlBuilder"></param>
        /// <returns></returns>
        public string Build(Pager pager, PagerSettings settings, Func<int, string> urlBuilder)
        {
            Pager pagerData = pager;

            // Get reference to the default or custom url builder for this pager.
            StringBuilder buffer = new StringBuilder();
            string cssClass = string.Empty;
            string urlParams = string.Empty;
            string url = string.Empty;

            string cssClassForPage = string.IsNullOrEmpty(settings.CssClass) ? string.Empty : " class=\"" + settings.CssClass + "\"";

            // Build the starting page link.
            if (pagerData.CanShowFirst && settings.ShowFirstAndLastPage)
            {
                // First
                url = urlBuilder(1);
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + settings.Language.First + "</a></li>");

                // This is to avoid putting ".." between 1 and 2 for example.
                // If 1 is the starting page and we want to display 2 as starting page.
                if (pagerData.CanShowPrevious)
                {
                    buffer.Append("&nbsp;&nbsp;&nbsp;");
                }
            }

            if (pagerData.CurrentPage > 1)
            {
                url = urlBuilder(pagerData.CurrentPage - 1);
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + settings.Language.Prev + "</a></li>");
            }
            // Build the previous page link.
            if (pagerData.CanShowPrevious)
            {
                // Previous
                url = urlBuilder(pagerData.StartingPage - 1);
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + "..." + "</a></li>");
            }
            // Each page number.
            for (int ndx = pagerData.StartingPage; ndx <= pagerData.EndingPage; ndx++)
            {
                cssClass = (ndx == pagerData.CurrentPage) ? " class=\"" + settings.CssCurrentPage + "\"" : cssClassForPage;
                url = urlBuilder(ndx);

                url = (ndx == pagerData.CurrentPage) ? string.Empty : " href=\"" + url + "\"";

                // Build page number link. <a href="<%=Url %>" class="<%=cssClass %>" ><%=ndx %></a>
                buffer.Append("<li" + cssClass + "><a" + url + ">" + ndx + "</a></li>");
            }

            // Build the next page link.
            if (pagerData.CanShowNext)
            {
                // Previous
                url = urlBuilder(pagerData.EndingPage + 1);
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + "..." + "</a></li>");
            }
            if (pagerData.CurrentPage < pagerData.TotalPages)
            {
                url = urlBuilder(pagerData.CurrentPage + 1);
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + settings.Language.Next + "</a></li>");
            }
            // Build the  ending page link.
            if (pagerData.CanShowLast && settings.ShowFirstAndLastPage)
            {
                url = urlBuilder(pagerData.TotalPages);

                // This is to avoid putting ".." between 7 and 8 for example.
                // If 7 is the ending page and we want to display 8 as total pages.
                if (pagerData.CanShowNext)
                {
                    buffer.Append("&nbsp;&nbsp;&nbsp;");
                }
                buffer.Append("<li" + cssClassForPage + "><a href=\"" + url + "\">" + settings.Language.Last + "</a></li>");
            }
            string content = buffer.ToString();
            return content;
        }
    }
}