using Microsoft.EntityFrameworkCore;
using UnitOfWork.Abstracts;
using UnitOfWork.Context;
using UnitOfWork.Entities;

namespace UnitOfWork.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}
