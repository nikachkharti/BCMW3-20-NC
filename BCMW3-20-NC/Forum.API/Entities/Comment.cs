using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.API.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CommentDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(Topic))]
        public Guid TopicId { get; set; }
        public Comment Topic { get; set; }
    }
}
