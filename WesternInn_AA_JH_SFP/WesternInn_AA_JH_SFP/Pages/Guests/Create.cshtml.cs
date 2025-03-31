using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WesternInn_AA_JH_SFP.Data;
using WesternInn_AA_JH_SFP.Models;

namespace WesternInn_AA_JH_SFP.Pages.Guests
{
    public class CreateModel : PageModel
    {
        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public CreateModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Guest Guest { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Guest.Add(Guest);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
