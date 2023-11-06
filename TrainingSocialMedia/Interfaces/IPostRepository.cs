using TrainingSocialMedia.DataTransferObjects.DataModels;

namespace TrainingSocialMedia.Interfaces;

public interface IPostRepository
{
    Task CreatePostAsync(NewPostDataModel newPostDataModel);
    Task<IReadOnlyList<PostDataModel>> GetPostsAsync();
    Task<PostDataModel?> GetPostAsync(int postId);
    Task EditPostAsync(PostDataModel postDataModel);
    Task DeletePostAsync(int postId);
}