using System.ComponentModel.DataAnnotations;

namespace WesternInn_AA_JH_SFP.Models
{
    public class Room
    {
        [Key, Required]
        public int ID { get; set; }
        [Required]
        [RegularExpression(@"^[1-3G]$")]
        public string Level { get; set; } = string.Empty;
        [Range(1,3)]
        public int BedCount { get; set; }
        [Range(50,300)]
        public decimal Price { get; set; }
        public ICollection<Booking>? TheBookings { get; set; }
    }
}
