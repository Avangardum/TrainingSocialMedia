using TrainingSocialMedia.DataTransferObjects.DataModels;

namespace TrainingSocialMedia.Interfaces;

public interface IUserRepository
{
    Task<UserDataModel?> GetCurrentUserAsync();
}