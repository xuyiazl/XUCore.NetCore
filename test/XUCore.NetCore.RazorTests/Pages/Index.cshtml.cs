using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace XUCore.RazorTests.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TestDBContext _dbContext;
        public IndexModel(TestDBContext context)
        {
            _dbContext = context;
        }

        [BindProperty(SupportsGet = true)]
        public int id { get; set; }

        public IList<Customer> Customers;

        public async Task OnGetAsync()
        {
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
