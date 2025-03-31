using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WesternInn_AA_JH_SFP.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        //Foreign Key
        [Required]
        public int RoomID { get; set; }

        //Foreign Key
        [Required]
        [DataType(DataType.EmailAddress)]
        public string GuestEmail { get; set; }

        
        [DataType(DataType.Date)]
        [Column(TypeName="date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckIn { get; set; }

        
        [DataType(DataType.Date)]
        [Column(TypeName="date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckOut { get; set; }

        [Range(0, 10000)]
        [DataType(DataType.Currency)]
        public decimal Cost { get; set; }

        public Room? TheRoom { get; set; }

        public Guest? TheGuest { get; set; }

    }
}
