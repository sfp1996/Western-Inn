using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WesternInn_AA_JH_SFP.Data;
using WesternInn_AA_JH_SFP.Models;

namespace WesternInn_AA_JH_SFP.Pages.Bookings
{
    [Authorize(Roles = "administrators")]
    public class EditModel : PageModel
    {
        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public EditModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking Booking { get; set; } = default!;
        public IList<Room> CheckRooms { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Booking == null)
            {
                return NotFound();
            }

            var booking =  await _context.Booking.FirstOrDefaultAsync(m => m.BookingID == id);
            if (booking == null)
            {
                return NotFound();
            }
            Booking = booking;
            ViewData["Email"] = new SelectList(_context.Guest, "Email", "FullName");
            ViewData["ID"] = new SelectList(_context.Room, "ID", "ID");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // input the parameters to be inserted into the query
            var checkIn = new SqliteParameter("checkIn", Booking.CheckIn);
            var checkOut = new SqliteParameter("checkOut", Booking.CheckOut);
            var roomID = new SqliteParameter("roomID", Booking.RoomID);
            var bookingID = new SqliteParameter("bookingID", Booking.BookingID);

            // Construct the query to check room availibility for user to book a room
            var checkRooms = _context.Room.FromSqlRaw("SELECT [Room].* from [Room] inner join [Booking] " +
                "on [Room].ID = [Booking].RoomID where [Room].ID = @roomID AND [Room].ID " +
                "not in (SELECT [Room].ID FROM [Room] inner join [Booking] ON [Room].ID = [Booking].RoomID " +
                "WHERE @checkIn < [Booking].CheckOut AND Booking.CheckIn < @checkOut EXCEPT SELECT [Room].ID FROM [Room] " +
                "inner join [Booking] on Room.ID = Booking.RoomID WHERE [Booking].BookingID = @bookingID)", checkIn, checkOut, roomID, bookingID);

            // Run the query and save the results in checkRooms for passing to content file
            CheckRooms = await checkRooms.ToListAsync();
            ViewData["ID"] = new SelectList(_context.Room, "ID", "ID");
            ViewData["Email"] = new SelectList(_context.Guest, "Email", "FullName");

            if (Booking.CheckIn > Booking.CheckOut || Booking.CheckIn == Booking.CheckOut)
            {
                ViewData["Error"] = "Error";
                return Page();
            }

            if (CheckRooms.Count == 0)
            {
                return Page();
            }

            _context.Attach(Booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(Booking.BookingID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./ManageBooking");
        }

        private bool BookingExists(int id)
        {
          return _context.Booking.Any(e => e.BookingID == id);
        }
    }
}
