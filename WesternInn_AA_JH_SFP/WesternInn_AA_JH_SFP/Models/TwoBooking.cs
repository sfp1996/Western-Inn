using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WesternInn_AA_JH_SFP.Models
{
    public class TwoBooking
    {
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckIn { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        public int bedCount { get; set; }

        public int roomID { get; set; }


    }
}
