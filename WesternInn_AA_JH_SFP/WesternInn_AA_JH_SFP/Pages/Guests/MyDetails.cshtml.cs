using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WesternInn_AA_JH_SFP.Data;
using WesternInn_AA_JH_SFP.Models;

namespace WesternInn_AA_JH_SFP.Pages.Guests
{
    [Authorize(Roles = "guests")]
    public class MyDetailsModel : PageModel
    {
        private readonly WesternInn_AA_JH_SFP.Data.ApplicationDbContext _context;

        public MyDetailsModel(WesternInn_AA_JH_SFP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guest? Person { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Confirm logged in users
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            Guest guest = await _context.Guest.FirstOrDefaultAsync(m => m.Email == _email);

            if (guest != null)
            {
                // existing user
                ViewData["ExistInDB"] = "true";
                Person = guest;
            }
            else
            {
                // new user
                ViewData["ExistInDB"] = "false";
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // Retrieve the logged-in user's email
            string _email = User.FindFirst(ClaimTypes.Name).Value;

            // Retrieve this guest from the database
            Guest guest = await _context.Guest.FirstOrDefaultAsync(m => m.Email == _email);

            if (guest != null)
            {
                // Existing user
                ViewData["ExistInDB"] = "true";
            }
            else
            {
                // New user
                ViewData["ExistInDB"] = "false";
                guest = new Guest();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            guest.Email = _email;

            // Add the Guest details in the database
            var success = await TryUpdateModelAsync<Guest>(guest, "Person",
                                s => s.Surname, s => s.GivenName, s => s.PostCode);
            if (!success)
            {
                return Page();
            }

            if ((string)ViewData["ExistInDB"] == "true")
            {
                // Update existing record in the database
                _context.Guest.Update(guest);
            }
            else
            {
                // Add new record into database
                _context.Guest.Add(guest);
            }

            try  // catch the conflict of editing the record
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            ViewData["SuccessDB"] = "success";
            return Page();
        }
    }
}
