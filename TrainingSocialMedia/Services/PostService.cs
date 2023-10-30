using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.DataTransferObjects.BusinessModels;
using TrainingSocialMedia.DataTransferObjects.DataModels;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Services;

public class PostService : IPostService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPostRepository _postRepository;

    public PostService(ApplicationDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager, 
        IHttpContextAccessor httpContextAccessor, IPostRepository postRepository)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _postRepository = postRepository;
    }

    public async Task<IReadOnlyList<PostBusinessModel>> GetPostsAsync()
    {
        var postEntities = await _dbContext.Posts.Include(p => p.Author).ToListAsync();
        return postEntities.Select(PostEntityToDto).ToList();
    }

    public async Task<PostBusinessModel?> GetPostAsync(int id)
    {
        var postEntity = await _dbContext.Posts.Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
        return postEntity is not null ? PostEntityToDto(postEntity) : null;
    }

    public async Task CreatePostAsync(NewPostBusinessModel newPostBusinessModel)
    {
        var httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("HttpContext not found");
        var authorId = _userManager.GetUserId(httpContext.User) ?? throw new Exception("Author id not found");
        var newPostDataModel = new NewPostDataModel { AuthorId = authorId, Content = newPostBusinessModel.Content };
        await _postRepository.CreatePost(newPostDataModel);
    }

    private PostBusinessModel PostEntityToDto(PostEntity postEntity)
    {
        return new PostBusinessModel
        {
            Id = postEntity.Id,
            AuthorUserName = postEntity.Author.UserName ?? throw new Exception("Author username not found"),
            Content = postEntity.Content
        };
    }
}