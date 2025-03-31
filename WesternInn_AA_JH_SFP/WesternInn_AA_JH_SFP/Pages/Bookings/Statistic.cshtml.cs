using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WesternInn_AA_JH_SFP.Models;


namespace WesternInn_AA_JH_SFP.Pages.Bookings
{
    [Authorize(Roles = "administrators")]
    public class StatisticModel : PageModel
    {
        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public StatisticModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<GuestPostcodeStat> GuestPostcodeStats { get; set; } = default!;

        public IList<RoomBookStat> RoomBookStats { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            //Split into groups based on the postcode
            var postcodeGroups = _context.Guest.GroupBy(p => p.PostCode);

            // Get the amount of guests that live in the specified postcode
            GuestPostcodeStats = await postcodeGroups
                             .Select(b => new GuestPostcodeStat {Postcode = b.Key, GuestPostAmount = b.Count()})
                             .ToListAsync();

            //Split into groups based on the room
            var roomGroups = _context.Booking.GroupBy(p => p.RoomID);

            // for each group, get its quantity and the amount of time its booked
            RoomBookStats = await roomGroups
                             .Select(b => new RoomBookStat { RoomID = b.Key, BookRoomNum = b.Count() })
                             .ToListAsync();

            return Page();
        }
    }
}
