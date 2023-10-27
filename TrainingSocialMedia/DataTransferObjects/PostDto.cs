using TrainingSocialMedia.Entities;

namespace TrainingSocialMedia.DataTransferObjects;

public class PostDto
{
    public int? Id { get; set; }
    public ApplicationUser? Author { get; set; }
    public string? Content { get; set; }
}