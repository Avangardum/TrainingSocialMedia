using TrainingSocialMedia.DataTransferObjects.ViewModels;

namespace TrainingSocialMedia.Interfaces;

public interface IPostPresenter
{
    Task CreatePostAsync(NewPostViewModel newPostViewModel);
    Task<IReadOnlyList<PostViewModel>> GetPostsAsync();
}