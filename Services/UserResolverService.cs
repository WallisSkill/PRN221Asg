using DependencyInjectionAutomatic.Service;
using Lombok.NET;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Services
{
    [Service]
    [RequiredArgsConstructor]
    public partial class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<UserResolverService> _logger;
        
        public int GetUser()
        {
            try
            {
                var userId = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    return int.Parse(userId);
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return 0;
            }
        }
    }
}
