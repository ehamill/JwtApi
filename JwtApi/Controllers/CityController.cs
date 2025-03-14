using Azure.Core;
using JwtApi.Entities;
using JwtApi.Models;
using JwtApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static JwtApi.Controllers.AuthController;

namespace JwtApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CityController(IGameService gameService) : ControllerBase
    {
        //public class City {
        //    public int cityID { get; set; }
        //    public string cityName { get; set; }
        //}

        
        [HttpGet]  //City/
        public async Task<JsonResult> Get()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            //int userID = int.Parse(UserId);
            UserId = "c51e59c1-08e0-4c38-b2c7-e81454f8b04f";

            var city = await gameService.GetCity(UserId);

            //string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var test = await CreateCity(UserId);
            City UserCity = new City();
            UserItems UserItems = new UserItems();
            UserResearch userResearch = new UserResearch();
            var WarReports = new List<WarReport>();
            var Map = new List<City>();

            try
            {
                //Map = await db.Cities.ToListAsync();
                //UserCity = await db.Cities
                //    .Include(c => c.Buildings)
                //    .Include(c => c.Heros).Include(c => c.TroopQueues)
                //    .Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? await CreateCity(UserId, Map);
                //WarReports = await GetWarReports();
                ////battles = await db.Battles.Where(c => c.AtkCityId == UserCity.CityId || c.DefCityId == UserCity.CityId).ToListAsync();
                ////UserCity.FutureCityData = await db.FutureCityData.Where(c => c.CityId == UserCity.CityId).OrderByDescending(c => c.DateTime).ToListAsync();
                //UserItems = await db.UserItems.Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? await CreateUserItems(UserId);
                //userResearch = await db.UserResearch.Where(c => c.UserId == UserId).FirstOrDefaultAsync() ?? await CreateUserResearch(UserId);
                //await ReplenishHeros(UserCity);//Replensh Free heros

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error at CityController, [GET]: " + ex.Message);
                Console.WriteLine(ex.Message);
            }

            //await UpdateResources(UserCity);
            //if (UserCity.Builder1Busy)
            //{
            //    await CheckBuilder1(UserCity);
            //}
            //if (UserCity.ResearchBusy)
            //{
            //    await CheckResearcher(UserCity, userResearch);
            //}
            //UserCity.ListOfBuildingsCost = GetNewBuildingsCost(UserCity, userResearch);
            //UserCity.ResearchCost = GetResearchCosts(UserCity, userResearch);
            //await CheckTroopQueues(UserCity);
            //UserCity.TroopProperties = GetDefaultTroops(UserCity, userResearch);

            //If done, add troops to city... delete queue?? Status..training-complete-cancelled
            //if not...

            //GetUpgradeBuildings..only need one for each, can calculate costs off of it
            //Sleep doesn't work..

           // return new JsonResult(new { WarReports, map = Map, userId = UserId, city = UserCity, userItems = UserItems, userResearch = userResearch });

            return new JsonResult( new { city, message="testing" });  //return Ok(retObj);

           // return new JsonResult(new { WarReports, map = Map, userId = UserId, city = UserCity, userItems = UserItems, userResearch = userResearch });
        }
    }
}
