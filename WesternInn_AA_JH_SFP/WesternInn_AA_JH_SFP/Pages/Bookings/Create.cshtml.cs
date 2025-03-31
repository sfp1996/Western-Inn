using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class CreateModel : PageModel
    {
        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public CreateModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Room> CheckRooms { get; set; } = default!;

        public IActionResult OnGet()
        {
        ViewData["Email"] = new SelectList(_context.Guest, "Email", "FullName");
        ViewData["ID"] = new SelectList(_context.Room, "ID", "ID");
            return Page();
        }

        [BindProperty]
        public Booking? Reservation { get; set; }



        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // input the parameters to be inserted into the query
            var checkIn = new SqliteParameter("checkIn", Reservation.CheckIn);
            var checkOut = new SqliteParameter("checkOut", Reservation.CheckOut);
            var roomID = new SqliteParameter("roomID", Reservation.RoomID);

            // Construct the query to check room availibility for user to book a room
            var checkRooms = _context.Room.FromSqlRaw("SELECT [Room].* from [Room] inner join [Booking] " +
                "on [Room].ID = [Booking].RoomID where [Room].ID = @roomID AND [Room].ID " +
                "not in (SELECT [Room].ID FROM [Room] inner join [Booking] ON [Room].ID = [Booking].RoomID " +
                "WHERE @checkIn < [Booking].CheckOut AND Booking.CheckIn < @checkOut)", checkIn, checkOut, roomID);

            // Run the query and save the results in checkRooms for passing to content file
            CheckRooms = await checkRooms.ToListAsync();
            ViewData["ID"] = new SelectList(_context.Room, "ID", "ID");
            ViewData["Email"] = new SelectList(_context.Guest, "Email", "FullName");

            if(Reservation.CheckIn > Reservation.CheckOut || Reservation.CheckIn == Reservation.CheckOut)
            {
                ViewData["Error"] = "Error";
                return Page();
            }

            if (CheckRooms.Count == 0)
            {
                return Page();
            }
            else
            {

                // Retrieve the logged-in user's email
                Booking booking = new Booking();

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Add the booking details in the database
                var success = await TryUpdateModelAsync<Booking>(booking, "Reservation",
                                         s => s.BookingID, s => s.CheckIn, s => s.CheckOut, s => s.Cost, s => s.GuestEmail, s => s.RoomID);
                if (!success)
                {
                    return Page();
                }

                // Add new record into database
                _context.Booking.Add(booking);


                try  // catch the conflict of editing the record
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToPage("./ManageBooking");
            }
                
        }
    }
}
 