using Microsoft.AspNetCore.Authorization;
using TrainingSocialMedia.Authorization.Requirements;
using TrainingSocialMedia.Interfaces;
using RouteData = Microsoft.AspNetCore.Components.RouteData;

namespace TrainingSocialMedia.Authorization.Handlers;

public class IsPostAuthorHandler : AuthorizationHandler<IsPostAuthorRequirement>
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    
    public IsPostAuthorHandler(IUserRepository userRepository, IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPostAuthorRequirement requirement)
    {
        var postId = context.Resource switch
        {
            int id => id,
            RouteData routeData => int.Parse(routeData.RouteValues["Id"].ToString() ?? "null"),
            _ => throw new ArgumentOutOfRangeException(nameof(context.Resource), 
                $"Unknown resource type ({context.Resource?.GetType()})")
        };

        var post = await _postRepository.GetPostAsync(postId);
        var currentUser = await _userRepository.GetCurrentUserAsync();
        if (post is not null && currentUser is not null && post.Author?.Id == currentUser.Id) 
            context.Succeed(requirement);
    }
}