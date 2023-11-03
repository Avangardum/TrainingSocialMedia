using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.DataTransferObjects.DataModels;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Repositories;

public class PostRepository : IPostRepository
{
    private static readonly Expression<Func<PostEntity, PostDataModel>> PostEntityToDataModelExpression = 
        pe => new PostDataModel
        {
            Id = pe.Id, 
            Content = pe.Content,
            Author = new() { Id = pe.Author.Id, UserName = pe.Author.UserName! }
        };
    
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public PostRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task CreatePostAsync(NewPostDataModel newPostDataModel)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var author = await dbContext.Users.FirstAsync(u => u.Id == newPostDataModel.AuthorId);
        var postEntity = new PostEntity { Author = author, Content = newPostDataModel.Content };
        dbContext.Posts.Add(postEntity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<PostDataModel>> GetPostsAsync()
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var postDataModels = await GetPostsWithAuthors(dbContext)
            .Select(PostEntityToDataModelExpression)
            .OrderByDescending(pdm => pdm.Id)
            .ToListAsync();
        return postDataModels;
    }

    public async Task<PostDataModel?> GetPostAsync(int postId)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var postDataModel = await GetPostsWithAuthors(dbContext)
            .Select(PostEntityToDataModelExpression)
            .SingleOrDefaultAsync(pe => pe.Id == postId);
        return postDataModel;
    }

    public async Task EditPostAsync(PostDataModel postDataModel)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var postEntity = await dbContext.Posts.SingleOrDefaultAsync(pe => pe.Id == postDataModel.Id);
        Debug.Assert(postEntity is not null);
        postEntity.Content = postDataModel.Content;
        await dbContext.SaveChangesAsync();
    }

    public async Task DeletePostAsync(int postId)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var postEntity = await dbContext.Posts.SingleOrDefaultAsync(pe => pe.Id == postId);
        if (postEntity is not null)
        {
            dbContext.Posts.Remove(postEntity);
            await dbContext.SaveChangesAsync();
        }
    }

    private IQueryable<PostEntity> GetPostsWithAuthors(ApplicationDbContext dbContext) =>
        dbContext.Posts.Include(p => p.Author);
}