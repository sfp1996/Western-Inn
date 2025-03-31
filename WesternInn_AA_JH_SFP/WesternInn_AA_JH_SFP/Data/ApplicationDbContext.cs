using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WesternInn_AA_JH_SFP.Models;

namespace WesternInn_AA_JH_SFP.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WesternInn_AA_JH_SFP.Models.Room> Room { get; set; }
        public DbSet<WesternInn_AA_JH_SFP.Models.Guest> Guest { get; set; }
        public DbSet<WesternInn_AA_JH_SFP.Models.Booking> Booking { get; set; }
    }
}