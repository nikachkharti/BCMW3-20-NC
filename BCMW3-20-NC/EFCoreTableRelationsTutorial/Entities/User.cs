using System.ComponentModel.DataAnnotations;

namespace EFCoreTableRelationsTutorial.Entities
{
    public class User
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Required]
        //[MaxLength(100)]
        public string Name { get; set; }

        public UserProfile UserProfile { get; set; } //1x1
    }
}
