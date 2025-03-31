using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WesternInn_AA_JH_SFP.Data;
using WesternInn_AA_JH_SFP.Models;

namespace WesternInn_AA_JH_SFP.Pages.Bookings
{
    [Authorize(Roles = "guests")]
    public class MyBookingModel : PageModel
    {
        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public MyBookingModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; } = default!;

        public async Task OnGetAsync(string? sortOrder)
        {
            string _email = User.FindFirst(ClaimTypes.Name).Value;
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "checkIn_asc";
            }
            var bookings = (IQueryable<Booking>)_context.Booking;

            switch (sortOrder)
            {
                case "checkIn_asc":
                    bookings = bookings.OrderBy(p => p.CheckIn);
                    break;
                case "checkIn_desc":
                    bookings = bookings.OrderByDescending(p => p.CheckIn);
                    break;

                case "cost_asc":
                    bookings = bookings.OrderBy(p => (double)p.Cost);
                    break;
                case "cost_desc":
                    bookings = bookings.OrderByDescending(p => (double)p.Cost);
                    break;
            }
            ViewData["NextCheckInBooking"] = sortOrder != "checkIn_asc" ? "checkIn_asc" : "checkIn_desc";
            ViewData["NextCostBooking"] = sortOrder != "cost_asc" ? "cost_asc" : "cost_desc";

            Booking = await bookings.AsNoTracking()
                .Include(p => p.TheGuest)
                .Where(p => p.GuestEmail == _email).ToListAsync();
        }
    }
}
