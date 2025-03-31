using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WesternInn_AA_JH_SFP.Data;
using WesternInn_AA_JH_SFP.Models;

namespace WesternInn_AA_JH_SFP.Pages.Bookings
{
    [Authorize(Roles = "administrators")]
    public class ManageBookingModel : PageModel
    {
        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public ManageBookingModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Booking != null)
            {
                Booking = await _context.Booking
                .Include(b => b.TheGuest)
                .Include(b => b.TheRoom).ToListAsync();
            }
        }
    }
}
