namespace TrainingSocialMedia.DataTransferObjects;

public class PostBusinessModel
{
    public required int Id { get; set; }
    public required string AuthorUserName { get; set; }
    public required string Content { get; set; }
}