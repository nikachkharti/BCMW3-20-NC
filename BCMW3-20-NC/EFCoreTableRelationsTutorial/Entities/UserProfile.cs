using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreTableRelationsTutorial.Entities
{
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Address { get; set; }


        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; } //1x1
    }
}
