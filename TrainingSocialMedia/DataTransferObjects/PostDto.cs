namespace TrainingSocialMedia.DataTransferObjects;

public class PostDto
{
    public required int Id { get; set; }
    public required string AuthorUserName { get; set; }
    public required string Content { get; set; }
}