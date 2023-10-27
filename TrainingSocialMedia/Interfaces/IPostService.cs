using TrainingSocialMedia.DataTransferObjects;

namespace TrainingSocialMedia.Interfaces;

public interface IPostService
{
    Task<IReadOnlyList<PostDto>> GetPosts();
    Task<PostDto?> GetPost(int id);
}