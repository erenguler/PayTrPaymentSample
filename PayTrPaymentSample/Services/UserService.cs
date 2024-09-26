using Microsoft.EntityFrameworkCore;
using PayTrPaymentSample.Data.Entities;

namespace PayTrPaymentSample.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetCurrentUser()
        {
            var userId = 1; // current user id
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        }
    }
}
