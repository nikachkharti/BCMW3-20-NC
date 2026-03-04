using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Core.Entities
{
    public class Hotel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, 5)]
        public byte Rating { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        //Manager Relationship (FK)
        [ForeignKey(nameof(Manager))]
        public string ManagerId { get; set; }
        public ApplicationUser Manager { get; set; }

        //Hotel → Rooms (1xM)
        public ICollection<Room> Rooms { get; set; }
    }
}
