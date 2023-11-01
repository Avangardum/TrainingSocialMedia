using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TrainingSocialMedia.DataTransferObjects.DataModels;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<UserEntity> _userManager;
    private readonly IMapper _mapper;

    public UserRepository(IHttpContextAccessor httpContextAccessor, UserManager<UserEntity> userManager, IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDataModel?> GetCurrentUserAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext ?? throw new Exception("HttpContext not found");
        var userEntity = await _userManager.GetUserAsync(httpContext.User);
        if (userEntity is null) return null;
        var userDataModel = _mapper.Map<UserDataModel>(userEntity);
        return userDataModel;
    }
}