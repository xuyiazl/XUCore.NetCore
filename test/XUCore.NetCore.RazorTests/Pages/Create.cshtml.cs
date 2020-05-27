using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XUCore.RazorTests
{
    public class CreateModel : PageModel
    {
        TestDBContext _dbContext;
        public CreateModel(TestDBContext context)
        {
            _dbContext = context;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _dbContext.AddAsync(Customer);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return RedirectToPage("index");
            }

            return Page();
        }
    }
}