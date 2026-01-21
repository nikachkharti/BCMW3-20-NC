using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.API.Entities
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public string ImageUrl { get; set; }

        [Column(TypeName = "Date")]
        public DateTime LastCommentDate { get; set; }

        public bool CommentsAreAllowed { get; set; } = true;
    }
}
