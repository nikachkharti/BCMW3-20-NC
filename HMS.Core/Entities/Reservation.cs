using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Core.Entities
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime CheckinDate { get; set; }

        [Required]
        public DateTime CheckoutDate { get; set; }

        public Guid GuestId { get; set; }
    }
}
