using System.Collections.Immutable;
using AutoMapper;
using TrainingSocialMedia.DataTransferObjects.BusinessModels;
using TrainingSocialMedia.DataTransferObjects.ViewModels;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Presenters;

public class PostPresenter : IPostPresenter
{
    private static readonly ImmutableList<string> PostCardBorderCssClasses = ImmutableList.Create(
        "border-primary",
        "border-secondary",
        "border-success",
        "border-danger",
        "border-warning",
        "border-info",
        "border-light",
        "border-dark"
    );
    
    private readonly IPostService _postService;
    private readonly IMapper _mapper;

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
        var viewModel = _mapper.Map<PostViewModel>(postBusinessModel);
        viewModel.PostCardBorderCssClass = GetPostCardBorderCssClass(postBusinessModel);
        return viewModel;
    }

    private string GetPostCardBorderCssClass(PostBusinessModel postBusinessModel)
    {
        var firstChar = postBusinessModel.Content.First();
        var cssClassIndex = firstChar % PostCardBorderCssClasses.Count;
        var cssClass = PostCardBorderCssClasses[cssClassIndex];
        return cssClass;
    }
}