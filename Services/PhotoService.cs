using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services;

[RequiredArgsConstructor]
[Service]
public partial class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _photoRepository;
        public void AddPhoto(Photo photo)
        {
            _photoRepository.AddPhoto(photo);
        }

    public void DeletePhoto(int postId)
    {
        _photoRepository.DeletePhoto(postId);
    }

    public List<Photo> GetPhotosById(int postId)
    {
        return _photoRepository.GetPhotoById(postId);
    }
}

