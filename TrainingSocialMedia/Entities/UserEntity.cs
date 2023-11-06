using Microsoft.AspNetCore.Identity;

namespace TrainingSocialMedia.Entities;

public class UserEntity : IdentityUser
{
    public ICollection<PostEntity> Posts { get; set; } = new List<PostEntity>();
}