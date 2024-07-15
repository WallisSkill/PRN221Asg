using PRN221_Assignment.Data;
using PRN221_Assignment.Models;

namespace PRN221_Assignment.Repository.Interface
{
    public interface IPhotoRepository
    {
        void AddPhoto(Photo photo);

        void DeletePhoto(int postId);

        List<Photo> GetPhotoById(int postId);
    }
}
