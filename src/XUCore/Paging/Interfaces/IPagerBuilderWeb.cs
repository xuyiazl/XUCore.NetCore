namespace XUCore.Paging
{
    /********************************************************************
    *           Copyright:       2009-2010
    *           Company:
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2010/12/12 12:45:55
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;

    /// <summary>
    /// Pager url mode builder interface.
    /// </summary>
    public interface IPagerBuilderWeb
    {
        /// <summary>
        /// Builds the entire html for the specified page index.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="urlBuilder">The lamda to build the url for a specific page</param>
        string Build(int pageIndex, int totalPages, Func<int, string> urlBuilder);

        /// <summary>
        /// Builds the entire html for the specified page index.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="settings">The settings for the pager</param>
        /// <param name="urlBuilder">The lamda to build the url for a specific page</param>
        string Build(int pageIndex, int totalPages, PagerSettings settings, Func<int, string> urlBuilder);

        /// <summary>
        /// Build the entire html for the pager.
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="settings"></param>
        /// <param name="urlBuilder">The lamda to build the url for a specific page</param>
        /// <returns></returns>
        string Build(Pager pager, PagerSettings settings, Func<int, string> urlBuilder);
    }
}