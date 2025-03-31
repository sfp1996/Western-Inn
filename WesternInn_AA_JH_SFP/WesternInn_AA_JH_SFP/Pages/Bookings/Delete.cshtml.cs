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
    public class DeleteModel : PageModel
    {
        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public DeleteModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Booking Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Booking == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(m => m.TheGuest)
                .Include(m => m.TheRoom)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }
            else 
            {
                Booking = booking;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Booking == null)
            {
                return NotFound();
            }
            var booking = await _context.Booking.FindAsync(id);

            if (booking != null)
            {
                Booking = booking;
                _context.Booking.Remove(Booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./ManageBooking");
        }
    }
}
