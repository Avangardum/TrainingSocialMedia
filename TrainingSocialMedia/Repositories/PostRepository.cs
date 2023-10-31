using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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

    private readonly ApplicationDbContext _dbContext;

    public PostRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private IIncludableQueryable<PostEntity, UserEntity> PostsWithAuthors =>
        _dbContext.Posts.Include(p => p.Author);

    public async Task CreatePost(NewPostDataModel newPostDataModel)
    {
        var author = await _dbContext.Users.FirstAsync(u => u.Id == newPostDataModel.AuthorId);
        var postEntity = new PostEntity { Author = author, Content = newPostDataModel.Content };
        _dbContext.Posts.Add(postEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<PostDataModel>> GetPosts()
    {
        var postDataModels = await PostsWithAuthors
            .Select(PostEntityToDataModelExpression)
            .OrderByDescending(pdm => pdm.Id)
            .ToListAsync();
        return postDataModels;
    }

    public async Task<PostDataModel?> GetPost(int postId)
    {
        var postDataModel = await PostsWithAuthors
            .Select(PostEntityToDataModelExpression)
            .SingleOrDefaultAsync(pe => pe.Id == postId);
        return postDataModel;
    }
}