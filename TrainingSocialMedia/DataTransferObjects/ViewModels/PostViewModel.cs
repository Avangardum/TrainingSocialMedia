namespace TrainingSocialMedia.DataTransferObjects.ViewModels;

public class PostViewModel
{
    public required int Id { get; set; }
    public required string AuthorUserName { get; set; }
    public required string Content { get; set; }
}