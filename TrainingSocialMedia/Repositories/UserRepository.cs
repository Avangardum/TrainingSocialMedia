using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.DataTransferObjects.DataModels;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly UserManager<UserEntity> _userManager;

    public UserRepository(IHttpContextAccessor httpContextAccessor, IMapper mapper, 
        IDbContextFactory<ApplicationDbContext> dbContextFactory, UserManager<UserEntity> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _dbContextFactory = dbContextFactory;
        _userManager = userManager;
    }

    public async Task<UserDataModel?> GetCurrentUserAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("HttpContext not found");
        var userId = _userManager.GetUserId(httpContext.User);
        if (userId is null) return null;
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var userDataModel = await dbContext.Users
            .Where(ue => ue.Id == userId)
            .Select(ue => new UserDataModel { Id = ue.Id, UserName = ue.UserName! })
            .SingleOrDefaultAsync();
        return userDataModel;
    }
}