using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Core.Entities
{
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        // FK to Hotel
        [ForeignKey(nameof(Hotel))]
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }

        // MxM with Reservation
        public ICollection<ReservationRoom> ReservationRooms { get; set; }
    }
}
