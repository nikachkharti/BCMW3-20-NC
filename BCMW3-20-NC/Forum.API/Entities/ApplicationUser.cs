using Microsoft.AspNetCore.Identity;

namespace Forum.API.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Topic> Topics { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
