using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Razors;
using XUCore.NetCore.RazorTests;

namespace XUCore.RazorTests.Pages
{
    [HtmlStatic(MinInterval = 0, Template = "/static/{page}.html")]
    public class IndexModel : PageModel
    {
        private readonly TestDBContext _dbContext;
        private readonly ICacheTest cacheTest;
        public IndexModel(TestDBContext context, ICacheTest cacheTest)
        {
            _dbContext = context;
            this.cacheTest = cacheTest;
        }

        [BindProperty(SupportsGet = true)]
        public int id { get; set; }

        public IList<Customer> Customers;

        public async Task OnGetAsync()
        {
            cacheTest.GetPermission();
            Customers = await _dbContext.Customer.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var entity = await _dbContext.Customer.FirstOrDefaultAsync(c => c.Id == id);

            if (entity != null)
            {
                _dbContext.Customer.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToPage();
        }

    }
}
