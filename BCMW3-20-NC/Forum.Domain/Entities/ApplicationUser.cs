using Microsoft.AspNetCore.Identity;

namespace Forum.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Topic> Topics { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
