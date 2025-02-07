using JwtApi.Entities;
using JwtApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtApi.Data
{
    //change to AppDbContext later   install Microsfof.entityframework.core  install sql server
    public class UserDbContext(DbContextOptions <UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hero> Heros { get; set; }
    }
}
