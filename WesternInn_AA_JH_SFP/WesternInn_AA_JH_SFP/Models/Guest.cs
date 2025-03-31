using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WesternInn_AA_JH_SFP.Models
{
    public class Guest
    {
        [Key, Required]
        [DataType(DataType.EmailAddress)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Surname")]
        [RegularExpression(@"[A-Z][a-z'-]{2,20}")]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Given Name")]
        [RegularExpression(@"[A-Z][a-z'-]{2,20}")]
        public string GivenName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Post Code")]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"[0-8][0-9]{3}")]
        public string PostCode { get; set; } = String.Empty;

        public ICollection<Booking>? TheBookings { get; set; }

        [NotMapped]
        public string FullName => $"{GivenName} {Surname}";

    }
}

