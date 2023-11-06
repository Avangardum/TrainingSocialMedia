using TrainingSocialMedia.DataTransferObjects.BusinessModels;

namespace TrainingSocialMedia.Interfaces;

public interface IPostService
{
    Task<IReadOnlyList<PostBusinessModel>> GetPostsAsync();
    Task<PostBusinessModel?> GetPostAsync(int id);
    Task CreatePostAsync(NewPostBusinessModel newPostBusinessModel);
    Task EditPostAsync(PostBusinessModel postBusinessModel);
    Task DeletePost(int postId);
}