using Azure.Core;
using JwtApi.Entities;
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

            //var city = new City() { 
            //cityID = userID,
            //cityName   ="my city name",
            //};

            return new JsonResult( new { city, map="testing" });  //return Ok(retObj);

           // return new JsonResult(new { WarReports, map = Map, userId = UserId, city = UserCity, userItems = UserItems, userResearch = userResearch });
        }
    }
}
