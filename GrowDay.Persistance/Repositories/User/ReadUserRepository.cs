using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadUserRepository : ReadGenericRepository<User>, IReadUserRepository
    {
        public ReadUserRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _table.FirstOrDefaultAsync(x=>x.Email == email && x.IsDeleted == false);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _table.FirstOrDefaultAsync(x => x.UserName == username && x.IsDeleted == false);
        }
    }
}
