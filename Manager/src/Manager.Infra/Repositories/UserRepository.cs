
using Microsoft.EntityFrameworkCore;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Infra.Context;

namespace Manager.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository {
        private readonly ManagerContext _context;

        public UserRepository(ManagerContext context) : base(context) {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email) {

            // em lowercase (letras minúsculas)
            var user = await _context.Users
                .Where(x=>x.Email.ToLower() == email.ToLower())
                    .AsNoTracking().ToListAsync();

            return user.FirstOrDefault();
        }

        public async Task<List<User>> SearchByEmailAsync(string email) {

            var allUsers = await _context.Users
                .Where(x=>x.Email.ToLower().Contains(email.ToLower()))
                    .AsNoTracking().ToListAsync();

            return allUsers;
        }

        public async Task<List<User>> SearchByNameAsync(string nome) {

            var allUsers = await _context.Users
                .Where(x=>x.Name.ToLower().Contains(nome.ToLower()))
                    .AsNoTracking().ToListAsync();

            return allUsers;
        }
    }
}