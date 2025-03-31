using System.ComponentModel.DataAnnotations;

namespace WesternInn_AA_JH_SFP.Models
{
    public class RoomBookStat
    {
        [Display(Name="Room ID")]
        public int RoomID { get; set; }

        [Display(Name = "Number of Bookings")]
        public int BookRoomNum { get; set; }
    }
}
