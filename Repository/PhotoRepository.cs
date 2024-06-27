using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;


namespace PRN221_Assignment.Repository;

    [RequiredArgsConstructor]
    [Service]
    public partial class PhotoRepository : IPhotoRepository
    {
        private readonly social_mediaContext _context;
        public void AddPhoto(Photo photo)
        {
            _context.Photos.Add(photo);
            _context.SaveChanges();
        }
}

