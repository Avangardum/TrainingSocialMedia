namespace TrainingSocialMedia.Entities;

public class PostEntity
{
    public int Id { get; set; }
    public required ApplicationUser Author { get; set; }
    public required string Content { get; set; }
}