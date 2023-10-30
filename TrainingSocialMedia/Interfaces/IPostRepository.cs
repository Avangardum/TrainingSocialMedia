using TrainingSocialMedia.DataTransferObjects.DataModels;

namespace TrainingSocialMedia.Interfaces;

public interface IPostRepository
{
    Task CreatePost(NewPostDataModel newPostDataModel);
    Task<IReadOnlyList<PostDataModel>> GetPosts();
}