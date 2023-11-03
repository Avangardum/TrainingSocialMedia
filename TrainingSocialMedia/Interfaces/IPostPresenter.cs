using TrainingSocialMedia.DataTransferObjects.ViewModels;

namespace TrainingSocialMedia.Interfaces;

public interface IPostPresenter
{
    Task<IReadOnlyList<PostViewModel>> GetPostsAsync();
    Task<PostViewModel?> GetPostAsync(int id);
    Task CreatePostAsync(NewPostViewModel newPostViewModel);
    Task EditPostAsync(PostViewModel postViewModel);
    Task DeletePost(int postId);
}