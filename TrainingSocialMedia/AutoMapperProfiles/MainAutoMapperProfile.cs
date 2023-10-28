using AutoMapper;
using TrainingSocialMedia.DataTransferObjects;
using TrainingSocialMedia.Entities;

namespace TrainingSocialMedia.AutoMapperProfiles;

public class MainAutoMapperProfile : Profile
{
    public MainAutoMapperProfile()
    {
        CreateMap<PostEntity, PostDto>();
    }
}