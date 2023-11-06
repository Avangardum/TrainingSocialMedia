namespace TrainingSocialMedia.DataTransferObjects.BusinessModels;

public class PostBusinessModel
{
    public required int Id { get; set; }
    public required UserBusinessModel? Author { get; set; }
    public required string Content { get; set; }
    public required bool IsAuthoredByCurrentUser { get; set; }
}