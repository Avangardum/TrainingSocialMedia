using Microsoft.AspNetCore.Authorization;
using TrainingSocialMedia.Authorization.Requirements;
using TrainingSocialMedia.Interfaces;
using RouteData = Microsoft.AspNetCore.Components.RouteData;

namespace TrainingSocialMedia.Authorization.Handlers;

public class IsPostAuthorHandler : AuthorizationHandler<IsPostAuthorRequirement>
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    
    private int _instanceCounter;
    private static int _staticCounter;

    public IsPostAuthorHandler(IUserRepository userRepository, IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPostAuthorRequirement requirement)
    {
        var instanceCount = ++_instanceCounter;
        var staticCount = ++_staticCounter;
        Console.WriteLine($"!!!HandleRequirementAsync start {instanceCount} {staticCount}"); // TODO: Remove
        
        var postId = context.Resource switch
        {
            int id => id,
            RouteData routeData => int.Parse(routeData.RouteValues["Id"].ToString() ?? "null"),
            HttpContext httpContext => int.Parse(httpContext.Request.RouteValues["Id"]?.ToString() ?? "null"),
            _ => throw new ArgumentOutOfRangeException(nameof(context.Resource), 
                $"Unknown resource type ({context.Resource?.GetType()})")
        };

        var post = await _postRepository.GetPost(postId);
        var currentUser = await _userRepository.GetCurrentUserAsync();
        if (post?.Author.Id == currentUser?.Id) context.Succeed(requirement);
        
        Console.WriteLine($"!!!HandleRequirementAsync end {instanceCount} {staticCount}"); // TODO: Remove
    }
}