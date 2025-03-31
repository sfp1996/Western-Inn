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
    [Authorize(Roles = "guests")]
    public class BookRoomModel : PageModel
    {

        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public BookRoomModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Room> CheckRooms { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public TwoBooking BookingInputs { get; set; }
        public IActionResult OnGetAsync()
        {
            ViewData["ID"] = new SelectList(_context.Room, "ID", "ID");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            ViewData["checkBooking"] = "checked";
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // input the parameters to be inserted into the query
            var checkIn = new SqliteParameter("checkIn", BookingInputs.CheckIn);
            var checkOut = new SqliteParameter("checkOut", BookingInputs.CheckOut);
            var roomID = new SqliteParameter("roomID", BookingInputs.roomID);

            // Construct the query to check room availibility for user to book a room
            var checkRooms = _context.Room.FromSqlRaw("SELECT [Room].* from [Room] inner join [Booking] " +
                "on [Room].ID = [Booking].RoomID where [Room].ID = @roomID AND [Room].ID " +
                "not in (SELECT [Room].ID FROM [Room] inner join [Booking] ON [Room].ID = [Booking].RoomID " +
                "WHERE @checkIn < [Booking].CheckOut AND Booking.CheckIn < @checkOut)", checkIn, checkOut, roomID);

            // Run the query and save the results in checkRooms for passing to content file
            CheckRooms = await checkRooms.ToListAsync();
            ViewData["ID"] = new SelectList(_context.Room, "ID", "ID");

            if (BookingInputs.CheckIn > BookingInputs.CheckOut || BookingInputs.CheckIn == BookingInputs.CheckOut)
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
                string _email = User.FindFirst(ClaimTypes.Name).Value;

               
                Booking booking = new Booking();

                if (!ModelState.IsValid)
                {
                    return Page();
                }
                


                booking.GuestEmail = _email;
                //Get amount of nights
                TimeSpan difference = BookingInputs.CheckOut - BookingInputs.CheckIn;
                int nights = difference.Days;
                //Continue here
                booking.Cost = CheckRooms.FirstOrDefault().Price * (decimal)nights;
                booking.CheckOut = BookingInputs.CheckOut;
                booking.CheckIn = BookingInputs.CheckIn;
                booking.RoomID = BookingInputs.roomID;


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
                ViewData["TotalCost"] = booking.Cost;
                ViewData["SuccessDB"] = "success";
                return Page();
            }
        }

    }

}


