using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.DataTransferObjects;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Services;

public class PostService : IPostService
{
    private readonly ApplicationDbContext _context;

    public PostService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PostDto>> GetPosts()
    {
        var postEntities = await _context.Posts.Include(p => p.Author).ToListAsync();
        return postEntities.Select(MapPostEntityToPostDto).ToList();
    }

    public async Task<PostDto?> GetPost(int id)
    {
        var postEntity = await _context.Posts.Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
        return postEntity is not null ? MapPostEntityToPostDto(postEntity) : null;
    }
    
    private PostDto MapPostEntityToPostDto(PostEntity postEntity)
    {
        return new PostDto
        {
            Id = postEntity.Id,
            Author = postEntity.Author,
            Content = postEntity.Content
        };
    }
}