using System.ComponentModel.DataAnnotations;

namespace WesternInn_AA_JH_SFP.Models
{
    public class GuestPostcodeStat
    {
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [Display(Name = "Number of Guests")]
        public int GuestPostAmount { get; set; }
    }
}
