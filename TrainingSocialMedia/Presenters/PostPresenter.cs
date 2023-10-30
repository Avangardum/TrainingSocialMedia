using AutoMapper;
using TrainingSocialMedia.DataTransferObjects;
using TrainingSocialMedia.DataTransferObjects.BusinessModels;
using TrainingSocialMedia.DataTransferObjects.ViewModels;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Presenters;

public class PostPresenter : IPostPresenter
{
    private IPostService _postService;
    private IMapper _mapper;

    public PostPresenter(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    public async Task CreatePostAsync(NewPostViewModel newPostViewModel)
    {
        var newPostBusinessModel = _mapper.Map<NewPostBusinessModel>(newPostViewModel);
        await _postService.CreatePostAsync(newPostBusinessModel);
    }

    public async Task<IReadOnlyList<PostViewModel>> GetPostsAsync()
    {
        var postBusinessModels = await _postService.GetPostsAsync();
        var postViewModels = postBusinessModels.Select(PostBusinessToViewModel).ToList();
        return postViewModels;
    }

    private PostViewModel PostBusinessToViewModel(PostBusinessModel postBusinessModel)
    {
        return _mapper.Map<PostViewModel>(postBusinessModel);
    }
}