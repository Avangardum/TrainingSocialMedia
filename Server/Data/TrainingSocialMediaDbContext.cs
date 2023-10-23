using Microsoft.EntityFrameworkCore;

namespace TrainingSocialMedia.Server.Data;

public class TrainingSocialMediaDbContext : DbContext
{
    public TrainingSocialMediaDbContext(DbContextOptions<TrainingSocialMediaDbContext> options) : base(options) { }
}