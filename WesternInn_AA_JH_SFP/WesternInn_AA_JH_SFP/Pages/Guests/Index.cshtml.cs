using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WesternInn_AA_JH_SFP.Data;
using WesternInn_AA_JH_SFP.Models;

namespace WesternInn_AA_JH_SFP.Pages.Guests
{
    public class IndexModel : PageModel
    {
        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public IndexModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Guest> Guest { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Guest != null)
            {
                Guest = await _context.Guest.ToListAsync();
            }
        }
    }
}
