using AutoMapper;
using TrainingSocialMedia.DataTransferObjects;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.ViewModels;

namespace TrainingSocialMedia.AutoMapperProfiles;

public class MainAutoMapperProfile : Profile
{
    public MainAutoMapperProfile()
    {
        CreateMap<PostEntity, PostBusinessModel>();
        CreateMap<PostBusinessModel, PostEntity>();
        CreateMap<NewPostViewModel, NewPostBusinessModel>();
    }
}