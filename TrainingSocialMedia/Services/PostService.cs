using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using TrainingSocialMedia.Constants;
using TrainingSocialMedia.DataTransferObjects.BusinessModels;
using TrainingSocialMedia.DataTransferObjects.DataModels;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private UserDataModel? _currentUser;

    public PostService(IPostRepository postRepository, IUserRepository userRepository, IMapper mapper, 
        IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal CurrentUserClaimsPrincipal => _httpContextAccessor.HttpContext?.User 
        ?? throw new InvalidOperationException("HttpContext is null.");

    public async Task<IReadOnlyList<PostBusinessModel>> GetPostsAsync()
    {
        var postDataModels = await _postRepository.GetPostsAsync();
        var postBusinessModels = new List<PostBusinessModel>();
        foreach (var postDataModel in postDataModels)
        {
            var postBusinessModel = await PostDataToBusinessModelAsync(postDataModel);
            postBusinessModels.Add(postBusinessModel);
        }
        return postBusinessModels;
    }

    public async Task<PostBusinessModel?> GetPostAsync(int id)
    {
        var postDataModel = await _postRepository.GetPostAsync(id);
        var postBusinessModel = postDataModel is not null ? await PostDataToBusinessModelAsync(postDataModel) : null;
        return postBusinessModel;
    }

    public async Task CreatePostAsync(NewPostBusinessModel newPostBusinessModel)
    {
        var currentUser = await _userRepository.GetCurrentUserAsync();
        Debug.Assert(currentUser is not null);
        var newPostDataModel = new NewPostDataModel { AuthorId = currentUser.Id, Content = newPostBusinessModel.Content };
        await _postRepository.CreatePostAsync(newPostDataModel);
    }

    public async Task EditPostAsync(PostBusinessModel postBusinessModel)
    {
        await VerifyCurrentUserIsPostAuthor(postBusinessModel.Id);
        var postDataModel = PostBusinessToDataModel(postBusinessModel);
        await _postRepository.EditPostAsync(postDataModel);
    }

    public async Task DeletePost(int postId)
    {
        await VerifyCurrentUserIsPostAuthor(postId);
        await _postRepository.DeletePostAsync(postId);
    }

    private async Task<PostBusinessModel> PostDataToBusinessModelAsync(PostDataModel postDataModel)
    {
        await LoadCurrentUserIfNotLoaded();
        var businessModel = _mapper.Map<PostBusinessModel>(postDataModel);
        businessModel.IsAuthoredByCurrentUser = _currentUser is not null && _currentUser.Id == postDataModel.Author?.Id;
        return businessModel;
    }
    
    private PostDataModel PostBusinessToDataModel(PostBusinessModel postBusinessModel)
    {
        var postDataModel = _mapper.Map<PostDataModel>(postBusinessModel);
        return postDataModel;
    }

    private async Task LoadCurrentUserIfNotLoaded()
    {
        if (_currentUser is not null) return;
        _currentUser = await _userRepository.GetCurrentUserAsync();
    }

    private async Task VerifyCurrentUserIsPostAuthor(int postId)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(CurrentUserClaimsPrincipal, postId, 
            Policies.IsPostAuthor);
        if (!authorizationResult.Succeeded) 
            throw new InvalidOperationException("Current user is not the author of the post.");
    }
}