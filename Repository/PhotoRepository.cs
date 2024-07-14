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

    public void DeletePhoto(int postId)
    {

        var photos = _context.Photos.Where(p => p.PostId == postId).ToList();
        foreach (var photo in photos)
        {
            _context.Photos.Remove(photo);
        }
        _context.SaveChanges();


    }

    public List<Photo> GetPhotoById(int postId)
    {
        return _context.Photos.Where(x => x.PostId == postId).ToList();
    }
}

