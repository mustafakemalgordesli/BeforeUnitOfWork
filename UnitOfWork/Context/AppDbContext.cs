using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UnitOfWork.Entities;

namespace UnitOfWork.Context;

public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
    : DbContext(dbContextOptions)
{ 
    public const string DEFAULT_SCHEMA = "dbo";

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}

