namespace TrainingSocialMedia.DataTransferObjects.DataModels;

public class PostDataModel
{
    public required int Id { get; set; }
    public required string AuthorUserName { get; set; }
    public required string Content { get; set; }
}