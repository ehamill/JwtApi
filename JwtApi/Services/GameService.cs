using JwtApi.Data;
using JwtApi.Entities;
using JwtApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtApi.Services
{
    public class GameService(UserDbContext db) : IGameService
    {

        public async Task<City> GetCity(string UserID) {

            var city = await db.Cities.Include(c => c.Buildings).FirstOrDefaultAsync(c => c.UserId == UserID);
            //UserCity = await db.Cities
            //    .Include(c => c.Buildings)
            //    .Include(c => c.Heros).Include(c => c.TroopQueues)
            //    .Where(c => c.UserId == UserId).FirstOrDefaultAsync()

            return city;





        }


    }
}
