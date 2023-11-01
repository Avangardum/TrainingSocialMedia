namespace TrainingSocialMedia.DataTransferObjects.BusinessModels;

public class PostBusinessModel
{
    public required int Id { get; set; }
    public required string AuthorUserName { get; set; }
    public required string Content { get; set; }
    public required bool IsAuthoredByCurrentUser { get; set; }
}