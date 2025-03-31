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
    [Authorize(Roles = "guests")]
    public class SearchRoomModel : PageModel
    {
        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public SearchRoomModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        //Booking checkIn/checkOut dates
        public TwoBooking BookingInputs { get; set; }

        public IList<Room> DiffRooms { get;set; } = default!;

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
                var bedNums = new SqliteParameter("bedNums", BookingInputs.bedCount);

                // Construct the query to get the room booked by bookingA but not bookingB
                var diffRooms = _context.Room.FromSqlRaw("SELECT DISTINCT [Room].* from [Room] inner join [Booking] " +
                    "on [Room].ID = [Booking].RoomID where [Room].BedCount = @bedNums AND [Room].ID " +
                    "not in (SELECT [Room].ID FROM [Room] inner join [Booking] ON [Room].ID = [Booking].RoomID " +
                    "WHERE @checkIn < [Booking].CheckOut AND Booking.CheckIn < @checkOut)", checkIn, checkOut, bedNums);

                // Run the query and save the results in diffBooking for passing to content file
                DiffRooms = await diffRooms.ToListAsync();
                if (BookingInputs.CheckIn > BookingInputs.CheckOut || BookingInputs.CheckIn == BookingInputs.CheckOut)
                {
                    ViewData["Error"] = "Error";
                    return Page();
                }
                return Page();
            }
        
    }
}
