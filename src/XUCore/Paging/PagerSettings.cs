namespace XUCore.Paging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/12 12:45:40
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    public class PagerLanguage
    {
        public static readonly PagerLanguage Default = new PagerLanguage("First", "&#171;", "&#187;", "Last");

        public PagerLanguage(string first, string prev, string next, string last)
        {
            First = first;
            Prev = prev;
            Next = next;
            Last = last;
        }

        public string First = "First";
        public string Prev = "Prev";
        public string Next = "Next";
        public string Last = "Last";
    }

    /// <summary>
    /// Pager settings.
    /// </summary>
    public class PagerSettings
    {
        /// <summary>
        /// Default settings
        /// </summary>
        public static readonly PagerSettings Default = new PagerSettings(7, "current", "", false, PagerLanguage.Default);

        /// <summary>
        /// Default construction
        /// </summary>
        public PagerSettings() { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="numberPagesToDisplay"></param>
        /// <param name="cssClassForCurrentPage"></param>
        /// <param name="cssClassForPage"></param>
        /// <param name="showFirstAndLastPage"></param>
        /// <param name="language"></param>
        public PagerSettings(int numberPagesToDisplay, string cssClassForCurrentPage,
            string cssClassForPage, bool showFirstAndLastPage, PagerLanguage language)
        {
            NumberPagesToDisplay = numberPagesToDisplay;
            CssCurrentPage = cssClassForCurrentPage;
            CssClass = cssClassForPage;
            ShowFirstAndLastPage = showFirstAndLastPage;
            Language = language;
        }

        /// <summary>
        /// How many pages to display in a row at once.
        /// </summary>
        public int NumberPagesToDisplay = 5;

        /// <summary>
        /// Name of css class used for currently displayed page.
        /// </summary>
        public string CssCurrentPage = string.Empty;

        /// <summary>
        /// Name of css class used for showing normal non-current pages.
        /// </summary>
        public string CssClass = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public bool ShowFirstAndLastPage = false;

        /// <summary>
        ///
        /// </summary>
        public PagerLanguage Language = PagerLanguage.Default;
    }
}