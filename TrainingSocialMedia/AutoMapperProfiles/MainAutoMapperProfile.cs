using AutoMapper;
using TrainingSocialMedia.DataTransferObjects;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.ViewModels;

namespace TrainingSocialMedia.AutoMapperProfiles;

public class MainAutoMapperProfile : Profile
{
    public MainAutoMapperProfile()
    {
        CreateMap<PostEntity, PostDto>();
        CreateMap<PostDto, PostEntity>();
        CreateMap<NewPostViewModel, NewPostDto>();
    }
}