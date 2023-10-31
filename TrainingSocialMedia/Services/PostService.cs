using System.Diagnostics;
using AutoMapper;
using TrainingSocialMedia.DataTransferObjects.BusinessModels;
using TrainingSocialMedia.DataTransferObjects.DataModels;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    private UserDataModel? _currentUser;

    public PostService(IPostRepository postRepository, IUserRepository userRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PostBusinessModel>> GetPostsAsync()
    {
        var postDataModels = await _postRepository.GetPosts();
        var postBusinessModels = new List<PostBusinessModel>();
        foreach (var postDataModel in postDataModels)
        {
            var postBusinessModel = await PostDataToBusinessModelAsync(postDataModel);
            postBusinessModels.Add(postBusinessModel);
        }
        return postBusinessModels;
    }
    
    public async Task CreatePostAsync(NewPostBusinessModel newPostBusinessModel)
    {
        var currentUser = await _userRepository.GetCurrentUserAsync();
        Debug.Assert(currentUser is not null);
        var newPostDataModel = new NewPostDataModel { AuthorId = currentUser.Id, Content = newPostBusinessModel.Content };
        await _postRepository.CreatePost(newPostDataModel);
    }

    private async Task<PostBusinessModel> PostDataToBusinessModelAsync(PostDataModel postDataModel)
    {
        await LoadCurrentUserIfNotLoaded();
        var businessModel = _mapper.Map<PostBusinessModel>(postDataModel);
        businessModel.IsAuthoredByCurrentUser = _currentUser is not null && _currentUser.Id == postDataModel.Author.Id;
        return businessModel;
    }

    private async Task LoadCurrentUserIfNotLoaded()
    {
        if (_currentUser is not null) return;
        _currentUser = await _userRepository.GetCurrentUserAsync();
    }
}