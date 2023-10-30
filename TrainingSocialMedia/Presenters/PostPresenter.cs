using AutoMapper;
using TrainingSocialMedia.DataTransferObjects;
using TrainingSocialMedia.DataTransferObjects.BusinessModels;
using TrainingSocialMedia.DataTransferObjects.ViewModels;
using TrainingSocialMedia.Interfaces;

namespace TrainingSocialMedia.Presenters;

public class PostPresenter : IPostPresenter
{
    private IPostService _postService;
    private IMapper _mapper;

    public PostPresenter(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    public async Task CreatePostAsync(NewPostViewModel newPostViewModel)
    {
        var newPostDto = _mapper.Map<NewPostBusinessModel>(newPostViewModel);
        await _postService.CreatePostAsync(newPostDto);
    }
}