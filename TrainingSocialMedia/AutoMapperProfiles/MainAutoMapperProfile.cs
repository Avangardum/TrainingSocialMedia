using AutoMapper;
using TrainingSocialMedia.DataTransferObjects;
using TrainingSocialMedia.DataTransferObjects.BusinessModels;
using TrainingSocialMedia.DataTransferObjects.ViewModels;
using TrainingSocialMedia.Entities;

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