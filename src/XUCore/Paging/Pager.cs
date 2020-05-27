namespace XUCore.Paging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/12 12:44:36
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;

    /// <summary>
    /// Holds the paging data.
    /// </summary>
    public partial class Pager : ICloneable
    {
        private int _currentPage;
        private int _totalPages;
        private int _previousPage;
        private int _startingPage;
        private int _endingPage;
        private int _nextPage;
        private PagerSettings _pagerSettings;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Pager()
            : this(1, 1, PagerSettings.Default)
        {
        }

        /// <summary>
        /// Constructor to set properties.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalPages"></param>
        public Pager(int currentPage, int totalPages)
            : this(currentPage, totalPages, PagerSettings.Default)
        {
        }

        /// <summary>
        /// Constructor to set properties.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalPages"></param>
        /// <param name="settings"></param>
        public Pager(int currentPage, int totalPages, PagerSettings settings)
        {
            _pagerSettings = settings;
            SetCurrentPage(currentPage, totalPages);
        }

        private static IPagerCalculator _instance = new PagerCalculator();
        private static readonly object _syncRoot = new object();

        /// <summary>
        /// Initialize pager calculator.
        /// </summary>
        /// <param name="pager"></param>
        public void Init(IPagerCalculator pager)
        {
            _instance = pager;
        }

        #region Data Members

        /// <summary>
        /// Set the current page and calculate the rest of the pages.
        /// </summary>
        /// <param name="currentPage"></param>
        public void SetCurrentPage(int currentPage)
        {
            SetCurrentPage(currentPage, _totalPages);
        }

        /// <summary>
        /// Set the current page and calculate the rest of the pages.
        /// </summary>
        /// <param name="currentPage">Current page</param>
        /// <param name="totalPages">Total pages in pager</param>
        public void SetCurrentPage(int currentPage, int totalPages)
        {
            if (totalPages < 0) totalPages = 1;
            if (currentPage < 0 || currentPage > totalPages) currentPage = 1;

            _currentPage = currentPage;
            _totalPages = totalPages;
            Calculate();
        }

        /// <summary>
        /// Current page
        /// </summary>
        public int CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        /// <summary>
        /// Total pages available
        /// </summary>
        public int TotalPages
        {
            get { return _totalPages; }
            set { _totalPages = value; }
        }

        /// <summary>
        /// Always 1.
        /// </summary>
        public int FirstPage { get { return 1; } }

        /// <summary>
        /// What is the previous page number if applicable.
        /// </summary>
        public int PreviousPage
        {
            get { return _previousPage; }
            set { _previousPage = value; }
        }

        /// <summary>
        /// Starting page.
        /// e.g.
        /// can be 1 as in                    1, 2, 3, 4, 5   next, last
        /// can be 6 as in   first, previous, 6, 7, 8, 9, 10  next, last
        /// </summary>
        public int StartingPage
        {
            get { return _startingPage; }
            set { _startingPage = value; }
        }

        /// <summary>
        /// Starting page.
        /// e.g.
        /// can be 5 as in                     1, 2, 3, 4, 5   next, last
        /// can be 10 as in   first, previous, 6, 7, 8, 9, 10  next, last
        /// </summary>
        public int EndingPage
        {
            get { return _endingPage; }
            set { _endingPage = value; }
        }

        /// <summary>
        /// What is the next page number if applicable.
        /// </summary>
        public int NextPage
        {
            get { return _nextPage; }
            set { _nextPage = value; }
        }

        /// <summary>
        /// Last page number is always the Total pages.
        /// </summary>
        public int LastPage { get { return _totalPages; } }

        /// <summary>
        /// Whether or not there are more than 1 page.
        /// </summary>
        public bool IsMultiplePages
        {
            get { return _totalPages > 1; }
        }

        /// <summary>
        /// Get the pager settings.
        /// </summary>
        public PagerSettings Settings
        {
            get { return _pagerSettings; }
            set { _pagerSettings = value; }
        }

        #endregion Data Members

        #region Navigation Checks

        /// <summary>
        /// Can show First page link?
        /// </summary>
        public bool CanShowFirst
        {
            get { return (_startingPage != 1); }
        }

        /// <summary>
        /// Can show previous link?
        /// </summary>
        public bool CanShowPrevious
        {
            get { return (_startingPage > 2); }
        }

        /// <summary>
        /// Can show Next page link?
        /// </summary>
        public bool CanShowNext
        {
            get { return (_endingPage < (_totalPages - 1)); }
        }

        /// <summary>
        /// Can show Last page link?
        /// </summary>
        public bool CanShowLast
        {
            get { return (_endingPage != _totalPages); }
        }

        #endregion Navigation Checks

        #region Navigation

        /// <summary>
        /// Move to the fist page.
        /// </summary>
        public void MoveFirst()
        {
            _currentPage = 1;
            Calculate();
        }

        /// <summary>
        /// Move to the previous page.
        /// </summary>
        public void MovePrevious()
        {
            _currentPage = _previousPage;
            Calculate();
        }

        /// <summary>
        /// Move to the next page.
        /// </summary>
        public void MoveNext()
        {
            _currentPage = _nextPage;
            Calculate();
        }

        /// <summary>
        /// Move to the last page.
        /// </summary>
        public void MoveLast()
        {
            _currentPage = _totalPages;
            Calculate();
        }

        /// <summary>
        /// Move to a specific page.
        /// </summary>
        /// <param name="selectedPage"></param>
        public void MoveToPage(int selectedPage)
        {
            _currentPage = selectedPage;
            Calculate();
        }

        #endregion Navigation

        #region Calculation

        /// <summary>
        /// Calcuate pages.
        /// </summary>
        public void Calculate()
        {
            Calculate(this, _pagerSettings);
        }

        /// <summary>
        /// Calculate the starting page and ending page.
        /// </summary>
        /// <param name="pagerData"></param>
        /// <param name="pagerSettings"></param>
        public static void Calculate(Pager pagerData, PagerSettings pagerSettings)
        {
            _instance.Calculate(pagerData, pagerSettings);
        }

        #endregion Calculation

        #region Html Generation

        /// <summary>
        /// Builds the html for non-ajax based url based paging.
        /// </summary>
        /// <param name="urlBuilder"></param>
        /// <returns></returns>
        public string ToHtml(Func<int, string> urlBuilder)
        {
            string html = PagerBuilderWeb.Instance.Build(this, this.Settings, urlBuilder);
            return html;
        }

        /// <summary>
        /// Builds the html for non-ajax based url based paging.
        /// </summary>
        /// <param name="urlBuilder"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public string ToHtml(Func<int, string> urlBuilder, PagerSettings settings)
        {
            string html = PagerBuilderWeb.Instance.Build(this, settings, urlBuilder);
            return html;
        }

        #endregion Html Generation

        #region Helper Methods

        /// <summary>
        /// Get the pager data using current page and totalPages.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalPages"></param>
        /// <param name="settings"></param>
        public static Pager Get(int currentPage, int totalPages, PagerSettings settings)
        {
            Pager data = new Pager(currentPage, totalPages, settings);
            return data;
        }

        #endregion Helper Methods

        #region ICloneable Members

        /// <summary>
        /// Clones the object.
        /// Good as long as properties are not objects.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion ICloneable Members
    }
}