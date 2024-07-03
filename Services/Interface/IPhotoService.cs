using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Services.Interface
{
    public interface IPhotoService
    {
        void AddPhoto(Photo photo);
        void DeletePhoto(int postId);
    }
}
