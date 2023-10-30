using TrainingSocialMedia.ViewModels;

namespace TrainingSocialMedia.Interfaces;

public interface IPostPresenter
{
    Task CreatePostAsync(NewPostViewModel newPostViewModel);
}