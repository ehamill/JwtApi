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
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Hero> Heros { get; set; }
        public DbSet<UserItems> UserItems { get; set; }
        public DbSet<UserResearch> UserResearch { get; set; }
        public DbSet<TroopQueue> TroopQueues { get; set; }
        public DbSet<Troop> Troops { get; set; }
        public DbSet<Battle> Battles { get; set; }
       // public DbSet<CityDefender> CityDefenders { get; set; }
        public DbSet<Email> Emails { get; set; }
        //public DbSet<FutureCityData> FutureCityData { get; set; }
        //public DbSet<CityData> CityDatas { get; set; }
        public DbSet<WarReport> WarReports { get; set; }
    }
}
