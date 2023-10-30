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

    public async Task<IReadOnlyList<PostDto>> GetPosts()
    {
        var postEntities = await _dbContext.Posts.Include(p => p.Author).ToListAsync();
        return postEntities.Select(postEntity => _mapper.Map<PostDto>(postEntity)).ToList();
    }

    public async Task<PostDto?> GetPost(int id)
    {
        var postEntity = await _dbContext.Posts.Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
        return postEntity is not null ? _mapper.Map<PostDto>(postEntity) : null;
    }

    public async Task CreatePost(NewPostDto newPostDto)
    {
        var httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("HttpContext not found");
        var author = await _userManager.GetUserAsync(httpContext.User) 
            ?? throw new Exception("User not found");
        var postEntity = new PostEntity { Author = author, Content = newPostDto.Content };
        await _dbContext.Posts.AddAsync(postEntity);
        await _dbContext.SaveChangesAsync();
    }
}