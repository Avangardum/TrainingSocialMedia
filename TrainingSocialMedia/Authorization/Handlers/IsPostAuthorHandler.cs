using Microsoft.AspNetCore.Authorization;
using TrainingSocialMedia.Authorization.Requirements;
using TrainingSocialMedia.Interfaces;
using RouteData = Microsoft.AspNetCore.Components.RouteData;

namespace TrainingSocialMedia.Authorization.Handlers;

public class IsPostAuthorHandler : AuthorizationHandler<IsPostAuthorRequirement>
{
    protected readonly IPostRepository _postRepository;
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
            RouteData routeData => (int)routeData.RouteValues["id"],
            _ => throw new ArgumentOutOfRangeException(nameof(context), "Unknown resource type")
        };

        var post = await _postRepository.GetPost(postId);
        var currentUser = await _userRepository.GetCurrentUserAsync();
        if (post?.Author.Id == currentUser?.Id) context.Succeed(requirement);
    }
}