using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.DataTransferObjects.DataModels;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PostRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreatePost(NewPostDataModel newPostDataModel)
    {
        var author = await _dbContext.Users.FirstAsync(u => u.Id == newPostDataModel.AuthorId);
        var postEntity = new PostEntity { Author = author, Content = newPostDataModel.Content };
        _dbContext.Posts.Add(postEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<PostDataModel>> GetPosts()
    {
        var postDataModels = await _dbContext.Posts
            .Include(p => p.Author)
            .Select(pe =>
                new PostDataModel
                {
                    Id = pe.Id, Content = pe.Content,
                    Author = new() { Id = pe.Author.Id, UserName = pe.Author.UserName! }
                })
            .OrderByDescending(pdm => pdm.Id)
            .ToListAsync();
        return postDataModels;
    }
}