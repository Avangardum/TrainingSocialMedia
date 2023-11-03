using System.Collections.Immutable;
using System.Diagnostics;
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

    public async Task<IReadOnlyList<PostViewModel>> GetPostsAsync()
    {
        var postBusinessModels = await _postService.GetPostsAsync();
        var postViewModels = postBusinessModels.Select(PostBusinessToViewModel).ToList();
        return postViewModels;
    }

    public async Task<PostViewModel?> GetPostAsync(int id)
    {
        var postBusinessModel = await _postService.GetPostAsync(id);
        var postViewModel = postBusinessModel is not null ? PostBusinessToViewModel(postBusinessModel) : null;
        return postViewModel;
    }

    public async Task CreatePostAsync(NewPostViewModel newPostViewModel)
    {
        var newPostBusinessModel = _mapper.Map<NewPostBusinessModel>(newPostViewModel);
        await _postService.CreatePostAsync(newPostBusinessModel);
    }

    public async Task EditPostAsync(PostViewModel postViewModel)
    {
        var postBusinessModel = PostViewToBusinessModel(postViewModel);
        await _postService.EditPostAsync(postBusinessModel);
    }

    public async Task DeletePost(int postId)
    {
        await _postService.DeletePost(postId);
    }

    private PostViewModel PostBusinessToViewModel(PostBusinessModel postBusinessModel)
    {
        var viewModel = _mapper.Map<PostViewModel>(postBusinessModel);
        viewModel.PostCardBorderCssClass = GetPostCardBorderCssClass(postBusinessModel);
        return viewModel;
    }
    
    private PostBusinessModel PostViewToBusinessModel(PostViewModel postViewModel)
    {
        var businessModel = _mapper.Map<PostBusinessModel>(postViewModel);
        return businessModel;
    }

    private string GetPostCardBorderCssClass(PostBusinessModel postBusinessModel)
    {
        var firstChar = postBusinessModel.Content.First();
        var cssClassIndex = firstChar % PostCardBorderCssClasses.Count;
        var cssClass = PostCardBorderCssClasses[cssClassIndex];
        return cssClass;
    }
}