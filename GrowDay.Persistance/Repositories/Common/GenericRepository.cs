using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Abstracts;
using GrowDay.Persistance.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class,IBaseEntity, new()
    {
        protected DbSet<T> _table;
        protected readonly GrowDayDbContext _context;


        public GenericRepository(GrowDayDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

    }
}
