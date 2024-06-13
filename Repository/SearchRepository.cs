using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;
using System.Diagnostics;

namespace PRN221_Assignment.Repository;

[RequiredArgsConstructor]
[Service]
public partial class SearchRepository : ISearchRepository
{
    private readonly social_mediaContext _context;

    public List<Post> SearchPost(string searchTerm)
    {
        return _context.Set<Post>().Where(p => p.Caption.Contains(searchTerm) && p.Caption!=null).ToList();
    }

    public List<User> SearchUser(string searchTerm)
    {
        return _context.Set<User>().Where(p => p.Fullname.Contains(searchTerm) && p.Fullname != null).ToList();
    }
}