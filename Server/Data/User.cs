using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TrainingSocialMedia.Server.Data;

public class User : IdentityUser
{
    [MaxLength(50)]
    public override string Id { get; set; } = null!;
}