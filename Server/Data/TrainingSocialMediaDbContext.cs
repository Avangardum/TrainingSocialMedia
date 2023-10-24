using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TrainingSocialMedia.Server.Data;

public class TrainingSocialMediaDbContext : IdentityDbContext<User>
{
    public TrainingSocialMediaDbContext(DbContextOptions<TrainingSocialMediaDbContext> options) : base(options) { }
}