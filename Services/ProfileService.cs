using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;

[Service]
[RequiredArgsConstructor]
public partial class ProfileService: IProfileService
{
    private readonly IProfileRepository _profileRepository;
    
    public User? GetUserInfo(int userid)
    {
        return _profileRepository.GetUserInfo(userid);
    }

    public List<Photo> GetUserPhoto(int userid)
    {
        return _profileRepository.GetUserPhoto(userid);
    }
}