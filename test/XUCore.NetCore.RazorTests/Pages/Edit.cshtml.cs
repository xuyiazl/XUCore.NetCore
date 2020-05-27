using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace XUCore.RazorTests
{
    public class EditModel : PageModel
    {
        TestDBContext _dbContext;
        public EditModel(TestDBContext context)
        {
            _dbContext = context;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _dbContext.Customer.FirstOrDefaultAsync(c => c.Id == id);
            if (Customer == null)
                return RedirectToPage("index");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbContext.Attach(Customer).State = EntityState.Modified;

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return RedirectToPage("index");
            }

            return Page();
        }
    }
}