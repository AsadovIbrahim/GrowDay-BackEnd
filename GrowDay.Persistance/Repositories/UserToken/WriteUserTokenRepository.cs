using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistence.Repositories;

public class WriteUserTokenRepository : WriteGenericRepository<UserToken>, IWriteUserTokenRepository {

    // Constructor

    public WriteUserTokenRepository(GrowDayDbContext context) : base(context) { }

    // Methods
}
