using TrainingSocialMedia.DataTransferObjects;

namespace TrainingSocialMedia.Interfaces;

public interface IPostService
{
    Task<IReadOnlyList<PostDto>> GetPostsAsync();
    Task<PostDto?> GetPostAsync(int id);
    Task CreatePostAsync(NewPostDto newPostDto);
}