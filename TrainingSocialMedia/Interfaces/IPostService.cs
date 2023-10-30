using TrainingSocialMedia.DataTransferObjects;

namespace TrainingSocialMedia.Interfaces;

public interface IPostService
{
    Task<IReadOnlyList<PostBusinessModel>> GetPostsAsync();
    Task<PostBusinessModel?> GetPostAsync(int id);
    Task CreatePostAsync(NewPostBusinessModel newPostBusinessModel);
}