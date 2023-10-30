using AutoMapper;
using TrainingSocialMedia.DataTransferObjects;
using TrainingSocialMedia.DataTransferObjects.BusinessModels;
using TrainingSocialMedia.DataTransferObjects.DataModels;
using TrainingSocialMedia.DataTransferObjects.ViewModels;
using TrainingSocialMedia.Entities;

namespace TrainingSocialMedia.AutoMapperProfiles;

public class MainAutoMapperProfile : Profile
{
    public MainAutoMapperProfile()
    {
        CreateMap<PostDataModel, PostBusinessModel>();
        CreateMap<PostBusinessModel, PostViewModel>();
        CreateMap<NewPostViewModel, NewPostBusinessModel>();
    }
}