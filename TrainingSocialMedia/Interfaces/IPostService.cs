using TrainingSocialMedia.DataTransferObjects.BusinessModels;

namespace TrainingSocialMedia.Interfaces;

public interface IPostService
{
    Task<IReadOnlyList<PostBusinessModel>> GetPostsAsync();
    Task CreatePostAsync(NewPostBusinessModel newPostBusinessModel);
}