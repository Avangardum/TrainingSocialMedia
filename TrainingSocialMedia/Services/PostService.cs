using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TrainingSocialMedia.DataTransferObjects.BusinessModels;
using TrainingSocialMedia.DataTransferObjects.DataModels;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Services;

public class PostService : IPostService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;

    public PostService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, 
        IPostRepository postRepository, IMapper mapper)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PostBusinessModel>> GetPostsAsync()
    {
        var postDataModels = await _postRepository.GetPosts();
        var postBusinessModels = postDataModels.Select(PostDataToBusinessModel).ToList();
        return postBusinessModels;
    }
    
    public async Task CreatePostAsync(NewPostBusinessModel newPostBusinessModel)
    {
        var httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("HttpContext not found");
        var authorId = _userManager.GetUserId(httpContext.User) ?? throw new Exception("Author id not found");
        var newPostDataModel = new NewPostDataModel { AuthorId = authorId, Content = newPostBusinessModel.Content };
        await _postRepository.CreatePost(newPostDataModel);
    }

    private PostBusinessModel PostDataToBusinessModel(PostDataModel postDataModel)
    {
        return _mapper.Map<PostBusinessModel>(postDataModel);
    }
}