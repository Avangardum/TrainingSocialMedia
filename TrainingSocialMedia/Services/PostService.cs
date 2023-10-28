using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.DataTransferObjects;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Services;

public class PostService : IPostService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PostService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PostDto>> GetPosts()
    {
        var postEntities = await _context.Posts.Include(p => p.Author).ToListAsync();
        return postEntities.Select(postEntity => _mapper.Map<PostDto>(postEntity)).ToList();
    }

    public async Task<PostDto?> GetPost(int id)
    {
        var postEntity = await _context.Posts.Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
        return postEntity is not null ? _mapper.Map<PostDto>(postEntity) : null;
    }
}