namespace TrainingSocialMedia.Data;

public class Post
{
    public int Id { get; set; }
    public ApplicationUser Author { get; set; } = null!;
    public string Content { get; set; } = null!;
}