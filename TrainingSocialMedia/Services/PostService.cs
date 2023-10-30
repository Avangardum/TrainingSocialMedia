using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.DataTransferObjects;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Services;

public class PostService : IPostService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostService(ApplicationDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager, 
        IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
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
        var author = await _userManager.GetUserAsync(httpContext.User) 
            ?? throw new Exception("User not found");
        var postEntity = new PostEntity { Author = author, Content = newPostBusinessModel.Content };
        await _dbContext.Posts.AddAsync(postEntity);
        await _dbContext.SaveChangesAsync();
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