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

        //Guest (ApplicationUser)
        [ForeignKey(nameof(Guest))]
        public string GuestId { get; set; }
        public ApplicationUser Guest { get; set; }

        //MxM with Room
        public ICollection<ReservationRoom> ReservationRooms { get; set; }
    }
}
