using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Domain.Entities
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
        public string ImagePublicId { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? LastCommentDate { get; set; } = null;

        public bool CommentsAreAllowed { get; set; } = true;

        [Required]
        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
